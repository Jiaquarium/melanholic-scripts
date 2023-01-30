using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Garden Labyrinth
/// </summary>
public class Script_LevelBehavior_46 : Script_LevelBehavior
{
    // ==================================================================
    // State Data
    public bool isPuzzleComplete;
    public bool didPlayFaceOff;

    // ==================================================================

    public bool isCurrentPuzzleComplete;
    
    [SerializeField] private Script_InteractablePaintingEntrance[] paintingEntrances;
    [SerializeField] private Script_InteractablePaintingEntrance ballroomPaintingEntrance;
    
    [Space][Header("Opening Cut Scene")][Space]
    
    [SerializeField] private float openingAfterPsyDuckSwitchWaitTime;
    [SerializeField] private Script_DialogueNode LatteOpeningNode;
    [SerializeField] private Script_DialogueNode KaffeOpeningNode;
    [SerializeField] private float openingCameraBlendTime;
    [SerializeField] private Script_VCamera followKaffeOpeningVCam;
    [SerializeField] private Script_VCamera followLatteOpeningVCam;
    
    [Space][Header("Success Cut Scene")][Space]
    
    [SerializeField] private float successTransitionFadeInTime;
    [SerializeField] private float successBlackScreenTime;
    [SerializeField] private float successAfterFadeWaitTime;
    [SerializeField] private float successAfterPsyDuckSwitchWaitTime;
    [SerializeField] private Script_DialogueNode successCutSceneNode;
    [SerializeField] private Script_VCamera successCutSceneVCam;
    [SerializeField] private Script_Marker puppetMasterPuzzleSuccessSpawn;
    [SerializeField] private Script_Marker puppetPuzzleSuccessSpawn;
    [SerializeField] private Script_Marker playerPuzzleSuccessSpawn;
    
    [Space][Header("Blocked Cut Scene")][Space]

    [Tooltip("Less than blend time of ANY CAMERA to KaffeFollow and LatteFollow")]
    [SerializeField] private float onBlockedDoneWaitToInteractTime;
    [SerializeField] private Script_DialogueNode KaffeBlockedNode;
    [SerializeField] private Script_DialogueNode LatteBlockedNode;
    [SerializeField] private Script_TriggerEnterOnce KaffeBlockedTrigger;
    [SerializeField] private Script_TriggerEnterOnce LatteBlockedTrigger;

    [Space]
    
    [SerializeField] private Script_MeetupPuzzleController meetupPuzzleController;
    
    // Needed to initialize "Player" copy.
    [SerializeField] private Script_LevelGrid levelGrid;
    
    [SerializeField] private Script_PuppetMaster puppetMaster;
    [SerializeField] private Script_Puppet puppet;
    
    [SerializeField] private Script_Marker puppetMasterSpawn;
    [SerializeField] private Script_Marker puppetSpawn;

    [SerializeField] private Script_VCamera followKaffeVCam;
    [SerializeField] private Script_VCamera followLatteVCam;
    [SerializeField] private Script_VCamera followKaffeCloseupVCam;
    [SerializeField] private Script_VCamera followLatteCloseupVCam;
    
    [SerializeField] private Transform movingLabyrinth;

    [SerializeField] private Script_ScarletCipherPiece scarletCipherPiece;

    [SerializeField] private Script_MeshFadeController gazeboFader;
    [SerializeField] private Script_Interactable gazebo;

    // Dev
    [SerializeField] private Script_Marker topFloorSwitchLocation;

    private Script_VCamera preOpeningVCam;
    private Script_VCamera puppeteerVCam;
    private Script_VCamera preSuccessVCam;

    private bool isUniqueBlockedCutScene;
    private bool isOpeningCutSceneDone;

    private bool didPsychicDuckSwitch;

    private bool isInitialized;

    public bool IsDone
    {
        get => meetupPuzzleController.IsDone;
    }

    public bool IsInitialized
    {
        get => isInitialized;
    }

    public bool IsUniqueBlockedCutScene
    {
        get => isUniqueBlockedCutScene;
        private set => isUniqueBlockedCutScene = value;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        Script_PuzzlesEventsManager.OnPuzzleSuccess += HandlePuzzleSuccess;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        Script_PuzzlesEventsManager.OnPuzzleSuccess -= HandlePuzzleSuccess;
    }

    public override void OnLevelInitComplete()
    {
        base.OnLevelInitComplete();

        // Play Kaffe Latte Opening Cut Scene.
        if (!isOpeningCutSceneDone)
        {
            OpeningCutScene();
        }
    }
    
    private void HandlePuzzleSuccess(string puzzleId)
    {
        if (puzzleId == meetupPuzzleController.PuzzleId)
        {
            PuzzleSuccessCutScene();
        }
    }

    public void PuzzleSuccessCutScene()
    {
        game.ChangeStateCutScene();
        
        StartCoroutine(game.TransitionFadeIn(successTransitionFadeInTime, () => {
            StartCoroutine(WaitToSetupCutScene());
        }));

        IEnumerator WaitToSetupCutScene()
        {
            var player = game.GetPlayer();
            
            // Remove Puppeteer Effect Hold if Active
            if (player.IsPuppeteerEffectHoldOn)
            {
                player.HandlePuppeteerEffectHold();
                // Instantly stop the Effect Hold animation with the default wait time
                // so Puppeteer is in default non-Effect when timeline starts.
                player.AnimatorEffectHold = false;
            }

            // Turn off SFX Volume, so won't hear Floor Switches go back up if standing on one
            Script_BackgroundMusicManager.Control.SetVolume(0f, Const_AudioMixerParams.ExposedSFXVolume);

            // Move all characters to cut scene position
            puppetMaster.Teleport(puppetMasterPuzzleSuccessSpawn.transform.position);
            puppetMaster.SetAnimatorControllerActive(false);
            
            puppet.Teleport(puppetPuzzleSuccessSpawn.transform.position);
            puppet.SetAnimatorControllerActive(false);
            
            var previousPosition = player.transform.position;
            
            player.Teleport(playerPuzzleSuccessSpawn.transform.position);
            player.FaceDirection(Directions.Down);
            
            // Snap main VCam for after cut scene
            game.SnapActiveCam(previousPosition);

            // Hide Labyrinth
            movingLabyrinth.gameObject.SetActive(false);

            // Switch to Success VCam
            preSuccessVCam = Script_VCamManager.VCamMain.ActiveVCamera;
            Script_VCamManager.VCamMain.SwitchBetweenVCams(preSuccessVCam, successCutSceneVCam);
            
            yield return new WaitForSeconds(successBlackScreenTime);

            // Turn back on SFX Volume
            Script_BackgroundMusicManager.Control.SetVolume(1f, Const_AudioMixerParams.ExposedSFXVolume);

            StartCoroutine(game.TransitionFadeOut(successTransitionFadeInTime, () => {
                // Start cut scene dialogue.
                PuzzleSuccessCutSceneDialogue();
            }));
        }
    }

    // Starts Kaffe's dialogue and snaps camera to Kaffe.
    // Also handle switching Player to Psychic Duck Animator for the cut scene to "understand" Kaffe & Latte
    public void PuzzleSuccessCutSceneDialogue()
    {
        StartCoroutine(WaitToSuccessCutSceneDialogue());
        
        IEnumerator WaitToSuccessCutSceneDialogue()
        {
            yield return new WaitForSeconds(successAfterFadeWaitTime);

            // If player is not Psychic Duck, switch to it and wait 1 more sec
            var activeMaskId = Script_ActiveStickerManager.Control.ActiveSticker?.id ?? string.Empty;
            var isWearingPsychicDuckMask = activeMaskId == Const_Items.PsychicDuckId;
            
            if (!isWearingPsychicDuckMask)
                HandlePlayerSwitchToPsychicDuck();
            else
                StartKaffeLatteCutSceneDialogue();
        }

        void HandlePlayerSwitchToPsychicDuck()
        {
            // Change animator, don't actually switch items
            game.GetPlayer().SetAnimatorControllerPsychicDuck(isActive: true, isSFX: true);
            didPsychicDuckSwitch = true;
            
            StartCoroutine(WaitToStartCutScene());
        }

        void StartKaffeLatteCutSceneDialogue()
        {
            SnapCloseUpVCams();
            KaffeCloseUp();

            UpdateLatte();
            Script_DialogueManager.DialogueManager.StartDialogueNode(successCutSceneNode);
        }

        IEnumerator WaitToStartCutScene()
        {
            yield return new WaitForSeconds(successAfterPsyDuckSwitchWaitTime);

            StartKaffeLatteCutSceneDialogue();            
        }
    }

    public void SnapCloseUpVCams()
    {
        Transform KaffeFocalPoint = followKaffeCloseupVCam.Follow;
        Transform LatteFocalPoint = followLatteCloseupVCam.Follow;
        
        Vector3 prevKaffeFocalPointPos = KaffeFocalPoint.transform.position;
        Vector3 prevLatteFocalPointPos = LatteFocalPoint.transform.position;
        
        game.SnapCam(prevKaffeFocalPointPos, KaffeFocalPoint, followKaffeCloseupVCam.CinemachineVirtualCamera);
        game.SnapCam(prevLatteFocalPointPos, LatteFocalPoint, followLatteCloseupVCam.CinemachineVirtualCamera);
    }

    /// <summary>
    /// 1. Camera slowly pans to Latte (to show location)
    /// 2. Latte opening dialogue
    /// 3. Camera slowly pans to Kaffe
    /// 4. Kaffe opening dialogue
    /// </summary>
    private void OpeningCutScene()
    {
        Dev_Logger.Debug($"{name} Opening cut scene!");
        
        game.ChangeStateCutScene();

        // Handle Player switching to Psychic Duck if needed
        // If player is not Psychic Duck, switch to it and wait 1 more sec
        var activeMaskId = Script_ActiveStickerManager.Control.ActiveSticker?.id ?? string.Empty;
        var isWearingPsychicDuckMask = activeMaskId == Const_Items.PsychicDuckId;

        if (!isWearingPsychicDuckMask)
            HandlePlayerSwitchToPsychicDuck();
        else
            StartOpeningCutScene();

        void HandlePlayerSwitchToPsychicDuck()
        {
            // Change animator, don't actually switch items
            game.GetPlayer().SetAnimatorControllerPsychicDuck(isActive: true, isSFX: true);
            didPsychicDuckSwitch = true;
            
            StartCoroutine(WaitToStartCutScene());
        }

        void StartOpeningCutScene()
        {
            preOpeningVCam = Script_VCamManager.VCamMain.ActiveVCamera;
            Script_VCamManager.VCamMain.SwitchBetweenVCams(preOpeningVCam, followLatteOpeningVCam);
            
            StartCoroutine(WaitForCameraBlend());
        }        

        IEnumerator WaitForCameraBlend()
        {
            yield return new WaitForSeconds(openingCameraBlendTime);
            
            Script_DialogueManager.DialogueManager.StartDialogueNode(LatteOpeningNode);
        }

        IEnumerator WaitToStartCutScene()
        {
            yield return new WaitForSeconds(openingAfterPsyDuckSwitchWaitTime);

            StartOpeningCutScene();            
        }
    }

    // ------------------------------------------------------------------
    // Unity Events
    public void OnUniqueBlockingTrigger(Transform other)
    {
        if (other.GetComponent<Script_Puppet>() == puppet)
            LatteBlockedCutScene();
        else if (other.GetComponent<Script_Puppet>() == puppetMaster)
            KaffeBlockedCutScene();
    }

    private void LatteBlockedCutScene()
    {
        game.ChangeStateCutScene();

        puppet.SetAnimatorControllerActive(false);
        
        // Player will always be in Puppet form in this case
        game.GetPlayer().SetAnimatorControllerPsychicDuck(true);
        Dev_Logger.Debug($"game.GetPlayer().MyAnimator.runtimeAnimatorController {game.GetPlayer().MyAnimator.runtimeAnimatorController}");
        
        Script_DialogueManager.DialogueManager.StartDialogueNode(LatteBlockedNode);
        
        puppeteerVCam = Script_VCamManager.VCamMain.ActiveVCamera;
        Script_VCamManager.VCamMain.SwitchBetweenVCams(puppeteerVCam, followLatteVCam);
        
        LatteBlockedTrigger.gameObject.SetActive(false);

        IsUniqueBlockedCutScene = true;
    }

    private void KaffeBlockedCutScene()
    {
        game.ChangeStateCutScene();
        
        puppetMaster.SetAnimatorControllerActive(false);
        
        // Player will always be in Puppet form in this case
        game.GetPlayer().SetAnimatorControllerPsychicDuck(true);
        Dev_Logger.Debug($"game.GetPlayer().MyAnimator.runtimeAnimatorController {game.GetPlayer().MyAnimator.runtimeAnimatorController}");
        
        Script_DialogueManager.DialogueManager.StartDialogueNode(KaffeBlockedNode);
        
        puppeteerVCam = Script_VCamManager.VCamMain.ActiveVCamera;
        Script_VCamManager.VCamMain.SwitchBetweenVCams(puppeteerVCam, followKaffeVCam);
        
        KaffeBlockedTrigger.gameObject.SetActive(false);

        IsUniqueBlockedCutScene = true;
    }

    public void OnBlockedCutSceneDone()
    {
        puppet.SetAnimatorControllerActive(true);
        puppetMaster.SetAnimatorControllerActive(true);

        Script_VCamManager.VCamMain.SwitchBetweenVCams(Script_VCamManager.VCamMain.ActiveVCamera, puppeteerVCam);
        puppeteerVCam = null;

        StartCoroutine(WaitToInteract());

        // Wait a bit before camera transition time, so it's evident we're switching back from Psyduck
        IEnumerator WaitToInteract()
        {
            yield return new WaitForSeconds(onBlockedDoneWaitToInteractTime);
            game.GetPlayer().SetAnimatorControllerPsychicDuck(isActive: false, isSFX: true);

            game.ChangeStateInteract();
            IsUniqueBlockedCutScene = false;
        }
    }

    private void LatteOpeningCloseUp()
    {
        Script_VCamManager.VCamMain.SwitchBetweenVCams(Script_VCamManager.VCamMain.ActiveVCamera, followLatteOpeningVCam);
    }

    private void KaffeOpeningCloseUp()
    {
        Script_VCamManager.VCamMain.SwitchBetweenVCams(Script_VCamManager.VCamMain.ActiveVCamera, followKaffeOpeningVCam);
    }

    // ------------------------------------------------------------------
    // Timeline Signals

    public void FinishQuestPaintings()
    {
        ballroomPaintingEntrance.DonePainting();

        foreach (var paintingEntrance in paintingEntrances)
        {
            paintingEntrance.DonePainting();
        }
    }

    public void HandleScarletCipherPiece()
    {
        scarletCipherPiece?.UpdateActiveState();
    }
    
    // Follows OnSuccessCutSceneDone's Timeline.
    public void OnCelestialGardensPaintingTimelineDone()
    {
        // Set Kaffe & Latte inactive.
        puppetMaster.gameObject.SetActive(false);
        puppet.gameObject.SetActive(false);
        
        // Fader canvas will be active already from previous Timeline.
        StartCoroutine(WaitToSetupDoneState());

        IEnumerator WaitToSetupDoneState()
        {
            // Switch to Main VCam.
            var transitionManager = Script_TransitionManager.Control;
            preSuccessVCam = Script_VCamManager.VCamMain.ActiveVCamera;
            Script_VCamManager.VCamMain.SwitchToMainVCam(preSuccessVCam);

            
            // If switched to Psychic Duck for cut scene, switch back in BG here.
            if (didPsychicDuckSwitch)
                game.GetPlayer().SetAnimatorControllerPsychicDuck(false);
            
            didPsychicDuckSwitch = false;
            
            yield return new WaitForSeconds(successBlackScreenTime);

            // Fade out black screen.
            StartCoroutine(game.TransitionFadeOut(successTransitionFadeInTime, () => {
                isPuzzleComplete        = true;
                isCurrentPuzzleComplete = true;

                transitionManager.OnCurrentQuestDone(
                    allQuestsDoneCb: () =>
                    {
                        transitionManager.FinalCutSceneAwakening();
                    }, 
                    defaultCb: () =>
                    {
                        HandlePlayFaceOff(OnSuccessDone);
                    }
                );
            }));
        }

        // Face Off is not played if Final Cut Scene Sequence should play.
        void HandlePlayFaceOff(Action cb)
        {
            if (!didPlayFaceOff)
            {
                var PRCSManager = Script_PRCSManager.Control;

                PRCSManager.TalkingSelfSequence(() => {
                    PRCSManager.PlayFaceOffTimeline(() => {
                        cb();
                        didPlayFaceOff = true;
                    });
                },
                isFaceOff: true);
            }
            else
            {
                cb();
            }
        }

        void OnSuccessDone()
        {
            // Fade back in level bgm unless it's KTV2 puzzle
            var bgm = Script_BackgroundMusicManager.Control;
            bgm.UnPause();
            bgm.FadeIn(game.ChangeStateInteract, Script_TransitionManager.FadeTimeSlow, Const_AudioMixerParams.ExposedBGVolume);
        }
    }

    // Called during the OnSuccessCutSceneDone Timeline
    public void SetGazeboInteractive()
    {
        // Fade in Gazebo completely and set colliders active.
        gazeboFader.MaxAlpha = 1f;
        gazeboFader.SetVisibility(true);
        gazebo.SetTargetColliders(true);
    }
    
    // ------------------------------------------------------------------
    // Next Node Actions
    
    // After Latte's Opening node
    public void SlowPanCameraToKaffeOpening()
    {
        Script_VCamManager.VCamMain.SwitchBetweenVCams(Script_VCamManager.VCamMain.ActiveVCamera, followKaffeOpeningVCam);
        
        StartCoroutine(WaitForCameraBlend());

        IEnumerator WaitForCameraBlend()
        {
            yield return new WaitForSeconds(openingCameraBlendTime);
            
            Script_DialogueManager.DialogueManager.StartDialogueNode(KaffeOpeningNode);
        }
    }

    public void OnOpeningCutSceneDone()
    {
        Script_VCamManager.VCamMain.SwitchBetweenVCams(Script_VCamManager.VCamMain.ActiveVCamera, preOpeningVCam);
        preOpeningVCam = null;

        StartCoroutine(WaitForCameraBlend());

        IEnumerator WaitForCameraBlend()
        {
            yield return new WaitForSeconds(openingCameraBlendTime);

            // Switch player back in BG
            if (didPsychicDuckSwitch)
                game.GetPlayer().SetAnimatorControllerPsychicDuck(isActive: false, isSFX: true);
            
            game.ChangeStateInteract();

            didPsychicDuckSwitch = false;
            isOpeningCutSceneDone = true;
        }
    }
    
    // Last Success Dialogue Node.
    public void OnSuccessCutSceneDone()
    {
        // Play Painting Done Timeline.
        // Ballroom Setup via Timeline Signals.
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);
    }

    // Success nodes.
    public void KaffeCloseUp()
    {
        Script_VCamManager.VCamMain.SwitchBetweenVCams(Script_VCamManager.VCamMain.ActiveVCamera, followKaffeCloseupVCam);
    }

    // Success nodes.
    public void LatteCloseUp()
    {
        Script_VCamManager.VCamMain.SwitchBetweenVCams(Script_VCamManager.VCamMain.ActiveVCamera, followLatteCloseupVCam);
    }

    // Called:
    // 1) Latte's Blocking reaction Before Node Action
    // 2) On Success
    public void UpdateKaffe()
    {
        Script_Names.UpdateKaffe();
    }

    // Called:
    // 1) Kaffe's Blocking reaction Before Node Action
    // 2) On Success
    public void UpdateLatte()
    {
        Script_Names.UpdateLatte();
    }

    // ------------------------------------------------------------------

    private void DoneState()
    {
        puppetMaster.gameObject.SetActive(false);
        puppet.gameObject.SetActive(false);
        movingLabyrinth.gameObject.SetActive(false);
    }
    
    // Depending on Execution Order, Meetup Puzzle Controller may need to have
    // Puppets set up beforehand. If so, it will handle initialization.
    public void InitializePuppets()
    {
        Model_PlayerState puppetMasterStartState = new Model_PlayerState(
            (int)puppetMasterSpawn.transform.position.x,
            (int)puppetMasterSpawn.transform.position.y,
            (int)puppetMasterSpawn.transform.position.z,
            puppetMasterSpawn.Direction
        );
        Model_PlayerState puppetStartState = new Model_PlayerState(
            (int)puppetSpawn.transform.position.x,
            (int)puppetSpawn.transform.position.y,
            (int)puppetSpawn.transform.position.z,
            puppetSpawn.Direction
        );

        puppetMaster.Setup(puppetMasterStartState.faceDirection, puppetMasterStartState);
        puppetMaster.InitializeOnLevel(puppetMasterStartState, levelGrid.transform);

        puppet.Setup(puppetStartState.faceDirection, puppetStartState);
        puppet.InitializeOnLevel(puppetStartState, levelGrid.transform);

        isInitialized = true;
    }
    
    public override void Setup()
    {
        if (IsDone)
        {
            DoneState();
        }
        else
        {
            if (!isInitialized)
            {
                InitializePuppets();
            }

            meetupPuzzleController.InitialState();
        }
    }
    
    #if UNITY_EDITOR
    [CustomEditor(typeof(Script_LevelBehavior_46))]
    public class Script_LevelBehavior_46Tester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_LevelBehavior_46 t = (Script_LevelBehavior_46)target;
            if (GUILayout.Button("Latte Blocked Cut Scene"))
            {
                t.LatteBlockedCutScene();
            }

            if (GUILayout.Button("Kaffe Blocked Cut Scene"))
            {
                t.KaffeBlockedCutScene();
            }
            
            if (GUILayout.Button("Puzzle Success Cut Scene"))
            {
                t.PuzzleSuccessCutScene();
                t.meetupPuzzleController.IsDone = true;
            }

            if (GUILayout.Button("Snap Kaffe VCam"))
            {
                t.SnapCloseUpVCams();
            }

            if (GUILayout.Button("Teleport Latte to Top FloorSwitch"))
            {
                t.puppet.Teleport(t.topFloorSwitchLocation.Position);
            }
        }
    }
    #endif
}

