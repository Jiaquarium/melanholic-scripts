using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Script_StaticNPC : Script_Interactable
{
    public enum States
    {
        Interact,
        Dialogue
    }
    
    public int StaticNPCId;
    public Script_Game game {get; set;}
    [SerializeField] protected Script_DialogueNode[] dialogueNodes;
    [SerializeField] protected Transform rendererChild;
    [SerializeField] protected Directions defaultFacingDirection;

    [SerializeField] protected int dialogueIndex;
    protected Directions facingDirection;
    protected Script_DialogueManager dialogueManager;
    [SerializeField] protected bool isMute;
    private Coroutine fadeOutCo;
    [Tooltip("Easier way to reference Game if we don't care about Setup()")]
    [SerializeField] protected bool isAutoSetup;
    [SerializeField] private States _state;

    public States State
    {
        get { return _state; }
        set { _state = value; }
    }

    public bool IsMute
    {
        get => isMute;
        set => isMute = value;
    }

    public Directions DefaultFacingDirection
    {
        get => defaultFacingDirection;
        set => defaultFacingDirection = value;
    }

    public Directions FacingDirection { get => facingDirection; }

    protected virtual void OnEnable()
    {
        InitialState();
        
        if (isAutoSetup)
            AutoSetup();
    }
    
    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// Should only be called by Player Action.
    /// </summary>
    public void HandleAction(string action)
    {
        if (action == Const_KeyCodes.InteractAction)
        {
            Dev_Logger.Debug($"CHECKING DISABLED DIRECTIONS: {CheckDisabledDirections()}");
            
            if (isDialogueCoolDown || CheckDisabledDirections() || dialogueManager.isInputDisabled)
            {
                Dev_Logger.Debug($"No interact {name}, isDialogueCoolDown {isDialogueCoolDown}, isInputDisabled {dialogueManager.isInputDisabled} or cooldown");
                return;
            }
            
            if (!game.GetPlayerIsTalking())
                TriggerDialogue();
            else
            {
                if (dialogueManager.IsDialogueSkippable())  dialogueManager.SkipTypingSentence();
                else                                        ContinueDialogue();
            }
        }    
    }

    /// <summary>
    /// Call externally from other than Player Action.
    /// For dialogue, ignores dialogue conditions like cooldowns, disabled directions, etc.
    /// </summary>
    public void ForceHandleAction(string action)
    {
        if (action == Const_KeyCodes.InteractAction)
        {
            dialogueManager.IsOnEndUpdateNPCState = true;
            
            if (!game.GetPlayerIsTalking())     TriggerDialogue();
            else
            {
                if (dialogueManager.IsDialogueSkippable())  dialogueManager.SkipTypingSentence();
                else                                        ContinueDialogue();
            }
        }
    }
    
    protected virtual void TriggerDialogue()
    {
        if (isMute)                 return;

        dialogueNodes = dialogueNodes.FilterNulls();

        if (dialogueNodes.Length > 0)
        {
            State = States.Dialogue;
            
            Dev_Logger.Debug($"{name} Dialogue idx: {dialogueIndex}");

            dialogueManager.StartDialogueNode(
                dialogueNodes[dialogueIndex],
                SFXOn: true,
                null,
                this
            );
            
            Dev_Logger.Debug($"TriggerDialogue, incrementing dialogue index from {dialogueIndex} to {dialogueIndex + 1}");
            HandleIncrementDialogueNodeIndex();
        }
    }

    protected void HandleIncrementDialogueNodeIndex()
    {
        if (dialogueIndex == dialogueNodes.Length - 1)  dialogueIndex = 0;    
        else                                            dialogueIndex++;
    }

    public void SwitchDialogueNodes(Script_DialogueNode[] nodes, bool isReset = true)
    {
        if (isReset)   dialogueIndex = 0; // reset index
        dialogueNodes = nodes; // switch out nodes
    }

    /// <summary>
    /// Prevents from TriggeringDialogue
    /// </summary>
    public void SetMute(bool _isMute)
    {
        isMute = _isMute;
    }

    public void SetVisibility(bool isVisible)
    {
        rendererChild.GetComponent<SpriteRenderer>().enabled = isVisible;
    }

    Action OnFadeOut()
    {
        return new Action(() => {
            this.gameObject.SetActive(false);
        });
    }

    public virtual void FadeOut(Action cb)
    {
        // if no callback passed in, uses default OnFadeOut()
        fadeOutCo = StartCoroutine(
            rendererChild.GetComponent<Script_SpriteFadeOut>().FadeOutCo(
                cb == null ? OnFadeOut() : cb
            )
        );
    }

    public virtual bool? ContinueDialogue()
    {
        bool? didContinue = dialogueManager.ContinueDialogue();
        
        if (didContinue == false)
            State = States.Interact;
        
        return didContinue;
    }

    public void SkipTypingSentence()
    {
        dialogueManager.SkipTypingSentence();
    }

    public virtual void Move() {}
    public virtual void Glimmer() {}
    public virtual void Freeze(bool isFrozen) {}

    protected virtual void InitialState()
    {
        dialogueIndex = 0;
    }

    protected virtual void AutoSetup()
    {
        game = Script_Game.Game;
    }

    public virtual void Setup()
    {
        game = Script_Game.Game;
        dialogueManager = game.dialogueManager;
    }
}
