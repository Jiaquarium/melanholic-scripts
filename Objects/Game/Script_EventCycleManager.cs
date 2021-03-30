using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cleared when the days cycle is over.
/// </summary>
public class Script_EventCycleManager : MonoBehaviour
{
    public static Script_EventCycleManager Control;
    
    // ------------------------------------------------------------------
    // Save State
    [SerializeField] private bool didTalkToIds;
    
    [SerializeField] private int didTalkToElleniaCountdown;
    private const int ElleniaCountdownMax = 2;
    // ------------------------------------------------------------------

    [SerializeField] private Script_Game game;
    [SerializeField] private Script_RunsManager runsManager;

    public bool DidTalkToIds
    {
        get => didTalkToIds;
        set => didTalkToIds = value;
    }

    public int DidTalkToEllenia
    {
        get => didTalkToElleniaCountdown;
        set => didTalkToElleniaCountdown = value;
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

    public void SetElleniaDidTalkCountdownMax()
    {
        didTalkToElleniaCountdown = ElleniaCountdownMax;
    }
    
    // Check if not the same day we talked with Ellenia and is still active count down
    public bool IsElleniaComfortable()
    {
        bool isSameDayTalked    = didTalkToElleniaCountdown == ElleniaCountdownMax;
        bool isTalkedActive     = didTalkToElleniaCountdown > 0;

        return !isSameDayTalked && isTalkedActive;
    }

    public bool IsElleniaHurt()
    {
        return game.IsRunDay(Script_Run.DayId.sat) && didTalkToElleniaCountdown == 0;
    }

    public void EndOfDayJobs()
    {
        didTalkToElleniaCountdown =  Mathf.Max(0, didTalkToElleniaCountdown - 1);
    }
    
    public void InitialState()
    {
        didTalkToIds                        = false;

        didTalkToElleniaCountdown           = 0;
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
