using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_MenuHotKeyInputManager : MonoBehaviour
{
    [SerializeField] private Script_InventoryManager inventoryManager;
    
    public void OnHotkey(Script_MenuController.InventoryStates state, int slotIndex)
    {
        var playerInput = Script_PlayerInputManager.Instance;
        
        if (playerInput.MyPlayerInput.actions[Const_KeyCodes.Effect1].WasPressedThisFrame())
        {
            Debug.Log($"Hot Key 1; slotIndex: {slotIndex}; state: {state}");
            HandleHotKey(Const_KeyCodes.Effect1, state, slotIndex);
        }
        else if (playerInput.MyPlayerInput.actions[Const_KeyCodes.Effect2].WasPressedThisFrame())
        {
            Debug.Log($"Hot Key 2; slotIndex: {slotIndex}; state: {state}");
            HandleHotKey(Const_KeyCodes.Effect2, state, slotIndex);
        }
        else if (playerInput.MyPlayerInput.actions[Const_KeyCodes.Effect3].WasPressedThisFrame())
        {
            Debug.Log($"Hot Key 3; slotIndex: {slotIndex}; state: {state}");
            HandleHotKey(Const_KeyCodes.Effect3, state, slotIndex);
        }
        else if (playerInput.MyPlayerInput.actions[Const_KeyCodes.Effect4].WasPressedThisFrame())
        {
            Debug.Log($"Hot Key 4; slotIndex: {slotIndex}; state: {state}");
            HandleHotKey(Const_KeyCodes.Effect4, state, slotIndex);
        }
        else if (playerInput.MyPlayerInput.actions[Const_KeyCodes.Effect5].WasPressedThisFrame())
        {
            Debug.Log($"Hot Key 5; slotIndex: {slotIndex}; state: {state}");
            HandleHotKey(Const_KeyCodes.Effect5, state, slotIndex);
        }
        else if (playerInput.MyPlayerInput.actions[Const_KeyCodes.Effect6].WasPressedThisFrame())
        {
            Debug.Log($"Hot Key 6; slotIndex: {slotIndex}; state: {state}");
            HandleHotKey(Const_KeyCodes.Effect6, state, slotIndex);
        }
        else if (playerInput.MyPlayerInput.actions[Const_KeyCodes.Effect7].WasPressedThisFrame())
        {
            Debug.Log($"Hot Key 7; slotIndex: {slotIndex}; state: {state}");
            HandleHotKey(Const_KeyCodes.Effect7, state, slotIndex);
        }
        else if (playerInput.MyPlayerInput.actions[Const_KeyCodes.Effect8].WasPressedThisFrame())
        {
            Debug.Log($"Hot Key 8; slotIndex: {slotIndex}; state: {state}");
            HandleHotKey(Const_KeyCodes.Effect8, state, slotIndex);
        }
        else if (playerInput.MyPlayerInput.actions[Const_KeyCodes.Effect9].WasPressedThisFrame())
        {
            Debug.Log($"Hot Key 9; slotIndex: {slotIndex}; state: {state}");
            HandleHotKey(Const_KeyCodes.Effect9, state, slotIndex);
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
        switch (keyCode)
        {
            case (Const_KeyCodes.Effect1):
                inventoryManager.HandleHotkeyStickUnstick(slotIndex, 0);
                break;
            case (Const_KeyCodes.Effect2):
                inventoryManager.HandleHotkeyStickUnstick(slotIndex, 1);
                break;
            case (Const_KeyCodes.Effect3):
                inventoryManager.HandleHotkeyStickUnstick(slotIndex, 2);
                break;
            case (Const_KeyCodes.Effect4):
                inventoryManager.HandleHotkeyStickUnstick(slotIndex, 3);
                break;
            default:
                break;
        }
    }

    private void HandleEquipmentHotKey(string keyCode, int slotIndex)
    {
        switch (keyCode)
        {
            case (Const_KeyCodes.Effect1):
                inventoryManager.HandleHotkeyStickUnstick(0, slotIndex);
                break;
            case (Const_KeyCodes.Effect2):
                inventoryManager.HandleHotkeyStickUnstick(1, slotIndex);
                break;
            case (Const_KeyCodes.Effect3):
                inventoryManager.HandleHotkeyStickUnstick(2, slotIndex);
                break;
            case (Const_KeyCodes.Effect4):
                inventoryManager.HandleHotkeyStickUnstick(3, slotIndex);
                break;
            case (Const_KeyCodes.Effect5):
                inventoryManager.HandleHotkeyStickUnstick(4, slotIndex);
                break;
            case (Const_KeyCodes.Effect6):
                inventoryManager.HandleHotkeyStickUnstick(5, slotIndex);
                break;
            case (Const_KeyCodes.Effect7):
                inventoryManager.HandleHotkeyStickUnstick(6, slotIndex);
                break;
            case (Const_KeyCodes.Effect8):
                inventoryManager.HandleHotkeyStickUnstick(7, slotIndex);
                break;
            case (Const_KeyCodes.Effect9):
                inventoryManager.HandleHotkeyStickUnstick(8, slotIndex);
                break;
            default:
                break;
        }
    }
}
