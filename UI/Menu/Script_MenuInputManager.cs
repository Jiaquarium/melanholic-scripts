using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_MenuInputManager : Script_ExitViewInputManager
{
    public Script_Game game;
    public Script_EventSystemLastSelected es;

    private void OnEnable()
    {
        Script_MenuEventsManager.OnExitMenu += CloseInventory;    
    }

    private void OnDisable()
    {
        Script_MenuEventsManager.OnExitMenu -= CloseInventory;
    }
    
    public override void HandleExitInput()
    {
        if (masterUIState != null && masterUIState.state == UIState.Disabled)
        {
            Dev_Logger.Debug($"{name} Cannot exit; masterUI.state: {masterUIState.state}");
            return;
        }

        var playerInput = game.GetPlayer().MyPlayerInput;
        
        if (
            playerInput.actions[Const_KeyCodes.Inventory].WasPressedThisFrame()
            || playerInput.actions[Const_KeyCodes.UICancel].WasPressedThisFrame()
        )
        {
            Dev_Logger.Debug("{name} Exit menu input detected");
            Script_MenuEventsManager.ExitMenu();
        }
    }

    private void CloseInventory()
    {
        game.CloseInventory();
        es.lastSelected = null;
    }

    public void Setup()
    {
        game = FindObjectOfType<Script_Game>();
    }
}
