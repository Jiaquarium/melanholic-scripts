using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Script_HintManager : MonoBehaviour
{
    public static Script_HintManager Control;
    
    public Script_CanvasGroupFadeInOut hintCanvasGroup;
    public TextMeshProUGUI hintCanvasText;

    private void Awake()
    {
        if (Control == null)
        {
            Control = this;
        }
        else if (Control != this)
        {
            Destroy(this.gameObject);
        }

        Script_Hint[] allHints = hintCanvasGroup.GetComponentsInChildren<Script_Hint>();
        foreach (Script_Hint h in allHints)     h.gameObject.SetActive(false);
    }

    /// <summary>
    /// Used to show predefined hint canvases
    /// </summary>
    public void FadeIn(Script_Hint hint, Action cb = null)
    {
        hintCanvasGroup.Initialize();
        hint.gameObject.SetActive(true);

        hintCanvasGroup.gameObject.SetActive(true);
        
        float fadeTime = Script_GraphicsManager.GetFadeTime(hint.fadeSpeed);
        StartCoroutine(hintCanvasGroup.FadeInCo(fadeTime, cb));
    }

    public void FadeOut(Script_Hint hint, Action cb = null)
    {
        float fadeTime = Script_GraphicsManager.GetFadeTime(hint.fadeSpeed);
        StartCoroutine(hintCanvasGroup.FadeOutCo(fadeTime, OnFadedOut));

        void OnFadedOut()
        {
            hint.gameObject.SetActive(false);
            
            hintCanvasGroup.gameObject.SetActive(false);
            hintCanvasGroup.Initialize();

            if (cb != null) cb();
        }
    }
    
    /// <summary>
    /// Used to set a simple text hint with the text canvas
    /// </summary>
    public void ShowTextHint(string s)
    {
        hintCanvasText.text = Script_Utils.FormatString(s);
        hintCanvasGroup.gameObject.SetActive(true);
    }

    public void HideTextHint()
    {
        hintCanvasGroup.gameObject.SetActive(false);
        hintCanvasText.text = "";
    }

    public void Setup()
    {
        HideTextHint();
    }
}
