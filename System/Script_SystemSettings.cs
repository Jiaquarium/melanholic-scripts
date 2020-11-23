using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SystemSettings : MonoBehaviour
{
    public static readonly int FrameRate = 60;

    public static void TargetFrameRate()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = FrameRate;
    }
    
    public static void FullScreen()
    {
        Screen.fullScreen = true;
        Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
    }
    
    public static void DisableMouse()
    {
        // if (!Debug.isDebugBuild)
        // {
            // Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        // }
    }

    void OnGUI() {
        int w = Screen.width, h = Screen.height;
 
		GUIStyle style = new GUIStyle();
 
		Rect rect = new Rect(0, 0, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = new Color (255f, 255f, 255f, 1.0f);

        GUI.Label(rect, $"cursor visible: {Cursor.visible}", style);
    }
}
