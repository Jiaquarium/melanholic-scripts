using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This Input Manager works slightly different than the rest because it
/// is on its own GameObject that is turned on when Settings are shown
/// and off when Settings are hidden.
/// </summary>
public class Script_SettingsInputManager : MonoBehaviour
{
    [SerializeField] private Script_SettingsController settingsController;
    [SerializeField] private Script_SettingsSystemController systemController;
    
    public void HandleExitInput()
    {
        if (Script_PlayerInputManager.Instance.MyPlayerInput.actions[Const_KeyCodes.UICancel].WasPressedThisFrame())
        {
            switch (settingsController.state)
            {
                case (Script_SettingsController.States.Graphics):
                    systemController.HandleGraphicsBack();
                    break;
                default:
                    settingsController.Back();
                    break;
            }
        }
    }
}
