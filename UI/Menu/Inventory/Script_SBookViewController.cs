using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Script_InventoryViewInputManager))]
/// <summary>
/// Tracks the last slot for Stickers
/// To do so, only have this controller active when selecting across slots
/// </summary>
public class Script_SBookViewController : Script_SlotsViewController
{
    // the main view controller
    public Script_InventoryController sBookController;
    [Space]
    protected Script_InventoryViewInputManager sBookInputManager;

    protected override void HandleExitInput() {
        sBookInputManager.HandleExitInput();
    }
    
    public override void Setup()
    {
        UpdateSlots();

        sBookInputManager = GetComponent<Script_InventoryViewInputManager>();
        sBookInputManager.Setup(sBookController);
    }
}
