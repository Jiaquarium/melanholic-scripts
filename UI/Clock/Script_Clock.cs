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
    public const float StartTime   = 21060f; // 5:51:00
    public const float IsCloseTime = 21960; // 5:30:00 ; 19795 is 5:29:55
    public const float EndTime     = 22260f; // 6:11:00 ; 20095 is 5:34:55 (nautical dawn Chicago, IL Jan 1 2021)
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
                CurrentTime += Time.deltaTime;
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
        bool isClose = CurrentTime >= IsCloseTime;
        bool hideColons;
        
        // if the current second is odd and active clock, hide colons to show blinking
        if (isClose)
        {
            // blinking every 0.1 sec
            hideColons = State == States.Active && (int)Mathf.Floor(CurrentTime * 10) % 2 == 1;
        }
        else
        {
            // blinking every 0.5 sec
            hideColons = State == States.Active && (int)Mathf.Floor(CurrentTime * 10) % 10 >= 5;
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