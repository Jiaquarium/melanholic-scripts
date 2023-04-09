using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_MenuHotKeyInputManager : MonoBehaviour
{
    [SerializeField] private Script_InventoryManager inventoryManager;
    
    public void OnHotkey(Script_MenuController.InventoryStates state, int slotIndex)
    {
        var rewiredInput = Script_PlayerInputManager.Instance.RewiredInput;
        
        if (rewiredInput.GetButtonDown(Const_KeyCodes.RWHotKey1))
        {
            Dev_Logger.Debug($"Hot Key 1; slotIndex: {slotIndex}; state: {state}");
            HandleHotKey(Const_KeyCodes.RWHotKey1, state, slotIndex);
        }
        else if (rewiredInput.GetButtonDown(Const_KeyCodes.RWHotKey2))
        {
            Dev_Logger.Debug($"Hot Key 2; slotIndex: {slotIndex}; state: {state}");
            HandleHotKey(Const_KeyCodes.RWHotKey2, state, slotIndex);
        }
        else if (rewiredInput.GetButtonDown(Const_KeyCodes.RWHotKey3))
        {
            Dev_Logger.Debug($"Hot Key 3; slotIndex: {slotIndex}; state: {state}");
            HandleHotKey(Const_KeyCodes.RWHotKey3, state, slotIndex);
        }
        else if (rewiredInput.GetButtonDown(Const_KeyCodes.RWHotKey4))
        {
            Dev_Logger.Debug($"Hot Key 4; slotIndex: {slotIndex}; state: {state}");
            HandleHotKey(Const_KeyCodes.RWHotKey4, state, slotIndex);
        }
        else if (rewiredInput.GetButtonDown(Const_KeyCodes.RWHotKey5))
        {
            Dev_Logger.Debug($"Hot Key 5; slotIndex: {slotIndex}; state: {state}");
            HandleHotKey(Const_KeyCodes.RWHotKey5, state, slotIndex);
        }
        else if (rewiredInput.GetButtonDown(Const_KeyCodes.RWHotKey6))
        {
            Dev_Logger.Debug($"Hot Key 6; slotIndex: {slotIndex}; state: {state}");
            HandleHotKey(Const_KeyCodes.RWHotKey6, state, slotIndex);
        }
        else if (rewiredInput.GetButtonDown(Const_KeyCodes.RWHotKey7))
        {
            Dev_Logger.Debug($"Hot Key 7; slotIndex: {slotIndex}; state: {state}");
            HandleHotKey(Const_KeyCodes.RWHotKey7, state, slotIndex);
        }
        else if (rewiredInput.GetButtonDown(Const_KeyCodes.RWHotKey8))
        {
            Dev_Logger.Debug($"Hot Key 8; slotIndex: {slotIndex}; state: {state}");
            HandleHotKey(Const_KeyCodes.RWHotKey8, state, slotIndex);
        }
        else if (rewiredInput.GetButtonDown(Const_KeyCodes.RWHotKey9))
        {
            Dev_Logger.Debug($"Hot Key 9; slotIndex: {slotIndex}; state: {state}");
            HandleHotKey(Const_KeyCodes.RWHotKey9, state, slotIndex);
        }
    }

    /// <summary>
    /// Allow Mask 1-4 joystick comamnds to work like hotkeys to equip from inventory. Do not allow this when
    /// on equipment view because there are 10 slots and wouldn't make sense to hotkey back only to slots 1-4.
    /// </summary>
    public void OnHotkeyJoystick(Script_MenuController.InventoryStates state, int slotIndex)
    {
        if (state == Script_MenuController.InventoryStates.Equipment)
            return;
        
        var rewiredInput = Script_PlayerInputManager.Instance.RewiredInput;
        
        if (
            rewiredInput.GetButtonDown(Const_KeyCodes.RWMask1)
            || rewiredInput.GetButtonDown(Const_KeyCodes.RWUnknownControllerMaskVert)
        )
        {
            Dev_Logger.Debug($"Joystick Hot Key 1; slotIndex: {slotIndex}");
            HandleHotKey(Const_KeyCodes.RWHotKey1, state, slotIndex);
        }
        else if (
            rewiredInput.GetButtonDown(Const_KeyCodes.RWMask2)
            || rewiredInput.GetButtonDown(Const_KeyCodes.RWUnknownControllerMaskHz)
        )
        {
            Dev_Logger.Debug($"Joystick Hot Key 2; slotIndex: {slotIndex}");
            HandleHotKey(Const_KeyCodes.RWHotKey2, state, slotIndex);
        }
        else if (
            rewiredInput.GetButtonDown(Const_KeyCodes.RWMask3)
            || rewiredInput.GetNegativeButtonDown(Const_KeyCodes.RWUnknownControllerMaskVert)
        )
        {
            Dev_Logger.Debug($"Joystick Hot Key 3; slotIndex: {slotIndex}");
            HandleHotKey(Const_KeyCodes.RWHotKey3, state, slotIndex);
        }
        else if (
            rewiredInput.GetButtonDown(Const_KeyCodes.RWMask4)
            || rewiredInput.GetNegativeButtonDown(Const_KeyCodes.RWUnknownControllerMaskHz)
        )
        {
            Dev_Logger.Debug($"Joystick Hot Key 4; slotIndex: {slotIndex}");
            HandleHotKey(Const_KeyCodes.RWHotKey4, state, slotIndex);
        }
    }

    private void HandleHotKey(string keyCode, Script_MenuController.InventoryStates state, int slotIndex)
    {
        switch (state)
        {
            case (Script_MenuController.InventoryStates.Inventory):
                HandleInventoryHotKey(keyCode, slotIndex);
                break;
            case (Script_MenuController.InventoryStates.Equipment):
                HandleEquipmentHotKey(keyCode, slotIndex);
                break;
            default:
                break;
        }
    }

    private void HandleInventoryHotKey(string keyCode, int slotIndex)
    {
        var type = Script_InventoryManager.Types.Stickers;
        
        switch (keyCode)
        {
            case (Const_KeyCodes.RWHotKey1):
                inventoryManager.HandleHotkeyStickUnstick(slotIndex, 0, type);
                break;
            case (Const_KeyCodes.RWHotKey2):
                inventoryManager.HandleHotkeyStickUnstick(slotIndex, 1, type);
                break;
            case (Const_KeyCodes.RWHotKey3):
                inventoryManager.HandleHotkeyStickUnstick(slotIndex, 2, type);
                break;
            case (Const_KeyCodes.RWHotKey4):
                inventoryManager.HandleHotkeyStickUnstick(slotIndex, 3, type);
                break;
            default:
                break;
        }
    }

    private void HandleEquipmentHotKey(string keyCode, int slotIndex)
    {
        var type = Script_InventoryManager.Types.Equipment;
        
        switch (keyCode)
        {
            case (Const_KeyCodes.RWHotKey1):
                inventoryManager.HandleHotkeyStickUnstick(0, slotIndex, type);
                break;
            case (Const_KeyCodes.RWHotKey2):
                inventoryManager.HandleHotkeyStickUnstick(1, slotIndex, type);
                break;
            case (Const_KeyCodes.RWHotKey3):
                inventoryManager.HandleHotkeyStickUnstick(2, slotIndex, type);
                break;
            case (Const_KeyCodes.RWHotKey4):
                inventoryManager.HandleHotkeyStickUnstick(3, slotIndex, type);
                break;
            case (Const_KeyCodes.RWHotKey5):
                inventoryManager.HandleHotkeyStickUnstick(4, slotIndex, type);
                break;
            case (Const_KeyCodes.RWHotKey6):
                inventoryManager.HandleHotkeyStickUnstick(5, slotIndex, type);
                break;
            case (Const_KeyCodes.RWHotKey7):
                inventoryManager.HandleHotkeyStickUnstick(6, slotIndex, type);
                break;
            case (Const_KeyCodes.RWHotKey8):
                inventoryManager.HandleHotkeyStickUnstick(7, slotIndex, type);
                break;
            case (Const_KeyCodes.RWHotKey9):
                inventoryManager.HandleHotkeyStickUnstick(8, slotIndex, type);
                break;
            default:
                break;
        }
    }
}
