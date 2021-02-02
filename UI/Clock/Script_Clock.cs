using System.Collections;
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

    public const float StartTime        = 18660f; // 5:11:00
    public const float AwareTime        = 20460f; // 5:41:00 half game time is passed, 30 min (6 min IRL)
    public const float WarningTime      = 21360f; // 5:56:00 15 min left (3 min IRL)
    public const float DangerTime       = 22200f; // 6:10:00 5 min left (1 min IRL)
    public const float EndTime          = 22260f; // 6:11:00 nautical dawn Chicago, IL Jan 1 2021
    public static float TimeMultiplier  = 5f;
    [SerializeField] private float currentTime;
    [SerializeField] private States state;
    [SerializeField] private TimeStates timeState;
    [SerializeField] private TextMeshProUGUI display;
    private float blinkTimer;
    public float CurrentTime
    {
        get => currentTime;
        set => currentTime = Mathf.Clamp(value, StartTime, EndTime);
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
        switch (State)
        {
            case (States.Active):
            {
                CurrentTime += Time.deltaTime * TimeMultiplier;
                if (CurrentTime > EndTime)  CurrentTime = EndTime;

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

    public void FastForwardTime(int sec)
    {
        CurrentTime += (float)sec;
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