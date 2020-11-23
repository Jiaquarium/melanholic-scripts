using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadEquipment : MonoBehaviour
{
    public void SaveEquipment(Model_SaveData data)
    {
        Script_Sticker[] equipmentItems = Script_Game.Game.GetEquipmentItems();
        
        string[] equipmentIds = new string[equipmentItems.Length];
        for (int i = 0; i < equipmentIds.Length; i++)
        {
            equipmentIds[i] = equipmentItems[i] != null ? equipmentItems[i].id : null;
        }
        data.equipmentIds = equipmentIds;
    }

    public void LoadEquipment(Model_SaveData data)
    {
        if (data.equipmentIds == null)
        {
            if (Debug.isDebugBuild) Debug.Log("No sticker items to load.");
            return;
        }

        string[] equipmentIds = data.equipmentIds;

        for (int i = 0; i < equipmentIds.Length; i++)
        {
            if (equipmentIds[i] == null)    continue;
            Script_Game.Game.AddEquippedItemInSlotById(equipmentIds[i], i);
        }
    }
}
