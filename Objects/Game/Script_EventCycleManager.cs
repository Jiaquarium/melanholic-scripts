using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cleared when the days cycle is over.
/// 
/// NOTE: Should NOT use plain DidTalk___ for anything besides Save Load.
/// Use the "Weekend Cycle Event Conditions" below for Getting and
/// DidTalk___Today and available Setters for Setting.
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

    private bool didTalkToIdsToday = false;

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

    /// <summary>
    /// To notify at the end of day to track didTalkToIds = true.
    /// Only do at day end in case didTalk state changes midday.
    /// </summary>
    public bool DidTalkToIdsToday
    {
        get => didTalkToIdsToday;
        set => didTalkToIdsToday = value;
    }

    // ------------------------------------------------------------------
    // Setters
    /// <summary>
    /// Counts the current day as well, so for 1 day after first Talked to still be Talkabale, set to 2.
    /// </summary>
    public void SetElleniaDidTalkCountdownMax()
    {
        didTalkToElleniaCountdown = ElleniaCountdownMax;
    }

    // ------------------------------------------------------------------
    // Weekday Cycle Events Conditions
    public bool IsIdsWoodsIntroDay()
    {
        return (
            runsManager.RunCycle == Script_RunsManager.Cycle.Weekday
            && (game.IsRunDay(Script_Run.DayId.mon) || game.IsRunDay(Script_Run.DayId.wed))
            && game.lastLevelBehavior == game.bayV2Behavior
        );
    }

    public bool IsIdsGivePsychicDuckDay()
    {
        return (
            runsManager.RunCycle == Script_RunsManager.Cycle.Weekday
            && game.IsRunDay(Script_Run.DayId.mon)
        );
    }
    
    // ------------------------------------------------------------------
    // Weekend Cycle Event Conditions
    public bool IsIdsInSanctuary()
    {
        return game.IsRunDay(Script_Run.DayId.fri) && !didTalkToIds;
    }

    public bool IsIdsDead()
    {
        return game.IsRunDay(Script_Run.DayId.sat) && !didTalkToIds;
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
        HandleTalkedToIds();

        void HandleTalkedToIds()
        {
            if (didTalkToIdsToday)  didTalkToIds = true;

            didTalkToIdsToday = false;
        }
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
