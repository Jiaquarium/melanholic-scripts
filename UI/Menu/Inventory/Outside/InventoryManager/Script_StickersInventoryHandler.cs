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
        int itemSlot,
        bool isBackground = false
    )
    {
        /// Try to equip, checking if we can equip it & if equipment is full
        int equipmentSlotId;
        if (equipment.HasSpace() && equipment.AddSticker(sticker, out equipmentSlotId))
        {
            /// On successful equip, remove sticker from Inventory
            /// and also update Sticker Holster (done in Equipment)
            inventory.RemoveItemInSlot(itemSlot);

            if (!isBackground)
                StickerOnSFX();
            
            return true;
        }
        else
        {
            Dev_Logger.Debug("Failed to equip sticker");
            GetComponent<Script_InventoryManager>().ErrorDullSFX();
            return false;
        }
    }

    public bool HotKeyStickUnstick(
        Script_Equipment equipment,
        Script_Inventory inventory,
        int inventorySlot,
        int equipmentSlot,
        bool isBackground = false
    )
    {
        try
        {
            // Cache and remove the sticker if it is not Active Sticker
            Script_Sticker equipmentSticker = equipment.GetStickerInSlot(equipmentSlot);
            var activeSticker = Script_ActiveStickerManager.Control.ActiveSticker;
            
            // If there is an Active Sticker, warn Player if it's the equipment sticker trying
            // to be switched out.
            if (activeSticker != null && activeSticker == equipmentSticker)
                return OnIsActiveEquippedError();

            equipment.RemoveStickerInSlot(equipmentSlot);
            
            // Cache and remove the sticker in specified inventory slot.
            Script_Sticker inventorySticker = inventory.GetItemInSlot(inventorySlot) as Script_Sticker;
            inventory.RemoveItemInSlot(inventorySlot);

            if (inventorySticker != null)
                equipment.AddStickerInSlot(inventorySticker, equipmentSlot);
            
            if (equipmentSticker != null)
                inventory.AddItemInSlot(equipmentSticker, inventorySlot);
            
            // Error SFX if trying to hotkey switch when both designated slots are empty. 
            if (inventorySticker == null && equipmentSticker == null)
                return OnError();
            
            if (!isBackground)
            {
                // If we only removed from equipment and added to inventory,
                // it's an unequip event.
                if (inventorySticker == null && equipmentSticker != null)
                    StickerOffSFX();
                else
                    StickerOnSFX();
            }
                
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed HotKeyStickUnstick with Error:\n{e}");
            
            return OnError();
        }

        bool OnIsActiveEquippedError()
        {
            GetComponent<Script_InventoryManager>().ErrorUnableSFX();
            return false;
        }
        
        bool OnError()
        {
            GetComponent<Script_InventoryManager>().ErrorDullSFX();
            return false;
        }
    }

    public bool UnstickSticker(
        Script_Sticker sticker,
        Script_Equipment equipment,
        Script_Inventory inventory,
        int stickerSlotId,
        bool isBackground = false
    )
    {
        // try to unequip, checking if inventory is full
        if (equipment.RemoveStickerInSlot(stickerSlotId))
        {
            /// On successful unequip, remove sticker from Equipment
            /// and also update Sticker Holster (done in Equipment)
            inventory.AddItem(sticker);

            if (!isBackground)
                StickerOffSFX();
            
            return true;
        }
        else
        {
            Debug.LogWarning("Failed to unstick sticker; no space in inventory or not found");
            // TODO: show messaging or SFX
            GetComponent<Script_InventoryManager>().ErrorDullSFX();
            
            return false;
        }
    }

    void StickerOnSFX()
    {
        settings.inventoryAudioSource.PlayOneShot(
            Script_SFXManager.SFX.StickerOn,
            Script_SFXManager.SFX.StickerOnVol
        );
    }

    void StickerOffSFX()
    {
        settings.inventoryAudioSource.PlayOneShot(
            Script_SFXManager.SFX.StickerOff,
            Script_SFXManager.SFX.StickerOffVol
        );
    }

    public void Setup(
        Script_InventoryAudioSettings _settings
    )
    {
        settings = _settings;
    }
}
