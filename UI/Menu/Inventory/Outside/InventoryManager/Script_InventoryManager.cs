using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Script_StickersInventoryHandler))]
[RequireComponent(typeof(Script_UsablesInventoryHandler))]
[RequireComponent(typeof(Script_CollectiblesInventoryHandler))]
[RequireComponent(typeof(Script_ItemDictionary))]
[RequireComponent(typeof(AudioSource))]
public class Script_InventoryManager : MonoBehaviour
{
    public enum Types
    {
        Stickers,
        Items
    }
    
    [SerializeField] private Script_MenuController menuController;
    
    [SerializeField] private Script_SBookOverviewController sBookController;
    [SerializeField] private Script_ItemsController itemsController;
    
    [SerializeField] private Script_InventoryViewController inventoryViewController;
    [SerializeField] private Script_InventoryViewController itemsViewController;
    
    [SerializeField] private Script_ItemChoices stickerChoices;
    [SerializeField] private Script_ItemChoices collectibleChoices;
    [SerializeField] private Script_ItemChoices usableChoices;
    [SerializeField] private Script_ItemChoice initialItemChoice;
    
    // Stickers
    [SerializeField] private Script_Inventory inventory;
    // Items
    [SerializeField] private Script_Items items;
    
    [SerializeField] private Script_Equipment equipment;
    [SerializeField] private Script_InventoryAudioSettings settings;
    
    [SerializeField] private Script_ItemChoicesInputManager stickersChoicesInputManager;
    [SerializeField] private Script_ItemChoicesInputManager itemsChoicesInputManager;
    
    [SerializeField] private Script_PersistentDropsContainer persistentDropsContainer;
    private Script_StickersInventoryHandler stickersHandler;
    private Script_CollectiblesInventoryHandler collectiblesHandler;
    private Script_UsablesInventoryHandler usablesHandler;
    private Script_ItemChoices itemChoices;

    // ------------------------------------------------------------------
    // Getters
    public Script_Item[] GetInventoryItems()
    {
        return inventory.Items;
    }

    public Script_Sticker[] GetEquipmentItems()
    {
        return equipment.Items;
    }
    
    public Script_Inventory GetInventory()
    {
        return inventory;
    }

    public Script_Item GetItemInSlot(int i)
    {
        return inventory.GetItemInSlot(i);
    }

    /// <summary>
    /// Gets the Item Object from the Item Dictionary by Id
    /// </summary>
    private bool GetItemObjectById(string itemId, out Script_ItemObject itemToAdd)
    {
        if (!GetComponent<Script_ItemDictionary>().myDictionary.TryGetValue(itemId, out itemToAdd))
        {
            Debug.LogError($"Getting item: {itemId} from ItemDictionary failed.");
            return false;
        }

        return true;
    }
    
    public bool CheckStickerEquipped(Script_Sticker item)
    {
        return equipment.SearchForSticker(item);
    }

    public bool CheckStickerEquippedById(string Id)
    {
        return equipment.SearchForStickerById(Id);
    }

    public Script_Item SearchInventoryForItemById(string Id, out int slot)
    {
        for (int i = 0; i < inventory.Items.Length; i++)
        {
            if (inventory.Items[i]?.id == Id)
            {
                slot = i;
                return inventory.Items[i];
            }
        }

        slot = -1;
        return null;
    }

    // ------------------------------------------------------------------
    // Setters
    public bool AddItemById(string itemId)
    {
        // Get the ItemObject from dict by Id
        Script_ItemObject itemToAdd;
        GetItemObjectById(itemId, out itemToAdd);

        return AddItem(itemToAdd.Item);
    }

    public bool AddItemInSlotById(string itemId, int i)
    {
        Script_ItemObject itemToAdd;
        GetItemObjectById(itemId, out itemToAdd);

        if (itemToAdd.Item is Script_Sticker)
            return inventory.AddItemInSlot(itemToAdd.Item, i);
        else
            return items.AddItemInSlot(itemToAdd.Item, i);
    }

    public bool AddEquippedItemInSlotById(string equipmentId, int i)
    {
        Script_ItemObject itemToAdd;
        GetItemObjectById(equipmentId, out itemToAdd);

        return equipment.AddStickerInSlot((Script_Sticker)itemToAdd.Item, i);
    }

    public Script_ItemObject InstantiateDropById(string itemId, Vector3 location, int levelBehavior)
    {
        Script_ItemObject itemToAdd;
        GetItemObjectById(itemId, out itemToAdd);

        if (itemToAdd == null)
        {
            Debug.LogError($"Unable to find persistent drop: {itemId}");
            return null;
        }

        return InstantiateDrop(itemToAdd, location, levelBehavior);
    }
    
    public bool AddItem(Script_Item item)
    {
        if (item is Script_Sticker)
            return inventory.AddItem(item);
        else
            return items.AddItem(item);
    }

    public bool RemoveItem(Script_Item item)
    {
        if (item is Script_Sticker)
            return inventory.RemoveItem(item);
        else
            return items.RemoveItem(item);
    }

    public void HighlightItem(
        int i,
        bool isOn,
        bool showDescription,
        Types type
    )
    {
        Debug.Log($"HighlightItem: {i}");
        
        switch(type)
        {
            case Types.Stickers:
                inventory.HighlightItem(i, isOn);
                break;
            case Types.Items:
                items.HighlightItem(i, isOn);
                break;
        }

        // call to replace itemDescription text
        if (isOn && showDescription)
        {
            Debug.Log($"Show item description text: {i}");
            HandleItemDescription(i, type);
        }
    }

    // ------------------------------------------------------------------
    // View
    private void HandleItemDescription(int i, Types type)
    {
        switch(type)
        {
            case Types.Stickers:
                Script_Item _item = inventory.GetItemInSlot(i);
                inventoryViewController.HandleItemDescription(_item);
                break;
            case Types.Items:
                Script_Item item = items.GetItemInSlot(i);
                itemsViewController.HandleItemDescription(item);
                break;
        }
    }

    // ------------------------------------------------------------------
    // Item Choices
    public bool ShowItemChoices(int itemSlotId, Types type)
    {
        switch (type)
        {
            case Types.Stickers:
                if (GetItemInStickers(itemSlotId))  return true;
                break;
            case Types.Items:
                if (GetItemInItems(itemSlotId))     return true;
                break;
        }

        Debug.Log("no item in slot");
        ErrorSFX();
        return false;
        
        bool GetItemInStickers(int itemSlotId)
        {
            if (inventory.GetItemInSlot(itemSlotId))
            {
                // SBook Controller will handle EventSystem, setting active
                Script_Item item = inventory.GetItemInSlot(itemSlotId);
                switch(item)
                {
                    case Script_Sticker sticker:
                        itemChoices = stickerChoices;
                        break;
                    default:
                        Debug.Log("Error. Did not match a type for this item.");
                        break;
                }

                itemChoices.itemSlotId = itemSlotId;
                itemChoices.SetDropChoice(item.isDroppable);
                sBookController.EnterItemChoices(itemChoices);
                stickersChoicesInputManager.gameObject.SetActive(true);

                return true;
            }

            return false;
        }

        bool GetItemInItems(int itemSlotId)
        {
            if (items.GetItemInSlot(itemSlotId))
            {
                // SBook Controller will handle EventSystem, setting active
                Script_Item item = items.GetItemInSlot(itemSlotId);
                
                Debug.Log($"ITEM IS TYPE: {item.GetType()}");
                
                switch(item)
                {
                    case Script_Collectible collectible:
                        itemChoices = collectibleChoices;
                        break;
                    case Script_Usable collectible:
                        itemChoices = usableChoices;
                        break;
                    default:
                        Debug.Log("Error. Did not match a type for this item.");
                        break;
                }

                itemChoices.itemSlotId = itemSlotId;
                itemChoices.SetDropChoice(item.isDroppable);
                itemsController.EnterItemChoices(itemChoices);
                itemsChoicesInputManager.gameObject.SetActive(true);

                return true;   
            }
            
            return false;
        }
    }

    void HideItemChoices()
    {
        itemChoices.gameObject.SetActive(false);
    }

    public void HandleItemChoice(ItemChoices itemChoice, int itemSlotId)
    {
        switch (itemChoice)
        {
            case ItemChoices.Stick:
                StickSticker(
                    (Script_Sticker)inventory.GetItemInSlot(itemSlotId),
                    equipment,
                    inventory,
                    itemSlotId
                );
                break;
            case ItemChoices.Examine:
                Examine(
                    (Script_Collectible)items.GetItemInSlot(itemSlotId)
                );
                break;
            case ItemChoices.Drop:
                Drop(items.GetItemInSlot(itemSlotId), itemSlotId);
                break;
            case ItemChoices.Use:
                Use(
                    (Script_Usable)items.GetItemInSlot(itemSlotId), itemSlotId
                );
                /// DON'T EnterInventory() here in case we need to exit on successful use
                /// CutScene will exit for us
                break;
            /// Cancel handled here, Submenu Input Controller uses this
            default: 
                HideItemChoices();
                EnterInventory();
                print("DEFAULT CASE");
                break;
        }
    }

    public void HandleEquipmentSlotOnEnter(int stickerSlotId)
    {
        if (equipment.GetStickerInSlot(stickerSlotId))
        {
            UnstickSticker(
                (Script_Sticker)equipment.GetStickerInSlot(stickerSlotId),
                equipment,
                inventory,
                stickerSlotId
            );
            return;
        }

        print("no sticker in slot");
        // TODO: SFX
        ErrorSFX();
    }

    void Drop(Script_Item item, int itemSlotId)
    {
        Debug.Log($"Dropping item {item.id}");
        HideItemChoices();
        EnterInventory();

        // need reference to the itemObject so we can recreate it in world space as itemObject
        Script_ItemObject itemToDrop;
        if (GetComponent<Script_ItemDictionary>().myDictionary.TryGetValue(item.id, out itemToDrop))
        {
            DropSFX();
            inventory.RemoveItemInSlot(itemSlotId);
            InstantiateDrop(
                itemToDrop,
                Script_Game.Game.GetPlayerLocation(),
                Script_Game.Game.level
            );
            Script_Game.Game.CloseInventory();
        }        
        else
        {
            ErrorSFX();
            Debug.LogError($"Drop item: {item.id} failed.");
            return;
        }
    }

    private Script_ItemObject InstantiateDrop(Script_ItemObject itemToDrop, Vector3 location, int LB)
    {
        // create the object in world space
        Script_ItemObject itemObj = Instantiate(
            itemToDrop,
            location,
            Quaternion.identity
        );
        
        // only make persistent if is Special
        if (itemObj.Item.IsSpecial)
        {
            itemObj.transform.SetParent(persistentDropsContainer.transform, true);
            itemObj.myLevelBehavior = LB;
        }
        
        return itemObj;
    }

    private void DropSFX()
    {
        GetComponent<AudioSource>().PlayOneShot(
            Script_SFXManager.SFX.PlayerDropItem,
            Script_SFXManager.SFX.PlayerDropItemVol
        );
    }
    
    private bool StickSticker(
        Script_Sticker sticker,
        Script_Equipment equipment,
        Script_Inventory inventory,
        int itemSlotId
    )
    {
        if (stickersHandler.StickSticker(sticker, equipment, inventory, itemSlotId))
        {
            /// Hide ItemChoices must come before hydrating slots;
            /// otherwise, will trigger a deselect and select on the slot
            /// due to exiting its EventSystem causing a flicker
            HideItemChoices();
            EnterInventory();
            return true;
        }

        return false;
    }

    void UnstickSticker(
        Script_Sticker sticker,
        Script_Equipment equipment,
        Script_Inventory inventory,
        int stickerSlotId
    )
    {
        stickersHandler.UnstickSticker(sticker, equipment, inventory, stickerSlotId);
    }

    void Examine(
        Script_Collectible collectible
    )
    {
        if (collectible.isExamineDisabled)
        {
            ErrorSFX();
            return;
        }
        
        HideItemChoices();
        collectiblesHandler.Examine(collectible);   
    }

    void Use(Script_Usable usable, int itemSlotId)
    {
        HideItemChoices();
        EnterInventory();
        
        if (usablesHandler.Use(usable))
        {
            usablesHandler.UseSFX(usable);
            
            inventory.RemoveItemInSlot(itemSlotId);
            /// Closing inventory will be handled by the UsableTarget
        }
        else
        {
            CantUseSFX();
            return;
        }
    }

    public bool TryUseKey(Script_UsableKey key)
    {
        // get key by id
        int slot;
        Script_Item foundItem = SearchInventoryForItemById(key.id, out slot);
        
        if (foundItem != null)
        {
            inventory.RemoveItemInSlot(slot);
            return true;
        }

        return false;
    }

    // ------------------------------------------------------------------
    // Item Choices

    // Check Topbar State to decide which Inventory to reenter.
    public void EnterInventory()
    {
        switch (menuController.topBarState)
        {
            case (Script_MenuController.TopBarStates.stickers):
                sBookController.EnterInventoryView();
                break;
            case (Script_MenuController.TopBarStates.items):
                itemsController.EnterInventoryView();
                break;
            default:
                Debug.LogWarning($"{name}: You are trying to reenter inventory while in non-inventory view.");
                break;
        }
    }

    public void ErrorSFX()
    {
        settings.inventoryAudioSource.PlayOneShot(settings.errorSFX, settings.errorVolume);
    }

    public void CantUseSFX()
    {
        settings.inventoryAudioSource.PlayOneShot(
            settings.UsableTargetNotFound,
            settings.UsableTargetNotFoundVol
        );
    }

    public void Setup()
    {
        stickersHandler = GetComponent<Script_StickersInventoryHandler>();
        collectiblesHandler = GetComponent<Script_CollectiblesInventoryHandler>();
        usablesHandler = GetComponent<Script_UsablesInventoryHandler>();

        stickersHandler.Setup(settings);
        collectiblesHandler.Setup(settings);
        usablesHandler.Setup(settings);
        
        stickerChoices.gameObject.SetActive(false);
        collectibleChoices.gameObject.SetActive(false);
        usableChoices.gameObject.SetActive(false);
        stickersChoicesInputManager.gameObject.SetActive(false);
        itemsChoicesInputManager.gameObject.SetActive(false);
    }
}
