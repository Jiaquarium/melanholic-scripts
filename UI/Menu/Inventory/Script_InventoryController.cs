using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Base for controllers needing inventory slots (e.g. SBook Controller, Items Controller)
/// </summary>
public abstract class Script_InventoryController : Script_CanvasGroupController
{
    public abstract void EnterInventoryView();
    
    public abstract void ExitInventoryView();
}
