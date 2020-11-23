using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Script_InventoryViewSlotSelectSound : Script_SelectSound, ISelectHandler
{
    public override void OnSelect(BaseEventData e)
    {
        // other option is to tell manager which sounds to play
        // lateUpdate can decide which one to pick (onSubmit takes priority)
        if (
            eventSystem.lastSelected != null
            && eventSystem.lastSelected.GetComponent<Script_InventoryViewSlot>() != null
        ) 
        {
            PlaySFX();
        }
    }
}
