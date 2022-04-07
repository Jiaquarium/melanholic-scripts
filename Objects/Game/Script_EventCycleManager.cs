using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
    [Tooltip("Count of times interacted positively with Ids on Weekend (e.g. Asked to dance, found in Garden)")]
    [SerializeField] private int idsPositiveInteractionCount;
    
    [Tooltip("Tracks if in previous day did interact positively with Ids on Weekend (e.g. Asked to dance, found in Garden)")]
    [SerializeField] private bool didInteractPositivelyWithIds;
    
    [SerializeField] private int didTalkToElleniaCountdown;
    
    // With CountdownMax = 3, you just have to talk with Ellenia any day. 
    private const int ElleniaCountdownMax = 3;
    // ------------------------------------------------------------------

    [SerializeField] private Script_LevelBehavior_27 LastElevator;
    [SerializeField] private Script_Game game;
    [SerializeField] private Script_RunsManager runsManager;

    [SerializeField] private bool didInteractPositivelyWithIdsToday = false;

    public int IdsPositiveInteractionCount
    {
        get => idsPositiveInteractionCount;
        set => idsPositiveInteractionCount = value;
    }
    
    /// <summary>
    /// Should only be accessed by SaveLoad State.
    /// </summary>
    public bool DidInteractPositivelyWithIds
    {
        get => didInteractPositivelyWithIds;
        set => didInteractPositivelyWithIds = value;
    }
    
    public int DidTalkToEllenia
    {
        get => didTalkToElleniaCountdown;
        set => didTalkToElleniaCountdown = value;
    }

    /// <summary>
    /// To notify at the end of day to track didInteractPositivelyWithIds = true.
    /// Only do at day end in case didTalk state changes midday.
    /// </summary>
    public bool DidInteractPositivelyWithIdsToday
    {
        get => didInteractPositivelyWithIdsToday;
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
    public bool IsLastElevatorTutorialRun()
    {
        return (
            runsManager.RunCycle == Script_RunsManager.Cycle.Weekday
            && game.IsRunDay(Script_Run.DayId.mon)
            && !LastElevator.GotPsychicDuck
        );
    }
    
    public bool IsIdsRoomIntroDay()
    {
        return (
            runsManager.RunCycle == Script_RunsManager.Cycle.Weekday
            && game.IsRunDay(Script_Run.DayId.wed)
        );
    }

    public bool IsIdsGivePsychicDuckDay()
    {
        return (
            runsManager.RunCycle == Script_RunsManager.Cycle.Weekday
            && game.IsRunDay(Script_Run.DayId.mon)
        );
    }

    public bool IsIdsHome()
    {
        return (
            runsManager.RunCycle == Script_RunsManager.Cycle.Weekday
            && game.IsRunDay(Script_Run.DayId.wed)
        );
    }
    
    // ------------------------------------------------------------------
    // Weekend Cycle Event Conditions
    
    // If didn't talk to Ids on Day 1 Weekend (Thu), he will be in Sanctuary.
    public bool IsIdsInSanctuary()
    {
        return (game.IsRunDay(Script_Run.DayId.fri) || game.IsRunDay(Script_Run.DayId.sat))
            && !didInteractPositivelyWithIds;
    }

    // If didn't talk to Ids by Day 3 Weekend (Sat), he will be dead (i.e. didn't talk to him on
    // Day 1 and then didn't find him in the Sanctuary Day 2).
    public bool IsIdsDead()
    {
        return game.IsRunDay(Script_Run.DayId.sat) && IdsPositiveInteractionCount == 0;
    }
    
    // Check if not the same day we talked with Ellenia and is still active count down.
    // Must talk with Ellenia on previous day for her to ask about her painting.
    public bool IsElleniaComfortable()
    {
        bool isSameDayTalked    = didTalkToElleniaCountdown == ElleniaCountdownMax;
        bool isTalkedActive     = didTalkToElleniaCountdown > 0;

        return !isSameDayTalked && isTalkedActive;
    }

    // If it's Day 3 and you didn't talk on previous day, then Ellenia will be hurt.
    public bool IsElleniaHurt()
    {
        Debug.Log($"didTalkToElleniaCountdown {didTalkToElleniaCountdown}");
        
        return game.IsRunDay(Script_Run.DayId.sat) && didTalkToElleniaCountdown == 0;
    }

    // ------------------------------------------------------------------
    // Weekend Handlers
    public int InteractPositiveWithIds()
    {
        idsPositiveInteractionCount++;
        didInteractPositivelyWithIdsToday = true;

        return idsPositiveInteractionCount;
    }

    // ------------------------------------------------------------------

    public void EndOfDayJobs()
    {
        didTalkToElleniaCountdown =  Mathf.Max(0, didTalkToElleniaCountdown - 1);
        HandleTalkedToIds();

        void HandleTalkedToIds()
        {
            didInteractPositivelyWithIds = didInteractPositivelyWithIdsToday;
            
            didInteractPositivelyWithIdsToday = false;
        }
    }
    
    public void InitialState()
    {
        didInteractPositivelyWithIds = false;

        idsPositiveInteractionCount = 0;
        didTalkToElleniaCountdown = 0;
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

#if UNITY_EDITOR
[CustomEditor(typeof(Script_EventCycleManager))]
public class Script_EventCycleManagerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_EventCycleManager t = (Script_EventCycleManager)target;
        if (GUILayout.Button("Interact Positive With Ids"))
        {
            t.InteractPositiveWithIds();
        }
    }
}
#endif