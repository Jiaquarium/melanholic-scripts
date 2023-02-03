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

    public int WellsWorldTheme;
    public int CelestialGardensTheme;
    public int XXXWorldTheme;

    public float myMaskDoneSongStartTime;
    public float r1AwakeningMynesMirrorStartTime;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Script_BgThemeSpeakersController bgThemeSpeakersController;

    [SerializeField] private Script_Game game;
    [SerializeField] private Script_Exits exitsManager;

    private int currentClipIndex = -1;
    private Coroutine currentFadeCoroutine;
    private Coroutine currentWaiToPlayCoroutine;
    private Coroutine currentExtraCoroutine;

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

    /// <summary>
    /// Used to track the play state, so we can restart Bgm on device output changes.
    /// </summary>
    public bool IsPlayingThisFrame { get; set; }
    
    void LateUpdate()
    {
        IsPlayingThisFrame = IsPlaying;
    }
    
    public void HandleStartLevelBgmNoFade(int bgmIndex, bool isBgmPaused)
    {
        // Set volume back to 1, since it'll be 0 from Fade Out
        SetVolume(1f, Const_AudioMixerParams.ExposedBGVolume);
        
        if (game.npcBgThemePlayer != null && game.GetNPCThemeMusicIsPlaying())
            return;
        
        if (isBgmPaused)
        {
            Dev_Logger.Debug($"Level {game.Levels.levelsData[game.level]} starting with PAUSED Bgm");
            Play(-1);
            return;
        }

        Dev_Logger.Debug($"StartLevelBgmNoFade bgmIndex {bgmIndex}, isBgmPaused {isBgmPaused}");
        
        Play(bgmIndex);
    }

    /// <summary>
    /// Fade in BGM but only if the index has changed and the index isn't silent.
    /// Otherwise, do the default play behavior.
    /// </summary>
    public void HandleStartLevelBgmFade(int bgmIndex, bool isBgmPaused)
    {
        float defaultBgmFadeInTime = Script_AudioEffectsManager.fadeFastTime;
        
        if (game.npcBgThemePlayer != null && game.GetNPCThemeMusicIsPlaying())
        {
            // Set volume back to 1, since it'll be 0 from Fade Out
            SetVolume(1f, Const_AudioMixerParams.ExposedBGVolume);
        }
        // If Bgm Index is silent, a BG Speaker might be present instead, so do default behavior.
        else if (isBgmPaused || bgmIndex == -1)
        {
            Dev_Logger.Debug($"Level {game.Levels.levelsData[game.level]} starting with PAUSED Bgm");
            // Set volume back to 1, since it'll be 0 from Fade Out
            SetVolume(1f, Const_AudioMixerParams.ExposedBGVolume);
            Play(-1);
        }
        // If bgm index changed, then fade in start music
        else if (bgmIndex != CurrentClipIndex)
        {
            FadeInBgmNextFrame();
        }
        else
        {
            // Set volume back to 1, since it'll be 0 from Fade Out
            SetVolume(1f, Const_AudioMixerParams.ExposedBGVolume);
            Play(bgmIndex);
        }

        void FadeInBgmNextFrame()
        {
            // Log warning if fade in time > level transition time
            if (defaultBgmFadeInTime > game.exitsHandler.TotalLevelTransitionTime)
                Debug.LogWarning("The time to fade out and in BGM is greater than total level transition time. Audio might break!");

            // Set Source's clip immediately and update clip index on the same frame as call
            // (to match StartLevelBgmNoFade's behavior)
            HandleLoadClip(bgmIndex, Source);
            CurrentClipIndex = bgmIndex;

            // Note: Do not set volume to 1f when fading in, since we need to
            // have volume start at 0f; otherwise, will hear popping.
            currentFadeCoroutine = StartCoroutine(FadeInPlayBgmNextFrame());
        }

        IEnumerator FadeInPlayBgmNextFrame()
        {
            // Wait a frame to give level behavior a chance to cancel BGM Manager Fading / setting volume to 0f.
            yield return null;

            Script_AudioMixerVolume.SetVolume(audioMixer, Const_AudioMixerParams.ExposedBGVolume, 0f);
            Play(bgmIndex, didLoadClip: true);
            FadeIn(null, defaultBgmFadeInTime, outputMixer: Const_AudioMixerParams.ExposedBGVolume);
        }
    }

    /// <summary>
    /// Fade out music when going to a different Bgm index level (or when new index is -1).
    /// </summary>
    /// <param name="levelToGo">The new upcoming level exiting to</param>
    public void HandleStopLevelBgmFade(int levelToGo, bool isFadeBGMUI = false)
    {
        Dev_Logger.Debug("Handle Fade Out BGM on exit followup");

        int newBgmIndex = game.GetLevelBgmIndex(levelToGo);
        
        // If level going to is the same, do not fade out.
        if (game.BgmIndex == newBgmIndex)
            return;
        else
        {
            FadeOut(null, exitsManager.DefaultLevelFadeOutTime, Const_AudioMixerParams.ExposedBGVolume);
            
            if (isFadeBGMUI)
            {
                Dev_Logger.Debug($"{name} Handle Fade Out ExposedBGMUIVolume on exit followup");
                FadeOutExtra(
                    out currentExtraCoroutine,
                    null,
                    exitsManager.DefaultLevelFadeOutTime,
                    Const_AudioMixerParams.ExposedBGMUIVolume
                );
            }
        }
    }

    public void HandleStopLevelBgmNoFade(int levelToGo)
    {
        Dev_Logger.Debug("Handle Fade Out BGM No Fade");
        
        int newBgmIndex = game.GetLevelBgmIndex(levelToGo);
        
        // If level going to is the same, do not fade out.
        if (game.BgmIndex == newBgmIndex)
            return;
        else
            Stop();
    }
    
    public void Play(
        int i,
        bool forcePlay = false,
        float startTime = 0f,
        bool didLoadClip = false,
        bool forceNewStartTime = false
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
        
        if (startTime > 0f || forceNewStartTime)
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
        string outputMixer = Const_AudioMixerParams.ExposedGameVolume,
        float startTime = 0f,
        bool isForceNewStartTime = false
    )
    {
        FadeIn(cb, fadeTime, outputMixer);
        
        currentWaiToPlayCoroutine = StartCoroutine(WaitNextFramePlay());
        
        // To avoid ripping sound.
        IEnumerator WaitNextFramePlay()
        {
            yield return null;
            
            Play(
                i,
                forcePlay,
                startTime,
                forceNewStartTime: isForceNewStartTime
            );
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
        Dev_Logger.Debug("~~~~ Pausing BGM ~~~~");
        
        float lastVol = Source.volume;
        Source.volume = 0f; // to avoid any ripping noise
        Source.Pause();
        
        EndCurrentCoroutines();
        
        Source.volume = lastVol;
    }

    /// <summary>
    /// Pause the Bgm source in a Level Behavior Setup. Must return BGM back
    /// to default level.
    /// </summary>
    public void PauseBgmOnSetup()
    {
        Pause();
        SetVolume(1f, Const_AudioMixerParams.ExposedBGVolume);
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
    public void SetVolume(float newVol, string outputMixer = Const_AudioMixerParams.ExposedGameVolume)
    {
        EndCurrentCoroutines();
        Script_AudioMixerVolume.SetVolume(audioMixer, outputMixer, newVol);
    }

    public void FadeOut(
        Action cb,
        float fadeTime = Script_AudioEffectsManager.fadeMedTime,
        string outputMixer = Const_AudioMixerParams.ExposedGameVolume,
        float targetVolume = 0f
    )
    {
        EndCurrentCoroutines();
        currentFadeCoroutine = StartCoroutine(
            Script_AudioMixerFader.Fade(
                audioMixer,
                outputMixer,
                fadeTime,
                targetVolume,
                () => {
                    if (cb != null)     cb();
                }
            )
        );
    }

    public void FadeIn(
        Action cb,
        float fadeTime = Script_AudioEffectsManager.fadeMedTime,
        string outputMixer = Const_AudioMixerParams.ExposedGameVolume,
        float targetVolume = 1f
    )
    {
        EndCurrentCoroutines();
        currentFadeCoroutine = StartCoroutine(
            Script_AudioMixerFader.Fade(
                audioMixer,
                outputMixer,
                fadeTime,
                targetVolume,
                () => {
                    if (cb != null)     cb();
                }
            )
        );
    }

    // Identical as above, but you must keep track of coroutines
    public void FadeOutExtra(
        out Coroutine coroutine,
        Action cb,
        float fadeTime = Script_AudioEffectsManager.fadeMedTime,
        string outputMixer = Const_AudioMixerParams.ExposedGameVolume,
        float targetVolume = 0f
    )
    {
        coroutine = StartCoroutine(
            Script_AudioMixerFader.Fade(
                audioMixer,
                outputMixer,
                fadeTime,
                targetVolume,
                () => {
                    if (cb != null)     cb();
                }
            )
        );
    }

    // Identical as above, but you must keep track of coroutines
    public void FadeInExtra(
        out Coroutine coroutine,
        Action cb,
        float fadeTime = Script_AudioEffectsManager.fadeMedTime,
        string outputMixer = Const_AudioMixerParams.ExposedGameVolume,
        float targetVolume = 1f
    )
    {
        coroutine = StartCoroutine(
            Script_AudioMixerFader.Fade(
                audioMixer,
                outputMixer,
                fadeTime,
                targetVolume,
                () => {
                    if (cb != null)     cb();
                }
            )
        );
    }

    public void StopExtraCoroutine(ref Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    public void FadeOutXFast(Action cb = null, string outputMixer = Const_AudioMixerParams.ExposedGameVolume)
    {
        FadeOut(cb, Script_AudioEffectsManager.fadeXFastTime, outputMixer);
    }

    public void FadeInXFast(Action cb = null, string outputMixer = Const_AudioMixerParams.ExposedGameVolume)
    {
        FadeIn(cb, Script_AudioEffectsManager.fadeXFastTime, outputMixer);
    }
    
    public void FadeOutFast(Action cb = null, string outputMixer = Const_AudioMixerParams.ExposedGameVolume)
    {
        FadeOut(cb, Script_AudioEffectsManager.fadeFastTime, outputMixer);
    }

    public void FadeInFast(Action cb = null, string outputMixer = Const_AudioMixerParams.ExposedGameVolume)
    {
        FadeIn(cb, Script_AudioEffectsManager.fadeFastTime, outputMixer);
    }

    public void FadeOutMed(Action cb = null, string outputMixer = Const_AudioMixerParams.ExposedGameVolume)
    {
        FadeOut(cb, Script_AudioEffectsManager.fadeMedTime, outputMixer);
    }

    public void FadeInMed(Action cb = null, string outputMixer = Const_AudioMixerParams.ExposedGameVolume)
    {
        FadeIn(cb, Script_AudioEffectsManager.fadeMedTime, outputMixer);
    }

    public void FadeOutSlow(Action cb = null, string outputMixer = Const_AudioMixerParams.ExposedGameVolume)
    {
        FadeOut(cb, Script_AudioEffectsManager.fadeSlowTime, outputMixer);
    }

    public void FadeInSlow(Action cb = null, string outputMixer = Const_AudioMixerParams.ExposedGameVolume)
    {
        FadeIn(cb, Script_AudioEffectsManager.fadeSlowTime, outputMixer);
    }

    public void FadeOutXSlow(Action cb = null, string outputMixer = Const_AudioMixerParams.ExposedGameVolume)
    {
        FadeOut(cb, Script_AudioEffectsManager.fadeXSlowTime, outputMixer);
    }

    public void FadeInXSlow(Action cb = null, string outputMixer = Const_AudioMixerParams.ExposedGameVolume)
    {
        FadeIn(cb, Script_AudioEffectsManager.fadeXSlowTime, outputMixer);
    }

    public void FadeOutXXSlow(Action cb = null, string outputMixer = Const_AudioMixerParams.ExposedGameVolume)
    {
        FadeOut(cb, Script_AudioEffectsManager.fadeXXSlowTime, outputMixer);
    }
    
    public void FadeInXXSlow(Action cb = null, string outputMixer = Const_AudioMixerParams.ExposedGameVolume)
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

    public void SetDefault(string outputMixer = Const_AudioMixerParams.ExposedGameVolume)
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

    public void InitialState()
    {
        Dev_Logger.Debug($"{name} initing Audio Mixer volumes");
        
        // Note: do not set ExposedBGVolume since it will be handled by default level fade in behavior and/or LB32. 
        SetVolume(1f, Const_AudioMixerParams.ExposedGameVolume);
        SetVolume(1f, Const_AudioMixerParams.ExposedMusicVolume);
        SetVolume(1f, Const_AudioMixerParams.ExposedBG2Volume);
        
        SetVolume(1f, Const_AudioMixerParams.ExposedFXVolume);
        SetVolume(1f, Const_AudioMixerParams.ExposedSFXVolume);
        
        SetVolume(1f, Const_AudioMixerParams.ExposedInteractionUIVolume);
        SetVolume(1f, Const_AudioMixerParams.ExposedBGMUIVolume);
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

        InitialState();
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

        GUILayout.Space(12);

        if (GUILayout.Button("Fade Out Master XXSlow"))
        {
            t.FadeOutXXSlow(null, Const_AudioMixerParams.ExposedGameVolume);
        }

        if (GUILayout.Button("Fade In Master XXSlow"))
        {
            t.FadeInXXSlow(null, Const_AudioMixerParams.ExposedGameVolume);
        }
    }
}
#endif