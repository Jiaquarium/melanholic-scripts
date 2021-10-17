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
    public static Script_Start Main; // can't take namespace Start
    public static StartStates startState;
    public static Script_GameOverController.DeathTypes deathType;
    public Script_StartOverviewController mainController;
    public enum StartStates
    {
        Start,
        GameOver
    }
    [SerializeField] private Script_SavedGameTitleControl savedGameTitleControl;
    [SerializeField] private Script_SceneManager sceneManager;
    [SerializeField] private Script_TimelineController crunchTimelineCtrl;

    [SerializeField] private Script_SFXManager SFXManager;
    
    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();    
    }
    
    private void Awake()
    {
        PlayerPrefs.DeleteAll();
        
        if (Main == null)
        {
            Main = this;
        }
        else if (Main != this)
        {
            Destroy(this.gameObject);
        }

        savedGameTitleControl.Setup();
        sceneManager.Setup();
        SFXManager.Setup();
        
        Script_SystemSettings.TargetFrameRate();
        Script_SystemSettings.SetScreenSettings();
        Script_SystemSettings.DisableMouse();
    }

    void Start()
    {
        mainController.Setup();

        switch(startState)
        {
            case(StartStates.GameOver):
                Debug.Log("Script_Start in Start() loading Game Over");
                mainController.InitializeGameOverState();
                mainController.ToGameOver(deathType);
                break;

            default:
                Debug.Log("Default Start Screeen");
                mainController.InitializeIntro();
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
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_Start))]
public class Script_StartTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_Start t = (Script_Start)target;
    }
}
#endif