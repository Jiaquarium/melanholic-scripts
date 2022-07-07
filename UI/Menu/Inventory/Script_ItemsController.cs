using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Defines the reactions to the general use Script_InventoryInputManager.
/// </summary>
public class Script_ItemsController : Script_InventoryController
{
    [SerializeField] private Script_MenuController inventoryController;
    [SerializeField] private Script_InventoryViewController itemsViewController;

    [SerializeField] private Script_ItemChoices itemChoices;
    
    [SerializeField] private Script_EventSystemLastSelected myEventSystem;
    
    // Button to select when exiting Items holder.
    [SerializeField] private GameObject onExitInventorySelectedGameObject;

    public override void EnterInventoryView()
    {
        print("EnterInventoryView() rehydrating itemsViewController and activating myEventSystem MAIN");
        myEventSystem.gameObject.SetActive(true);
        itemsViewController.gameObject.SetActive(true);
        itemsViewController.RehydrateState();

        inventoryController.ChangeStateToItemsView();
    }

    public override void ExitInventoryView()
    {
        itemsViewController.gameObject.SetActive(false);
        inventoryController.ChangeStateToOverview();

        EventSystem.current.SetSelectedGameObject(onExitInventorySelectedGameObject);
    }

    public void EnterItemChoices(Script_ItemChoices _itemChoices)
    {
        itemsViewController.gameObject.SetActive(false);
        
        // Enter Inventory will reset to active. This must come before activating
        // the next Event System to avoid 2 Event System warning.
        EventSystem.current.gameObject.SetActive(false);
        
        itemChoices = _itemChoices;
        itemChoices.gameObject.SetActive(true);

        // Reactivating the same item choices from a different item, will not trigger First Selected
        // on Event System, so do it manually.
        itemChoices.SetFirstChoiceActive();
    }

    /// <summary>
    /// Handle input from CollectibleInventoryHandler
    /// </summary>
    public void EnterFullArt()
    {
        myEventSystem.gameObject.SetActive(true);
        EventSystem.current.sendNavigationEvents = false;
        itemsViewController.gameObject.SetActive(false);
    }
    
    public void ExitFullArt()
    {
        EventSystem.current.sendNavigationEvents = true;
        itemsViewController.gameObject.SetActive(true);

        EnterInventoryView();
    }

    private void InitializeState()
    {
        itemsViewController.gameObject.SetActive(false);
    }

    public override void Setup()
    {
        InitializeState();
        itemsViewController.Setup();
    }
}
