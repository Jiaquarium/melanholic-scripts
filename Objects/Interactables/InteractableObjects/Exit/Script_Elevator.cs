using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(Script_TimelineController))]
public class Script_Elevator : Script_InteractableObjectExit
{
    public static string IsClosed                   = "IsClosed";
    public static string CloseTrigger               = "Close";
    public static float preDoorCloseWaitTime        = 0.5f;
    [SerializeField] private bool isClosed          = true;
    [SerializeField] private Animator doorsAnimator;

    void OnEnable()
    {
        Script_PlayerEventsManager.OnEnteredElevator += OnEnteredElevator;
        UpdateState();
    }

    void OnDisable()
    {
        Script_PlayerEventsManager.OnEnteredElevator -= OnEnteredElevator;
    }
    
    void Start()
    {
        Debug.Log($"Setting Elevator IsClosed: {isClosed}");
        // UpdateState();
    }
    
    public override void ActionDefault()
    {
        if (isClosed)
        {
            game.ChangeStateCutScene();
            
            /// Elevator in World Opens -> OnElevatorDoorsOpened()
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);
            SetClosedState(false);
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


    /// Signal Reactions START ========================================================================
    
    /// <summary>
    /// -> OnEnteredElevator()
    /// Afterwards:
    /// 1) Player steps
    /// 2) Canvas shows closing of elevator doors
    /// </summary>
    public void OnElevatorDoorsOpened()
    {
        Script_Game.Game.GetPlayer().EnterElevator();
    }

    public void OnElevatorDoorsClosedArrival()
    {
        Script_Game.Game.ChangeStateInteract();
    }

    /// Signal Reactions END ========================================================================
    
    private void OnEnteredElevator()
    {
        Script_Game.Game.GetPlayer().FaceDirection(Directions.Down);

        /// Elevator Canvas Closing Timeline
        StartCoroutine(WaitToCloseDoors());

        IEnumerator WaitToCloseDoors()
        {
            yield return new WaitForSeconds(preDoorCloseWaitTime);

            /// Call manager to handle Canvas UI
            game.ElevatorCloseDoorsCutScene(exit);
        }    
    }
}
