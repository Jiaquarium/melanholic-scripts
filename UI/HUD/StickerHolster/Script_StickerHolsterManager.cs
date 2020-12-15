using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages StickerHolster
/// Also Controls the view that contains Sticker Holster which nests Active Sticker
/// </summary>
public class Script_StickerHolsterManager : MonoBehaviour
{
    public static Script_StickerHolsterManager Control;
    [SerializeField] private Script_Game game;
    [SerializeField] private Script_StickerHolster stickerHolster;
    [SerializeField] private FadeSpeeds fadeSpeed;

    void Update()
    {
        if (
            game.state == Const_States_Game.Interact
            && game.GetPlayer().State == Const_States_Player.Interact
            && !game.IsInHotel()
        )
        {
            stickerHolster.GetComponent<Script_CanvasGroupController>().FadeIn(fadeSpeed.ToFadeTime(), null);
        }
        else
        {
            stickerHolster.GetComponent<Script_CanvasGroupController>().FadeOut(fadeSpeed.ToFadeTime(), null);
        }
    }
    
    public bool AddSticker(Script_Sticker stickerToAdd, int i)
    {
        return stickerHolster.AddStickerInSlot(stickerToAdd, i);
    }

    public bool RemoveSticker(int i)
    {
        return stickerHolster.RemoveStickerInSlot(i);
    }

    public Script_Sticker GetSticker(int i)
    {
        return stickerHolster.GetStickerInSlot(i);
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
