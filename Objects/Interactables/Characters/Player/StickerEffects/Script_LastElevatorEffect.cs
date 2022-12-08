using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LastElevatorEffect : Script_StickerEffect
{
    [SerializeField] protected Script_ExitMetadataObject exit;
    [SerializeField] private Script_Elevator.Types type;
    
    // Note: deprecated, no longer using behaviors since skipping BayV1
    [SerializeField] private Script_ElevatorBehavior elevatorExitBehavior;
    [SerializeField] private Script_ElevatorBehavior elevatorSaveAndStartWeekendBehavior;
    
    void Awake()
    {
        exit = Script_Game.Game.LastElevatorExit;
    }
    
    public override void Effect()
    {
        Script_Game game = Script_Game.Game;

        game.ChangeStateCutScene();
        
        game.ElevatorCloseDoorsCutScene(exit, elevatorExitBehavior, type);
    }

    protected override void OnEquip()
    {
        player.SetIsLastElevatorEffect();
        
        base.OnEquip();
        OnEquipControllerSynced();

        player.StopMoving();
    }

    protected override void OnUnequip()
    {
        base.OnEquip();
        OnUnequipControllerSynced();

        player.SetIsInteract();
    }

    protected override void OnUnequipSwitch()
    {
        base.OnEquip();

        player.SetIsInteract();
    }
}
