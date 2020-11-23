using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Script_StaticNPC : Script_Interactable
{
    public int StaticNPCId;
    public Script_Game game {get; set;}
    [SerializeField] protected Script_DialogueNode[] dialogueNodes;
    [SerializeField] protected Transform rendererChild;
    [SerializeField] protected Directions defaultFacingDirection;

    [SerializeField] protected int dialogueIndex;
    protected Script_DialogueManager dialogueManager;
    [SerializeField] protected bool isMute;
    private Coroutine fadeOutCo;

    // Update is called once per frame
    void Update()
    {
        AdjustRotation();
    }

    public void HandleAction(string action)
    {
        if (action == Const_KeyCodes.Action1)
        {
            if (isDialogueCoolDown)     return;
            
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
            Destroy(this.gameObject);
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

    public virtual void ContinueDialogue()
    {
        dialogueManager.ContinueDialogue();
    }

    public void SkipTypingSentence()
    {
        dialogueManager.SkipTypingSentence();
    }

    public void AdjustRotation()
    {
        rendererChild.transform.forward = Camera.main.transform.forward;
    }

    public virtual void Move() {}
    public virtual void Glimmer() {}
    public virtual void Freeze(bool isFrozen) {}

    public virtual void Setup()
    {
        game = Script_Game.Game;
        dialogueManager = game.dialogueManager;

        AdjustRotation();
    }
}
