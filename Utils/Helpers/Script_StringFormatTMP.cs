using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Formats TMP strings.
/// Auto-updates.
/// 
/// Set useDynamicDisplay to see formatting as dev'ing
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class Script_StringFormatTMP : MonoBehaviour
{
    /// <summary>
    /// Populate with the dynamic text field.
    /// </summary>
    [SerializeField] private bool useDynamicDisplay;
    [TextArea(3,10)] [SerializeField] private string dynamicText;
    
    public string DynamicText
    {
        get => dynamicText;
        set => dynamicText = value;
    }

    void Start()
    {
        string unformattedStr = GetComponent<TextMeshProUGUI>().text; 
        GetComponent<TextMeshProUGUI>().text = Script_Utils.FormatString(unformattedStr);
    }

    void OnValidate()
    {
        if (useDynamicDisplay)  DynamicDisplay();
    }

    void Update()
    {
        if (useDynamicDisplay)  DynamicDisplay();
        else                    FormatTMPText();
    }

    private void FormatTMPText()
    {
        string unformattedStr = GetComponent<TextMeshProUGUI>().text; 
        GetComponent<TextMeshProUGUI>().text = Script_Utils.FormatString(unformattedStr);   
    }

    private void DynamicDisplay()
    {
        GetComponent<TextMeshProUGUI>().text = Script_Utils.FormatString(dynamicText);
    }
}
