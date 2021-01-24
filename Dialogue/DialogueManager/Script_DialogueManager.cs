using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// NOTE: DialogueManager is Setup on EVERY level
/// 
/// Entry point for dialogue canvas related logic
/// CurrentNode switches on NextDialogueNode(); is left in state on EndDialogue()
/// Unity Event handlers on Nodes fire at end of Node
/// End Dialogue Event also available
/// </summary>
public class Script_DialogueManager : MonoBehaviour
{
    public static Script_DialogueManager DialogueManager;
    public static float DialogueContinuationFlickerInterval = 0.75f;
    public Script_Interactable activeInteractable; // for cut scenes this will be null
    public CanvasGroup inputManagerCanvas;
    public Script_Game game;
    public CanvasGroup canvas;
    public AudioSource audioSource;
    public AudioClip dialogueStartSoundFX;
    public AudioClip typeSFX;

    public Transform activeCanvas;
    public TextMeshProUGUI activeCanvasText;
    public TextMeshProUGUI[] activeCanvasTexts;
    /*
        NAME
        LINE
        LINE
    */
    public Canvas DefaultCanvas;
    public TextMeshProUGUI DefaultCanvasName;
    public TextMeshProUGUI[] DefaultCanvasDialogueTexts;

    public Canvas DefaultCanvasTop;
    public TextMeshProUGUI DefaultCanvasNameTop;
    public TextMeshProUGUI[] DefaultCanvasDialogueTextsTop;

    public Canvas CanvasChoice1Row;
    public TextMeshProUGUI CanvasChoice1RowName;
    public TextMeshProUGUI[] CanvasChoice1RowDialogueTexts;

    public Canvas CanvasChoice1RowTop;
    public TextMeshProUGUI CanvasChoice1RowTopName;
    public TextMeshProUGUI[] CanvasChoice1RowTopDialogueTexts;

    public Canvas SaveChoiceCanvas;
    public TextMeshProUGUI SaveChoiceName;
    public TextMeshProUGUI[] SaveChoiceDialogueTexts;

    /// based on SaveChoiceCanvas
    public Canvas PaintingEntranceChoiceCanvas;
    public TextMeshProUGUI[] PaintingEntranceChoiceDialogueTexts;

    public Canvas ReadChoiceCanvasBottom; 
    public TextMeshProUGUI[] ReadChoiceDialogueTexts;

    /*
        LINE
        LINE
    */
    public Canvas DefaultReadTextCanvas;
    public TextMeshProUGUI[] DefaultReadTextCanvasTexts;
    public Canvas DefaultReadTextCanvasTop;
    public TextMeshProUGUI[] DefaultReadTextCanvasTextsTop;
    public Canvas ItemDescriptionCanvasBottom;
    public TextMeshProUGUI[] ItemDescriptionCanvasTextsBottom;
    public Canvas ItemDescriptionCanvasTop;
    public TextMeshProUGUI[] ItemDescriptionCanvasTextsTop;
    public Script_SaveManager saveManager;
    public Script_PaintingEntranceManager paintingEntranceManager;
    [SerializeField] private Script_InputManager inputManager;
    [SerializeField] private Script_FullArtManager fullArtManager;
    [SerializeField] private Script_DialogueManagerCanvasHandler canvasHandler;

    

    public bool isRenderingDialogueSection = false;
    public int lineCount = 0;
    public Queue<Model_DialogueSection> dialogueSections;
    public Queue<string> lines;
    public float pauseLength;
    public float charPauseLength;
    public float charPauseFast;
    public float charPauseDefault;
    public float typingVolumeScale;
    public float dialogueStartVolumeScale;
    public Script_DialogueNode currentNode;
    public bool isInputMode = false;
    public bool noContinuationIcon;
    public bool isKeepingDialogueUp;

    
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI dialogueText;
    private Script_ChoiceManager choiceManager;
    private string playerName;
    private IEnumerator coroutine;
    private Model_DialogueSection dialogueSection;
    private string formattedLine;
    /// <summary>
    /// to notify special characters we must play SFX for (e.g. always play SFX after "|" pause)
    /// </summary>
    private bool shouldPlayTypeSFX = true;
    private bool isSilentTyping = false;
    /// Flag to disable inputs while full art is fading in or out
    public bool isInputDisabled;

    void Update()
    {
        // called after finishes rendering dialogue sections and lines 
        if (
            currentNode != null
            && !string.IsNullOrEmpty(currentNode.data.updateAction)
            && dialogueSections.Count == 0
            && lines.Count == 0
            && !isRenderingDialogueSection
        )
        {
            game.HandleDialogueNodeUpdateAction(currentNode.data.updateAction);
        }
    }
    
    /// <summary>
    /// Currently everything runs through ContinueDialogue to check for Sections.Count
    /// </summary>
    public void StartDialogueNode(
        Script_DialogueNode node,
        bool SFXOn = true,
        string type = null,
        Script_Interactable talkingInteractive = null
    )
    {
        Debug.Log($"Starting dialogue node. isInputDisabled: {isInputDisabled}");
        
        if (isInputDisabled)    return;
        currentNode = node;
        activeInteractable = talkingInteractive;

        /// Dev warning
        if (activeInteractable == null && game.state == Const_States_Game.Interact)
            Debug.LogWarning("Ensure to reference the caller for StartDialogueNode()");

        SetupTypingSpeed();

        if (currentNode.data.OnBeforeNodeAction.CheckUnityEventAction())
        {
            currentNode.data.OnBeforeNodeAction.Invoke();
        }

        if (currentNode.data.fullArt != null)
        {
            isInputDisabled = true;
            
            fullArtManager.ShowFullArt(
                currentNode.data.fullArt,
                currentNode.data.fadeIn, () => {    // Use node fadeIn speed so fullArt can be extensible
                    isInputDisabled = false;
                    StartDialogue(currentNode.data.dialogue, type ?? currentNode.data.type, SFXOn);
                },
                Script_FullArtManager.FullArtState.DialogueManager
            );
        }
        else
        {
            StartDialogue(currentNode.data.dialogue, type ?? currentNode.data.type, SFXOn);
        }
    }

    /// <summary>
    /// called via player input to move forward in dialogue to ATTEMPT to continue dialogue
    /// </summary>
    /// <returns>
    /// null if the command is disabled (e.g. unskippable typing, etc)
    /// true if is continuing, false if not
    /// </returns>
    public bool? ContinueDialogue()
    {
        // prevent from stacking continuations
        if (isRenderingDialogueSection || isInputMode || isInputDisabled)
        {   
            print(
                $"could not continue dialogue " +
                $"isRenderingDialogueSection: {isRenderingDialogueSection} " +
                $"isInputMode: {isInputMode} " +
                $"isInputDisabled: {isInputDisabled}"
            );
            return null;
        }
        if (dialogueSections.Count == 0)
        {
            bool isEnding = OnEndDialogueSections();
            return !isEnding;
        }

        DisplayNextDialoguePortion();
        return true;
    }

    public void NextDialogueNode(int childIdx)
    {
        // to prevent multiple calls when in the middle of handling fade in callback
        if (isInputDisabled)    return;
        
        ShowDialogue();
        EndInputMode();
        
        Script_FullArt lastFullArt      = currentNode.data.fullArt;
        FadeSpeeds lastFadeOutSpeed     = currentNode.data.fadeOut;   

        currentNode = currentNode.data.children[childIdx];

        /// Current node is empty
        if (currentNode.data.dialogue.sections.Length == 0)
        {
            OnEndDialogueSections();
            return;
        }
        SetupTypingSpeed();

        EnqueueDialogueSections(currentNode.data.dialogue);
        SetupCanvases(currentNode.data.dialogue, currentNode.data.type);
        
        // if (nameText != null)
        //     nameText.text = Script_Utils.FormatString(currentNode.data.dialogue.name) + ":";
        
        isInputDisabled = true;
        
        // fade into a new fullArt
        if (currentNode.data.fullArt)
        {
            // use fadeIn if it's a new fullArt or fadeTransition if just switching out a fullArt
            FadeSpeeds fadeInSpeed = lastFullArt == null ? currentNode.data.fadeIn : currentNode.data.fadeTransition;
            if (lastFullArt != null)
            {
                fullArtManager.TransitionOutFullArt(lastFullArt, fadeInSpeed, null);
            }
            fullArtManager.ShowFullArt(
                currentNode.data.fullArt,
                fadeInSpeed, () =>
                {
                    DisplayNextDialoguePortionNextFrame();
                },
                Script_FullArtManager.FullArtState.DialogueManager
            );
        }
        else
        {
            if (lastFullArt == null)
            {
                /// Wait for next frame to show dialogue so Inputs don't carry over into 
                /// dialogue (e.g. enter from choices being read as skip dialogue input)
                DisplayNextDialoguePortionNextFrame();
                return;
            }
            
            /// Only remove if DialogueManager is the controller of the fullArtCanvas atm
            if (fullArtManager.state == Script_FullArtManager.FullArtState.DialogueManager)
            {
                Debug.Log("Calling HideFullArt() from DialogueManager");
                fullArtManager.HideFullArt(lastFullArt, lastFadeOutSpeed, () =>
                {
                    DisplayNextDialoguePortionNextFrame();
                });
            }
        }

        void DisplayNextDialoguePortionNextFrame()
        {
            StartCoroutine(WaitToDisplayNextDialoguePortion());
            IEnumerator WaitToDisplayNextDialoguePortion()
            {
                yield return null;
                DisplayNextDialoguePortion();
                isInputDisabled = false;
            }
        }
    }

    public bool IsDialogueSkippable()
    {
        return isRenderingDialogueSection && !dialogueSection.isUnskippable;
    }

    void SetupTypingSpeed()
    {
        if (currentNode.data.typingSpeed == DialogueTypingSpeed.Fast)
            charPauseLength = charPauseFast;
        else
            charPauseLength = charPauseDefault;
    }

    bool CheckNodeChildren()
    {
        if (
            currentNode.data.children.Length > 0
            && dialogueSections.Count == 0
        )
        {
            foreach (Script_DialogueNode node in currentNode.data.children)
            {
                if (node == null)   Debug.LogError("You're missing references to dialogue child nodes ♥.");
            }
            return true;
        }

        return false;
    }

    /// <summary>
    /// ensures the choices also need player input
    /// </summary>
    /// <returns></returns>
    bool CheckChoices()
    {
        if (
            (currentNode.data.children.Length > 1 || currentNode.data.isChoices)
            && !( // following node types have their own managers
                currentNode is Script_DialogueNode_SavePoint
                || currentNode is Script_DialogueNode_PaintingEntrance
            )
            && !CheckChoiceAutoHandled() // if autohandled, don't need player input
        )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void HandleChoices()
    {
        isInputMode = true;
        choiceManager.StartChoiceMode(currentNode);
    }

    /// <summary>
    /// use the OnAutoChoice events to automatically choose between dialogue nodes
    /// e.g. choosing a certain dialogue node if player spoke with X NPC
    /// </summary>
    /// <returns></returns>
    bool HandleAutoNextNodeChoice()
    {
        OnNextNodeChoiceArgs myModifiableEvent = new OnNextNodeChoiceArgs();
        // allow eventHandlers to modify the object
        currentNode.data.OnAutoChoice.Invoke(myModifiableEvent);
        int childIdx = myModifiableEvent.choice;
        if (childIdx > -1)
        {
            NextDialogueNode(childIdx);
            return true;
        }

        return false;
    }

    void EnqueueDialogueSections(Model_Dialogue dialogue)
    {
        foreach(Model_DialogueSection _dialogueSection in dialogue.sections)
        {
            dialogueSections.Enqueue(_dialogueSection);
        }
    }
    
    void StartDialogue(
        Model_Dialogue dialogue,
        string type,
        bool SFXOn = true
    )
    {
        isKeepingDialogueUp = false;
        ClearQueueState();
        
        SetupCanvases(dialogue, type);
        
        // specify silent typing dialogue types
        if (
            type == Const_DialogueTypes.Type.Read
            || type == Const_DialogueTypes.Type.Item
            || type == Const_DialogueTypes.Type.PaintingEntrance
        )
            isSilentTyping = true;
        else
            isSilentTyping = false;

        /// Item types, player will be in picking-up state 
        if (type != Const_DialogueTypes.Type.Item)     Script_Game.Game.GetPlayer().SetIsTalking();
        ShowDialogue();

        if (SFXOn && !isSilentTyping)
        {
            audioSource.PlayOneShot(dialogueStartSoundFX, dialogueStartVolumeScale);
        }

        EnqueueDialogueSections(dialogue);
        ContinueDialogue();
    }

    private void DisplayNextDialoguePortion()
    {   
        playerName = Script_Names.Player;
        
        StartRenderingDialoguePortion();

        dialogueSection = dialogueSections.Dequeue();
        
        if (dialogueSection.noContinuationIcon)     noContinuationIcon = true;
        else                                        noContinuationIcon = false;

        foreach(string _line in dialogueSection.lines)
        {
            lines.Enqueue(_line);
        }
        
        lineCount = 0;
        ClearActiveCanvasTexts();
        DisplayNextLine();
    }

    void DisplayNextLine()
    {
        // prevent from stacking continuations
        // if (isRenderingDialogueSection || isInputMode)    return;
        if (lines.Count == 0)
        {
            FinishRenderingDialogueSection();
            return;
        }
        activeCanvasText = activeCanvasTexts[lineCount];

        if (currentNode.data.isDynamicLines)
        {
            activeCanvasText.enableWordWrapping = false;
            activeCanvasText.overflowMode = TMPro.TextOverflowModes.Overflow;
        }
        else
        {
            activeCanvasText.enableWordWrapping = true;
        }
        
        string unformattedLine = lines.Dequeue();

        formattedLine = Script_Utils.FormatString(unformattedLine);

        coroutine = TypeLine(formattedLine);
        
        StartCoroutine(coroutine);
    }

    /// <summary>
    /// start: tag converter algo, takes tagged text
    /// e.g. <i><b><size=18>hello world</i></b></size=18> into
    /// <i><b><size=18>h</i></b></size=18><i><b><size=18>e</i></b></size=18> etc etc...
    /// NOTE: doesn't work for nested tags!!! need to separate it like <i>something</i><i><b>{12}</b></i>
    /// </summary>
    IEnumerator TypeLine(string sentence)
    {
        bool isTracking = false;
        bool isWrapNextLetter = false;
        bool isFindingClosingTags = false;
        bool isSkipLetter = false;
        
        string wrappedChar = "";
        string wrap = "";
        
        int tagsCount = 0;

        activeCanvasText.text = "";
        
        foreach(char letter in formattedLine.ToCharArray())
        {
            // play TypeSFX on pauses
            if (letter.Equals('|'))
            {
                shouldPlayTypeSFX = true;
                yield return new WaitForSeconds(pauseLength);
            }

            else if (isFindingClosingTags)
            {
                if (letter.Equals('>'))
                {
                    tagsCount--;
                    // reset state, we know we've covered all the tags we've added
                    if (tagsCount == 0)
                    {
                        // sets to starting state
                        wrappedChar = "";
                        wrap = ""; 

                        isFindingClosingTags = false;
                        isTracking = false;
                        isSkipLetter = false;
                    }
                }
            }
            else
            {
                if (isWrapNextLetter)
                {
                    // if we find < and no wrapped Char yet, we know it's another tag
                    if (letter.Equals('<'))
                    {
                        if (wrappedChar == "")
                        {
                            wrap += letter;
                            isSkipLetter = true;
                            isWrapNextLetter = false;
                        }
                        // else, done wrapping characters since wrappedChar
                        // is loaded; we know we're now looking for closing tags
                        else
                        {
                            isFindingClosingTags = true;
                            isSkipLetter = true;
                            isWrapNextLetter = false;
                        }
                    }
                    else
                    {
                        // will give you all the tags
                        // ex: <size=18><b><i>[char]</i></b></size>
                        wrappedChar = WrapCharWithTags(wrap, letter);
                        isSkipLetter = false;
                    }
                }
                else if (letter.Equals('<'))
                {
                    isSkipLetter = true;
                    wrap += letter;
                    isTracking = true;
                }
                else if (isTracking)
                {
                    isSkipLetter = true;
                    wrap += letter;

                    if (letter.Equals('>'))
                    {
                        isWrapNextLetter = true;
                        tagsCount++;
                    }
                }

                /*
                    end
                */

                if (!isSkipLetter)
                {
                    // only play typeSFX every other char
                    if (shouldPlayTypeSFX == true && !isSilentTyping)
                    {
                        audioSource.PlayOneShot(typeSFX, typingVolumeScale);
                        shouldPlayTypeSFX = false;
                    } else
                    {
                        shouldPlayTypeSFX = true;
                    }

                    activeCanvasText.text += wrappedChar == "" ? letter.ToString() : wrappedChar;

                    yield return new WaitForSeconds(charPauseLength);
                }
            }
        }

        lineCount++;
        DisplayNextLine();
    }

    string WrapCharWithTags(string wrap, char c)
    {
        // wrap will be <size><i><b>
            // need to check if next char is <, if not we know it's a char
            // if is continue adding to wrap
        // only handles <size...>, <i> & <b>
		
        string size20Rx =           "<size=20>";
        string size16Rx =           "<size=16>";
        string boldRx =             "<b>";
        string italicRx =           "<i>";
        string wrappedChar =        c.ToString();
		
		if (Regex.IsMatch(wrap, size20Rx))  wrappedChar = "<size=20>" + wrappedChar + "</size>";
        if (Regex.IsMatch(wrap, size16Rx))  wrappedChar = "<size=16>" + wrappedChar + "</size>";
        if (Regex.IsMatch(wrap, boldRx))    wrappedChar = "<b>" + wrappedChar + "</b>";
        if (Regex.IsMatch(wrap, italicRx))  wrappedChar = "<i>" + wrappedChar + "</i>";
        
        return wrappedChar;
    }

    void StartRenderingDialoguePortion()
    {
        isRenderingDialogueSection = true;
    }
    
    void FinishRenderingDialogueSection()
    {
        isRenderingDialogueSection = false;

        // check if should prompt input mode
        if (CheckForInputMode())    
        {
            StartInputMode(currentNode.data.inputMode);
            return;
        }

        // show choices after finishing typing line or skipped
        if (CheckNodeChildren())
        {
            // check for choices first
            if (CheckChoices())  {
                HandleChoices();
                return;
            }
        }

        if (currentNode is Script_DialogueNode_SavePoint)
        {
            // the SavePoint node contains the custom prompt
            // after pressing space on a SavePoint node, will prompt the saveChoices
            saveManager.StartSavePromptMode();
            isInputMode = true;
            return;
        }
        else if (currentNode is Script_DialogueNode_PaintingEntrance)
        {
            /// 1 child means it is a prompt before the final painting entrance question
            if (currentNode.data.children.Length > 1)
            {
                paintingEntranceManager.StartPaintingEntrancePromptMode();
                isInputMode = true;
                return;
            }
        }
    }

    /// <summary>
    /// checks if there are any events on the event handler
    /// counts as not null if has attached an object target;
    /// doesn't need to have to specify a function
    /// </summary>
    bool CheckChoiceAutoHandled()
    {
        Model_NodeChoiceEvent onClickEvent = currentNode.data.OnAutoChoice;
        for (int i = 0; i < onClickEvent.GetPersistentEventCount(); i++)
        {
            if (onClickEvent.GetPersistentTarget(i) != null)    return true;
        }

        return false;
    }
    bool CheckForInputMode()
    {   
        if (
            dialogueSections != null
            && dialogueSections.Count == 0
            && !isRenderingDialogueSection
            && currentNode.data.action == Const_DialogueActions.InputMode
        )
        {
            return true;
        }

        return false;
    }

    void ClearQueueState()
    {
        dialogueSections.Clear();
        lines.Clear();
    }

    void ClearActiveCanvasTexts()
    {
        foreach (TextMeshProUGUI t in activeCanvasTexts)
        {
            t.text = "";
        }   
    }

    /// Clear EVERY TING all dialogue text
    // TODO REFACTOR: Instead call Clear from Script_Canvas
    // Change all Canvas references to Script_Canvas
    void ClearAllCanvasTexts()
    {
        DefaultCanvasName.text = "";
        foreach (TextMeshProUGUI t in DefaultCanvasDialogueTexts)
        {
            t.text = "";
        }
        DefaultCanvasNameTop.text = "";
        foreach (TextMeshProUGUI t in DefaultCanvasDialogueTextsTop)
        {
            t.text = "";
        }
        foreach (TextMeshProUGUI t in CanvasChoice1RowDialogueTexts)
        {
            t.text = "";
        }
        foreach (TextMeshProUGUI t in CanvasChoice1RowTopDialogueTexts)
        {
            t.text = "";
        }
        foreach (TextMeshProUGUI t in SaveChoiceDialogueTexts)
        {
            t.text = "";
        }
        foreach (TextMeshProUGUI t in PaintingEntranceChoiceDialogueTexts)
        {
            t.text = "";
        }
        foreach (TextMeshProUGUI t in ReadChoiceDialogueTexts)
        {
            t.text = "";
        }
        foreach (TextMeshProUGUI t in DefaultReadTextCanvasTexts)
        {
            t.text = "";
        }
        foreach (TextMeshProUGUI t in DefaultReadTextCanvasTextsTop)
        {
            t.text = "";
        }
        foreach (TextMeshProUGUI t in ItemDescriptionCanvasTextsBottom)
        {
            t.text = "";
        }
        foreach (TextMeshProUGUI t in ItemDescriptionCanvasTextsTop)
        {
            t.text = "";
        }
    }

    public void StartInputMode(InputMode inputMode)
    {
        isInputMode = true;

        // set input canvas active
        inputManager.Initialize(inputMode);

        inputManagerCanvas.gameObject.SetActive(true);
        inputManager.gameObject.SetActive(true);
    }

    public void EndInputMode(int childIdx)
    {
        isInputMode = false;

        // set input canvas active
        inputManagerCanvas.gameObject.SetActive(false);
        inputManager.gameObject.SetActive(false);
        
        print("childIdx: " + childIdx);

        if (CheckNodeChildren() && childIdx < currentNode.data.children.Length)
        {
            NextDialogueNode(childIdx);
        }
        else
        {
            HandleEndDialogue();
        }
    }

    public void EndInputMode()
    {
        isInputMode = false;    
    }

    public void EndSaveEntryMode()
    {
        isInputMode = false;
    }

    /// <summary>
    /// called when about to finish up dialoge portions, via player input
    /// </summary>
    /// <returns>
    /// true if actually is ending dialogue
    /// </returns>
    public bool OnEndDialogueSections()
    {
        // move to next node
        /// If is a choice node (>1 child), then allow choiceManager to have control
        if (CheckNodeChildren() && !CheckChoices())
        {   
            /// NOTE: this will be handled further down if actually ending dialogue
            /// to prevent any race cases
            HandleDialogueNodeAction(); 

            if (!HandleAutoNextNodeChoice())    NextDialogueNode(0);

            return false;
        }
        // actually end dialogue
        else
        {
            /// HandDialogueNodeAction() handled later
            HandleEndDialogue();
            return true;
        }
    }

    /// <summary>
    /// function when ACTUALLY ENDING dialogue (e.g. no more nodes )
    /// NOTE: an event could trigger another dialogue start though
    /// </summary>
    public void HandleEndDialogue()
    {
        isInputDisabled = true;
        EndInputMode();
        
        /// Handle ending on a FullArt node
        /// Allow the InteractableFullArt object to handle this if it's a fullArt object
        /// e.g. not just fullArt attached to a dialogueNode
        bool isFullArtControlledByMe = fullArtManager.state == Script_FullArtManager.FullArtState.DialogueManager;
        if (fullArtManager.activeFullArt != null && isFullArtControlledByMe)
        {
            fullArtManager.HideFullArt(fullArtManager.activeFullArt, currentNode.data.fadeOut, () =>
            {
                isInputDisabled = false;
                EndDialogue();
            });
        }
        else
        {
            isInputDisabled = false;
            EndDialogue();
        }

        void EndDialogue()
        {
            if (!HandleKeepDialogueUpOnAction())
                HideDialogue();

            /// in picking up state, player state will be handled by player Action
            /// upon finishing the item description dialogue
            if (currentNode.data.type != Const_DialogueTypes.Type.Item)
            {
                Script_Game.Game.GetPlayer().SetIsInteract();
            }

            // end dialogue event
            HandleDialogueNodeAction();
            Script_DialogueEventsManager.DialogueEndEvent();
            activeInteractable?.StartDialogueCoolDown();
            activeInteractable = null;
        }
    }


    // actions will be activated after "space" is pressed to move to next dialogue
    void HandleDialogueNodeAction()
    {
        if (currentNode.data.OnNextNodeAction.CheckUnityEventAction())
        {
            currentNode.data.OnNextNodeAction.Invoke();
        }

        if (!string.IsNullOrEmpty(currentNode.data.action))
        {
            print("doing node action: " + currentNode.data.action);
            game.HandleDialogueNodeAction(currentNode.data.action);
        }
    }

    bool HandleKeepDialogueUpOnAction()
    {
        // on option to keep dialogue up (for command prompts e.g. tutorials)
        if (
            currentNode.data.showDialogueOnAction
            && (
                !string.IsNullOrEmpty(currentNode.data.action)
                || !string.IsNullOrEmpty(currentNode.data.updateAction)
            )
        )
        {
            // set for dialogue continuation icon
            isKeepingDialogueUp = true;
            return true;
        }

        return false;
    }

    public void SkipTypingSentence()
    {
        if (dialogueSection.isUnskippable)  return;
        
        Debug.Log("Skipping typing sentence");

        // replace all dialogue portions
        if (isRenderingDialogueSection)
        {
            StopCoroutine(coroutine);

            for (int i = 0; i < dialogueSection.lines.Length; i++)
            {
                // interpolate playerName
                string unformattedLine = dialogueSection.lines[i];
                string _formattedLine = Script_Utils.FormatString(unformattedLine);
                
                // remove pause indicators
                _formattedLine = _formattedLine.Replace("|", string.Empty);
                
                activeCanvasText = activeCanvasTexts[i];
                activeCanvasText.text = _formattedLine;
            }

            lines.Clear();
            FinishRenderingDialogueSection();
        }
    }

    public void HideDialogue()
    {
        // Hide at end of dialogue so we don't see flicker when we change types
        if (activeCanvas != null)   activeCanvas.gameObject.SetActive(false);
        canvas.gameObject.SetActive(false);
        canvas.alpha = 0f;
        canvas.blocksRaycasts = false;
    }

    void ShowDialogue()
    {
        canvas.gameObject.SetActive(true);
        canvas.alpha = 1f;
        canvas.blocksRaycasts = true;
    }

    void SetupCanvases(Model_Dialogue dialogue, string type)
    {
        /// Allow option item descriptions to not override text
        if (
            currentNode.data.type == Const_DialogueTypes.Type.Item
            && currentNode.data.isKeepLastDialogueUp
        )
        {
            Debug.Log("SetupCanvases(): not clearing all canvases, only disabling non-written ones");
            canvasHandler.DisableOnlyEmptyCanvases();
        }
        else
        {
            Debug.Log("SetupCanvases(): clearing all canvases now");
            ClearAllCanvasTexts();
            canvasHandler.DisableCanvases();
        }
        
        string canvasLocType = Const_DialogueTypes.Location.Bottom;
        if (currentNode.data.locationType != null)
        {
            canvasLocType = currentNode.data.locationType;
        }
        
        if (type == Const_DialogueTypes.Type.Read)
        {
            if (CheckChoices() && !CheckForInputMode())
            {
                SetDialogueCanvasToReadChoice();
            }
            else
            {
                if (canvasLocType == Const_DialogueTypes.Location.Top)
                {
                    activeCanvas = DefaultReadTextCanvasTop.transform;
                    activeCanvasTexts = DefaultReadTextCanvasTextsTop;
                    DefaultReadTextCanvasTop.gameObject.SetActive(true);
                }
                else
                {
                    activeCanvas = DefaultReadTextCanvas.transform;
                    activeCanvasTexts = DefaultReadTextCanvasTexts;
                    DefaultReadTextCanvas.gameObject.SetActive(true);
                }
                
                // activeCanvas.GetComponent<Script_Canvas>().ContinuationIcon.Setup();
            }
        }
        else if (type == Const_DialogueTypes.Type.Item)
        {
            if (canvasLocType == Const_DialogueTypes.Location.Top)
            {
                activeCanvas = ItemDescriptionCanvasTop.transform;
                activeCanvasTexts = ItemDescriptionCanvasTextsTop;
                ItemDescriptionCanvasTop.gameObject.SetActive(true);
            }
            else
            {
                activeCanvas = ItemDescriptionCanvasBottom.transform;
                activeCanvasTexts = ItemDescriptionCanvasTextsBottom;
                ItemDescriptionCanvasBottom.gameObject.SetActive(true);
            }

            // if other continuation icons are active, disable them
            canvasHandler.DisableInactiveContinuationIcons();
            // activeCanvas.GetComponent<Script_Canvas>().ContinuationIcon.Setup();
        }
        else if (currentNode is Script_DialogueNode_SavePoint)
        {
            activeCanvas = SaveChoiceCanvas.transform;
            SaveChoiceCanvas.gameObject.SetActive(true);
            nameText = SaveChoiceName;
            activeCanvasTexts = SaveChoiceDialogueTexts;
            // SetupName(dialogue.name);
        }
        else if (
            currentNode is Script_DialogueNode_PaintingEntrance
            || currentNode.data.type == Const_DialogueTypes.Type.PaintingEntrance
        )
        {
            activeCanvas = PaintingEntranceChoiceCanvas.transform;
            PaintingEntranceChoiceCanvas.gameObject.SetActive(true);
            activeCanvasTexts = PaintingEntranceChoiceDialogueTexts;
        }
        else if (
            CheckChoices()
            && !(currentNode is Script_DialogueNode_SavePoint)
            && !CheckForInputMode()
        )
        {
            // don't set continuation for choice canvas section
            SetDialogueCanvasToCanvasChoice1Row(canvasLocType);
        }
        else
        {
            if (canvasLocType == Const_DialogueTypes.Location.Top)
            {
                activeCanvas = DefaultCanvasTop.transform;
                // activeCanvas.GetComponent<Script_Canvas>().ContinuationIcon.Setup();

                DefaultCanvasTop.gameObject.SetActive(true);
                activeCanvasTexts = DefaultCanvasDialogueTextsTop;
                nameText = DefaultCanvasNameTop;
            }
            else
            {
                activeCanvas = DefaultCanvas.transform;
                // activeCanvas.GetComponent<Script_Canvas>().ContinuationIcon.Setup();

                DefaultCanvas.gameObject.SetActive(true);
                activeCanvasTexts = DefaultCanvasDialogueTexts;
                nameText = DefaultCanvasName;
            }
            // SetupName(dialogue.name);           
        }
        foreach (var c in activeCanvasTexts)     c.gameObject.SetActive(true);
        activeCanvas.GetComponent<Script_Canvas>().Setup();
        SetupName(dialogue.name);
    }

    void SetupName(string name)
    {
        Debug.Log("Attempting to set up name now");

        if (nameText == null)   return;

        nameText.text = Script_Utils.FormatString(name) + (string.IsNullOrEmpty(name) ? "" : ":");

        if (Debug.isDebugBuild && Const_Dev.IsDevMode && (name == "" || name == null))
        {
            Debug.LogWarning("No name was provided for dialogue");
        }
    }

    void SetDialogueCanvasToCanvasChoice1Row(string loc)
    {
        if  (loc == Const_DialogueTypes.Location.Top)
        {
            activeCanvas = CanvasChoice1RowTop.transform;
            CanvasChoice1RowTop.gameObject.SetActive(true);
            
            activeCanvasTexts = CanvasChoice1RowTopDialogueTexts;
            nameText = CanvasChoice1RowTopName;
            nameText.text = Script_Utils.FormatString(currentNode.data.dialogue.name) + ":";
        }
        else
        {
            activeCanvas = CanvasChoice1Row.transform;
            CanvasChoice1Row.gameObject.SetActive(true);
            
            activeCanvasTexts = CanvasChoice1RowDialogueTexts;
            nameText = CanvasChoice1RowName;
            nameText.text = Script_Utils.FormatString(currentNode.data.dialogue.name) + ":";
        }
    }

    void SetDialogueCanvasToReadChoice()
    {
        activeCanvas = ReadChoiceCanvasBottom.transform;
        ReadChoiceCanvasBottom.gameObject.SetActive(true);
        
        activeCanvasTexts = ReadChoiceDialogueTexts;
    }

    void InitialState()
    {
        HideDialogue();
        ClearAllCanvasTexts();
        canvasHandler.DisableCanvases();
    }

    public void Initialize()
    {
        Debug.Log($"{name} INITIALIZED");
        
        if (DialogueManager == null)
        {
            DialogueManager = this;
        }
        else if (DialogueManager != this)
        {
            Destroy(this.gameObject);
        }
    }

    // NOTE: DialogueManager is Setup on EVERY level
    public void Setup()
    {
        dialogueSections = new Queue<Model_DialogueSection>();
        lines = new Queue<string>();

        inputManager.Setup();
        inputManager.gameObject.SetActive(false);
        
        choiceManager = GetComponent<Script_ChoiceManager>();
        choiceManager.Setup();
        
        saveManager.Setup();
        paintingEntranceManager.Setup();
        
        fullArtManager.Setup();

        InitialState();
    }
}
