using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_UsableKeyTarget : Script_UsableTarget
{
    [SerializeField] protected bool isLocked = true;
    [SerializeField] protected Script_UsableKey myKey;
    [SerializeField] protected Script_TileMapExitEntrance myExit;
    [SerializeField] private Script_TreasureChestLocked myTreasureChest;

    public Script_UsableKey MyKey
    {
        get => myKey;
    }
    
    public virtual bool Unlock(Script_UsableKey key)
    {
        Dev_Logger.Debug($"{name}: TRYING TO UNLOCK ME with Key Id {key.id}!!!");
        
        if (key == myKey)
        {
            if (myTreasureChest != null && myTreasureChest.CheckDisabledDirections())
                return false;
            
            Script_Game.Game.CloseInventory(noSFX: true);
            OnUnlock(key);
            return true;
        }

        return false;
    }

    protected virtual void OnUnlock(Script_UsableKey key)
    {
        Dev_Logger.Debug($"YAY UNLOCKED!!!");
        // unlock animation
        isLocked = false;
        if (myExit != null)
        {
            myExit.IsDisabled = false;
            Script_ItemsEventsManager.Unlock(key, Id);
        }
        if (myTreasureChest != null)
            myTreasureChest.UnlockWithKey();
    }
}
