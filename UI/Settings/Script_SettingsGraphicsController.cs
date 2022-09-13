using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class Script_SettingsGraphicsController : MonoBehaviour
{
    public enum GraphicsStates
    {
        Overview = 0,
        FullScreenMode = 1,
        Resolutions = 2,
    }

    public GraphicsStates graphicsState;
    [SerializeField] private GameObject firstSelected;
    [SerializeField] private GameObject fullScreenChoicesFirstSelected;
    [SerializeField] private GameObject onExitFullScreenChoicesFirstSelected;

    [SerializeField] private List<Button> fullScreenButtons;
    [SerializeField] private List<Image> screenButtonHighlightsLight;

    [SerializeField] private GameObject resolutionsFirstSelected;
    [SerializeField] private GameObject onExitResolutionsFirstSelected;

    [SerializeField] private GameObject resolutionsHolder;
    [SerializeField] private List<Script_SettingsResolutionChoice> resolutions;
    
    [SerializeField] private Script_SettingsController settingsController;

    void OnValidate()
    {
        PopulateResolutionChoices();
    }
    
    void OnEnable()
    {
        PopulateResolutionChoices();
        HandleDisabledResolutions();
        HandleFullScreenHighlight();
    }
    
    void Update()
    {
        if (graphicsState != GraphicsStates.FullScreenMode)
            HandleFullScreenHighlight();
        
        if (graphicsState != GraphicsStates.Resolutions)
            HandleDisabledResolutions();
    }
    
    public void ToGraphics()
    {
        graphicsState = GraphicsStates.Overview;

        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    public void HandleGraphicsBack()
    {
        switch (graphicsState)
        {
            case GraphicsStates.FullScreenMode:
                graphicsState = GraphicsStates.Overview;
                HandleFullScreenHighlight();
                EventSystem.current.SetSelectedGameObject(onExitFullScreenChoicesFirstSelected);
                break;
            case GraphicsStates.Resolutions:
                graphicsState = GraphicsStates.Overview;
                HandleDisabledResolutions();
                EventSystem.current.SetSelectedGameObject(onExitResolutionsFirstSelected);
                break;
            default:
                settingsController.Back();
                break;
        }
    }

    public void SetResolution(Script_SettingsResolutionChoice resolutionChoice)
    {
        Debug.Log($"Setting resolution to x: {resolutionChoice.resolution.x}, y: {resolutionChoice.resolution.y}");

        DisplayInfo currentWindow = Screen.mainWindowDisplayInfo;
        
        if (
            resolutionChoice.resolution.x > currentWindow.width
            || resolutionChoice.resolution.y > currentWindow.height
        )
        {
            Script_SFXManager.SFX.PlayBlipError();
            return;
        }

        Screen.SetResolution(resolutionChoice.resolution.x, resolutionChoice.resolution.y, FullScreenMode.Windowed);
    }

    // ------------------------------------------------------------
    // Unity Events

    // Slot Holder Button
    public void ToFullScreenChoices()
    {
        HandleFullScreenUpdate();
        RemoveFullScreenHighlight();

        graphicsState = GraphicsStates.FullScreenMode;
        EventSystem.current.SetSelectedGameObject(fullScreenChoicesFirstSelected);
    }

    public void SetFullScreenWindow()
    {
        Debug.Log($"Setting to FullScreenMode: {FullScreenMode.FullScreenWindow}");
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }

    public void SetWindowed()
    {
        Debug.Log($"Setting to FullScreenMode: {FullScreenMode.Windowed}");
        Screen.fullScreenMode = FullScreenMode.Windowed;
    }

    public void SetMaximizedWindow()
    {
        Debug.Log($"Setting to FullScreenMode: {FullScreenMode.MaximizedWindow}");
        Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
    }

    public void SetExclusiveFullScreen()
    {
        Debug.Log($"Setting to FullScreenMode: {FullScreenMode.ExclusiveFullScreen}");
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
    }

    /// <summary>
    /// Navigate to the first available resolution.
    /// </summary>
    public void ToResolutions()
    {
        HandleDisabledResolutions();

        Script_SettingsResolutionChoice firstAvailableResolution = null;
        
        for (var i = 0; i < resolutions.Count; i++)
        {
            if (resolutions[i].MyButton.enabled)
            {
                firstAvailableResolution = resolutions[i];
                break;
            }
        }

        if (firstAvailableResolution == null)
        {
            Script_SFXManager.SFX.PlayBlipError();
            return;
        }
        
        graphicsState = GraphicsStates.Resolutions;
        EventSystem.current.SetSelectedGameObject(firstAvailableResolution.gameObject);
    }

    // ------------------------------------------------------------

    /// <summary>
    /// Highlight the current Full Screen Mode upon entrance.
    /// </summary>
    private void HandleFullScreenUpdate()
    {
        Debug.Log($"HandleFullScreenUpdate {Screen.fullScreenMode}");
        
        fullScreenChoicesFirstSelected = (Screen.fullScreenMode switch
        {
            FullScreenMode.FullScreenWindow => fullScreenButtons[0],
            FullScreenMode.Windowed => fullScreenButtons[1],
            FullScreenMode.MaximizedWindow => fullScreenButtons[2],
            FullScreenMode.ExclusiveFullScreen => fullScreenButtons[3],
            _ => fullScreenButtons[0]
        }).gameObject;
    }

    /// <summary>
    /// When not inside Full Screen Mode Choices, show a highlight on the
    /// current Mode.
    /// </summary>
    private void HandleFullScreenHighlight()
    {
        RemoveFullScreenHighlight();

        var activeHighlight = Screen.fullScreenMode switch
        {
            FullScreenMode.FullScreenWindow => screenButtonHighlightsLight[0],
            FullScreenMode.Windowed => screenButtonHighlightsLight[1],
            FullScreenMode.MaximizedWindow => screenButtonHighlightsLight[2],
            FullScreenMode.ExclusiveFullScreen => screenButtonHighlightsLight[3],
            _ => screenButtonHighlightsLight[0]
        };

        activeHighlight.gameObject.SetActive(true);
    }

    private void RemoveFullScreenHighlight()
    {
        screenButtonHighlightsLight.ForEach(highlight => highlight.gameObject.SetActive(false));
    }

    private void HandleDisabledResolutions()
    {
        DisplayInfo currentWindow = Screen.mainWindowDisplayInfo;
        
        resolutions.ForEach(res => {
            bool isResolutionTooBig = res.resolution.x > currentWindow.width
                || res.resolution.y > currentWindow.height;
            res.ButtonHighlighter.Activate(!isResolutionTooBig);
        });
    }

    private void PopulateResolutionChoices()
    {
        resolutions = resolutionsHolder.GetComponentsInChildren<Script_SettingsResolutionChoice>(true).ToList();
    }
}
