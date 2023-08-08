using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Same special intro state handling to Celestial Gardens, will only be played when it's time for 3rd
/// FaceOff, then saved into state (only playing once per game unless day is not saved)
/// </summary>

public class Script_LevelBehavior_44 : Script_LevelBehavior
{
    public const string MapName = Script_Names.XXXWorld;
    
    // ==================================================================
    // State Data

    public bool didIntro;
    public bool didDontKnowMeThought;
    public bool didTakeABow;
    
    // ==================================================================

    [SerializeField] private Script_InteractablePaintingEntrance[] paintingEntrances;
    [SerializeField] private Script_InteractablePaintingEntrance ballroomPaintingEntrance;
    
    // Doors to hide on Special Intro timeline
    [SerializeField] private List<Script_DoorExit> doorsToSaloon;
    [SerializeField] private List<GameObject> treeSetsAutumn;

    [SerializeField] private Script_ScarletCipherPiece[] scarletCipherPieces;

    [SerializeField] private Script_Trigger[] stageTriggers;

    [SerializeField] private float waitBeforeIntroTime;
    [SerializeField] private Script_DialogueNode introNode;
    [SerializeField] private Script_DialogueNode dontKnowMeNode;
    [SerializeField] private Script_DialogueNode takeABowNode;
    [SerializeField] private Script_DialogueNode takeABowDoneNode;
    [SerializeField] private float waitBeforeTakeABowDoneDialogueTime;

    [SerializeField] private float filmGrainEndingIntensity;
    [SerializeField] private float filmGrainBlendTime;
    [SerializeField] private Script_PostProcessingManager postProcessingManager;
    [SerializeField] private Script_PostProcessingSettings postProcessingSettings;

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
    [SerializeField] private PlayableDirector introDirector;
    [SerializeField] private Script_GlitchFXManager glitchFXManager;
    [SerializeField] private Script_VCamera introZoomOutGameVCam;
    [SerializeField] private Script_TransitionManager transitionManager;
    [SerializeField] private Script_MapNotification mapNotification;
    [SerializeField] private Script_LevelCustomFadeBehavior levelCustomFadeBehavior;
    
    private bool isSpecialIntroFraming;
    private bool didMapNotification;
    
    public bool IsSpecialIntro => (game.faceOffCounter == 2 || Const_Dev.IsSpecialIntroOnEntrances) && !didIntro;

    protected override void OnEnable()
    {
        Script_GameEventsManager.OnLevelInitComplete                    += OnLevelInitCompleteEvent;
        Script_GameEventsManager.OnLevelBlackScreenDone                 += OnLevelBlackScreenDone;

        Script_ScarletCipherEventsManager.OnScarletCipherPiecePickUp    += OnScarletCipherPickUp;

        Script_TransitionsEventsManager.OnMapNotificationTeletypeDone   += OnMapNotificationTeletypeDone;

        if (!IsSpecialIntro)
            SetDistanceVCamActive();
    }

    protected override void OnDisable()
    {
        Script_GameEventsManager.OnLevelInitComplete                    -= OnLevelInitCompleteEvent;
        Script_GameEventsManager.OnLevelBlackScreenDone                 -= OnLevelBlackScreenDone;

        Script_ScarletCipherEventsManager.OnScarletCipherPiecePickUp    -= OnScarletCipherPickUp;

        Script_TransitionsEventsManager.OnMapNotificationTeletypeDone   -= OnMapNotificationTeletypeDone;

        SetDistanceCamInactive();
    }

    void Start()
    {
        if (IsSpecialIntro)
        {
            // Set custom shadow distance for special intro
            Script_GraphicsManager.Control.SetXXXWorldSpecialIntroShadowDistance();
            
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

    // To only occur after Special Intro
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

    // Hide all Scarlet Cipher pieces when any is picked up on a World Tile.
    private void OnScarletCipherPickUp(int scarletCipherId)
    {
        if (scarletCipherId == scarletCipherPieces[0].ScarletCipherId)
        {
            foreach (var scarletCipherPiece in scarletCipherPieces)
            {
                scarletCipherPiece.UpdateActiveState();
            }
        }
    }
    
    // ------------------------------------------------------------------
    // Timeline Signals
    
    public void FinishQuestPaintings()
    {
        foreach (var painting in paintingEntrances)
        {
            painting.DonePainting();
        }

        ballroomPaintingEntrance.DonePainting();
    }

    // ------------------------------------------------------------------
    // Timeline Signals - Special Intro Only
    
    // XXXWorld Intro Timeline
    public void OnFrameAfterStartIntro()
    {
        transitionManager.TimelineRemoveBlackScreen(isOver: false);
        
        // Level should be covered by fader (controlled via Timeline) at this point, so okay to remove these
        // objects during timeline
        SetDoorsToSaloonActive(false);
        SetTreeSetsAutumnActive(false);
    }

    // XXXWorld Intro Timeline
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

    // XXX World Intro Timeline
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
    
    // ------------------------------------------------------------------
    // Next Node Actions

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

    public void OnDontKnowMeThoughtDone()
    {
        game.ChangeStateInteract();
    }

    public void StartFilmGrain()
    {
        postProcessingManager.InitialState();

        FilmGrain filmgrain = postProcessingManager.SetFilmGrainTakeABow(0f);
        postProcessingManager.BlendInFilmGrainIntensity(filmgrain, filmGrainEndingIntensity, filmGrainBlendTime);
    }
    
    public void TakeABowPRCS()
    {
        var bgmManager = Script_BackgroundMusicManager.Control;
        bgmManager.FadeOutXFast(bgmManager.Pause, Const_AudioMixerParams.ExposedBGVolume);

        // Take A Bow should only have vignette, no film grain during the cutscene
        postProcessingSettings.CloseFilmGrain();
        
        var vignette = postProcessingSettings.SetVignetteTakeABow();
        Script_PostProcessingSettings.SetRefVignetteActive(ref vignette, true);

        Script_PRCSManager.Control.OpenPRCSCustom(Script_PRCSManager.CustomTypes.TakeABow);
    }

    public void OnTakeABowDoneDialogueDone()
    {
        game.ChangeStateInteract();
        UnlockRaveStageAchievement();
    }

    // ------------------------------------------------------------------
    // Unity Event Triggers
    
    // PlayerEnterOnce DontKnowMe Trigger
    public void DontKnowMeThought()
    {
        if (!didDontKnowMeThought)
        {
            didDontKnowMeThought = true;
            game.ChangeStateCutScene();
            Script_DialogueManager.DialogueManager.StartDialogueNode(dontKnowMeNode);
        }
    }

    // Rave Stage Trigger
    public void HandleRaveAchievement()
    {
        // If already did cut scene doublecheck Rave Stage achievement in case Player exited out before achievement 
        if (didTakeABow)
            UnlockRaveStageAchievement();
        else
        {
            didTakeABow = true;
            game.ChangeStateCutScene();
            Script_DialogueManager.DialogueManager.StartDialogueNode(takeABowNode);
        }
    }

    // ------------------------------------------------------------------
    // Timeline Signals

    public void OnTakeABowDone()
    {
        Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
            isOpen: false,
            framing: Script_UIAspectRatioEnforcerFrame.Framing.TakeABow,
            isNoAnimation: true
        );

        Script_PRCSManager.Control.ClosePRCSCustom(Script_PRCSManager.CustomTypes.TakeABow, () => {
            glitchFXManager.SetBlend(0f);
            
            // Close Vignette and reset Film Grain props
            postProcessingManager.InitialState();

            var bgmManager = Script_BackgroundMusicManager.Control;
            bgmManager.SetVolume(0f, Const_AudioMixerParams.ExposedBGVolume);
            bgmManager.FadeInXSlow(null, Const_AudioMixerParams.ExposedBGVolume);
            bgmManager.UnPause();
            
            StartCoroutine(WaitToTakeABowDoneDialogue());
        });

        IEnumerator WaitToTakeABowDoneDialogue()
        {
            yield return new WaitForSeconds(waitBeforeTakeABowDoneDialogueTime);
            
            Script_DialogueManager.DialogueManager.StartDialogueNode(takeABowDoneNode);
        }
    }

    public void StartGlitch()
    {
        glitchFXManager.SetXHigh();
        glitchFXManager.SetBlend(1f);
    }

    // ------------------------------------------------------------------
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
            bgm.XXXWorldTheme,
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
            // Revert priority that was set during Intro Timeline
            introDirector.Stop();
            introZoomOutGameVCam.SetPriority(0);

            // Revert shadow distance
            Script_GraphicsManager.Control.SetDefaultShadowDistance();

            SetDistanceVCamActive();
            game.SnapActiveCam(game.GetPlayer().transform.position);
             
            SetDoorsToSaloonActive(true);
            SetTreeSetsAutumnActive(true);
            
            yield return new WaitForSeconds(blackScreenTimeIntro);

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

    private void SetDoorsToSaloonActive(bool isActive)
    {
        doorsToSaloon.ForEach(door => door.gameObject.SetActive(isActive));
    }

    private void SetTreeSetsAutumnActive(bool isActive)
    {
        treeSetsAutumn.ForEach(treeSet => treeSet.gameObject.SetActive(isActive));
    }

    private void SetDistanceVCamActive()
    {
        var vCamManager = Script_VCamManager.VCamMain;
        if (vCamManager != null)
            vCamManager.SetNewVCam(distanceVCam);
    }

    private void SetDistanceCamInactive()
    {
        var vCamManager = Script_VCamManager.VCamMain;
        if (vCamManager != null)
            vCamManager.SwitchToMainVCam(distanceVCam);
    }

    private void UnlockRaveStageAchievement() => Script_AchievementsManager.Instance.UnlockRaveStage();

    // ------------------------------------------------------------------
    
    public override void Setup()
    {
        base.Setup();

        foreach (var trigger in stageTriggers)
            trigger.gameObject.SetActive(!didDontKnowMeThought);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_LevelBehavior_44))]
public class Script_LevelBehavior_44Tester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_LevelBehavior_44 t = (Script_LevelBehavior_44)target;
        if (GUILayout.Button("Take A Bow PRCS"))
        {
            t.TakeABowPRCS();
        }
    }
}
#endif