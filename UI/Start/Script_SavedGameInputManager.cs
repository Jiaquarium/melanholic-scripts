using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SavedGameInputManager : Script_ExitViewInputManager
{
    void OnEnable()
    {
        Script_StartEventsManager.OnExitFileActions += ExitFileActionsMode;
    }

    void OnDisable()
    {
        Script_StartEventsManager.OnExitFileActions -= ExitFileActionsMode;
    }

    public override void HandleExitInput()
    {
        if (Script_PlayerInputManager.Instance.MyPlayerInput.actions[Const_KeyCodes.UICancel].WasPressedThisFrame())
        {
            Debug.Log($"SavedGameInputManager: Cancel called, firing ExitFileActionsMode event");
            Script_StartEventsManager.ExitFileActions();
        }
    }

    private void ExitFileActionsMode()
    {
        if (Script_Start.Main.mainController.State != SavedGameState.Start)
            Script_Start.Main.mainController.State = SavedGameState.Start;
    }
}
