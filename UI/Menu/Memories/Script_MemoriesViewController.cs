using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Very similar to EntriesViewController
/// 
/// Initialize is called by higher level controller
/// </summary>
public class Script_MemoriesViewController : Script_SlotsViewController
{
    [SerializeField] private Script_MenuController menuController;

    [SerializeField] private Transform noMemoriesView;
    [SerializeField] private Transform memoriesView;
    [SerializeField] private Transform memoryDetailView;
    [SerializeField] private Transform memoryDetailNoneSelectedView;
    [SerializeField] private Transform memoryDetailSelectedView;
    
    
    private void OnEnable()
    {
        Setup();
    }
    
    public void EnterEntriesView()
    {
        RehydrateState();
        gameObject.SetActive(true);

        menuController.ChangeStateToEntriesView();
    }

    public void ExitEntriesView()
    {
        gameObject.SetActive(false);
        menuController.ChangeStateToOverview();

        EventSystem.current.SetSelectedGameObject(memoriesView.gameObject);
    }
    
    public void OnEntrySelect(string text)
    {
        memoryDetailSelectedView.GetComponent<Script_EntryDetailView>().SetText(text);
        Dev_Logger.Debug($"Set entry detail text: {text}.");
        
        ShowEntryDetail();
    }

    public void UpdateCanvasState()
    {
        UpdateSlots();
        
        if (slotsHolder.childCount == 0)
        {
            noMemoriesView.gameObject.SetActive(true);
            memoriesView.gameObject.SetActive(false);
            memoryDetailView.gameObject.SetActive(false);
            Dev_Logger.Debug("memoryDetailView set inactive UpdateCanvasState()");
        }
        else
        {
            memoriesView.gameObject.SetActive(true);
            noMemoriesView.gameObject.SetActive(false);
        }
    }

    void NoneSelectedState()
    {
        memoryDetailNoneSelectedView.gameObject.SetActive(true);
        memoryDetailSelectedView.gameObject.SetActive(false);

        Dev_Logger.Debug("HideEntryDetauk() Hiding entriesDetailSelectedView");
    }

    void ShowEntryDetail()
    {
        Dev_Logger.Debug("Show entry detail after setting text.");
        
        memoryDetailSelectedView.gameObject.SetActive(true);
        memoryDetailNoneSelectedView.gameObject.SetActive(false);
        memoryDetailView.gameObject.SetActive(true);
    }

    /// <summary>
    /// Should be only called once on init by higher level controller
    /// </summary>
    public void InitializeState()
    {
        gameObject.SetActive(false);   
        if (lastSelected == null)   NoneSelectedState();
    }
    
    public override void Setup()
    {
        UpdateCanvasState();
        base.Setup();
    }
}
