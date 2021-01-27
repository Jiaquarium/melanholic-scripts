using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_LevelBehavior_41 : Script_LevelBehavior
{
    // ==================================================================
    // State Data
    public bool didPickUpMelancholyPianoSticker;
    // ==================================================================
    
    [SerializeField] private Script_StickerObject melancholyPianoSticker;

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
        if (itemId == melancholyPianoSticker.Item.id)
        {
            didPickUpMelancholyPianoSticker = true;
        }
    }

    public override void Setup()
    {
        if (didPickUpMelancholyPianoSticker)    melancholyPianoSticker?.gameObject.SetActive(false);
        else                                    melancholyPianoSticker.gameObject.SetActive(true);
    }        
}