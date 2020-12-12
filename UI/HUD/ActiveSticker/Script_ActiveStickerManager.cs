using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages ActiveSticker
/// </summary>
public class Script_ActiveStickerManager : MonoBehaviour
{
    public static Script_ActiveStickerManager Control;
    [SerializeField] private Script_Game game;
    [SerializeField] private Script_ActiveSticker activeSticker;
    [SerializeField] private FadeSpeeds fadeSpeed;

    void Update()
    {
        if (
            game.state == Const_States_Game.Interact
            && game.GetPlayer().State == Const_States_Player.Interact
        )
        {
            activeSticker.GetComponent<Script_CanvasGroupController>().FadeIn(fadeSpeed.ToFadeTime(), null);
        }
        else
        {
            activeSticker.GetComponent<Script_CanvasGroupController>().FadeOut(fadeSpeed.ToFadeTime(), null);
        }
    }
    
    public Script_Sticker GetSticker()
    {
        return activeSticker.GetSticker();
    }
    
    public bool AddSticker(Script_Sticker stickerToAdd)
    {
        return activeSticker.AddSticker(stickerToAdd);
    }

    public bool RemoveSticker()
    {
        return activeSticker.RemoveSticker();
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
