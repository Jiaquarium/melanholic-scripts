using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LevelBehavior_13 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool didPickUpLightSticker;
    
    /* ======================================================================= */

    [SerializeField] private Script_StickerObject lightSticker;

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
        if (itemId == lightSticker.Item.id)
        {
            didPickUpLightSticker = true;
        }
    }

    public override void Setup()
    {
        if (lightSticker != null)
        {
            if (didPickUpLightSticker)      lightSticker.gameObject.SetActive(false);
            else                            lightSticker.gameObject.SetActive(true);
        }
    }
}
