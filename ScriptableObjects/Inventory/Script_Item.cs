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
    public bool isPersistentDrop;
}
