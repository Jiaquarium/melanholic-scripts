using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// We default startup with Exclusive Fullscreen (Mac). This will default to Fullscreen Windowed on other OSs.
/// </summary>
public class Script_SystemSettings : MonoBehaviour
{
    private static int vSyncCount = 1;
    private static int targetFrameRate = 60;
    [SerializeField] private FullScreenMode currentScreenMode = 0;

    public static void DisableMouse()
    {
        Cursor.visible = false;
    }

    public void TargetFrameRate()
    {
        QualitySettings.vSyncCount = vSyncCount;
        
        // Unity ignores the value of targetFrameRate if you set vSyncCount.
        Application.targetFrameRate = targetFrameRate;
    }
    
    public void SetScreenSettings()
    {
        // Screen.fullScreenMode = currentScreenMode;
    }

    void Update()
    {
        if (!Const_Dev.IsDevMode)
            return;
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            currentScreenMode += 1;
            if (!FullScreenMode.IsDefined(typeof(FullScreenMode), currentScreenMode))
                currentScreenMode = 0;
            
            Screen.fullScreenMode = currentScreenMode;
        }
    }
}
