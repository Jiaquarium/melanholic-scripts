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
    public const string MapName = "Basement";
    
    public const string NRoomTriggerId = "room_N";
    public const string ERoomTriggerId = "room_E";
    
    // =======================================================================
    //  STATE DATA
    public bool gotBoarNeedle;
    // =======================================================================
    
    public bool isCurrentPuzzleComplete;
    
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

    [SerializeField] private Script_DialogueNode afterIntroRevealNodeTalkedWithMyne;
    [SerializeField] private Script_DialogueNode afterIntroRevealNodeNotTalkedWithMyne;

    public Script_DialogueNode danceIntroNode;
    public Script_DialogueNode playerDanceIntroNode;
    public Script_DialogueNode badDanceOutroNode;
    public Script_DialogueNode goodDanceOutroNode;
    
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
    public int mistakesAllowed;

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
    [SerializeField] private bool isRetry = false;
    
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
    
    private int lastLeftMove;
    private int lastDownMove;
    private int lastUpMove;
    private int lastRightMove;

    private int bgTransitionIdx;
    private float scrollSpeedDelta;
    
    private bool didMapNotification;
    private bool isTimelineControlled = false;

    private bool isIdsDeadPRCSDone;

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
    }

    protected override void OnDisable()
    {
        Script_GameEventsManager.OnLevelInitComplete    -= OnLevelInitCompleteEvent;
        
        IdsDirector.stopped                             -= OnIdsMovesDone;
        Script_DDREventsManager.OnDDRDone               -= OnDDRDone;
        Script_DDREventsManager.OnDDRMusicStart         -= OnDDRStartPlayerDanceMusic;
        Script_ItemsEventsManager.OnItemStash           -= OnItemStash;
        
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
    public void NameplateTimeline()
    {
        game.ChangeStateCutScene();
        
        Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
            isOpen: true,
            framing: Script_UIAspectRatioEnforcerFrame.Framing.ConstantThin,
            cb: () => namePlatePRCSPlayer.Play()
        );
    }
    
    public void OnNameplateDone()
    {
        namePlatePRCSPlayer.Stop();

        Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
            isOpen: false,
            framing: Script_UIAspectRatioEnforcerFrame.Framing.ConstantThin,
            cb: () => dm.StartDialogueNode(AfterIntroRevealNode, SFXOn: false)
        );
    }

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

        danceSetupDirector.GetComponent<Script_TimelineController>()
            .PlayableDirectorPlayFromTimelines(0, 0);
    }
    
    public void WaitToDDR()
    {
        BgTransitionsInitialState();
        
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
        if (!isRetry)
        {
            IdsStartDancing();
            isRetry = true;
            return;
        }
        
        // Otherwise, it's a retry, dance setup directly to DDR.
        WaitToDDR();
        
        void IdsStartDancing()
        {
            audioSourceIdsDance.time = 0f;
            audioSourceIdsDance.Play();
            
            isIdsDancing = true;
            
            crystalChandelier.GetComponent<Script_CrystalChandelier>()
                .StartSpinning();
        }
    }
    
    
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

    public override bool ActivateTrigger(string Id)
    {
        bool isPsychicDuckActive = Script_ActiveStickerManager.Control.IsActiveSticker(Const_Items.PsychicDuckId);
        if (!isPsychicDuckActive)   return false;

        if (Id == NRoomTriggerId && activeTriggerIndex == 0)
        {
            return NRoomTriggerReaction();
        }
        else if (Id == ERoomTriggerId && activeTriggerIndex == 1)
        {
            return ERoomTriggerReaction();
        }

        return false;
    }

    public void OnNRoomTriggerPlayerStay(string Id)
    {
        bool isPsychicDuckActive = Script_ActiveStickerManager.Control.IsActiveSticker(Const_Items.PsychicDuckId);
        if (!isPsychicDuckActive)    return;
        
        if (activeTriggerIndex == 0)    NRoomTriggerReaction();
    }

    public void OnERoomTriggerPlayerStay(string Id)
    {
        bool isPsychicDuckActive = Script_ActiveStickerManager.Control.IsActiveSticker(Const_Items.PsychicDuckId);
        if (!isPsychicDuckActive)    return;
        
        if (activeTriggerIndex == 1)    ERoomTriggerReaction();
    }

    public bool NRoomTriggerReaction()
    {
        Script_EventCycleManager.Control.DidTalkToIdsToday = true;

        triggers[0].gameObject.SetActive(false);
        
        HandlePauseMusic();

        game.PlayNPCBgTheme(IdsBgThemePlayerPrefab);
        game.ChangeStateCutScene();
        game.PlayerFaceDirection(Directions.Up);
        dm.StartDialogueNode(IntroNode);
        
        Script_VCamManager.VCamMain.SetNewVCam(VCamLB10FollowIds);
        
        // if (activeTriggerIndex == triggerLocations.Length - 1) isCurrentPuzzleComplete = true;
        activeTriggerIndex++;
        return true;
    }

    public bool ERoomTriggerReaction()
    {
        triggers[1].gameObject.SetActive(false);
        
        game.PauseNPCBgTheme();

        HandlePauseMusic();

        game.ChangeStateCutScene();
        
        game.PlayerFaceDirection(Directions.Right);
        game.GetMovingNPC(0).FaceDirection(Directions.Left);
        dm.StartDialogueNode(danceIntroNode);
        
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
            dm.StartDialogueNode(playerDanceIntroNode);
        }

        void HandleLeftMove(Model_SongMoves songMoves, float time)
        {
            if (leftMoveCount > songMoves.leftMoveTimes.Length - 1)
                return;

            Debug.Log($"Time {time}");
            Debug.Log($"songMoves.leftMoveTimes[leftMoveCount] {songMoves.leftMoveTimes[leftMoveCount]}");

            if (
                time >= songMoves.leftMoveTimes[leftMoveCount]
                && leftMoveCount > lastLeftMove
            )
            {
                Debug.Log("IDS FACING LEFT");
                
                Ids.FaceDirection(Directions.Left);
                lastLeftMove = leftMoveCount;
                leftMoveCount++;
            }
        }

        void HandleDownMove(Model_SongMoves songMoves, float time)
        {
            if (downMoveCount > songMoves.downMoveTimes.Length - 1)
                return;

            Debug.Log($"Time {time}");
            Debug.Log($"songMoves.downMoveTimes[downMoveCount] {songMoves.downMoveTimes[downMoveCount]}");

            if (
                time >= songMoves.downMoveTimes[downMoveCount]
                && downMoveCount > lastDownMove
            )
            {
                Debug.Log("IDS FACING RIGHT");
                
                Ids.FaceDirection(Directions.Down);
                lastDownMove = downMoveCount;
                downMoveCount++;
            }
        }

        void HandleUpMove(Model_SongMoves songMoves, float time)
        {
            if (upMoveCount > songMoves.upMoveTimes.Length - 1) 
                return;

            if (
                time >= songMoves.upMoveTimes[upMoveCount]
                && upMoveCount > lastUpMove
            )
            {
                Ids.FaceDirection(Directions.Up);
                lastUpMove = upMoveCount;
                upMoveCount++;
            }
        }

        void HandleRightMove(Model_SongMoves songMoves, float time)
        {
            if (rightMoveCount > songMoves.rightMoveTimes.Length - 1)
                return;

            if (
                time >= songMoves.rightMoveTimes[rightMoveCount]
                && rightMoveCount > lastRightMove
            )
            {
                game.GetMovingNPC(0).FaceDirection(Directions.Right);
                lastRightMove = rightMoveCount;
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
            DDRFinish(goodDanceOutroNode);

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
        lastLeftMove = -1;
        
        downMoveCount = 0;
        lastDownMove = -1;
        
        upMoveCount = 0;
        lastUpMove = -1;
        
        rightMoveCount = 0;
        lastRightMove = -1;
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
            if (Script_EventCycleManager.Control.IsIdsDead())
                HandleIdsDead();
            else if (Script_EventCycleManager.Control.IsIdsInSanctuary())
                HandleIdsNotHome();
            else
                HandleIdsHome();
        }

        void HandleIdsHome()
        {
            // Ids note only appears when he's not home on Weekend Day 2.
            IdsLeaveMeBeNote.gameObject.SetActive(false);

            // Ids only dies if not talked to by Weekend Day 3.
            DeadIds.gameObject.SetActive(false);
            
            Ids.gameObject.SetActive(true);
            treasureChest.gameObject.SetActive(true);
            
            foreach (Script_Trigger t in triggers)
                t.gameObject.SetActive(true);
        }

        void HandleIdsNotHome()
        {
            Script_BackgroundMusicManager.Control.Stop();
            
            // Ids leaves note when he's not home on Weekend Day 2.
            IdsLeaveMeBeNote.gameObject.SetActive(true);

            // Ids only dies if not talked to by Weekend Day 3.
            DeadIds.gameObject.SetActive(false);

            Ids.gameObject.SetActive(false);
            
            foreach (Script_Trigger t in triggers)
                t.gameObject.SetActive(false);
        }

        void HandleIdsDead()
        {
            Script_BackgroundMusicManager.Control.Stop();
            
            // Ids note only appears when he's not home on Weekend Day 2.
            IdsLeaveMeBeNote.gameObject.SetActive(false);

            // Ids dies since not talked to by Weekend Day 3.
            DeadIds.gameObject.SetActive(true);

            Ids.gameObject.SetActive(false);
            
            foreach (Script_Trigger t in triggers)
                t.gameObject.SetActive(false);
        }
    }
    
    // Only if the timeline is finished do we increment Timeline count. Based on where
    // we are on Timeline count, spawn Ids accordingly
    void HandleIdsSpawn()
    {
        if (timelinesDoneCount > IdsSpawns.Length)
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
            treasureChest.IsOpen = true;
        }

        fireworks.ForEach(firework => firework.gameObject.SetActive(false));

        isInitialized = true;
    }

    // ----------------------------------------------------------------------
    // Timeline Signals

    public void TimelineSetup()
    {
        BaseSetup();
        
        DeadIds.gameObject.SetActive(false);
        Ids.gameObject.SetActive(false);

        isTimelineControlled = true;
    }

    // ----------------------------------------------------------------------
    
    private void SwitchFromDanceCamToAfterDanceCam(bool isSuccess)
    {
        var cam = isSuccess
            ? VCamLB10AfterDanceSuccessEase
            : VCamLB10AfterDanceFailEase;
        
        Script_VCamManager.VCamMain.SwitchBetweenVCams(VCamLB10Dance, cam);
    }

    public override void Setup()
    {
        Ids.SetMoveSpeedWalk();
        
        if (lb9.speaker == null)    game.SwitchBgMusic(4);

        BaseSetup();
        HandleIdsInRoom();
        HandleIdsSpawn();
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
                t.NameplateTimeline();
            }

            if (GUILayout.Button("DDR"))
            {
                t.WaitToDDR();
            }
        }
    }
    #endif
}
