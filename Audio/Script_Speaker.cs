using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Script_Speaker : MonoBehaviour
{
    private AudioSource source;
    
    /// <summary>
    /// Used to track the play state, so we can restart the Speaker on device output changes.
    /// </summary>
    public bool IsPlayingThisFrame { get; set; }

    public AudioSource Source
    {
        get
        {
            if (source == null)
                source = GetComponent<AudioSource>();
            
            return source;
        }
    }

    protected virtual void OnEnable()
    {
        Script_AudioConfiguration.Instance.AddSpeaker(this);
    }

    protected virtual void OnDisable()
    {
        Script_AudioConfiguration.Instance.RemoveSpeaker(this);
    }

    void LateUpdate()
    {
        IsPlayingThisFrame = Source.isPlaying;
    }
    
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
