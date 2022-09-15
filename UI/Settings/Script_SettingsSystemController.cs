using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class Script_SettingsSystemController : MonoBehaviour
{
    public enum SystemState
    {
        Overview = 0,
        Resolutions = 1,
    }

    public SystemState systemState;
    
    [SerializeField] private GameObject firstSelected;
    
    [SerializeField] private GameObject onExitResolutionsFirstSelected;
    [SerializeField] private GameObject resolutionsHolder;
    [SerializeField] private List<Script_SettingsResolutionChoice> resolutions;
    [SerializeField] private TextMeshProUGUI resolutionsHelperText;
    [SerializeField] private TextMeshProUGUI resolutionCurrentText;
    
    [SerializeField] private Script_SettingsController settingsController;
    [SerializeField] private Camera cam;

    void OnValidate()
    {
        PopulateResolutionChoices();
    }
    
    void OnEnable()
    {
        InitialState();
    }
    
    void Update()
    {
        if (systemState != SystemState.Resolutions)
            HandleDisabledResolutions();
    }

    void LateUpdate()
    {
        UpdateVPResolutionText();
    }
    
    public void ToGraphics()
    {
        systemState = SystemState.Overview;

        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    public void HandleGraphicsBack()
    {
        switch (systemState)
        {
            case SystemState.Resolutions:
                systemState = SystemState.Overview;
                HandleDisabledResolutions();
                EventSystem.current.SetSelectedGameObject(onExitResolutionsFirstSelected);
                ExitSubmenuSFX();
                resolutionsHelperText.gameObject.SetActive(false);
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
        SubmitSFX();
    }

    // ------------------------------------------------------------
    // Unity Events

    /// <summary>
    /// Navigate to the first available resolution.
    /// - Button Resolutions Holder: OnClick
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

        // Screen is too small for any available resolutions
        if (firstAvailableResolution == null)
        {
            Script_SFXManager.SFX.PlayBlipError();
            return;
        }
        
        systemState = SystemState.Resolutions;
        EventSystem.current.SetSelectedGameObject(firstAvailableResolution.gameObject);

        resolutionsHelperText.gameObject.SetActive(true);
        EnterSubmenuSFX();
    }

    // ------------------------------------------------------------

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

    private void UpdateVPResolutionText()
    {
        float screenPixelWidth = cam.pixelWidth;
		float screenPixelHeight = cam.pixelHeight;
		string screenPixelWidthText = string.Format("{0}", screenPixelWidth);
		string screenPixelHeightText = string.Format("{0}", screenPixelHeight);
		
		var VPText = $"{screenPixelWidthText}X{screenPixelHeightText}";
        resolutionCurrentText.text = VPText;
    }

    private void EnterSubmenuSFX()
    {
        Script_SFXManager.SFX.PlayEnterSubmenu();
    }

    private void ExitSubmenuSFX()
    {
        Script_SFXManager.SFX.PlayExitSubmenuPencil();
    }

    private void SubmitSFX()
    {
        Script_SFXManager.SFX.PlayUIChoiceSubmit();
    }

    private void DullErrorSFX()
    {
        Script_SFXManager.SFX.PlayDullError();
    }

    private void InitialState()
    {
        PopulateResolutionChoices();
        HandleDisabledResolutions();
        resolutionsHelperText.gameObject.SetActive(false);
    }
}
