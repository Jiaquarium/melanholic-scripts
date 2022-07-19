using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// UI element to display Script_Item.Description in the Menu
/// Should show when hovering over an item in the inventory (NOT IN EQUIPMENT)
/// </summary>
public class Script_ItemDescription : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI NameTMP;
    [SerializeField] private TextMeshProUGUI DescriptionTMP;
    public static string nullDescription = "";
    
    public string Name
    {
        get { return NameTMP.text; }
        set {
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogWarning(
                    $"You are trying to pass a null name for an item description. Items should have names."
                );
            }
            NameTMP.text = Script_Utils.FormatString(value);
        }
    }

    public string Text
    {
        get { return DescriptionTMP.text; }
        set {
            if (string.IsNullOrEmpty(value))
                DescriptionTMP.text = nullDescription;
            else
                DescriptionTMP.text = Script_Utils.FormatString(
                    value,
                    isFormatInventoryKey: true,
                    isFormatSpeedKey: true
                );
        }
    }
}
