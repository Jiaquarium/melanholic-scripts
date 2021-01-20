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
    
    [SerializeField] private Script_Run.DayId _startDay = Script_Run.DayId.fri;
    
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
        set => _runCycle = value;
    }

    public Cycle NextRunCycle
    {
        get => _nextRunCycle;
        set => _nextRunCycle = value;
    }

    private Script_Run.DayId StartDay
    {
        get => _startDay;
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

    private int IncrementRunIdxInCycle(int runIdx, Script_Run[] cycle)
    {
        int newRunIdx           = runIdx;
        int cycleIdx            = 0;

        // translate to cycle idx
        for (int i = 0; i < cycle.Length; i++)
            if (cycle[i] == all[runIdx])    cycleIdx = i;
        
        cycleIdx++;
        if (cycleIdx >= cycle.Length)   cycleIdx = 0;
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
        RunCycle = Cycle.Weekend;
        RunIdx = GetRunIdxByDayId(StartDay);
    }
    
    public void InitialState(int idx, Cycle cycle)
    {
        RunCycle = cycle;
        RunIdx = idx;
    }

    public void Load(
        int idx
    )
    {
        /// TBD: MAKE DYNAMIC
        InitialState(idx, Cycle.Weekend);
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
        if (GUILayout.Button("IncrementRun()"))
        {
            t.IncrementRun();
        }
    }
}
#endif