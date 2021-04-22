using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_InventoryInputManager : Script_InventoryViewInputManager
{
    protected override void ExitView()
    {
        inventoryController.ExitInventoryView();
        base.ExitView();
    }
}
