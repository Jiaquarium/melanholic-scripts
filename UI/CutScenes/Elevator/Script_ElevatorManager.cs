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

    [SerializeField] private Script_ElevatorBehavior currentExitBehavior;
    [SerializeField] private Script_Elevator currentElevator;

    [SerializeField] private Model_Exit currentExitData;

    [SerializeField] private Script_ExitMetadataObject grandMirrorEntrance;

    [SerializeField] private CanvasGroup countdownCanvasGroup;
    [SerializeField] private CanvasGroup lastElevatorMessageCanvasGroup;

    [SerializeField] private FadeSpeeds lastElevatorPromptFadeInTime;
    [SerializeField] private float waitToRevealLastElevatorChoicesTime;
    [SerializeField] private Script_CanvasGroupController lastElevatorPromptController;
    [SerializeField] private Script_CanvasGroupController lastElevatorPromptChoicesController;

    [SerializeField] private Script_Game game;
    [SerializeField] private Script_BackgroundMusicManager bgm;

    [SerializeField] private bool isBgmOn = true;

    public bool IsBgmOn
    {
        get => isBgmOn;
        set => isBgmOn = value;
    }
    
    /// <summary>
    /// UI Closes Elevator Doors
    /// We also set the currentExitBehavior that was passed from the From:Elevator
    /// to be passed to the To:Elevator and called when player is about to interact
    /// -> OnDoorsClosed()
    /// </summary>
    public void CloseDoorsCutScene(
        Script_ExitMetadataObject exit,
        Script_ElevatorBehavior exitBehavior,
        Script_Elevator.Types type,
        Model_Exit exitOverrideData = null,
        Script_Exits.ExitType? exitTypeOverride = null
    )
    {
        // For Last Elevator Sticker.
        currentExitData = exitOverrideData ?? exit?.data;
        
        if (exitTypeOverride != null)
            exitType = (Script_Exits.ExitType)exitTypeOverride;
        
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
            case (Script_Elevator.Types.GrandMirror):
                // Send Player to Grand Mirror room.
                currentExitData = grandMirrorEntrance.data;
                
                elevatorTimelineController.PlayableDirectorPlayFromTimelines(0, 2);
                break;
            case (Script_Elevator.Types.Effect):
                elevatorTimelineController.PlayableDirectorPlayFromTimelines(0, 3);
                break;
        }
    }

    // ------------------------------------------------------------------
    // Timeline Signals
    
    /// <summary>
    /// Called when elevator UI canvas done closing
    /// Calls any exit behaviors right after Game.Exit() is called
    /// </summary>
    public void OnDoorsClosed()
    {
        ExitBehavior();
        
        Debug.Log("@@@@@@@@@ {name} CURRENT EXIT DATA @@@@@@@@@");
        Script_Utils.DebugToConsole(currentExitData);
        
        /// Set up the new level in the background
        Script_Game.Game.Exit(
            currentExitData.level,
            currentExitData.playerSpawn,
            currentExitData.facingDirection,
            true,
            isExitSFXSilent,
            exitType
        );

        currentExitData = null;
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
                currentElevator = null;
                
                Debug.Log($"You are not exposing a public {elevatorName} property on current Level Behavior");
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
        
        // If there exists a World Elevator start animating it
        if (currentElevator != null)
        {
            currentElevator.SetClosing();
        }
        else
        {
            Debug.Log($"{name} There is no current World Elevator, changing state to interact");
            
            game.ChangeStateInteract();
        }

        
        bgm.FadeInMed(outputMixer: Const_AudioMixerParams.ExposedBGVolume);
        
        // When entering Bay v1 from Last Elevator, the BGM will be paused via Behavior.
        if (!bgm.IsPlaying)
        {
            Debug.Log("Start BGM from Elevator Manager");
            game.StartBgMusicNoFade();
        }
        
        IsBgmOn = true;
    }

    // LastElevatorCanvasTimeline
    public void OpenLastElevatorPrompt()
    {
        lastElevatorPromptController.FadeIn(lastElevatorPromptFadeInTime.ToFadeTime());
    }

    // LastElevatorCanvasTimeline_No
    public void OnLastElevatorCanceledTimelineDone()
    {
        Script_Game.Game.ChangeStateInteract();
    }

    // ------------------------------------------------------------------
    // Unity Events
    
    // LastElevatorPrompt UI Text OnTypingDone Event
    public void OpenLastElevatorPromptChoices()
    {
        // Wait before showing choices
        StartCoroutine(WaitToRevealChoices());        

        IEnumerator WaitToRevealChoices()
        {
            yield return new WaitForSeconds(waitToRevealLastElevatorChoicesTime);

            lastElevatorPromptChoicesController.FadeIn(lastElevatorPromptFadeInTime.ToFadeTime());
        }
    }
    
    // LastElevatorPrompt UI Yes
    public void LastElevatorConfirmedTimeline()
    {
        // Fade Screen to Black
        Script_TransitionManager.Control.TimelineFadeIn(FadeSpeeds.Med.GetFadeTime(), () => {
            // Remove prompt
            lastElevatorPromptController.Close();
            lastElevatorPromptChoicesController.Close();

            // Remove Fader
            Script_TransitionManager.Control.TimelineFadeOut(FadeSpeeds.Fast.GetFadeTime(), null, isOver: true);
            
            // Note: Fader takes 0.25 seconds to fade out, ensure Timeline has at least this much
            // of a pause before showing an event.
            // Play Yes Timeline
            elevatorTimelineController.PlayableDirectorPlayFromTimelines(0, 4);
        }, isOver: true);
    }

    // LastElevatorPrompt UI No
    public void LastElevatorCanceledTimeline()
    {
        Script_TransitionManager.Control.TimelineFadeIn(FadeSpeeds.Med.GetFadeTime(), () => {
            // Remove prompt
            lastElevatorPromptController.Close();
            lastElevatorPromptChoicesController.Close();

            // Remove Fader
            Script_TransitionManager.Control.TimelineFadeOut(FadeSpeeds.Fast.GetFadeTime(), null, isOver: true);
            
            // Note: Fader takes 0.25 seconds to fade out, ensure Timeline has at least this much
            // of a pause before showing an event.
            // Play No Timeline
            elevatorTimelineController.PlayableDirectorPlayFromTimelines(0, 5);
        }, isOver: true);
    }

    public void Setup()
    {
        elevatorCanvasGroupController.Close();
        lastElevatorPromptController.Close();
        lastElevatorPromptChoicesController.Close();

        // Do not set alpha because they will only be controlled
        // via Timeline Activation without animating the alpha.
        countdownCanvasGroup.gameObject.SetActive(false);
        lastElevatorMessageCanvasGroup.gameObject.SetActive(false);
    }
}