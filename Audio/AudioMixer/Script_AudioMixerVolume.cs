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
    
    static public bool GetVolume(
        AudioMixer audioMixer,
        string exposedParam,
        out float vol,
        bool isDecibel = false
    )
    {
        bool isParamExist = audioMixer.GetFloat(exposedParam, out vol);
        vol = isDecibel ? vol : vol.ConvertDecibelToFloat();

        return isParamExist;
    }
}
