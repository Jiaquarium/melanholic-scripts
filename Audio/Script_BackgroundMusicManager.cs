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
    private Coroutine currentWaiToPlayCoroutine;

    public void Play(int i, bool forcePlay = false)
    {
        var source = GetComponent<AudioSource>();
        
        if (i == -1)
        {
            source.Stop();
            return;
        }
        
        if (
            i == currentClipIndex
            && !forcePlay
            && source.isPlaying
        )
        {
            // continue track is isPlaying
            return;
        }

        source.clip = AudioClips[i];
        source.Play();

        currentClipIndex = i;
    }

    public void PlayFadeIn(
        int i,
        Action cb,
        bool forcePlay = true,
        float fadeTime = Script_AudioEffectsManager.fadeMedTime,
        string outputMixer = Const_AudioMixerParams.ExposedMasterVolume
    )
    {
        // SetVolume(0f, Const_AudioMixerParams.ExposedBGVolume);
        FadeIn(cb, fadeTime, outputMixer);
        
        currentWaiToPlayCoroutine = StartCoroutine(WaitNextFramePlay());
        
        // To avoid ripping sound.
        IEnumerator WaitNextFramePlay()
        {
            yield return null;
            
            Play(i);
        }
    }

    public void Stop()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.volume = 0f;
        source.Stop();
        source.volume = 1f;
    }

    public void Pause()
    {
        AudioSource source = GetComponent<AudioSource>();
        float lastVol = source.volume;
        source.volume = 0f; // to avoid any ripping noise
        source.Pause();
        source.volume = lastVol;
    }

    public void PauseAll()
    {
        Pause();
        PauseBgThemeSpeakers();
    }

    public void UnPause()
    {
        var source = GetComponent<AudioSource>();
        if (source != null)
            source.UnPause();
    }

    public void UnPauseAll()
    {
        UnPause();
        UnPauseBgThemeSpeakers();
    }

    public bool GetIsPlaying()
    {
        return GetComponent<AudioSource>().isPlaying;
    }

    // ------------------------------------------------------------------
    // AudioMixer Helpers
    public void SetVolume(float newVol, string outputMixer = Const_AudioMixerParams.ExposedMasterVolume)
    {
        EndCurrentCoroutines();
        Script_AudioMixerVolume.SetVolume(audioMixer, outputMixer, newVol);
    }
    
    public void FadeOut(
        Action cb,
        float fadeTime = Script_AudioEffectsManager.fadeMedTime,
        string outputMixer = Const_AudioMixerParams.ExposedMasterVolume
    )
    {
        EndCurrentCoroutines();
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
        EndCurrentCoroutines();
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

    private void EndCurrentCoroutines()
    {
        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
            currentFadeCoroutine = null;
        }

        if (currentWaiToPlayCoroutine != null)
        {
            StopCoroutine(currentWaiToPlayCoroutine);
            currentWaiToPlayCoroutine = null;
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

    public void SetDefault(string outputMixer = Const_AudioMixerParams.ExposedMasterVolume)
    {
        EndCurrentCoroutines();
        SetVolume(1f, outputMixer);
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