using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles the entries slots view
/// </summary>
public class Script_EntriesViewController : Script_SlotsViewController
{
    [SerializeField] private Script_MenuController menuController;

    [SerializeField] private Transform noEntriesView;
    [SerializeField] private Transform entriesView;
    [SerializeField] private Transform entriesDetailView;
    [SerializeField] private Transform entriesHolder;
    [SerializeField] private Transform entryDetailNoneSelectedView;
    [SerializeField] private Transform entryDetailSelectedView;
    
    public void EnterEntriesView()
    {
        RehydrateState();
        
        menuController.ChangeStateToEntriesView();
    }

    public void ExitEntriesView()
    {
        menuController.ChangeStateToOverview();

        EventSystem.current.SetSelectedGameObject(entriesView.gameObject);
    }
    
    public void OnEntrySelect(string text)
    {
        entryDetailSelectedView.GetComponent<Script_EntryDetailView>().SetText(text);
        Debug.Log($"Set entry detail text: {text}.");
        
        ShowEntryDetail();
    }

    protected override void SetLast()
    {
        base.SetLast();
        lastSlotIndex = lastSelected.GetComponent<Script_Entry>().Id;
    }

    public void UpdateCanvasState()
    {
        UpdateSlots();
        
        if (entriesHolder.childCount == 0)
        {
            noEntriesView.gameObject.SetActive(true);
            entriesView.gameObject.SetActive(false);
            entriesDetailView.gameObject.SetActive(false);
            Debug.Log("entriesDetailView set inactive UpdateCanvasState()");
        }
        else
        {
            entriesView.gameObject.SetActive(true);
            noEntriesView.gameObject.SetActive(false);
        }
    }

    void HideEntryDetail()
    {
        entryDetailNoneSelectedView.gameObject.SetActive(true);
        entryDetailSelectedView.gameObject.SetActive(false);

        Debug.Log("HideEntryDetauk() Hiding entriesDetailSelectedView");
    }

    void ShowEntryDetail()
    {
        Debug.Log("Show entry detail after setting text.");
        
        entryDetailSelectedView.gameObject.SetActive(true);
        entryDetailNoneSelectedView.gameObject.SetActive(false);
        entriesDetailView.gameObject.SetActive(true);
    }
    
    private void Awake()
    {
        
    }
    
    public override void Setup()
    {
        UpdateCanvasState();
        Debug.Log("Hide entry detail in Setup()");
        HideEntryDetail();
        base.Setup();
        // InitializeState();
    }
}
