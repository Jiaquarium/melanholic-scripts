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
                exitToLobby.Type = Script_Exits.ExitType.SaveAndRestart;
                elevator.State = Script_InteractableObject.States.Disabled;
                break;
            case (State.SaveAndStartWeekendCycle):
                PlayerDefaultState();
                exitToLobby.Type = Script_Exits.ExitType.SaveAndStartWeekendCycle;
                elevator.State = Script_InteractableObject.States.Disabled;
                break;
            default:
                exitToLobby.Type = Script_Exits.ExitType.Default;
                HandleElevatorStateDefault();
                break;
        }

        HandleElevatorDisabledState(elevator);
    }

    protected override void OnDisable()
    {
        // after exiting, we'll always come back to a default Elevator Bay v1
        exitToLobby.Type = Script_Exits.ExitType.Default;
    }

    private void HandleElevatorStateDefault()
    {
        var isSunday = game.IsRunDay(Script_Run.DayId.sun);
        
        if (isSunday)
            elevator.State = Script_InteractableObject.States.Disabled;
        else
            elevator.State = Script_InteractableObject.States.Active;
    }

    private void HandleElevatorDisabledState(Script_Elevator elevator)
    {
        if (elevator.State == Script_InteractableObject.States.Disabled)
        {
            bool isSunday = game.IsRunDay(Script_Run.DayId.sun);
            
            elevatorSundayDisabledText.gameObject.SetActive(isSunday);
            elevatorDisabledText.gameObject.SetActive(!isSunday);
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
    
    public override void Setup()
    {
        game.SetupInteractableObjectsExit(exitParent, isInit);

        isInit = false;
    }        
}