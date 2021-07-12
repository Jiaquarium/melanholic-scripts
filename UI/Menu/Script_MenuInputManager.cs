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
            Debug.Log($"{name} Cannot exit; masterUI.state: {masterUIState.state}");
            return;
        }

        if (Input.GetButtonDown("Inventory") || Input.GetButtonDown("Cancel"))
        {
            Debug.Log("{name} Exit menu input detected");
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
