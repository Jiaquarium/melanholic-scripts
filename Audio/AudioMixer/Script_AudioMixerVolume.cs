using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class Script_AudioMixerVolume : MonoBehaviour
{
    static public void SetVolume(AudioMixer audioMixer, string exposedParam, float targetVol) =>
        audioMixer.SetFloat(exposedParam, targetVol.ConvertFloatToDecibel());

    static public void SetMasterVolume(float targetVol) =>
        AudioListener.volume = targetVol;
}
