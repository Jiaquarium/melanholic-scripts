using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SettingsFullScreenChoice : MonoBehaviour
{
    [SerializeField] private bool isFullScreen;
    [SerializeField] private Script_SettingsSystemController settingsSystemController;

    public bool IsFullScreen => isFullScreen;

    // Called from Button OnClick while in Settings > System > Fullscreen
    public void OnClick()
    {
        settingsSystemController.SubmitSFX();
        
        // Change FS mode
        Screen.fullScreen = IsFullScreen;

        // Disable nav for a bit
        settingsSystemController.EnableNavigation(false);
        StartCoroutine(WaitToReenableNavigation());

        IEnumerator WaitToReenableNavigation()
        {
            yield return new WaitForSecondsRealtime(Script_SettingsSystemController.WaitAfterFullScreenSwitchTime);
            settingsSystemController.EnableNavigation(true);
        }
    }
}
