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
        /// Try to equip, checking if we can equip it & if equipment is full
        int equipmentSlotId;
        if (equipment.AddSticker(sticker, out equipmentSlotId))
        {
            /// On successful equip, remove sticker from Inventory
            /// and also update Sticker Holster (done in Equipment)
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
            /// On successful unequip, remove sticker from Equipment
            /// and also update Sticker Holster (done in Equipment)
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
