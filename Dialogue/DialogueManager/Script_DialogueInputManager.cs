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
            game.GetPlayer().MyPlayerInput.actions[Const_KeyCodes.Interact].WasPressedThisFrame()
            && game.state == Const_States_Game.CutScene
        )
        {
            Debug.Log("HandleCutSceneDialogueAction()");
            game.HandleContinuingDialogueActions(Const_KeyCodes.Interact);
        }
    }
}
