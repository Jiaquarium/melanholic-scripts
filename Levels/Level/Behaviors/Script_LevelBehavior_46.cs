using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    [SerializeField] private float successTransitionFadeInTime;
    [SerializeField] private float successBlackScreenTime;
    [SerializeField] private float successAfterFadeWaitTime;
    
    [SerializeField] private Script_MeetupPuzzleController meetupPuzzleController;
    
    // Needed to initialize "Player" copy.
    [SerializeField] private Script_LevelGrid levelGrid;
    
    [SerializeField] private Script_PuppetMaster puppetMaster;
    [SerializeField] private Script_Puppet puppet;
    
    [SerializeField] private Script_Marker puppetMasterSpawn;
    [SerializeField] private Script_Marker puppetSpawn;

    [SerializeField] private Script_DialogueNode LatteOpeningNode;
    [SerializeField] private Script_DialogueNode KaffeOpeningNode;
    [SerializeField] private float openingCameraBlendTime;
    
    [SerializeField] private Script_DialogueNode KaffeBlockedNode;
    [SerializeField] private Script_DialogueNode LatteBlockedNode;
    [SerializeField] private Script_DialogueNode successCutSceneNode;

    [SerializeField] private Script_TriggerEnterOnce KaffeBlockedTrigger;
    [SerializeField] private Script_TriggerEnterOnce LatteBlockedTrigger;

    [SerializeField] private Script_VCamera followKaffeOpeningVCam;
    [SerializeField] private Script_VCamera followLatteOpeningVCam;
    [SerializeField] private Script_VCamera followKaffeVCam;
    [SerializeField] private Script_VCamera followLatteVCam;
    [SerializeField] private Script_VCamera followKaffeCloseupVCam;
    [SerializeField] private Script_VCamera followLatteCloseupVCam;
    [SerializeField] private Script_VCamera successCutSceneVCam;

    [SerializeField] private Script_Marker puppetMasterPuzzleSuccessSpawn;
    [SerializeField] private Script_Marker puppetPuzzleSuccessSpawn;
    [SerializeField] private Script_Marker playerPuzzleSuccessSpawn;
    [SerializeField] private Transform movingLabyrinth;

    [SerializeField] private Script_ScarletCipherPiece scarletCipherPiece;

    private Script_VCamera preOpeningVCam;
    private Script_VCamera puppeteerVCam;
    private Script_VCamera preSuccessVCam;

    private bool isUniqueBlockedCutScene;
    private bool isOpeningCutSceneDone;

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
            preSuccessVCam = Script_VCamManager.ActiveVCamera;
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
    public void PuzzleSuccessCutSceneDialogue()
    {
        StartCoroutine(WaitToSuccessCutSceneDialogue());
        
        IEnumerator WaitToSuccessCutSceneDialogue()
        {
            yield return new WaitForSeconds(successAfterFadeWaitTime);
            
            SnapCloseUpVCams();
            KaffeCloseUp();

            UpdateLatte();
            Script_DialogueManager.DialogueManager.StartDialogueNode(successCutSceneNode);
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
        Debug.Log($"{name} Opening cut scene!");
        
        game.ChangeStateCutScene();

        preOpeningVCam = Script_VCamManager.ActiveVCamera;
        Script_VCamManager.VCamMain.SwitchBetweenVCams(preOpeningVCam, followLatteOpeningVCam);
        
        StartCoroutine(WaitForCameraBlend());

        IEnumerator WaitForCameraBlend()
        {
            yield return new WaitForSeconds(openingCameraBlendTime);
            
            Script_DialogueManager.DialogueManager.StartDialogueNode(LatteOpeningNode);
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
        
        Script_DialogueManager.DialogueManager.StartDialogueNode(LatteBlockedNode);
        
        puppeteerVCam = Script_VCamManager.ActiveVCamera;
        Script_VCamManager.VCamMain.SwitchBetweenVCams(puppeteerVCam, followLatteVCam);
        
        LatteBlockedTrigger.gameObject.SetActive(false);

        IsUniqueBlockedCutScene = true;
    }

    private void KaffeBlockedCutScene()
    {
        game.ChangeStateCutScene();
        puppetMaster.SetAnimatorControllerActive(false);
        
        Script_DialogueManager.DialogueManager.StartDialogueNode(KaffeBlockedNode);
        
        puppeteerVCam = Script_VCamManager.ActiveVCamera;
        Script_VCamManager.VCamMain.SwitchBetweenVCams(puppeteerVCam, followKaffeVCam);
        
        KaffeBlockedTrigger.gameObject.SetActive(false);

        IsUniqueBlockedCutScene = true;
    }

    public void OnBlockedCutSceneDone()
    {
        game.ChangeStateInteract();
        
        puppet.SetAnimatorControllerActive(true);
        puppetMaster.SetAnimatorControllerActive(true);

        Script_VCamManager.VCamMain.SwitchBetweenVCams(Script_VCamManager.ActiveVCamera, puppeteerVCam);
        puppeteerVCam = null;

        IsUniqueBlockedCutScene = false;
    }

    private void LatteOpeningCloseUp()
    {
        Script_VCamManager.VCamMain.SwitchBetweenVCams(Script_VCamManager.ActiveVCamera, followLatteOpeningVCam);
    }

    private void KaffeOpeningCloseUp()
    {
        Script_VCamManager.VCamMain.SwitchBetweenVCams(Script_VCamManager.ActiveVCamera, followKaffeOpeningVCam);
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
            preSuccessVCam = Script_VCamManager.ActiveVCamera;
            Script_VCamManager.VCamMain.SwitchToMainVCam(preSuccessVCam);
            
            yield return new WaitForSeconds(successBlackScreenTime);

            // Fade out black screen.
            StartCoroutine(game.TransitionFadeOut(successTransitionFadeInTime, () => {
                isPuzzleComplete        = true;
                isCurrentPuzzleComplete = true;

                Script_TransitionManager.Control.OnCurrentQuestDone(() => {
                    if (!didPlayFaceOff)
                    {
                        var PRCSManager = Script_PRCSManager.Control;

                        PRCSManager.TalkingSelfSequence(() => {
                            PRCSManager.PlayFaceOffTimeline(() => {
                                game.ChangeStateInteract();
                                didPlayFaceOff = true;
                            });
                        });
                    }
                    else
                    {
                        game.ChangeStateInteract();
                    }
                });                
            }));
        }
    }
    
    // ------------------------------------------------------------------
    // Next Node Actions
    
    // After Latte's Opening node
    public void SlowPanCameraToKaffeOpening()
    {
        Script_VCamManager.VCamMain.SwitchBetweenVCams(Script_VCamManager.ActiveVCamera, followKaffeOpeningVCam);
        
        StartCoroutine(WaitForCameraBlend());

        IEnumerator WaitForCameraBlend()
        {
            yield return new WaitForSeconds(openingCameraBlendTime);
            
            Script_DialogueManager.DialogueManager.StartDialogueNode(KaffeOpeningNode);
        }
    }

    public void OnOpeningCutSceneDone()
    {
        Script_VCamManager.VCamMain.SwitchBetweenVCams(Script_VCamManager.ActiveVCamera, preOpeningVCam);
        preOpeningVCam = null;

        StartCoroutine(WaitForCameraBlend());

        IEnumerator WaitForCameraBlend()
        {
            yield return new WaitForSeconds(openingCameraBlendTime);
            
            game.ChangeStateInteract();

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

    public void KaffeCloseUp()
    {
        Script_VCamManager.VCamMain.SwitchBetweenVCams(Script_VCamManager.ActiveVCamera, followKaffeCloseupVCam);
    }

    public void LatteCloseUp()
    {
        Script_VCamManager.VCamMain.SwitchBetweenVCams(Script_VCamManager.ActiveVCamera, followLatteCloseupVCam);
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
        }
    }
    #endif
}

