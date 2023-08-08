using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.EventSystems;


[RequireComponent(typeof(Script_TimelineController))]
[RequireComponent(typeof(AudioSource))]
public class Script_LevelBehavior_27 : Script_LevelBehavior
{
    // =======================================================================
    // State Data START
    [SerializeField] private bool _gotPsychicDuck;
    // State Data END
    // =======================================================================

    [SerializeField] private float waitToGivePsychicDuckTime;
    
    [SerializeField] private Transform exitParent;
    [SerializeField] private Script_Elevator elevator; /// Ref'ed by ElevatorManager
    
    [SerializeField] private Script_StickerObject PsychicDuck;
    
    [SerializeField] private Script_TimelineController IdsTimelineController;
    [SerializeField] private Script_DemonNPC Ids;

    [SerializeField] private Script_Marker moveSetDestination;
    
    private bool isGlitched;
    
    private bool isInit = true;

    public bool GotPsychicDuck
    {
        get => _gotPsychicDuck;
        set => _gotPsychicDuck = value;
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();

        if (game.IsEileensMindQuestDone && game.RunCycle == Script_RunsManager.Cycle.Weekday)
        {
            var glitchFXManager = Script_GlitchFXManager.Control;
            glitchFXManager.SetDefault();
            glitchFXManager.SetBlend(1f);
            isGlitched = true;
        }

        Script_NPCEventsManager.OnNPCMovesSetsDone += OnMoveSetDone;

        Script_Game.IsRunningDisabled = true;
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();

        if (isGlitched)
        {
            Script_GlitchFXManager.Control.SetBlend(0f);
            isGlitched = false;
        }

        Script_NPCEventsManager.OnNPCMovesSetsDone -= OnMoveSetDone;

        Script_Game.IsRunningDisabled = false;
    }
    
    private void HandleGrandMirrorPaintingEntrance()
    {
        if (game.IsGrandMirrorSetup())
        {
            // Notify Elevator to go to Grand Mirror Room
            elevator.Type = Script_Elevator.Types.GrandMirror;
        }
        else
        {
            elevator.Type = Script_Elevator.Types.Last;
        }
    }

    // ----------------------------------------------------------------------
    // Next Node Action Unity Events START
    public void GivePsychicDuck()
    {
        // Need to properly exit out, see note in ItemPickUpTheatricsPlayer
        if (GotPsychicDuck)
            return;
        
        StartCoroutine(WaitToGivePsychicDuck());

        IEnumerator WaitToGivePsychicDuck()
        {
            game.ChangeStateCutScene();
            
            yield return new WaitForSeconds(waitToGivePsychicDuckTime);

            game.HandleItemReceive(PsychicDuck);
            GotPsychicDuck = true;

            // Switch Ids Dialoge to Big Ids'
            Ids.MyDialogueState = Script_DemonNPC.DialogueState.Talked;
        }
    }
    // Next Node Action Unity Events END
    // ----------------------------------------------------------------------
    // ----------------------------------------------------------------------
    // Timeline Signals START
    public void OnItemPickUpDone()
    {
        game.ChangeStateCutScene();

        /// Ids moves to door and blocks it
        Ids.SetMovingNPCMatchPlayer(false);
        
        Ids.ApproachTarget(moveSetDestination.transform.position, Vector3.zero, Directions.Down, NPCEndCommands.None);
    }
    
    public void OnIdsMoveToBlockEntranceDone()
    {
        elevator.State = Script_InteractableObject.States.Active;
        Ids.SetExtraInteractableBoxes(true);
        
        // Allow Player to interact with Ids from the side after cut scene.
        Ids.DisableL = false;
        Ids.DisableR = false;
        
        // Reset Ids animator to begin at beginning of animation loop.
        Ids.MyAnimator.ResetAnimator(0);

        game.ChangeStateInteract();
    }
    
    // Timeline Signals END
    // ----------------------------------------------------------------------

    private void OnMoveSetDone()
    {
        IdsTimelineController.PlayableDirectorPlayFromTimelines(0, 0);
    }

    public override void Setup()
    {
        game.SetupInteractableObjectsExit(exitParent, isInit);
        HandleGrandMirrorPaintingEntrance();

        /// Setup Ids intro on Run 0
        if (Script_EventCycleManager.Control.IsLastElevatorTutorialRun())
        {
            Ids.gameObject.SetActive(true);
            Ids.SetExtraInteractableBoxes(false);
            elevator.State = Script_InteractableObject.States.Disabled;
        }
        else
            Ids.gameObject.SetActive(false);
        
        isInit = false;
    }
}