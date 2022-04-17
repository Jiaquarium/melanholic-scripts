using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SystemSettings : MonoBehaviour
{
    [SerializeField] private int vSyncCount = 1;
    [SerializeField] private int targetFrameRate = 60;
    [SerializeField] private FullScreenMode currentScreenMode = 0;

    public static void DisableMouse()
    {
        Cursor.visible = false;
    }

    public void TargetFrameRate()
    {
        QualitySettings.vSyncCount = vSyncCount;
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
