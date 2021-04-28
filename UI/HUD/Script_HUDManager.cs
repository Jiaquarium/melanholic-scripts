using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Control the overall HUD CanvasGroup visibility.
/// Handle the Time CanvasGroup visibility.  
/// </summary>
public class Script_HUDManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup HUDCanvasGroup;

    [SerializeField] private Script_CanvasGroupController timeCanvasGroup;
    
    [SerializeField] private FadeSpeeds fadeSpeed;
    
    [SerializeField] private Script_Game game;
    
    void Update()
    {
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
        HUDCanvasGroup.gameObject.SetActive(true);
    }
}
