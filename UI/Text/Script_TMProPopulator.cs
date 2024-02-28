using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Script_TMProPopulator : MonoBehaviour
{
    public string Id;
    [SerializeField] private Script_LocalizationUtils.LangEnum forceLang;
    
#if UNITY_EDITOR
    // Revert any state changes back to default EN to clean up Editor View
    void OnApplicationQuit()
    {
        Script_Game.ChangeLangToEN();
        HandlePopulateById();
    }   
#endif
    
    void OnValidate()
    {
        HandlePopulateById();
    }

    void Awake()
    {
        HandlePopulateById();
    }

    void OnEnable()
    {
        HandlePopulateById();
    }

    public void UpdateTextId(string _Id)
    {
        Id = _Id;
        HandlePopulateById();
    }

    private void HandlePopulateById()
    {
        if (!String.IsNullOrEmpty(Id))
        {
            string unformatted = String.Empty;
            
            if (forceLang != Script_LocalizationUtils.LangEnum.Default)
            {
                string languageCode = forceLang.LangEnumToLang();
                unformatted = Script_UIText.Text[Id].GetProp<string>(languageCode);
            }
            else
            {
                unformatted = Script_LocalizationUtils.SwitchTextOnLang(
                    EN_text: Script_UIText.Text[Id].EN,
                    CN_text: Script_UIText.Text[Id].CN
                );
            }
            
            if (String.IsNullOrEmpty(unformatted))
                unformatted = string.Empty;

            string formatted = Script_Utils.FormatString(unformatted);
            
            GetComponent<TextMeshProUGUI>().text = formatted;
        }
    }
}
