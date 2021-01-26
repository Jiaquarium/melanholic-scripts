using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Ordering:
/// 
/// 1. Intro() from prompt -> yes node NextNodeAction
/// 2. StartDialogue() from Timeline End Signal
/// 3. End() from Dialogue End NextNodeAction
/// </summary>
public class Script_MynesMirror : Script_InteractableObjectText
{
    [Tooltip("Specifies which mirror to save as")]
    public int MynesMirrorId;
    [SerializeField] private Script_DialogueNode _MynesConversationNode;
    
    private Script_DialogueNode MynesConversationNode
    {
        get => _MynesConversationNode;
        set => _MynesConversationNode = value;
    }

    void OnValidate()
    {
        if (Id != MynesMirrorId)
        {
            Debug.LogError($"This Mynes Mirror {name} Id and MynesMirrorId were not matching");
        }
        Id = MynesMirrorId;
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        
        Script_MynesMirrorEventsManager.OnEndTimeline += StartDialogue;
    }

    void OnDisable()
    {
        Script_MynesMirrorEventsManager.OnEndTimeline -= StartDialogue;
    }

    protected override bool CheckIsDisabled()
    {
        bool isActivated = Script_ScarletCipherManager.Control.MynesMirrorsActivationStates[MynesMirrorId];
        return isActivated || base.CheckIsDisabled();
    }

    /// <summary>
    /// The relevant choice node calls this to check its Id upon selecting
    /// </summary>
    public bool CheckCipher(int choiceIdx)
    {
        return (Script_ScarletCipherManager.Control.HandleCipherSlot(MynesMirrorId, choiceIdx));
    }

    // ------------------------------------------------------------------
    // Signal Reactions START
    /// <summary>
    /// Begin Myne's dialogue, the end of Timeline calls MynesMirrorManager to fire this event
    /// </summary>
    public void StartDialogue()
    {
        Script_DialogueManager.DialogueManager.StartDialogueNode(MynesConversationNode);
    }
    // Signal Reactions END
    // ------------------------------------------------------------------
    
    // ------------------------------------------------------------------
    // Next Node Actions START
    /// <summary>
    /// Shows Myne's dramatic entrance
    /// </summary>
    public void Intro()
    {
        game.ChangeStateCutScene();
        Script_PRCSManager.Control.OpenPRCSCustom(Script_PRCSManager.CustomTypes.MynesMirror);
    }
    
    /// <summary>
    /// Remove cut scene
    /// Called from last Dialogue Node
    /// </summary>
    public void End()
    {
        Script_PRCSManager.Control.ClosePRCSCustom(Script_PRCSManager.CustomTypes.MynesMirror, () => {
            game.ChangeStateInteract();
            Script_ScarletCipherManager.Control.MynesMirrorsActivationStates[MynesMirrorId] = true;
        });
    } 
    // Next Node Actions END
    // ------------------------------------------------------------------
}
