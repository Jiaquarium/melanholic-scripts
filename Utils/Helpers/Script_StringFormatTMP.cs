using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Formats TMP strings
/// 
/// Set useDynamicDisplay to see formatting as dev'ing
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class Script_StringFormatTMP : MonoBehaviour
{
    [SerializeField] private bool useDynamicDisplay;
    [TextArea(3,10)]
    [SerializeField] private string dynamicText;
    
    void Start()
    {
        string unformattedStr = GetComponent<TextMeshProUGUI>().text; 
        GetComponent<TextMeshProUGUI>().text = Script_Utils.FormatString(unformattedStr);
    }

    void OnValidate()
    {
        if (useDynamicDisplay)
        {
            GetComponent<TextMeshProUGUI>().text = Script_Utils.FormatString(dynamicText);
        }
    }
}
