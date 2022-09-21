using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class Script_AudioMixerFader : MonoBehaviour
{
    static public IEnumerator Fade(
        AudioMixer audioMixer,
        string exposedParam,
        float fadeTime,
        float targetVol,
        Action cb
    )
    {
        float currentTime = 0;
        float currentVol;
        audioMixer.GetFloat(exposedParam, out currentVol);
        currentVol = currentVol.ConvertDecibelToFloat();
        float clampedVol = Mathf.Clamp(targetVol, 0.0001f, 1f);

        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, clampedVol, currentTime / fadeTime);
            
            Dev_Logger.Debug($"New vol: {newVol}");
            audioMixer.SetFloat(exposedParam, newVol.ConvertFloatToDecibel());

            yield return null;
        }

        if (cb != null)     cb();
        yield break;
    }
}
