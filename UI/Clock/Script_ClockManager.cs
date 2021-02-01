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
    
    [SerializeField] private Script_Clock clock;
    [SerializeField] private Script_Game game;
    [SerializeField] private FadeSpeeds fadeSpeed;
    private bool didFireDoneEvent;
    
    public float ClockTime
    {
        get => clock.CurrentTime;
    }
    
    public Script_Clock.TimeStates ClockTimeState
    {
        get => clock.TimeState;
    }

    void Update()
    {
        if (clock.State == Script_Clock.States.Done && !didFireDoneEvent)
        {
            TimesUp();
        }
        
        if (
            game.state == Const_States_Game.Interact
            && game.GetPlayer().State == Const_States_Player.Interact
        )
        {
            clock.GetComponent<Script_CanvasGroupController>().FadeIn(fadeSpeed.ToFadeTime(), null);
            if (game.IsInHotel())   clock.State = Script_Clock.States.Paused;
            else                    clock.State = Script_Clock.States.Active;
        }
        else
        {
            clock.GetComponent<Script_CanvasGroupController>().FadeOut(fadeSpeed.ToFadeTime(), null);
        }
    }

    public void TimesUp()
    {
        Debug.Log("TIMES UP@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");

        clock.CurrentTime = Script_Clock.EndTime;
        
        /// Fire Done Event
        Script_ClockEventsManager.TimesUp();
        didFireDoneEvent = true;
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