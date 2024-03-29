﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_InventoryManager))]
[RequireComponent(typeof(AudioSource))]
public class Script_UsablesInventoryHandler : MonoBehaviour
{
    private Script_InventoryAudioSettings settings;
    
    public bool Use(Script_Usable usable)
    {
        bool isUsed = false;
        
        switch (usable)
        {
            case Script_UsableKey key:
                Dev_Logger.Debug($"Usable case matched (type:key): {key}");
                isUsed = Script_Game.Game.GetPlayer().UseUsableKey(key);

                break;
            default: // Cancel falls in here
                Debug.LogWarning("You haven't implemented this type of Usable");
                break;
        }

        return isUsed;
    }

    public void UseSFX(Script_Usable usable)
    {
        switch(usable)
        {
            case Script_UsableKey key:
                // Using a key's SFX will be handled by the target object.
                break;
            default:
                Debug.LogWarning("Not a valid usable type used somehow, no SFX");
                break;
        }
    }


    public void Setup(
        Script_InventoryAudioSettings _settings
    )
    {
        settings = _settings;
    }
}
