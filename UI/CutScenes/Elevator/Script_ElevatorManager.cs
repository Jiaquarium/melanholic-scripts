using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ElevatorManager : MonoBehaviour
{
    /// Ensure matches level behavior's Elevator property name
    [SerializeField] private string elevatorName;  
    [SerializeField] private Script_TimelineController elevatorTimelineController;
    [SerializeField] private Script_CanvasGroupController elevatorCanvasGroupController;
    [SerializeField] private bool isExitSFXSilent = true;
    [SerializeField] private Script_Exits.ExitType exitType = Script_Exits.ExitType.Elevator;
    [SerializeField] private Script_ExitMetadataObject currentExit;
    [SerializeField] private Script_ElevatorBehavior currentExitBehavior;
    [SerializeField] private Script_Elevator currentElevator;
    
    /// <summary>
    /// UI Closes Elevator Doors
    /// We also set the currentExitBehavior that was passed from the From:Elevator
    /// to be passed to the To:Elevator and called when player is about to interact
    /// -> OnDoorsClosed()
    /// </summary>
    public void CloseDoorsCutScene(
        Script_ExitMetadataObject exit,
        Script_ElevatorBehavior exitBehavior,
        Script_Elevator.Types type
    )
    {
        currentExit = exit;
        currentExitBehavior = exitBehavior;
        elevatorCanvasGroupController.Open();
        
        switch (type)
        {
            case (Script_Elevator.Types.Default):
                elevatorTimelineController.PlayableDirectorPlayFromTimelines(0, 0);
                break;
            case (Script_Elevator.Types.Last):
                elevatorTimelineController.PlayableDirectorPlayFromTimelines(0, 1);
                break;
        }
    }

    /// Signal Reactions START ========================================================================
    /// <summary>
    /// Called when elevator UI canvas done closing
    /// Calls any exit behaviors right after Game.Exit() is called
    /// </summary>
    public void OnDoorsClosed()
    {
        ExitBehavior();
        
        /// Set up the new level in the background
        Script_Game.Game.Exit(
            currentExit.data.level,
            currentExit.data.playerSpawn,
            currentExit.data.facingDirection,
            true,
            isExitSFXSilent,
            exitType
        );

        currentExit = null;
        SetInitialElevatorState();

        void ExitBehavior()
        {
            currentExitBehavior?.Effect();
            currentExitBehavior = null;
        }

        void SetInitialElevatorState()
        {
            /// Start with elevator doors open if transported to a Bay
            Script_LevelBehavior currentLevelBehavior = Script_Game.Game.levelBehavior;
            if (currentLevelBehavior.HasField(elevatorName))
            {
                Debug.Log($"Setting initial state of: {elevatorName}");

                currentElevator = currentLevelBehavior.GetField<Script_Elevator>(elevatorName);
                
                if (!currentElevator.gameObject.activeSelf)
                {
                    Debug.LogWarning("The Elevator exposed by Level Behavior is inactive");
                }

                currentElevator.SetClosedState(false);
            }
            else
            {
                Debug.LogError($"You are not exposing a public {elevatorName} property on current Level Behavior");
            }
        }
    }

    /// <summary>
    /// Called when elevator UI canvas done opening 
    /// </summary>
    public void OnDoorsOpened()
    {
        elevatorCanvasGroupController.Close();

        /// Animate doors closed
        Debug.Log("Done opening UI elevator doors; animate World elevator doors closing");
        
        currentElevator.SetClosing();
    }

    /// Signal Reactions END ========================================================================

    public void Setup()
    {
        elevatorCanvasGroupController.Close();   
    }
}