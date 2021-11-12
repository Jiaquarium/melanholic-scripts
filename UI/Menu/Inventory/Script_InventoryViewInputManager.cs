using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_InventoryViewInputManager : Script_ExitViewInputManager
{
    [SerializeField] protected Script_EventSystemLastSelected eventSystemLastSelected;
    protected Script_InventoryController inventoryController;

    protected virtual void OnEnable()
    {
        Script_MenuEventsManager.OnExitSubmenu += ExitView;    
    }

    protected virtual void OnDisable()
    {
        Script_MenuEventsManager.OnExitSubmenu -= ExitView;
    }
    
    public override void HandleExitInput()
    {
        if (masterUIState != null && masterUIState.state == UIState.Disabled)
            return;

        if (Input.GetButtonDown(Const_KeyCodes.Inventory) || Input.GetButtonDown(Const_KeyCodes.Cancel))
        {
            print("HandleExitInput()");
            Script_MenuEventsManager.ExitSubmenu();
        }
    }

    protected virtual void ExitView()
    {
        eventSystemLastSelected.UpdateCurrentSelected();   
    }

    public void Setup(
        Script_InventoryController _inventoryController
    )
    {
        inventoryController = _inventoryController;
    }
}
