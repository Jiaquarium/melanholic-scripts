using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles dialogue actions when not directly interfacing with Interactable
/// </summary>
public class Script_DialogueInputManager : MonoBehaviour
{
    [SerializeField] private Script_Game game;
    [SerializeField] private Script_DialogueManager dialogueManager;
    
    void Update()
    {
        if (game.state == Const_States_Game.CutScene)
            HandleCutSceneDialogueAction();
        else if (dialogueManager != null && dialogueManager.IsHandlingNPCOnHit)
            HandleNPCOnHitReactionDialogueAction();
    }
    
    protected virtual void HandleCutSceneDialogueAction()
    {
        if (Script_PlayerInputManager.Instance.RewiredInput.GetButtonDown(Const_KeyCodes.RWInteract))
        {
            Dev_Logger.Debug("HandleCutSceneDialogueAction()");
            game.HandleContinuingDialogueActions(Const_KeyCodes.InteractAction);
        }
    }

    private void HandleNPCOnHitReactionDialogueAction()
    {
        if (Script_PlayerInputManager.Instance.RewiredInput.GetButtonDown(Const_KeyCodes.RWInteract))
        {
            Dev_Logger.Debug("HandleNPCOnHitReactionDialogueAction()");
            game.HandleContinuingDialogueActions(Const_KeyCodes.InteractAction);
        }
    }
}
