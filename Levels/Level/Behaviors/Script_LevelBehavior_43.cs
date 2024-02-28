using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Special Intro reaction will only be played when FaceOff 1 is already complete,
/// and will be saved in state (will only happen once unless Day isn't saved)
/// 
/// World Tiles References:
/// - Game
/// - World Tiles Controller
/// - LB46 Painting Entrances
/// </summary>

public class Script_LevelBehavior_43 : Script_LevelBehavior
{
    public static string MapName = Script_Names.CelestialGardensWorld;
    
    // ==================================================================
    // State Data

    public bool didIntro;
    
    // ==================================================================
    
    [SerializeField] private float waitBeforeIntroTime;
    [SerializeField] private Script_DialogueNode introNode;

    // ------------------------------------------------------------------
    // Intro Only
    
    [Space][Header("Intro")][Space]
    [SerializeField] private float specialCaseFadeInTime;
    [SerializeField] private float specialCaseWaitInBlackTime;
    [SerializeField] private float waitToPlayIntroDirectorTime;
    [SerializeField] private float bgmFadeInTimeIntro;
    [SerializeField] private float waitToFadeInBlackScreenTime;
    [SerializeField] private float fadeInBlackScreenTimeIntro;
    [SerializeField] private float blackScreenTimeIntro;
    [SerializeField] private float fadeOutBlackScreenTimeIntro;
    [SerializeField] private Script_PostProcessingSettings postProcessingVignette075;
    [SerializeField] private PlayableDirector introDirector;
    [SerializeField] private Script_GlitchFXManager glitchFXManager;
    [SerializeField] private Script_VCamera introZoomOutGameVCam;
    [SerializeField] private Script_TransitionManager transitionManager;
    [SerializeField] private Script_MapNotification mapNotification;
    [SerializeField] private Script_LevelCustomFadeBehavior levelCustomFadeBehavior;
    public bool IsSpecialIntro => (game.faceOffCounter == 1 || Const_Dev.IsSpecialIntroOnEntrances) && !didIntro;
    private bool isSpecialIntroFraming;
    
    // ------------------------------------------------------------------
    // Trailer Only
    
    [SerializeField] private PlayableDirector trailerDirector;
    [SerializeField] private float waitToPlayTrailerDirectorTime;
    
    // ------------------------------------------------------------------
    
    private bool didMapNotification;

    protected override void OnEnable()
    {
        Script_GameEventsManager.OnLevelInitComplete += OnLevelInitCompleteEvent;
        Script_GameEventsManager.OnLevelBlackScreenDone += OnLevelBlackScreenDone;

        Script_TransitionsEventsManager.OnMapNotificationTeletypeDone += OnMapNotificationTeletypeDone;
    }

    protected override void OnDisable()
    {
        Script_GameEventsManager.OnLevelInitComplete -= OnLevelInitCompleteEvent;
        Script_GameEventsManager.OnLevelBlackScreenDone -= OnLevelBlackScreenDone;

        Script_TransitionsEventsManager.OnMapNotificationTeletypeDone -= OnMapNotificationTeletypeDone;
    }

    void Start()
    {
        if (IsSpecialIntro)
        {
            // Set custom shadow distance for special intro
            Script_GraphicsManager.Control.SetCelestialGardensSpecialIntroShadowDistance();
            
            Script_BackgroundMusicManager.Control.SetVolume(0f, Const_AudioMixerParams.ExposedBGVolume);
            game.StopBgMusic();

            // Put up frame
            Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
                isOpen: true,
                framing: Script_UIAspectRatioEnforcerFrame.Framing.ConstantDefault,
                isNoAnimation: true
            );
            isSpecialIntroFraming = true;

            // Remove this black screen with timeline signal later
            // Timeline will then use the over canvas (since the Under canvas needs to be controlled
            // by script later)
            transitionManager.TimelineBlackScreen(isOver: false);
        }
    }
    
    protected override void Update()
    {
        if (Const_Dev.IsTrailerMode)
            HandleTrailerPan();
    }

    private void OnLevelBlackScreenDone()
    {
        if (!didMapNotification && !IsSpecialIntro)
        {
            Script_MapNotificationsManager.Control.PlayMapNotification(MapName);
            didMapNotification = true;
        }
    }
    
    private void OnLevelInitCompleteEvent()
    {
        if (IsSpecialIntro)
        {
            game.ChangeStateCutScene();
            PlaySpecialIntro();
        }
    }

    /// <summary>
    /// This will only happen when Special Intro happens now (slightly different vs. Wells World
    /// where the "fireplace" text continues until finding the Lantern, since that text works as a hint
    /// vs. this one is more only flavor)
    /// </summary>
    private void HandleIntroReaction()
    {
        game.ChangeStateCutScene();

        StartCoroutine(WaitToIntroDialogue());

        didIntro = true;

        IEnumerator WaitToIntroDialogue()
        {
            yield return new WaitForSeconds(waitBeforeIntroTime);
            Script_DialogueManager.DialogueManager.StartDialogueNode(introNode);            
        }        
    }

    // ----------------------------------------------------------------------
    // Next Node Action

    public void OnIntroDialogueDone()
    {
        if (isSpecialIntroFraming)
        {
            Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
                isOpen: false,
                framing: Script_UIAspectRatioEnforcerFrame.Framing.ConstantDefault,
                cb: game.ChangeStateInteract
            );

            isSpecialIntroFraming = false;
        }
        else
        {
            game.ChangeStateInteract();
        }
    }

    // ----------------------------------------------------------------------
    // Timeline Signals - Special Intro Only
    
    // Cel Gardens Intro Timeline
    public void OnFrameAfterStartIntro()
    {
        transitionManager.TimelineRemoveBlackScreen(isOver: false);
    }

    // Cel Gardens Intro Timeline
    public void SetScanlineTransitionGlitch(bool isOn)
    {
        if (isOn)
        {
            glitchFXManager.SetScanlineTransition();
            glitchFXManager.SetBlend(1f);
        }
        else
        {
            glitchFXManager.SetDefault();
            glitchFXManager.SetBlend(0f);
        }
    }

    public void SetTimelineSpeed(float newSpeed)
    {
        introDirector.playableGraph.GetRootPlayable(0).SetSpeed(newSpeed);
    }

    // Cel Gardens Intro Timeline
    // Switch camera
    public void PlayCustomMapNotification()
    {
        Script_MapNotificationsManager.Control.PlayMapNotification(
            MapName,
            type: Script_MapNotificationsManager.Type.SpecialIntro,
            isSFXOn: true
        );
        didMapNotification = true;

        // See Wells World (LB42) for detail
        introZoomOutGameVCam.SetPriority(2);
    }
    
    // ----------------------------------------------------------------------
    // Special Intro Only

    /// <summary>
    /// See Wells World (LB42) for detail
    /// </summary>
    public void SpecialCaseFadeIn()
    {
        if (IsSpecialIntro)
        {
            // Note: MUST SET for custom fade behavior to work properly
            levelCustomFadeBehavior.SpecialCaseFadeInTime = specialCaseFadeInTime;
            // Note: MUST SET for custom fade behavior to work properly
            levelCustomFadeBehavior.IsSpecialFadeIn = true;
        }
    }
    
    /// <summary>
    /// See Wells World (LB42) for detail
    /// </summary>
    public void SpecialCaseWaitInBlack()
    {
        if (IsSpecialIntro)
        {
            // Note: MUST SET for custom fade behavior to work properly
            levelCustomFadeBehavior.SpecialCaseWaitInBlackTime = specialCaseWaitInBlackTime;
            // Note: MUST SET for custom fade behavior to work properly
            levelCustomFadeBehavior.IsSpecialWaitInBlack = true;
        }
    }
    
    private void PlaySpecialIntro()
    {
        var bgm = Script_BackgroundMusicManager.Control;
        bgm.PlayFadeIn(
            bgm.CelestialGardensTheme,
            forcePlay: true,
            fadeTime: bgmFadeInTimeIntro,
            outputMixer: Const_AudioMixerParams.ExposedBGVolume,
            startTime: 0,
            isForceNewStartTime: true
        );
        
        StartCoroutine(WaitToPlay());

        IEnumerator WaitToPlay()
        {
            yield return new WaitForSeconds(waitToPlayIntroDirectorTime);

            introDirector.Play();

            Dev_Logger.Debug($"Playing special intro Time: {Time.time}");
        }
    }

    private void OnMapNotificationTeletypeDone(bool isWorldPaintingIntro)
    {
        Dev_Logger.Debug($"OnMapNotificationTeletypeDone isWorldPaintingIntro {isWorldPaintingIntro}");
        
        if (!isWorldPaintingIntro)
            return;
        
        StartCoroutine(WaitToFadeInBlackScreen());

        IEnumerator WaitToFadeInBlackScreen()
        {
            yield return new WaitForSeconds(waitToFadeInBlackScreenTime);
            
            // Fade black screen in
            transitionManager.TimelineFadeIn(
                fadeInBlackScreenTimeIntro,
                () => {
                    StartCoroutine(WaitToFadeOutBlackScreen());
                },
                isOver: false
            );
        }

        IEnumerator WaitToFadeOutBlackScreen()
        {
            // Revert priority that was set during Intro Timeline & ensure to reinit intro volume
            introDirector.Stop();
            introZoomOutGameVCam.SetPriority(0);

            // Revert shadow distance
            Script_GraphicsManager.Control.SetDefaultShadowDistance();
             
            yield return new WaitForSeconds(blackScreenTimeIntro);

            // Must wait at least a frame after stopping timeline to modify its objects
            postProcessingVignette075.gameObject.SetActive(false);
            postProcessingVignette075.InitialStateWeight();

            // Fade black screen out & remove map notification
            var mapNotificationManager = Script_MapNotificationsManager.Control;
            transitionManager.TimelineFadeOut(
                fadeOutBlackScreenTimeIntro,
                () => {
                    mapNotification.Close(
                        () => {
                            // Must reinitiate mapNotification for World Paintings because the default
                            // OnTeletypeDone is not called when it's a Special Intro
                            mapNotificationManager.InitialState();
                            
                            game.ChangeStateInteract();
                            
                            HandleIntroReaction();
                        },
                        mapNotificationManager.SpecialIntroFadeOutTime
                    );
                },
                isOver: false
            );
        }
    }
    
    // ------------------------------------------------------------------
    // Trailer Only

    private void HandleTrailerPan()
    {
        if (Input.GetButtonDown(Const_KeyCodes.TrailerCam))
        {
            trailerDirector.Stop();

            StartCoroutine(WaitToPlay());
        }

        IEnumerator WaitToPlay()
        {
            yield return new WaitForSeconds(waitToPlayTrailerDirectorTime);

            trailerDirector.Play();
        }
    }

    // ------------------------------------------------------------------

    public override void Setup()
    {
        base.Setup();
    }
    
    #if UNITY_EDITOR
    [CustomEditor(typeof(Script_LevelBehavior_43))]
    public class Script_LevelBehavior_43Tester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_LevelBehavior_43 t = (Script_LevelBehavior_43)target;
            if (GUILayout.Button("Play Special Intro"))
            {
                t.PlaySpecialIntro();
            }
        }
    }
    #endif
}