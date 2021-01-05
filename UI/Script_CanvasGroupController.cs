using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Entry point to easily control canvas groups
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Script_CanvasGroupFadeInOut))]
public class Script_CanvasGroupController : MonoBehaviour
{
    private const float DefaultFadeTime = 0.5f;
    private Coroutine fadeOutCoroutine;
    private Coroutine fadeInCoroutine;
    private bool isFadedIn;
    private bool isFadedOut;
    
    public virtual void Open()
    {
        CanvasGroup c = GetComponent<CanvasGroup>();
        c.alpha = 1f;
        c.gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        CanvasGroup c = GetComponent<CanvasGroup>();
        c.alpha = 0f;
        c.gameObject.SetActive(false);
    }

    public virtual void FadeIn(float t = DefaultFadeTime, Action a = null)
    {
        Script_CanvasGroupFadeInOut fader = GetComponent<Script_CanvasGroupFadeInOut>();
        fader.gameObject.SetActive(true);
        if (fader == null)  return;

        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = null;
        }
        if (isFadedIn || fadeInCoroutine != null)     return;

        Debug.Log("Fading in Canvas Group");

        isFadedOut = false;
        fadeInCoroutine = StartCoroutine(fader.FadeInCo(t, () => {
            if (a != null) a();
            isFadedIn = true;
        }));
    }

    /// <summary>
    /// NOTE: will close canvas group afterwards
    /// </summary>
    public virtual void FadeOut(float t = DefaultFadeTime, Action a = null)
    {
        Script_CanvasGroupFadeInOut fader = GetComponent<Script_CanvasGroupFadeInOut>();
        if (fader == null)  return;

        if (fadeInCoroutine != null)
        {
            StopCoroutine(fadeInCoroutine);
            fadeInCoroutine = null;
        }
        if (isFadedOut || fadeOutCoroutine != null)     return;

        Debug.Log("Fading out Canvas Group");

        isFadedIn = false;
        fadeOutCoroutine = StartCoroutine(fader.FadeOutCo(t, () => {
            if (a != null) a();
            isFadedOut = true;
            fader.gameObject.SetActive(false);
        }));
    }

    public virtual void Setup()
    {

    } 
}
