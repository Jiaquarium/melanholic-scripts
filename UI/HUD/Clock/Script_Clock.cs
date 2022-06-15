﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Script_CanvasGroupController))]
public class Script_Clock : MonoBehaviour
{
    public enum States
    {
        Active,
        Paused,
        Done
    }

    public enum TimeStates
    {
        None = 0,
        Aware = 1,
        Warning = 2,
        Danger = 3
    }

    public const float StartTime        = 18000f; // 5:00:00
    public const float AwareTime        = StartTime + 1800f; // 30 min game time passed, 30 min left (6 min IRL)
    public const float WarningTime      = StartTime + 2700f; // 45 min game time passed, 15 min left (3 min IRL)
    public const float DangerTime       = StartTime + 3300f; // 55 min game time passed, 5 min left (1 min IRL)
    public const float EndTime          = StartTime + 3600f; // 60 min game time passed, "Nautical Dawn" Chicago Jan 1 is 6:11am, roughly 6:00am
    public static float TimeMultiplier  = 5f;
    public static float TotalTime       = EndTime - StartTime;

    public const float R2CursedTime     = StartTime + 600f; // 10 min game time passed
    public const float R2IdsDeadTime    = StartTime + 2100f; // 35 min game time passed
    
    [SerializeField] private float currentTime;
    
    [SerializeField] private States state;
    [SerializeField] private TimeStates timeState;
    
    [SerializeField] private TextMeshProUGUI display;

    [SerializeField] private Script_ClockManager clockManager;
    
    private float blinkTimer;
    
    public float CurrentTime
    {
        get => currentTime;
        set => currentTime = Mathf.Clamp(value, StartTime, EndTime);
    }

    public float PercentTimeElapsed
    {
        get => (CurrentTime - StartTime) / TotalTime;
    }

    public float TimeLeft
    {
        get => Mathf.Max(EndTime - CurrentTime, 0);
    }
    
    public States State
    {
        get => state;
        set => state = value;
    }

    public TimeStates TimeState
    {
        get => timeState;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Const_Dev.IsTrailerMode)
        {
            if (Input.GetButtonDown(Const_KeyCodes.DevIncrement) && Input.GetButton(Const_KeyCodes.Time))
                FastForwardTime(60, true);
            else if (Input.GetButtonDown(Const_KeyCodes.DevDecrement) && Input.GetButton(Const_KeyCodes.Time))
                FastForwardTime(-60, true);
        }
        
        switch (State)
        {
            case (States.Active):
            {
                CurrentTime += Time.deltaTime * TimeMultiplier;
                if (CurrentTime > EndTime)
                    CurrentTime = EndTime;

                UpdateTimeState();

                break;
            }
            case (States.Paused):
            {
                break;
            }
            case (States.Done):
            {
                break;
            }
        }

        CheckDone();
        DisplayTime();
    }

    public void FastForwardTime(int sec, bool isRoundDownToMinute = false)
    {
        CurrentTime += (float)sec;

        if (isRoundDownToMinute)
            CurrentTime = Script_Utils.RoundSecondsDownToMinute(CurrentTime);

        CheckDone();
    }

    private void CheckDone()
    {
        if (CurrentTime >= EndTime)
        {
            State = States.Done;
        }
    }
    
    private void DisplayTime()
    {
        bool isClose                = CurrentTime >= WarningTime;
        bool hideColons;
        float MultiplierHalf        = TimeMultiplier / 2;
        float MultiplierFourth      = MultiplierHalf / 2;
        float MultiplierEigth       = MultiplierFourth / 2;
        
        // if the current second is odd and active clock, hide colons to show blinking
        if (isClose)
        {
            // blinking every other in game time
            hideColons = State == States.Active
                && (int)Mathf.Floor(CurrentTime) % 2 == 0;
        }
        else
        {
            // blinking every 2 sec in game time, every .5 sec real time
            hideColons = State == States.Active
                && (int)Mathf.Floor(CurrentTime) % TimeMultiplier >= MultiplierHalf;
        }

        string displayTime = (CurrentTime).FormatSecondsClock(isClose, hideColons);
        
        if (isClose)    display.fontStyle = FontStyles.Bold;
        else            display.fontStyle = FontStyles.Normal;
        
        display.text = displayTime;
    }

    private void UpdateTimeState()
    {
        if      (CurrentTime >= DangerTime)         timeState = TimeStates.Danger;
        else if (CurrentTime >= WarningTime)        timeState = TimeStates.Warning;
        else if (CurrentTime >= AwareTime)          timeState = TimeStates.Aware;
        else                                        timeState = TimeStates.None;
    }

    public void InitialState()
    {
        CurrentTime = StartTime;
    }

    public void Setup(States _State)
    {
        InitialState();
        State = _State;
    }
}