using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Script_Slot))]
public class Script_ItemHighlight : MonoBehaviour
{
    [SerializeField] private Script_InventoryManager.Types inventoryType;
    [SerializeField] private bool withItemDescription;
    
    public void HighlightAndShowDescription(
        bool isOn
    )
    {
        Dev_Logger.Debug($"HighlightAndShowDescription({isOn}, {withItemDescription}, {inventoryType})");

        int myId = GetComponent<Script_Slot>().Id;
        Script_Game.Game.HighlightItem(myId, isOn, withItemDescription, inventoryType);
    }
}
