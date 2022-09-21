using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_EquipmentInputManager : Script_InventoryViewInputManager
{
    protected override void ExitView()
    {
        Dev_Logger.Debug("ExitView caught from equipment input manager");
        
        var controller = inventoryController as Script_SBookOverviewController;
        controller?.ExitEquipmentView();

        base.ExitView();
    }
}
