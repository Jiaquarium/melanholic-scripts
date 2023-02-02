using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Script_CanvasGroupFadeInOut))]
[RequireComponent(typeof(CanvasGroup))]
public class Script_FullArtBgCanvasGroup : MonoBehaviour
{
    public void InitializeState()
    {
        GetComponent<CanvasGroup>().alpha = 0f;
    }

    public void FadeIn(
        out Coroutine coroutine,
        FadeSpeeds fadeSpeed,
        Action cb,
        float maxAlpha = 1f
    )
    {
        float fadeInTime = Script_Utils.GetFadeTime(fadeSpeed);
        coroutine = StartCoroutine(
            GetComponent<Script_CanvasGroupFadeInOut>()
                .FadeInCo(fadeInTime, () => 
                {
                    if (cb != null)     cb();
                },
                maxAlpha
            )
        );
    }

    public void FadeOut(FadeSpeeds fadeSpeed, Action cb)
    {
        float fadeOutTime = Script_Utils.GetFadeTime(fadeSpeed);
        StartCoroutine(
            GetComponent<Script_CanvasGroupFadeInOut>()
                .FadeOutCo(fadeOutTime, () => 
                {
                    if (cb != null)     cb();
                }
            )
        );
    }

    public void StopMyCoroutineRef(ref Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    public void Initialize()
    {
        GetComponent<Script_CanvasGroupFadeInOut>().Initialize();   
    }
}
