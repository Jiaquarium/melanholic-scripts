using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_InteractableObjectText : Script_InteractableObject
{
    public Script_DialogueNode[] dialogueNodes;
    public bool isBlocking;
    
    [SerializeField] protected int dialogueIndex;
    [SerializeField] private bool disableLeft;
    [SerializeField] private bool disableUp;
    [SerializeField] private bool disableRight;
    [SerializeField] private bool disableDown;
    [SerializeField] private Script_LightSwitch myLightSwitch;
    [SerializeField] private bool allowNonreadDialogueNodes;
    protected Script_DialogueManager dialogueManager;
        

    protected override void AutoSetup()
    {
        base.AutoSetup();
        dialogueManager = Script_DialogueManager.DialogueManager;
    }
    
    public void SetupDialogueNodeText(
        Script_DialogueManager _dialogueManager,
        Script_Player _player
    )
    {
        dialogueManager = _dialogueManager;
    }

    public override void SwitchDialogueNodes(
        Script_DialogueNode[] _dialogueNodes
    )
    {
        dialogueNodes = _dialogueNodes;
    }

    public override void ActionDefault()
    {
        if (isDialogueCoolDown)     return;

        if (CheckDisabledDirections())  return;
        if (myLightSwitch != null && !myLightSwitch.isOn)   return;
        
        /// Initiate dialogue node
        if (Script_Game.Game.GetPlayer().State != Const_States_Player.Dialogue)
        {
            if (dialogueNodes == null || dialogueNodes.Length == 0)
            {
                Debug.LogWarning("No dialogue nodes provided for text object");
                return;
            }
            
            dialogueManager.StartDialogueNode(
                dialogueNodes[dialogueIndex],
                SFXOn: true,
                type: allowNonreadDialogueNodes ? null : Const_DialogueTypes.Type.Read,
                this
            );
            HandleDialogueNodeIndex();
        }
        /// Player is mid-dialogue, can either 1) skip 2) continue if no longer rendering dialogue
        else
        {
            if (dialogueManager.IsDialogueSkippable())
            {
                Debug.Log("Attempting to skip typing sentence via IOText");
                dialogueManager.SkipTypingSentence();
            }
            else
            {
                dialogueManager.ContinueDialogue();
            }
        }
    }

    protected bool CheckDisabledDirections()
    {
        Directions directionToPlayer = Script_Utils.GetDirectionToTarget(
                                            transform.position,
                                            Script_Game.Game.GetPlayer().transform.position
                                        );
        if (
            directionToPlayer == Directions.Left && disableLeft
            || directionToPlayer == Directions.Up && disableUp
            || directionToPlayer == Directions.Right && disableRight
            || directionToPlayer == Directions.Down && disableDown
        )
        {
            return true;
        }

        return false;
    }

    void HandleDialogueNodeIndex()
    {
        if (dialogueIndex == dialogueNodes.Length - 1)
        {
            dialogueIndex = 0;    
        }
        else
        {
            dialogueIndex++;
        }
    }
}
