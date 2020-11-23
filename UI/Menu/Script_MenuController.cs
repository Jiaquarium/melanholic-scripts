using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles overall inventory inventoryState tracking and settings
/// does not handle visibility
/// 
/// Keeps track of selected based on their UIId
/// </summary>
public class Script_MenuController : Script_UIState
{
    
    public TopBarStates topBarState;
    public enum TopBarStates{
        thoughts,
        inventory,
        entries
    }
    public string inventoryState;
    public bool isSBookDisabled;
    public GameObject SBookOverviewButton;
    public GameObject initialStateSelected;
    public Script_CanvasGroupController_Thoughts thoughtsController;
    public Script_SBookOverviewController SBookController;
    [SerializeField] private Script_EntriesController entriesController;
    public Script_InventoryViewSettings inventorySettings;
    public Script_EntriesViewSettings entriesSettings;
    public Script_InventoryOverviewSettings inventoryOverviewSettings;
    
    [SerializeField] private Script_MenuInputManager inputManager;
    [SerializeField] private string newlySelected;
    [SerializeField] private string currentSelected;
    [SerializeField] private Script_InventoryManager inventoryManager;
    // [SerializeField] private AudioSource submenuEnterAudioSource;


    void OnEnable()
    {
        ChangeTopBarState(topBarState);
    }
    
    void Update()
    {        
        if (string.IsNullOrEmpty(inventoryState))
        {
            InitializeState();
        }
        else if (inventoryState == Const_States_InventoryOverview.Overview)
        {
            HandleActiveButton();
            ShowActivePanel();
            inputManager.HandleExitInput();
        }
    }

    public void ChangeStateToOverview()
    {
        inventoryState = Const_States_InventoryOverview.Overview;
        ChangeRepeatDelay(inventoryOverviewSettings.repeatDelay, inventoryOverviewSettings.inputActionsPerSecond);
    }

    public void ChangeStateToInventoryView()
    {
        Debug.Log($"MenuController inventoryState before stickers view: {inventoryState}");
        inventoryState = Const_States_InventoryOverview.InventoryView;
        ChangeRepeatDelay(inventorySettings.repeatDelay, inventorySettings.inputActionsPerSecond);
    }

    public void ChangeStateToEquipmentView()
    {
        inventoryState = Const_States_InventoryOverview.EquipmentView;
        ChangeRepeatDelay(inventorySettings.repeatDelay, inventorySettings.inputActionsPerSecond);
    }

    public void ChangeStateToEntriesView()
    {
        inventoryState = Const_States_InventoryOverview.EntriesView;
        ChangeRepeatDelay(entriesSettings.repeatDelay, entriesSettings.inputActionsPerSecond);
    }

    void HandleActiveButton()
    {    
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            Script_ButtonMetadata lastSelectedBtn = EventSystem
                .current.currentSelectedGameObject
                .GetComponent<Script_ButtonMetadata>();
            
            // TODO: remove Script_ButtonMetadata, too confusing...
            if (lastSelectedBtn != null)
            {
                newlySelected = lastSelectedBtn.UIId;
                // ChangeRepeatDelay(lastSelectedBtn.repeatDelay, lastSelectedBtn.inputActionsPerSecond);
            }
            else
            {
                Debug.LogError($"Trying to update MenuController inventoryState to "
                    + EventSystem.current.currentSelectedGameObject.name
                    + ". You're probably missing a Script_ButtonMetadata on a menu button.");
            }
        }
    }

    /// <summary>
    /// TODO: make this cleaner, REMOVE BUTTON METADATA, way too confusing...
    /// Currently the top bar state is set by checking the UIIds of which
    /// last top bar Id was selected last; the state is saved so we know
    /// which top bar element we're on
    /// </summary>
    void ShowActivePanel()
    {
        // if (newlySelected == Script_UIIds.Thoughts || topBarState == TopBarStates.thoughts)
        // {
        //     currentSelected = Script_UIIds.Thoughts;
        //     thoughtsController.Open();
        //     SBookController.Close();
            
        //     topBarState = TopBarStates.thoughts;
        // }
        // else if (newlySelected == Script_UIIds.SBook || topBarState == TopBarStates.inventory)
        // {
        //     currentSelected = Script_UIIds.SBook;
        //     SBookController.Open();
        //     thoughtsController.Close();

        //     topBarState = TopBarStates.inventory;
        // }
    }

    public void ChangeTopBarState(TopBarStates state)
    {
        switch(state)
        {
            case TopBarStates.thoughts:
                topBarState = TopBarStates.thoughts;

                currentSelected = Script_UIIds.Thoughts;
                thoughtsController.Open();
                SBookController.Close();
                entriesController.Close();

                break;
            case TopBarStates.inventory:
                topBarState = TopBarStates.inventory;

                currentSelected = Script_UIIds.SBook;
                SBookController.Open();
                thoughtsController.Close();
                entriesController.Close();

                break;
            case TopBarStates.entries:
                topBarState = TopBarStates.entries;

                currentSelected = Script_UIIds.Entries;
                entriesController.Open();
                SBookController.Close();
                thoughtsController.Close();

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
    // public Script_Inventory GetInventory()
    // {
    //     return inventoryManager.GetInventory();
    // }

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

    public Script_ItemObject InstantiateDropById(string itemId, Vector3 location, int LB)
    {
        return inventoryManager.InstantiateDropById(itemId, location, LB);
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

        inventoryState = Const_States_InventoryOverview.Overview;
    }

    public void Setup()
    {
        inputManager = GetComponent<Script_MenuInputManager>();

        inputManager.Setup();
        thoughtsController.Setup();
        SBookController.Setup();
        entriesController.Setup();
        inventoryManager.Setup();

        isSBookDisabled = true;

        if (Debug.isDebugBuild && Const_Dev.IsDevMode)
        {
            Debug.Log("<b>SBook</b> enabled by default for debugging.");
            isSBookDisabled = false;
        }
    }
}
