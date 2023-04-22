using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Note:the flow to end the game with Last Elevator Effect will leave some mixers at 0f volume from exit fade.
/// Last Elevator Effect will actually fade out "Music" mixer without pausing any speakers, until confirm yes,
/// where it will Pause every audio except the DroneLoud bgThemePlayer and "Music" and "SFX" will be reset.
/// </summary>
public class Script_ElevatorManager : MonoBehaviour
{
    /// Ensure matches level behavior's Elevator property name
    [SerializeField] private string elevatorName;  
    [SerializeField] private Script_TimelineController elevatorTimelineController;
    [SerializeField] private Script_CanvasGroupController elevatorCanvasGroupController;

    [SerializeField] private bool isExitSFXSilent = true;
    [SerializeField] private Script_Exits.ExitType exitType = Script_Exits.ExitType.Elevator;

    [SerializeField] private Script_ElevatorBehavior currentExitBehavior;
    [SerializeField] private Script_Elevator currentElevator;

    [SerializeField] private Model_Exit currentExitData;

    [SerializeField] private Script_ExitMetadataObject grandMirrorEntrance;

    [SerializeField] private CanvasGroup countdownCanvasGroup;
    [SerializeField] private CanvasGroup lastElevatorMessageCanvasGroup;

    [SerializeField] private FadeSpeeds lastElevatorPromptFadeInTime;
    [SerializeField] private float waitToRevealLastElevatorChoicesTime;

    [SerializeField] private Script_Game game;
    [SerializeField] private Script_BackgroundMusicManager bgm;

    [SerializeField] private bool isBgmOn = true;

    [Space][Header("Last Elevator Effect")][Space]
    [SerializeField] private Script_CanvasGroupController lastElevatorPromptController;
    [SerializeField] private Script_CanvasGroupController lastElevatorPromptChoicesController;
    [SerializeField] private Script_LastElevatorEffectController lastElevatorEffectController;
    [SerializeField] private float bgmFadeOutTime;
    [SerializeField] private Script_BgThemePlayer droneLoudBgPlayer;
    [SerializeField] private float droneLoudFadeOutTime;
    [SerializeField] private float waitBeforeSaveDialogueTime;
    [SerializeField] private Script_ExitMetadataObject lobbySpawn;
    [SerializeField] private int bayV1BgmIdx;
    [SerializeField] private float bgmFadeInTime;
    [SerializeField] private EventSystem lastElevatorEffectChoicesEventSystem;
    [SerializeField] private AudioSource interactionUISource;
    [SerializeField] private PlayableDirector elevatorCanvasGroupDirector;

    [SerializeField] private Image elevatorLeftImage;
    [SerializeField] private Image elevatorRightImage;
    [SerializeField] private Vector3 elevatorLeftDefaultPosition;
    [SerializeField] private Vector3 elevatorRightDefaultPosition;

    private Coroutine musicCoroutine;
    private Coroutine fxCoroutine;
    private bool isElevatorEffect;

    public bool IsBgmOn
    {
        get => isBgmOn;
        set => isBgmOn = value;
    }

    public bool IsFinishingLastElevatorTimeline { get; private set; }

    void OnEnable()
    {
        Script_CombatEventsManager.OnHitCancelUI += OnHitCancelUI;
    }

    void OnDisable()
    {
        Script_CombatEventsManager.OnHitCancelUI -= OnHitCancelUI;
    }
    
    /// <summary>
    /// UI Closes Elevator Doors
    /// We also set the currentExitBehavior that was passed from the From:Elevator
    /// to be passed to the To:Elevator and called when player is about to interact
    /// -> OnDoorsClosed()
    /// </summary>
    public void CloseDoorsCutScene(
        Script_ExitMetadataObject exit,
        Script_ElevatorBehavior exitBehavior,
        Script_Elevator.Types type,
        Model_Exit exitOverrideData = null,
        Script_Exits.ExitType? exitTypeOverride = null
    )
    {
        // For Last Elevator Sticker.
        currentExitData = exitOverrideData ?? exit?.data;
        
        if (exitTypeOverride != null)
            exitType = (Script_Exits.ExitType)exitTypeOverride;
        
        currentExitBehavior = exitBehavior;
        elevatorCanvasGroupController.Open();
        
        switch (type)
        {
            case (Script_Elevator.Types.Default):
                elevatorTimelineController.PlayableDirectorPlayFromTimelines(0, 0);
                break;
            case (Script_Elevator.Types.Last):
                elevatorTimelineController.PlayableDirectorPlayFromTimelines(0, 1);
                break;
            case (Script_Elevator.Types.GrandMirror):
                // Send Player to Grand Mirror room.
                currentExitData = grandMirrorEntrance.data;
                
                elevatorTimelineController.PlayableDirectorPlayFromTimelines(0, 2);
                break;
            case (Script_Elevator.Types.Effect):
                elevatorTimelineController.PlayableDirectorPlayFromTimelines(0, 3);
                break;
        }
    }

    // ------------------------------------------------------------------
    // Timeline Signals
    
    // Start of LastElevatorEffectTimeline
    public void OnLastElevatorEffectStart()
    {
        isElevatorEffect = true;
        
        // Don't stop current coroutines. This will allow a chance for leftover bgm coroutines
        // to finish up, which will still work since we modify a parent mixer, and don't pause anything.
        bgm.FadeOutExtra(
            out musicCoroutine,
            () => {
                droneLoudBgPlayer.gameObject.SetActive(true);
            },
            bgmFadeOutTime,
            outputMixer: Const_AudioMixerParams.ExposedMusicVolume
        );
        bgm.FadeOutExtra(out fxCoroutine, null, bgmFadeOutTime, Const_AudioMixerParams.ExposedFXVolume);
    }

    // Start of LastElevatorEffectTimeline_No
    public void OnLastElevatorNo()
    {
        droneLoudBgPlayer.FadeOutStop(null, droneLoudFadeOutTime);
    }

    // LastElevatorEffectTimeline_No: LastElevatorNoFadeInBgm
    public void LastElevatorNoFadeInBgm()
    {
        bgm.FadeInExtra(out musicCoroutine, null, bgmFadeInTime, Const_AudioMixerParams.ExposedMusicVolume);
        bgm.FadeInExtra(out fxCoroutine, null, bgmFadeInTime, Const_AudioMixerParams.ExposedFXVolume);
    }
    
    /// <summary>
    /// Called when elevator UI canvas done closing
    /// Calls any exit behaviors right after Game.Exit() is called
    /// </summary>
    public void OnDoorsClosed()
    {
        ExitBehavior();
        
        Dev_Logger.Debug("@@@@@@@@@ {name} CURRENT EXIT DATA @@@@@@@@@");
        Script_Utils.DebugToConsole(currentExitData);
        
        /// Set up the new level in the background
        Script_Game.Game.Exit(
            currentExitData.level,
            currentExitData.playerSpawn,
            currentExitData.facingDirection,
            true,
            isExitSFXSilent,
            exitType
        );

        currentExitData = null;
        SetInitialElevatorState();

        void ExitBehavior()
        {
            currentExitBehavior?.Effect();
            currentExitBehavior = null;
        }

        void SetInitialElevatorState()
        {
            /// Start with elevator doors open if transported to a Bay
            Script_LevelBehavior currentLevelBehavior = Script_Game.Game.levelBehavior;
            if (currentLevelBehavior.HasField(elevatorName))
            {
                Dev_Logger.Debug($"Setting initial state of: {elevatorName}");

                currentElevator = currentLevelBehavior.GetField<Script_Elevator>(elevatorName);
                
                if (!currentElevator.gameObject.activeSelf)
                {
                    Debug.LogWarning("The Elevator exposed by Level Behavior is inactive");
                }

                currentElevator.SetClosedState(false);
            }
            else
            {
                currentElevator = null;
                
                Dev_Logger.Debug($"You are not exposing a public {elevatorName} property on current Level Behavior");
            }
        }
    }

    /// <summary>
    /// Called when elevator UI canvas done opening 
    /// </summary>
    public void OnDoorsOpened()
    {
        elevatorCanvasGroupController.Close();

        /// Animate doors closed
        Dev_Logger.Debug("Done opening UI elevator doors; animate World elevator doors closing");
        
        // If there exists a World Elevator start animating it
        if (currentElevator != null)
        {
            currentElevator.SetClosing();
        }
        else
        {
            Dev_Logger.Debug($"{name} There is no current World Elevator, changing state to interact");
            
            game.ChangeStateInteract();
        }

        bgm.FadeInMed(outputMixer: Const_AudioMixerParams.ExposedBGVolume);
        
        // When entering Bay v1 from Last Elevator, the BGM will be paused via Behavior.
        if (!bgm.IsPlaying)
        {
            Dev_Logger.Debug("Start BGM from Elevator Manager");
            game.StartBgMusicNoFade();
        }
        
        IsBgmOn = true;
    }

    // LastElevatorCanvasTimeline
    public void OpenLastElevatorPrompt()
    {
        lastElevatorPromptController.FadeIn(lastElevatorPromptFadeInTime.ToFadeTime());
    }

    // LastElevatorCanvasTimeline_No
    public void OnLastElevatorCanceledTimelineDone()
    {
        elevatorCanvasGroupController.Close();
        Script_Game.Game.ChangeStateInteract();

        isElevatorEffect = false;
    }

    // Note: Bgm will be faded out at this point.
    public void OnEffectYesFadeInBgm()
    {
        try
        {
            Dev_Logger.Debug($"{name} Pausing all speakers & bgm except Drone bgThemePlayer");

            // Pause all bgm before resetting volumes
            var audioConfig = Script_AudioConfiguration.Instance;
            audioConfig.RemoveSpeaker(droneLoudBgPlayer);
            Script_AudioConfiguration.Instance.PauseAll();
            audioConfig.AddSpeaker(droneLoudBgPlayer);
            
            Dev_Logger.Debug($"{name} Resetting volume for Music and SFX mixer params");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"{name} Failed timeline Last Elevator Yes signal with: {e}");
        }
    }

    public void OnEffectYesTimelineDone()
    {
        var exitType = Script_Exits.ExitType.SaveAndRestart;
        
        if (game.IsLastElevatorSaveAndStartWeekendCycle())
            exitType = Script_Exits.ExitType.SaveAndStartWeekendCycle;
        
        StartCoroutine(WaitBeforeSave());

        isElevatorEffect = false;
        
        IEnumerator WaitBeforeSave()
        {
            yield return new WaitForSeconds(waitBeforeSaveDialogueTime);

            // Note: On save, player state is overriden anyways in game.NextRunSaveInitialize, so
            // actually don't need to pass in lobbySpawn here.
            game.Exit(
                lobbySpawn.data.level,
                lobbySpawn.data.playerSpawn,
                lobbySpawn.data.facingDirection,
                isExit: false,
                isSilent: true,
                exitType
            );
        }
    }

    // ------------------------------------------------------------------
    // Unity Events
    
    // LastElevatorPrompt UI Text OnTypingDone Event
    public void OpenLastElevatorPromptChoices()
    {
        // Wait before showing choices
        StartCoroutine(WaitToRevealChoices());        

        IEnumerator WaitToRevealChoices()
        {
            yield return new WaitForSeconds(waitToRevealLastElevatorChoicesTime);
            lastElevatorPromptChoicesController.FadeIn(lastElevatorPromptFadeInTime.ToFadeTime());
        }
    }
    
    // LastElevatorPrompt UI Choices: Yes
    public void LastElevatorConfirmedTimeline()
    {
        // Prevent multiple calls in case button press and escape land on same frame
        if (IsFinishingLastElevatorTimeline)
            return;
        IsFinishingLastElevatorTimeline = true;
        
        lastElevatorEffectChoicesEventSystem.sendNavigationEvents = false;
        lastElevatorEffectController.gameObject.SetActive(false);

        // Prevent player from being affected by spikes, since this flow should carry player to Day End
        game.GetPlayer().isInvincible = true;

        var sfx = Script_SFXManager.SFX;
        interactionUISource.PlayOneShot(sfx.SubmitTransition, sfx.SubmitTransitionVol);
        
        var blackFadeInTime = FadeSpeeds.Med.GetFadeTime();
        
        // Fade Screen to Black
        Script_TransitionManager.Control.TimelineFadeIn(blackFadeInTime, () => {
            // Remove prompt
            lastElevatorPromptController.Close();
            lastElevatorPromptChoicesController.Close();
            IsFinishingLastElevatorTimeline = false;

            // Remove Fader
            Script_TransitionManager.Control.TimelineFadeOut(FadeSpeeds.Fast.GetFadeTime(), null, isOver: true);
            
            // Note: Fader takes 0.25 seconds to fade out, ensure Timeline has at least this much
            // of a pause before showing an event.
            // Play Yes Timeline
            if (game.IsLastElevatorSaveAndStartWeekendCycle())
                elevatorTimelineController.PlayableDirectorPlayFromTimelines(0, 6);
            else
                elevatorTimelineController.PlayableDirectorPlayFromTimelines(0, 4);
        }, isOver: true);
    }

    // LastElevatorPrompt UI Choices: No
    public void LastElevatorCanceledTimeline()
    {
        // Prevent multiple calls in case button press and escape land on same frame
        if (IsFinishingLastElevatorTimeline)
            return;
        IsFinishingLastElevatorTimeline = true;
        
        lastElevatorEffectChoicesEventSystem.sendNavigationEvents = false;
        lastElevatorEffectController.gameObject.SetActive(false);

        var sfx = Script_SFXManager.SFX;
        interactionUISource.PlayOneShot(sfx.SubmitTransitionNegative, sfx.SubmitTransitionNegativeVol);
        
        Script_TransitionManager.Control.TimelineFadeIn(FadeSpeeds.Med.GetFadeTime(), () => {
            // Remove prompt
            lastElevatorPromptController.Close();
            lastElevatorPromptChoicesController.Close();
            IsFinishingLastElevatorTimeline = false;

            // Remove Fader
            Script_TransitionManager.Control.TimelineFadeOut(FadeSpeeds.Fast.GetFadeTime(), null, isOver: true);
            
            // Note: Fader takes 0.25 seconds to fade out, ensure Timeline has at least this much
            // of a pause before showing an event.
            // Play No Timeline
            elevatorTimelineController.PlayableDirectorPlayFromTimelines(0, 5);
        }, isOver: true);
    }

    // Called from Last Elevator Prompt Choices Slow Awake EventSystem
    public void SetLastElevatorEffectInputManagerActive()
    {
        lastElevatorEffectController.gameObject.SetActive(true);
    }

    // ------------------------------------------------------------------

    /// <summary>
    /// Used specifically to cancel the Last Elevator Effect timelines on an interrupting event like hit
    /// </summary>
    private void InitialState()
    {
        // Stop detecting input for choice if it's up
        if (lastElevatorEffectChoicesEventSystem.gameObject.activeInHierarchy)
            lastElevatorEffectChoicesEventSystem.sendNavigationEvents = false;

        // Remove choice canvas & input manager
        lastElevatorPromptController.Close();
        lastElevatorPromptChoicesController.Close();
        lastElevatorEffectController.gameObject.SetActive(false);
        IsFinishingLastElevatorTimeline = false;

        // Stop the current timeline
        if (
            elevatorCanvasGroupDirector.playableGraph.IsValid()
            && elevatorCanvasGroupDirector.playableGraph.IsPlaying()
        )
            elevatorCanvasGroupDirector.Stop();
        
        // Set Elevator Door Images to default positions
        elevatorLeftImage.rectTransform.anchoredPosition = elevatorLeftDefaultPosition;
        elevatorRightImage.rectTransform.anchoredPosition = elevatorRightDefaultPosition;
        
        // Cancel any of the Audio mixer coroutines
        bgm.StopExtraCoroutine(ref musicCoroutine);
        bgm.StopExtraCoroutine(ref fxCoroutine);

        // Reset mixers & drone player
        bgm.SetVolume(1f, Const_AudioMixerParams.ExposedMusicVolume);
        bgm.SetVolume(1f, Const_AudioMixerParams.ExposedFXVolume);
        droneLoudBgPlayer.SoftStop();

        elevatorCanvasGroupController.Close();
    }

    // Note: this event will not be called on hit when Player is invincible
    private void OnHitCancelUI(Script_HitBox hitBox, Script_HitBoxBehavior hitBoxBehavior)
    {
        if (isElevatorEffect)
        {
            bool isStateHandled = (hitBoxBehavior != null && hitBoxBehavior.IsHitBoxBehaviorStateChanging())
                || Script_ClockManager.Control.IsClockDoneState;
            
            Dev_Logger.Debug($"{name} OnHitCanceLUI isStateHandled {isStateHandled}");
            
            InitialState();

            if (!isStateHandled)
                game.ChangeStateInteract();
        }
    }

    public void Setup()
    {
        elevatorCanvasGroupController.Close();
        lastElevatorPromptController.Close();
        lastElevatorPromptChoicesController.Close();

        // Do not set alpha because they will only be controlled
        // via Timeline Activation without animating the alpha.
        countdownCanvasGroup.gameObject.SetActive(false);
        lastElevatorMessageCanvasGroup.gameObject.SetActive(false);

        droneLoudBgPlayer.gameObject.SetActive(false);

        elevatorLeftDefaultPosition = elevatorLeftImage.rectTransform.anchoredPosition;
        elevatorRightDefaultPosition = elevatorRightImage.rectTransform.anchoredPosition;

        lastElevatorEffectController.gameObject.SetActive(false);
        IsFinishingLastElevatorTimeline = false;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_ElevatorManager))]
    public class Script_ElevatorManagerTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_ElevatorManager t = (Script_ElevatorManager)target;
            if (GUILayout.Button("Elevator Effect Initial State"))
            {
                t.InitialState();
            }
        }
    }
#endif
}