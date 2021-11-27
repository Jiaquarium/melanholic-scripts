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
    private static string BGMParam = Const_AudioMixerParams.ExposedBGVolume;
    
    public static Script_TransitionManager Control;
    
    public enum FinalNotifications
    {
        Default         = 0,
        Ids             = 1,
        Ellenia         = 2
    }
    
    public enum Endings
    {
        None            = 0,
        Bad             = 1,
        Good            = 2,
        True            = 3,
        Dream           = 4,
    }

    // Time to leave save progress message up. Check SaveViewManager.ShowSaveAndRestarMessage's
    // Fade In time, this must be set to a value >= so there is enough time to fade in the message.
    [SerializeField] private float restartGameTimeOnSave;
    // Time to wait to restart game after Bad Ending cut scene.
    [SerializeField] private float restartGameTimeOnBadEnding;
    // Time to wait to before starting restart game timeline.
    [SerializeField] private float toTitleWaitTime;
    
    public Script_CanvasGroupFadeInOut fader;
    [SerializeField] private Script_CanvasGroupController timelineFaderOver;
    [SerializeField] private Script_CanvasGroupController timelineFaderUnder;
    
    [SerializeField] private Script_CanvasGroupController underDialogueController;
    [SerializeField] private Script_Game game;
    [SerializeField] private Script_TimeManager timeManager;

    [SerializeField] private float dieTimeScale;
    [SerializeField] private float timeScaleEaseInDuration;
    private float timeScaleTimer;
    private Coroutine timeScaleCoroutine;
    
    public const float RestartPlayerFadeInTime = 0.25f;
    public const float RestartPlayerFadeOutTime = 1f;
    public const float FadeTimeSlow = 2.0f;
    public const float UnderDialogueFadeTime = 1.5f;
    public Script_CanvasGroupController restartPrompt;

    [Header("Endings")]
    [SerializeField] private FadeSpeeds fadeOutEndingScreenSpeed;
    [SerializeField] private int endingTheme;
    [SerializeField] private int endingOceanTheme;

    private Action onAllPuzzlesDoneCutsceneDone;
    private Script_GameOverController.DeathTypes deathType;
    private bool didPlayAllPuzzlesDoneCutScene = false;
    
    public float RestartGameTimeOnSave
    {
        get => restartGameTimeOnSave;
    }

    public float RestartGameTimeOnBadEnding
    {
        get => restartGameTimeOnBadEnding;
    }

    public float ToTitleWaitTime
    {
        get => toTitleWaitTime;
    }
    
    public IEnumerator FadeIn(float t, Action action)
    {
        return fader.FadeInCo(t, action);
    }

    public Coroutine FadeInCoroutine(float t, Action cb = null)
    {
        return StartCoroutine(FadeIn(t, cb));
    }

    public IEnumerator FadeOut(float t, Action action)
    {
        return fader.FadeOutCo(t, action);
    }

    public Coroutine FadeOutCoroutine(float t, Action cb = null)
    {
        return StartCoroutine(FadeOut(t, cb));
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

    public void TimelineBlackScreen(bool isOver = false)
    {
        var canvasGroupController = isOver ? timelineFaderOver : timelineFaderUnder;

        canvasGroupController.Open();
    }

    public void TimelineRemoveBlackScreen(bool isOver = false)
    {
        var canvasGroupController = isOver ? timelineFaderOver : timelineFaderUnder;

        canvasGroupController.Close();
    }

    public void TimelineFadeIn(float t, Action action, bool isOver = false)
    {
        var canvasGroupController = isOver ? timelineFaderOver : timelineFaderUnder;
        
        canvasGroupController.FadeIn(t, action);
    }

    public void TimelineFadeOut(float t, Action action, bool isOver = false)
    {
        var canvasGroupController = isOver ? timelineFaderOver : timelineFaderUnder;
        
        canvasGroupController.FadeOut(t, action);
    }

    public void DieEffects(Script_GameOverController.DeathTypes _deathType)
    {
        deathType = _deathType;
        
        game.ChangeStateCutScene();

        /// Slow down time and fade screen to black
        Time.timeScale = dieTimeScale;
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
        // Time HUD Remain Up to indicate Time Up.
        Script_HUDManager.Control.IsTimesUp = true;
        
        game.ChangeStateCutScene();

        // Fade out BGM
        Script_BackgroundMusicManager bgm = Script_BackgroundMusicManager.Control;
        bgm.FadeOutMed(() => {
            bgm.Stop();
            bgm.SetVolume(1f, BGMParam);
        }, BGMParam);
        
        EaseToPausedTimeScale();
        
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 1);

        // Timeline will call Game.LevelsInactivate.
    }

    private void EaseToPausedTimeScale()
    {
        timeScaleTimer = timeScaleEaseInDuration;
        timeScaleCoroutine = StartCoroutine(DecrementTimeScale());
        
        IEnumerator DecrementTimeScale()
        {
            while (timeScaleTimer > 0)
            {
                Time.timeScale = timeScaleTimer / timeScaleEaseInDuration;
                timeScaleTimer -= Time.deltaTime;
                
                yield return null;
            }

            timeScaleCoroutine = null;
        }
    }

    /// <summary>
    /// Fade Screen to Black and store which Ending to play, signal to play Ending Cut Scene.
    /// </summary>
    public void StartEndingSequence(Endings endingOverride = Endings.None)
    {
        if (endingOverride != Endings.None)     game.ActiveEnding = endingOverride;

        game.ChangeStateCutScene();
        
        var bgm = Script_BackgroundMusicManager.Control;
        bgm.FadeOutSlow(bgm.Stop);

        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 2);
    }

    /// <summary>
    /// Checks whether or not to play the final cut scene
    /// </summary>
    public bool OnCurrentQuestDone(Action cb = null, FinalNotifications type = FinalNotifications.Default)
    {
        Debug.Log($"{name} Check for All Puzzles Done Cut Scene");
        
        if (game.IsAllQuestsDoneToday() && !didPlayAllPuzzlesDoneCutScene)
        {
            // Final Cut Scene
            game.ChangeStateCutScene();

            switch (type)
            {
                case (FinalNotifications.Ids):
                    // Ids timeline uses a custom timeline that considers the Player in the current room,
                    // since Control track results in deactivating the current room too early.
                    GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 7);
                    break;
                case (FinalNotifications.Ellenia):
                    // Same with Ellenia's room (Painting corresponding with Eileen's Mind Quest)
                    GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 8);
                    break;
                default:
                    GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 6);
                    break;
            }

            if (cb != null)
                onAllPuzzlesDoneCutsceneDone = cb;
            
            didPlayAllPuzzlesDoneCutScene = true;

            return true;
        }

        if (cb != null)
            cb();
        
        return false;
    }

    // ------------------------------------------------------------------
    // Timeline Signals
    public void OnTimesUpPlayableDone()
    {
        // Stop TimeScale ease if is still running.
        if (timeScaleCoroutine != null)
        {
            StopCoroutine(timeScaleCoroutine);
            timeScaleCoroutine = null;
        }
        
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
    
    // After Realization Text is done.
    public void OnStartTheEndScreen()
    {
        var bgm = Script_BackgroundMusicManager.Control;
        bgm.Play(endingOceanTheme);
    }
    
    // After screen has faded to Black play the proper timeline.
    public void PlayEndingCutScene()
    {
        var bgm = Script_BackgroundMusicManager.Control;
        
        bgm.Stop();
        bgm.SetVolume(1f);
        
        switch (game.ActiveEnding)
        {
            case (Endings.Good):
                bgm.Play(endingTheme);
                
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
        // Fade in Black
        var fadeTimeScreen = fadeOutEndingScreenSpeed.GetFadeTime();
        var bgm = Script_BackgroundMusicManager.Control;
        
        // Fade out BGM (1.5s)
        bgm.FadeOutSlow(null);
        
        // XSlow (2.0s)
        TimelineFadeIn(fadeTimeScreen, () => {
            StartCoroutine(NextFrameCredits());
        }, isOver: true);
        
        IEnumerator NextFrameCredits()
        {
            yield return null;
            
            TimelineRemoveBlackScreen(isOver: true);
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 3);
        }
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

    // ------------------------------------------------------------
    // Restarting
    public void PlayRestartGameTimeline()
    {
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 9);
    }

    public void PlayToTitleTimeline()
    {
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 10);
    }

    // ------------------------------------
    // Restarting: Unity Events called Restart UI Choices
    public void ToTitleScreen()
    {
        game.ToTitle();
    }

    public void Restart()
    {
        game.Restart();
    }

    // ------------------------------------------------------------

    public void InitialState()
    {
        fader.GetComponent<Script_CanvasGroupController>().InitialState();
        timelineFaderUnder.InitialState();
        timelineFaderOver.InitialState();
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