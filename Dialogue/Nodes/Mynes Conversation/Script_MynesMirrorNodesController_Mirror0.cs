using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dining room Mirror. (index 0, Mirror #1)
/// </summary>
public class Script_MynesMirrorNodesController_Mirror0 : Script_MynesMirrorNodesController
{
    [SerializeField] private Script_LevelBehavior_10 IdsRoomBehavior; 
    [SerializeField] private Script_LevelBehavior_20 BallroomBehavior; 
    [SerializeField] private Script_LevelBehavior_25 ElleniasRoomBehavior; 
    [SerializeField] private Script_LevelBehavior_27 LastElevatorBehavior; 
    
    [Header("Special Conditions")]
    [SerializeField] private Script_DialogueNodeSet afterSealingNodes;
    
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
                    return _friNodes.Nodes;

                case (Script_Run.DayId.sat):
                    return _satNodes.Nodes;

                case (Script_Run.DayId.sun):
                    return _sunNodes.Nodes;
                
                default:
                    return _monNodes.Nodes;
            }
        }
    }

    public override Script_DialogueNode[] SpecialConditionNodes
    {
        get
        {
            // Ballroom Elder's Speech cut scene done.
            if (
                BallroomBehavior.isKingIntroCutSceneDone
                && !Script_MynesMirrorManager.Control.DidSealingDialogue
            )
            {
                Script_MynesMirrorManager.Control.DidSealingDialogue = true;
                return afterSealingNodes.Nodes;
            }
            else
            {
                return null;
            }
        }
    }
}
