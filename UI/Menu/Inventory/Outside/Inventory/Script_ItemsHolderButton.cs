using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Script_ItemsHolderButton : MonoBehaviour, ISelectHandler
{
    public Script_InventoryController inventoryController;

    public bool isAutoEnter;

    private bool isAutoEnteringInventoryView;

    void LateUpdate()
    {
        if (isAutoEnteringInventoryView)
        {
            inventoryController.EnterInventoryView();
            isAutoEnteringInventoryView = false;
        }
    }
    
    /// <summary>
    /// called from OnClick handler
    /// </summary>
    public void OnEnter()
    {
        if (isAutoEnter)
            return;
        
        inventoryController.EnterInventoryView();
    }

    /// <summary>
    /// Must wait until the end of the frame to Rehydrate into the Inventory View
    /// or Unity will error trying to select the same GameObject on the same frame.
    /// </summary>
    public virtual void OnSelect(BaseEventData e)
    {
        if (isAutoEnter)
            isAutoEnteringInventoryView = true;
    }
}
