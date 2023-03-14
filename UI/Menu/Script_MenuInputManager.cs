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

        var rewiredInput = Script_PlayerInputManager.Instance.RewiredInput;
        
        if (
            rewiredInput.GetButtonDown(Const_KeyCodes.RWInventory)
            || rewiredInput.GetButtonDown(Const_KeyCodes.RWUICancel)
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
        game = Script_Game.Game;
    }
}
