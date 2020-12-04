using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Write {0} to replace with Runs data
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class Script_RunsTMPFormatter : MonoBehaviour
{
    [TextArea(3,10)]
    [SerializeField] private string dynamicText;
    
    void Start()
    {
        UpdateRunsText();
    }

    void OnValidate()
    {
        UpdateRunsText();
    }

    void Update()
    {
        UpdateRunsText();
    }

    private void UpdateRunsText()
    {
        string runsText = string.Format(
            dynamicText,
            Script_Game.Game.Run
        );
        GetComponent<TextMeshProUGUI>().text = runsText;    
    }
}
