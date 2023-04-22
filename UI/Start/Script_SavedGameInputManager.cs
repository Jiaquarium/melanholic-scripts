using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SavedGameInputManager : Script_ExitViewInputManager
{
    private const int InputFrameBuffer = 10;
    
    [SerializeField] private Script_SavedGameViewController savedGameViewController;
    [SerializeField] private Script_StartOverviewController startOverviewController;
    
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
        if (Script_PlayerInputManager.Instance.RewiredInput.GetButtonDown(Const_KeyCodes.RWUICancel))
        {
            // Check if its meant for Back navigation from Saved Games overview
            if (
                masterUIState.state == UIState.Interact
                && savedGameViewController.menuState == Script_SavedGameViewController.MenuStates.Overview
                && startOverviewController.State == SavedGameState.Start
            )
            {
                // Give some delay before processing this exit input to avoid accidentally nav'ing back
                if (Time.frameCount - savedGameViewController.LastExitInputFrameCount <= InputFrameBuffer)
                    return;
                
                savedGameViewController.LastExitInputFrameCount = Time.frameCount;
                startOverviewController.ToStartScreenNonIntro();
                return;
            }
            
            Dev_Logger.Debug($"SavedGameInputManager: Cancel called, firing ExitFileActionsMode event");
            savedGameViewController.LastExitInputFrameCount = Time.frameCount;
            Script_StartEventsManager.ExitFileActions();
        }
    }

    private void ExitFileActionsMode()
    {
        if (Script_Start.Main.mainController.State != SavedGameState.Start)
            Script_Start.Main.mainController.State = SavedGameState.Start;
    }
}
