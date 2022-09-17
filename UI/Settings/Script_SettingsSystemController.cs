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
    private const string ClickTrigger = "click";

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

    public float MasterVolumeLogarithmic
    {
        get
        {
            float vol;
            audioMixer.GetFloat(Const_AudioMixerParams.MasterVolume, out vol);
            return vol;
        }
    }
    
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

    private void SetMasterVolume(float newVol) => Script_AudioMixerVolume.SetVolume(
        audioMixer, Const_AudioMixerParams.MasterVolume, newVol
    );

    /// <summary>
    /// Set Audio Mixer volume, using its native logarithmic scale
    /// </summary>
    /// <param name="newVol">Logarithmic volume</param>
    private void SetMasterVolumeRaw(float newVol) => Script_AudioMixerVolume.SetVolumeRaw(
        audioMixer, Const_AudioMixerParams.MasterVolume, newVol
    );
    
    private void UpdateMasterVolUI()
    {
        float logarithmicFill = MasterVolumeLogarithmic;

        // Convert logarithmic to decimal
        float percentFill = Mathf.Pow(10, (logarithmicFill / 20));
        
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

    public void Save(Model_SettingsData settingsData)
    {
        float cleanedMasterVolume = MasterVolumeLogarithmic;
        
        Model_SystemData systemData = new Model_SystemData(
            _masterVolume: cleanedMasterVolume
        );

        settingsData.systemData = systemData;
    }

    public void Load(Model_SettingsData settingsData)
    {
        if (settingsData.systemData == null)
            return;
        
        LoadMasterVolume(settingsData.systemData);

        void LoadMasterVolume(Model_SystemData systemData)
        {
            // Ensure logarithmic volume is within Audio Mixer range (-80db to 0db).
            // Nonmatching type will automatically be set as default value (0f).
            float loadedVolume = Mathf.Clamp(systemData.masterVolume, -80f, 0f);
            
            SetMasterVolumeRaw(loadedVolume);
        }
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

            if (GUILayout.Button("Print Master Volume (Log)"))
            {
                Debug.Log(t.MasterVolumeLogarithmic);
            }
        }
    }
    #endif
}
