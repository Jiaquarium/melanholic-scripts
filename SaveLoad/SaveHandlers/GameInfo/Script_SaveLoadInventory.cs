using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Script_SaveLoadInventory : MonoBehaviour
{
    public void SaveInventory(Model_SaveData data)
    {
        // Unstuck stickers in Inventory
        Script_Item[] stickers = Script_Game.Game.GetStickers();
        string[] stickersIds = new string[stickers.Length];
        
        // Items in Inventory
        Script_Item[] items = Script_Game.Game.GetItems();
        string[] itemsIds = new string[items.Length];
        

        for (int i = 0; i < stickersIds.Length; i++)
        {
            if (stickers[i] == null)  continue;
            
            bool isSticker = stickers[i] is Script_Sticker;
            if (isSticker)   stickersIds[i] = stickers[i]?.id;
        }

        for (int i = 0; i < itemsIds.Length; i++)
        {
            if (items[i] == null)    continue;

            bool isPersistent = items[i].IsSpecial;
            if (isPersistent)   itemsIds[i] = items[i]?.id;
        }

        data.stickersIds = stickersIds;
        data.itemsIds = itemsIds;
    }

    public void LoadInventory(Model_SaveData data)
    {
        if (data.stickersIds == null)
        {
            if (Debug.isDebugBuild) Dev_Logger.Debug($"{name} No Sticker inventory items to load.");
        }
        else
        {
            string[] stickersIds = data.stickersIds;

            for (int i = 0; i < stickersIds.Length; i++)
            {
                if (stickersIds[i] == null)    continue;
                
                Dev_Logger.Debug($"Adding Sticker by id: {stickersIds[i]}, to slot: {i}");

                Script_Game.Game.AddItemInSlotById(stickersIds[i], i);
            }
        }

        if (data.itemsIds == null)
        {
            if (Debug.isDebugBuild) Dev_Logger.Debug($"{name} No Items inventory items to load.");
        }
        else
        {
            string[] itemsIds = data.itemsIds;

            for (int i = 0; i < itemsIds.Length; i++)
            {
                if (itemsIds[i] == null)    continue;
                
                Dev_Logger.Debug($"Adding Item by id: {itemsIds[i]}, to slot: {i}");
                
                Script_Game.Game.AddItemInSlotById(itemsIds[i], i);
            }
        }
    }
}
