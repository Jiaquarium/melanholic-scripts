using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will reactivate the main event system 
/// </summary>
public class Script_ItemChoicesInputManager : Script_InventoryViewInputManager
{
    [SerializeField] private Script_InventoryManager inventoryManager;
    
    protected override void OnDisable()
    {
        /// Handles Drop and Use item choice where we exit directly from ItemChoices
        /// which would fail to register exit input, and fail to call ExitView()
        base.OnDisable();
        gameObject.SetActive(false);
    }
    
    protected override void ExitView()
    {
        Dev_Logger.Debug("Caught Exit Input: Exiting submenu from ItemChoicesInputManager");

        // exit item choices and go back to inventory
        gameObject.SetActive(false);
        inventoryManager.HandleItemChoice(ItemChoices.Cancel, -1);
    }
}
