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
    [SerializeField] private Transform exitParent;
    [SerializeField] private Script_Elevator elevator; /// Ref'ed by ElevatorManager
    [SerializeField] private Script_StickerObject PsychicDuck;
    [SerializeField] private Script_TimelineController IdsTimelineController;
    private bool isInit = true;
    private bool gotPsychicDuck = false;
    
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

    // ----------------------------------------------------------------------
    // Next Node Action Unity Events START
    public void GivePsychicDuck()
    {
        if (gotPsychicDuck)     return; // need to properly exit out, see note in ItemPickUpTheatricsPlayer

        game.HandleItemReceive(PsychicDuck);
        gotPsychicDuck = true;
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
        
        isInit = false;
    }
}