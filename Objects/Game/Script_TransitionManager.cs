using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Handles ending sequences, fading in and out, etc.
/// </summary>
[RequireComponent(typeof(Script_TimelineController))]
public class Script_TransitionManager : MonoBehaviour
{
    public enum Endings
    {
        Bad,
        Good,
        True,
        Dream,
    }
    
    public Script_CanvasGroupFadeInOut fader;
    
    [SerializeField] private Script_CanvasGroupController underDialogueController;
    [SerializeField] private Script_Game game;
    [SerializeField] private Script_TimeManager timeManager;
    
    public const float RestartPlayerFadeInTime = 0.25f;
    public const float RestartPlayerFadeOutTime = 1f;
    public float underDialogueFadeTime = 1.5f;
    public Script_CanvasGroupController restartPrompt;

    private Script_GameOverController.DeathTypes deathType;
    private Endings activeEnding;
    
    public IEnumerator FadeIn(float t, Action action)
    {
        return fader.FadeInCo(t, action);
    }

    public IEnumerator FadeOut(float t, Action action)
    {
        return fader.FadeOutCo(t, action);
    }

    public void UnderDialogueBlackScreen()
    {
        underDialogueController.Open();
    }

    public void UnderDialogueFadeIn(float t, Action action)
    {
        underDialogueController.FadeIn(t, action);
    }
    
    public void UnderDialogueFadeOut(float t, Action action)
    {
        underDialogueController.FadeOut(t, action);
    }

    public void DieEffects(Script_GameOverController.DeathTypes _deathType)
    {
        deathType = _deathType;
        
        game.ChangeStateCutScene();

        /// Slow down time and fade screen to black
        Time.timeScale = timeManager.dieTimeScale;
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);
    }

    public void OnDiePlayableDone(PlayableDirector aDirector)
    {
        if (
            aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[0]
        )
        {
            // return timeScale to normal
            Time.timeScale = 1.0f;
            
            Script_SceneManager.ToGameOver(deathType);
        }
    }

    public void TimesUpEffects()
    {
        game.ChangeStateCutScene();

        /// Slow down time and fade screen to black
        Time.timeScale = timeManager.dieTimeScale;
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 1);
    }

    /// <summary>
    /// Fade Screen to Black and store which Ending to play, signal to play Ending Cut Scene.
    /// </summary>
    public void StartEndingSequence(Endings ending)
    {
        game.ChangeStateCutScene();
        Script_BackgroundMusicManager.Control.FadeOutSlow();

        activeEnding = ending;

        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 2);        
    }

    // ------------------------------------------------------------------
    // Signal Reactions START
    public void OnTimesUpPlayableDone()
    {
        Time.timeScale = 1.0f;

        // Prompt Player
        FadeInRestartPrompt();
    }

    // After screen has faded to Black play the proper timeline.
    public void PlayEndingCutScene()
    {
        switch (activeEnding)
        {
            case (Endings.Good):
                GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 3);
                break;
            case (Endings.True):
                break;
            case (Endings.Dream):
                break;
        }
    }

    // After played proper ending cut scene.
    public void RollCredits()
    {
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 4);
    }

    // ------------------------------------------------------------------

    public void FadeInRestartPrompt()
    {
        Debug.Log("Show prompt to player on how they would like to restart");
        
        restartPrompt.Close();
        restartPrompt.FadeIn();
    }

    public void FadeOutRestartPrompt()
    {
        Debug.Log("Show prompt to player on how they would like to restart");
        
        restartPrompt.FadeOut(default, () => {
            restartPrompt.Close();
        });
    }

    /// ------------------------------------------------------------
    /// Called from RestartPrompt UIChoices
    /// ------------------------------------------------------------
    /// <summary>
    /// Restart from last save (Tedmunch or initialized)
    /// </summary>
    public void RestartRun()
    {
        game.RestartRun();
    }

    /// <summary>
    /// Restart from the initialized run, will erase game data
    /// </summary>
    public void RestartGameInitialized()
    {
        game.RestartInitialized();
    }

    public void ToTitleScreen()
    {
        game.ToTitleScreen();
    }
    /// ------------------------------------------------------------

    public void Setup()
    {
        fader.gameObject.SetActive(true);
        restartPrompt.Close();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_TransitionManager))]
public class Script_TransitionManagerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_TransitionManager t = (Script_TransitionManager)target;
        if (GUILayout.Button("TimesUpEffects()"))
        {
            t.TimesUpEffects();
        }

        if (GUILayout.Button("OnTimesUpPlayableDone()"))
        {
            t.OnTimesUpPlayableDone();
        }

        if (GUILayout.Button("FadeInRestartPrompt()"))
        {
            t.FadeInRestartPrompt();
        }

        if (GUILayout.Button("FadeOutRestartPrompt()"))
        {
            t.FadeOutRestartPrompt();
        }

        if (GUILayout.Button("Good Ending"))
        {
            t.StartEndingSequence(Script_TransitionManager.Endings.Good);
        }
    }
}
#endif