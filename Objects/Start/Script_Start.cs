using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Entry point for Title scene
/// </summary>
public class Script_Start : MonoBehaviour
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public string curse;
    /* ======================================================================= */
    
    public static Script_Start Main; // can't take namespace Start
    
    private const string StartOptionsStartCurseId = "start_options_start_curse";
    public static StartStates startState;
    public static Script_GameOverController.DeathTypes deathType;
    public Script_StartOverviewController mainController;
    public enum StartStates
    {
        Start,
        GameOver,
        BackToMainMenu
    }
    [SerializeField] private Script_SavedGameTitleControl savedGameTitleControl;
    [SerializeField] private Script_SceneManager sceneManager;
    [SerializeField] private Script_TimelineController crunchTimelineCtrl;

    [SerializeField] private Script_BackgroundMusicManager bgmManager;
    [SerializeField] private Script_SFXManager SFXManager;
    [SerializeField] private Script_SystemSettings systemSettings;
    [SerializeField] private Script_SettingsController settingsController;
    [SerializeField] private Script_SaveSettingsControl saveSettingsControl;
    [SerializeField] private Script_AudioConfiguration audioConfiguration;
    [SerializeField] private Script_CanvasGroupController initFader;
    [SerializeField] private Script_GraphicsManager startGraphicsManager;
    
    [Space][Header("Good Ending Curse")][Space]
    [SerializeField] private Script_SaveCurseControl saveCurseControl;
    [SerializeField] private Script_TMProPopulator startTMProPopulator;
    [SerializeField] private Script_TMProRandomizer TMProRandomizer;
    [SerializeField] private Script_ImageDistorterController TMProRandomizerController;
    
    public Script_GraphicsManager StartGraphicsManager => startGraphicsManager;

    private void Awake()
    {
        Time.timeScale = 1f;
        
        if (Main == null)
        {
            Main = this;
        }
        else if (Main != this)
        {
            Destroy(this.gameObject);
        }

        // To cover red bars on opening on mac.
        initFader.Open();

        savedGameTitleControl.Setup();
        saveSettingsControl.Setup();
        saveCurseControl.Setup();
        sceneManager.Setup();
        SFXManager.Setup();
        bgmManager.Setup();
        settingsController.Setup();
        audioConfiguration.Setup();
        
        Script_SystemSettings.DisableMouse();
        systemSettings.TargetFrameRate();
        systemSettings.SetScreenSettings();
    }

    void Start()
    {
        Script_PlayerInputManager.Instance.Setup();
        saveSettingsControl.Load();
        saveCurseControl.Load();
        Script_PlayerInputManager.Instance.UpdateKeyBindingUIs();
        mainController.Setup();
        HandleCurse();

        switch(startState)
        {
            case (StartStates.GameOver):
                Dev_Logger.Debug("Script_Start in Start() loading Game Over");
                CloseInitFader();
                mainController.InitializeGameOverState();
                mainController.ToGameOver(deathType);
                break;

            case (StartStates.BackToMainMenu):
                Dev_Logger.Debug("Coming back from Game, No Intro Start Screen");
                CloseInitFader();
                mainController.InitializeIntroSimple(isForceInitedSimple: true);
                break;

            default:
                Dev_Logger.Debug("Default Start Screeen");
                // Simple Intro Timeline will handle closing InitFader
                mainController.InitializeIntroSimple();
                break;
        }
    }

    public void CrunchTransitionDown()
    {
        crunchTimelineCtrl.PlayableDirectorPlayFromTimelines(0, 0);
    }
    
    public void CrunchTransitionUp()
    {
        crunchTimelineCtrl.PlayableDirectorPlayFromTimelines(0, 1);
    }

    private void HandleCurse()
    {
        if (curse == Script_SaveCurseControl.CurseTag)
        {
            startTMProPopulator.UpdateTextId(StartOptionsStartCurseId);
            TMProRandomizer.DefaultId = StartOptionsStartCurseId;
            TMProRandomizerController.gameObject.SetActive(true);
        }
    }

    // ----------------------------------------------------------------------
    // Timeline Signals
    
    /// <summary>
    /// - Simple Intro Timeline
    /// Note: will get called again on Simple Intro as a precaution
    /// </summary>
    public void CloseInitFader()
    {
        initFader.Close();
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Script_Start))]
    public class Script_StartTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_Start t = (Script_Start)target;

            if (GUILayout.Button("Save Curse"))
            {
                t.saveCurseControl.Save();
            }

            if (GUILayout.Button("Crunch Transition"))
            {
                t.CrunchTransitionDown();
            }
        }
    }
    #endif
}