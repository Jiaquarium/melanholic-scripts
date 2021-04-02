using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_LevelBehavior_47 : Script_LevelBehavior
{
    // ==================================================================
    // State Data
    public bool didPickUpPuppeteerSticker;
    // ==================================================================
    
    [SerializeField] private Script_StickerObject puppeteerSticker;
    [SerializeField] private Script_DemonNPC Ids;

    protected override void OnEnable()
    {
        Script_ItemsEventsManager.OnItemPickUp += OnItemPickUp;
    }

    protected override void OnDisable()
    {
        Script_ItemsEventsManager.OnItemPickUp -= OnItemPickUp;
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
    public void TrackTalkedToIdsEndOfDay()
    {
        Script_EventCycleManager.Control.DidTalkToIdsToday = true;   
    }

    public override void Setup()
    {
        if (puppeteerSticker != null)
        {
            if (didPickUpPuppeteerSticker)  puppeteerSticker.gameObject.SetActive(false);
            else                            puppeteerSticker.gameObject.SetActive(true);
        }

        // Returning to the same room after talking to Ids, he will be gone.
        if (Script_EventCycleManager.Control.DidTalkToIdsToday)
        {
            Ids.gameObject.SetActive(false);
        }
        else
        {
            if (Script_EventCycleManager.Control.IsIdsInSanctuary())
                Ids.gameObject.SetActive(true);
            else
                Ids.gameObject.SetActive(false);
        }
    }
}