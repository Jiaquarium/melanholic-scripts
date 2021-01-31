using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LevelBehavior_4 : Script_LevelBehavior
{
    // ==================================================================
    // State Data
    public bool didPickUpMelancholyPianoSticker;
    // ==================================================================

    [SerializeField] private bool[] switchesStates;
    [SerializeField] private Transform lightSwitchesParent;
    [SerializeField] private Script_StickerObject melancholyPianoSticker;

    private Script_LBSwitchHandler switchHandler;
    private bool isInitialized = false;
    

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
    
    public override void SetSwitchState(int Id, bool isOn)
    {
        switchHandler.SetSwitchState(switchesStates, Id, isOn);
    }

    public override void Setup()
    {
        switchHandler = GetComponent<Script_LBSwitchHandler>();
        switchHandler.Setup(game);
        switchesStates = switchHandler.SetupSwitchesState(
            lightSwitchesParent,
            switchesStates,
            isInitialize: !isInitialized
        );

        if (melancholyPianoSticker != null)
        {
            if (didPickUpMelancholyPianoSticker)
                melancholyPianoSticker.gameObject.SetActive(false);
            else
                melancholyPianoSticker.gameObject.SetActive(true);
        }

        isInitialized = true;
    }
}
