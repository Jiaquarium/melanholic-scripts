using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Item : ScriptableObject
{
    public string id;
    public Sprite focusedSprite;
    public Sprite sprite;
    public bool isDroppable;
    [Tooltip("Items that will be persistent after leaving Kelsingor. Stickers by default are special.")]
    public bool _isSpecial;
    public string localizedName;
    public string altSteamDeckDescriptionId;

    public bool IsSpecial
    {
        get => _isSpecial;
    }

    public string Description
    {
        get => (
            // Currently only English description needs to be shortened
            Script_Game.Lang == Const_Languages.EN
            && Script_Game.IsSteamRunningOnSteamDeck
            && !String.IsNullOrEmpty(altSteamDeckDescriptionId)
                ? Script_UIText.Text[altSteamDeckDescriptionId].GetProp<string>(Script_Game.Lang)
                : Script_UIText.Text[id].GetProp<string>(Script_Game.Lang)
        ) ?? string.Empty;
    }
}
