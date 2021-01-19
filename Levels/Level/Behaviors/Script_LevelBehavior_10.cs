using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;
using System;

/// <summary>
/// Respawns are handled with IdsSpawns which will auto reset Ids positions
/// timelines to play are tracked with timelineCount
/// Uses MoveSets for approaching player
/// 
/// Events:
/// Ids should only be here on Sunday
/// </summary>
[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_10 : Script_LevelBehavior
{
    public bool isDone;
    [SerializeField] private Script_Trigger[] triggers;
    [SerializeField] private int activeTriggerIndex;
    [SerializeField] private int timelineCount;
    [SerializeField] private Script_Marker[] IdsSpawns;
    public bool isDeskSwitchedIn;
    public bool isInitialized;


    public float afterPRCSWaitTime;
    public float dropDiscoBallTime;
    public float postIdsDanceWaitTime;
    

    public Script_DialogueManager dm;
    public Script_DDRManager DDRManager;
    public Script_LevelBehavior_9 lb9;
    public Script_BgThemePlayer IdsBgThemePlayerPrefab;
    public Script_BgThemePlayer IdsCandyDanceShortThemePlayerPrefab;
    public Script_BgThemePlayer PlayerCandyDanceThemePlayerPrefab;
    public Script_DialogueNode introNode;
    [SerializeField] private Script_DialogueNode afterIntroRevealNode;
    public Script_DialogueNode introNode2;
    public Script_DialogueNode introNode3;
    public Script_DialogueNode danceIntroNode;
    public Script_DialogueNode playerDanceIntroNode;
    public Script_DialogueNode badDanceOutroNode;
    public Script_DialogueNode goodDanceOutroNode;
    public Script_DialogueNode deskIONode;
    public Script_DialogueNode chaiseLoungeIONode;
    [SerializeField] private Script_DialogueNode showIdsOnKelsingorPRCSNode;
    [SerializeField] private Script_DialogueNode showMagicCircleOnKelsingorNode;
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
    [SerializeField] private PlayableDirector ZoomDirector;
    [SerializeField] private Script_VCamera VCamLB10FollowIds;
    [SerializeField] private Script_PRCSPlayer magicCirclePlayer;
    [SerializeField] private PlayableDirector magicCircleDirector; 
    [SerializeField] private Script_PRCSPlayer namePlatePRCSPlayer;
    [SerializeField] private PlayableDirector nameplateDirector;
    [SerializeField] private Script_ItemObject smallKey;

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

    protected override void OnEnable()
    {
        IdsDirector.stopped                     += OnIdsMovesDone;    
        ZoomDirector.stopped                    += OnZoomDirectorDone;
        magicCircleDirector.stopped             += OnPRCSIntroDone;
        Script_PRCSEventsManager.OnPRCSDone     += PRCSAenimalsRoleReaction;
        nameplateDirector.stopped               += OnNameplateDone;
        Script_DDREventsManager.OnDDRDone       += OnDDRDone;
        Script_ItemsEventsManager.OnItemStash   += OnItemStash;
        
        if (timelineCount >= IdsSpawns.Length)  Ids.gameObject.SetActive(false);
        else
        {
            Ids.transform.position = IdsSpawns[timelineCount].Position;
            Ids.DefaultFacingDirection = IdsSpawns[timelineCount].Direction;
        }

        Ids.UpdateLocation();
        if (timelineCount == 0)
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);
    }

    protected override void OnDisable()
    {
        IdsDirector.stopped                     -= OnIdsMovesDone;
        ZoomDirector.stopped                    -= OnZoomDirectorDone;
        magicCircleDirector.stopped             -= OnPRCSIntroDone;
        Script_PRCSEventsManager.OnPRCSDone     -= PRCSAenimalsRoleReaction;
        nameplateDirector.stopped               -= OnNameplateDone;
        Script_DDREventsManager.OnDDRDone       -= OnDDRDone;
        Script_ItemsEventsManager.OnItemStash   -= OnItemStash;
        
        Script_AudioMixerVolume.SetVolume(
            audioMixer,
            Const_AudioMixerParams.ExposedBGVolume,
            1f
        );
    }
    
    protected override void Update()
    {
        HandleAction();
        HandleIdsDanceScene();
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

    public void ZoomInOnPlayer()
    {
        GetComponent<Script_TimelineController>().PlayableDirectorPlay(1);
    }
    public void MagicCircleIntro()
    {
        magicCirclePlayer.Play();
    }
    public void PRCSShowMagicCircle()
    {
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(2, 4);
    }
    public void PRCSIntroStop()
    {
        magicCirclePlayer.Stop();
        StartCoroutine(WaitToStartSBookIntroNode());

        IEnumerator WaitToStartSBookIntroNode()
        {
            yield return new WaitForSeconds(afterPRCSWaitTime);
            dm.StartDialogueNode(introNode2, SFXOn: false);
        }
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
    // Next Node Action END
    // ------------------------------------------------------------------------------------
    // ------------------------------------------------------------------------------------
    // Timeline Signals START
    public void OnIdsExitsIdsRoom()
    {
        Ids.gameObject.SetActive(false);
        game.ChangeStateInteract();

        isDone = true;
    }

    // Timeline Signals END
    // ------------------------------------------------------------------------------------
    private void OnItemStash(string stashItemId)
    {
        if (stashItemId == smallKey.Item.id)    IdsExits();

        void IdsExits()
        {
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 2);
        }
    }
    

    private void OnNameplateDone(PlayableDirector aDirector)
    {
        namePlatePRCSPlayer.Stop();
        dm.StartDialogueNode(afterIntroRevealNode, SFXOn: false);
    }

    private void PRCSAenimalsRoleReaction(Script_PRCSPlayer PRCSPlayer)
    {
        if (PRCSPlayer == magicCirclePlayer)
        {
            Debug.Log("PRCS (Aenimals Role) DONE");

            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(2, 3);
        }
    }

    private void OnPRCSIntroDone(PlayableDirector aDirector)
    {
        if (magicCircleDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[3])
        {
            dm.StartDialogueNode(showIdsOnKelsingorPRCSNode, SFXOn: false);
        }
        else if (magicCircleDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[4])
        {
            dm.StartDialogueNode(showMagicCircleOnKelsingorNode, SFXOn: false);
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
        else if (aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[2])
        {
            Ids.FaceDirection(Directions.Right);
            game.ChangeStateInteract();
        }
        Ids.UpdateLocation();
        timelineCount++;
    }
    private void OnZoomDirectorDone(PlayableDirector aDirector)
    {
        dm.StartDialogueNode(introNode3, false);
        game.EnableSBook(true);
    }

    public override bool ActivateTrigger(string Id)
    {
        if (Id == "room_N" && activeTriggerIndex == 0)
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
            game.PlayNPCBgTheme(IdsBgThemePlayerPrefab);
            game.ChangeStateCutScene();
            game.PlayerFaceDirection(Directions.Up);
            dm.StartDialogueNode(introNode);
            
            Script_VCamManager.VCamMain.SetNewVCam(VCamLB10FollowIds);
            
            // if (activeTriggerIndex == triggerLocations.Length - 1) isDone = true;
            activeTriggerIndex++;
            return true;
        }
        else if (Id == "room_E" && activeTriggerIndex == 1)
        {
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

        return false;
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

    public override void Setup()
    {
        Ids.SetMoveSpeedWalk();
        if (!isInitialized)
        {
            crystalChandelier.SetActive(false);
            playerSpotLight.Setup(0f);
            IdsSpotLight.Setup(0f);
        }
        foreach (Transform t in IOTexts )   game.SetupInteractableObjectsText(t, !isInitialized);
        game.SetupMovingNPC(Ids, !isInitialized);
        isInitialized = true;
        
        if (!isDeskSwitchedIn)      ChaseLoungeSwitchIn();
        else                        DeskSwitchIn();
        
        if (lb9.speaker == null)    game.SwitchBgMusic(4);

        if (!game.IsRunDay(Script_Run.DayId.sun))
        {
            Ids.gameObject.SetActive(false);
            foreach (Script_Trigger t in triggers)  t.gameObject.SetActive(false);
        }
        else
        {
            Ids.gameObject.SetActive(true);
            foreach (Script_Trigger t in triggers)  t.gameObject.SetActive(true);
        }
    }
}
