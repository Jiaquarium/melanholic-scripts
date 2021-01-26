using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Script_CanvasGroupController))]
public class Script_Clock : MonoBehaviour
{
    public enum States {
        Active,
        Paused,
        Done
    }
    public const float StartTime        = 18660f; // 5:11:00
    public const float IsCloseTime      = 21660f; // 6:01:00
    public const float OneMinTilTime    = 22200f; // 6:10:00
    public const float EndTime          = 22260f; // 6:11:00 nautical dawn Chicago, IL Jan 1 2021
    public float TimeMultipler          = 4f;
    [SerializeField] private float _currentTime;
    [SerializeField] private States _state;
    [SerializeField] private TextMeshProUGUI display;
    private float blinkTimer;
    public float CurrentTime
    {
        get { return _currentTime; }
        set { _currentTime = Mathf.Clamp(value, StartTime, EndTime); }
    }
    public States State
    {
        get { return _state; }
        set { _state = value; }
    }
    
    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case (States.Active):
            {
                CurrentTime += Time.deltaTime * TimeMultipler;
                if (CurrentTime > EndTime)  CurrentTime = EndTime;
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
        bool isClose                = CurrentTime >= IsCloseTime;
        bool hideColons;
        float MultiplierHalf        = TimeMultipler / 2;
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
                && (int)Mathf.Floor(CurrentTime) % TimeMultipler >= MultiplierHalf;
        }

        string displayTime = (CurrentTime).FormatSecondsClock(isClose, hideColons);
        
        if (isClose)    display.fontStyle = FontStyles.Bold;
        else            display.fontStyle = FontStyles.Normal;
        
        display.text = displayTime;
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