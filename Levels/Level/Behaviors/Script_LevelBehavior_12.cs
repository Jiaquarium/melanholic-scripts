using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Audio;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Uses legacy action strings instead of NextNodeAction Unity Event handler
/// </summary>
[RequireComponent(typeof(Script_TimelineController))]
[RequireComponent(typeof(Script_AudioMixerVolume))]
[RequireComponent(typeof(AudioSource))]
public class Script_LevelBehavior_12 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool isDone;
    public bool isCutSceneDone;
    /* ======================================================================= */
    
    [SerializeField] private bool isInit = true;
    [SerializeField] private Script_SyncedTriggerPuzzleController puzzleTriggerController;
    
    [SerializeField] private Transform playerReflectionEro;
    
    [SerializeField] private Transform pushablesParent;
    [SerializeField] private Transform fullArtParent;
    
    [SerializeField] private Transform triggers;
    [SerializeField] private Script_TriggerEnterOnce[] cutSceneTriggers;
    
    [SerializeField] private Transform cutScenePlayerTargetDestination;
    
    [SerializeField] private Script_DialogueManager dialogueManager;
    
    [SerializeField] private Script_DialogueNode thinkingNode;
    [SerializeField] private Script_DialogueNode erasTalkingNode;
    [SerializeField] private Script_DialogueNode erasQuestionNode;
    [SerializeField] private Script_DialogueNode erasConfirmNode;
    [SerializeField] private Script_DialogueNode erasFinalNode;
    
    [SerializeField] private Script_BgThemePlayer erasBgThemePuzzlePlayerPrefab;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource SFXSource;
    
    [SerializeField] private Script_VCamera staticZoomOutVCam;
    
    [SerializeField] private float beforeTalkToErasWaitTime;
    [SerializeField] private float preShakeRoomWaitTime;
    [SerializeField] private float shakeRoomMisfireWaitTime;
    [SerializeField] private float beforeShakeRoomWaitTime;
    [SerializeField] private float beforeShakeFadeOutMusicTime;
    [SerializeField] private float shakeRoomTime;
    [SerializeField] private float afterShakeRoomWaitTime;
    [SerializeField] private float bgMusicFadeInTime;
    
    [SerializeField] private Script_Fireplace fireplacePlayer;
    [SerializeField] private Script_Fireplace fireplaceMirror;
    
    [SerializeField] private Transform explosion;
    
    [SerializeField] TimelineAsset playerApproachMirrorTimeline;
    [SerializeField] TimelineAsset playerRetreatTimeline;
    public List<GameObject> playerObjsToBind = new List<GameObject>();
    
    [SerializeField] private AudioSource proximityCracklingFire;
    [SerializeField] private AudioSource proximityFire;
    [SerializeField] private PlayableDirector fireplacePlayable;
    
    [SerializeField] private float zoomBackInTime;
    
    private PlayableDirector playerPlayableDirector;
    
    private int playerMovesNeededToReachMirror;

    protected override void OnEnable()
    {
        Script_PuzzlesEventsManager.OnPuzzleProgress += OnPuzzleProgress;
        Script_PuzzlesEventsManager.OnPuzzleSuccess += OnPuzzleComplete;
        fireplacePlayable.stopped += OnFireplaceExplosionDone;

        Debug.Log("LB12: enabled");
        if (!isDone)
        {
            Debug.Log("LB12: Changing camera now");
            CutScenesDoneCamera();
        }
    }
    protected override void OnDisable()
    {
        Script_PuzzlesEventsManager.OnPuzzleProgress -= OnPuzzleProgress;
        Script_PuzzlesEventsManager.OnPuzzleSuccess -= OnPuzzleComplete;
        fireplacePlayable.stopped -= OnFireplaceExplosionDone;

        playerPlayableDirector = null;

        if (Script_VCamManager.VCamMain != null)
            Script_VCamManager.VCamMain.SwitchToMainVCam(staticZoomOutVCam);
        
        Script_AudioMixerVolume.SetVolume(
            audioMixer,
            Const_AudioMixerParams.ExposedBGVolume,
            1f
        );
    }
    
    public override bool ActivateTrigger(string Id)
    {
        if (Id == "cut-scene_thinking" && !isCutSceneDone)
        {
            ThinkingCutScene();
            return true;
        }
        else if (Id == "cut-scene_eras-laughs" && !isCutSceneDone)
        {
            // ErasLaughsCutScene();
            ApproachMirrorCutScene();
            return true;
        }

        return false;
    }

    public override void HandleDialogueNodeAction(string a)
    {
        if (a == "interact")
        {
            game.ChangeStateInteract();
        }
        // else if (a == "pre-shake-room")
        // {
        //     PreShakeRoom();
        // }
        // else if (a == "shake-room-misfire")
        // {
        //     ShakeRoomMisfireSequence();
        // }
        // else if (a == "shake-room")
        // {
        //     ShakeRoomSequence();
        // }
        // else if (a == "finish-cut-scenes")
        // {
            
        // }
    }

    /// <summary>
    /// NextNodeAction(s) Start =============================================================================
    /// </summary>
    public void Interact()
    {
        game.ChangeStateInteract();   
    }
    
    public void PreShakeRoom()
    {
        // fade out music
        StartCoroutine(
            Script_AudioMixerFader.Fade(
                audioMixer,
                Const_AudioMixerParams.ExposedBGVolume,
                beforeShakeFadeOutMusicTime,
                0f,
                () => { game.StopBgTheme(); }
            )
        );
        
        // wait a bit, then talk
        StartCoroutine(WaitToStartErasQuestion());
        
        IEnumerator WaitToStartErasQuestion()
        {
            yield return new WaitForSeconds(preShakeRoomWaitTime);
            dialogueManager.StartDialogueNode(erasQuestionNode);   
        }
    }

    public void ShakeRoomMisfireSequence()
    {   
        // wait longer for the shake "misfire," then Eras talks
        StartCoroutine(WaitToStartErasConfirm());
        
        IEnumerator WaitToStartErasConfirm()
        {
            yield return new WaitForSeconds(shakeRoomMisfireWaitTime);
            dialogueManager.StartDialogueNode(erasConfirmNode);   
        }
    }

    public void ShakeRoomSequence()
    {
        StartCoroutine(WaitToShakeRoom());
        
        IEnumerator WaitToShakeRoom()
        {
            yield return new WaitForSeconds(beforeShakeRoomWaitTime);

            // shake room
            SFXSource.PlayOneShot(Script_SFXManager.SFX.heartBeat, Script_SFXManager.SFX.heartBeatVol);
            Script_VCamManager.VCamMain.GetComponent<Script_CameraShake>().Shake(
                shakeRoomTime,
                Const_Camera.Shake.AmplitudeMed,
                Const_Camera.Shake.FrequencyDefault,
                null
            );
            yield return new WaitForSeconds(shakeRoomTime);

            yield return new WaitForSeconds(afterShakeRoomWaitTime);
            // start fade in music
            dialogueManager.StartDialogueNode(erasFinalNode);
        }
    }
    
    public void FinishCutScenes()
    {
        isCutSceneDone = true;
        game.ChangeStateInteract();
        game.UnPauseBgMusic();       
        FadeInMusic();
        CutScenesDoneCamera();
    }

    /// /// <summary>
    /// NextNodeAction(s) End =============================================================================
    /// </summary>

    public void CutScenesDoneCamera()
    {
        Script_VCamManager.VCamMain.SetNewVCam(staticZoomOutVCam);
    }

    public void FadeInMusic()
    {
        // set master volume to 0 to prep fade in
        Script_AudioMixerVolume.SetVolume(
            audioMixer,
            Const_AudioMixerParams.ExposedBGVolume,
            0f    
        );            

        // fade in master volume
        StartCoroutine(
            Script_AudioMixerFader.Fade(
                audioMixer,
                Const_AudioMixerParams.ExposedBGVolume,
                bgMusicFadeInTime,
                1f,
                null
            )
        );
    }

    void OnPuzzleProgress()
    {    
        print("PUZZLE PROGRESS: FIRE BECOMES BIGGER!!!");
        fireplacePlayer.GrowFire();
        fireplaceMirror.GrowFire();

        // when nearing SUCCESS switch out audioClip of prox speaker with "fire"
        if (puzzleTriggerController.currentSuccessCount == 1)
        {
            proximityFire.gameObject.SetActive(true);
            proximityCracklingFire.volume = 0f;
            proximityCracklingFire.gameObject.SetActive(false);
        }
    }

    public void OnPuzzleComplete(string arg)
    {
        Script_PuzzlesEventsManager.PuzzleProgress();
        
        isDone = true;
        
        game.ChangeStateCutScene();
        game.DisableExits(false, 0);
        
        float defaultFastFadeTime = Script_AudioEffectsManager.GetFadeTime(FadeSpeeds.Fast);
        StartCoroutine(
            Script_AudioMixerFader.Fade(
                audioMixer,
                Const_AudioMixerParams.ExposedBGVolume,
                beforeShakeFadeOutMusicTime,
                0f,
                () => { game.StopBgMusic(); }
            )
        );

        // Bind Player & Player Ghost to the Retreat Timeline & play.
        Script_Player player = Script_Game.Game.GetPlayer();
        
        playerObjsToBind.Clear();
        
        // Player Transform Track
        playerObjsToBind.Add(player.gameObject);
        // Player Signal Receiver Track
        playerObjsToBind.Add(player.gameObject);

        playerPlayableDirector.BindTimelineTracks(playerRetreatTimeline, playerObjsToBind);

        StartCoroutine(WaitForExplosionCutScene());
        
        // Zoom camera back in.
        IEnumerator WaitForExplosionCutScene()
        {
            Script_VCamManager.VCamMain.SwitchToMainVCam(staticZoomOutVCam);

            yield return new WaitForSeconds(zoomBackInTime);

            // play manually do to dynamic handling of Player timeline via Script_TimelineController
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);
            
            playerPlayableDirector.Play(playerRetreatTimeline);
        }
    }

    public void ThinkingCutScene()
    {
        game.ChangeStateCutScene();
        game.PlayerFaceDirection(Directions.Up);
        dialogueManager.StartDialogueNode(thinkingNode);        
    }

    /// <summary>
    /// called from trigger
    /// </summary>
    public void ApproachMirrorCutScene()
    {
        game.PauseBgMusic();
        game.ChangeStateCutScene();
        game.PlayerFaceDirection(Directions.Up);
        
        // calc distance from mirror
        playerMovesNeededToReachMirror = (int)Mathf.Abs(game.GetPlayer().transform.position.z
            - cutScenePlayerTargetDestination.position.z);
        
        Debug.Log($"playerMovesNeededToReachMirror: {playerMovesNeededToReachMirror}");
        HandleMovePlayerToMirror(playerMovesNeededToReachMirror);
    }

    /// <summary>
    /// called from player's playableDirector listener
    /// </summary>
    /// <param name="aDirector">given from listener</param>
    public override void HandlePlayableDirectorStopped(PlayableDirector aDirector)
    {
        print($"HandlePlayableDirectorStopped(): Director: {aDirector}, playableAsset: {aDirector.playableAsset}");
        // no follow up action on puzzle complete player timeline (isDone flag)
        if (aDirector == playerPlayableDirector && !isDone)
            HandleMovePlayerToMirror(playerMovesNeededToReachMirror);
    }
    /// <summary>
    /// called as event listener
    /// </summary>
    private void OnFireplaceExplosionDone(PlayableDirector aDirector)
    {
        print($"HandlePlayableDirectorStopped(): Director: {aDirector}, playableAsset: {aDirector.playableAsset}");
        // finished fireplace explosion timeline
        if (aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[0])
        {
            game.ChangeStateInteract();
        }
    }

    private void HandleMovePlayerToMirror(int distance)
    {
        if (distance == 0)  StartCoroutine(TalkToErasCutScene());
        else                MovePlayerToMirror();
        
        void MovePlayerToMirror()
        {
            playerMovesNeededToReachMirror--;
            playerPlayableDirector.Play(playerApproachMirrorTimeline);
        }
    }


    /// <summary>
    /// from HandlePlayableDirectorStopped() when player reaches target
    /// </summary>
    IEnumerator TalkToErasCutScene()
    {
        // pause
        yield return new WaitForSeconds(beforeTalkToErasWaitTime);
        // slowly fade into fullart dialogue
        // start brass de chocobo song
        game.PlayNPCBgTheme(erasBgThemePuzzlePlayerPrefab);
        dialogueManager.StartDialogueNode(erasTalkingNode);
    }

    public void InitializeFire(bool isClose)
    {
        // initialize fire grow in fireplace
        fireplacePlayer.InitializeFire();
        fireplaceMirror.InitializeFire();

        // initialize fire SFX
        proximityCracklingFire.gameObject.SetActive(!isClose);
        proximityFire.gameObject.SetActive(isClose);
    }

    /// <summary>
    /// To be invoked via SyncedTriggerPuzzleController's InitialState function through C#reflection
    /// </summary>
    public void InitializeFireStart()
    {
        InitializeFire(false);
    }

    private void Awake() {
        explosion.gameObject.SetActive(false);
    }
    
    public override void Setup()
    {
        game.SetupPlayerReflection(playerReflectionEro);
        game.SetupInteractableFullArt(fullArtParent, isInit);
        isInit = false;
        
        playerPlayableDirector = game.GetPlayer().Director;
        
        if (isCutSceneDone)
        {
            foreach (Script_TriggerEnterOnce trigger in cutSceneTriggers)
                trigger.gameObject.SetActive(false);
        }

        if (isDone)
        {
            fireplacePlayable.gameObject.SetActive(false);
            game.DisableExits(false, 0);
            puzzleTriggerController.isComplete = true;
            pushablesParent.gameObject.SetActive(false);
            Script_AudioMixerVolume.SetVolume(
                audioMixer,
                Const_AudioMixerParams.ExposedBGVolume,
                0f
            );
        }
        else
        {
            game.DisableExits(isDisabled: true, 0);

            if (puzzleTriggerController.currentSuccessCount == 1)   InitializeFire(isClose: true);
            else                                                    InitializeFire(isClose: false);
            
            puzzleTriggerController.Setup();
            game.SetupPushables(pushablesParent, isInit);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_LevelBehavior_12))]
public class Script_LevelBehavior_12Tester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_LevelBehavior_12 lb = (Script_LevelBehavior_12)target;
        if (GUILayout.Button("ThinkingCutScene()"))
        {
            lb.ThinkingCutScene();
        }
        
        if (GUILayout.Button("ApproachMirrorCutScene()"))
        {
            lb.ApproachMirrorCutScene();
        }

        if (GUILayout.Button("FadeInMusic()"))
        {
            lb.FadeInMusic();
        }

        if (GUILayout.Button("Complete Puzzle"))
        {
            lb.OnPuzzleComplete(null);
        }
    }
}
#endif