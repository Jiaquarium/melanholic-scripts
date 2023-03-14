using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadDrops : MonoBehaviour
{
    [SerializeField] private Script_Game game;
    
    public void SaveDrops(Model_SaveData data)
    {
        // Model_PersistentDrop[] dropsData = game.GetPersistentDrops();
        // data.drops = dropsData;

        // foreach (Model_PersistentDrop drop in dropsData)
        //     Dev_Logger.Debug(drop.itemId);
    }

    public void LoadDrops(Model_SaveData data)
    {
        // if (data.drops == null)
        // {
        //     if (Debug.isDebugBuild) Dev_Logger.Debug("No drops data to load.");
        //     return;
        // }

        // Model_PersistentDrop[] dropsData = data.drops;
        // foreach (Model_PersistentDrop drop in dropsData)
        // {
        //     float[] vectorArray = drop.location;
        //     Vector3 location = new Vector3(vectorArray[0], vectorArray[1], vectorArray[2]);
        //     int dropLevelBehavior = drop.levelBehavior;
            
        //     if (game.InstantiateDropById(drop.itemId, location, dropLevelBehavior))
        //     {
        //         Dev_Logger.Debug($"Successful load of persistent drop {drop.itemId}");
        //     }
        //     else
        //     {
        //         Debug.LogError($"Error in loading persistent drop {drop.itemId}");
        //     }
        // }
    }
}
