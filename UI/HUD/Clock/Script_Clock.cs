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

    public const float StartTime        = 18000f; // 5:00:00
    public const float FourthTime       = StartTime + 900f; // 15 min game time passed
    public const float AwareTime        = StartTime + 1800f; // 30 min game time passed, 30 min left (6 min IRL)
    public const float WarningTime      = StartTime + 2700f; // 45 min game time passed, 15 min left (3 min IRL)
    public const float DangerTime       = StartTime + 3300f; // 55 min game time passed, 5 min left (1 min IRL)
    public const float EndTime          = StartTime + 3600f; // 60 min game time passed, "Nautical Dawn" Chicago Jan 1 is 6:11am, roughly 6:00am
    public static float TimeMultiplier  = 5f;
    public static float TotalTime       = EndTime - StartTime;

    public const float R2CursedTime     = StartTime + 1800f; // 5:30
    public const float R2IdsDeadTime    = StartTime + 2700f; // 5:45

    public const float FinalRoundGrandMirrorTime = EndTime - 5f; // 5 seconds before end time
    
    [SerializeField] private float currentTime;
    
    [SerializeField] private States state;
    [SerializeField] private TimeStates timeState;
    
    [SerializeField] private TextMeshProUGUI display;

    [SerializeField] private Script_ClockManager clockManager;
    [SerializeField] private Script_Game game;
    
    private float sundayClockStartBlinkingTimeStamp;
    private bool isSundayClockStartBlinkingTimeStampSet;
    
    private float blinkTimer; // Dev
    private bool lastHideColons; // Dev
    
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

    public float SundayTimer { get; set; }
    
    // Update is called once per frame
    void Update()
    {
        if (Const_Dev.IsTrailerMode || Debug.isDebugBuild || Const_Dev.IsSpecsDisplayOn || Const_Dev.IsDevMode)
        {
            if (Input.GetButtonDown(Const_KeyCodes.DevIncrement) && Input.GetButton(Const_KeyCodes.Time))
                FastForwardTime(60, true);
            else if (Input.GetButtonDown(Const_KeyCodes.DevDecrement) && Input.GetButton(Const_KeyCodes.Time))
                FastForwardTime(-60, true);

            if (Input.GetKeyDown(KeyCode.P) && Input.GetButton(Const_KeyCodes.Time))
                FastForwardTime(600, true);
            else if (Input.GetKeyDown(KeyCode.O) && Input.GetButton(Const_KeyCodes.Time))
                FastForwardTime(-600, true);
        }
        
        switch (State)
        {
            case (States.Active):
            {
                CurrentTime += Time.deltaTime * TimeMultiplier;
                if (CurrentTime > EndTime)
                    CurrentTime = EndTime;

                UpdateTimeState();
                CheckDone();
                
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

        if (clockManager.DidSetSunEndTime && !isSundayClockStartBlinkingTimeStampSet)
        {
            SundayTimer -= Time.deltaTime;
            
            if (SundayTimer <= 0)
            {
                SundayTimer = 0f;
                
                sundayClockStartBlinkingTimeStamp = Time.time;
                isSundayClockStartBlinkingTimeStampSet = true;
            }
        }

        bool isForceDefaultFormat = clockManager.DidSetSunEndTime || clockManager.DidSetGoodEndingEndTime;
        DisplayTime(isForceDefaultFormat);
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
    
    public void DisplayTime(bool forceDefault = false)
    {
        bool isClose                = CurrentTime >= WarningTime && !forceDefault;
        bool hideColons;

        // On Sunday, show the normal clock format but at 6am, blink every half second.
        if (forceDefault)
        {
            // On Good Ending, freeze colon blinking (as if time is frozen at 6am).
            if (clockManager.DidSetGoodEndingEndTime)
            {
                hideColons = false;
            }
            else
            {
                var isSundayBlinking = SundayTimer <= 0f;
                var normalizedTime = Time.time - sundayClockStartBlinkingTimeStamp;
                hideColons = isSundayBlinking
                    && (int)Mathf.Floor(normalizedTime * 2) % 2 == 0;
            }
        }
        else
        {
            if (isClose)
            {
                // If the current second is odd and active clock, hide colons to show blinking
                // Blinking on every other sec in game time.
                hideColons = State == States.Active
                    && (int)Mathf.Floor(CurrentTime) % 2 == 0;
            }
            else
            {
                // Blink every half second
                hideColons = State == States.Active
                    && (CurrentTime * 2) % (TimeMultiplier * 2) >= TimeMultiplier;
            }
        }

        // ------------------------------------------------------------------
        // For Dev Only
        if (Const_Dev.IsClockDebug)
        {
            if (hideColons != lastHideColons)
            {
                Dev_Logger.Debug($"mod {(CurrentTime * 2) % (TimeMultiplier * 2)} time: {Time.time - blinkTimer}");

                blinkTimer = Time.time;
                lastHideColons = hideColons;
            }
            else
            {
                Dev_Logger.Debug($"mod {(CurrentTime * 2) % (TimeMultiplier * 2)}");
            }
            isClose = true;
        }

        if (Const_Dev.IsClockShowColonsAlways)
            hideColons = false;

        // ------------------------------------------------------------------

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
        // For Dev, without this if booting up game from Grand Mirror,
        // the clock will not be at final round time
        if (
            Debug.isDebugBuild
            && game.levelBehavior == game.grandMirrorRoomBehavior
            && game.grandMirrorRoomBehavior.IsFinalRound
        )
        {
            clockManager.SetFinalRoundGrandMirrorTime();
            return;
        }
            
        CurrentTime = StartTime;
    }

    public void Setup(States _State)
    {
        InitialState();
        State = _State;
        DisplayTime();
    }
}