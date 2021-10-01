using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Manages ActiveSticker, the View is controlled by StickerHolster
/// </summary>
public class Script_ActiveStickerManager : MonoBehaviour
{
    public static Script_ActiveStickerManager Control;
    
    [SerializeField] private Script_StickerHolsterManager stickerHolsterManager;
    
    [SerializeField] private Script_Game game;
    [SerializeField] private Script_ActiveSticker activeSticker;
    [SerializeField] private FadeSpeeds fadeSpeed;
    [SerializeField] private PlayableDirector activeStickerDirector;

    public Script_Sticker ActiveSticker
    {
        get => activeSticker.Sticker;
    }

    public int ActiveSlot
    {
        get
        {
            var stickers = stickerHolsterManager.Stickers;
            
            for (int i = 0; i < stickers.Length; i++)
            {
                if (stickers[i] == activeSticker.Sticker)
                    return i;
            }

            return -1;
        }
    }

    public bool IsActiveSticker(string id)
    {
        return ActiveSticker?.id == id;
    }
    
    public bool AddSticker(Script_Sticker stickerToAdd, int i)
    {
        stickerHolsterManager.HighlightSlot(i, true);

        return activeSticker.AddSticker(stickerToAdd);
    }

    public bool RemoveSticker(int i)
    {
        stickerHolsterManager.HighlightSlot(i, false);
        
        return activeSticker.RemoveSticker();
    }

    public bool RemoveActiveSticker()
    {
        int i = ActiveSlot;
        
        if (i < 0)
            return false;
        
        return RemoveSticker(i);
    }

    public void AnimateActiveStickerSlot()
    {
        activeStickerDirector.Play();
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
