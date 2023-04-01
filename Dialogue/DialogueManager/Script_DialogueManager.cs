using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
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
/// 
/// Notes on Dialogue Nodes:
/// -   Default Psychic Nodes, refrain from putting any events / actions following
///     or player's "can't understand" feedback will not fire. 
/// </summary>
public class Script_DialogueManager : MonoBehaviour
{
    public const float charTypeTime = 0.01f; // <100fps
    public const float pauseLength = 0.475f;
    public const char DefaultDemonNPCChar = '�';
    public const float autoNextWaitTime = 0.05f;
    
    public enum States
    {
        Active              = 0,
        Disabled            = 1,
    }

    [SerializeField] private States state;
    
    public static Script_DialogueManager DialogueManager;
    public static float DialogueContinuationFlickerInterval = 0.75f;
    private static char PauseTextCommand = '|';
    
    
    [SerializeField] private float dialogueFadeTime;

    // NOTE: for cut scenes this may be null if not defined by call to StartDialogue.
    public Script_Interactable activeInteractable;
    public Script_Game game;
    public CanvasGroup canvas;
    public AudioSource audioSource;
    public AudioClip dialogueStartSoundFX;
    public float dialogueStartVolumeScale;
    public float typingVolumeScale;

    public Script_CanvasGroupController activeCanvas;
    public TextMeshProUGUI activeCanvasText;
    public TextMeshProUGUI[] activeCanvasTexts;
    /*
        NAME
        LINE
        LINE
    */
    [SerializeField] private Script_CanvasGroupController DefaultCanvas;
    [SerializeField] private TextMeshProUGUI DefaultCanvasName;
    [SerializeField] private TextMeshProUGUI[] DefaultCanvasDialogueTexts;

    // public Canvas DefaultCanvasTop;
    // public TextMeshProUGUI DefaultCanvasNameTop;
    // public TextMeshProUGUI[] DefaultCanvasDialogueTextsTop;

    [SerializeField] private Script_CanvasGroupController CanvasChoice1Row;
    public TextMeshProUGUI CanvasChoice1RowName;
    public TextMeshProUGUI[] CanvasChoice1RowDialogueTexts;

    // public Canvas CanvasChoice1RowTop;
    // public TextMeshProUGUI CanvasChoice1RowTopName;
    // public TextMeshProUGUI[] CanvasChoice1RowTopDialogueTexts;

    // public Canvas SaveChoiceCanvas;
    // public TextMeshProUGUI SaveChoiceName;
    // public TextMeshProUGUI[] SaveChoiceDialogueTexts;

    /// based on SaveChoiceCanvas
    public Script_CanvasGroupController PaintingEntranceChoiceCanvas;
    public TextMeshProUGUI[] PaintingEntranceChoiceDialogueTexts;

    [SerializeField] private Script_CanvasGroupController ReadChoiceCanvasBottom; 
    public TextMeshProUGUI[] ReadChoiceDialogueTexts;

    /*
        LINE
        LINE
    */
    public Script_CanvasGroupController DefaultReadTextCanvas;
    public TextMeshProUGUI[] DefaultReadTextCanvasTexts;
    
    // public Canvas DefaultReadTextCanvasTop;
    // public TextMeshProUGUI[] DefaultReadTextCanvasTextsTop;
    
    public Script_CanvasGroupController ItemDescriptionCanvasBottom;
    public TextMeshProUGUI[] ItemDescriptionCanvasTextsBottom;
    
    // public Canvas ItemDescriptionCanvasTop;
    // public TextMeshProUGUI[] ItemDescriptionCanvasTextsTop;
    
    public Script_PaintingEntranceManager paintingEntranceManager;
    [SerializeField] private Script_InputManager inputManager;
    [SerializeField] private Script_FullArtManager fullArtManager;
    [SerializeField] private Script_DialogueManagerCanvasHandler canvasHandler;
    [SerializeField] private Script_MynesMirrorManager mynesMirrorManager;

    public bool isRenderingDialogueSection = false;
    public int lineCount = 0;
    public Queue<Model_DialogueSection> dialogueSections;
    public Queue<string> lines;
    
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

    private bool isSilentTyping = false;
    /// Flag to disable inputs while full art is fading in or out
    public bool isInputDisabled;
    
    // Flag to force an update of a passed in NPC's state.
    private bool isForceUpdateNPCState;

    // ------------------------------------------------------------------
    // Dialogue Box Modifiers
    
    [SerializeField] private Script_ImageDistorterController defaultDialogueImageDistorter;
    
    // ------------------------------------------------------------------
    // Player Feedback to First Interactions
    [SerializeField] private Script_DialogueNode cantUnderstandReactionNode;
    [SerializeField] private float beforeCantUnderstandReactionWaitTime;
    [SerializeField] private bool shouldCantUnderstandReaction;
    private bool didCantUnderstandReactionDone;

    // ------------------------------------------------------------------
    // Player Feedback to Hit NPC
    public bool IsHandlingNPCOnHit { get; set; }
    
    private States State
    {
        get => state;
        set => state = value;
    }

    public bool IsOnEndUpdateNPCState
    {
        get => isForceUpdateNPCState;
        set => isForceUpdateNPCState = value;
    }

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
    
    // ------------------------------------------------------------------
    // Player Dialogue Reactions
    
    // If another cut scene or dialogue is planned after, skip this interaction.
    private IEnumerator WaitToTryCantUnderstandDialogue(Script_DialogueNode prevNode)
    {
        // Manually prevent HUD from fading back in.
        Script_HUDManager.Control.IsPaused = true;
        
        if (
            // Ensure we didn't enter a new cut scene.
            Script_Game.Game.state == Const_States_Game.Interact
            && Script_Game.Game.GetPlayer().State == Const_States_Player.Interact
            // Ensure a new dialogue didn't already start.
            && currentNode == prevNode
        )
        {
            Dev_Logger.Debug("WaitToTryCantUnderstandDialogue: Set game to Cut Scene.");
            isInputDisabled = true;
            Script_Game.Game.GetPlayer().SetIsTalking();
            game.ChangeStateCutScene();

            yield return new WaitForSeconds(beforeCantUnderstandReactionWaitTime);

            isInputDisabled = false;
            StartDialogueNode(cantUnderstandReactionNode, SFXOn: false);
            didCantUnderstandReactionDone = true;
        }
        
        Script_HUDManager.Control.IsPaused = false;
    }

    // ------------------------------------------------------------------
    
    public bool IsActive()
    {
        return State == States.Active;
    }

    public void SetActive(bool isActive)
    {
        if (isActive)   State = States.Active;
        else            State = States.Disabled;
    }
    
    public void StartDialogueNodeNextFrame(
        Script_DialogueNode node,
        bool SFXOn = true,
        string type = null,
        Script_Interactable talkingInteractive = null
    )
    {
        StartCoroutine(WaitToStartDialogueNode());

        IEnumerator WaitToStartDialogueNode()
        {
            yield return null;
            StartDialogueNode(node, SFXOn, type, talkingInteractive);
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
        Dev_Logger.Debug($"Starting dialogue node. isInputDisabled: {isInputDisabled}");
        
        if (isInputDisabled)
            return;
        
        // Handle keeping dialogue & FullArt up (new node, mid convo)
        Script_FullArt lastFullArt = null;
        bool isLeaveFullArtUp = false;
        
        if (currentNode != null)
        {
            lastFullArt = GetLastActiveFullArt(currentNode.data);
            isLeaveFullArtUp = currentNode.data.isLeaveFullArtUp;
        }
        
        currentNode = node;
        activeInteractable = talkingInteractive;

        /// Dev warning
        if (
            activeInteractable == null
            && game.state == Const_States_Game.Interact
            // Item dialogues will not come from an interactable and behave as cut scenes 
            && !(node is Script_ItemDialogueNode)
        )
            Debug.LogWarning("Ensure to reference the caller for StartDialogueNode()");

        SetupTypingSpeed();

        if (currentNode.data.OnBeforeNodeAction.CheckUnityEventAction())
        {
            currentNode.data.OnBeforeNodeAction.Invoke();
        }

        isInputDisabled = true;
        
        Script_FullArt currentFullArt = GetStartNodeFullArt(currentNode.data);

        Dev_Logger.Debug($"{name} lastFullArt {lastFullArt} currentFullArt {currentFullArt} isLeaveFullArtUp {isLeaveFullArtUp}");

        if (currentFullArt != null)
        {
            // In the case last dialogue node isKeepThisDialogue == true and was a different full art,
            // ensure to remove it
            // Note: Uses the current node's Fade In speed to always match.
            if (isKeepingDialogueUp && lastFullArt != null)
            {
                if (lastFullArt != currentFullArt)
                {
                    Dev_Logger.Debug("Fading out last full art");
                    fullArtManager.TransitionOutFullArt(lastFullArt, currentNode.data.fadeIn, null);
                    StartDialogueWithFullArt();
                }
                else
                    StartDialogueNoFullArt();

                return;
            }
            
            StartDialogueWithFullArt();
        }
        else
        {
            StartDialogueNoFullArt();
        }

        // Note: When starting dialogue with FA, need to set Player state synchronously (as is the case in StartDialogue);
        // otherwise, Player can still move away as FA fades in.
        void StartDialogueWithFullArt()
        {
            Dev_Logger.Debug("Fading in new full art");
            string currentType = type ?? currentNode.data.type;
            HandlePlayerState(currentType);
            
            // Handle changing left up FullArt to new pose on dialogue start.
            if (lastFullArt != null && lastFullArt != currentFullArt && isLeaveFullArtUp)
                fullArtManager.InitialState(lastFullArt);
            
            fullArtManager.ShowFullArt(
                currentFullArt,
                currentNode.data.fadeIn, () => {
                    StartDialogue(currentNode.data.dialogue, currentType, SFXOn);
                },
                Script_FullArtManager.FullArtState.DialogueManager
            );
        }

        void StartDialogueNoFullArt()
        {
            string currentType = type ?? currentNode.data.type;
            HandlePlayerState(currentType);

            StartDialogue(currentNode.data.dialogue, currentType, SFXOn);
        }

        // This tells dialogue continuation how to continue.
        // For Item types, player will be in picking-up state, which
        // follows a different dialogue pattern.
        void HandlePlayerState(string currentType)
        {
            if (currentType != Const_DialogueTypes.Type.Item)
                Script_Game.Game.GetPlayer().SetIsTalking();
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
        if (
            isRenderingDialogueSection
            || isInputMode
            || isInputDisabled
            || !IsActive()
        )
        {   
            Dev_Logger.Debug(
                $"could not continue dialogue " +
                $"isRenderingDialogueSection: {isRenderingDialogueSection} " +
                $"isInputMode: {isInputMode} " +
                $"isInputDisabled: {isInputDisabled} "
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
        if (isInputDisabled)
            return;
        
        EndInputMode();
        
        Script_FullArt lastFullArt      = GetLastActiveFullArt(currentNode.data);
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
        // Must disable dialogue continuation icon until after fade-ins
        activeCanvas.canvasChild.DisableContinuationIcon();

        // Ensure to immediately reinit the active canvas, since not fading in when doing NextNode.
        ShowDialogue();
        
        isInputDisabled = true;

        // Fetch the full art override or node's full art
        Script_FullArt currentFullArt = GetStartNodeFullArt(currentNode.data);

        // Fade into a new fullArt
        if (currentFullArt != null)
        {
            // If the current full art and last are the same, then no need to do anything with Full Art
            if (currentFullArt == lastFullArt)
            {
                Dev_Logger.Debug($"currentFullArt {currentFullArt} same as lastFullArt {lastFullArt}");

                DisplayNextDialoguePortionNextFrame();
            }
            else
            {
                Dev_Logger.Debug($"currentFullArt {currentFullArt} different vs. lastFullArt {lastFullArt}");
                
                // use fadeIn if it's a new fullArt or fadeTransition if just switching out a fullArt
                FadeSpeeds fadeInSpeed = lastFullArt == null ? currentNode.data.fadeIn : currentNode.data.fadeTransition;
                if (lastFullArt != null)
                {
                    fullArtManager.TransitionOutFullArt(lastFullArt, fadeInSpeed, null);
                }
                fullArtManager.ShowFullArt(
                    currentFullArt,
                    fadeInSpeed, () =>
                    {
                        DisplayNextDialoguePortionNextFrame();
                    },
                    Script_FullArtManager.FullArtState.DialogueManager
                );
            }
        }
        else
        {
            Dev_Logger.Debug($"{name} Next Dialogue Node, NO currentFullArt {currentFullArt}");
            
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
                Dev_Logger.Debug("Calling HideFullArt() from DialogueManager");
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
                // Reenable Continuation Icon
                activeCanvas.canvasChild.Setup();
                
                DisplayNextDialoguePortion();
                isInputDisabled = false;
            }
        }
    }

    /// <summary>
    /// Handle getting the last FullArt that was/is up whether it is an override or default object ref
    /// </summary>
    /// <param name="lastNodeData">data prop</param>
    /// <returns>Active FA ONLY if in FullArtState.DialogueManager state or last node's Full Art prop</returns>
    private Script_FullArt GetLastActiveFullArt(Model_DialogueNode lastNodeData)
    {
        Script_FullArt lastFullArt = lastNodeData.FullArt;
        
        // Get whatever FullArt has been left up, handling even the override case.
        // Full art manager will set activeFullArt to null after finishing FullArt hide/close, so test if it hasn't
        // been taken down yet (e.g. mid-convo)
        if (
            fullArtManager.state == Script_FullArtManager.FullArtState.DialogueManager
            && fullArtManager.activeFullArt != null
        )
            lastFullArt = fullArtManager.activeFullArt;
        
        return lastFullArt;
    }
    
    /// <summary>
    /// Checks the node's first dialogue section to see if a FA override is present,
    /// if one is, return it.
    /// </summary>
    /// <param name="nodeData">data prop</param>
    /// <returns>The full art if override is present, the node's full art, or null if neither are present</returns>
    private Script_FullArt GetStartNodeFullArt(Model_DialogueNode nodeData)
    {
        Script_FullArt currentFullArt = nodeData.FullArt;
        
        // Check if dialogue sections exist
        if (
            nodeData.dialogue != null
            && nodeData.dialogue.sections != null
            && nodeData.dialogue.sections.Length > 0
        )
        {
            // Check if FA override is populated
            var fullArtOverride = fullArtManager.GetFullArt(nodeData.dialogue.sections[0].fullArtOverride, out _);

            if (fullArtOverride != null)
                currentFullArt = fullArtOverride;
        }

        return currentFullArt;
    }

    public bool IsDialogueSkippable()
    {
        return isRenderingDialogueSection && !dialogueSection.isUnskippable;
    }

    // Handle nodes with typingSpeed DialogueTypingSpeed.Fast
    void SetupTypingSpeed() {}

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
    /// Ensures the choices also need player input.
    /// </summary>
    bool CheckChoices()
    {
        if (
            currentNode.IsChoicesNode()
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
        foreach (Model_DialogueSection _dialogueSection in dialogue.sections)
        {
            dialogueSections.Enqueue(_dialogueSection);
        }
    }
    
    private void StartDialogue(
        Model_Dialogue dialogue,
        string type,
        bool SFXOn = true
    )
    {
        Dev_Logger.Debug($"Start Dialogue Id {currentNode.Id}");

        bool noFadeIn = isKeepingDialogueUp || (
            currentNode.data.isCustomDialogueFadeIn
            && currentNode.data.dialogueFadeInSpeed == FadeSpeeds.None
        );

        isInputDisabled = false;
        isKeepingDialogueUp = false;
        
        ClearQueueState();
        
        SetupCanvases(dialogue, type);
        // Must disable dialogue continuation icon until after fade-ins
        activeCanvas.canvasChild.DisableContinuationIcon();

        // Specify silent typing dialogue types
        if (
            type == Const_DialogueTypes.Type.Read
            || type == Const_DialogueTypes.Type.Item
            || type == Const_DialogueTypes.Type.ItemNoPickUp
            || type == Const_DialogueTypes.Type.PaintingEntrance
        )
            isSilentTyping = true;
        else
            isSilentTyping = false;

        ShowDialogue();

        Dev_Logger.Debug("Fade In dialogue canvas");
        
        if (SFXOn && !isSilentTyping)
        {
            audioSource.PlayOneShot(dialogueStartSoundFX, dialogueStartVolumeScale);
        }

        EnqueueDialogueSections(dialogue);
        
        activeCanvas.InitialState();
        
        if (noFadeIn)
        {
            activeCanvas.Open();
            SetupContinueDialogue();
        }
        else
        {
            var fadeTime = currentNode.data.isCustomDialogueFadeIn ? 
                currentNode.data.dialogueFadeInSpeed.ToFadeTime()
                : dialogueFadeTime;
            activeCanvas.FadeIn(fadeTime, SetupContinueDialogue);
        }

        void SetupContinueDialogue()
        {
            // Reenable Continuation Icon
            activeCanvas.canvasChild.Setup();

            ContinueDialogue();
        }
    }

    private void DisplayNextDialoguePortion()
    {   
        playerName = Script_Names.Player;
        
        isRenderingDialogueSection = true;

        dialogueSection = dialogueSections.Dequeue();
        
        if (dialogueSection.noContinuationIcon)     noContinuationIcon = true;
        else                                        noContinuationIcon = false;

        if (dialogueSection.waitForTimeline)        SetActive(false);

        foreach(string _line in dialogueSection.lines)
        {
            lines.Enqueue(_line);
        }
        
        // Handle FA override
        HandleMidConvoFullArtOverride(dialogueSection);
        
        lineCount = 0;
        ClearActiveCanvasTexts();
        DisplayNextLine();
    }

    /// <summary>
    /// Change FullArt poses between dialogue sections.
    /// </summary>
    private void HandleMidConvoFullArtOverride(Model_DialogueSection section)
    {
        if (section.fullArtOverride == FullArtPortrait.None)
            return;
        
        bool isMynesMirrorNode;
        Script_FullArt fullArtOverride = fullArtManager.GetFullArt(section.fullArtOverride, out isMynesMirrorNode);

        // Allow Mynes Mirror Manager to handle
        if (isMynesMirrorNode)
        {
            mynesMirrorManager.HandleMidConvoPortraitOverride(section.fullArtOverride);
        }
        // Prevent a second call if the active full art is the same as override. 
        else if (fullArtOverride != null && fullArtOverride != fullArtManager.activeFullArt)
        {
            fullArtManager.InitialState(fullArtManager.activeFullArt);
            fullArtManager.OpenFullArt(fullArtOverride, Script_FullArtManager.FullArtState.DialogueManager);
        }
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
        activeCanvasText.enableWordWrapping = true;
        
        string unformattedLine = lines.Dequeue();

        formattedLine = Script_Utils.FormatString(
            unformattedLine,
            isFormatInventoryKey: currentNode.data.isFormatInventoryKey,
            isFormatSpeedKey: currentNode.data.isFormatSpeedKey,
            isFormatMaskCommandKey: currentNode.data.isFormatMaskCommandKey
        );

        HandleTeletypeReveal(formattedLine, activeCanvasText);
    }

    private void HandleTeletypeReveal(string sentence, TextMeshProUGUI textUI)
    {
        // for Default Psychic Nodes
        if (isZalgofyNode)
            sentence = Script_Utils.ZalgofyUnrichString(sentence);
        
        coroutine = TeletypeRevealLine(
            sentence,
            activeCanvasText,
            OnTeletypeRevealLineDone
        );
        StartCoroutine(coroutine);

        void OnTeletypeRevealLineDone()
        {
            lineCount++;
            DisplayNextLine();

            // Look at current line, if is Auto Skip, call Continue Dialogue
            if (dialogueSection.autoNext)
            {
                // Disable and continue so can still read
                isInputDisabled = true;
                
                StartCoroutine(WaitToContinue());
            }
        }

        IEnumerator WaitToContinue()
        {
            yield return new WaitForSeconds(autoNextWaitTime);

            isInputDisabled = false;
            ContinueDialogue();
        }
    }

    private bool isZalgofyNode => currentNode != null
            && currentNode.data != null
            && currentNode.data.isZalgofy;

    public static IEnumerator TeletypeRevealLine(
        string sentence,
        TextMeshProUGUI textUI,
        Action cb,
        bool silenceOverride = false,
        bool isGlitchText = false,
        AudioClip sfxOverride = null
    )
    {   
        if (isGlitchText)
        {
            textUI.text = sentence;
            sentence = ZalgofyString(sentence, textUI);
        }
        
        // Hide Text Commands.
        string formattedSentence = sentence.Replace(
            PauseTextCommand.ToString(),
            $"<size=0>{PauseTextCommand.ToString()}</size>"
        );

        // First initialize the canvas with text and hide all text.
        textUI.text = formattedSentence;
        textUI.maxVisibleCharacters = 0;
        TMP_CharacterInfo[] charInfos = textUI.textInfo.characterInfo;

        // TMP won't fill TextInfo with data until mesh is rendered so force the update manually.
        // (Needed for skipping sentence on same frame as render, so we'll have access to an up
        // to-date textInfo.characterCount)
        textUI.ForceMeshUpdate(true);
        
        Dev_Logger.Debug($"FormattedSentence <{formattedSentence}>");
        Dev_Logger.Debug($"characterCount <{textUI.textInfo.characterCount}> maxVisibleCharacters <{textUI.maxVisibleCharacters}>");
        
        // Must wait a frame so TMP can update for new hidden text.
        yield return null;

        // Get # of visible characters in Text object.
        int totalVisibleCharacters = textUI.textInfo.characterCount;
        int visibleCount = 0;
        
        while (visibleCount < totalVisibleCharacters)
        {
            if (silenceOverride)
                Script_SFXManager.SFX.StartDialogueTyping(sfxOverride);
            
            // Reveal current character.
            textUI.maxVisibleCharacters = visibleCount;
            
            // Get the next visible character and handle Text Commands.
            TMP_CharacterInfo charInfo = textUI.textInfo.characterInfo[visibleCount];
            char nextVisibleChar = charInfo.character;
            
            if (nextVisibleChar == PauseTextCommand)
                yield return new WaitForSeconds(pauseLength);
            else
            {
                // Don't pause before the first character reveal.
                if (visibleCount > 0)
                    yield return new WaitForSeconds(charTypeTime);
            }
            
            visibleCount++;
        }

        // Reveal the last character.
        textUI.maxVisibleCharacters = visibleCount;

        // Stop SFX
        if (silenceOverride)
            Script_SFXManager.SFX.StopDialogueTyping();

        if (cb != null)
            cb();
    }

    public static string ZalgofyString(
        string sentence,
        TextMeshProUGUI textUI
    )
    {
        var stringBuilder = new StringBuilder(sentence);
        
        // Note: should this actually be textUI.textInfo.characterCount + 1?
        int totalVisible = textUI.textInfo.characterCount;
        char zalgoLetter;

        // Replace all letters with Zalgo text
        for (var i = 0; i < totalVisible; i++)
        {
            TMP_CharacterInfo charInfo = textUI.textInfo.characterInfo[i];
            
            // Replace letters/digits with Zalgo letters.
            if (Char.IsLetterOrDigit(charInfo.character))
            {
                zalgoLetter = charInfo.character.Zalgofy();
                stringBuilder.Remove(charInfo.index, 1);
                stringBuilder.Insert(charInfo.index, Char.ToString(zalgoLetter));
            }
        }

        return stringBuilder.ToString();
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

        if (currentNode is Script_DialogueNode_PaintingEntrance)
        {
            // 1 child means it is a prompt before the final painting entrance question
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
            && currentNode.data.inputMode != InputMode.None
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
        foreach (TextMeshProUGUI t in CanvasChoice1RowDialogueTexts)
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
        foreach (TextMeshProUGUI t in ItemDescriptionCanvasTextsBottom)
        {
            t.text = "";
        }
    }

    public void StartInputMode(InputMode inputMode)
    {
        isInputMode = true;

        // Set input canvas active
        inputManager.Initialize(inputMode, null, null);
        inputManager.gameObject.SetActive(true);
    }

    public void EndInputMode(int childIdx)
    {
        isInputMode = false;

        inputManager.gameObject.SetActive(false);
        inputManager.End();
        
        Dev_Logger.Debug("childIdx: " + childIdx);

        if (CheckNodeChildren() && childIdx < currentNode.data.children.Length)
        {
            NextDialogueNode(childIdx);
        }
        else
        {
            Dev_Logger.Debug("EndInputMode > HandleEndDialogue");
            
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
            Dev_Logger.Debug("OnEndDialogueSections > HandleEndDialogue");
            
            // HandDialogueNodeAction() handled later
            HandleEndDialogue();
            return true;
        }
    }

    /// <summary>
    /// ACTUALLY ENDING dialogue (e.g. no more nodes)
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
        var canvasFadeTime = currentNode.data.isCustomDialogueFadeOut ? 
            currentNode.data.dialogueFadeOutSpeed.ToFadeTime()
            : dialogueFadeTime;
        
        if (
            fullArtManager.activeFullArt != null
            && isFullArtControlledByMe
            && !currentNode.data.isLeaveFullArtUp
        )
        {
            Dev_Logger.Debug($"{name} FullArtEndDialogue() isFullArtControlledByMe: {isFullArtControlledByMe} fullArtManager.activeFullArt {fullArtManager.activeFullArt}");
            StartCoroutine(FullArtEndDialogue());
        }
        else
        {
            Dev_Logger.Debug($"{name} NoFullArtEndDialogue() isFullArtControlledByMe: {isFullArtControlledByMe} fullArtManager.activeFullArt {fullArtManager.activeFullArt}");
            StartCoroutine(NoFullArtEndDialogue());
        }

        // Waiting until the fading out to end dialogue prevents processing the End Dialogue
        // input with a new interaction (e.g. a cut scene happens when in front of a Text Object).
        IEnumerator FullArtEndDialogue()
        {
            yield return null;
            
            if (!HandleKeepDialogueUp())
            {
                ClearAllCanvasTexts();
                activeCanvas.FadeOut(canvasFadeTime, () => {
                    fullArtManager.HideFullArt(
                        fullArtManager.activeFullArt, currentNode.data.fadeOut, OnEndDialogue
                    );
                });
            }
            else
            {
                OnEndDialogue();
            }
        }
        
        IEnumerator NoFullArtEndDialogue()
        {
            // Avoid processing a new interaction with the same Input event.
            yield return null;
            
            if (!HandleKeepDialogueUp())
            {
                ClearAllCanvasTexts();
                activeCanvas.FadeOut(canvasFadeTime, OnEndDialogue);
            }
            else
            {
                OnEndDialogue();
            }
        }

        void OnEndDialogue()
        {
            isInputDisabled = false;
            
            /// In picking up state, player state will be handled by player Action
            /// upon finishing the item description dialogue. Otherwise, set the player state
            /// back to Interact.
            if (currentNode.data.type != Const_DialogueTypes.Type.Item)
            {
                // Handle if coming from a State where Dialogue was called externally
                // (meaning Player was changed to Dialogue State) and Player
                // needs to return to the previous state, not Interact.
                if (
                    Script_Game.Game.GetPlayer().LastState == Const_States_Player.Puppeteer
                    || Script_Game.Game.GetPlayer().LastState == Const_States_Player.PuppeteerNull
                    || Script_Game.Game.GetPlayer().LastState == Const_States_Player.LastElevatorEffect
                )
                    Script_Game.Game.GetPlayer().SetLastState();
                // Default Player state after dialogue is interact.
                else
                    Script_Game.Game.GetPlayer().SetIsInteract();
            }

            // Handle NPC state when externally calling NPC HandleAction -> DialogueAction
            // from script instead of an interaction.
            if (IsOnEndUpdateNPCState)
            {
                var NPC = activeInteractable as Script_StaticNPC;
                if (NPC != null)
                {
                    Dev_Logger.Debug($"{activeInteractable} State {NPC.State}");
                    NPC.State = Script_StaticNPC.States.Interact;
                }

                IsOnEndUpdateNPCState = false;
            }

            // To stop NPC On Hit reaction dialogue handling
            if (IsHandlingNPCOnHit)
            {
                IsHandlingNPCOnHit = false;
            }
            
            HandleDialogueNodeAction();
            Script_DialogueEventsManager.DialogueEndEvent();

            activeInteractable?.StartDialogueCoolDown();
            activeInteractable = null;

            // Handle the first interaction with DemonNPC without Psychic Duck.
            if (shouldCantUnderstandReaction)
            {
                StartCoroutine(WaitToTryCantUnderstandDialogue(currentNode));
                shouldCantUnderstandReaction = false;
            }
        }
    }

    // ------------------------------------------------------------------
    // Next Node Action (Can't Understand Dialogue Node)
    public void OnCantUnderstandDialogueDone()
    {
        StartCoroutine(WaitNextFrameGameInteract());
        
        // Must wait for next frame or Player will interact again.
        IEnumerator WaitNextFrameGameInteract()
        {
            yield return null;
            
            Dev_Logger.Debug("OnCantUnderstandDialogueDone: Set game to Interact");
            game.ChangeStateInteract();
        }
    }
    // ------------------------------------------------------------------
    // Other Unity Events

    public void TurnOnDefaultDialogueBoxShake()
    {
        defaultDialogueImageDistorter.enabled = true;
    }

    public void TurnOffDefaultDialogueBoxShake()
    {
        defaultDialogueImageDistorter.enabled = false;
    }

    // ------------------------------------------------------------------

    // actions will be activated after "space" is pressed to move to next dialogue
    private void HandleDialogueNodeAction()
    {
        if (currentNode.data.OnNextNodeAction.CheckUnityEventAction())
        {
            currentNode.data.OnNextNodeAction.Invoke();
        }

        if (!string.IsNullOrEmpty(currentNode.data.action))
        {
            Dev_Logger.Debug("doing node action: " + currentNode.data.action);
            game.HandleDialogueNodeAction(currentNode.data.action);
        }
    }

    private bool HandleKeepDialogueUp()
    {
        // on option to keep dialogue up (for command prompts e.g. tutorials)
        if (currentNode.data.isKeepThisDialogueUp)
        {
            // Dialogue Icon needs this as well (to know to continue blinking).
            isKeepingDialogueUp = true;

            Dev_Logger.Debug($"Keep current dialogue up isKeepingDialogueUp: {isKeepingDialogueUp}");
            return true;
        }

        return false;
    }

    public void SkipTypingSentence()
    {
        Dev_Logger.Debug($"dialogueSection {dialogueSection}, dialogueSection == null: {dialogueSection == null}");
        
        if (dialogueSection == null || dialogueSection.isUnskippable)
            return;
        
        Dev_Logger.Debug("Skipping typing sentence");

        // replace all dialogue portions
        if (isRenderingDialogueSection)
        {
            StopCoroutine(coroutine);

            for (int i = 0; i < dialogueSection.lines.Length; i++)
            {
                activeCanvasText = activeCanvasTexts[i];
                
                // If the current dialogue is glitch text, no need to interpolate,
                // every taglike element will have been zalgofied.
                if (!isZalgofyNode)
                {
                    // Interpolate dynamic strings.
                    string unformattedLine = dialogueSection.lines[i];
                    string _formattedLine = Script_Utils.FormatString(
                        unformattedLine,
                        isFormatInventoryKey: currentNode.data.isFormatInventoryKey,
                        isFormatSpeedKey: currentNode.data.isFormatSpeedKey,
                        isFormatMaskCommandKey: currentNode.data.isFormatMaskCommandKey
                    );
                    
                    // Remove pause indicators
                    _formattedLine = _formattedLine.Replace("|", string.Empty);

                    activeCanvasText.text = _formattedLine;
                }
                
                // Reset TMP visibility.
                activeCanvasText.maxVisibleCharacters = activeCanvasText.textInfo.characterCount + 1;

                // Stop dialogue typing SFX.
                Script_SFXManager.SFX.StopDialogueTyping();
                
                Dev_Logger.Debug($"SkipTypingSentence() activeCanvasText.text {activeCanvasText.text}");
                Dev_Logger.Debug($"SkipTypingSentence() activeCanvasText.maxVisibleCharacters {activeCanvasText.maxVisibleCharacters} activeCanvasText.textInfo.characterCount {activeCanvasText.textInfo.characterCount}");
            }

            lines.Clear();
            FinishRenderingDialogueSection();
        }
    }

    private void HideDialogue()
    {
        // Hide at end of dialogue so we don't see flicker when we change types
        if (activeCanvas != null)
            activeCanvas.Close();

        canvas.gameObject.SetActive(false);
        canvas.alpha = 0f;
        canvas.blocksRaycasts = false;
    }

    private void ShowDialogue()
    {
        if (activeCanvas != null)
            activeCanvas.Open();
        
        canvas.gameObject.SetActive(true);
        canvas.alpha = 1f;
        canvas.blocksRaycasts = true;
    }

    private void SetupCanvases(Model_Dialogue dialogue, string type)
    {
        /// Allow option item descriptions to not override text
        if (
            (
                currentNode.data.type == Const_DialogueTypes.Type.Item
                || currentNode.data.type == Const_DialogueTypes.Type.ItemNoPickUp
            )
            && currentNode.data.isKeepLastDialogueUp
        )
        {
            Dev_Logger.Debug("SetupCanvases(): not clearing all canvases, only disabling non-written ones");
            canvasHandler.DisableOnlyEmptyCanvases();
        }
        else
        {
            Dev_Logger.Debug("SetupCanvases(): clearing all canvases now");
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
                activeCanvas = DefaultReadTextCanvas;
                activeCanvasTexts = DefaultReadTextCanvasTexts;
                DefaultReadTextCanvas.gameObject.SetActive(true);
            }
        }
        else if (
            type == Const_DialogueTypes.Type.Item
            || type == Const_DialogueTypes.Type.ItemNoPickUp
        )
        {
            activeCanvas = ItemDescriptionCanvasBottom;
            activeCanvasTexts = ItemDescriptionCanvasTextsBottom;
            ItemDescriptionCanvasBottom.gameObject.SetActive(true);

            // If other continuation icons are active, disable them
            canvasHandler.DisableInactiveContinuationIcons();
        }
        else if (
            currentNode is Script_DialogueNode_PaintingEntrance
            || currentNode.data.type == Const_DialogueTypes.Type.PaintingEntrance
        )
        {
            activeCanvas = PaintingEntranceChoiceCanvas;
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
            SetDialogueCanvasToCanvasChoice1Row();
        }
        else
        {
            activeCanvas = DefaultCanvas;
            DefaultCanvas.gameObject.SetActive(true);
            activeCanvasTexts = DefaultCanvasDialogueTexts;
            nameText = DefaultCanvasName;
        }
        
        foreach (var c in activeCanvasTexts)
            c.gameObject.SetActive(true);
        
        activeCanvas.canvasChild.Setup();
        SetupName(dialogue.name);
    }

    private void SetupName(string name)
    {
        Dev_Logger.Debug("Attempting to set up name now");

        if (nameText == null)
            return;

        nameText.text = Script_Utils.FormatString(name).FormatName();

        if (Debug.isDebugBuild && Const_Dev.IsDevMode && (name == "" || name == null))
        {
            Debug.LogWarning("No name was provided for dialogue");
        }
    }

    private void SetDialogueCanvasToCanvasChoice1Row()
    {
        activeCanvas = CanvasChoice1Row;
        CanvasChoice1Row.gameObject.SetActive(true);
        
        activeCanvasTexts = CanvasChoice1RowDialogueTexts;
        nameText = CanvasChoice1RowName;
        nameText.text = Script_Utils.FormatString(currentNode.data.dialogue.name) + ":";
    }

    private void SetDialogueCanvasToReadChoice()
    {
        activeCanvas = ReadChoiceCanvasBottom;
        ReadChoiceCanvasBottom.gameObject.SetActive(true);
        
        activeCanvasTexts = ReadChoiceDialogueTexts;
    }

    public void HandleFirstPsychicInteractionNoDuck()
    {
        // R2 Elder Dialogue Freeze Bug: Don't do this on Act 2, player will have already become accustomed
        // to using Psychic Duck anyways
        if (game.RunCycle != Script_RunsManager.Cycle.Weekday)
            return;
        
        if (!didCantUnderstandReactionDone)
            shouldCantUnderstandReaction = true;
    }

    public void InitialState()
    {
        HideDialogue();
        ClearAllCanvasTexts();
        canvasHandler.Setup();
    }

    public void Initialize()
    {
        Dev_Logger.Debug($"{name} INITIALIZED");
        
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
        
        paintingEntranceManager.Setup();
        
        fullArtManager.Setup();

        InitialState();
    }
}
