using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SavedGameSubmenuInputManager : Script_ExitViewInputManager
{
    [SerializeField] Script_StartOverviewController mainController; 
    
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
        
        if (Script_PlayerInputManager.Instance.MyPlayerInput.actions[Const_KeyCodes.UICancel].WasPressedThisFrame())
        {
            Debug.Log($"SavedGameSubmenuInputManager: Cancel called, firing exitSubmenu event");
            Script_StartEventsManager.ExitSubmenu();
        }
    }

    private void CloseSubmenu()
    {
        mainController.EnterSavedGamesSelectView();
        Script_SFXManager.SFX.PlayExitSubmenuPencil();
    }   
}
