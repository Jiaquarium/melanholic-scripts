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
        Default     = 0,
        Save        = 1,
    }

    [SerializeField] private Transform exitParent;
    [SerializeField] private Script_Elevator elevator; /// Ref'ed by ElevatorManager
    [SerializeField] private State _behavior;  // Use an elevator Effect to change this
    [SerializeField] private Script_TileMapExitEntrance exitToLobby;

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
                exitToLobby.Type = Script_Exits.ExitType.SaveAndRestart;
                break;
            default:
                exitToLobby.Type = Script_Exits.ExitType.Default;
                break;
        }
    }

    protected override void OnDisable()
    {
        // after exiting, we'll always come back to a default Elevator Bay v1
        exitToLobby.Type = Script_Exits.ExitType.Default;
    }

    public override void Setup()
    {
        game.SetupInteractableObjectsExit(exitParent, isInit);

        isInit = false;
    }        
}