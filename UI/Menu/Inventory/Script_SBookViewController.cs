using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Script_SBookViewInputManager))]
/// <summary>
/// Tracks the last slot
/// To do so, only have this controller active when selecting across slots
/// </summary>
public class Script_SBookViewController : Script_SlotsViewController
{
    /// <summary>
    /// the main view controller
    /// </summary>
    public Script_SBookOverviewController sBookController;
    [Space]
    protected Script_SBookViewInputManager sBookInputManager;

    protected override void HandleExitInput() {
        sBookInputManager.HandleExitInput();
    }
    
    public override void Setup()
    {
        UpdateSlots();

        sBookInputManager = GetComponent<Script_SBookViewInputManager>();
        sBookInputManager.Setup(sBookController);
    }
}
