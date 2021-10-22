using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PsychicNodesController : MonoBehaviour
{
    [SerializeField] protected Script_DialogueNodeSet _monNodes;
    [SerializeField] protected Script_DialogueNodeSet _tueNodes;
    [SerializeField] protected Script_DialogueNodeSet _wedNodes;
    [SerializeField] protected Script_DialogueNodeSet _thuNodes;
    [SerializeField] protected Script_DialogueNodeSet _friNodes;
    [SerializeField] protected Script_DialogueNodeSet _satNodes;
    [SerializeField] protected Script_DialogueNodeSet _sunNodes;

    void Awake()
    {
        if (
            _monNodes == null
            || _tueNodes == null
            || _wedNodes == null
            || _thuNodes == null
            || _friNodes == null
            || _satNodes == null
            || _sunNodes == null
        )
        {
            Debug.LogError($"{name} You are missing PsychicNode reference for a day");
        }
    }

    public virtual Script_DialogueNode[] Nodes
    {
        get
        {
            Script_Run currentRun = Script_Game.Game.Run;
            Debug.Log($"Returning Psychic Nodes for {currentRun.dayId}");

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

    public virtual Script_DialogueNode[] SpecialConditionNodes
    {
        get => null;
    }
}
