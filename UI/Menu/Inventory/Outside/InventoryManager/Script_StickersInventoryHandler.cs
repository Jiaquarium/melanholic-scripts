using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_InventoryManager))]
public class Script_StickersInventoryHandler : MonoBehaviour
{
    private Script_InventoryAudioSettings settings;

    public bool StickSticker(
        Script_Sticker sticker,
        Script_Equipment equipment,
        Script_Inventory inventory,
        int itemSlotId
    )
    {
        // try to equip, checking if we can equip it & if equipment is full
        if (equipment.AddSticker(sticker))
        {
            // if can equip, remove item from inventory
            inventory.RemoveItemInSlot(itemSlotId);
            StickerOnSFX();
            return true;
        }
        else
        {
            Debug.Log("Failed to equip sticker");
            GetComponent<Script_InventoryManager>().ErrorSFX();
            return false;
        }
    }

    public void UnstickSticker(
        Script_Sticker sticker,
        Script_Equipment equipment,
        Script_Inventory inventory,
        int stickerSlotId
    )
    {
        // try to unequip, checking if inventory is full
        if (equipment.RemoveStickerInSlot(stickerSlotId))
        {
            // if can equip, remove item from inventory
            inventory.AddItem(sticker);
            StickerOffSFX();
        }
        else
        {
            print("failed to unstick sticker; no space in inventory or not found");
            // TODO: show messaging or SFX
            GetComponent<Script_InventoryManager>().ErrorSFX();
        }
    }

    void StickerOnSFX()
    {
        settings.inventoryAudioSource.PlayOneShot(settings.stickerOnSFX, settings.stickerOnVol);
    }

    void StickerOffSFX()
    {
        settings.inventoryAudioSource.PlayOneShot(settings.stickerOffSFX, settings.stickerOffVol);
    }

    public void Setup(
        Script_InventoryAudioSettings _settings
    )
    {
        settings = _settings;
    }
}
