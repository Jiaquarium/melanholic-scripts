using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ResolutionManager : MonoBehaviour
{
    [SerializeField] private int displayIdx;
    private bool isFullScreen;
    
    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();    
    }
    
    void Awake()
    {
        PlayerPrefs.DeleteAll();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (Display.displays.Length > 1)
            displayIdx = GetCurrentDisplayIdx();
    }

    // Update is called once per frame
    void Update()
    {
        if (Screen.fullScreen && !isFullScreen)
        {
            SetFullScreenResolution();
            
            isFullScreen = true;
        }

        if (Display.displays.Length > 1)
        {
            HandleFullScreenDisplayChange();
        }

        if (!Screen.fullScreen && isFullScreen)
        {
            isFullScreen = false;
        }
    }

    // Set resolution the the current display. Unity defaults to setting
    // resolution of the main display which could be different than the
    // current display.
    private void SetFullScreenResolution()
    {
        DisplayInfo displayInfo = Screen.mainWindowDisplayInfo;
        Screen.SetResolution(displayInfo.width, displayInfo.height, fullscreen: true);
    }

    // If full screen and changed displays, then update new fullscreen resolution.
    private void HandleFullScreenDisplayChange()
    {
        int lastDisplayIdx = displayIdx;
        displayIdx = GetCurrentDisplayIdx();

        // Changed displays and is full screen
        if (displayIdx != lastDisplayIdx && Screen.fullScreen)
            SetFullScreenResolution();
    }

    private int GetCurrentDisplayIdx()
    {
        List<DisplayInfo> displayInfos = new List<DisplayInfo>();
        Screen.GetDisplayLayout(displayInfos);
        
        return displayInfos.IndexOf(Screen.mainWindowDisplayInfo);
    }
}