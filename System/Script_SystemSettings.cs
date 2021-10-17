using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SystemSettings : MonoBehaviour
{
    public static readonly int FrameRate = 60;

    [SerializeField] private FullScreenMode currentScreenMode = 0;

    public static void TargetFrameRate()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = FrameRate;
    }
    
    public static void SetScreenSettings()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }

    public static void DisableMouse()
    {
        Cursor.visible = false;
    }

    void Start()
    {
        if (Const_Dev.IsDevMode)
            Screen.fullScreenMode = currentScreenMode;
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
