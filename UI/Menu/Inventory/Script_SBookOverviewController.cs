using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// handles outside SBook's event system (current selected) state and for visiblity
/// of views
/// Sets EventSystemMainMenu inactive when we enter itemChoices were each set of choices
/// has their own EventSystem. This avoids us keeping track and rehydrating to unaccessible
/// elements
/// Their EventSystem handles setting which active first
/// </summary>
public class Script_SBookOverviewController : Script_InventoryController
{
    public GameObject SBookOutsideCanvas;
    public GameObject outsideSBookSelectOnDownFromTopBar;
    public GameObject[] outsideSBookBtnsToTrack;
    public Script_InventoryViewController inventoryViewController;
    public Script_EquipmentViewController equipmentViewController;
    public Script_MenuController inventoryController;
    [SerializeField] private Script_ItemChoices itemChoices;
    [SerializeField] private Script_EventSystemLastSelected myEventSystem;
    
    [Space]
    // The last button selected, either the SBook Cover Button or Stickers Holder Button.
    [SerializeField] private GameObject lastSelectedBeforeExit;

    
    private void Update() {
        if (inventoryController.InventoryState != Script_MenuController.InventoryStates.Overview)   return;
        
        SetOutsideLastSelected();
    }

    public void EnterEquipmentView()
    {
        myEventSystem.gameObject.SetActive(true);
        equipmentViewController.gameObject.SetActive(true);
        equipmentViewController.RehydrateState();

        inventoryController.ChangeStateToEquipmentView();
    }

    public void ExitEquipmentView()
    {
        equipmentViewController.gameObject.SetActive(false);
        inventoryController.ChangeStateToOverview();

        EventSystem.current.SetSelectedGameObject(lastSelectedBeforeExit);
    }

    public override void EnterInventoryView()
    {
        print("EnterInventoryView() rehydrating inventoryViewController and activating myEventSystem MAIN");
        myEventSystem.gameObject.SetActive(true);
        inventoryViewController.gameObject.SetActive(true);
        inventoryViewController.RehydrateState();

        inventoryController.ChangeStateToInventoryView();
    }

    public override void ExitInventoryView()
    {
        inventoryViewController.gameObject.SetActive(false);
        inventoryController.ChangeStateToOverview();
        print("setting lastSelectedBeforeExit: " + lastSelectedBeforeExit);
        
        EventSystem.current.SetSelectedGameObject(lastSelectedBeforeExit);
    }

    public void EnterItemChoices(Script_ItemChoices _itemChoices)
    {
        inventoryViewController.gameObject.SetActive(false);
        
        itemChoices = _itemChoices;
        itemChoices.gameObject.SetActive(true);

        EventSystem.current.gameObject.SetActive(false);
    }

    /// <summary>
    /// saves the slot we were on -- this object needs to be set inactive to keep the reference
    /// </summary>
    void SetOutsideLastSelected()
    {
        foreach (GameObject b in outsideSBookBtnsToTrack)
        {
            if (
                EventSystem.current.currentSelectedGameObject == b
                && outsideSBookSelectOnDownFromTopBar != b
            )
            {
                lastSelectedBeforeExit = b;
            }
        }
    }

    private void InitializeState()
    {
        SBookOutsideCanvas.SetActive(true);
        inventoryViewController.gameObject.SetActive(false);
        equipmentViewController.gameObject.SetActive(false);
    }

    public override void Setup()
    {
        InitializeState();
        inventoryViewController.Setup();
        equipmentViewController.Setup();
    }
}
