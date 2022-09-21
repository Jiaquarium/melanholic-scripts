using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Should only be active when focused in on inventory view
/// </summary>
public class Script_InventoryViewController : Script_SBookViewController
{
    [SerializeField] private Script_InventoryManager inventoryManager;
    [SerializeField] private Script_ItemDescription itemDescription;

    void OnDisable()
    {
        Dev_Logger.Debug("InventoryViewController OnDisable();");
        inventoryManager.SetItemDescription(itemDescription, false);
    }

    public override void Setup()
    {
        base.Setup();
        gameObject.SetActive(false);
        inventoryManager.SetItemDescription(itemDescription, false);
    }
}
