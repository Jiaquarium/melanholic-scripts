using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Script_TMProSetFontUnique : MonoBehaviour
{
    [Tooltip("Will be auto-populated if using Text Styles")]
    [SerializeField] protected TMP_FontAsset font;
    
    protected TextMeshProUGUI text;

    public TMP_FontAsset Font
    {
        get => font;
    }

#if UNITY_EDITOR
    // Revert any state changes back to default EN to clean up Editor View
    void OnApplicationQuit()
    {
        Script_Game.ChangeLangToEN();
        SetFontAttributes();
    }   
#endif
    
    // Also call OnEnable to ensure localization data is up to date
    void OnEnable()
    {
        SetFontAttributes();
    }
    
    protected virtual void OnValidate()
    {
        SetFontAttributes();
    }

    public virtual void SetFontAttributes()
    {
        text = GetComponent<TextMeshProUGUI>();
        if (font != null)
            text.font = font;
    }
}
