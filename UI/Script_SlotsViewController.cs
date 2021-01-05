using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Script_ExitViewInputManager))]
public class Script_SlotsViewController : MonoBehaviour
{
    public Transform slotsHolder;
    public Transform[] slots;
    [Space]
    public int lastSlotIndex;
    [SerializeField] protected Transform lastSelected;
    protected Script_ExitViewInputManager inputManager;

    public Transform GetSlotTransform(int i)
    {
        if (i < slots.Length)   return slots[i];
        else                    return null;
    }

    public Transform[] GetSlots()
    {
        return slots;
    }
    
    private void OnValidate() {
        UpdateSlots();    
    }
    
    // Update is called once per frame
    protected virtual void Update()
    {
        ShowActiveSlot();
        HandleExitInput();
    }

    protected virtual void HandleExitInput() {
        inputManager.HandleExitInput();
    }

    protected void ShowActiveSlot()
    {
        if (
            EventSystem.current.currentSelectedGameObject != null
            && lastSelected != null
            && lastSelected.gameObject != null
            && lastSelected.GetComponent<Script_Slot>() != null
            && EventSystem.current.currentSelectedGameObject != lastSelected.gameObject
        )
        {   
            SetLast();
        }
    }

    protected virtual void SetLast() {
        try
        {
            lastSelected = EventSystem.current.currentSelectedGameObject.transform;
            lastSlotIndex = lastSelected.GetComponent<Script_Slot>().Id;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"errored lastSelected was {lastSelected}");
        }
    }

    public void RehydrateState()
    {
        if (lastSelected == null)
        {
            Debug.Log("Can't rehydrate. Initializing Slots state.");
            InitializeState();
        }
        Debug.Log($"SlotsViewController: Rehydrating with lastSelected: {lastSelected}");
        EventSystem.current.SetSelectedGameObject(lastSelected.gameObject);
    }

    public virtual void InitializeState(int i = 0)
    {
        lastSlotIndex = i;
        lastSelected = slots[lastSlotIndex];
    }

    public virtual void UpdateSlots()
    {
        slots = slotsHolder.GetChildren<Transform>();
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].GetComponent<Script_Slot>() != null)
                slots[i].GetComponent<Script_Slot>().Id = i;
        }
    }

    public virtual void Setup()
    {
        UpdateSlots();

        // setup input manager here
        inputManager = GetComponent<Script_ExitViewInputManager>();
    }
}
