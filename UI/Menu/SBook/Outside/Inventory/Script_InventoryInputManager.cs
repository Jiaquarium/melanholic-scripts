using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_InventoryInputManager : Script_SBookViewInputManager
{
    protected override void ExitView()
    {
        sBookController.ExitInventoryView();
        base.ExitView();
    }
}
