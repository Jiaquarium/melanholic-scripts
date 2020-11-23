using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ItemsEventsManager : MonoBehaviour
{
    public delegate void ItemPickUpDelegate(string itemId);
    public static event ItemPickUpDelegate OnItemPickUp;
    public static void ItemPickUp(string itemId)
    {
        if (OnItemPickUp != null)   OnItemPickUp(itemId);
    }
    public delegate void ItemStashDelegate(string itemId);
    public static event ItemStashDelegate OnItemStash;
    public static void ItemStash(string itemId)
    {
        if (OnItemStash != null)   OnItemStash(itemId);
    }

    public delegate void UnlockDelegate(Script_UsableKey key, string targetId);
    public static event UnlockDelegate OnUnlock;
    public static void Unlock(Script_UsableKey key, string targetId)
    {
        if (OnUnlock != null)   OnUnlock(key, targetId);
    }

    public delegate void ItemPickUpTheatricDoneDelegate(Script_ItemPickUpTheatricsPlayer player);
    public static event ItemPickUpTheatricDoneDelegate OnItemPickUpTheatricDone;
    public static void ItemPickUpTheatricDone(Script_ItemPickUpTheatricsPlayer player)
    {
        if (OnUnlock != null)   OnItemPickUpTheatricDone(player);
    }
}
