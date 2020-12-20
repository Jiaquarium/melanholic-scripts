﻿using System.Collections;
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
    [SerializeField] private Script_Clock clock;
    [SerializeField] private Script_Game game;
    [SerializeField] private FadeSpeeds fadeSpeed;
    private bool didFireDoneEvent;

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

    public void Setup()
    {
        InitialState();
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
    }
}
#endif