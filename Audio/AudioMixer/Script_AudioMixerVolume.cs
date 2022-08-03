using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class Script_AudioMixerVolume : MonoBehaviour
{
    static public void SetVolume(AudioMixer audioMixer, string exposedParam, float targetVol)
    {
        float clampedVol = Mathf.Clamp(targetVol, 0.0001f, 1);

        audioMixer.SetFloat(exposedParam, Mathf.Log10(clampedVol) * 20);
    }
}
