using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Script_TMProSetFontUniqueStyle : Script_TMProSetFontUnique
{
    [SerializeField] private Script_TextStylesManager textStylesManager;
    [SerializeField] private TextStyle textStyle;
    private Script_TextStyle myTextStyle;

    public Script_TextStylesManager TextStylesManager { get => textStylesManager; }
    
    public override void SetFontAttributes()
    {
        // To prevent editor prefab errors
        if (textStylesManager == null)
            return;
        
        text = GetComponent<TextMeshProUGUI>();

        myTextStyle = textStylesManager.GetTextStyle(textStyle);

        font = myTextStyle.fontAsset;
        text.font = font;
        
        text.fontSize = myTextStyle.fontSize;
        text.characterSpacing = myTextStyle.characterSpacing;
        text.lineSpacing = myTextStyle.lineSpacing;
        text.wordSpacing = myTextStyle.wordSpacing;
    }
}
