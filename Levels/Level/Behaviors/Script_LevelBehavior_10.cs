using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;

/// <summary>
/// Respawns are handled with IdsSpawns which will auto reset Ids positions
/// timelines to play are tracked with timelineCount
/// Uses MoveSets for approaching player
/// </summary>
[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_10 : Script_LevelBehavior
{
    public bool isDone;
    [SerializeField] private int activeTriggerIndex;
    [SerializeField] private int timelineCount;
    [SerializeField] private Vector3[] IdsSpawns;
    [SerializeField] Script_TileMapExitEntrance exitInfo;
    [SerializeField] Script_AudioOneShotSource oneShotPrefab;
    public bool isDeskSwitchedIn;
    public bool isUnlocked;
    public bool isSpecterStoryStarted;
    public bool isInitialized;


    public float afterPRCSWaitTime;
    public float dropDiscoBallTime;
    public float postIdsDanceWaitTime;
    public float IdsWaitTimeBeforeUnlocking;
    public float IdsExitWaitTimeAfterUnlock;
    public Vector3 IdsExitLocation;
    

    public Script_DialogueManager dm;
    public Script_DDRManager DDRManager;
    public Script_Exits exitsHandler;
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
    public Script_DialogueNode specterStoryNode;
    public Script_DialogueNode finalComplimentNode;
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
    public Script_DoorLock doorLock;
    public GameObject chaiseLoungeObject;
    public GameObject deskObject;
    public AudioSource audioSource;
    public AudioMixer audioMixer;
    public AudioMixerGroup audioMixerGroup;
    public AudioClip exitSFX;
    public float exitVol;
    public AudioClip onExitFadeOutDoneSFX;
    public float onExitFadeOutDoneVol;
    public float fadeOutTransitionTime;
    
    public Model_SongMoves playerSongMoves;
    public Model_SongMoves IdsSongMoves;
    public int mistakesAllowed;
    public Transform[] IOTexts;
    public Script_MovingNPC_Ids Ids;
    [SerializeField] private PlayableDirector IdsDirector;
    [SerializeField] private PlayableDirector ZoomDirector;
    [SerializeField] private Script_VCamera VCamLB10FollowIds;
    [SerializeField] private Script_PRCSPlayer magicCirclePlayer;
    [SerializeField] private PlayableDirector magicCircleDirector; 
    [SerializeField] private Script_PRCSPlayer namePlatePRCSPlayer;
    [SerializeField] private PlayableDirector nameplateDirector;

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
    private bool isFinalComplimentDone;

    protected override void OnEnable()
    {
        IdsDirector.stopped                 += OnIdsMovesDone;    
        ZoomDirector.stopped                += OnZoomDirectorDone;
        magicCircleDirector.stopped         += OnPRCSIntroDone;
        Script_PRCSEventsManager.OnPRCSDone += PRCSAenimalsRoleReaction;
        nameplateDirector.stopped           += OnNameplateDone;
        
        if (timelineCount >= IdsSpawns.Length)  Ids.gameObject.SetActive(false);
        else                                    Ids.transform.position = IdsSpawns[timelineCount];

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
        
        Script_AudioMixerVolume.SetVolume(
            audioMixer,
            Const_AudioMixerParams.ExposedBGVolume,
            1f
        );
    }
    
    protected override void Update()
    {
        // HandleNPCActuallyMove();
        HandleAction();

        HandleIdsDanceScene();
        if (game.state == "ddr" && DDR)    HandlePlayerDDRFinish();
    }

    public override void HandleDialogueNodeAction(string a)
    {
        if (a == "ids-dance")      WaitToIdsDance();
        else if (a == "DDR")
        {
            DDR = true;
            WaitToDDR();
        }
        else if (a == "no-to-go-deeper")
        {
            game.ChangeStateInteract();
        }
        else if (a == "approach-exit")
        {
            game.ChangeStateCutSceneNPCMoving();
            game.GetMovingNPC(0).ApproachTarget(
                IdsExitLocation,
                Vector3.zero,
                Directions.Left,
                NPCEndCommands.None
            );
        }
        else if (a == "exit")            StartCoroutine(WaitToIdsExit());
    }

    /// <summary>
    /// NextNodeAction START ===========================================================================
    /// </summary>
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
    public void IdsWalkToWRoom()
    {
        game.ChangeStateCutScene();
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 2);
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
    /// <summary>
    /// NextNodeAction END  ===========================================================================
    /// </summary>

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
        }
        else if (aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[1])
        {
            Ids.FaceDirection(Directions.Left);
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
        else if (Id == "room_W" && activeTriggerIndex == 2)
        {
            if (lb9.speaker != null)
            {
                lb9.speaker.audioSource.Pause();
                Destroy(lb9.speaker.gameObject);
            }
            
            game.ChangeStateCutSceneNPCMoving();
            game.PlayerFaceDirection(Directions.Left);
            game.GetMovingNPC(0).ApproachTarget(
                game.GetPlayerLocation(),
                new Vector3(-2f, 0, 0),
                Directions.Right,
                NPCEndCommands.None
            );
            
            activeTriggerIndex++;
            return true;
        }

        return false;
    }    

    void WaitToIdsDance()
    {
        game.ChangeStateCutScene();
        SwitchLightsOutAnimation();
    }

    void WaitToDDR()
    {
        DDRManager.Activate();
        DDRManager.StartMusic(
            playerSongMoves,
            PlayerCandyDanceThemePlayerPrefab,
            mistakesAllowed
        );
        crystalChandelier.GetComponent<Script_CrystalChandelier>()
            .StartSpinning();
        
        game.ChangeStateDDR(); // this triggers the HandleDDRFinish
    }

    IEnumerator WaitToTalkAfterIdsDance()
    {
        yield return new WaitForSeconds(postIdsDanceWaitTime);
        
        game.GetMovingNPC(0).FaceDirection(Directions.Left);
        dm.StartDialogueNode(playerDanceIntroNode);
    }

    IEnumerator WaitToIdsExit()
    {
        yield return new WaitForSeconds(IdsWaitTimeBeforeUnlocking);
        doorLock.Unlock();
    }

    void HandleIdsDanceScene()
    {
        if (isIdsDancing && game.GetNPCBgThemeActive())
        {
            timer += Time.deltaTime;
            
            HandleLeftMove();
            HandleDownMove();
            HandleUpMove();
            HandleRightMove();

            if (!game.GetNPCThemeMusicIsPlaying())
            {
                crystalChandelier.GetComponent<Script_CrystalChandelier>()
                    .StopSpinning();
                isIdsDancing = false;
                StartCoroutine(WaitToTalkAfterIdsDance());
            }
        }
    }

    void HandlePlayerDDRFinish()
    {
        // handle fail case
        if (DDRManager.didFail)
        {
            DDRFinish(badDanceOutroNode);
        }
        else if (!game.GetNPCThemeMusicIsPlaying())
        {
            DDRManager.Deactivate();
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

    protected override void HandleAction()
    {   
        base.HandleDialogueAction();
    }

    void SwitchLightsOutAnimation()
    {
        // todo: setinactive after
        crystalChandelier.SetActive(true);
        StartCoroutine(
            lights.GetComponent<Script_MoveDirection>().MoveSmooth(
                dropDiscoBallTime,
                lightsUpOffset,
                () => {
                    game.PlayNPCBgTheme(IdsCandyDanceShortThemePlayerPrefab);
                    isIdsDancing = true;
                    crystalChandelier.GetComponent<Script_CrystalChandelier>()
                        .StartSpinning();

                    DeskSwitchIn();
                    isDeskSwitchedIn = true;
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

    public override void HandleDDRArrowClick(int tier) {

    }

    public override void HandleMovingNPCAllMovesDone()
    {
        if (activeTriggerIndex == 3 && !isSpecterStoryStarted)
        {
            isSpecterStoryStarted = true;
            game.ChangeStateCutScene();
            dm.StartDialogueNode(specterStoryNode);
            Ids.SetMute(false);
        }
        else if (activeTriggerIndex == 3 && !isFinalComplimentDone)
        {
            isFinalComplimentDone = true;
            game.ChangeStateCutScene();
            dm.StartDialogueNode(finalComplimentNode);
        }
    }

    // animation done callback
    public override void OnDoorLockUnlock(int id)
    {
        StartCoroutine(DoorLockUnlockAction(id));
    }

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

    IEnumerator DoorLockUnlockAction(int id)
    {
        yield return new WaitForSeconds(IdsExitWaitTimeAfterUnlock);

        Ids.gameObject.SetActive(false);
        game.ChangeStateInteract();
        game.DisableExits(false, 0);
        isUnlocked = true;
        isDone = true;
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

    public override void HandleExitCutScene()
    {
        game.ChangeStateCutScene();
        game.GetPlayer().isInvisible = true;
        audioSource.PlayOneShot(exitSFX, exitVol);
        
        // handle if player went back a room and activated persisting proximity speaker
        if (lb9.speaker != null)
        {
            lb9.speaker.audioSource.outputAudioMixerGroup = audioMixerGroup;
        }

        StartCoroutine(
            Script_AudioMixerFader.Fade(
                audioMixer,
                Const_AudioMixerParams.ExposedBGVolume,
                fadeOutTransitionTime,
                0f,
                // continue to handle if player activated persisting proximity speaker from previous room
                () => {
                    if (lb9.speaker != null)
                    {
                        lb9.speaker.audioSource.Stop();
                        Destroy(lb9.speaker.gameObject);
                    }           
                }
            )
        );
        StartCoroutine(
            game.TransitionFadeIn(fadeOutTransitionTime, () => {
                audioSource.PlayOneShot(onExitFadeOutDoneSFX, onExitFadeOutDoneVol);
                Instantiate(oneShotPrefab, Vector2.zero, Quaternion.identity);
                // game.MelanholicTitleCutScene();
                game.Exit(
                    exitInfo.Level,
                    exitInfo.PlayerNextSpawnPosition,
                    exitInfo.PlayerFacingDirection,
                    true,
                    true
                );
            })
        );
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
        
        if (!isDeskSwitchedIn)
        {
            ChaseLoungeSwitchIn();
        }
        else
        {
            DeskSwitchIn();
        }
        
        if (lb9.speaker == null)
        {
            game.SwitchBgMusic(4);
        }

        // TODO: COMBINE THIS STATE WITH DOOR LOCK?
        if (!isDone)
        {
            // we're in mid convo in last room
            if (activeTriggerIndex >= 3)
            {
                // if isSpecterStoryStarted = false, will go back to question
                isSpecterStoryStarted = true;
                Ids.SetMute(false);
            }
            else
            {
                Ids.SetMute(true);
            }
        }
        else
        {
            Ids.gameObject.SetActive(false);
        }

        if (!isUnlocked)
        {
            game.DisableExits(true, 0);
        }
        else
        {
            game.DisableExits(false, 0);
            doorLock.gameObject.SetActive(false);
        }
    }
}
