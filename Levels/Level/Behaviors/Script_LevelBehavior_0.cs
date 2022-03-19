using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;

/// <summary>
/// Play Ids intro run timeline either after cut scene is done or upon initiation on Mon & Wed.
/// </summary>

[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_0 : Script_LevelBehavior
{
    // =======================================================================
    //  STATE DATA
    public bool didStartThought;
    public bool[] demonSpawns;
    public bool isDone;
    // =======================================================================

    private Script_PlayerMovementAnimator playerMovementAnimator;
    [SerializeField] private string hintTriggerId;
    
    [SerializeField] private Script_Hint hint; 
    
    [SerializeField] private Script_PRCS wellJustOpened; 
    [SerializeField] private Script_DemonNPC Ids;

    [SerializeField] private Transform defaultTrees;
    [SerializeField] private Transform finalTrees;

    private bool didIdsRun;

    private void Start()
    {
        Debug.Log($"{name} didStartThought: {didStartThought}");
        
        if (!didStartThought)
        {
            Debug.Log($"**** {name} starting wells cut scene ****");
            Script_PRCSManager.Control.OpenPRCSNoFade(wellJustOpened);
        }
    }
    
    protected override void OnEnable()
    {
    }

    protected override void OnDisable()
    {
    }

    public override void OnLevelInitComplete()
    {
        if (!didStartThought)
        {
            game.ChangeStateCutScene();
            /// Start Timeline fading in the well light
            wellJustOpened.PlayMyTimeline();
        }
        else
        {
            HandlePlayIdsTimeline();   
        }
    }
    
    public override bool ActivateTrigger(string Id){
        if (Id == hintTriggerId && !isDone)
        {
            hint.Show();
            return true;
        }        

        return false;
    }

    /// <summary>
    /// NextNodeAction START ===============================================================
    /// </summary>
    public void OnWellCutSceneDone()
    {
        
    }
    
    /// <summary>
    /// NextNodeAction END ===============================================================
    /// </summary>
    /// ===========================================================================================
    /// Signal Reactions START 
    /// ===========================================================================================
    public void OnWellJustOpenedDone()
    {
        Script_PRCSManager.Control.HidePRCS(wellJustOpened, FadeSpeeds.Slow, () => {
            game.ChangeStateInteract();
            didStartThought = true;

            HandlePlayIdsTimeline();
        });
    }

    public void OnIdsTimelineDone()
    {
        game.ChangeStateInteract();
        didIdsRun = true;
    }
    /// Signal Reactions END ========================================================================

    // On Mon (Tutorial Run) or Wed, Ids should lead you into the Mansion.
    private void HandlePlayIdsTimeline()
    {
        if (ShouldPlayIdsIntro())
        {
            game.ChangeStateCutScene();
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(1, 1);
        }
    }

    private bool ShouldPlayIdsIntro()
    {
        return !didIdsRun && (
            Script_EventCycleManager.Control.IsIdsRoomIntroDay()
            || Script_EventCycleManager.Control.IsLastElevatorTutorialRun()
        );
    }

    public override void Setup()
    {
        playerMovementAnimator = game.GetPlayer().MyAnimator.GetComponent<Script_PlayerMovementAnimator>();

        if (ShouldPlayIdsIntro())
            Ids.gameObject.SetActive(true);
        else
            Ids.gameObject.SetActive(false);
        
        defaultTrees.gameObject.SetActive(true);
        finalTrees.gameObject.SetActive(false);
    }
}
