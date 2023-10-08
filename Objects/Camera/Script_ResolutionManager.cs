using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Resolution Manager will not run until:
///     1. WINDOWS: A few frames into Simple Intro Timeline; MAC: on play Simple Intro Timeline
///     2. Skipping the Simple Intro Timeline
///     3. Immediately on Game scene
///     4. Immediately upon returning back to Start from Game scene
/// </summary>
public class Script_ResolutionManager : MonoBehaviour
{
    static private int Interval = 4;
    
    [SerializeField] private int displayIdx;
    private bool isFullScreen;
    
    private bool isRanOnFirstAvailableFrame;

    void OnApplicationQuit()
    {
        
    }
    
    void Awake()
    {
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (Display.displays.Length > 1)
            displayIdx = Script_Utils.GetCurrentDisplayIdx();
    }

    // Update is called once per frame
    void Update()
    {
        // Ensure to do fullscreen resolution fix on first frame and can then throttle after
        if (!isRanOnFirstAvailableFrame)
        {
            HandleFixingFullScreenResolution();
            isRanOnFirstAvailableFrame = true;
            return;
        }
        
        // Then throttle after, since these calls can be expensive
        if (Time.frameCount % Interval != 0)
            return;
        
        HandleFixingFullScreenResolution();
    }

    // Fix if maximized at a wrong resolution due to varying OS maximizing behaviors
    private void HandleFixingFullScreenResolution()
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
#if UNITY_STANDALONE_WIN
        Script_Utils.SetFullScreenOnWindows(isAlreadyFullScreen: true);
#endif

#if UNITY_STANDALONE_OSX
        Script_Utils.SetFullScreenOnMac(
            Script_GraphicsManager.TargetAspectStatic, isAlreadyFullScreen: true
        );
#endif
    }

    // If full screen and changed displays, then update new fullscreen resolution.
    private void HandleFullScreenDisplayChange()
    {
        int lastDisplayIdx = displayIdx;
        displayIdx = Script_Utils.GetCurrentDisplayIdx();

        // Changed displays and is full screen
        if (displayIdx != lastDisplayIdx && Screen.fullScreen)
            SetFullScreenResolution();
    }
}
