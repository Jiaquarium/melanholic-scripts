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
    
    [SerializeField] private Script_Game game;
    
    private bool isPaused;

    public bool IsPaused
    {
        get => isPaused;
        set => isPaused = value;
    }

    void Update()
    {
        if (isPaused)
            return;
        
        if (
            game.state == Const_States_Game.Interact
            && game.GetPlayer().State == Const_States_Player.Interact
        )
        {
            timeCanvasGroup.FadeIn(fadeSpeed.ToFadeTime(), null);
        }
        else
        {
            timeCanvasGroup.FadeOut(fadeSpeed.ToFadeTime(), null);
        }
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
