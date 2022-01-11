using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_ItemSlotButtonHighlighter))]
public class Script_InventoryViewSlot : Script_Slot
{
    [SerializeField] private Script_InventoryManager.Types type;
    [SerializeField] Script_InventoryManager inventoryManager;

    public Script_InventoryManager.Types Type
    {
        get => type;
    }
    
    /// <summary>
    /// called from Inventory Slot OnClick
    /// </summary>
    public void OnEnter()
    {
        switch (type)
        {
            case (Script_InventoryManager.Types.Stickers):
                OnEnterSticker();        
                break;
            case (Script_InventoryManager.Types.Items):
                OnEnterItem();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Immediately "Prep" the Mask OnEnter.
    /// </summary>
    private void OnEnterSticker()
    {
        inventoryManager.HandleOnEnterStick(Id);
    }

    private void OnEnterItem()
    {
        // Keep button highlighted ONLY if going to show choices
        if (inventoryManager.ShowItemChoices(Id, type))
        {
            Script_ItemSlotButtonHighlighter h = GetComponent<Script_ItemSlotButtonHighlighter>();
            h.isEnterPressed = true;
            
            // To maintain highlight when we enter into itemChoices
            h.ForceHighlightItemChoices();
        }
    }
}
