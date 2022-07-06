using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
        Items,
        Equipment,
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
    public Script_Item[] GetStickers()
    {
        return inventory.Items;
    }

    public Script_Item[] GetItems()
    {
        return items.Items;
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

    public Script_Item SearchStickersForItemById(string Id, out int slot)
    {
        return inventory.SearchForItemById(Id, out slot);
    }

    public Script_Item SearchItemsForItemById(string Id, out int slot)
    {
        return items.SearchForItemById(Id, out slot);
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
        {
            Debug.Log($"{name} Trying to add item {item} to stickers inventory");
            return inventory.AddItem(item);
        }
        else
        {
            Debug.Log($"{name} Trying to add item {item} to items inventory");
            return items.AddItem(item);
        }
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
            case Types.Equipment:
                equipment.HighlightItem(i, isOn);
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

    public void Organize()
    {
        inventory.Organize();
    }

    // ------------------------------------------------------------------
    // View
    private void HandleItemDescription(int i, Types type)
    {
        Script_Item item;
        
        switch(type)
        {
            case Types.Stickers:
                item = inventory.GetItemInSlot(i);
                inventoryViewController.HandleItemDescription(item);
                break;
            case Types.Equipment:
                item = equipment.GetStickerInSlot(i) as Script_Item;
                inventoryViewController.HandleItemDescription(item);
                break;
            case Types.Items:
                item = items.GetItemInSlot(i);
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
        ErrorDullSFX();
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

    /// <summary>
    /// In the current build the only Sticker Option is to Prep (Stick),
    /// this handles that flow, bypassing Item Choices.
    /// </summary>
    public bool HandleOnEnterStick(int slotId)
    {
        var sticker = inventory.GetItemInSlot(slotId) as Script_Sticker;

        if (sticker == null)
        {
            ErrorDullSFX();
            return false;
        }
        
        if (stickersHandler.StickSticker(sticker, equipment, inventory, slotId))
        {
            EnterInventory();
            return true;
        }

        ErrorDullSFX();
        return false;   
    }

    public void HandleEquipmentSlotOnEnter(int stickerSlotId)
    {
        var stickerToRemove = equipment.GetStickerInSlot(stickerSlotId) as Script_Sticker;
        
        // No Sticker in Slot.
        if (stickerToRemove == null)
            ErrorDullSFX();
        // Sticker is in use.
        else if (Script_ActiveStickerManager.Control.ActiveSticker == stickerToRemove)
            ErrorUnableSFX();
        else
            UnstickSticker(stickerToRemove, equipment, inventory, stickerSlotId, false);
    }

    /// <summary>
    /// Equipping and unequipping via hot key. Switch the Masks in either slot.
    /// Error SFX if neither slot has a Mask.
    /// </summary>
    public bool HandleHotkeyStickUnstick(int stickerSlotId, int equipmentSlotId)
    {
        return stickersHandler.HotKeyStickUnstick(
            equipment,
            inventory,
            stickerSlotId,
            equipmentSlotId,
            isBackground: false
        );
    }

    // Note, this will not remove ones past the point of a full equipment.
    public bool HandleUnequipAll(string[] excludes = null)
    {
        // Unequip currently worn mask.
        Script_Game.Game.GetPlayer().DefaultStickerState();
        
        bool didRemoveAll = true;

        for (int i = 0; i < Script_Equipment.numItemSlots; i++)
        {
            var stickerToRemove = equipment.GetStickerInSlot(i) as Script_Sticker;
            
            if (stickerToRemove == null)
                continue;
            else
            {
                if (excludes != null && excludes.FirstOrDefault(id => id == stickerToRemove.id) != null)
                    continue;
                
                bool didRemove = UnstickSticker(stickerToRemove, equipment, inventory, i, isBackground: true);
                if (!didRemove)
                    didRemoveAll = false;
            }
        }

        return didRemoveAll;
    }

    public bool StickStickerByIdBackground(string id)
    {
        int itemSlot;
        var sticker = inventory.SearchForItemById(id, out itemSlot) as Script_Sticker;

        if (sticker == null)
        {
            Debug.LogWarning($"You are trying to stick a mask not in inventory <{id}>");
            return false;
        }

        return stickersHandler.StickSticker(sticker, equipment, inventory, itemSlot, isBackground: true);
    }

    private void Drop(Script_Item item, int itemSlotId)
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
            Script_Game.Game.CloseInventory(noSFX: true);
        }        
        else
        {
            ErrorDullSFX();
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

    bool UnstickSticker(
        Script_Sticker sticker,
        Script_Equipment equipment,
        Script_Inventory inventory,
        int stickerSlotId,
        bool isBackground = false
    )
    {
        return stickersHandler.UnstickSticker(sticker, equipment, inventory, stickerSlotId, isBackground);
    }

    void Examine(
        Script_Collectible collectible
    )
    {
        if (collectible.isExamineDisabled)
        {
            ErrorDullSFX();
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
            
            // Usables will only be stored in Items and not Stickers Inventory.
            items.RemoveItemInSlot(itemSlotId);
            
            /// Closing inventory will be handled by the UsableTarget
        }
        else
        {
            ErrorUnableSFX();
            return;
        }
    }

    // Search in Items which hold Usables.
    public bool TryUseKey(Script_UsableKey key)
    {
        int slot;
        Script_Item foundItem = SearchItemsForItemById(key.id, out slot);
        
        if (foundItem != null)
        {
            items.RemoveItemInSlot(slot);
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

    public void ErrorDullSFX()
    {
        settings.inventoryAudioSource.PlayOneShot(
            Script_SFXManager.SFX.UIErrorSFX, Script_SFXManager.SFX.UIErrorSFXVol
        );
    }

    public void ErrorUnableSFX()
    {
        settings.inventoryAudioSource.PlayOneShot(
            Script_SFXManager.SFX.ErrorBlip,
            Script_SFXManager.SFX.ErrorBlipVol
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

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_InventoryManager))]
    public class Script_InventoryManagerTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_InventoryManager t = (Script_InventoryManager)target;
            
            if (GUILayout.Button("Slot 0, Hotkey 1 (Equipment Slot 0)"))
            {
                t.HandleHotkeyStickUnstick(0, 0);
            }

            if (GUILayout.Button("Remove Item Slot 0"))
            {
                t.inventory.RemoveItemInSlot(0);
            }

            if (GUILayout.Button("Remove Item Slot 1"))
            {
                t.inventory.RemoveItemInSlot(1);
            }

            if (GUILayout.Button("Remove Item Slot 2"))
            {
                t.inventory.RemoveItemInSlot(2);
            }

            if (GUILayout.Button("Remove Item Slot 3"))
            {
                t.inventory.RemoveItemInSlot(3);
            }

            if (GUILayout.Button("Remove Item Slot 4"))
            {
                t.inventory.RemoveItemInSlot(4);
            }

            if (GUILayout.Button("Remove Item Slot 5"))
            {
                t.inventory.RemoveItemInSlot(5);
            }

            if (GUILayout.Button("Remove Item Slot 6"))
            {
                t.inventory.RemoveItemInSlot(6);
            }

            if (GUILayout.Button("Remove Item Slot 7"))
            {
                t.inventory.RemoveItemInSlot(7);
            }

            if (GUILayout.Button("Remove Item Slot 8"))
            {
                t.inventory.RemoveItemInSlot(8);
            }
        }
    }
#endif   
}
