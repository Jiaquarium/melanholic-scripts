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
    [SerializeField] private Script_BgThemeSpeakersController bgThemeSpeakersController;

    private int currentClipIndex = -1;
    private Coroutine currentFadeCoroutine;

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

    // ------------------------------------------------------------------
    // AudioMixer Helpers
    public void SetVolume(float newVol, string outputMixer = Const_AudioMixerParams.ExposedMasterVolume)
    {
        EndCurrentCoroutine();
        Script_AudioMixerVolume.SetVolume(audioMixer, outputMixer, newVol);
    }
    
    public void FadeOut(
        Action cb,
        float fadeTime = Script_AudioEffectsManager.fadeMedTime,
        string outputMixer = Const_AudioMixerParams.ExposedMasterVolume
    )
    {
        EndCurrentCoroutine();
        currentFadeCoroutine = StartCoroutine(
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

    public void FadeIn(
        Action cb,
        float fadeTime = Script_AudioEffectsManager.fadeMedTime,
        string outputMixer = Const_AudioMixerParams.ExposedMasterVolume
    )
    {
        EndCurrentCoroutine();
        currentFadeCoroutine = StartCoroutine(
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

    public void FadeOutFast(Action cb = null, string outputMixer = Const_AudioMixerParams.ExposedMasterVolume)
    {
        FadeOut(cb, Script_AudioEffectsManager.fadeFastTime, outputMixer);
    }

    public void FadeInFast(Action cb = null, string outputMixer = Const_AudioMixerParams.ExposedMasterVolume)
    {
        FadeIn(cb, Script_AudioEffectsManager.fadeFastTime, outputMixer);
    }

    public void FadeOutMed(Action cb = null, string outputMixer = Const_AudioMixerParams.ExposedMasterVolume)
    {
        FadeOut(cb, Script_AudioEffectsManager.fadeMedTime, outputMixer);
    }

    public void FadeInMed(Action cb = null, string outputMixer = Const_AudioMixerParams.ExposedMasterVolume)
    {
        FadeIn(cb, Script_AudioEffectsManager.fadeMedTime, outputMixer);
    }

    public void FadeOutSlow(Action cb = null, string outputMixer = Const_AudioMixerParams.ExposedMasterVolume)
    {
        FadeOut(cb, Script_AudioEffectsManager.fadeSlowTime, outputMixer);
    }

    public void FadeInSlow(Action cb = null, string outputMixer = Const_AudioMixerParams.ExposedMasterVolume)
    {
        FadeIn(cb, Script_AudioEffectsManager.fadeSlowTime, outputMixer);
    }

    public void FadeOutXSlow(Action cb = null, string outputMixer = Const_AudioMixerParams.ExposedMasterVolume)
    {
        FadeOut(cb, Script_AudioEffectsManager.fadeXSlowTime, outputMixer);
    }

    public void FadeInXSlow(Action cb = null, string outputMixer = Const_AudioMixerParams.ExposedMasterVolume)
    {
        FadeIn(cb, Script_AudioEffectsManager.fadeXSlowTime, outputMixer);
    }

    private void EndCurrentCoroutine()
    {
        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
            currentFadeCoroutine = null;
        }
    }

    // ------------------------------------------------------------------
    // BgThemePlayer Controller Helpers
    public void PauseBgThemeSpeakers()
    {
        bgThemeSpeakersController.PauseSpeakers();
    }

    public void UnPauseBgThemeSpeakers()
    {
        bgThemeSpeakersController.UnPauseSpeakers();
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
        if (GUILayout.Button("FadeOutFast()"))
        {
            t.FadeOutFast(null);
        }
        if (GUILayout.Button("FadeInFast()"))
        {
            t.FadeInFast(null);
        }
        if (GUILayout.Button("FadeOutMed()"))
        {
            t.FadeOutMed(null);
        }
        if (GUILayout.Button("FadeInMed()"))
        {
            t.FadeInMed(null);
        }
        if (GUILayout.Button("FadeOutSlow()"))
        {
            t.FadeOutSlow(null);
        }
        if (GUILayout.Button("FadeInSlow()"))
        {
            t.FadeInSlow(null);
        }
    }
}
#endif