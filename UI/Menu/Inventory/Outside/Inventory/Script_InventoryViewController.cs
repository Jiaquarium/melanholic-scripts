using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Should only be active when focused in on inventory view
/// </summary>
public class Script_InventoryViewController : Script_SBookViewController
{
    [SerializeField] private Script_ItemDescription itemDescription;
    [SerializeField] private Script_InventoryManager inventoryManager;

    void OnDisable()
    {
        Debug.Log("InventoryViewController OnDisable();");
        itemDescription.gameObject.SetActive(false);
    }

    public void HandleItemDescription(Script_Item item)
    {
        if (item == null)
        {
            itemDescription.gameObject.SetActive(false);
            return;
        }
        
        itemDescription.Name = item.name;
        itemDescription.Text = item.description;
        itemDescription.gameObject.SetActive(true);
    }

    public override void Setup()
    {
        base.Setup();
        gameObject.SetActive(false);
        itemDescription.gameObject.SetActive(false);
    }
}
