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
    [SerializeField] private bool isForceUp;

    [SerializeField] private Canvas timeHUDCanvas;

    private FadeSpeeds defaultFadeSpeed;
    private int timeHUDdefaultSortingOrder;

    public bool IsTimesUp { get; set; }

    public bool IsPaused
    {
        get => isPaused;
        set => isPaused = value;
    }

    public FadeSpeeds FadeSpeed
    {
        get => fadeSpeed;
        set => fadeSpeed = value;
    }

    public bool IsForceUp
    {
        get => isForceUp;
        set => isForceUp = value;
    }

    public int TimeHUDSortingOrder
    {
        get => timeHUDCanvas.sortingOrder;
        set => timeHUDCanvas.sortingOrder = value;
    }
    
    void Update()
    {
        // After IsTimesUp, this will be controlled by TimesUp timeline.
        if (isPaused || IsTimesUp)
        {
            return;
        }
        
        if (IsClockShowing() || IsTimesUp || IsForceUp)
        {
            if (!timeCanvasGroup.gameObject.activeInHierarchy || timeCanvasGroup.MyCanvasGroup.alpha < 1f)
                timeCanvasGroup.FadeIn(FadeSpeed.ToFadeTime(), null, isUnscaledTime: true);
        }
        else
        {
            if (timeCanvasGroup.gameObject.activeInHierarchy)
                timeCanvasGroup.FadeOut(FadeSpeed.ToFadeTime(), null, isUnscaledTime: true);
        }
    }

    /// <summary>
    /// Should clock show depending on Player and Game states
    /// </summary>
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
            && !game.IsHideHUD;
    }

    public void SetFadeSpeedDefault()
    {
        FadeSpeed = defaultFadeSpeed;
    }

    public void SetTimeHUDSortingOrderDefault()
    {
        TimeHUDSortingOrder = timeHUDdefaultSortingOrder;
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
        
        // To avoid HUD showing up on Load. Note, must call Close (to set alpha) or the clock canvas will
        // cut appear in Lobby first initialization.
        timeCanvasGroup.Close();

        defaultFadeSpeed = FadeSpeed;
        timeHUDdefaultSortingOrder = TimeHUDSortingOrder;
    }
}
