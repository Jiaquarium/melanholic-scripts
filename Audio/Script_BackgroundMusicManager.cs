using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_BackgroundMusicManager : MonoBehaviour
{
    public static Script_BackgroundMusicManager Control;    
    public AudioSource AudioSource;
    public AudioClip[] AudioClips;

    [SerializeField] private AudioMixer audioMixer;

    private int currentClipIndex = -1;

    public void Play(int i, bool forcePlay = false)
    {
        if (i == -1)
        {
            GetComponent<AudioSource>().Stop();
            return;
        }
        
        if (
            i == currentClipIndex
            && !forcePlay
            && GetComponent<AudioSource>().isPlaying
        )
        {
            // continue track is isPlaying
            return;
        }

        GetComponent<AudioSource>().clip = AudioClips[i];
        GetComponent<AudioSource>().Play();

        currentClipIndex = i;
    }

    public void Stop()
    {
        GetComponent<AudioSource>().Stop();    
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

    public bool GetIsPlaying()
    {
        return GetComponent<AudioSource>().isPlaying;
    }

    public void FadeOutMaster(
        Action cb,
        float fadeTime = Script_AudioEffectsManager.fadeMedTime,
        string outputMixer = Const_AudioMixerParams.ExposedMasterVolume
    )
    {
        StartCoroutine(
            Script_AudioMixerFader.Fade(
                audioMixer,
                outputMixer,
                0.5f,
                0f,
                () => {
                    if (cb != null)     cb();
                }
            )
        );
    }

    public void FadeInMaster(
        Action cb,
        float fadeTime = Script_AudioEffectsManager.fadeMedTime,
        string outputMixer = Const_AudioMixerParams.ExposedMasterVolume
    )
    {
        StartCoroutine(
            Script_AudioMixerFader.Fade(
                audioMixer,
                outputMixer,
                fadeTime,
                1f,
                () => {
                    if (cb != null)     cb();
                }
            )
        );
    }

    public void FadeOutMasterFast(Action cb)
    {
        FadeOutMaster(cb, Script_AudioEffectsManager.fadeFastTime);
    }

    public void FadeInMasterFast(Action cb)
    {
        FadeInMaster(cb, Script_AudioEffectsManager.fadeFastTime);
    }

    public void FadeOutMasterMed(Action cb)
    {
        FadeOutMaster(cb, Script_AudioEffectsManager.fadeMedTime);
    }

    public void FadeInMasterMed(Action cb)
    {
        FadeInMaster(cb, Script_AudioEffectsManager.fadeMedTime);
    }

    public void FadeOutMasterSlow(Action cb)
    {
        FadeOutMaster(cb, Script_AudioEffectsManager.fadeSlowTime);
    }

    public void FadeInMasterSlow(Action cb)
    {
        FadeInMaster(cb, Script_AudioEffectsManager.fadeSlowTime);
    }

    public void Setup()
    {
        if (Control == null)
        {
            Control = this;
        }
        else if (Control != this)
        {
            Destroy(this.gameObject);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_BackgroundMusicManager))]
public class Script_BackgroundMusicManagerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_BackgroundMusicManager t = (Script_BackgroundMusicManager)target;
        if (GUILayout.Button("FadeOutMasterFast()"))
        {
            t.FadeOutMasterFast(null);
        }
        if (GUILayout.Button("FadeInMasterFast()"))
        {
            t.FadeInMasterFast(null);
        }
        if (GUILayout.Button("FadeOutMasterMed()"))
        {
            t.FadeOutMasterMed(null);
        }
        if (GUILayout.Button("FadeInMasterMed()"))
        {
            t.FadeInMasterMed(null);
        }
        if (GUILayout.Button("FadeOutMasterSlow()"))
        {
            t.FadeOutMasterSlow(null);
        }
        if (GUILayout.Button("FadeInMasterSlow()"))
        {
            t.FadeInMasterSlow(null);
        }
    }
}
#endif