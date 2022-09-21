using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_EquipmentSlot : Script_Slot
{
    [SerializeField] Script_InventoryManager inventoryManager;

    /// <summary>
    /// called from Equipment Slot OnClick
    /// </summary>
    public void OnEnter()
    {
        Dev_Logger.Debug($"{name} Enter is clicked on me");
        
        inventoryManager.HandleEquipmentSlotOnEnter(Id);
    }
}
