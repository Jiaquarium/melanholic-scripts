using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Script_SettingsScreenshakeChoice : Script_SettingsRadioChoice
{
    [SerializeField] private bool isDisableScreenshake;

    private bool IsDisableScreenshake
    {
        get => isDisableScreenshake;
        set => isDisableScreenshake = value;
    }

    public override void OnSelect(BaseEventData e)
    {
        // Prevent setting when first entering the submenu
        if (Script_SettingsSystemController.IsScreenshakeDisabled != IsDisableScreenshake)
        {
            Dev_Logger.Debug($"Setting IsScreenshakeDisabled: {IsDisableScreenshake}");
            Script_SettingsSystemController.IsScreenshakeDisabled = IsDisableScreenshake;
            settingsSystemController.SubmitSFX();
        }
    }
}
