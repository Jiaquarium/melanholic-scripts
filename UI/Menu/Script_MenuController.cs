using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Handles overall menu state based on TopBar and settings
/// 
/// TBD DELETE ButtonMetadata & UIIds, not being used anymore
/// </summary>
public class Script_MenuController : Script_UIState
{
    public enum TopBarStates
    {
        stickers        = 0,
        items           = 1,
        notes           = 2,
        entries         = 3,
        thoughts        = 4,
        memories        = 5
    }

    public enum InventoryStates
    {
        None        = 0,
        Overview    = 1,
        Inventory   = 2,
        Equipment   = 3,
        Items       = 4,
        Entries     = 5
    }
    
    public TopBarStates topBarState;
    public InventoryStates inventoryState;
    public bool isSBookDisabled;
    public GameObject SBookOverviewButton;
    public GameObject initialStateSelected;
    public Script_SBookOverviewController SBookController;
    [SerializeField] private Script_ItemsController itemsController;
    [SerializeField] private Script_NotesController notesController;
    [SerializeField] private Script_EntriesController entriesController;
    public Script_CanvasGroupController_Thoughts thoughtsController;
    [SerializeField] private Script_MemoriesController memoriesController;
    public Script_InventoryViewSettings inventorySettings;
    public Script_EntriesViewSettings entriesSettings;
    public Script_InventoryOverviewSettings inventoryOverviewSettings;
    
    [SerializeField] private Script_MenuInputManager inputManager;
    [SerializeField] private Script_InventoryManager inventoryManager;
    [SerializeField] private Script_EntriesViewController entriesViewController;
    [SerializeField] private Button entriesTopBarButton;
    private Selectable entriesTopBarSelectOnDownButton;
    [SerializeField] private Script_MemoriesViewController memoriesViewController;
    [SerializeField] private Button memoriesTopBarButton;
    private Selectable memoriesTopBarSelectOnDownButton;

    public InventoryStates InventoryState
    {
        get => inventoryState;
        set => inventoryState = value;
    }

    void OnEnable()
    {
        ChangeTopBarState(topBarState);
    }
    
    void Update()
    {        
        if (InventoryState == InventoryStates.None)
        {
            InitializeState();
        }
        else if (InventoryState == InventoryStates.Overview)
        {
            inputManager.HandleExitInput();
        }

        HandleNullViewStates();
    }

    public void ChangeStateToOverview()
    {
        InventoryState = InventoryStates.Overview;
    }

    public void ChangeStateToInventoryView()
    {
        Debug.Log($"MenuController inventoryState before stickers view: {InventoryState}");
        InventoryState = InventoryStates.Inventory;
    }

    public void ChangeStateToEquipmentView()
    {
        InventoryState = InventoryStates.Equipment;
    }

    public void ChangeStateToItemsView()
    {
        InventoryState = InventoryStates.Items;
    }

    public void ChangeStateToEntriesView()
    {
        InventoryState = InventoryStates.Entries;
    }

    public void ChangeTopBarState(TopBarStates state)
    {
        switch(state)
        {
            case TopBarStates.stickers:
                topBarState = TopBarStates.stickers;

                SBookController.Open();
                itemsController.Close();
                thoughtsController.Close();
                entriesController.Close();
                memoriesController.Close();
                notesController.Close();

                break;

            case TopBarStates.items:
                topBarState = TopBarStates.items;

                SBookController.Close();
                itemsController.Open();
                thoughtsController.Close();
                entriesController.Close();
                memoriesController.Close();
                notesController.Close();

                break;
            
            case TopBarStates.notes:
                topBarState = TopBarStates.notes;

                SBookController.Close();
                itemsController.Close();
                notesController.Open();
                thoughtsController.Close();
                entriesController.Close();
                memoriesController.Close();

                break;            

            case TopBarStates.entries:
                topBarState = TopBarStates.entries;

                SBookController.Close();
                itemsController.Close();
                notesController.Close();
                entriesController.Open();
                thoughtsController.Close();
                memoriesController.Close();

                break;

            case TopBarStates.thoughts:
                topBarState = TopBarStates.thoughts;

                SBookController.Close();
                itemsController.Close();
                notesController.Close();
                entriesController.Close();
                thoughtsController.Open();
                memoriesController.Close();

                break;

            case TopBarStates.memories:
                topBarState = TopBarStates.memories;

                SBookController.Close();
                itemsController.Close();
                notesController.Close();
                entriesController.Close();
                thoughtsController.Close();
                memoriesController.Open();

                break;
                
            default:
                break;
        }
    }

    // allow menu to be "sticker" for non multiple UI
    public void ChangeRepeatDelay(float t, int a)
    {
        EventSystem.current.GetComponent<StandaloneInputModule>().repeatDelay = t;
        EventSystem.current.GetComponent<StandaloneInputModule>().inputActionsPerSecond = a;
    }

    public void EnableSBook(bool isActive)
    {
        SBookOverviewButton.SetActive(isActive);
    }

    public Script_Item[] GetInventoryItems()
    {
        return inventoryManager.GetInventoryItems();
    }

    public Script_Sticker[] GetEquipmentItems()
    {
        return inventoryManager.GetEquipmentItems();
    }

    public void HighlightItem(int id, bool isOn, bool showDesc)
    {
        inventoryManager.HighlightItem(id, isOn, showDesc);
    }

    public bool AddItem(Script_Item item)
    {
        return inventoryManager.AddItem(item);
    }

    public bool AddItemInSlotById(string itemId, int i)
    {
        return inventoryManager.AddItemInSlotById(itemId, i);
    }

    public bool AddItemById(string itemId)
    {
        return inventoryManager.AddItemById(itemId);
    }

    public bool AddEquippedItemInSlotById(string equipmentId, int i)
    {
        return inventoryManager.AddEquippedItemInSlotById(equipmentId, i);
    }

    public bool CheckStickerEquipped(Script_Sticker sticker)
    {
        return inventoryManager.CheckStickerEquipped(sticker);
    }

    public bool CheckStickerEquippedById(string stickerId)
    {
        return inventoryManager.CheckStickerEquippedById(stickerId);
    }

    /// <summary>
    /// Note: Excludes equipped items.
    /// </summary>
    public Script_Item GetInventoryItem(string itemId, out int slot)
    {
        return inventoryManager.SearchInventoryForItemById(itemId, out slot);
    }

    public Script_ItemObject InstantiateDropById(string itemId, Vector3 location, int LB)
    {
        return inventoryManager.InstantiateDropById(itemId, location, LB);
    }

    public bool TryUseKey(Script_UsableKey key)
    {
        return inventoryManager.TryUseKey(key);
    }

    public bool RemoveItem(Script_Item item)
    {
        return inventoryManager.RemoveItem(item);
    }

    public void HandleNullViewStates()
    {
        EntriesTopBarState();
        MemoriesTopBarState();

        void EntriesTopBarState()
        {
            Navigation entriesNav = entriesTopBarButton.GetComponent<Selectable>().navigation;    
            entriesNav.selectOnDown = entriesViewController.slots.Length == 0 ? null : entriesTopBarSelectOnDownButton;
            entriesTopBarButton.GetComponent<Selectable>().navigation = entriesNav;
        }

        void MemoriesTopBarState()
        {
            Navigation memoriesNav = memoriesTopBarButton.GetComponent<Selectable>().navigation;    
            memoriesNav.selectOnDown = memoriesViewController.slots.Length == 0 ? null : memoriesTopBarSelectOnDownButton;
            memoriesTopBarButton.GetComponent<Selectable>().navigation = memoriesNav;
        }
    }

    private void CacheTopBarNav()
    {
        CacheEntriesTopBarNav();
        CacheMemoriesTopBarNav();
        
        void CacheEntriesTopBarNav()
        {
            Navigation entriesNav = entriesTopBarButton.GetComponent<Selectable>().navigation;    
            entriesTopBarSelectOnDownButton = entriesNav.selectOnDown;
        }
        
        void CacheMemoriesTopBarNav()
        {
            Navigation memoriesNav = memoriesTopBarButton.GetComponent<Selectable>().navigation;    
            memoriesTopBarSelectOnDownButton = memoriesNav.selectOnDown;
        }
    }
    

    public void InitializeState(EventSystem eventSystem = null)
    {
        if (eventSystem != null)
        {
            eventSystem.SetSelectedGameObject(initialStateSelected);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(initialStateSelected);
        }

        InventoryState = InventoryStates.Overview;
    }

    public void Setup()
    {
        CacheTopBarNav();
        
        inputManager = GetComponent<Script_MenuInputManager>();

        inputManager.Setup();
        thoughtsController.Setup();
        SBookController.Setup();
        itemsController.Setup();
        entriesController.Setup();
        memoriesController.Setup();
        inventoryManager.Setup();

        isSBookDisabled = true;

        if (Debug.isDebugBuild && Const_Dev.IsDevMode)
        {
            Debug.Log("<b>SBook</b> enabled by default for debugging.");
            isSBookDisabled = false;
        }
    }
}
