using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

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
    public bool IsSpecialIntro => game.faceOffCounter == 2 && !didIntro;
    private bool isSpecialIntroFraming;
    
    // ------------------------------------------------------------------

    private bool didMapNotification;

    protected override void OnEnable()
    {
        Script_GameEventsManager.OnLevelInitComplete                    += OnLevelInitCompleteEvent;
        Script_GameEventsManager.OnLevelBlackScreenDone                 += OnLevelBlackScreenDone;

        Script_ScarletCipherEventsManager.OnScarletCipherPiecePickUp    += OnScarletCipherPickUp;

        Script_TransitionsEventsManager.OnMapNotificationTeletypeDone   += OnMapNotificationTeletypeDone;
    }

    protected override void OnDisable()
    {
        Script_GameEventsManager.OnLevelInitComplete                    -= OnLevelInitCompleteEvent;
        Script_GameEventsManager.OnLevelBlackScreenDone                 -= OnLevelBlackScreenDone;

        Script_ScarletCipherEventsManager.OnScarletCipherPiecePickUp    -= OnScarletCipherPickUp;

        Script_TransitionsEventsManager.OnMapNotificationTeletypeDone   -= OnMapNotificationTeletypeDone;
    }

    void Start()
    {
        if (IsSpecialIntro)
        {
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
        Script_AchievementsManager.Instance.UnlockRaveStage();
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

    // ------------------------------------------------------------------
    
    public override void Setup()
    {
        base.Setup();

        foreach (var trigger in stageTriggers)
            trigger.gameObject.SetActive(!didDontKnowMeThought);
    }
}