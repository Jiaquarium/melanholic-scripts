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
        var rewiredInput = Script_PlayerInputManager.Instance.RewiredInput;
        
        if (
            rewiredInput.GetButtonDown(Const_KeyCodes.RWUICancel)
            || rewiredInput.GetButtonDown(Const_KeyCodes.RWUnknownControllerSettings)
        )
        {
            switch (settingsController.state)
            {
                case (Script_SettingsController.States.System):
                    systemController.HandleSystemSubmenuEscBack();
                    break;
                default:
                    settingsController.Back();
                    break;
            }
        }
    }
}
