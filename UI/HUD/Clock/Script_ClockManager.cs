using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Manages Clock Visibility and Firing of Times Up Event
/// </summary>
public class Script_ClockManager : MonoBehaviour
{
    public static Script_ClockManager Control;
    public static int TimebarIncrements = 60;
    
    [SerializeField] private Script_HUDManager HUDManager;
    [SerializeField] private Script_Clock clock;
    [SerializeField] private Script_Timebar timebar;
    
    [SerializeField] private Script_Game game;
    
    private bool didFireDoneEvent;
    
    public float ClockTime
    {
        get => clock.CurrentTime;
    }

    public float PercentTimeElapsed
    {
        get => clock.PercentTimeElapsed;
    }
    
    public float TimeLeft
    {
        get => clock.TimeLeft;
    }

    public Script_Clock.States ClockState
    {
        get => clock.State;
    }
    
    public Script_Clock.TimeStates ClockTimeState
    {
        get => clock.TimeState;
    }

    void Update()
    {
        if (clock.State == Script_Clock.States.Done)
        {
            if (!didFireDoneEvent)
                TimesUp();

            return;
        }
        
        if (IsClockRunning())
            clock.State = Script_Clock.States.Active;
        else
            clock.State = Script_Clock.States.Paused;

        HandleTimebar();
    }

    public bool IsClockRunning()
    {
        return !game.IsInHotel()
            && HUDManager.IsClockShowing();
    }

    public void TimesUp()
    {
        Debug.Log("TIMES UP@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");

        clock.CurrentTime = Script_Clock.EndTime;
        
        /// Fire Done Event
        Script_ClockEventsManager.TimesUp();
        didFireDoneEvent = true;
    }

    public void HandleTimebar()
    {
        float timeElapsed = clock.CurrentTime - Script_Clock.StartTime;
        float donePercent = timeElapsed / Script_Clock.TotalTime;
        int doneValue = (int)Mathf.Floor(donePercent * TimebarIncrements);
        
        timebar.TimeElapsed = doneValue;
    }

    public void InitialState()
    {
        Script_Clock.States initialClockState = game.IsInHotel()
            ? Script_Clock.States.Paused
            : Script_Clock.States.Active;
        
        clock.Setup(initialClockState);
    }

    public void FastForwardTime(int sec)
    {
        clock.FastForwardTime(sec);
        HandleTimebar();
    }

    /// <summary>
    /// Meant to be called only once to instantiate and set dependencies/refs
    /// </summary>
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

        InitialState();
    }

    // ------------------------------------------------------------------
    // Dev only
    public void DangerTime()
    {
        clock.CurrentTime = Script_Clock.DangerTime;
    }

    public void WarningTime()
    {
        clock.CurrentTime = Script_Clock.WarningTime;
    }

    public void AwareTime()
    {
        clock.CurrentTime = Script_Clock.AwareTime;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_ClockManager))]
public class Script_ClockManagerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_ClockManager t = (Script_ClockManager)target;
        if (GUILayout.Button("TimesUp()"))
        {
            t.TimesUp();
        }

        if (GUILayout.Button("AwareTime()"))
        {
            t.AwareTime();
        }

        if (GUILayout.Button("WarningTime()"))
        {
            t.WarningTime();
        }
        
        if (GUILayout.Button("DangerTime()"))
        {
            t.DangerTime();
        }
    }
}
#endif