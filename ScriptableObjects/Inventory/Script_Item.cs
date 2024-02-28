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

    public bool IsSpecial
    {
        get => _isSpecial;
    }

    public string Description
    {
        get => Script_UIText.Text[id].GetProp<string>(Script_Game.Lang) ?? string.Empty;
    }
}
