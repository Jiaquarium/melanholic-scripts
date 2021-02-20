using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LastElevatorEffect : Script_StickerEffect
{
    [SerializeField] protected Script_ExitMetadataObject exit;
    [SerializeField] private Script_Elevator.Types type;
    [SerializeField] private Script_ElevatorBehavior elevatorExitBehavior;
    
    void Awake()
    {
        exit = Script_Game.Game.LastElevatorExit;
    }
    
    public override void Effect()
    {
        Script_Game.Game.ElevatorCloseDoorsCutScene(exit, elevatorExitBehavior, type);
    }
}
