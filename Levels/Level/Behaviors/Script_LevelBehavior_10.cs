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
    public const string MapName = "The Sanctuary";
    
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
    
    public bool isDeskSwitchedIn;
    public bool isInitialized;


    public float dropDiscoBallTime;
    public float postIdsDanceWaitTime;
    

    public Script_DialogueManager dm;
    public Script_DDRManager DDRManager;
    public Script_LevelBehavior_9 lb9;
    public Script_BgThemePlayer IdsBgThemePlayerPrefab;
    public Script_BgThemePlayer IdsCandyDanceShortThemePlayerPrefab;
    public Script_BgThemePlayer PlayerCandyDanceThemePlayerPrefab;
    
    [SerializeField] private Script_DialogueNode introNodeTalkedWithMyne;
    [SerializeField] private Script_DialogueNode introNodeNotTalkedWithMyne;

    [SerializeField] private Script_DialogueNode afterIntroRevealNodeTalkedWithMyne;
    [SerializeField] private Script_DialogueNode afterIntroRevealNodeNotTalkedWithMyne;

    public Script_DialogueNode danceIntroNode;
    public Script_DialogueNode playerDanceIntroNode;
    public Script_DialogueNode badDanceOutroNode;
    public Script_DialogueNode goodDanceOutroNode;
    public Script_DialogueNode deskIONode;
    public Script_DialogueNode chaiseLoungeIONode;
    public GameObject lights;
    public GameObject crystalChandelier;
    public Script_LightFadeIn playerSpotLight;
    public Script_LightFadeIn IdsSpotLight;
    public Vector3 lightsUpOffset;
    public Vector3 lightsDownOffset;
    public Vector3 crystalChandelierDownOffset;
    public Vector3 crystalChandelierUpOffset;
    public GameObject chaiseLoungeObject;
    public GameObject deskObject;
    public AudioMixer audioMixer;
    
    public Model_SongMoves playerSongMoves;
    public Model_SongMoves IdsSongMoves;
    public int mistakesAllowed;
    public Transform[] IOTexts;
    public Script_MovingNPC Ids;
    [SerializeField] private PlayableDirector IdsDirector;
    [SerializeField] private Script_VCamera VCamLB10FollowIds;
    [SerializeField] private Script_PRCSPlayer namePlatePRCSPlayer;
    [SerializeField] private PlayableDirector nameplateDirector;
    [SerializeField] private Script_ItemObject smallKey;
    [SerializeField] private Script_ItemObject boarNeedle;
    [SerializeField] private Script_TreasureChest treasureChest;

    [SerializeField] private Script_InteractableFullArt IdsLeaveMeBeNote;
    [SerializeField] private Script_InteractableFullArt DeadIds;

    private bool DDR = false;
    private bool isIdsDancing = false;
    private float timer;
    private int leftMoveCount;
    private int downMoveCount;
    private int upMoveCount;
    private int rightMoveCount;
    private bool leftMoveDone;
    private bool downMoveDone;
    private bool upMoveDone;
    private bool rightMoveDone;
    
    private bool didMapNotification;
    private bool isTimelineControlled = false;

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
        nameplateDirector.stopped                       += OnNameplateDone;
        Script_DDREventsManager.OnDDRDone               += OnDDRDone;
        Script_ItemsEventsManager.OnItemStash           += OnItemStash;
    }

    protected override void OnDisable()
    {
        Script_GameEventsManager.OnLevelInitComplete    -= OnLevelInitCompleteEvent;
        
        IdsDirector.stopped                             -= OnIdsMovesDone;
        nameplateDirector.stopped                       -= OnNameplateDone;
        Script_DDREventsManager.OnDDRDone               -= OnDDRDone;
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

    protected override void HandleAction()
    {   
        base.HandleDialogueAction();
    }

    // ------------------------------------------------------------------------------------
    // Next Node Action Start
    public void NameplateTimeline()
    {
        game.ChangeStateCutScene();
        namePlatePRCSPlayer.Play();
    }
    
    public void IdsWalkToERoom()
    {
        game.ChangeStateCutScene();
        Script_VCamManager.VCamMain.SwitchToMainVCam(Script_VCamManager.ActiveVCamera);
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 1);
    }

    // pass DDR node
    public void IdsGivesSmallKey()
    {
        game.ChangeStateCutScene();
        game.HandleItemReceive(smallKey);
    }

    public void WaitToIdsDance()
    {
        game.ChangeStateCutScene();
        SwitchLightsOutAnimation(OnIdsDance);

        void OnIdsDance()
        {
            game.PlayNPCBgTheme(IdsCandyDanceShortThemePlayerPrefab);
            isIdsDancing = true;
            crystalChandelier.GetComponent<Script_CrystalChandelier>()
                .StartSpinning();

            DeskSwitchIn();
            isDeskSwitchedIn = true;
        }
    }

    public void WaitToDDR()
    {
        DDRManager.Activate();
        DDRManager.StartMusic(
            playerSongMoves,
            PlayerCandyDanceThemePlayerPrefab,
            mistakesAllowed
        );
        crystalChandelier.GetComponent<Script_CrystalChandelier>()
            .StartSpinning();
        
        DDR = true;
        game.ChangeStateDDR(); // this triggers the HandleDDRFinish
    }

    public void OnDDRFailDialogueDone()
    {
        game.ChangeStateInteract();
    }

    public void DDRTryAgain()
    {
        game.ChangeStateCutScene();
        SwitchLightsOutAnimation(WaitToDDR);
    }
    
    // ------------------------------------------------------------------------------------
    // Timeline Signals START
    public void OnIdsExitsIdsRoom()
    {
        Ids.gameObject.SetActive(false);
    }

    public void OnQuestDone()
    {
        isCurrentPuzzleComplete = true;
        
        isTimelineControlled = true;
        Script_TransitionManager.Control.OnCurrentQuestDone(() => {
            game.ChangeStateInteract();
            isTimelineControlled = false;
        }, Script_TransitionManager.FinalNotifications.Ids);
    }

    // ------------------------------------------------------------------------------------

    // Happens after Pass DDR and after Ids gives Super Small Key.
    private void OnItemStash(string stashItemId)
    {
        if (stashItemId == smallKey.Item.id)            IdsExits();
        else if (stashItemId == boarNeedle.Item.id)     gotBoarNeedle = true;

        void IdsExits()
        {
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 2);
        }
    }
    
    private void OnNameplateDone(PlayableDirector aDirector)
    {
        namePlatePRCSPlayer.Stop();
        dm.StartDialogueNode(AfterIntroRevealNode, SFXOn: false);
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
        
        if (lb9.speaker != null)
        {
            lb9.speaker.audioSource.Pause();
            Destroy(lb9.speaker.gameObject);
        }
        else
        {
            game.PauseBgMusic();
        }
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

        if (lb9.speaker != null)
        {
            lb9.speaker.audioSource.Pause();
            Destroy(lb9.speaker.gameObject);
        }

        game.ChangeStateCutScene();
        
        game.PlayerFaceDirection(Directions.Right);
        game.GetMovingNPC(0).FaceDirection(Directions.Left);
        dm.StartDialogueNode(danceIntroNode);
        
        activeTriggerIndex++;
        return true;
    }

    void HandleIdsDanceScene()
    {
        if (isIdsDancing)
        {
            timer += Time.deltaTime;
            
            HandleLeftMove();
            HandleDownMove();
            HandleUpMove();
            HandleRightMove();

            /// Once stops playing Ids song
            if (!game.GetNPCThemeMusicIsPlaying())
            {
                crystalChandelier.GetComponent<Script_CrystalChandelier>()
                    .StopSpinning();
                isIdsDancing = false;
                StartCoroutine(WaitToTalkAfterIdsDance());
            }
        }

        IEnumerator WaitToTalkAfterIdsDance()
        {
            yield return new WaitForSeconds(postIdsDanceWaitTime);
            
            game.GetMovingNPC(0).FaceDirection(Directions.Left);
            dm.StartDialogueNode(playerDanceIntroNode);
        }

        void HandleLeftMove()
        {
            if (leftMoveCount > IdsSongMoves.leftMoveTimes.Length - 1)    return;

            if (timer >= IdsSongMoves.leftMoveTimes[leftMoveCount] && !leftMoveDone)
            {
                game.GetMovingNPC(0).FaceDirection(Directions.Left);
                leftMoveCount++;
                leftMoveDone = true;
            }
            else
            {
                leftMoveDone = false;
            }
        }

        void HandleDownMove()
        {
            if (downMoveCount > IdsSongMoves.downMoveTimes.Length - 1)    return;

            if (timer >= IdsSongMoves.downMoveTimes[downMoveCount] && !downMoveDone)
            {
                game.GetMovingNPC(0).FaceDirection(Directions.Down);
                downMoveCount++;
                downMoveDone = true;
            }
            else
            {
                downMoveDone = false;
            }
        }

        void HandleUpMove()
        {
            if (upMoveCount > IdsSongMoves.upMoveTimes.Length - 1)    return;

            if (timer >= IdsSongMoves.upMoveTimes[upMoveCount] && !upMoveDone)
            {
                game.GetMovingNPC(0).FaceDirection(Directions.Up);
                upMoveCount++;
                upMoveDone = true;
            }
            else
            {
                upMoveDone = false;
            }
        }

        void HandleRightMove()
        {
            if (rightMoveCount > IdsSongMoves.rightMoveTimes.Length - 1)    return;

            if (timer >= IdsSongMoves.rightMoveTimes[rightMoveCount] && !rightMoveDone)
            {
                game.GetMovingNPC(0).FaceDirection(Directions.Right);
                rightMoveCount++;
                rightMoveDone = true;
            }
            else
            {
                rightMoveDone = false;
            }
        }
    }

    void OnDDRDone()
    {
        if (DDRManager.didFail)
        {
            Debug.Log($"**** OnDDRDone starting Bad Node ****");
            DDRFinish(badDanceOutroNode);
        }
        else
        {
            Debug.Log($"**** OnDDRDone starting Good Node ****");
            DDRFinish(goodDanceOutroNode);
        }
    }

    void DDRFinish(Script_DialogueNode node)
    {
        DDR = false;
        game.ManagePlayerViews(Const_States_PlayerViews.Health);
        game.ChangeStateCutScene();
        game.PlayerFaceDirection(Directions.Right);
        
        crystalChandelier.GetComponent<Script_CrystalChandelier>()
            .StopSpinning();
        SwitchLightsInAnimation();

        dm.StartDialogueNode(node);
    }

    void SwitchLightsOutAnimation(Action cb)
    {
        // todo: setinactive after
        crystalChandelier.SetActive(true);
        StartCoroutine(
            lights.GetComponent<Script_MoveDirection>().MoveSmooth(
                dropDiscoBallTime,
                lightsUpOffset,
                () => {
                    if (cb != null) cb();
                }
            )
        );
        StartCoroutine(
            crystalChandelier.GetComponent<Script_MoveDirection>().MoveSmooth(
                dropDiscoBallTime,
                crystalChandelierDownOffset,
                null
            )
        );
        StartCoroutine(playerSpotLight.FadeInLightOnTarget(
            dropDiscoBallTime,
            game.GetPlayerTransform(),
            null
        ));
        StartCoroutine(IdsSpotLight.FadeInLightOnTarget(
            dropDiscoBallTime,
            game.GetNPC(0).GetComponent<Transform>(),
            null
        ));
    }

    void SwitchLightsInAnimation()
    {
        StartCoroutine(
            lights.GetComponent<Script_MoveDirection>().MoveSmooth(
                dropDiscoBallTime,
                lightsDownOffset,
                null
            )
        );
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
    }

    public override void HandleDDRArrowClick(int tier) {}

    void ChaseLoungeSwitchIn()
    {
        SetupIOsDialogue(chaiseLoungeIONode);
        chaiseLoungeObject.SetActive(true);
        deskObject.SetActive(false);
    }

    void DeskSwitchIn()
    {
        SetupIOsDialogue(deskIONode);
        chaiseLoungeObject.SetActive(false);
        deskObject.SetActive(true);
    }

    void SetupIOsDialogue(Script_DialogueNode node)
    {
        List<Script_InteractableObject> IOs = game.GetInteractableObjects();
        foreach (Script_InteractableObject IO in IOs)
        {
            if (IO.nameId == "chaise-lounge")
            {
                IO.SwitchDialogueNodes(new Script_DialogueNode[]{node});
            }
        }
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
            foreach (Script_Trigger t in triggers)  t.gameObject.SetActive(true);
        }

        void HandleIdsNotHome()
        {
            Script_BackgroundMusicManager.Control.Stop();
            
            // Ids leaves note when he's not home on Weekend Day 2.
            IdsLeaveMeBeNote.gameObject.SetActive(true);

            // Ids only dies if not talked to by Weekend Day 3.
            DeadIds.gameObject.SetActive(false);

            Ids.gameObject.SetActive(false);
            foreach (Script_Trigger t in triggers)  t.gameObject.SetActive(false);
        }

        void HandleIdsDead()
        {
            Script_BackgroundMusicManager.Control.Stop();
            
            // Ids note only appears when he's not home on Weekend Day 2.
            IdsLeaveMeBeNote.gameObject.SetActive(false);

            // Ids dies since not talked to by Weekend Day 3.
            DeadIds.gameObject.SetActive(true);

            Ids.gameObject.SetActive(false);
            foreach (Script_Trigger t in triggers)  t.gameObject.SetActive(false);
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
            playerSpotLight.Setup(0f);
            IdsSpotLight.Setup(0f);
        }
        
        foreach (Transform t in IOTexts )   game.SetupInteractableObjectsText(t, !isInitialized);
        game.SetupMovingNPC(Ids, !isInitialized);
        
        if (!isDeskSwitchedIn)      ChaseLoungeSwitchIn();
        else                        DeskSwitchIn();

        // Ids DDR quest is completed and the Locked Treasure Chest was opened.
        if (gotBoarNeedle)
        {
            treasureChest.IsOpen = true;
        }

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

    public override void Setup()
    {
        Ids.SetMoveSpeedWalk();
        
        if (lb9.speaker == null)    game.SwitchBgMusic(4);

        BaseSetup();
        HandleIdsInRoom();
        HandleIdsSpawn();
    }
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
    }
}
#endif