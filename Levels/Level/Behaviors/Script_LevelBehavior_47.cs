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

    public override void Setup()
    {
        if (puppeteerSticker != null)
        {
            if (didPickUpPuppeteerSticker)  puppeteerSticker.gameObject.SetActive(false);
            else                            puppeteerSticker.gameObject.SetActive(true);
        }
    }
}