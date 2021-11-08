using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// This will ignore any Script_TMPProSetFontUnique that are children.
/// </summary>
public class Script_TMProSetFont : MonoBehaviour
{
    [SerializeField] private TMP_FontAsset font;
    
    void OnValidate()
    {
        var texts = GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (var text in texts)
        {
            if (text.GetComponent<Script_TMProSetFontUnique>()?.Font == null)
            {
                if (font != null)
                    text.font = font;
            }
        }
    }
}
