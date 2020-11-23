using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Script_SaveLoadInventory : MonoBehaviour
{
    public void SaveInventory(Model_SaveData data)
    {
        Script_Item[] inventoryItems = Script_Game.Game.GetInventoryItems();
        
        string[] inventoryIds = new string[inventoryItems.Length];
        for (int i = 0; i < inventoryIds.Length; i++)
        {
            inventoryIds[i] = inventoryItems[i] != null ? inventoryItems[i].id : null;
        }
        data.inventoryIds = inventoryIds;
    }

    public void LoadInventory(Model_SaveData data)
    {
        if (data.inventoryIds == null)
        {
            if (Debug.isDebugBuild) Debug.Log("No inventory items to load.");
            return;
        }

        string[] inventoryIds = data.inventoryIds;

        for (int i = 0; i < inventoryIds.Length; i++)
        {
            if (inventoryIds[i] == null)    continue;
            
            Debug.Log($"Adding item by id: {inventoryIds[i]}, to slot: {i}");

            Script_Game.Game.AddItemInSlotById(inventoryIds[i], i);
        }
    }
}
