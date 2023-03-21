using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Rewired;

[RequireComponent(typeof(Script_InventoryViewInputManager))]
/// <summary>
/// Tracks the last slot for Stickers
/// To do so, only have this controller active when selecting across slots
/// </summary>
public class Script_SBookViewController : Script_SlotsViewController
{
    [SerializeField] private Script_MenuController menuController;
    [SerializeField] private Script_MenuHotKeyInputManager hotKeyInputManager;
    
    // the main view controller
    public Script_InventoryController sBookController;
    [Space]
    protected Script_InventoryViewInputManager sBookInputManager;

    protected override void Update()
    {
        if (HandleNavigatedOut())
            return;
        
        ShowActiveSlot();
        HandleExitInput();

        // Inventory State is set with OnClick on StickersHolder Button
        // or exited via HandleExitInput. This ensures we have state for InventoryState
        // and SlotId up-to-date before checking for Hot Keys.
        if (hotKeyInputManager != null)
        {
            Controller lastController = Script_PlayerInputManager.Instance.GetLastActiveController;
            if (lastController != null && lastController.type == ControllerType.Joystick)
                hotKeyInputManager.OnHotkeyJoystick(menuController.InventoryState, lastSlotIndex);
            else
                hotKeyInputManager.OnHotkey(menuController.InventoryState, lastSlotIndex);
        }
    }
    
    protected override void HandleExitInput() {
        sBookInputManager.HandleExitInput();
    }
    
    public override void Setup()
    {
        base.Setup();

        sBookInputManager = GetComponent<Script_InventoryViewInputManager>();
        sBookInputManager.Setup(sBookController);
    }
}
