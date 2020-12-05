using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages Clock Visibility
/// </summary>
public class Script_ClockManager : MonoBehaviour
{
    [SerializeField] private Script_Clock clock;
    [SerializeField] private Script_Game game;
    [SerializeField] private FadeSpeeds fadeSpeed;
    private Coroutine clockFadeCoroutine;
    private bool didFireDoneEvent;

    void Update()
    {
        if (clock.State == Script_Clock.States.Done && !didFireDoneEvent)
        {
            Debug.Log("TIMES UP@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            
            /// Fire Done Event
            Script_ClockEventsManager.TimesUp();
            didFireDoneEvent = true;
        }
        
        if (
            game.state == Const_States_Game.Interact
            && game.GetPlayer().GetIsInteract()
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

    public void Setup()
    {
        didFireDoneEvent = false;
        Script_Clock.States initialClockState = game.IsInHotel()
            ? Script_Clock.States.Paused
            : Script_Clock.States.Active;
        
        clock.Setup(initialClockState);
    }
}
