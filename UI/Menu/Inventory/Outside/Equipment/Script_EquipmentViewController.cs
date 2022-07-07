using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Should only be active when focused in on equipment view
/// </summary>
public class Script_EquipmentViewController : Script_SBookViewController
{
    [SerializeField] private Script_InventoryManager inventoryManager;
    [SerializeField] private Script_ItemDescription itemDescription;
    
    void OnDisable()
    {
        Debug.Log("EquipmentViewController OnDisable();");
        inventoryManager.SetItemDescription(itemDescription, false);
    }
    
    public override void Setup()
    {
        base.Setup();
        gameObject.SetActive(false);
    }
}
