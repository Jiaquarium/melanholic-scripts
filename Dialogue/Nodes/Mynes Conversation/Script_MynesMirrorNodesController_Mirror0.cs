using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dining room Mirror. (index 0, Mirror #1)
/// </summary>
public class Script_MynesMirrorNodesController_Mirror0 : Script_MynesMirrorNodesController
{
    [SerializeField] private Script_LevelBehavior_27 LastElevatorBehavior; 
    [SerializeField] private Script_LevelBehavior_25 ElleniasRoomBehavior; 
    [SerializeField] private Script_LevelBehavior_10 IdsRoomBehavior; 
    
    public override Script_DialogueNode[] Nodes
    {
        get
        {
            Script_Run currentRun = Script_Game.Game.Run;

            switch (currentRun.dayId)
            {
                case (Script_Run.DayId.mon):
                    return _monNodes.Nodes;

                case (Script_Run.DayId.tue):
                    return _tueNodes.Nodes;

                case (Script_Run.DayId.wed):
                    return _wedNodes.Nodes;

                case (Script_Run.DayId.thu):
                    return _thuNodes.Nodes;

                case (Script_Run.DayId.fri):
                    // check if Player got Psychic Duck from Ids at Last Elevator
                    return LastElevatorBehavior.GotPsychicDuck ? null : _friNodes.Nodes;

                case (Script_Run.DayId.sat):
                    // check if Ellenia password is done
                    return ElleniasRoomBehavior.isPuzzleComplete ? null : _satNodes.Nodes;

                case (Script_Run.DayId.sun):
                    // check if Ids dance is done
                    return IdsRoomBehavior.gotBoarNeedle ? null : _sunNodes.Nodes;
                
                default:
                    return _monNodes.Nodes;
            }
        }
    }
}
