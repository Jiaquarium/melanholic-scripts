using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Input Manager works slightly different than the rest because it
/// is on its own GameObject that is turned on when Settings are shown
/// and off when Settings are hidden.
/// </summary>
public class Script_SettingsInputManager : MonoBehaviour
{
    [SerializeField] private Script_SettingsController settingsController;
    
    public void HandleExitInput()
    {
        if (Script_PlayerInputManager.Instance.MyPlayerInput.actions[Const_KeyCodes.UICancel].WasPressedThisFrame())
        {
            settingsController.Back();
        }
    }
}
