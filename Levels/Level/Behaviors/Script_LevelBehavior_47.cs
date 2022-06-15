using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_LevelBehavior_47 : Script_LevelBehavior
{
    public const string MapName = "Rock Garden";
    
    // ==================================================================
    // State Data
    public bool didPickUpPuppeteerSticker;
    // ==================================================================
    
    [SerializeField] private Script_StickerObject puppeteerSticker;
    [SerializeField] private Script_DemonNPC Ids;

    private bool didMapNotification;
    private bool didInteractPositiveWithIds;

    protected override void OnEnable()
    {
        base.OnEnable();
        
        Script_GameEventsManager.OnLevelInitComplete    += OnLevelInitCompleteEvent;
        Script_ItemsEventsManager.OnItemPickUp          += OnItemPickUp;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        Script_GameEventsManager.OnLevelInitComplete    -= OnLevelInitCompleteEvent;
        Script_ItemsEventsManager.OnItemPickUp          -= OnItemPickUp;
    }

    private void OnLevelInitCompleteEvent()
    {
        if (!didMapNotification)
        {
            Script_MapNotificationsManager.Control.PlayMapNotification(MapName);
            didMapNotification = true;
        }
    }

    private void OnItemPickUp(string itemId)
    {
        if (itemId == puppeteerSticker.Item.id)
        {
            didPickUpPuppeteerSticker = true;
        }
    }

    // ------------------------------------------------------------------
    // Next Node Actions
    
    /// <summary>
    /// Should only be able to increment positive interactions by 1 per day.
    /// </summary>
    public void TrackTalkedToIdsEndOfDay()
    {
        if (!didInteractPositiveWithIds)
        {
            Script_EventCycleManager.Control.InteractPositiveWithIds();
            didInteractPositiveWithIds = true;
        }
    }

    public override void Setup()
    {
        if (puppeteerSticker != null)
        {
            if (didPickUpPuppeteerSticker)  puppeteerSticker.gameObject.SetActive(false);
            else                            puppeteerSticker.gameObject.SetActive(true);
        }

        if (Script_EventCycleManager.Control.IsIdsInSanctuary())
            Ids.gameObject.SetActive(true);
        else
            Ids.gameObject.SetActive(false);
    }
}