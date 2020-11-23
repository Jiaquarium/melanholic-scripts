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
        // give prompt
        print("enter is clicked on me");
        
        // TODO: TEST FOR IF ITEM IS PRESENT
        
        inventoryManager.HandleEquipmentSlotOnEnter(Id);

        // Script_Game.Game.SetActiveItem(Id);
    }
}
