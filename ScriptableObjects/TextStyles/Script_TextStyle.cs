using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "TextStyle", menuName = "TextStyle")]
public class Script_TextStyle : ScriptableObject
{
    public TMP_FontAsset fontAsset;
    public float fontSize;
    public float characterSpacing;
    public float lineSpacing;
    public float wordSpacing;
}