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

    [SerializeField] private Script_HUDManager HUDManager;

    private bool IsHolsterShowing
    {
        get => game.state == Const_States_Game.Interact
            && !game.IsInHotel()
            && (
                game.GetPlayer().State == Const_States_Player.Interact
                || game.GetPlayer().State == Const_States_Player.LastElevatorEffect
            );
    }

    void Update()
    {
        if (HUDManager.IsPaused)
            return;
        
        if (IsHolsterShowing)
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
        if (GetSticker(i) == Script_ActiveStickerManager.Control.ActiveSticker)
        {
            Script_ActiveStickerManager.Control.RemoveSticker(i);
        }
        
        return stickerHolster.RemoveStickerInSlot(i);
    }

    public Script_Sticker GetSticker(int i)
    {
        return stickerHolster.GetStickerInSlot(i);
    }

    public void HighlightSlot(int i, bool isHighlight)
    {
        stickerHolster.HighlightStickerInSlot(i, isHighlight);
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
