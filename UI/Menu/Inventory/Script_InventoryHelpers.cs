using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Script_InventoryHelpers
{
    public static Script_Item GetItemInSlot(int Id, Script_Item[] items)
    {
        return items[Id];
    }

    public static bool HasSpace(Script_Item[] items)
    {
        foreach (Script_Item item in items)
            if (item == null)   return true;
        
        return false;
    }

    /// <summary>
    /// Add item to inventory.
    /// </summary>
    /// <param name="itemToAdd">item ScriptableObject</param>
    /// <returns>
    /// True if successfully added; False means it was full
    /// </returns>
    public static bool AddItem(
        Script_Item itemToAdd,
        Script_Item[] items,
        Image[] itemImages
    )
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = itemToAdd;
                itemImages[i].sprite = itemToAdd.sprite;
                itemImages[i].enabled = true;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// For loading into specific slots
    /// </summary>
    public static bool AddItemInSlot(
        Script_Item itemToAdd,
        int i,
        Script_Item[] items,
        Image[] itemImages
    )
    {
        if (items[i] != null)
            Debug.LogWarning($"You are about to overwrite item in slot {i}. Be careful this isn't a bug.");
        items[i] = itemToAdd;
        itemImages[i].sprite = itemToAdd.sprite;
        itemImages[i].enabled = true;
        return true;
    }

    /// Must remove by slot in case there are duplicates of that item
    public static bool RemoveItemInSlot(
        int i,
        Script_Item[] items,
        Image[] itemImages
    )
    {
        items[i] = null;
        itemImages[i].sprite = null;
        itemImages[i].enabled = false;

        return true;
    }

    public static bool RemoveItem(
        Script_Item itemToRemove,
        Script_Item[] items,
        Image[] itemImages
    )
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == itemToRemove)
            {
                items[i] = null;
                itemImages[i].sprite = null;
                itemImages[i].enabled = false;
                
                return true;
            }
        }

        return false;   
    }

    public static void HighlightItem(
        int i,
        bool isFocus,
        Script_Item[] items,
        Image[] itemImages
    )
    {
        if (items[i] == null)   return;
        if (isFocus)    itemImages[i].sprite = items[i].focusedSprite;
        else            itemImages[i].sprite = items[i].sprite;   
    }

    public static Script_Item SearchForItemById(
        string Id,
        out int slot,
        Script_Item[] items
    )
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i]?.id == Id)
            {
                slot = i;
                return items[i];
            }
        }

        slot = -1;
        return null;
    }

    public static void Organize
    (
        Script_Item[] items,
        Image[] itemImages
    )
    {
        // make new lists
        List<Script_Item> itemsList = new List<Script_Item>();
        List<Image> itemImagesList = new List<Image>();

        for (var i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
                itemsList.Add(items[i]);
            
            if (itemImages[i] != null)
                itemImagesList.Add(itemImages[i]);
        }

        // replace current items and itemImages with these
        for (var i = 0; i < items.Length; i++)
        {
            if (i > itemsList.Count - 1)
            {
                items[i] = null;
                itemImages[i].sprite = null;
                itemImages[i].enabled = false;
                continue;
            }
            
            items[i] = itemsList[i];
            itemImages[i].sprite = items[i].sprite;
            itemImages[i].enabled = true;
        }
    }
}
