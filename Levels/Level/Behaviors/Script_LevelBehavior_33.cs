using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LevelBehavior_33 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    
    /* ======================================================================= */
    public enum State
    {
        Default                     = 0,
        Save                        = 1,
        SaveAndStartWeekendCycle    = 2,
    }

    [SerializeField] private Transform exitParent;
    [SerializeField] private Script_Elevator elevator; /// Ref'ed by ElevatorManager
    [SerializeField] private State _behavior;  // Use an elevator Effect to change this
    [SerializeField] private Script_TileMapExitEntrance exitToLobby;
    [SerializeField] private Script_InteractableObjectText elevatorDisabledText;
    [SerializeField] private Script_InteractableObjectText elevatorSundayDisabledText;
    [SerializeField] private Script_InteractableObjectText elevatorGoodEndingDisabledText;

    [SerializeField] private Script_ElevatorManager elevatorManager;

    private bool isInit = true;

    public State Behavior
    {
        get => _behavior;
        set => _behavior = value;
    }

    protected override void OnEnable()
    {
        // change exit behavior depending on state
        // state will be modified by the last elevator
        switch (Behavior)
        {
            case (State.Save):
                PlayerDefaultState();
                // Give Bgm control to ElevatorManager when coming from Last Elevator.
                PauseBgmForElevator();
                exitToLobby.Type = Script_Exits.ExitType.SaveAndRestart;
                elevator.State = Script_InteractableObject.States.Disabled;
                break;
            case (State.SaveAndStartWeekendCycle):
                PlayerDefaultState();
                // Give Bgm control to ElevatorManager when coming from Last Elevator.
                PauseBgmForElevator();
                exitToLobby.Type = Script_Exits.ExitType.SaveAndStartWeekendCycle;
                elevator.State = Script_InteractableObject.States.Disabled;
                break;
            default:
                exitToLobby.Type = Script_Exits.ExitType.Default;
                HandleElevatorStateDefault();
                break;
        }

        HandleElevatorDisabledState(elevator);

        PauseBgmForElevator();
        
        Script_Game.IsRunningDisabled = true;

        void PauseBgmForElevator()
        {
            Dev_Logger.Debug($"PauseBgmForElevator elevatorManager.IsBgmOn {elevatorManager.IsBgmOn}");

            // Only stop Bgm if the elevator manager hasn't already restarted it.
            // This happens on same frame but after Bgm Start on InitLevel.
            if (!elevatorManager.IsBgmOn)
            {
                Script_BackgroundMusicManager.Control.Stop();
            }
        }
    }

    protected override void OnDisable()
    {
        // after exiting, we'll always come back to a default Elevator Bay v1
        exitToLobby.Type = Script_Exits.ExitType.Default;
        Script_Game.IsRunningDisabled = false;
    }

    private void HandleElevatorStateDefault()
    {
        var isSunday = game.IsRunDay(Script_Run.DayId.sun);
        var isGoodEnding = game.ActiveEnding == Script_TransitionManager.Endings.Good;
        
        if (isSunday || isGoodEnding)
            elevator.State = Script_InteractableObject.States.Disabled;
        else
            elevator.State = Script_InteractableObject.States.Active;
    }

    private void HandleElevatorDisabledState(Script_Elevator elevator)
    {
        if (elevator.State == Script_InteractableObject.States.Disabled)
        {
            bool isSunday = game.IsRunDay(Script_Run.DayId.sun);
            bool isGoodEnding = game.ActiveEnding == Script_TransitionManager.Endings.Good;
            
            if (isSunday)
            {
                elevatorDisabledText.gameObject.SetActive(false);
                elevatorSundayDisabledText.gameObject.SetActive(true);
                elevatorGoodEndingDisabledText.gameObject.SetActive(false);
            }
            else if (isGoodEnding)
            {
                elevatorDisabledText.gameObject.SetActive(false);
                elevatorSundayDisabledText.gameObject.SetActive(false);
                elevatorGoodEndingDisabledText.gameObject.SetActive(true);
            }
            else
            {
                elevatorDisabledText.gameObject.SetActive(true);
                elevatorSundayDisabledText.gameObject.SetActive(false);
                elevatorGoodEndingDisabledText.gameObject.SetActive(false);
            }
        }
        else
        {
            elevatorDisabledText.gameObject.SetActive(false);
        }
    }

    private void PlayerDefaultState()
    {
        game.GetPlayer().DefaultStickerState();
    }

    private void PauseBgmForElevator()
    {
        // Only stop Bgm if the elevator manager hasn't already restarted it.
        // This happens on same frame but after Bgm Start on InitLevel.
        if (!elevatorManager.IsBgmOn)
            Script_BackgroundMusicManager.Control.Stop();
    }
    
    public override void Setup()
    {
        game.SetupInteractableObjectsExit(exitParent, isInit);

        isInit = false;
    }        
}