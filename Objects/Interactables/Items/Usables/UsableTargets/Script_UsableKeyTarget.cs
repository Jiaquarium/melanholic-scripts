using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_UsableKeyTarget : Script_UsableTarget
{
    [SerializeField] protected bool isLocked = true;
    [SerializeField] protected string myKey;
    [SerializeField] protected Script_TileMapExitEntrance myExit;
    [SerializeField] private Script_TreasureChestLocked myTreasureChest;
    
    public virtual bool Unlock(Script_UsableKey key)
    {
        Debug.Log($"{name}: TRYING TO UNLOCK ME with Key Id {key.id}!!!");
        
        if (key.id == myKey)
        {
            Script_Game.Game.CloseInventory();
            OnUnlock(key);
            return true;
        }

        return false;
    }

    protected virtual void OnUnlock(Script_UsableKey key)
    {
        Debug.Log($"YAY UNLOCKED!!!");
        // unlock animation
        isLocked = false;
        if (myExit != null)             myExit.IsDisabled = false;
        if (myTreasureChest != null)    myTreasureChest.UnlockWithKey();

        Script_ItemsEventsManager.Unlock(key, Id);
    }
}
