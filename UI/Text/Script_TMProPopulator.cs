using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Script_TMProPopulator : MonoBehaviour
{
    public string Id;
    
    void OnValidate()
    {
        HandlePopulateById();
    }

    void Awake()
    {
        HandlePopulateById();
    }

    private void HandlePopulateById()
    {
        if (!String.IsNullOrEmpty(Id))
        {
            string unformatted = Script_UIText.Text[Id].EN;
            string formatted = Script_Utils.FormatString(unformatted);
            
            GetComponent<TextMeshProUGUI>().text = formatted;
        }
    }
}
