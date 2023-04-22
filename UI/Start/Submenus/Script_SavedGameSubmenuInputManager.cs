using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will only be active when a File Action Submenu is active
/// </summary>
public class Script_SavedGameSubmenuInputManager : Script_ExitViewInputManager
{
    [SerializeField] private Script_StartOverviewController mainController; 
    
    void OnEnable()
    {
        Script_StartEventsManager.OnExitSubmenu += CloseSubmenu;
    }

    void OnDisable()
    {
        Script_StartEventsManager.OnExitSubmenu -= CloseSubmenu;
    }
    
    public override void HandleExitInput()
    {
        if (masterUIState != null && masterUIState.state == UIState.Disabled)
            return;
        
        if (Script_PlayerInputManager.Instance.RewiredInput.GetButtonDown(Const_KeyCodes.RWUICancel))
        {
            Dev_Logger.Debug($"SavedGameSubmenuInputManager: Cancel called, firing exitSubmenu event");
            mainController.OnFileActionsSubmenuShortcutExit();
            Script_StartEventsManager.ExitSubmenu();
        }
    }

    private void CloseSubmenu()
    {
        mainController.EnterSavedGamesSelectView();
        Script_SFXManager.SFX.PlayExitSubmenuPencil();
    }   
}
