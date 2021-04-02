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

    [SerializeField] private Transform exitParent;
    [SerializeField] private Script_Elevator elevator; /// Ref'ed by ElevatorManager
    [SerializeField] private Script_StickerObject PsychicDuck;
    [SerializeField] private Script_TimelineController IdsTimelineController;
    [SerializeField] private Transform Ids;
    [SerializeField] private Transform GrandMirrorPaintingEntrance;
    private bool isInit = true;

    public bool GotPsychicDuck
    {
        get => _gotPsychicDuck;
        set => _gotPsychicDuck = value;
    }
    
    protected override void OnEnable()
    {
        Script_ItemsEventsManager.OnItemPickUpTheatricDone += OnItemPickUpTheatricsDone;
    }

    protected override void OnDisable()
    {
        Script_ItemsEventsManager.OnItemPickUpTheatricDone -= OnItemPickUpTheatricsDone;
    }

    public void OnItemPickUpTheatricsDone(Script_ItemPickUpTheatricsPlayer theatricsPlayer)
    {
        if (theatricsPlayer == PsychicDuck.pickUpTheatricsPlayer)
        {
            game.ChangeStateCutScene();

            /// Ids moves to door and blocks it
            IdsTimelineController.PlayableDirectorPlayFromTimelines(0, 0);
        }
    }

    private void HandleGrandMirrorPaintingEntrance()
    {
        if (game.IsGrandMirrorSetup())
        {
            // switch out elevator for Grand Mirror
            GrandMirrorPaintingEntrance.gameObject.SetActive(true);
            exitParent.gameObject.SetActive(false);
        }
        else
        {
            GrandMirrorPaintingEntrance.gameObject.SetActive(false);
            exitParent.gameObject.SetActive(true);
        }
    }

    // ----------------------------------------------------------------------
    // Next Node Action Unity Events START
    public void GivePsychicDuck()
    {
        if (GotPsychicDuck)     return; // need to properly exit out, see note in ItemPickUpTheatricsPlayer

        game.HandleItemReceive(PsychicDuck);
        GotPsychicDuck = true;
    }
    // Next Node Action Unity Events END
    // ----------------------------------------------------------------------
    // ----------------------------------------------------------------------
    // Timeline Signals START
    public void OnIdsMoveToBlockEntranceDone()
    {
        game.ChangeStateInteract();
    }
    // Timeline Signals END
    // ----------------------------------------------------------------------

    public override void Setup()
    {
        game.SetupInteractableObjectsExit(exitParent, isInit);
        HandleGrandMirrorPaintingEntrance();

        /// Setup Ids intro on Run 0
        if (!GotPsychicDuck && Script_EventCycleManager.Control.IsIdsGivePsychicDuckDay())
            Ids.gameObject.SetActive(true);
        else
            Ids.gameObject.SetActive(false);
        
        isInit = false;
    }
}