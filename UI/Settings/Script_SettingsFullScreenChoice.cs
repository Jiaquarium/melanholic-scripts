using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Note, this only supports 2 choices, On / Off state
/// </summary>
public class Script_SettingsFullScreenChoice : Script_SettingsRadioChoice
{
    [SerializeField] private bool isFullScreen;
    [SerializeField] private Script_SettingsFullScreenChoice otherChoice;

    public override void OnSelect(BaseEventData e)
    {
        bool isCurrentlyFullScreen = Screen.fullScreen;
        if (isCurrentlyFullScreen != isFullScreen)
        {
            // Change FS mode
            Screen.fullScreen = isFullScreen;

            // Disable nav for a bit
            settingsSystemController.EnableNavigation(false);
            StartCoroutine(WaitToReenableNavigation());
        }

        IEnumerator WaitToReenableNavigation()
        {
            yield return null;
            
            // Only do SFX if Screen.fullScreen was successfully switched. Otherwise, set active object back to the other button.
            if (Screen.fullScreen == isFullScreen)
                settingsSystemController.SubmitSFX();
            else
                EventSystem.current.SetSelectedGameObject(otherChoice.gameObject);

            yield return new WaitForSecondsRealtime(Script_SettingsSystemController.WaitAfterFullScreenSwitchTime);
            
            settingsSystemController.EnableNavigation(true);
        }
    }
}
