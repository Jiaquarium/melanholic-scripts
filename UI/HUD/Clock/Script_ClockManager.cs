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
    public static readonly int HurtTrigger = Animator.StringToHash("hurt");
    
    [SerializeField] private Script_HUDManager HUDManager;
    [SerializeField] private Script_Clock clock;
    [SerializeField] private Script_Timebar timebar;

    // Delay before clock on Sunday starts blinking. This is to build
    // some suspense, and bc we're using an external clock, we need to
    // standardize when we start so the blink always starts the same visually
    // (e.g. not cutting to a blink right when canvas fades in)
    [SerializeField] private float delayBeforeClockBlinkSunday;
    
    [SerializeField] private Animator timeHUDAnimator;
    
    [SerializeField] private Script_Game game;
    
    private bool didFireDoneEvent;
    private bool didSetSunEndTime;
    
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
        set => clock.State = value;
    }
    
    public Script_Clock.TimeStates ClockTimeState
    {
        get => clock.TimeState;
    }

    public bool DidSetSunEndTime { get => didSetSunEndTime; }

    public float DelayBeforeClockBlinkSunday { get => delayBeforeClockBlinkSunday; }

    void Update()
    {
        if (!DidSetSunEndTime)
            HandleSundayTime();
        
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
            && !game.IsInGrandMirrorRoom()
            && HUDManager.IsClockShowing();
    }

    public void TimesUp()
    {
        Dev_Logger.Debug("TIMES UP@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");

        clock.CurrentTime = Script_Clock.EndTime;
        
        /// Fire Done Event
        Script_ClockEventsManager.TimesUp();
        didFireDoneEvent = true;
    }

    public void HandleTimebar()
    {
        float timeElapsed = clock.CurrentTime - Script_Clock.StartTime;
        float donePercent = timeElapsed / Script_Clock.TotalTime;
        
        // Only move on minute increments.
        int doneValue = (int)Mathf.Floor(donePercent * TimebarIncrements);

        float fillAmount = (float)doneValue / (float)TimebarIncrements;
        
        timebar.Fill = fillAmount;
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

        // Timebar animation to notify of time change
        timeHUDAnimator.SetTrigger(HurtTrigger);
    }

    public void SetFinalRoundGrandMirrorTime()
    {
        clock.CurrentTime = Script_Clock.FinalRoundGrandMirrorTime;    
    }

    public void SetEndTime()
    {
        clock.CurrentTime = Script_Clock.EndTime;
    }

    private void HandleSundayTime()
    {
        if (game.RunCycle == Script_RunsManager.Cycle.Sunday)
        {
            ClockState = Script_Clock.States.Paused;
            SetEndTime();
            didSetSunEndTime = true;
        }
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

    public void R2CursedTime()
    {
        clock.CurrentTime = Script_Clock.R2CursedTime;
    }

    public void R2IdsDeadTime()
    {
        clock.CurrentTime = Script_Clock.R2IdsDeadTime;
    }

    public void FourthTime()
    {
        clock.CurrentTime = Script_Clock.FourthTime;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_ClockManager))]
public class Script_ClockManagerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_ClockManager t = (Script_ClockManager)target;
        if (GUILayout.Button("Times Up"))
        {
            t.TimesUp();
        }

        if (GUILayout.Button("Fourth Time"))
        {
            t.FourthTime();
        }
        
        if (GUILayout.Button("Aware Time"))
        {
            t.AwareTime();
        }

        if (GUILayout.Button("Warning Time"))
        {
            t.WarningTime();
        }
        
        if (GUILayout.Button("Danger Time"))
        {
            t.DangerTime();
        }

        if (GUILayout.Button("R2 Cursed Time"))
        {
            t.R2CursedTime();
        }

        if (GUILayout.Button("R2 Ids Dead Time"))
        {
            t.R2IdsDeadTime();
        }

        if (GUILayout.Button("Fast Forward Time 1 min"))
        {
            t.FastForwardTime(60);
        }
    }
}
#endif