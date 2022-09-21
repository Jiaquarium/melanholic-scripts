using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_GardenLightsController : Script_TriggerPuzzleController
{
    public Script_Item requiredItem;
    [SerializeField] private Script_LevelBehavior_19 LB19;
    
    
    public override void TriggerActivated(string Id, Collider other)
    {
        Dev_Logger.Debug($"Garden trigger {Id} activated with {other.transform.parent.GetComponent<Script_CollectibleObject>().Item}");

        Script_Item itemDropped = other.transform.parent.GetComponent<Script_CollectibleObject>()?.Item;
        if (itemDropped == requiredItem)
        {
            LB19.DropSpotActivated();
        }
    }

    public override void TriggerReactivated(string Id, Collider other)
    {
        Dev_Logger.Debug($"Garden trigger {Id} activated with {other.transform.parent.GetComponent<Script_CollectibleObject>().Item}");

        Script_Item itemDropped = other.transform.parent.GetComponent<Script_CollectibleObject>()?.Item;
        if (itemDropped == requiredItem)
        {
            LB19.DropSpotReactivated();
        }
    }

    public override void TriggerDeactivated(string Id, Collider other)
    {
        Dev_Logger.Debug($"Garden trigger {Id} deactivated with {other.transform.parent.GetComponent<Script_CollectibleObject>().Item}");

        Script_Item itemDropped = other.transform.parent.GetComponent<Script_CollectibleObject>()?.Item;
        if (itemDropped == requiredItem)
        {
            LB19.DropSpotDeactivated();
        }
    }
}
