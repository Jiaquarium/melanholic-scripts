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
public class Script_SBookOverviewController : Script_CanvasGroupController
{
    public GameObject SBookOutsideCanvas;
    public GameObject SBookInsideCanvas;
    public GameObject insideSBookInitialStateSelected;
    public GameObject outsideSBookInitialStateSelected;
    public GameObject topBarButtonToChangeNav;
    public GameObject insideSBookSelectOnDownFromTopBar;
    public GameObject outsideSBookSelectOnDownFromTopBar;
    public GameObject[] insideSBookBtnsToTrack;
    public GameObject[] outsideSBookBtnsToTrack;
    public Script_InventoryViewController inventoryViewController;
    public Script_EquipmentViewController equipmentViewController;
    public Script_MenuController inventoryController;
    public Script_EntriesViewController entriesViewController;
    [SerializeField] private Script_ItemChoices itemChoices;
    [SerializeField] private Script_EventSystemLastSelected myEventSystem;
    [Space]
    [SerializeField] private bool isInsideSBook;
    [SerializeField] private GameObject lastSelectedBeforeExit;

    
    private void Update() {
        if (inventoryController.inventoryState != Const_States_InventoryOverview.Overview)   return;
        
        // if (isInsideSBook)
        // {
        //     SetInsideLastSelected();
        // }
        // else
        // {
        //     SetOutsideLastSelected();
        // }

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

    public void EnterInventoryView()
    {
        print("EnterInventoryView() rehydrating inventoryViewController and activating myEventSystem MAIN");
        myEventSystem.gameObject.SetActive(true);
        inventoryViewController.gameObject.SetActive(true);
        inventoryViewController.RehydrateState();

        inventoryController.ChangeStateToInventoryView();
    }

    public void ExitInventoryView()
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
    /// handle input from CollectibleInventoryHandler
    /// </summary>
    public void EnterFullArt()
    {
        myEventSystem.gameObject.SetActive(true);
        EventSystem.current.sendNavigationEvents = false;
        inventoryViewController.gameObject.SetActive(false);
    }
    public void ExitFullArt()
    {
        EventSystem.current.sendNavigationEvents = true;
        inventoryViewController.gameObject.SetActive(true);

        EnterInventoryView();
    }

    /// <summary> =========================================================================
    /// INSIDE S-BOOK FUNCS
    /// </summary> ========================================================================
    // public void EnterEntriesView()
    // {
    //     entriesViewController.gameObject.SetActive(true);
    //     entriesViewController.RehydrateState();
        
    //     inventoryController.ChangeStateToEntriesView();
    // }
    

    public void ExitEntriesView()
    {
        inventoryController.ChangeStateToOverview();
        entriesViewController.gameObject.SetActive(false);

        EventSystem.current.SetSelectedGameObject(lastSelectedBeforeExit);
    }
    
    public void EnterSBook()
    {
        // SetOverviewNavigationInsideSBook();
        
        SBookInsideCanvas.SetActive(true);
        SBookOutsideCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(insideSBookInitialStateSelected);
        isInsideSBook = true;
    }

    public void ExitSBook()
    {
        // SetOverviewNavigationOutsideSBook();

        SBookInsideCanvas.SetActive(false);
        SBookOutsideCanvas.SetActive(true);

        EventSystem.current.SetSelectedGameObject(outsideSBookInitialStateSelected);
        isInsideSBook = false;
    }

    /// <summary>
    /// Set the explicit movement from topBar (changes if on inside/outside SBook)
    /// </summary>
    // void SetOverviewNavigationInsideSBook()
    // {
    //     Navigation btnNav = topBarButtonToChangeNav.GetComponent<Selectable>().navigation;
    //     btnNav.selectOnDown = insideSBookSelectOnDownFromTopBar.GetComponent<Button>();
    //     topBarButtonToChangeNav.GetComponent<Selectable>().navigation = btnNav;
    // }
    // void SetOverviewNavigationOutsideSBook()
    // {
    //     Navigation btnNav = topBarButtonToChangeNav.GetComponent<Selectable>().navigation;
    //     btnNav.selectOnDown = outsideSBookSelectOnDownFromTopBar.GetComponent<Button>();
    //     topBarButtonToChangeNav.GetComponent<Selectable>().navigation = btnNav;
    // }

    // void SetInsideLastSelected()
    // {
    //     foreach (GameObject b in insideSBookBtnsToTrack)
    //     {
    //         if (
    //             EventSystem.current.currentSelectedGameObject == b
    //             && insideSBookSelectOnDownFromTopBar != b
    //         )
    //         {
    //             lastSelectedBeforeExit = b;
    //             // insideSBookSelectOnDownFromTopBar = b;
    //             SetOverviewNavigationInsideSBook();
    //         }
    //     }
    // }

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
                // was only needed w/o openCloseButton, which now contains this switching nav logic 
                // outsideSBookSelectOnDownFromTopBar = b;
                // SetOverviewNavigationOutsideSBook();
            }
        }
    }

    void InitializeState()
    {
        // SetOverviewNavigationOutsideSBook();

        // SBookInsideCanvas.SetActive(false);
        SBookOutsideCanvas.SetActive(true);
        isInsideSBook = false;
        inventoryViewController.gameObject.SetActive(false);
        equipmentViewController.gameObject.SetActive(false);
        entriesViewController.gameObject.SetActive(false);
    }

    public override void Setup()
    {
        InitializeState();
        inventoryViewController.Setup();
        equipmentViewController.Setup();
        entriesViewController.Setup();
    }
}
