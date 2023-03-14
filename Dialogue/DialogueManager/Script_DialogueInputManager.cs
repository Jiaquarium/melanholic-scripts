using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_DialogueInputManager : MonoBehaviour
{
    [SerializeField] private Script_Game game;
    
    void Update()
    {
        HandleCutSceneDialogueAction();
    }
    
    protected virtual void HandleCutSceneDialogueAction()
    {
        if (
            Script_PlayerInputManager.Instance.RewiredInput.GetButtonDown(Const_KeyCodes.RWInteract)
            && game.state == Const_States_Game.CutScene
        )
        {
            Dev_Logger.Debug("HandleCutSceneDialogueAction()");
            game.HandleContinuingDialogueActions(Const_KeyCodes.InteractAction);
        }
    }
}
