using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(Script_TimelineController))]
public class Script_Elevator : Script_InteractableObjectExit
{
    public enum Types
    {
        Default     = 0,
        Last        = 1,
        GrandMirror = 2,
    }
    public static string IsClosed                   = "IsClosed";
    public static string CloseTrigger               = "Close";
    public static float preDoorCloseWaitTime        = 0.5f;
    
    [SerializeField] private Types type;
    [SerializeField] private bool isClosed          = true;
    [SerializeField] private Animator doorsAnimator;
    [SerializeField] private Script_ElevatorBehavior elevatorExitBehavior;

    public Types Type
    {
        get => type;
        set => type = value;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
        Script_PlayerEventsManager.OnEnteredElevator += OnEnteredElevator;
        UpdateState();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        Script_PlayerEventsManager.OnEnteredElevator -= OnEnteredElevator;
    }
    
    protected override void Start()
    {
        base.Start();
        
        Debug.Log($"Setting Elevator IsClosed: {isClosed}");
        // UpdateState();
    }

    protected override void ActionDefault()
    {
        if (isClosed)
        {
            game.ChangeStateCutScene();
            
            /// Elevator in World Opens -> OnElevatorDoorsOpened()
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);
            SetClosedState(false);

            Script_BackgroundMusicManager.Control.FadeOutMed();
        }
    }

    /// <summary>
    /// Using Timeline
    /// </summary>
    public void SetClosing()
    {
        // doorsAnimator.SetTrigger(CloseTrigger); /// Using Timeline instead
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 1);
        SetClosedState(true);
    }

    public void SetClosedState(bool _isClosed)
    {
        Debug.Log($"Setting closed state to {_isClosed}");
        
        isClosed = _isClosed;
        UpdateState();
    }

    private void UpdateState()
    {
        doorsAnimator.SetBool(IsClosed, isClosed);
    }

    // ------------------------------------------------------------------
    // Signal Reactions START
    
    /// <summary>
    /// -> OnEnteredElevator()
    /// Afterwards:
    /// 1) Player steps in
    /// 2) Emits EnterElevator signal
    /// 2) Script_Elevator:OnEnteredElevatorCanvas() reacts, shows closing of elevator doors
    /// </summary>
    public void OnElevatorDoorsOpened()
    {
        game.GetPlayer().EnterElevator();
    }

    /// <summary>
    /// Called after World elevator doors are done closing on current active Elevator
    /// (To:Elevator)
    /// </summary>
    public void OnElevatorDoorsClosedArrival()
    {
        Debug.Log($"{name} OnElevatorDoorsClosedArrival()");
        game.ChangeStateInteract();
    }
    // Signal Reactions END
    // ------------------------------------------------------------------
    
    private void OnEnteredElevator()
    {
        Script_Game.Game.GetPlayer().FaceDirection(Directions.Down);

        /// Elevator Canvas Closing Timeline
        StartCoroutine(WaitToCloseDoors());

        IEnumerator WaitToCloseDoors()
        {
            yield return new WaitForSeconds(preDoorCloseWaitTime);

            /// Call manager to handle Canvas UI
            switch (type)
            {
                case (Script_Elevator.Types.Default):
                    game.ElevatorCloseDoorsCutScene(exit, elevatorExitBehavior, Type);
                    break;
                case (Script_Elevator.Types.Last):
                    game.ElevatorCloseDoorsCutScene(exit, elevatorExitBehavior, Type);
                    break;
                case (Script_Elevator.Types.GrandMirror):
                    // Manager contains Grand Mirror info.
                    game.ElevatorCloseDoorsCutScene(null, null, Type);
                    break;
                default:
                    game.ElevatorCloseDoorsCutScene(exit, elevatorExitBehavior, Type);
                    break;
            }
        }    
    }
}
