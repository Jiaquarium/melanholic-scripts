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

    [SerializeField] private Script_Game game;

    private int currentClipIndex = -1;
    private Coroutine currentFadeCoroutine;
    private Coroutine currentWaiToPlayCoroutine;

    public AudioSource Source => GetComponent<AudioSource>();
    
    public int CurrentClipIndex
    {
        get => currentClipIndex;
        set => currentClipIndex = value;
    }

    public bool IsPlaying
    {
        get => Source?.isPlaying ?? false;
    }

    public void StartLevelBgmNoFade(int bgmIndex, bool isBgmPaused)
    {
        if (game.npcBgThemePlayer != null && game.GetNPCThemeMusicIsPlaying())
            return;
        
        if (isBgmPaused)
        {
            Debug.Log($"Level {game.Levels.levelsData[game.level]} starting with PAUSED Bgm");
            Play(-1);
            return;
        }
        
        Play(bgmIndex);
    }

    /// <summary>
    /// Fade in BGM but only if the index has changed and the index isn't silent.
    /// Otherwise, do the default play behavior.
    /// </summary>
    public void StartLevelBgmFade(int bgmIndex, bool isBgmPaused)
    {
        if (game.npcBgThemePlayer != null && game.GetNPCThemeMusicIsPlaying())
            return;
        
        Debug.Log($"Level {game.Levels.levelsData[game.level]} attempting to play bgmIndex {bgmIndex}");
        
        // If Bgm Index is silent, a BG Speaker might be present instead, so do default behavior.
        if (isBgmPaused || bgmIndex == -1)
        {
            Debug.Log($"Level {game.Levels.levelsData[game.level]} starting with PAUSED Bgm");
            Play(-1);
            return;
        }
        
        // If bgm index changed, then fade in start music
        if (bgmIndex != CurrentClipIndex)
        {
            // Log warning if fade out and fade in time > level transition time
            if (Script_AudioEffectsManager.fadeFastTime > game.exitsHandler.TotalLevelTransitionTime)
                Debug.LogWarning("The time to fade out and in BGM is greater than total level transition time. Audio might break!");

            // Set Source's clip immediately and update clip index on the same frame as call
            // (to match StartLevelBgmNoFade's behavior)
            HandleLoadClip(bgmIndex, Source);
            CurrentClipIndex = bgmIndex;

            currentFadeCoroutine = StartCoroutine(FadeInPlayBgmNextFrame());
        }
        else
        {
            Play(bgmIndex);
        }

        IEnumerator FadeInPlayBgmNextFrame()
        {
            // Wait a frame to give level behavior a chance to cancel BGM Manager Fading / setting volume to 0f.
            yield return null;
            
            SetVolume(0f, Const_AudioMixerParams.ExposedBGVolume);
            Play(bgmIndex, didLoadClip: true);
            FadeInFast(outputMixer: Const_AudioMixerParams.ExposedBGVolume);
        }
    }
    
    public void Play(
        int i,
        bool forcePlay = false,
        float startTime = 0f,
        bool didLoadClip = false
    )
    {
        if (i == -1)
        {
            Stop();
            return;
        }
        
        // Give option to load clip beforehand if calling this
        // in a coroutine.
        if (!didLoadClip)
            HandleLoadClip(i, Source);

        // Continue track
        if (
            i == CurrentClipIndex
            && !forcePlay
            && Source.isPlaying
        )
        {
            return;
        }
        
        if (startTime > 0f)
            Source.time = startTime;
        
        Source.Play();
        CurrentClipIndex = i;
    }

    private void HandleLoadClip(
        int i,
        AudioSource source
    )
    {
        source.clip = AudioClips[i];
    }

    public void PlayFadeIn(
        int i,
        Action cb = null,
        bool forcePlay = true,
        float fadeTime = Script_AudioEffectsManager.fadeMedTime,
        string outputMixer = Const_AudioMixerParams.ExposedMasterVolume,
        float startTime = 0f
    )
    {
        // SetVolume(0f, Const_AudioMixerParams.ExposedBGVolume);
        FadeIn(cb, fadeTime, outputMixer);
        
        currentWaiToPlayCoroutine = StartCoroutine(WaitNextFramePlay());
        
        // To avoid ripping sound.
        IEnumerator WaitNextFramePlay()
        {
            yield return null;
            
            Play(i, forcePlay, startTime);
        }
    }

    public void Stop()
    {
        Source.volume = 0f;
        Source.Stop();

        EndCurrentCoroutines();

        Source.volume = 1f;
    }

    public void Pause()
    {
        Debug.Log("~~~~ Pausing BGM ~~~~");
        
        float lastVol = Source.volume;
        Source.volume = 0f; // to avoid any ripping noise
        Source.Pause();
        
        EndCurrentCoroutines();
        
        Source.volume = lastVol;
    }

    public void PauseAll()
    {
        Pause();
        PauseBgThemeSpeakers();
    }

    public void UnPause()
    {
        if (Source != null)
            Source.UnPause();
    }

    public void UnPauseAll()
    {
        UnPause();
        UnPauseBgThemeSpeakers();
    }

    public bool GetIsPlaying()
    {
        return Source.isPlaying;
    }

    public AudioClip GetClip(int i) => AudioClips[i];

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
                fadeTime,
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

    public void FadeOutXFast(Action cb = null, string outputMixer = Const_AudioMixerParams.ExposedMasterVolume)
    {
        FadeOut(cb, Script_AudioEffectsManager.fadeXFastTime, outputMixer);
    }

    public void FadeInXFast(Action cb = null, string outputMixer = Const_AudioMixerParams.ExposedMasterVolume)
    {
        FadeIn(cb, Script_AudioEffectsManager.fadeXFastTime, outputMixer);
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

    public void FadeOutXXSlow(Action cb = null, string outputMixer = Const_AudioMixerParams.ExposedMasterVolume)
    {
        FadeOut(cb, Script_AudioEffectsManager.fadeXXSlowTime, outputMixer);
    }
    
    public void FadeInXXSlow(Action cb = null, string outputMixer = Const_AudioMixerParams.ExposedMasterVolume)
    {
        FadeIn(cb, Script_AudioEffectsManager.fadeXXSlowTime, outputMixer);
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

    // ------------------------------------------------------------------
    // Bgm Handlers
    public void PlayElderTragedy()
    {
        Play(26, forcePlay: true);
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
        if (GUILayout.Button("Fade Out Fast"))
        {
            t.FadeOutFast(null, Const_AudioMixerParams.ExposedBGVolume);
        }
        if (GUILayout.Button("Fade In Fast"))
        {
            t.FadeInFast(null, Const_AudioMixerParams.ExposedBGVolume);
        }
        if (GUILayout.Button("Fade Out Med"))
        {
            t.FadeOutMed(null, Const_AudioMixerParams.ExposedBGVolume);
        }
        if (GUILayout.Button("Fade In Med"))
        {
            t.FadeInMed(null, Const_AudioMixerParams.ExposedBGVolume);
        }
        if (GUILayout.Button("Fade Out Slow"))
        {
            t.FadeOutSlow(null, Const_AudioMixerParams.ExposedBGVolume);
        }
        if (GUILayout.Button("Fade In Slow"))
        {
            t.FadeInSlow(null, Const_AudioMixerParams.ExposedBGVolume);
        }
        if (GUILayout.Button("Fade In XXSlow"))
        {
            t.FadeInXXSlow(null, Const_AudioMixerParams.ExposedBGVolume);
        }
        if (GUILayout.Button("Set BG Volume 0f"))
        {
            t.SetVolume(0f, Const_AudioMixerParams.ExposedBGVolume);
        }
        if (GUILayout.Button("Stop"))
        {
            t.Stop();
        }
        if (GUILayout.Button("Play"))
        {
            int level = Script_Game.Game.level;
            t.Play(Script_Game.Game.Levels.levelsData[level].bgMusicAudioClipIndex);
        }
    }
}
#endif