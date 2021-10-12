using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(CanvasGroup))]
public class Script_CanvasGroupFadeInOut : MonoBehaviour
{
    public IEnumerator FadeInCo(
        float t,
        Action cb = null,
        float maxAlpha = 1f,
        bool isUnscaledTime = false
    )
    {
        maxAlpha = maxAlpha <= 0f ? 1f : maxAlpha;
        CanvasGroup cg = GetComponent<CanvasGroup>();
        float alpha = cg.alpha;

        if (t <= 0)     cg.alpha = maxAlpha;

        while (cg.alpha < maxAlpha)
        {
            var deltaTime = isUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            
            alpha += (deltaTime / t) * maxAlpha;
            if (alpha > maxAlpha)   alpha = maxAlpha;

            cg.alpha = alpha;

            yield return null;
        }
        if (cb != null)    cb();
    }
    
    public IEnumerator FadeOutCo(
        float t,
        Action cb = null,
        bool isUnscaledTime = false
    )
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        float alpha = cg.alpha;

        if (t <= 0)     cg.alpha = 0f;

        while (cg.alpha > 0f)
        {
            var deltaTime = isUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            
            alpha -= deltaTime / t;
            if (alpha > 1f)   alpha = 1f;

            cg.alpha = alpha;

            yield return null;
        }

        if (cb != null)    cb();
    }

    public void SetAlpha(float alpha)
    {
        GetComponent<CanvasGroup>().alpha = alpha;
    }

    public void Initialize()
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.alpha = 0f;
    }
}
