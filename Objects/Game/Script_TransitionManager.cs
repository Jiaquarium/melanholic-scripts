﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System;
using UnityEngine.EventSystems;

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

    public enum FinalNotificationWaitTimes
    {
        None = 0,
        Ellenia = 1,
        Ids = 2,
        Eileen = 3,
        WellsWorld = 4,
        CelestialGardens = 5,
        XXXWorld = 6
    }
    
    public enum Endings
    {
        None            = 0,
        Bad             = 1,
        Good            = 2,
        True            = 3,
        Dream           = 4,
    }

    public const float RestartPlayerFadeInTime = 0.25f;
    public const float RestartPlayerFadeOutTime = 1f;
    public const float FadeTimeSlow = 2.0f;
    public const float UnderDialogueFadeTime = 1.5f;

    private const float WaitForFinalFramingElleniaTime = 0.25f;
    private const float WaitForFinalFramingEileenTime = 0.25f;
    private const float WaitForFinalFramingIdsTime = 0f;
    private const float WaitForFinalFramingWellsWorldTime = 0.25f;
    // 0.25f will put frame up at end of zooming camera out back to default, which looks nice 
    private const float WaitForFinalFramingCelestialGardensTime = 0.25f;
    private const float WaitForFinalFramingXXXWorldTime = 0.25f;

    // Time to leave save progress message up. Check SaveViewManager.ShowSaveAndRestarMessage's
    // Fade In time, this must be set to a value >= so there is enough time to fade in the message.
    [SerializeField] private float restartGameTimeOnSave;
    [SerializeField] private float restartGameShowCompleteMessageTime;
    
    [Tooltip("Ensure is less than restartGameTimeOnSave to avoid clicking when changing Scenes.")]
    [SerializeField] private float fadeOutMusicTimeOnSave;
    
    [Tooltip("Wait time after clicking Restart... UI choice on Bad Ending The Sealing cut scene.")]
    [SerializeField] private float restartGameTimeOnBadEnding;
    
    [Tooltip("Wait time after clicking To Main Menu UI choice on Bad Ending The Sealing cut scene.")]
    [SerializeField] private float toTitleWaitTime;
    
    [Header("Fading Canvases")]
    [SerializeField] private Script_CanvasGroupController underDialogueController;
    public Script_CanvasGroupFadeInOut fader;
    [SerializeField] private Script_CanvasGroupController timelineUnderHUD;
    [SerializeField] private Script_CanvasGroupController timelineFaderUnder;
    [SerializeField] private Script_CanvasGroupController timelineFaderUnderWhite;
    [SerializeField] private Script_CanvasGroupController timelineFaderOver;
    [SerializeField] private Script_CanvasGroupController timelineFaderOverWhite;
    [SerializeField] private Script_CanvasGroupController faderOverWhite;
    [SerializeField] private Script_CanvasGroupController faderSceneTransition;

    [SerializeField] private Script_Game game;
    [SerializeField] private Script_TimeManager timeManager;

    [Header("Good Endings")]
    [SerializeField] private Script_GoodEndingController goodEndingController;
    [SerializeField] private int endingThemeGood;
    [SerializeField] private Script_BgThemePlayer oceanBgThemePlayer;
    [SerializeField] private float fadeOutEndingTime;
    [SerializeField] private float fadeOceanBgmBackTime;
    [SerializeField] private Script_CanvasGroupController endingsCanvasGroup;
    [SerializeField] private Script_CanvasGroupController goodEndingCanvasGroup;
    [SerializeField] private Script_CanvasGroupController endingsBgCanvasGroup;
    [Tooltip("Overlay canvas group for The End text and End message UI")]
    [SerializeField] private Canvas goodEndingOverlaysCanvas;

    [Header("True Ending Only")]
    [SerializeField] private int endingThemeTrue;
    [SerializeField] private float fadeOutMainMelodyTime;
    [SerializeField] private int thankYouThemeTrue;
    [SerializeField] private Script_CanvasGroupController trueEndingCanvasGroup;
    [SerializeField] private Script_CreditsController creditsController;

    [Header("Bad Ending")]
    [SerializeField] private float dieTimeScale;
    [SerializeField] private float timeScaleEaseInDuration;
    [SerializeField] private float onRestartSelectBgmFadeTime;
    [SerializeField] private Script_CanvasGroupController elevatorCanvasGroup;
    [SerializeField] private Script_CanvasGroupController KelsingorCanvasGroup;
    [SerializeField] private Script_CanvasGroupController SealingCanvasGroup;
    [SerializeField] private Script_CanvasGroupController restartPrompt;
    [SerializeField] private EventSystem restartPromptEventSystem;
    
    private float timeScaleTimer;
    private Coroutine timeScaleCoroutine;

    private Action onAllPuzzlesDoneCutsceneDone;
    private Script_GameOverController.DeathTypes deathType;
    private bool didPlayAllPuzzlesDoneCutScene = false;
    
    public float RestartGameTimeOnSave => restartGameTimeOnSave;

    public float RestartGameShowCompleteMessageTime => restartGameShowCompleteMessageTime;

    public float FadeOutMusicTimeOnSave => fadeOutMusicTimeOnSave;

    public float RestartGameTimeOnBadEnding => restartGameTimeOnBadEnding;

    public float ToTitleWaitTime => toTitleWaitTime;

    public IEnumerator FadeIn(float t, Action action, bool isUnscaledTime = false)
    {
        return fader.FadeInCo(t, action, isUnscaledTime: isUnscaledTime);
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

    public void TimelineUnderHUDBlackScreenOpen()
    {
        timelineUnderHUD.Open();
    }

    public void TimelineUnderHUDBlackScreenClose()
    {
        timelineUnderHUD.Close();
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

    public void FadeInSceneTransition(float t, Action action)
    {
        faderSceneTransition.FadeIn(t, action, isUnscaledTime: true);
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
        SetupTheSealingCanvasGroup();

        // Timeline will call Game.LevelsInactivate.

        void SetupTheSealingCanvasGroup()
        {
            elevatorCanvasGroup.Open();
            KelsingorCanvasGroup.Close();
        }
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
        if (endingOverride != Endings.None)
            game.ActiveEnding = endingOverride;

        game.ChangeStateCutScene();

        Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
            isOpen: true,
            framing: Script_UIAspectRatioEnforcerFrame.Framing.Ending,
            cb: () => {
                var bgm = Script_BackgroundMusicManager.Control;
                bgm.FadeOutSlow(bgm.Stop);

                if (endingOverride == Endings.True)
                    GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 11);
                else
                    GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 2);
            }
        );
    }

    /// <summary>
    /// Checks whether or not to play the final cut scene
    /// </summary>
    public bool OnCurrentQuestDone(
        Action allQuestsDoneCb = null,
        Action defaultCb = null,
        FinalNotifications type = FinalNotifications.Default,
        FinalNotificationWaitTimes waitType = FinalNotificationWaitTimes.None
    )
    {
        Dev_Logger.Debug($"{name} Check for All Puzzles Done Cut Scene");
        
        if (game.IsAllQuestsDoneToday() && !didPlayAllPuzzlesDoneCutScene)
        {
            // Final Cut Scene
            game.ChangeStateCutScene();

            // Hide King
            game.BallroomBehavior.SetKingEclaireActive(false);
            
            var bgm = Script_BackgroundMusicManager.Control;
            bgm.FadeOutMed(() => {
                bgm.Pause();
                bgm.SetVolume(1f);
            });

            StartCoroutine(WaitForFraming());

            if (allQuestsDoneCb != null)
                onAllPuzzlesDoneCutsceneDone = allQuestsDoneCb;
            
            didPlayAllPuzzlesDoneCutScene = true;

            return true;
        }

        if (defaultCb != null)
            defaultCb();
        
        return false;

        IEnumerator WaitForFraming()
        {
            float waitTime = waitType switch
            {
                FinalNotificationWaitTimes.Ellenia => WaitForFinalFramingElleniaTime,
                FinalNotificationWaitTimes.Eileen => WaitForFinalFramingEileenTime,
                FinalNotificationWaitTimes.Ids => WaitForFinalFramingIdsTime,
                FinalNotificationWaitTimes.WellsWorld => WaitForFinalFramingWellsWorldTime,
                FinalNotificationWaitTimes.CelestialGardens => WaitForFinalFramingCelestialGardensTime,
                FinalNotificationWaitTimes.XXXWorld => WaitForFinalFramingXXXWorldTime,
                _ => 0f,
            };
            
            yield return new WaitForSecondsRealtime(waitTime);
            
            // Framing (Thin looks better here, since ConstantDefault covers too much of each scene)
            Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
                isOpen: true,
                framing: Script_UIAspectRatioEnforcerFrame.Framing.ConstantThin,
                isNoAnimation: false,
                cb: HandleFinalAwakeningNotification
            );
        }

        void HandleFinalAwakeningNotification()
        {
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
        }
    }

    public void FinalCutSceneAwakening()
    {
        var PRCSManager = Script_PRCSManager.Control;

        PRCSManager.TalkingSelfSequence(() => {
            PRCSManager.SetAwakeningFinalActive(true);
            PRCSManager.PlayAwakeningFinalTimeline();
        });
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

        // The Sealing achievement
        Script_AchievementsManager.Instance.UnlockSealing();
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
        
    }
    
    // After screen has faded to Black play the proper timeline.
    public void PlayEndingCutScene()
    {
        var bgm = Script_BackgroundMusicManager.Control;
        
        bgm.Stop();
        bgm.SetVolume(1f);

        endingsCanvasGroup.Open();
        
        switch (game.ActiveEnding)
        {
            case (Endings.Good):
                goodEndingController.InitialState();
                StartCoroutine(WaitToPlayGoodEndingTimeline());
                break;
            case (Endings.True):
                GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 5);
                break;
            case (Endings.Dream):
                break;
        }

        IEnumerator WaitToPlayGoodEndingTimeline()
        {
            yield return new WaitForSecondsRealtime(Script_GoodEndingController.WaitToPlayGoodEndingTimeAfterFadeOut);
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 4);
        }
    }

    // GoodEndingTimeline, TrueEndingTimeline
    // Endings Timelines (at start) to play Ending Theme + Ocean Vibes SFX
    public void EndingBgm(bool isTrueEnding)
    {
        var endingTheme = isTrueEnding ? endingThemeTrue : endingThemeGood; 
        var bgm = Script_BackgroundMusicManager.Control;
        
        bgm.Play(endingTheme);
        oceanBgThemePlayer.gameObject.SetActive(true);
    }

    // Called from Ending Timeline, so the The End screen just has Ocean Vibes.
    public void TrueEndingFadeOutEndingMelody()
    {
        var bgm = Script_BackgroundMusicManager.Control;
        bgm.FadeOut(bgm.Stop, fadeOutMainMelodyTime, Const_AudioMixerParams.ExposedBGVolume);
    }

    public void RestartOceanBgThemePlayer()
    {
        oceanBgThemePlayer.FadeInPlay(fadeTime: fadeOceanBgmBackTime);
    }

    // TrueEndingAchievement signal from True Ending Timeline on the final pause waiting for key input
    public void TrueEndingAchievement()
    {
        Script_SaveCurseControl.Instance.Delete();
        Script_AchievementsManager.Instance.UnlockNauticalDawn();
    }
    
    // After played proper ending cut scene.
    public void RollCredits()
    {
        // For dev purposes; game should already be in cut scene from True Ending - The End
        game.ChangeStateCutScene();
        
        var bgm = Script_BackgroundMusicManager.Control;
        
        // Fade out Ocean SFX
        if (oceanBgThemePlayer?.gameObject.activeInHierarchy ?? false)
            oceanBgThemePlayer.FadeOutStop(null, fadeOutEndingTime);

        // Ensure BGM is Stopped
        bgm.Stop();        

        // Fade Out to black
        TimelineFadeIn(fadeOutEndingTime, () => {
            goodEndingCanvasGroup.gameObject.SetActive(false);
            goodEndingOverlaysCanvas.gameObject.SetActive(false);
            trueEndingCanvasGroup.gameObject.SetActive(false);

            // Set Borders back to Default
            Script_UIAspectRatioEnforcerFrame.Control.MatchBorders();

            // Remove BgCanvasGroup from EndingsFadeToBlackTimeline
            endingsBgCanvasGroup.gameObject.SetActive(false);
            
            StartCoroutine(NextFrameCredits());
        }, isOver: true);
        
        IEnumerator NextFrameCredits()
        {
            bgm.SetVolume(1f, Const_AudioMixerParams.ExposedBGVolume);
            bgm.SetVolume(1f, Const_AudioMixerParams.ExposedSFXVolume);
            bgm.SetVolume(1f, Const_AudioMixerParams.ExposedGameVolume);
            
            yield return null;
            
            TimelineRemoveBlackScreen(isOver: true);
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 3);
        }
    }

    // Credits Timeline
    public void ThankYouMelody()
    {
        var bgm = Script_BackgroundMusicManager.Control;
        bgm.Play(thankYouThemeTrue);
    }

    // Good Ending Timeline
    public void GoodEndingFadeOutBackToMainMenu()
    {
        var bgm = Script_BackgroundMusicManager.Control;
        
        // Fade out Ocean SFX
        if (oceanBgThemePlayer?.gameObject.activeInHierarchy ?? false)
            oceanBgThemePlayer.FadeOutStop(null, fadeOutEndingTime);

        // Ensure BGM is Stopped
        bgm.Stop();        

        // Fade Out to black
        TimelineFadeIn(fadeOutEndingTime, () => {
            goodEndingCanvasGroup.gameObject.SetActive(false);
            goodEndingOverlaysCanvas.gameObject.SetActive(false);
            trueEndingCanvasGroup.gameObject.SetActive(false);

            // Reset grain & vignette
            goodEndingController.PostProcessingInitialState();

            // Show black screen text. ~2.25f delay at start of timeline (to match black screen at GoodEnding start).
            // 2.0-2.5s looks the best to give some breathing room from The End screen.
            Script_PRCSManager.Control.OpenPRCSCustom(Script_PRCSManager.CustomTypes.AfterGoodEnding);
        }, isOver: true);
    }

    // Sealing Timeline
    public void PlayTheSealingBgm()
    {
        var bgm = Script_BackgroundMusicManager.Control;
        bgm.SetDefault(Const_AudioMixerParams.ExposedBGVolume);
        bgm.PlayElderTragedy();
    }

    // Sealing Timeline
    public void FramingOpen()
    {
        Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
            isOpen: true,
            framing: Script_UIAspectRatioEnforcerFrame.Framing.TheSealing,
            isNoAnimation: true
        );
    }

    // ------------------------------------------------------------------

    public void FadeOutOceanBgm(float t)
    {
        oceanBgThemePlayer.FadeOutStop(fadeTime: t);
    }
    
    public void FadeInRestartPrompt()
    {
        Dev_Logger.Debug("Show prompt to player on how they would like to restart");
        
        restartPrompt.Close();
        restartPrompt.FadeIn();
        restartPromptEventSystem.SetSelectedGameObject(restartPromptEventSystem.firstSelectedGameObject);   
    }

    public void FadeOutRestartPrompt()
    {
        Dev_Logger.Debug("Show prompt to player on how they would like to restart");
        
        restartPrompt.FadeOut(default, () => {
            restartPrompt.Close();
        });
    }

    // ------------------------------------------------------------
    // Restarting
    public void PlayRestartGameTimeline()
    {
        // Fade out BGM as Timeline fades screen out.
        var bgm = Script_BackgroundMusicManager.Control;
        bgm.FadeOut(null, onRestartSelectBgmFadeTime, Const_AudioMixerParams.ExposedBGVolume);
        
        // 1 sec long fade out
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 9);
    }

    public void PlayToTitleTimeline()
    {
        // Fade out BGM as Timeline fades screen out.
        var bgm = Script_BackgroundMusicManager.Control;
        bgm.FadeOut(null, onRestartSelectBgmFadeTime, Const_AudioMixerParams.ExposedMusicVolume, isUnscaledTime: true);
        bgm.FadeOutExtra(
            out Coroutine coroutine,
            null, onRestartSelectBgmFadeTime,
            Const_AudioMixerParams.ExposedFXVolume,
            isUnscaledTime: true
        );
        
        // 1 sec long fade out
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 10);
    }

    // Util for fading out to Title with custom fade time
    public void ToTitleFadeOut(float t)
    {
        // Fade out BGM as Timeline fades screen out.
        var bgm = Script_BackgroundMusicManager.Control;
        bgm.FadeOut(null, t, Const_AudioMixerParams.ExposedMusicVolume, isUnscaledTime: true);
        bgm.FadeOutExtra(
            out Coroutine coroutine,
            null, t,
            Const_AudioMixerParams.ExposedFXVolume,
            isUnscaledTime: true
        );

        faderSceneTransition.InitialState();
        faderSceneTransition.FadeIn(t, Script_SceneManager.ToTitleScene, isUnscaledTime: true);
    }

    // ------------------------------------
    // Restarting: Unity Events called Restart UI Choices
    public void ToTitleScreen()
    {
        // Disable event system to avoid being able to interact with UI after making selection.
        var eventSystem = EventSystem.current;
        if (eventSystem != null)
            eventSystem.sendNavigationEvents = false;
        
        game.ToTitle();
    }

    public void Restart()
    {
        // Disable event system to avoid being able to interact with UI after making selection.
        var eventSystem = EventSystem.current;
        if (eventSystem != null)
            eventSystem.sendNavigationEvents = false;
        
        game.Restart();
    }

    // ------------------------------------------------------------
    // Audio
    
    public void EnterMenuSFX()
    {
        var sfx = Script_SFXManager.SFX;
        sfx.Play(sfx.OpenCloseBook, sfx.OpenCloseBookVol);
    }

    // The Sealing choices try again
    public void SealingTryAgainSFX()
    {
        var sfx = Script_SFXManager.SFX;
        sfx.PlaySubmitTransition();
    }

    // The Sealing choices go to main menu
    public void SealingGoToMainMenuSFX()
    {
        var sfx = Script_SFXManager.SFX;
        sfx.PlaySubmitTransitionCancel();
    }

    // ------------------------------------
    // Initializing

    public void InitialState()
    {
        fader.GetComponent<Script_CanvasGroupController>().InitialState();

        InitialStateExcludingLevelFader();
    }

    public void InitialStateExcludingLevelFader()
    {
        underDialogueController.InitialState();
        timelineUnderHUD.InitialState();
        timelineFaderUnder.InitialState();
        timelineFaderUnderWhite.InitialState();
        timelineFaderOver.InitialState();
        timelineFaderOverWhite.InitialState();
        faderOverWhite.InitialState();
        faderSceneTransition.InitialState();
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
        
        restartPrompt.Close();

        endingsCanvasGroup.gameObject.SetActive(false);
        goodEndingCanvasGroup.gameObject.SetActive(false);
        goodEndingOverlaysCanvas.gameObject.SetActive(false);
        goodEndingController.Setup();
        trueEndingCanvasGroup.gameObject.SetActive(false);
        SealingCanvasGroup.gameObject.SetActive(false);
        endingsBgCanvasGroup.gameObject.SetActive(false);

        fader.GetComponent<Script_CanvasGroupController>().Close();
        
        underDialogueController.Close();
        timelineUnderHUD.Close();
        timelineFaderUnder.Close();
        timelineFaderUnderWhite.Close();
        timelineFaderOver.Close();
        timelineFaderOverWhite.Close();
        faderOverWhite.Close();
        faderSceneTransition.Close();

        creditsController.Setup();
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_TransitionManager))]
    public class Script_TransitionManagerTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_TransitionManager t = (Script_TransitionManager)target;
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

            if (GUILayout.Button("Final Cut Scene Awakening"))
            {
                t.FinalCutSceneAwakening();
            }
            
            if (GUILayout.Button("Bad Ending"))
            {
                t.TimesUpEffects();
            }

            if (GUILayout.Button("Good Ending"))
            {
                t.StartEndingSequence(Script_TransitionManager.Endings.Good);
            }

            if (GUILayout.Button("True Ending"))
            {
                t.StartEndingSequence(Script_TransitionManager.Endings.True);
            }

            if (GUILayout.Button("Roll Credits"))
            {
                t.RollCredits();
            }

            if (GUILayout.Button("To Title"))
            {
                t.PlayToTitleTimeline();
            }
        }
    }
#endif
}
