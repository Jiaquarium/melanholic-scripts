using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cleared when the days cycle is over.
/// </summary>
public class Script_EventCycleManager : MonoBehaviour
{
    public static Script_EventCycleManager Control;
    
    [SerializeField] private bool didTalkToIds;

    [SerializeField] private Script_Game game;
    [SerializeField] private Script_RunsManager runsManager;

    public bool DidTalkToIds
    {
        get => didTalkToIds;
        set => didTalkToIds = value;
    }

    // ------------------------------------------------------------------
    // Weekday Cycle Events
    public bool IsIdsWoodsIntroDay()
    {
        return (
            runsManager.RunCycle == Script_RunsManager.Cycle.Weekday
            && (game.IsRunDay(Script_Run.DayId.mon) || game.IsRunDay(Script_Run.DayId.wed))
            && game.lastLevelBehavior == game.bayV2Behavior
        );
    }
    
    // ------------------------------------------------------------------
    // Weekend Cycle Events
    public bool IsIdsSick()
    {
        return (
            (game.IsRunDay(Script_Run.DayId.fri) || game.IsRunDay(Script_Run.DayId.sat))
            && !didTalkToIds
        );
    }

    public bool IsIdsDead()
    {
        return game.IsRunDay(Script_Run.DayId.sat) && !didTalkToIds;
    }

    public void InitialState()
    {
        didTalkToIds = false;
    }

    public void Setup()
    {
        if (Control == null)
        {
            Control = this;
        }
        else if (Control != this)
        {
            Destroy(this.gameObject);
        }   
    }
}
