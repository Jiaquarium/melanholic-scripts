using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class Script_AudioSourceFader : MonoBehaviour
{
    public AudioSource Source { get => audioSource; }

    private AudioSource audioSource;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void FadeOut(float fadeTime, Action cb = null)
    {
        StartCoroutine(FadeOutCoroutine(fadeTime, cb));
    }
    
    private IEnumerator FadeOutCoroutine(float fadeTime, Action cb)
    {
        float timer = 0;
        float currentVol = Source.volume;
        
        while (timer < fadeTime)
        {
            timer += Time.unscaledDeltaTime;
            float newVol = Mathf.Lerp(currentVol, 0f, timer / fadeTime);
            Source.volume = newVol;

            yield return null;
        }

        Source.Stop();
        Source.volume = 1f;

        if (cb != null)
            cb();
    }
}
