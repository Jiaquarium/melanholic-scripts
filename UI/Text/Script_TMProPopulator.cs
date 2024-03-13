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

    [Header("Dynamic String Opt-In")]
    /// <summary>
    /// Options to use dynamic rebind strings. Only opt-in when necessary because are expensive calls.
    /// </summary>
    [SerializeField] private bool isFormatInventoryKey;
    [SerializeField] private bool isFormatSpeedKey;
    [SerializeField] private bool isFormatMaskCommandKey;
    [SerializeField] private bool isFormatInteractKey;

    [Header("Alt Gamepad Text")]
    /// <summary>
    /// Specify an alternate Text ID to switch to if a gamepad is connected.
    /// </summary>
    [SerializeField] private bool isAltTextGamepadConnected;
    [SerializeField] private string altIdGamepadConnected;

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
        var myId = Id;
        var isForceControllerIdBuild = false;

        // Handle alt Text ID to switch to if gamepad is connected
        if (isAltTextGamepadConnected && !String.IsNullOrEmpty(altIdGamepadConnected))
        {
            Script_PlayerInputManager playerInput = Script_PlayerInputManager.Instance;

            if (playerInput != null && playerInput.IsJoystickConnected)
            {
                myId = altIdGamepadConnected;
                isForceControllerIdBuild = true;
            }
        }
        
        if (!String.IsNullOrEmpty(myId))
        {
            string unformatted = String.Empty;
            
            if (forceLang != Script_LocalizationUtils.LangEnum.Default)
            {
                string languageCode = forceLang.LangEnumToLang();
                unformatted = Script_UIText.Text[myId].GetProp<string>(languageCode);
            }
            else
            {
                unformatted = Script_LocalizationUtils.SwitchTextOnLang(
                    EN_text: Script_UIText.Text[myId].EN,
                    CN_text: Script_UIText.Text[myId].CN
                );
            }
            
            if (String.IsNullOrEmpty(unformatted))
                unformatted = string.Empty;

            string formatted = Script_Utils.FormatString(
                unformatted,
                isFormatInventoryKey: isFormatInventoryKey,
                isFormatSpeedKey: isFormatSpeedKey,
                isFormatMaskCommandKey: isFormatMaskCommandKey,
                isFormatInteractKey: isFormatInteractKey,
                isForceControllerIdBuild: isForceControllerIdBuild
            );
            
            GetComponent<TextMeshProUGUI>().text = formatted;
        }
    }
}
