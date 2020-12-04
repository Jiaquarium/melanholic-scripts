using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Entry point to easily control canvas groups
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class Script_CanvasGroupController : MonoBehaviour
{
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

    public virtual void FadeIn(float t, Action a)
    {
        Open();
        Script_CanvasGroupFadeInOut fader = GetComponent<Script_CanvasGroupFadeInOut>();
        if (fader == null)  return;

        StartCoroutine(fader.FadeInCo(t, a));
    }

    /// <summary>
    /// NOTE: will close canvas group afterwards
    /// </summary>
    public virtual void FadeOut(float t, Action a)
    {
        Script_CanvasGroupFadeInOut fader = GetComponent<Script_CanvasGroupFadeInOut>();
        if (fader == null)  return;

        StartCoroutine(fader.FadeOutCo(t, () => {
            if (a != null) a();
            Close();
        }));
    }

    public virtual void Setup()
    {

    } 
}
