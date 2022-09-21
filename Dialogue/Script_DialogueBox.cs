using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Script_DialogueBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] TMPs;
    
    public void Clear() {
        Dev_Logger.Debug("<><><><><><><><><><><> Clearing all TMP children on enable <><><><><><><><><><><>");
        foreach (TextMeshProUGUI TMP in TMPs)
        {
            TMP.text = string.Empty;
        }
    }
    
    void OnValidate()
    {
        TMPs = transform.GetChildren<TextMeshProUGUI>();
        if (TMPs.Length == 0)   Debug.LogError("Must have at least 1 TMP child;");
    }

    public string GetText()
    {
        string concatedTextOfTMPs = "";
        
        foreach (TextMeshProUGUI TMP in TMPs)
        {
            concatedTextOfTMPs += TMP.text;
        }

        return concatedTextOfTMPs;
    }
}
