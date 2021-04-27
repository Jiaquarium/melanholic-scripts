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

    // ==================================================================

    public bool isCurrentPuzzleComplete;
    
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

    [SerializeField] private Script_DialogueNode KaffeBlockedNode;
    [SerializeField] private Script_DialogueNode LatteBlockedNode;
    [SerializeField] private Script_DialogueNode successCutSceneNode;

    [SerializeField] private Script_TriggerEnterOnce KaffeBlockedTrigger;
    [SerializeField] private Script_TriggerEnterOnce LatteBlockedTrigger;

    [SerializeField] private Script_VCamera followKaffeVCam;
    [SerializeField] private Script_VCamera followLatteVCam;
    [SerializeField] private Script_VCamera followKaffeCloseupVCam;
    [SerializeField] private Script_VCamera followLatteCloseupVCam;
    [SerializeField] private Script_VCamera successCutSceneVCam;

    [SerializeField] private Script_Marker puppetMasterPuzzleSuccessSpawn;
    [SerializeField] private Script_Marker puppetPuzzleSuccessSpawn;
    [SerializeField] private Script_Marker playerPuzzleSuccessSpawn;
    [SerializeField] private Transform movingLabyrinth;

    private Script_VCamera puppeteerVCam;
    private Script_VCamera preSuccessVCam;

    public bool IsDone
    {
        get => meetupPuzzleController.IsDone;
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
    
    protected override void HandleAction()
    {
        // for cutScene dialogue
        base.HandleDialogueAction();
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
            // Move all characters to cut scene position
            puppetMaster.Teleport(puppetMasterPuzzleSuccessSpawn.transform.position);
            puppetMaster.SetAnimatorControllerActive(false);
            
            puppet.Teleport(puppetPuzzleSuccessSpawn.transform.position);
            puppet.SetAnimatorControllerActive(false);
            
            var player = game.GetPlayer();
            player.Teleport(playerPuzzleSuccessSpawn.transform.position);
            player.FaceDirection(Directions.Down);
            
            // Hide Labyrinth
            movingLabyrinth.gameObject.SetActive(false);

            // Switch to Success VCam
            preSuccessVCam = Script_VCamManager.ActiveVCamera;
            Script_VCamManager.VCamMain.SwitchBetweenVCams(preSuccessVCam, successCutSceneVCam);
            
            yield return new WaitForSeconds(successBlackScreenTime);

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

    // ------------------------------------------------------------------
    // Unity Events
    public void OnUniqueBlockingTrigger(Transform other)
    {
        if (other.GetComponent<Script_Puppet>() == puppet)
        {
            game.ChangeStateCutScene();
            Script_DialogueManager.DialogueManager.StartDialogueNode(LatteBlockedNode);
            
            puppeteerVCam = Script_VCamManager.ActiveVCamera;
            Script_VCamManager.VCamMain.SwitchBetweenVCams(puppeteerVCam, followLatteVCam);
            
            LatteBlockedTrigger.gameObject.SetActive(false);
        }
        else if (other.GetComponent<Script_Puppet>() == puppetMaster)
        {
            game.ChangeStateCutScene();
            Script_DialogueManager.DialogueManager.StartDialogueNode(KaffeBlockedNode);
            
            puppeteerVCam = Script_VCamManager.ActiveVCamera;
            Script_VCamManager.VCamMain.SwitchBetweenVCams(puppeteerVCam, followKaffeVCam);
            
            KaffeBlockedTrigger.gameObject.SetActive(false);
        }
    }

    public void OnBlockedCutSceneDone()
    {
        game.ChangeStateInteract();

        Script_VCamManager.VCamMain.SwitchBetweenVCams(Script_VCamManager.ActiveVCamera, puppeteerVCam);
        puppeteerVCam = null;
    }

    // ------------------------------------------------------------------
    // Timeline Signals

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
                // TBD Give Scarlet Cipher Piece?
                
                isPuzzleComplete        = true;
                isCurrentPuzzleComplete = true;

                Script_TransitionManager.Control.OnCurrentQuestDone(() => {
                    game.ChangeStateInteract();
                });                
            }));
        }
    }
    
    // ------------------------------------------------------------------
    // Next Node Actions
    
    // Last Success Dialogue Node.
    public void OnSuccessCutSceneDone()
    {
        // Play Painting Done Timeline.
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

    public void UpdateKaffe()
    {
        Script_Names.UpdateKaffe();
    }

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
    
    public override void Setup()
    {
        if (IsDone)
        {
            DoneState();
        }
        else
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

            puppetMaster.Setup(puppetMasterStartState.faceDirection, puppetMasterStartState, false);
            puppetMaster.InitializeOnLevel(puppetMasterStartState, false, levelGrid.transform);

            puppet.Setup(puppetStartState.faceDirection, puppetStartState, false);
            puppet.InitializeOnLevel(puppetStartState, false, levelGrid.transform);

            meetupPuzzleController.InitialState();
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_LevelBehavior_46))]
public class Script_LevelBehavior_46Tester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_LevelBehavior_46 t = (Script_LevelBehavior_46)target;
        if (GUILayout.Button("Puzzle Success Cut Scene"))
        {
            t.PuzzleSuccessCutScene();
        }

        if (GUILayout.Button("Snap Kaffe VCam"))
        {
            t.SnapCloseUpVCams();
        }
    }
}
#endif