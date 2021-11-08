using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Script_TMProSetFontUnique : MonoBehaviour
{
    [SerializeField] private TMP_FontAsset font;
    
    public TMP_FontAsset Font
    {
        get => font;
    }
    
    void OnValidate()
    {
        var text = GetComponent<TextMeshProUGUI>();
        if (font != null)
            text.font = font;
    }
}
