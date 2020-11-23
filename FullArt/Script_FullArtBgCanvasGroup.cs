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

    public void FadeIn(FadeSpeeds fadeSpeed, Action cb, float maxAlpha = 1f)
    {
        float fadeInTime = Script_GraphicsManager.GetFadeTime(fadeSpeed);
        StartCoroutine(
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
        float fadeOutTime = Script_GraphicsManager.GetFadeTime(fadeSpeed);
        StartCoroutine(
            GetComponent<Script_CanvasGroupFadeInOut>()
                .FadeOutCo(fadeOutTime, () => 
                {
                    if (cb != null)     cb();
                }
            )
        );
    }
}
