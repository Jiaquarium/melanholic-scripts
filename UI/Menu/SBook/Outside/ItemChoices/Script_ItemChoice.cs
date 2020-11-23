using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ItemChoice : Script_UIChoice
{
    [SerializeField] private Script_ItemChoices itemChoices;
    [SerializeField] private ItemChoices itemChoice;
    [SerializeField] private Script_InventoryManager inventoryManager;
    
    /// <summary>
    /// called from Item Choice OnClick handler
    /// </summary>
    public override void HandleSelect()
    {
        inventoryManager.HandleItemChoice(itemChoice, itemChoices.itemSlotId);
    }

    public ItemChoices ItemChoice {
        get { return itemChoice; }
    }
}
