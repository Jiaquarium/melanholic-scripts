using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Handles important cut scenes (e.g. ending sequences, all puzzles done)
/// and fading in and out, etc.
/// </summary>
[RequireComponent(typeof(Script_TimelineController))]
public class Script_TransitionManager : MonoBehaviour
{
    public static Script_TransitionManager Control;
    
    public enum Endings
    {
        None    = 0,
        Bad     = 1,
        Good    = 2,
        True    = 3,
        Dream   = 4,
    }
    
    public Script_CanvasGroupFadeInOut fader;
    [SerializeField] private Script_CanvasGroupController timelineFader;
    
    [SerializeField] private Script_CanvasGroupController underDialogueController;
    [SerializeField] private Script_Game game;
    [SerializeField] private Script_TimeManager timeManager;
    
    public const float RestartPlayerFadeInTime = 0.25f;
    public const float RestartPlayerFadeOutTime = 1f;
    public const float UnderDialogueFadeTime = 1.5f;
    public Script_CanvasGroupController restartPrompt;

    private Action onAllPuzzlesDoneCutsceneDone;
    private Script_GameOverController.DeathTypes deathType;
    
    public IEnumerator FadeIn(float t, Action action)
    {
        return fader.FadeInCo(t, action);
    }

    public IEnumerator FadeOut(float t, Action action)
    {
        return fader.FadeOutCo(t, action);
    }

    public void UnderDialogueBlackScreen(bool isOpen = true)
    {
        if (isOpen) underDialogueController.Open();
        else        underDialogueController.Close();
    }

    public void UnderDialogueFadeIn(float t, Action action)
    {
        underDialogueController.FadeIn(t, action);
    }
    
    public void UnderDialogueFadeOut(float t, Action action)
    {
        underDialogueController.FadeOut(t, action);
    }

    public void TimelineBlackScreen()
    {
        timelineFader.Open();
    }

    public void TimelineFadeIn(float t, Action action)
    {
        timelineFader.FadeIn(t, action);
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
    public void StartEndingSequence(Endings endingOverride = Endings.None)
    {
        if (endingOverride != Endings.None)     game.ActiveEnding = endingOverride;

        game.ChangeStateCutScene();
        Script_BackgroundMusicManager.Control.FadeOutSlow();

        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 2);        
    }

    /// <summary>
    /// Checks whether or not to play the final cut scene
    /// </summary>
    public bool OnCurrentQuestDone(Action cb = null)
    {
        if (game.IsAllQuestsDoneToday())
        {
            // Final Cut Scene
            game.ChangeStateCutScene();

            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 6);

            if (cb != null) onAllPuzzlesDoneCutsceneDone = cb;

            return true;
        }

        if (cb != null) cb();
        return false;
    }

    // ------------------------------------------------------------------
    // Timeline Signals
    public void OnTimesUpPlayableDone()
    {
        Time.timeScale = 1.0f;

        // Prompt Player
        FadeInRestartPrompt();
    }

    // Also called at end of AllPuzzlesDoneNotification Timeline before fade back in.
    public void HideLevelGrid(bool isActive = false)
    {
        // Set Current Inactive so will not disrupt Cut Scene.
        Script_LevelGrid currentGrid = game.levelBehavior.transform.GetParentRecursive<Script_LevelGrid>();
        if (currentGrid != null)
            currentGrid.gameObject.SetActive(isActive);
        else
            Debug.LogError($"Need to fix hierarchy so Script_LevelGrid is a parent above {game.levelBehavior}");
    }

    // Call at the beginning of activating Levels to Unhide Player
    public void HandlePlayerInEileensRoom()
    {
        if (game.IsInEileensRoom)
            game.UnhidePlayer();
    }

    public void HandlePlayerInIdsRoom()
    {
        if (game.IsInIdsRoom)
            game.UnhidePlayer();
    }

    public void HandlePlayerInElleniasRoom()
    {
        if (game.IsInElleniasRoom)
            game.UnhidePlayer();
    }

    public void HandlePlayerInBallroom()
    {
        if (game.IsInBallroom)
            game.UnhidePlayer();
    }

    public void OnAllPuzzlesDoneNotificationDone()
    {
        if (onAllPuzzlesDoneCutsceneDone != null)
        {
            onAllPuzzlesDoneCutsceneDone();
            onAllPuzzlesDoneCutsceneDone = null;
        }
    }

    // After screen has faded to Black play the proper timeline.
    public void PlayEndingCutScene()
    {
        switch (game.ActiveEnding)
        {
            case (Endings.Good):
                GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 4);
                break;
            case (Endings.True):
                GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 5);
                break;
            case (Endings.Dream):
                break;
        }
    }

    // After played proper ending cut scene.
    public void RollCredits()
    {
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 3);
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

    public void InitialState()
    {
        fader.GetComponent<Script_CanvasGroupController>().InitialState();
        timelineFader.InitialState();
    }
    
    public void Setup()
    {
        if (Control == null)
        {
            Control = this;
        }
        else if (Control != this)
        {
            Destroy(this.gameObject);
        }
        
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

        if (GUILayout.Button("True Ending"))
        {
            t.StartEndingSequence(Script_TransitionManager.Endings.True);
        }
    }
}
#endif