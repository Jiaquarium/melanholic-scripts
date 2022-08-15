using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Respawns are handled with IdsSpawns which will auto reset Ids positions
/// timelines to play are tracked with timelinesDoneCount
/// Uses MoveSets for approaching player
/// 
/// Events:
/// Ids should only be here on Sunday
/// </summary>
[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_10 : Script_LevelBehavior
{
    public const string MapName = Script_Names.Basement;
    
    public const string NRoomTriggerId = "room_N";
    public const string ERoomTriggerId = "room_E";

    public enum DialogueState
    {
        Weekend = 0,
        Weekday = 9,
    }
    
    // =======================================================================
    //  STATE DATA
    public bool gotBoarNeedle;
    // =======================================================================
    
    public bool isCurrentPuzzleComplete;
    
    // [SerializeField] private DialogueState dialogueState;

    [SerializeField] private int clubMusicIdx;
    [SerializeField] private int sadIdsThemeIdx;
    
    [SerializeField] private Script_Trigger[] triggers;
    [SerializeField] private int activeTriggerIndex;
    [SerializeField] private int timelinesDoneCount;
    
    // Tells us where Ids should spawn.
    [SerializeField] private Script_Marker[] IdsSpawns;
    
    public bool isInitialized;


    public float lightsFadeOutTime;
    public float dropDiscoBallTime;
    public float postIdsDanceWaitTime;
    

    public Script_DialogueManager dm;
    public Script_DDRManager DDRManager;
    [SerializeField] private Script_DDRConductor DDRConductor;
    
    public Script_LevelBehavior_9 lb9;
    public Script_BgThemePlayer IdsBgThemePlayerPrefab;

    [SerializeField] private Script_DialogueNode introNodeTalkedWithMyne;
    [SerializeField] private Script_DialogueNode introNodeNotTalkedWithMyne;
    [SerializeField] private float waitBeforeIdsDialogueTime;

    [SerializeField] private Script_DialogueNode afterIntroRevealNodeTalkedWithMyne;
    [SerializeField] private Script_DialogueNode afterIntroRevealNodeNotTalkedWithMyne;

    public Script_DialogueNode danceIntroNode;
    public Script_DialogueNode playerDanceIntroNode;
    public Script_DialogueNode badDanceOutroNode;
    public Script_DialogueNode goodDanceOutroNode;

    [SerializeField] private Script_DialogueNode[] defaultRetryNodes;
    
    public GameObject crystalChandelier;
    public Script_LightFadeIn playerSpotLight;
    public Script_LightFadeIn IdsSpotLight;
    
    [SerializeField] private Script_LightFadeIn levelDirectionalLight;
    [SerializeField] private float directionalLightDanceIntensity;
    
    [SerializeField] private Script_LightFadeIn[] additionalFadingLights;
    [SerializeField] private Script_MoveDirection[] movingLights;
    public Vector3 lightsUpOffset;
    public Vector3 lightsDownOffset;
    
    public Vector3 crystalChandelierDownOffset;
    public Vector3 crystalChandelierUpOffset;
    public AudioMixer audioMixer;
    
    public Model_SongMoves playerSongMoves;
    public Model_SongMoves IdsSongMoves;
    
    [SerializeField] private int mistakesAllowed;
    [SerializeField] private int maxMistakesAllowed;
    [SerializeField] private int increaseAllotmentInterval;
    [SerializeField] private int currentTry;

    [SerializeField] private List<float> bgTransitionTimes;
    [SerializeField] private Canvas StarryNightBg;
    [SerializeField] private Script_TextureScroller StarryNightScroller;
    [SerializeField] private Script_MeshFadeController BlackBgFader;
    [SerializeField] private float blackBgFadeInTime;
    [SerializeField] private float blackBgFadeOutTime;
    [SerializeField] private float startStarryNightScrollSpeed;
    [SerializeField] private float maxStarryNightScrollSpeed;
    [SerializeField] private float starryNightMaxScrollSpeedTimeToReach;

    [SerializeField] private List<Script_Firework> fireworks;
    
    public Script_MovingNPC Ids;
    [SerializeField] private PlayableDirector IdsDirector;
    
    [SerializeField] private Script_VCamera VCamLB10FollowIds;
    [SerializeField] private Script_VCamera VCamLB10Dance;
    
    // After Dance, eases out to this cam which shows both Ids and Player.
    [SerializeField] private Script_VCamera VCamLB10AfterDanceFailEase;
    
    // After Dance, eases out to this cam which shows both Ids and Player.
    // Eases out FAST back to Main Cam right before Item Receive.
    [SerializeField] private Script_VCamera VCamLB10AfterDanceSuccessEase;
    
    [SerializeField] private float VCamDanceSuccessToMainBlendTime;
    
    [SerializeField] private PlayableDirector danceSetupDirector;
    [SerializeField] private Script_Marker playerDancePosition;
    [SerializeField] private bool didIdsDance = false;
    
    [SerializeField] private Script_PRCSPlayer namePlatePRCSPlayer;
    [SerializeField] private PlayableDirector nameplateDirector;
    [SerializeField] private Script_ItemObject smallKey;
    [SerializeField] private Script_ItemObject boarNeedle;
    [SerializeField] private Script_TreasureChest treasureChest;

    [SerializeField] private Script_InteractableFullArt IdsLeaveMeBeNote;
    [SerializeField] private Script_InteractableObject DeadIds;

    [SerializeField] private AudioSource audioSourceIdsDance;

    private bool isIdsDancing = false;
    private bool isIdsDancingWithPlayer = false;
    
    private int leftMoveCount;
    private int downMoveCount;
    private int upMoveCount;
    private int rightMoveCount;

    private int bgTransitionIdx;
    private float scrollSpeedDelta;
    
    private bool didMapNotification;
    private bool isTimelineControlled = false;

    [Space]
    [Header("Weekend")]
    [Space]
    // Weekend Only
    
    [SerializeField] private Script_Marker weekendDialoguePlayerPosition;
    [SerializeField] private List<Script_DialogueNode> weekendIdsCutSceneDialogues;

    [SerializeField] private float waitBeforeWeekendDialogueTime;
    [SerializeField] private float waitBetweenWeekendDialogueTime;
    [SerializeField] private float waitAfterWeekendDialogueTime;
    
    [UnityEngine.Serialization.FormerlySerializedAs("weeekendDay0Nodes")]
    [SerializeField] private List<Script_DialogueNode> weekendDay0Nodes;
    
    [UnityEngine.Serialization.FormerlySerializedAs("weeekendDay1Nodes")]
    [SerializeField] private List<Script_DialogueNode> weekendDay1Nodes;
    
    [UnityEngine.Serialization.FormerlySerializedAs("weeekendDay2Nodes")]
    [SerializeField] private List<Script_DialogueNode> weekendDay2Nodes;

    [SerializeField] private Script_DialogueNode weekendDanceIntroNode;
    [SerializeField] private Script_DialogueNode playerDanceIntroNodeWeekend;
    [SerializeField] private Script_DialogueNode goodDanceOutroNodeWeekend;

    [SerializeField] private Script_DialogueNode[] IdsNRoomWeekendDay2TalkedDialogue;
    [SerializeField] private Script_DialogueNode weekendDay2RetalkNRoomDialogue;
    
    private bool didIdsLeaveWeekend;
    private bool isIdsDeadPRCSDone;
    private bool didInteractPositiveWithIds;
    private bool isWeekendDay2Retalk;
    

    public int BGMIdx { get; set; }

    public int CurrentTry { get => currentTry; }
    
    private Script_DialogueNode IntroNode
    {
        get => Script_MynesMirrorManager.Control.DidInteract ?
            introNodeTalkedWithMyne : introNodeNotTalkedWithMyne;
    }

    private Script_DialogueNode AfterIntroRevealNode
    {
        get => Script_MynesMirrorManager.Control.DidInteract ?
            afterIntroRevealNodeTalkedWithMyne : afterIntroRevealNodeNotTalkedWithMyne;
    }

    protected override void OnEnable()
    {
        Script_GameEventsManager.OnLevelInitComplete    += OnLevelInitCompleteEvent;
        
        IdsDirector.stopped                             += OnIdsMovesDone;    
        Script_DDREventsManager.OnDDRDone               += OnDDRDone;
        Script_DDREventsManager.OnDDRMusicStart         += OnDDRStartPlayerDanceMusic;
        Script_ItemsEventsManager.OnItemStash           += OnItemStash;

        InitialStateFireworks();
    }

    protected override void OnDisable()
    {
        Script_GameEventsManager.OnLevelInitComplete    -= OnLevelInitCompleteEvent;
        
        IdsDirector.stopped                             -= OnIdsMovesDone;
        Script_DDREventsManager.OnDDRDone               -= OnDDRDone;
        Script_DDREventsManager.OnDDRMusicStart         -= OnDDRStartPlayerDanceMusic;
        Script_ItemsEventsManager.OnItemStash           -= OnItemStash;
        
        InitialStateFireworks();
        
        if (!isTimelineControlled)
        {
            Script_AudioMixerVolume.SetVolume(
                audioMixer,
                Const_AudioMixerParams.ExposedBGVolume,
                1f
            );
        }

        isTimelineControlled = false;
    }
    
    void Awake()
    {
        BlackBgFader.gameObject.SetActive(false);
        BlackBgFader.SetVisibility(false);
        StarryNightBg.gameObject.SetActive(false);
    }
    
    protected override void Update()
    {
        HandleAction();
        HandleIdsDanceScene();
    }

    private void OnLevelInitCompleteEvent()
    {
        if (!didMapNotification)
        {
            Script_MapNotificationsManager.Control.PlayMapNotification(MapName);
            didMapNotification = true;
        }
    }

    // ------------------------------------------------------------------------------------
    // Next Node Action Start
    public void PreTheoryDialogue()
    {
        game.ChangeStateCutScene();
        
        dm.StartDialogueNode(AfterIntroRevealNode, SFXOn: false);
    }
    
    public void OnNameplateDone()
    {
        // namePlatePRCSPlayer.Stop();
    }

    
    // TalkedWithMyne & NotTalkedWithMyne Nodes
    public void IdsWalkToERoom()
    {
        game.ChangeStateCutScene();
        Script_VCamManager.VCamMain.SwitchToMainVCam(Script_VCamManager.VCamMain.ActiveVCamera);
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 1);
    }

    // pass DDR node
    public void IdsGivesSmallKey()
    {
        game.ChangeStateCutScene();
        
        Script_VCamManager.VCamMain.SwitchToMainVCam(VCamLB10AfterDanceSuccessEase);
        
        StartCoroutine(WaitToItemReceive());

        // Wait until after the camera is blended (.5s blend, so wait .75s)
        IEnumerator WaitToItemReceive()
        {
            yield return new WaitForSeconds(VCamDanceSuccessToMainBlendTime);

            game.HandleItemReceive(smallKey);
        }
    }

    public void DanceSetupCutScene()
    {
        game.ChangeStateCutScene();
        
        // Timeline Events:
        // 1. Fade Screen Out
        // 2. Reposition Player & change camera
        // 3. Fade Out Lights
        // 4. Drop Chandeleir
        // 5. Ids Dances
        Script_LightFXManager.Control.IsPaused = true;

        // If it's the Ids Dance cut scene, do Framing.
        if (!didIdsDance)
        {
            Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
                isOpen: true,
                framing: Script_UIAspectRatioEnforcerFrame.Framing.ConstantThin,
                cb: DanceSetupTimeline
            );
        }
        else
            DanceSetupTimeline();

        void DanceSetupTimeline()
        {
            danceSetupDirector.GetComponent<Script_TimelineController>()
                .PlayableDirectorPlayFromTimelines(0, 0);       
        }
    }
    
    // Player Dance Intro Node
    public void WaitToDDR(bool isRetry = false)
    {
        BgTransitionsInitialState();

        // No Framing
        if (isRetry)
            StartDDR();
        else
        {
            Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
                isOpen: false,
                framing: Script_UIAspectRatioEnforcerFrame.Framing.ConstantThin,
                cb: StartDDR
            );
        }
        
        void BgTransitionsInitialState()
        {
            bgTransitionIdx = 0;

            // Black faded out
            BlackBgFader.SetVisibility(false);
            BlackBgFader.gameObject.SetActive(false);

            // StarryNight initialized
            StarryNightScroller.ScrollSpeed = startStarryNightScrollSpeed;
            StarryNightBg.gameObject.SetActive(false);
            StarryNightBg.GetComponent<Script_MeshFadeController>().SetVisibility(true);
            scrollSpeedDelta = maxStarryNightScrollSpeed - startStarryNightScrollSpeed;
        }

        void StartDDR()
        {
            if (DDRManager.MistakeCrossesLength != maxMistakesAllowed)
            {
                Debug.LogError("Max mistakes need to equal total number of mistake crosses available");
            }
            
            HandleIncreaseMistakesAllowed();
            
            DDRManager.Activate(
                mistakesAllowed,
                playerSongMoves,
                () => {
                    DDRManager.StartMusic();
                    crystalChandelier.GetComponent<Script_CrystalChandelier>()
                        .StartSpinning();
                }
            );
            
            game.ChangeStateDDR();

            void HandleIncreaseMistakesAllowed()
            {
                if (currentTry > 0 && currentTry % increaseAllotmentInterval == 0)
                    mistakesAllowed++;
                
                if (!Const_Dev.IsDevMode)
                    mistakesAllowed = Mathf.Min(mistakesAllowed, maxMistakesAllowed);
            
                currentTry++;
            }
        }
    }

    // PsychicNode FailDDRNode
    public void OnDDRFailDialogueDone()
    {
        Script_VCamManager.VCamMain.SwitchToMainVCam(VCamLB10AfterDanceFailEase);
        game.ChangeStateInteract();
    }

    public void DDRTryAgain()
    {
        game.ChangeStateCutScene();
        
        DanceSetupCutScene();
    }

    public void OnTalkToIdsRetry()
    {
        HandlePauseMusic();
    }

    public void IdsExitsWeekend()
    {
        game.ChangeStateCutScene();
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 4);
    }

    // ------------------------------------------------------------------------------------
    // Next Node Actions - Weekend Dialogue
    
    public void InteractPositiveWithIds()
    {
        if (!didInteractPositiveWithIds)
        {
            didInteractPositiveWithIds = true;
            Script_EventCycleManager.Control.InteractPositiveWithIds();
        }
    }
    
    public void WeekendDay0FullArtNode()
    {
        StartCoroutine(WaitForDialogue());
        
        IEnumerator WaitForDialogue()
        {
            yield return new WaitForSeconds(waitBetweenWeekendDialogueTime);

            dm.StartDialogueNode(weekendDay0Nodes[0]);
        }
    }

    public void WeekendDay0ExitNodeA()
    {
        StartCoroutine(WaitForDialogue());
        
        IEnumerator WaitForDialogue()
        {
            yield return new WaitForSeconds(waitBetweenWeekendDialogueTime);

            dm.StartDialogueNode(weekendDay0Nodes[1]);
        }
    }

    public void WeekendDay0ExitNodeB()
    {
        StartCoroutine(WaitForDialogue());
        
        IEnumerator WaitForDialogue()
        {
            yield return new WaitForSeconds(waitBetweenWeekendDialogueTime);

            dm.StartDialogueNode(weekendDay0Nodes[2]);
        }
    }

    public void WeekendDay1FullArtNode()
    {
        StartCoroutine(WaitForDialogue());
        
        IEnumerator WaitForDialogue()
        {
            yield return new WaitForSeconds(waitBetweenWeekendDialogueTime);

            dm.StartDialogueNode(weekendDay1Nodes[0]);
        }
    }

    public void WeekendDay1ExitNodeA()
    {
        StartCoroutine(WaitForDialogue());
        
        IEnumerator WaitForDialogue()
        {
            yield return new WaitForSeconds(waitBetweenWeekendDialogueTime);

            dm.StartDialogueNode(weekendDay1Nodes[1]);
        }
    }

    public void WeekendDay1ExitNodeB()
    {
        StartCoroutine(WaitForDialogue());
        
        IEnumerator WaitForDialogue()
        {
            yield return new WaitForSeconds(waitBetweenWeekendDialogueTime);

            dm.StartDialogueNode(weekendDay1Nodes[2]);
        }
    }

    public void WeekendDay2FullArtNode()
    {
        StartCoroutine(WaitForDialogue());
        
        IEnumerator WaitForDialogue()
        {
            yield return new WaitForSeconds(waitBetweenWeekendDialogueTime);

            dm.StartDialogueNode(weekendDay2Nodes[0]);
        }
    }

    public void WeekendDay2NoFullArtNodeB()
    {
        StartCoroutine(WaitForDialogue());
        
        IEnumerator WaitForDialogue()
        {
            yield return new WaitForSeconds(waitBetweenWeekendDialogueTime);

            dm.StartDialogueNode(weekendDay2Nodes[1]);
        }
    }

    public void WeekendDay2FullArtNodeB()
    {
        StartCoroutine(WaitForDialogue());
        
        IEnumerator WaitForDialogue()
        {
            yield return new WaitForSeconds(waitBetweenWeekendDialogueTime);

            dm.StartDialogueNode(weekendDay2Nodes[2]);
        }
    }

    public void WeekendDay2RetalkNoFullArtNode()
    {
        StartCoroutine(WaitForDialogue());
        
        IEnumerator WaitForDialogue()
        {
            yield return new WaitForSeconds(waitBetweenWeekendDialogueTime);

            dm.StartDialogueNode(weekendDay2Nodes[3]);
        }
    }

    public void WeekendDay2DDRPassGiveSmallKeyNode()
    {
        StartCoroutine(WaitForDialogue());
        
        IEnumerator WaitForDialogue()
        {
            yield return new WaitForSeconds(waitBetweenWeekendDialogueTime);

            dm.StartDialogueNode(weekendDay2Nodes[4]);
        }
    }

    /// <summary>
    /// Need to also close the Frame before walking to E Room on Day 2 Weekend.
    /// </summary>
    public void IdsWalkToERoomWeekend()
    {
        // Change Ids Psychic Nodes back to retry nodes in case they were
        // switched if Player chose Day 2 Choice A dialogue.
        Script_DemonNPC demonNPCIds = (Script_DemonNPC)Ids;
        demonNPCIds.SwitchPsychicNodes(defaultRetryNodes);
        
        FadeInWeekendBGM();
        
        Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
            isOpen: false,
            framing: Script_UIAspectRatioEnforcerFrame.Framing.ConstantThin,
            cb: IdsWalkToERoom
        );
    }

    // Wkd Day 2 - DialogueNode Psychic FullArt Choices A
    public void OnWeekendDay2NRoomDialogueChoiceADone()
    {
        FadeInWeekendBGM();

        Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
            isOpen: false,
            framing: Script_UIAspectRatioEnforcerFrame.Framing.ConstantThin,
            cb: () => {
                game.ChangeStateInteract();
                
                // Switch dialogue nodes for Ids
                Script_DemonNPC demonNPCIds = (Script_DemonNPC)Ids;
                demonNPCIds.SwitchPsychicNodes(IdsNRoomWeekendDay2TalkedDialogue);
            }
        );

    }
    
    private void FadeInWeekendBGM()
    {
        Script_BackgroundMusicManager.Control.SetVolume(0f, Const_AudioMixerParams.ExposedBGVolume);

        StartCoroutine(FadeInBGMNextFrame());

        IEnumerator FadeInBGMNextFrame()
        {
            yield return null;
            
            Script_BackgroundMusicManager.Control.FadeInFast(null, Const_AudioMixerParams.ExposedBGVolume);
            
            if (lb9.speaker == null)
                game.SwitchBgMusic(BGMIdx);
        }
    }

    public void IdsFacePlayerPermanently()
    {
        // Ids will automatically face back to his Default direction, but Player
        // may have interacted with him from another direction before the cut scene
        Ids.DefaultFacingDirectionDisabled = true;
        Ids.FaceDirection(Ids.transform.position.GetDirectionToTarget(game.GetPlayer().transform.position));
    }
    
    // Weekend Dialogue Cut Scene Setup Timeline
    public void InitializeRetalk()
    {
        isWeekendDay2Retalk = true;

        HandleWeekendNRoom();
    }
    
    // ------------------------------------------------------------------------------------
    // Unity Events
    public void PlayIdsDeadPRCS()
    {
        Debug.Log("Play Ids dead PRCS");

        if (isIdsDeadPRCSDone)
            return;
        
        game.ChangeStateCutScene();
        
        Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
            isOpen: true,
            framing: Script_UIAspectRatioEnforcerFrame.Framing.IdsDead,
            cb: OnFramingAnimationDone
        );
        
        void OnFramingAnimationDone()
        {
            Script_PRCSManager.Control.OpenPRCSCustom(Script_PRCSManager.CustomTypes.IdsDead);

            Script_BackgroundMusicManager.Control.PauseAll();
            
            Script_BackgroundMusicManager.Control.SetVolume(1f, Const_AudioMixerParams.ExposedBGVolume);
        }
    }
    
    // ------------------------------------------------------------------------------------
    // Timeline Signals START
    
    // DanceSetupDirector DanceSetupTimeline
    public void DanceSetup()
    {
        game.GetPlayer().Teleport(playerDancePosition.Position);
        
        Script_VCamManager.VCamMain.SetNewVCam(VCamLB10Dance);
        
        InitializeIdsDance();
        
        // Move moving lights up.
        foreach (var movingLight in movingLights)
        {
            StartCoroutine(
                movingLight.MoveSmooth(lightsFadeOutTime, lightsUpOffset, null)
            );
        }
        
        StartCoroutine(playerSpotLight.FadeInLightOnTarget(
            lightsFadeOutTime,
            game.GetPlayer().transform,
            null
        ));
        StartCoroutine(IdsSpotLight.FadeInLightOnTarget(
            lightsFadeOutTime,
            game.GetNPC(0).GetComponent<Transform>(),
            null
        ));

        levelDirectionalLight.GetComponent<Light>().intensity = directionalLightDanceIntensity;
        
        foreach (var fadingLight in additionalFadingLights)
        {
            StartCoroutine(fadingLight.FadeOutLight(
                lightsFadeOutTime,
                null
            ));
        }
    }

    // DanceSetupDirector DanceSetupTimeline
    public void DropCrystalChandeleir()
    {
        // todo: setinactive after
        crystalChandelier.SetActive(true);
        
        StartCoroutine(
            crystalChandelier.GetComponent<Script_MoveDirection>().MoveSmooth(
                dropDiscoBallTime,
                crystalChandelierDownOffset,
                null
            )
        );
    }

    public void OnDanceSetupDone()
    {
        // Ids Dancing
        if (!didIdsDance)
        {
            IdsStartDancing();
            didIdsDance = true;
            return;
        }
        
        // Otherwise, it's a retry, dance setup directly to DDR.
        WaitToDDR(isRetry: true);
        
        void IdsStartDancing()
        {
            audioSourceIdsDance.time = 0f;
            audioSourceIdsDance.Play();
            
            isIdsDancing = true;
            
            crystalChandelier.GetComponent<Script_CrystalChandelier>()
                .StartSpinning();
        }
    }
    
    
    // Ids Exits Weekend Timeline 
    public void OnIdsExitsIdsRoom()
    {
        Ids.gameObject.SetActive(false);
    }

    public void OnQuestDone()
    {
        isCurrentPuzzleComplete = true;
        isTimelineControlled = true;

        var transitionManager = Script_TransitionManager.Control;
        transitionManager.OnCurrentQuestDone(
            allQuestsDoneCb: () =>
            {
                isTimelineControlled = false;
                
                transitionManager.FinalCutSceneAwakening();
            }, 
            defaultCb: () =>
            {
                isTimelineControlled = false;
                
                game.ChangeStateInteract();
            },
            Script_TransitionManager.FinalNotifications.Ids
        );
    }
    
    // Called from PlayIdsDeadPRCS Timeline.
    public void OnIdsDeadPRCSDone()
    {
        Script_BackgroundMusicManager.Control.FadeOutMed(() => {
                Script_BackgroundMusicManager.Control.UnPauseAll();
                Script_BackgroundMusicManager.Control.FadeInSlow(null, Const_AudioMixerParams.ExposedBGVolume);
            },
            Const_AudioMixerParams.ExposedBGVolume
        );
        
        Script_PRCSManager.Control.ClosePRCSCustom(Script_PRCSManager.CustomTypes.IdsDead, () => {
            isIdsDeadPRCSDone = true;
            
            Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
                isOpen: false,
                framing: Script_UIAspectRatioEnforcerFrame.Framing.IdsDead,
                cb: game.ChangeStateInteract
            );
        });    
    }

    // Final Awakening Timeline
    public void TimelineSetup()
    {
        BaseSetup();
        
        DeadIds.gameObject.SetActive(false);
        Ids.gameObject.SetActive(false);

        isTimelineControlled = true;
    }

    // Weekend Dialogue Cut Scene Setup
    public void SetupWeekendDialogueCutScene()
    {
        game.GetPlayer().Teleport(weekendDialoguePlayerPosition.Position);
        game.GetPlayer().FaceDirection(Directions.Up);
        
        Ids.DefaultFacingDirectionDisabled = false;
        Ids.FaceDirection(Directions.Down);
    }

    // Weekend Dialogue Cut Scene Setup Timeline
    public void StartWeekendDialogue()
    {
        if (isWeekendDay2Retalk)
        {
            dm.StartDialogueNode(weekendDay2RetalkNRoomDialogue);    

            isWeekendDay2Retalk = false;
            return;
        }
        
        dm.StartDialogueNode(weekendIdsCutSceneDialogues[0]);
    }

    // Note: ARCHIVE, used previously for leaving on 3-day cycle first 2 days.
    public void OnWeekendIdsExitsRoomTimelineDone()
    {
        // Script_BackgroundMusicManager.Control.SetVolume(0f, Const_AudioMixerParams.ExposedBGVolume);
        
        // // LB9 Persisting Speaker will have been destroyed at this point because HandlePauseMusic
        // // will have destroyed the speaker at the beginning on the cut scene.
        // if (lb9.speaker == null)
        //     game.SwitchBgMusic(BGMIdx);
        
        // Script_BackgroundMusicManager.Control.FadeInMed(null, Const_AudioMixerParams.ExposedBGVolume);
        
        // StartCoroutine(WaitToCloseFrame());

        // IEnumerator WaitToCloseFrame()
        // {
        //     yield return new WaitForSeconds(waitAfterWeekendDialogueTime);

        //     Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
        //         isOpen: false,
        //         framing: Script_UIAspectRatioEnforcerFrame.Framing.ConstantThin,
        //         cb: () => {
        //             game.ChangeStateInteract();
        //             didIdsLeaveWeekend = true;
        //             timelinesDoneCount++;
        //         }
        //     );
        // }
    }

    // ------------------------------------------------------------------------------------

    // Happens after Pass DDR and after Ids gives Super Small Key.
    private void OnItemStash(string stashItemId)
    {
        if (stashItemId == smallKey.Item.id)
            IdsExits();
        else if (stashItemId == boarNeedle.Item.id)
            gotBoarNeedle = true;

        void IdsExits()
        {
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 2);
        }
    }

    private void OnIdsMovesDone(PlayableDirector aDirector)
    {
        if (aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[0])
        {
            Ids.FaceDirection(Directions.Down);
            Ids.DefaultFacingDirection = Directions.Down;
        }
        else if (aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[1])
        {
            Ids.FaceDirection(Directions.Left);
            Ids.DefaultFacingDirection = Directions.Left;
            game.ChangeStateInteract();
        }
        
        /// Timelines[2] exit is handled by Timeline Signal

        Ids.UpdateLocation();
        timelinesDoneCount++;
    }

    public void OnNRoomTriggerPlayerStay(string Id)
    {
        if (activeTriggerIndex == 0 && timelinesDoneCount == 1)
            NRoomTriggerReaction();
    }

    public void OnERoomTriggerPlayerStay(string Id)
    {
        if (activeTriggerIndex == 1 && timelinesDoneCount == 2 && !didIdsLeaveWeekend)
            ERoomTriggerReaction();
    }

    public bool NRoomTriggerReaction() {
        activeTriggerIndex++;
        
        return game.RunCycle == Script_RunsManager.Cycle.Weekday
            ? HandleWeekdayNRoom()
            : HandleWeekendNRoom();
    }
    
    private bool HandleWeekdayNRoom()
    {
        game.ChangeStateCutScene();
        HandlePauseMusic();
        game.PlayNPCBgTheme(IdsBgThemePlayerPrefab);
        game.PlayerFaceDirection(Directions.Up);
        
        Script_VCamManager.VCamMain.SetNewVCam(VCamLB10FollowIds);
        
        StartCoroutine(WaitForIdsDialogue());

        return true;
        
        IEnumerator WaitForIdsDialogue()
        {
            yield return new WaitForSeconds(waitBeforeIdsDialogueTime);
            
            dm.StartDialogueNode(IntroNode);
        }
    }

    /// <summary>
    /// On the weekend, wait to increment activeTriggerIndex until Ids actually
    /// walks to E Room since he can still remain in NRoom.
    /// </summary>
    private bool HandleWeekendNRoom()
    {
        game.ChangeStateCutScene();
        
        // Silent when talking on Weekend.
        Script_BackgroundMusicManager.Control.FadeOutMed(
            () => {
                HandlePauseMusic();
                Script_BackgroundMusicManager.Control.SetVolume(1f, Const_AudioMixerParams.ExposedBGVolume);
            },
            Const_AudioMixerParams.ExposedBGVolume
        );

        StartCoroutine(WaitBeforeCutScene());
        
        return true;
        
        IEnumerator WaitBeforeCutScene()
        {
            yield return new WaitForSeconds(waitBeforeWeekendDialogueTime);

            Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
                isOpen: true,
                framing: Script_UIAspectRatioEnforcerFrame.Framing.ConstantThin,
                cb: () => GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(1, 3)
            );        
        }
    }

    public bool ERoomTriggerReaction()
    {
        triggers[1].gameObject.SetActive(false);
        
        game.PauseNPCBgTheme();

        HandlePauseMusic();

        game.ChangeStateCutScene();
        
        game.PlayerFaceDirection(Directions.Right);
        game.GetMovingNPC(0).FaceDirection(Directions.Left);
        
        var node = game.RunCycle == Script_RunsManager.Cycle.Weekend
            ? weekendDanceIntroNode
            : danceIntroNode;
        dm.StartDialogueNode(node);
        
        activeTriggerIndex++;
        return true;
    }

    private void HandlePauseMusic()
    {
        if (lb9.speaker != null)
        {
            lb9.speaker.audioSource.Pause();
            Destroy(lb9.speaker.gameObject);
        }
        else
        {
            game.PauseBgMusic();
        }
    }

    // Turn flag on for Ids to start tracking Player Dance Moves
    // This must occur after DSP time is set for SongPosition on Conductor.
    private void OnDDRStartPlayerDanceMusic()
    {
        InitializeIdsDance();
        isIdsDancingWithPlayer = true;
    }
    
    private void HandleIdsDanceScene()
    {
        if (isIdsDancing)
        {
            HandleLeftMove(IdsSongMoves, audioSourceIdsDance.time);
            HandleDownMove(IdsSongMoves, audioSourceIdsDance.time);
            HandleUpMove(IdsSongMoves, audioSourceIdsDance.time);
            HandleRightMove(IdsSongMoves, audioSourceIdsDance.time);

            // Once stops playing Ids song
            if (!audioSourceIdsDance.isPlaying)
            {
                crystalChandelier.GetComponent<Script_CrystalChandelier>()
                    .StopSpinning();
                isIdsDancing = false;
                StartCoroutine(WaitToTalkAfterIdsDance());
            }
        }
        else if (isIdsDancingWithPlayer)
        {
            HandleLeftMove(playerSongMoves, DDRConductor.SongPosition);
            HandleDownMove(playerSongMoves, DDRConductor.SongPosition);
            HandleUpMove(playerSongMoves, DDRConductor.SongPosition);
            HandleRightMove(playerSongMoves, DDRConductor.SongPosition);
            HandleBgTransitions();
        }

        IEnumerator WaitToTalkAfterIdsDance()
        {
            yield return new WaitForSeconds(postIdsDanceWaitTime);
            
            Ids.FaceDirection(Directions.Left);
            
            var node = game.RunCycle == Script_RunsManager.Cycle.Weekend
                ? playerDanceIntroNodeWeekend
                : playerDanceIntroNode;
            dm.StartDialogueNode(node);
        }

        void HandleLeftMove(Model_SongMoves songMoves, float time)
        {
            if (leftMoveCount > songMoves.leftMoveTimes.Length - 1)
                return;

            if (time >= songMoves.leftMoveTimes[leftMoveCount])
            {
                Debug.Log($"IDS FACING LEFT: Time {time}");
                Debug.Log($"Time {time}");
                Debug.Log($"songMoves.leftMoveTimes[leftMoveCount] {songMoves.leftMoveTimes[leftMoveCount]}");
                
                Ids.FaceDirection(Directions.Left);
                leftMoveCount++;
            }
        }

        void HandleDownMove(Model_SongMoves songMoves, float time)
        {
            if (downMoveCount > songMoves.downMoveTimes.Length - 1)
                return;

            if (time >= songMoves.downMoveTimes[downMoveCount])
            {
                Debug.Log($"IDS FACING DOWN: Time {time}");
                Debug.Log($"Time {time}");
                Debug.Log($"songMoves.downMoveTimes[downMoveCount] {songMoves.downMoveTimes[downMoveCount]}");
                
                Ids.FaceDirection(Directions.Down);
                downMoveCount++;
            }
        }

        void HandleUpMove(Model_SongMoves songMoves, float time)
        {
            if (upMoveCount > songMoves.upMoveTimes.Length - 1) 
                return;

            if (time >= songMoves.upMoveTimes[upMoveCount])
            {
                Debug.Log($"IDS FACING UP: Time {time}");
                Debug.Log($"songMoves.upMoveTimes[upMoveCount] {songMoves.upMoveTimes[upMoveCount]}");
                
                Ids.FaceDirection(Directions.Up);
                upMoveCount++;
            }
        }

        void HandleRightMove(Model_SongMoves songMoves, float time)
        {
            if (rightMoveCount > songMoves.rightMoveTimes.Length - 1)
                return;

            if (time >= songMoves.rightMoveTimes[rightMoveCount])
            {
                Debug.Log($"IDS FACING RIGHT: Time {time}");
                Debug.Log($"songMoves.rightMoveTimes[rightMoveCount] {songMoves.rightMoveTimes[rightMoveCount]}");
                
                Ids.FaceDirection(Directions.Right);
                rightMoveCount++;
            }
        }
    }

    private void HandleBgTransitions()
    {
        if (bgTransitionIdx > bgTransitionTimes.Count - 1)
            return;
        
        if (DDRConductor.SongPosition >= bgTransitionTimes[bgTransitionIdx])
        {
            switch (bgTransitionIdx)
            {
                case 0:
                    // Fade In Black & Activate StarryNight in the background
                    BlackBgFader.gameObject.SetActive(true);
                    BlackBgFader.FadeIn(blackBgFadeInTime, () => {
                        StarryNightBg.gameObject.SetActive(true);
                    });
                    
                    bgTransitionIdx++;
                    break;
                case 1:
                    // Fade Out Black
                    BlackBgFader.FadeOut(blackBgFadeOutTime);
                    
                    bgTransitionIdx++;
                    break;
                case 2:
                    // Start increasing scroll speed, leave Idx pointer here.
                    float scrollSpeedIncrease = (Time.deltaTime / starryNightMaxScrollSpeedTimeToReach)
                        * scrollSpeedDelta;
                    
                    StarryNightScroller.ScrollSpeed = Mathf.Min(
                        StarryNightScroller.ScrollSpeed + scrollSpeedIncrease,
                        maxStarryNightScrollSpeed
                    );

                    break;
            }
        }
    }

    private void OnDDRDone()
    {
        isIdsDancingWithPlayer = false;
        Ids.FaceDirection(Directions.Left);

        // After DDR is done, zoom out camera to VCamLB10DanceConvo showing Ids and Player equally.
        // Ensure to blend back to Main Cam on both Success and Fail cases.
        if (DDRManager.didFail)
        {
            Debug.Log($"**** OnDDRDone starting Bad Node ****");
            DDRFinish(badDanceOutroNode);
            
            SwitchFromDanceCamToAfterDanceCam(false);
        }
        else
        {
            Debug.Log($"**** OnDDRDone starting Good Node ****");
            
            var node = game.RunCycle == Script_RunsManager.Cycle.Weekend
                ? goodDanceOutroNodeWeekend
                : goodDanceOutroNode;
            DDRFinish(node);

            SwitchFromDanceCamToAfterDanceCam(true);
        }
    }

    void DDRFinish(Script_DialogueNode node)
    {
        game.ManagePlayerViews(Const_States_PlayerViews.Health);
        game.ChangeStateCutScene();
        game.PlayerFaceDirection(Directions.Right);
        
        crystalChandelier.GetComponent<Script_CrystalChandelier>()
            .StopSpinning();
        
        Script_LightFXManager.Control.IsPaused = false;
        SwitchLightsInAnimation();
        TearDownBgTransitions();

        dm.StartDialogueNode(node);
    }

    void SwitchLightsInAnimation()
    {
        // Move moving lights back down.
        foreach (var movingLight in movingLights)
        {
            StartCoroutine(
                movingLight.MoveSmooth(dropDiscoBallTime, lightsDownOffset, null)
            );
        }
        
        // Move chandeleir back up.
        StartCoroutine(
            crystalChandelier.GetComponent<Script_MoveDirection>().MoveSmooth(
                dropDiscoBallTime,
                crystalChandelierUpOffset,
                () => {
                    crystalChandelier.SetActive(false);
                }
            )
        );

        StartCoroutine(playerSpotLight.FadeOutLight(
            dropDiscoBallTime,
            null
        ));
        StartCoroutine(IdsSpotLight.FadeOutLight(
            dropDiscoBallTime,
            null
        ));
        StartCoroutine(levelDirectionalLight.FadeInLightOnTarget(
            dropDiscoBallTime,
            null,
            null,
            Script_LightFXManager.Control.CurrentIntensity
        ));

        foreach (var fadingLight in additionalFadingLights)
        {
            StartCoroutine(fadingLight.FadeInLightOnTarget(
                dropDiscoBallTime,
                null,
                null
            ));
        }

        // DanceTearDownTimeline
        danceSetupDirector.GetComponent<Script_TimelineController>()
            .PlayableDirectorPlayFromTimelines(0, 1);
    }

    private void TearDownBgTransitions()
    {
        // Fade out Black Bg & Stars in case reached these points in song.
        BlackBgFader.FadeOut();
        
        // If StarryNight was showing, fade it out, otherwise need to
        // cut it or it will flicker behind the fading BlackBg
        if (bgTransitionIdx > 1)
            StarryNightBg.GetComponent<Script_MeshFadeController>().FadeOut();
        else
            StarryNightBg.GetComponent<Script_MeshFadeController>().SetVisibility(false);
    }

    public override void HandleDDRArrowClick(int tier) {}

    void InitializeIdsDance()
    {
        leftMoveCount = 0;
        downMoveCount = 0;
        upMoveCount = 0;
        rightMoveCount = 0;
    }

    void HandleIdsInRoom()
    {
        if (game.RunCycle == Script_RunsManager.Cycle.Weekday)
        {
            if (Script_EventCycleManager.Control.IsIdsHome())
                HandleIdsHome();
            else
                HandleIdsNotHome();
        }
        else
        {
            // Completed DDR quest Weekend
            if (isCurrentPuzzleComplete)
            {
                HandleDDRDone();
                return;
            }
            
            // If haven't completed DDR quest, and past 5:XXam, then Ids
            // will be dead.
            if (Script_EventCycleManager.Control.IsIdsDead())
                HandleIdsDead();
            // If haven't completed DDR quest, and past 5:XXam, then Ids
            // will be in Rock Garden.
            else if (Script_EventCycleManager.Control.IsIdsInSanctuary())
                HandleIdsNotHome();
            else
                HandleIdsHome();
        }

        void HandleIdsHome()
        {
            IdsLeaveMeBeNote.gameObject.SetActive(false);
            DeadIds.gameObject.SetActive(false);
            Ids.gameObject.SetActive(true);

            treasureChest.gameObject.SetActive(true);
            
            foreach (Script_Trigger t in triggers)
                t.gameObject.SetActive(true);
        }

        void HandleIdsNotHome()
        {
            Script_BackgroundMusicManager.Control.Stop();
            
            IdsLeaveMeBeNote.gameObject.SetActive(true);
            DeadIds.gameObject.SetActive(false);
            Ids.gameObject.SetActive(false);

            treasureChest.gameObject.SetActive(true);
            
            foreach (Script_Trigger t in triggers)
                t.gameObject.SetActive(false);
        }

        void HandleIdsDead()
        {
            Script_BackgroundMusicManager.Control.Stop();
            
            IdsLeaveMeBeNote.gameObject.SetActive(false);
            DeadIds.gameObject.SetActive(true);
            Ids.gameObject.SetActive(false);

            treasureChest.gameObject.SetActive(true);
            
            foreach (Script_Trigger t in triggers)
                t.gameObject.SetActive(false);
        }

        void HandleDDRDone()
        {
            Script_BackgroundMusicManager.Control.Stop();
            
            IdsLeaveMeBeNote.gameObject.SetActive(false);
            DeadIds.gameObject.SetActive(false);
            Ids.gameObject.SetActive(false);

            treasureChest.gameObject.SetActive(true);
            
            foreach (Script_Trigger t in triggers)
                t.gameObject.SetActive(false);
        }
    }
    
    // Only if the timeline is finished do we increment Timeline count. Based on where
    // we are on Timeline count, spawn Ids accordingly
    void HandleIdsSpawn()
    {
        if (timelinesDoneCount > IdsSpawns.Length || didIdsLeaveWeekend)
        {
            Ids.gameObject.SetActive(false);
        }
        else if (timelinesDoneCount == 0)
        {
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);
        }
        else
        {
            Ids.transform.position = IdsSpawns[timelinesDoneCount - 1].Position;
            Ids.DefaultFacingDirection = IdsSpawns[timelinesDoneCount - 1].Direction;
            Ids.FaceDefaultDirection();
        }

        Ids.UpdateLocation();
    }

    /// <summary>
    /// Call this from Timeline to opt out of Cycle set up (e.g. Final Awakening).
    /// </summary>
    private void BaseSetup()
    {
        if (!isInitialized)
        {
            crystalChandelier.SetActive(false);
        }
        
        game.SetupMovingNPC(Ids, !isInitialized);
        
        // Ids DDR quest is completed and the Locked Treasure Chest was opened.
        if (gotBoarNeedle)
        {
            treasureChest.IsEmpty = true;
        }

        InitialStateFireworks();

        isInitialized = true;
    }

    private void InitialStateFireworks()
    {
        fireworks.ForEach(firework => firework.gameObject.SetActive(false));
    }
    
    private void SwitchFromDanceCamToAfterDanceCam(bool isSuccess)
    {
        var cam = isSuccess
            ? VCamLB10AfterDanceSuccessEase
            : VCamLB10AfterDanceFailEase;
        
        Script_VCamManager.VCamMain.SwitchBetweenVCams(VCamLB10Dance, cam);
    }

    /// <summary>
    /// Need to establish only on game load because this could change
    /// depending on Ids Positive Interaction 
    /// </summary>
    public void InitializeBGMOnRun()
    {
        // bool isWeekday = game.RunCycle == Script_RunsManager.Cycle.Weekday;
        // bool isWeekendIdsDanceDay = game.RunCycle == Script_RunsManager.Cycle.Weekend
        //     && !Script_EventCycleManager.Control.IsIdsInSanctuary()
        //     && !Script_EventCycleManager.Control.IsIdsDead();

        BGMIdx = clubMusicIdx;
    }

    public override void Setup()
    {
        Ids.SetMoveSpeedWalk();
        
        if (lb9.speaker == null)
            game.SwitchBgMusic(BGMIdx);

        BaseSetup();
        HandleIdsInRoom();
        // HandleIdsDialogue();
        HandleIdsSpawn();

        // void HandleIdsDialogue()
        // {
            // if (game.RunCycle == Script_RunsManager.Cycle.Weekday)
            //     dialogueState = DialogueState.Weekday;
            // else
            //     dialogueState = DialogueState.Weekend;
        // }
    }
    
    #if UNITY_EDITOR
    [CustomEditor(typeof(Script_LevelBehavior_10))]
    public class Script_LevelBehavior_10Tester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_LevelBehavior_10 t = (Script_LevelBehavior_10)target;
            if (GUILayout.Button("Ids DDR Success Quest Done"))
            {
                t.IdsGivesSmallKey();
            }

            if (GUILayout.Button("Lights Fade Out"))
            {
                t.DanceSetupCutScene();
            }

            if (GUILayout.Button("Lights Fade In"))
            {
                t.SwitchLightsInAnimation();
            }

            if (GUILayout.Button("Nameplate Timeline"))
            {
                t.PreTheoryDialogue();
            }

            if (GUILayout.Button("DDR"))
            {
                t.WaitToDDR();
            }
        }
    }
    #endif
}
