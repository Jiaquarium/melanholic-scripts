﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Script_InteractableObjectText : Script_InteractableObject
{
    public Script_DialogueNode[] dialogueNodes;
    public bool isBlocking;
    
    [SerializeField] protected int dialogueIndex;
    [SerializeField] private Script_LightSwitch myLightSwitch;
    [SerializeField] private bool allowNonreadDialogueNodes;
    [SerializeField] protected Script_DialogueManager dialogueManager;
    [SerializeField] private UnityEvent _preTextAction;

    private UnityEvent PreTextAction
    {
        get => _preTextAction;
    }

    protected override void OnEnable()
    {
        
        
        base.OnEnable();
    }

    protected override void Awake()
    {
        base.Awake();
    }
    
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
        Script_DialogueNode[] _dialogueNodes,
        bool isReset = true
    )
    {
        if (isReset)   dialogueIndex = 0;
        dialogueNodes = _dialogueNodes;
    }

    public void ForceAction() => ActionDefault();
    
    protected override void ActionDefault()
    {
        if (CheckDisabled())
        {
            Dev_Logger.Debug($"{name} is disabled");
            return;
        }
        
        /// Initiate dialogue node
        if (Script_Game.Game.GetPlayer().State != Const_States_Player.Dialogue)
        {
            InvokePreAction();
            
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
            
            // Invoke binded Events only upon initially prompting the text object.
            MyAction.SafeInvoke();
            HandleDialogueNodeIndex();
        }
        /// Player is mid-dialogue, can either 1) skip 2) continue if no longer rendering dialogue
        else
        {
            ContinueDialogue();
        }
    }

    protected void ContinueDialogue()
    {
        if (dialogueManager.IsDialogueSkippable())
        {
            Dev_Logger.Debug("Attempting to skip typing sentence via IOText");
            dialogueManager.SkipTypingSentence();
        }
        else
        {
            dialogueManager.ContinueDialogue();
        }
    }

    protected override bool CheckDisabled()
    {
        return
        (
            isDialogueCoolDown
            || CheckDisabledDirections()
            || (myLightSwitch != null && !myLightSwitch.isOn)
        );
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

    // Invoke an action before starting the dialogue node.
    protected void InvokePreAction()
    {
        if (PreTextAction.CheckUnityEventAction()) PreTextAction.Invoke();
    }

    public override void InitializeState()
    {
        dialogueIndex = 0;
        
        base.InitializeState();
    }
}
