using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages ActiveSticker, the View is controlled by StickerHolster
/// </summary>
public class Script_ActiveStickerManager : MonoBehaviour
{
    public static Script_ActiveStickerManager Control;
    [SerializeField] private Script_Game game;
    [SerializeField] private Script_ActiveSticker activeSticker;
    [SerializeField] private FadeSpeeds fadeSpeed;

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
