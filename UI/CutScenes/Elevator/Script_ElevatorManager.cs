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
    [SerializeField] private Script_Exits.FollowUp followUp = Script_Exits.FollowUp.CutSceneNoFade;
    [SerializeField] private Script_ExitMetadataObject currentExit;
    [SerializeField] private Script_Elevator currentElevator;
    
    /// <summary>
    /// -> OnDoorsClosed()
    /// </summary>
    public void CloseDoorsCutScene(Script_ExitMetadataObject exit)
    {
        currentExit = exit;
        elevatorCanvasGroupController.Open();
        elevatorTimelineController.PlayableDirectorPlayFromTimelines(0, 0);
    }

    /// Signal Reactions START ========================================================================
    /// <summary>
    /// Called when elevator UI canvas done closing 
    /// </summary>
    public void OnDoorsClosed()
    {
        /// Set up the new level in the background
        Script_Game.Game.Exit(
            currentExit.data.level,
            currentExit.data.playerSpawn,
            currentExit.data.facingDirection,
            true,
            isExitSFXSilent,
            followUp
        );

        currentExit = null;

        SetInitialElevatorState();

        void SetInitialElevatorState()
        {
            /// Start with elevator doors open if transported to a Bay
            Script_LevelBehavior currentLevelBehavior = Script_Game.Game.levelBehavior;
            if (currentLevelBehavior.HasField(elevatorName))
            {
                Debug.Log($"Setting initial state of: {elevatorName}");
                currentElevator = currentLevelBehavior.GetField<Script_Elevator>(elevatorName);
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
        Debug.Log("Done opening UI elevator doors; animate doors closing");
        
        currentElevator.SetClosing();
    }

    /// Signal Reactions END ========================================================================

    public void Setup()
    {
        elevatorCanvasGroupController.Close();   
    }
}