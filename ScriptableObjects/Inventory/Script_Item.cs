using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Item : ScriptableObject
{
    public string id;
    public Sprite focusedSprite;
    public Sprite sprite;
    [TextArea(1, 3)]
    public string description;
    public bool isDroppable;
    [Tooltip("Items that will be persistent after leaving Kelsingor. Stickers by default are special.")]
    public bool _isSpecial;

    public bool IsSpecial
    {
        get => _isSpecial;
    }

    public string Description
    {
        get => Script_UIText.Text[id].GetProp<string>(Const_Dev.Lang) ?? description;
    }
}
