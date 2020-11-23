using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Script_SeasonsTree : MonoBehaviour
{
    public FadeSpeeds fadeSpeed;
    [SerializeField] private Script_SpriteFadeOut spriteFader;
    [SerializeField] private Seasons mySeason;
    
    public void FadeOut(Action cb, float? _fadeTime = null)
    {
        float fadeTime = _fadeTime ?? Script_GraphicsManager.GetFadeTime(fadeSpeed);

        StartCoroutine(spriteFader.FadeOutCo(cb, fadeTime));
    }

    public void FadeIn(Action cb, float? _fadeTime = null)
    {
        float fadeTime = _fadeTime ?? Script_GraphicsManager.GetFadeTime(fadeSpeed);

        StartCoroutine(spriteFader.FadeInCo(cb, fadeTime));
    }

    public void Setup(Seasons season)
    {
        if (mySeason != season)
        {
            spriteFader.SetVisibility(false);
        }
    }
}
