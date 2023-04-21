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
/// 
/// Handles UI for Settings > System and Settings > Sound
/// </summary>
public class Script_SettingsSystemController : MonoBehaviour
{
    // ==================================================================
    // State Data
    public static bool IsScreenshakeDisabled;
    // ==================================================================
    
    public enum SystemState
    {
        SystemOverview = 0,
        MasterVolume = 1,
        Resolutions = 2,
        FullScreenSelect = 3,
        MusicVolume = 4,
        FXVolume = 5,
        Screenshake = 6,
        SoundOverview = 7,
    }

    public const float WaitAfterFullScreenSwitchTime = 0.4f;
    public const float DBFloor = -80f;
    public const float DBCeiling = 0f;
    private const float FillIncrement = 10f;
    
    public static readonly int ClickTrigger = Script_SettingsController.ClickTrigger;
    private const string OnFallback = "on";
    private const string OffFallback = "off";

    public SystemState systemState;
    
    [UnityEngine.Serialization.FormerlySerializedAs("firstSelected")]
    [SerializeField] private GameObject firstSelectedSystem;
    [SerializeField] private GameObject firstSelectedSound;
    
    [Space][Header("---- Master Volume ----")][Space]
    [SerializeField] private GameObject masterVolHolder;
    [SerializeField] private GameObject onExitMasterVolFirstSelected;
    [SerializeField] private List<Animator> masterVolArrows;
    [UnityEngine.Serialization.FormerlySerializedAs("timebar")]
    [SerializeField] private Script_Timebar masterTimebar;
    [SerializeField] private TextMeshProUGUI masterVolCurrentText;
    
    [Space][Header("---- Music Volume ----")][Space]
    [SerializeField] private GameObject musicVolHolder;
    [SerializeField] private GameObject onExitMusicVolFirstSelected;
    [SerializeField] private List<Animator> musicVolArrows;
    [SerializeField] private Script_Timebar musicTimebar;
    [SerializeField] private TextMeshProUGUI musicVolCurrentText;

    [Space][Header("---- SFX Volume ----")][Space]
    [SerializeField] private GameObject sfxVolHolder;
    [SerializeField] private GameObject onExitSfxVolFirstSelected;
    [SerializeField] private List<Animator> sfxVolArrows;
    [SerializeField] private Script_Timebar sfxTimebar;
    [SerializeField] private TextMeshProUGUI sfxVolCurrentText;
    
    [Space][Header("---- Force Resolution ----")][Space]
    [SerializeField] private GameObject onExitResolutionsFirstSelected;
    [SerializeField] private GameObject resolutionsHolder;
    [SerializeField] private List<Script_SettingsResolutionChoice> resolutions;
    [SerializeField] private TextMeshProUGUI resolutionsHelperText;
    [SerializeField] private TextMeshProUGUI resolutionCurrentText;

    [Space][Header("---- Full Screen ----")][Space]
    [SerializeField] private GameObject onExitFullScreenSelectFirstSelected;
    [SerializeField] private List<Script_SettingsFullScreenChoice> fullScreenOptions;
    [SerializeField] private TextMeshProUGUI fullScreenCurrentText;
    private const string FullScreenOnId = "graphics_fullscreen_current_on";
    private const string FullScreenOffId = "graphics_fullscreen_current_off";
    private static string FullScreenOnTextLocalized;
    private static string FullScreenOffTextLocalized;
    private bool isFullScreen;

    [Space][Header("---- Screenshake ----")][Space]
    [SerializeField] private GameObject onExitScreenshakeSelectFirstSelected;
    [SerializeField] private List<Script_SettingsScreenshakeChoice> screenshakeOptions;
    [SerializeField] private TextMeshProUGUI screenshakeCurrentText;
    private const string ScreenshakeOnId = "graphics_screenshake_current_on";
    private const string ScreenshakeOffId = "graphics_screenshake_current_off";
    private static string ScreenshakeOnTextLocalized;
    private static string ScreenshakeOffTextLocalized;
    
    [Space][Header("---- General ----")][Space]
    [SerializeField] private Script_SettingsController settingsController;
    [SerializeField] private Camera cam;
    [SerializeField] private AudioMixer audioMixer;

    private List<Animator> currentVolArrows;
    private Script_Timebar currentTimebar;
    private TextMeshProUGUI currentVolDisplayText;

    public float AudioListenerMasterVolume => AudioListener.volume;

    private Rewired.Player rewiredInput;
    
    void OnValidate()
    {
        PopulateResolutionChoices();
    }
    
    void OnEnable()
    {
        InitialState();
    }

    void Start()
    {
        // Note: should only reference in Start or later since Player Input singleton is set in Awake
        rewiredInput = Script_PlayerInputManager.Instance.RewiredInput;

        FullScreenOnTextLocalized = Script_UIText.Text[FullScreenOnId].GetProp<string>(Const_Dev.Lang) ?? OnFallback;
        FullScreenOffTextLocalized = Script_UIText.Text[FullScreenOffId].GetProp<string>(Const_Dev.Lang) ?? OffFallback;
        ScreenshakeOnTextLocalized = Script_UIText.Text[ScreenshakeOnId].GetProp<string>(Const_Dev.Lang) ?? OnFallback;
        ScreenshakeOffTextLocalized = Script_UIText.Text[ScreenshakeOffId].GetProp<string>(Const_Dev.Lang) ?? OffFallback;
    }
    
    void Update()
    {
        switch (systemState)
        {
            // Handle System Overview is active UI
            case SystemState.SystemOverview:
            case SystemState.Resolutions:
            case SystemState.FullScreenSelect:
            case SystemState.Screenshake:
                HandleDisabledResolutions();
                UpdateFullScreenUI();
                UpdateScreenShakeText();
                break;
            // Handle Sound Overview is active UI
            case SystemState.MasterVolume:
            case SystemState.MusicVolume:
            case SystemState.FXVolume:
                HandleMasterVolumeInput();            
                break;
            // Don't detect input if on Sound Overview
            case SystemState.SoundOverview:
            default:
                break;
        }
    }

    void LateUpdate()
    {
        UpdateVPResolutionText();
    }
    
    public void ToGraphics()
    {
        systemState = SystemState.SystemOverview;

        InitialState();
        EventSystem.current.SetSelectedGameObject(firstSelectedSystem);
    }

    public void ToSound()
    {
        systemState = SystemState.SoundOverview;
        
        InitialState();
        EventSystem.current.SetSelectedGameObject(firstSelectedSound);
    }

    public void HandleSystemSubmenuEscBack()
    {
        switch (systemState)
        {
            case SystemState.MasterVolume:
                systemState = SystemState.SoundOverview;
                InitialState();
                EventSystem.current.SetSelectedGameObject(onExitMasterVolFirstSelected);
                ExitSubmenuSFX();
                break;
            case SystemState.MusicVolume:
                systemState = SystemState.SoundOverview;
                InitialState();
                EventSystem.current.SetSelectedGameObject(onExitMusicVolFirstSelected);
                ExitSubmenuSFX();
                break;
            case SystemState.FXVolume:
                systemState = SystemState.SoundOverview;
                InitialState();
                EventSystem.current.SetSelectedGameObject(onExitSfxVolFirstSelected);
                ExitSubmenuSFX();
                break;
            case SystemState.Resolutions:
                systemState = SystemState.SystemOverview;
                InitialState();
                EventSystem.current.SetSelectedGameObject(onExitResolutionsFirstSelected);
                ExitSubmenuSFX();
                break;
            case SystemState.FullScreenSelect:
                systemState = SystemState.SystemOverview;
                InitialState();
                EventSystem.current.SetSelectedGameObject(onExitFullScreenSelectFirstSelected);
                ExitSubmenuSFX();
                break;
            case SystemState.Screenshake:
                systemState = SystemState.SystemOverview;
                InitialState();
                EventSystem.current.SetSelectedGameObject(onExitScreenshakeSelectFirstSelected);
                ExitSubmenuSFX();
                break;
            case SystemState.SystemOverview:
            case SystemState.SoundOverview:
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
        UpdateCurrentVolumeRefs(SystemState.MasterVolume);
        EventSystem.current.SetSelectedGameObject(masterVolHolder);
        masterVolArrows.ForEach(arrow => arrow.gameObject.SetActive(true));
        EnterSubmenuSFX();
    }

    /// <summary>
    /// - Music Volume Holder: OnClick
    /// </summary>
    public void ToMusicVolume()
    {
        systemState = SystemState.MusicVolume;
        UpdateCurrentVolumeRefs(SystemState.MusicVolume);
        EventSystem.current.SetSelectedGameObject(musicVolHolder);
        musicVolArrows.ForEach(arrow => arrow.gameObject.SetActive(true));
        EnterSubmenuSFX();
    }

    /// <summary>
    /// - SFX Volume Holder: OnClick
    /// </summary>
    public void ToSfxVolume()
    {
        systemState = SystemState.FXVolume;
        UpdateCurrentVolumeRefs(SystemState.FXVolume);
        EventSystem.current.SetSelectedGameObject(sfxVolHolder);
        sfxVolArrows.ForEach(arrow => arrow.gameObject.SetActive(true));
        EnterSubmenuSFX();
    }

    private void UpdateCurrentVolumeRefs(SystemState state)
    {
        switch (state)
        {
            case SystemState.MasterVolume:
                currentTimebar = masterTimebar;
                currentVolArrows = masterVolArrows;
                currentVolDisplayText = masterVolCurrentText;
                break;
            case SystemState.MusicVolume:
                currentTimebar = musicTimebar;
                currentVolArrows = musicVolArrows;
                currentVolDisplayText = musicVolCurrentText;
                break;
            case SystemState.FXVolume:
                currentTimebar = sfxTimebar;
                currentVolArrows = sfxVolArrows;
                currentVolDisplayText = sfxVolCurrentText;
                break;
        }
    }

    /// <summary>
    /// - Full Screen Select Holder: OnClick
    /// </summary>
    public void ToFullScreenSelect()
    {
        systemState = SystemState.FullScreenSelect;
        
        bool isCurrentlyFullScreen = Screen.fullScreen;
        var currentFSOption = isCurrentlyFullScreen ? fullScreenOptions[0] : fullScreenOptions[1];
        EventSystem.current.SetSelectedGameObject(currentFSOption.gameObject);

        // Allow focused highlighting to work again
        fullScreenOptions[0].HoldHighlight(false);
        fullScreenOptions[1].HoldHighlight(false);

        EnterSubmenuSFX();
    }

    /// <summary>
    /// - Screenshake Select Holder: OnClick
    /// </summary>
    public void ToScreenshakeSelect()
    {
        systemState = SystemState.Screenshake;

        bool isScreenShakeOn = !IsScreenshakeDisabled;
        var currentScreenshakeOption = isScreenShakeOn ? screenshakeOptions[0] : screenshakeOptions[1];
        EventSystem.current.SetSelectedGameObject(currentScreenshakeOption.gameObject);
        
        // Make options react to highlighting like normal
        screenshakeOptions[0].HoldHighlight(false);
        screenshakeOptions[1].HoldHighlight(false);

        EnterSubmenuSFX();
    }

    // ------------------------------------------------------------

    private void HandleMasterVolumeInput()
    {
        if (rewiredInput.GetNegativeButtonDown(Const_KeyCodes.RWHorizontal))
        {
            if (currentTimebar.Fill > 0f)
            {
                SetSettingsVolume(currentTimebar.Fill - (1 / FillIncrement), systemState);
                UpdateMasterVolUI();
                SubmitSFX();
                currentVolArrows[0].SetTrigger(ClickTrigger);
            }
            else
                DullErrorSFX();
        }
        else if (rewiredInput.GetButtonDown(Const_KeyCodes.RWHorizontal))
        {
            if (currentTimebar.Fill < 1f)
            {
                SetSettingsVolume(currentTimebar.Fill + (1 / FillIncrement), systemState);
                UpdateMasterVolUI();
                SubmitSFX();
                currentVolArrows[1].SetTrigger(ClickTrigger);
            }
            else
                DullErrorSFX();
        }
    }

    /// <summary>
    /// Ensures Listener volume is only set by desired increments.
    /// </summary>
    private void SetSettingsVolume(float newVol, SystemState systemState)
    {
        float nearestIntVol = Mathf.Round(newVol * FillIncrement) / FillIncrement;
        
        switch (systemState)
        {
            case SystemState.MasterVolume:
                Script_AudioMixerVolume.SetMasterVolume(nearestIntVol);
                break;
            case SystemState.MusicVolume:
                // Music Setting and UI Music Setting should always be set together
                Script_AudioMixerVolume.SetVolume(
                    audioMixer, Const_AudioMixerParams.ExposedMusicSettingVolume, nearestIntVol
                );
                Script_AudioMixerVolume.SetVolume(
                    audioMixer, Const_AudioMixerParams.ExposedUIMusicSettingVolume, nearestIntVol
                );
                break;
            case SystemState.FXVolume:
                // FX Setting and UI FX Setting should always be set together
                Script_AudioMixerVolume.SetVolume(
                    audioMixer, Const_AudioMixerParams.ExposedFXSettingVolume, nearestIntVol
                );
                Script_AudioMixerVolume.SetVolume(
                    audioMixer, Const_AudioMixerParams.ExposedUIFXSettingVolume, nearestIntVol
                );
                break;
            default:
                break;
        }
    }
    
    private void UpdateMasterVolUI()
    {
        UpdateMasterVolUI();
        UpdateMusicVolUI();
        UpdateSfxVolUI();
        
        void UpdateMasterVolUI()
        {
            float percentFill = AudioListenerMasterVolume;
        
            // Ensure to round to the proper increment
            masterTimebar.Fill = Mathf.Round(percentFill * FillIncrement) / FillIncrement;

            // Display volume as a whole number 0-10
            masterVolCurrentText.text = $"{Mathf.Round(masterTimebar.Fill * FillIncrement)}";
        }

        void UpdateMusicVolUI()
        {
            float percentFill = GetAudioMixerVolume(Const_AudioMixerParams.ExposedMusicSettingVolume, false);

            musicTimebar.Fill = Mathf.Round(percentFill * FillIncrement) / FillIncrement;
            musicVolCurrentText.text = $"{Mathf.Round(musicTimebar.Fill * FillIncrement)}";
        }

        void UpdateSfxVolUI()
        {
            float percentFill = GetAudioMixerVolume(Const_AudioMixerParams.ExposedFXSettingVolume, false);
        
            sfxTimebar.Fill = Mathf.Round(percentFill * FillIncrement) / FillIncrement;
            sfxVolCurrentText.text = $"{Mathf.Round(sfxTimebar.Fill * FillIncrement)}";
        }
    }
    
    private void HandleDisabledResolutions()
    {
        DisplayInfo currentWindow = Screen.mainWindowDisplayInfo;
        bool isDisabledResolutionFocused = false;
        
        resolutions.ForEach(res => {
            bool isResolutionTooBig = res.resolution.x > currentWindow.width
                || res.resolution.y > currentWindow.height;
            res.ButtonHighlighter.Activate(!isResolutionTooBig);
            
            // Handle Player has focused a resolution selection that will be deactivated when switching monitors
            var currentSelected = EventSystem.current?.currentSelectedGameObject;
            if (isResolutionTooBig && currentSelected != null && res.gameObject == currentSelected)
                isDisabledResolutionFocused = true;
        });

        // If focused selection is now deactivated, select the next available resolution
        if (isDisabledResolutionFocused)
        {
            foreach (var res in resolutions)
            {
                if (res.GetComponent<Button>().enabled)
                {
                    EventSystem.current.SetSelectedGameObject(res.gameObject);
                    break;
                }
            }
        }
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

    private void UpdateFullScreenUI(bool isForceUpdate = false)
    {
        bool currentFS = Screen.fullScreen;
        fullScreenCurrentText.text = currentFS ? FullScreenOnTextLocalized : FullScreenOffTextLocalized;
        
        // Once inside Full Screen Select, just allow buttonHighlighter to handle UI
        if (systemState == SystemState.FullScreenSelect)
            return;
        
        if (isForceUpdate || isFullScreen != currentFS)
        {
            Dev_Logger.Debug($"UpdateFullScreenUI currentFullScreen {isFullScreen}");
            isFullScreen = currentFS;
            HandleUnfocusedHighlights();
        }

        void HandleUnfocusedHighlights()
        {
            // Handle UnfocusedHighlight below
            var focusedFsOption = isFullScreen ? fullScreenOptions[0] : fullScreenOptions[1];
            var unfocusedFsOption = isFullScreen ? fullScreenOptions[1] : fullScreenOptions[0];

            focusedFsOption.HoldHighlight(true);
            focusedFsOption.Select();
            // Hide the focused highlight and show the unfocused highlight
            focusedFsOption.HandleUnfocusedHighlight(true);

            unfocusedFsOption.HoldHighlight(false);
            unfocusedFsOption.Deselect();
            // Hide both the focused and unfocused highlights
            unfocusedFsOption.HandleUnfocusedHighlight(false);
        }
    }

    private void UpdateScreenshakeUI()
    {
        UpdateScreenShakeText();
        
        // Focus button that is highlighted
        var focusedScreenshakeOption = IsScreenshakeDisabled ? screenshakeOptions[1] : screenshakeOptions[0];
        var unfocusedScreenshakeOption = IsScreenshakeDisabled ? screenshakeOptions[0] : screenshakeOptions[1];
        
        focusedScreenshakeOption.HoldHighlight(true);
        focusedScreenshakeOption.Select();
        // Hide the focused highlight and show the unfocused highlight
        focusedScreenshakeOption.HandleUnfocusedHighlight(true);

        unfocusedScreenshakeOption.HoldHighlight(false);
        unfocusedScreenshakeOption.Deselect();
        // Hide both the focused and unfocused highlights
        unfocusedScreenshakeOption.HandleUnfocusedHighlight(false);
    }

    private void UpdateScreenShakeText()
    {
        screenshakeCurrentText.text = IsScreenshakeDisabled
            ? ScreenshakeOffTextLocalized
            : ScreenshakeOnTextLocalized;
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
        float musicVolumeDecibels = GetAudioMixerVolume(Const_AudioMixerParams.ExposedMusicSettingVolume, true);
        float fxVolumeDecibels = GetAudioMixerVolume(Const_AudioMixerParams.ExposedFXSettingVolume, true);
        
        Model_SystemData systemData = new Model_SystemData(
            _masterVolume: masterVolumeDecibels,
            _musicVolume: musicVolumeDecibels,
            _fxVolume: fxVolumeDecibels,
            _isScreenshakeDisabled: IsScreenshakeDisabled
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
        
        HandleVolumeSettingsLoad(settingsData.systemData);

        // Other system data to load
        IsScreenshakeDisabled = settingsData.systemData.isScreenshakeDisabled;
    }

    public void LoadOnlyMasterVolume(Model_SettingsData settingsData)
    {
        if (settingsData.systemData == null)
            return;
        
        HandleVolumeSettingsLoad(settingsData.systemData);
    }

    private void HandleVolumeSettingsLoad(Model_SystemData systemData)
    {
        // Ensure decibel volume range is within Audio Listener range of 0.0001f to 1.0f (-80db to 0db)
        // Nonmatching type will automatically be set as default value (0f).
        // dB value of -80 will convert to .0001f so ensure to round this to 0.
        float loadedVolumeDecibel = Mathf.Clamp(systemData.masterVolume, DBFloor, DBCeiling);
        float loadedMusicVolumeDecibel = Mathf.Clamp(systemData.musicVolume, DBFloor, DBCeiling);
        float loadedFxVolumeDecibel = Mathf.Clamp(systemData.fxVolume, DBFloor, DBCeiling);
        
        SetSettingsVolume(loadedVolumeDecibel.ConvertDecibelToFloat(), SystemState.MasterVolume);
        SetSettingsVolume(loadedMusicVolumeDecibel.ConvertDecibelToFloat(), SystemState.MusicVolume);
        SetSettingsVolume(loadedFxVolumeDecibel.ConvertDecibelToFloat(), SystemState.FXVolume);
    }

    // ------------------------------------------------------------

    public void EnableNavigation(bool isEnabled)
    {
        EventSystem.current.sendNavigationEvents = isEnabled;
    }

    private float GetAudioMixerVolume(string audioParam, bool isDecibel)
    {
        float vol = 1f;
        Script_AudioMixerVolume.GetVolume(audioMixer, audioParam, out vol, isDecibel);
        return vol;
    }
    
    private void EnterSubmenuSFX()
    {
        Script_SFXManager.SFX.PlayEnterSubmenu();
    }

    private void ExitSubmenuSFX()
    {
        Script_SFXManager.SFX.PlayExitSubmenuPencil();
    }

    public void SubmitSFX()
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
        musicVolArrows.ForEach(arrow => arrow.gameObject.SetActive(false));
        sfxVolArrows.ForEach(arrow => arrow.gameObject.SetActive(false));
        UpdateMasterVolUI();
        UpdateFullScreenUI(isForceUpdate: true);
        UpdateScreenshakeUI();
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
                t.SetSettingsVolume(1f, SystemState.MasterVolume);
            }

            if (GUILayout.Button("Set Master Volume 0.5f"))
            {
                t.SetSettingsVolume(0.5f, SystemState.MasterVolume);
            }

            if (GUILayout.Button("Set Master Volume 0f"))
            {
                t.SetSettingsVolume(0f, SystemState.MasterVolume);
            }

            if (GUILayout.Button("Print Master Volume (Float)"))
            {
                Dev_Logger.Debug($"{t.AudioListenerMasterVolume}");
            }
        }
    }
    #endif
}
