using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Script_Speaker : MonoBehaviour
{
    public void Pause()
    {
        AudioSource audio = GetComponent<AudioSource>();
        float lastVol = audio.volume;
        audio.volume = 0f; // to avoid any ripping noise
        audio.Pause();
        audio.volume = lastVol;
    }

    public void UnPause()
    {
        if (GetComponent<AudioSource>() != null)
            GetComponent<AudioSource>().UnPause();
    }
}
