using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_22 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool isMyneCutSceneDone;
    /* ======================================================================= */
    
    [SerializeField] public Script_DialogueManager dm;
    [SerializeField] private Script_DialogueNode MDialogueNode;
    [SerializeField] private Script_DialogueNode MApproachedDialogueNode;
    [SerializeField] private float ApproachDialogueWaitTime;
    [SerializeField] private Script_TrackedPushablesTriggerPuzzleController puzzleController;
    [SerializeField] private Script_MovingNPC Myne;
    [SerializeField] private PlayableDirector MyneIntroDirector;
    [SerializeField] private PlayableDirector MyneApproachDirector;
    [SerializeField] private PlayableDirector MyneExitDirector;
    [SerializeField] private Script_VCamera myneFollowCamera;
    [SerializeField] private Script_BgThemePlayer myneBgThemePlayer;
    [SerializeField] private Script_LevelBehavior_24 LB24;
    [SerializeField] private Script_InteractableObjectTextParent textParent;
    private bool isInit = true;

    protected override void OnEnable()
    {
        MyneIntroDirector.stopped += OnMyneIntroPlayableDirectorStopped;
        MyneApproachDirector.stopped += OnApproachedPlayableDirectorStopped;
        MyneExitDirector.stopped += OnExitPlayableDirectorStopped;

        Script_GameEventsManager.OnLevelInitComplete += PlayCutScene;

        if (ShouldPlayCutScene())
        {
            game.PauseBgMusic(); // to avoid unwanted spurt of music at very beginning 
        }
    }

    protected override void OnDisable()
    {
        MyneIntroDirector.stopped -= OnMyneIntroPlayableDirectorStopped;
        MyneApproachDirector.stopped -= OnApproachedPlayableDirectorStopped;
        MyneExitDirector.stopped -= OnExitPlayableDirectorStopped;

        Script_GameEventsManager.OnLevelInitComplete -= PlayCutScene;
    }
    
    private void PlayCutScene()
    {
        if (ShouldPlayCutScene())
        {
            PuzzleCompleteCutScene();
        }
    }

    private void OnMyneIntroPlayableDirectorStopped(PlayableDirector aDirector)
    {
        StartMDialogue();
        myneBgThemePlayer.gameObject.SetActive(true);
    }

    private void OnApproachedPlayableDirectorStopped(PlayableDirector aDirector)
    {
        StartMApproachedDialogue();
    }

    private void OnExitPlayableDirectorStopped(PlayableDirector aDirector)
    {
        game.ChangeStateInteract();
        game.UnPauseBgMusic();
        DefaultCamera();
        isMyneCutSceneDone = true;
    }
    
    /* =========================================================================
    M CUTSCENE START
    ========================================================================= */
    public void PuzzleCompleteCutScene()
    {
        game.PauseBgMusic();
        game.ChangeStateCutScene();
        GetComponent<Script_TimelineController>().PlayableDirectorPlay(0);
        /// Myne is set active via Timeline
    }
    
    /// <summary>
    /// called from Unity Event (OnNextNodeAction())
    /// </summary>
    public void AnimateApproach()
    {
        GetComponent<Script_TimelineController>().PlayableDirectorPlay(1);
    }
    /// <summary>
    /// called from Unity Event (OnNextNodeAction())
    /// </summary>
    public void CutMusic()
    {
        myneBgThemePlayer.GetComponent<AudioSource>().volume = 0f;
        myneBgThemePlayer.gameObject.SetActive(false);
    }

    /// <summary>
    /// dialogue to start when exiting from KTV room2 with the puzzle completed
    /// </summary>
    public void StartMDialogue()
    {
        game.ChangeStateCutScene();
        dm.StartDialogueNode(MDialogueNode);
    }

    public void StartMApproachedDialogue()
    {
        print("starting appraoched dialogue");
        game.ChangeStateCutScene();
        dm.StartDialogueNode(MApproachedDialogueNode);
    }

    public void EndMyneCutScene()
    {
        GetComponent<Script_TimelineController>().PlayableDirectorPlay(2);
        /// Myne is set inactive via Timeline
    }

    private void DefaultCamera()
    {
        Script_VCamManager.VCamMain.SwitchToMainVCam(myneFollowCamera);
    }
    /* ========================================================================= */

    protected override void HandleAction()
    {
        // for cutScene dialogue
        base.HandleDialogueAction();
    }

    private bool ShouldPlayCutScene()
    {
        return LB24.isPuzzleComplete && LB24.didPickUpSpringStone && !isMyneCutSceneDone;
    }

    /// <summary>
    /// use this for pure initializations since Awake() only happens once per game cycle
    /// </summary>
    private void Awake()
    {
        // setup puzzle rooms
        puzzleController.Setup();
        /// Set completion state after set up to ensure no race conditions and we're not
        /// resetting trackables to default position in their Setup
        if (LB24.isPuzzleComplete)  LB24.PuzzleFinishedState();
    }
    
    public override void Setup()
    {
        game.SetupMovingNPC(Myne, isInit);
        game.SetupInteractableObjectsText(textParent.transform, isInit);

        isInit = false;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_LevelBehavior_22))]
public class Script_LevelBehavior_22Tester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_LevelBehavior_22 lb = (Script_LevelBehavior_22)target;
        if (GUILayout.Button("PuzzleCompleteCutScene()"))
        {
            lb.PuzzleCompleteCutScene();
        }

        if (GUILayout.Button("StartMDialogue()"))
        {
            lb.StartMDialogue();
        }

        if (GUILayout.Button("AnimateApproach()"))
        {
            lb.AnimateApproach();
        }

        if (GUILayout.Button("StartMApproachedDialogue()"))
        {
            lb.StartMApproachedDialogue();
        }

        if (GUILayout.Button("EndMyneCutScene()"))
        {
            lb.EndMyneCutScene();
        }
    }
}
#endif