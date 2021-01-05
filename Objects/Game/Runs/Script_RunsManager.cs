using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_RunsManager : MonoBehaviour
{
    public static Script_RunsManager Control;
    public static readonly int EroIntroRun = -1;
    public static readonly int LightupPaintingsPuzzleRun = -1;
    public static readonly int MelzIntroRun = -1;

    [SerializeField] private FadeSpeeds fadeSpeed;
    [SerializeField] private Script_CanvasGroupController runsCanvasGroup;
    [SerializeField] private Script_Game game;

    void Update()
    {
        if (
            game.state == Const_States_Game.Interact
            && game.GetPlayer().State == Const_States_Player.Interact
        )
        {
            runsCanvasGroup.GetComponent<Script_CanvasGroupController>().FadeIn(fadeSpeed.ToFadeTime(), null);
        }
        else
        {
            runsCanvasGroup.GetComponent<Script_CanvasGroupController>().FadeOut(fadeSpeed.ToFadeTime(), null);
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
    }
}
