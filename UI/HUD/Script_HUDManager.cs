using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Control the overall HUD CanvasGroup visibility.
/// Handle the Time CanvasGroup visibility.  
/// </summary>
public class Script_HUDManager : MonoBehaviour
{
    public static Script_HUDManager Control;
    
    [SerializeField] private CanvasGroup HUDCanvasGroup;

    [SerializeField] private Script_CanvasGroupController timeCanvasGroup;
    
    [SerializeField] private FadeSpeeds fadeSpeed;
    
    [SerializeField] private Script_ClockManager clockManager;
    [SerializeField] private Script_Game game;
    
    [SerializeField] private bool isPaused;

    public bool IsTimesUp { get; set; }

    public bool IsPaused
    {
        get => isPaused;
        set => isPaused = value;
    }

    void Update()
    {
        // After IsTimesUp, this will be controlled by TimesUp timeline.
        if (isPaused || IsTimesUp)
        {
            return;
        }
        
        if (IsClockShowing() || IsTimesUp)
        {
            timeCanvasGroup.FadeIn(fadeSpeed.ToFadeTime(), null, isUnscaledTime: true);
        }
        else
        {
            timeCanvasGroup.FadeOut(fadeSpeed.ToFadeTime(), null, isUnscaledTime: true);
        }
    }

    public bool IsClockShowing()
    {
        return game.state == Const_States_Game.Interact
            && (
                game.GetPlayer().State == Const_States_Player.Interact
                
                // Also allow time to run during following Effects
                || game.GetPlayer().State == Const_States_Player.Puppeteer
                || game.GetPlayer().State == Const_States_Player.LastElevatorEffect
                || game.GetPlayer().State == Const_States_Player.MelancholyPiano
            )
            // Don't show HUD on Sunday.
            && game.RunCycle != Script_RunsManager.Cycle.Sunday;
    }

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
        
        HUDCanvasGroup.gameObject.SetActive(true);
    }
}
