using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Store the run by idx
/// </summary>
public class Script_RunsManager : MonoBehaviour
{
    public enum Cycle
    {
        Weekend = 0,
        Weekday = 1,
    }
    
    [SerializeField] private Script_Run.DayId _startDay;
    [SerializeField] private Script_Run.DayId _weekendStartDay;
    
    [Tooltip("Current run index")] [SerializeField] private int _runIdx;
    [SerializeField] private Cycle _runCycle;
    [SerializeField] private Cycle _nextRunCycle;
    [SerializeField] private Script_Run[] weekdayCycle;
    [SerializeField] private Script_Run[] weekendCycle;
    [SerializeField] private Script_Run[] all;
    
    [Space]

    [SerializeField] private FadeSpeeds fadeSpeed;
    [SerializeField] private Script_CanvasGroupController runsCanvasGroup;
    
    [SerializeField] private Script_Game game;
    [SerializeField] private Script_EventCycleManager eventCycleManager;

    public int RunIdx
    {
        get => _runIdx;      
        private set => _runIdx = value;
    }
    
    public Script_Run Run
    {
        get => all[RunIdx];
    }

    public Cycle RunCycle
    {
        get => _runCycle;
        private set => _runCycle = value;
    }

    private Script_Run.DayId StartDay
    {
        get => _startDay;
    }

    private Script_Run.DayId WeekendStartDay
    {
        get => _weekendStartDay;
    }
    
    void Update()
    {
        HandleRunsCanvas();
    }

    public Script_Run GetRunByIdx(int i)
    {
        return all[i];
    }

    public int GetRunIdxByDayId(Script_Run.DayId dayId)
    {
        int idx = -1;
        
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].dayId == dayId) idx = i;
        }

        return idx;
    }

    public int IncrementRun()
    {
        switch (RunCycle)
        {
            case (Cycle.Weekend):
                RunIdx = IncrementRunIdxInCycle(RunIdx, weekendCycle);
                break;
            default: // (Cycle.Weekday)
                RunIdx = IncrementRunIdxInCycle(RunIdx, weekdayCycle);
                break;
        }

        return RunIdx;
    }

    public void SetRun(Script_Run.DayId dayId)
    {
        RunIdx = GetRunIdxByDayId(dayId);
        RunCycle = GetCycleByRunIds(RunIdx);
    }

    private int IncrementRunIdxInCycle(int runIdx, Script_Run[] cycle)
    {
        int newRunIdx           = runIdx;
        int cycleIdx            = 0;

        // translate to cycle idx
        for (int i = 0; i < cycle.Length; i++)
            if (cycle[i] == all[runIdx])    cycleIdx = i;
        
        cycleIdx++;
        eventCycleManager.EndOfDayJobs();
        
        if (cycleIdx >= cycle.Length)
        {
            ClearEventCycle();
            
            cycleIdx = 0;
        }

        Script_Run newRun = cycle[cycleIdx];

        // translate back to all idx
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i] == newRun)
            {
                newRunIdx = i;
                break;
            }
        }

        return newRunIdx;
    }

    public void StartWeekendCycle()
    {
        RunCycle = Cycle.Weekend;
        RunIdx = GetRunIdxByDayId(WeekendStartDay);
    }

    private void ClearEventCycle()
    {
        eventCycleManager.InitialState();
    }

    public Cycle GetCycleByRunIds(int i)
    {
        Script_Run run = all[i];

        foreach (Script_Run weekdayRun in weekdayCycle)
        {
            if (weekdayRun == run)  return Cycle.Weekday;
        }

        return Cycle.Weekend;
    }

    private void HandleRunsCanvas()
    {
        if (
            game.state == Const_States_Game.Interact
            && game.GetPlayer().State == Const_States_Player.Interact
        )
        {
            runsCanvasGroup.GetComponent<Script_CanvasGroupController>().FadeIn(fadeSpeed.ToFadeTime(), null);
        }
        else
        {
            runsCanvasGroup.GetComponent<Script_CanvasGroupController>().FadeOut(fadeSpeed.ToFadeTime(), null);
        }
    }

    public void Initialize()
    {
        // RunCycle = Cycle.Weekday;
        RunIdx = GetRunIdxByDayId(StartDay);
    }
    
    public void InitialState(int idx, Cycle cycle)
    {
        RunCycle = cycle;
        RunIdx = idx;

        ClearEventCycle();
    }

    public void Load(
        int idx
    )
    {
        Cycle cycle = GetCycleByRunIds(idx);
        
        InitialState(idx, cycle);
    }

    public void Setup()
    {
        
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_RunsManager))]
public class Script_RunsManagerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_RunsManager t = (Script_RunsManager)target;
        if (GUILayout.Button("Exit via Last Elevator Increment"))
        {
            Script_Game game = Script_Game.Game;
            
            Model_Exit exitData = new Model_Exit(
                game.level,
                game.GetPlayer().transform.position,
                game.GetPlayer().FacingDirection
            );
            
            game.ElevatorCloseDoorsCutScene(
                null,
                null,
                Script_Elevator.Types.Last,
                exitData,
                Script_Exits.ExitType.SaveAndRestartOnLevel
            );
        }
        
        if (GUILayout.Button("IncrementRun()"))
        {
            t.IncrementRun();
        }

        if (GUILayout.Button("Print Cycle By Run Idx"))
        {
            Debug.Log($"mon {t.GetCycleByRunIds(0)}");
            Debug.Log($"tue {t.GetCycleByRunIds(1)}");
            Debug.Log($"wed {t.GetCycleByRunIds(2)}");
            Debug.Log($"thu {t.GetCycleByRunIds(3)}");
            Debug.Log($"fri {t.GetCycleByRunIds(4)}");
            Debug.Log($"sat {t.GetCycleByRunIds(5)}");
            Debug.Log($"sun {t.GetCycleByRunIds(6)}");
        }
    }
}
#endif