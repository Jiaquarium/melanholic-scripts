using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using UnityEngine.Audio;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Will create / update settingsData save file on Back out of System Settings.
/// </summary>
public class Script_SettingsSystemController : MonoBehaviour
{
    public enum SystemState
    {
        Overview = 0,
        MasterVolume = 1,
        Resolutions = 2,
    }

    private const float FillIncrement = 10f;
    private static readonly int ClickTrigger = Animator.StringToHash("click");

    public SystemState systemState;
    
    [SerializeField] private GameObject firstSelected;
    
    [SerializeField] private GameObject masterVolHolder;
    [SerializeField] private GameObject onExitMasterVolFirstSelected;
    [SerializeField] private List<Animator> masterVolArrows;
    [SerializeField] private Script_Timebar timebar;
    [SerializeField] private TextMeshProUGUI masterVolCurrentText;
    
    [SerializeField] private GameObject onExitResolutionsFirstSelected;
    [SerializeField] private GameObject resolutionsHolder;
    [SerializeField] private List<Script_SettingsResolutionChoice> resolutions;
    [SerializeField] private TextMeshProUGUI resolutionsHelperText;
    [SerializeField] private TextMeshProUGUI resolutionCurrentText;
    
    [SerializeField] private Script_SettingsController settingsController;
    [SerializeField] private Camera cam;
    [SerializeField] private AudioMixer audioMixer;

    public float AudioListenerMasterVolume => AudioListener.volume;
    
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
        if (systemState == SystemState.MasterVolume)
            HandleMasterVolumeInput();
        
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

    public void HandleSystemSubmenuEscBack()
    {
        switch (systemState)
        {
            case SystemState.MasterVolume:
                systemState = SystemState.Overview;
                InitialState();
                EventSystem.current.SetSelectedGameObject(onExitMasterVolFirstSelected);
                ExitSubmenuSFX();
                break;
            case SystemState.Resolutions:
                systemState = SystemState.Overview;
                InitialState();
                EventSystem.current.SetSelectedGameObject(onExitResolutionsFirstSelected);
                ExitSubmenuSFX();
                break;
            default:
                settingsController.Back();
                break;
        }
    }

    public void SetResolution(Script_SettingsResolutionChoice resolutionChoice)
    {
        Dev_Logger.Debug($"Setting resolution to x: {resolutionChoice.resolution.x}, y: {resolutionChoice.resolution.y}");

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

    /// <summary>
    /// - Master Volume Holder: OnClick
    /// </summary>
    public void ToMasterVolume()
    {
        systemState = SystemState.MasterVolume;
        EventSystem.current.SetSelectedGameObject(masterVolHolder);
        masterVolArrows.ForEach(arrow => arrow.gameObject.SetActive(true));
        EnterSubmenuSFX();
    }

    // ------------------------------------------------------------

    private void HandleMasterVolumeInput()
    {
        if (Input.GetButtonDown(Const_KeyCodes.Left))
        {
            if (timebar.Fill > 0f)
            {
                SetMasterVolume(timebar.Fill - (1 / FillIncrement));
                UpdateMasterVolUI();
                SubmitSFX();
                masterVolArrows[0].SetTrigger(ClickTrigger);
            }
            else
                DullErrorSFX();
        }
        else if (Input.GetButtonDown(Const_KeyCodes.Right))
        {
            if (timebar.Fill < 1f)
            {
                SetMasterVolume(timebar.Fill + (1 / FillIncrement));
                UpdateMasterVolUI();
                SubmitSFX();
                masterVolArrows[1].SetTrigger(ClickTrigger);
            }
            else
                DullErrorSFX();
        }
    }

    /// <summary>
    /// Ensures Listener volume is only set by desired increments.
    /// </summary>
    private void SetMasterVolume(float newVol) => Script_AudioMixerVolume.SetMasterVolume(
        Mathf.Round(newVol * FillIncrement) / FillIncrement
    );
    
    private void UpdateMasterVolUI()
    {
        float percentFill = AudioListenerMasterVolume;
        
        // Ensure to round to the proper increment
        timebar.Fill = Mathf.Round(percentFill * FillIncrement) / FillIncrement;

        // Display volume as a whole number 0-10
        masterVolCurrentText.text = $"{Mathf.Round(timebar.Fill * FillIncrement)}";
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

    private void UpdateVPResolutionText()
    {
        float screenPixelWidth = cam.pixelWidth;
		float screenPixelHeight = cam.pixelHeight;
		string screenPixelWidthText = string.Format("{0}", screenPixelWidth);
		string screenPixelHeightText = string.Format("{0}", screenPixelHeight);
		
		var VPText = $"{screenPixelWidthText}X{screenPixelHeightText}";
        resolutionCurrentText.text = VPText;
    }

    // ------------------------------------------------------------
    // Saving & Loading

    /// <summary>
    /// Save as a decibel since if the value becomes corrupted, it will default to 0f, meaning
    /// Audio Listener will default to 1f; otherwise, Audio Listener would default to 0f and game
    /// would start silently.
    /// </summary>
    public void Save(Model_SettingsData settingsData)
    {
        float masterVolumeDecibels = AudioListenerMasterVolume.ConvertFloatToDecibel();
        
        Model_SystemData systemData = new Model_SystemData(
            _masterVolume: masterVolumeDecibels
        );

        settingsData.systemData = systemData;
    }

    /// <summary>
    /// Load will use all System Load Handlers. If we need to load separate fields, define separate handlers.
    /// </summary>
    public void Load(Model_SettingsData settingsData)
    {
        if (settingsData.systemData == null)
            return;
        
        HandleMasterVolumeLoad(settingsData.systemData);
    }

    public void LoadOnlyMasterVolume(Model_SettingsData settingsData)
    {
        if (settingsData.systemData == null)
            return;
        
        HandleMasterVolumeLoad(settingsData.systemData);
    }

    private void HandleMasterVolumeLoad(Model_SystemData systemData)
    {
        // Ensure decibel volume range is within Audio Listener range of 0.0001f to 1.0f (-80db to 0db)
        // Nonmatching type will automatically be set as default value (0f).
        float loadedVolumeDecibel = Mathf.Clamp(systemData.masterVolume, -80f, 0f);
        
        // dB value of -80 will convert to .0001f so ensure to round this to 0.
        SetMasterVolume(loadedVolumeDecibel.ConvertDecibelToFloat());
    }

    // ------------------------------------------------------------

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
        masterVolArrows.ForEach(arrow => arrow.gameObject.SetActive(false));
        UpdateMasterVolUI();
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Script_SettingsSystemController))]
    public class Script_SettingsSystemControllerTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_SettingsSystemController t = (Script_SettingsSystemController)target;
            if (GUILayout.Button("Set Master Volume 1f"))
            {
                t.SetMasterVolume(1f);
            }

            if (GUILayout.Button("Set Master Volume 0.5f"))
            {
                t.SetMasterVolume(0.5f);
            }

            if (GUILayout.Button("Set Master Volume 0f"))
            {
                t.SetMasterVolume(0f);
            }

            if (GUILayout.Button("Print Master Volume (Float)"))
            {
                Dev_Logger.Debug($"{t.AudioListenerMasterVolume}");
            }
        }
    }
    #endif
}
