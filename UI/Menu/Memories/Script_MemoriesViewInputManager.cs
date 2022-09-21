using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_MemoriesViewInputManager : Script_ExitViewInputManager
{
    [SerializeField] protected Script_EventSystemLastSelected eventSystemLastSelected;
    [SerializeField] private Script_MemoriesViewController memoriesViewController;
    
    // protected virtual void OnEnable()
    // {
    //     Script_MenuEventsManager.OnExitSubmenu += ExitView;    
    // }

    // protected virtual void OnDisable()
    // {
    //     Script_MenuEventsManager.OnExitSubmenu -= ExitView;
    // }
    
    // public override void HandleExitInput()
    // {
    //     if (masterUIState != null && masterUIState.state == UIState.Disabled)
    //         return;

    //     if (Input.GetButtonDown(Const_KeyCodes.Inventory) || Input.GetButtonDown(Const_KeyCodes.Cancel))
    //     {
    //         Dev_Logger.Debug("HandleExitInput()");
    //         Script_MenuEventsManager.ExitSubmenu();
    //     }
    // }
    
    // protected virtual void ExitView()
    // {
    //     memoriesViewController.ExitEntriesView();

    //     eventSystemLastSelected.UpdateCurrentSelected();
    // }
}
