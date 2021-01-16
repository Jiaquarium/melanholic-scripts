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
    public Directions facingDirection;
    protected Script_DialogueManager dialogueManager;
    [SerializeField] protected bool isMute;
    private Coroutine fadeOutCo;
    [Tooltip("Easier way to reference Game if we don't care about Setup()")] [SerializeField] private bool isAutoSetup;
    [SerializeField] private States _state;

    public States State
    {
        get { return _state; }
        protected set { _state = value; }
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

    protected virtual void Start()
    {
        if (isAutoSetup)  AutoSetup();
    }
    
    // Update is called once per frame
    void Update()
    {
    }

    public void HandleAction(string action)
    {
        if (action == Const_KeyCodes.Action1)
        {
            Debug.Log($"CHECKING DISABLED DIRECTIONS: {CheckDisabledDirections()}");
            
            if (isDialogueCoolDown || CheckDisabledDirections())
            {
                Debug.Log($"Cannot interact with {name}, either still dialogue cool down or disabled direction");
                return;
            }
            
            if (!game.GetPlayerIsTalking())     TriggerDialogue();
            else
            {
                if (dialogueManager.IsDialogueSkippable())  dialogueManager.SkipTypingSentence();
                else                                        ContinueDialogue();
            }
        }    
    }
    
    public virtual void TriggerDialogue()
    {
        if (isMute)                 return;
        
        if (dialogueNodes.Length > 0)
        {
            State = States.Dialogue;
            
            dialogueManager.StartDialogueNode(
                dialogueNodes[dialogueIndex],
                SFXOn: true,
                null,
                this
            );
            HandleDialogueNodeIndex();
        }
    }

    protected void HandleDialogueNodeIndex()
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
        
        if (didContinue == false)   State = States.Interact;
        
        return didContinue;
    }

    public void SkipTypingSentence()
    {
        dialogueManager.SkipTypingSentence();
    }

    public virtual void Move() {}
    public virtual void Glimmer() {}
    public virtual void Freeze(bool isFrozen) {}

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
