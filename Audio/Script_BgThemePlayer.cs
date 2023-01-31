using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Used to create separate speaker for BG music so we can
/// play a separate track and/or persist audio states.
/// 
/// Default will give Script_Game reference to it once activated
/// Can disable this if we want this just to be a one-off bg music
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Script_BgThemePlayer : Script_Speaker
{
    [Tooltip("Note: this will NOT affect adding this speaker to AudioConfiguration")]
    public bool isUntrackedSource = false;
    
    [Range(0f, 1f)]
    [SerializeField] private float fadeOutTargetVol;
    
    [Range(0f, 1f)]
    [SerializeField] private float fadeInTargetVol = 1f;
    [SerializeField] private FadeSpeeds fadeSpeed;
    [SerializeField] private float fadeInTimeOverride = 0f;
    [SerializeField] private float fadeOutTimeOverride = 0f;

    [SerializeField] private bool isFadeInOnEnable;

    private Coroutine currentFadeCoroutine;    
    private Coroutine currentWaiToPlayCoroutine;
    
    protected override void OnEnable()
    {
        base.OnEnable();

        if (isFadeInOnEnable)
        {
            Source.playOnAwake = false;
            Source.volume = 0f;
            FadeInPlay();
        }

        if (!isUntrackedSource)
        {
            Script_Game.Game.npcBgThemePlayer = this;
        }
    }

    public void SoftStop()
    {
        Source.volume = 0f;
        Source.Stop();
        Source.volume = 1f;
        this.gameObject.SetActive(false);
    }

    public void FadeOutStop(Action cb = null, float fadeTime = 0f)
    {
        float _fadeOutTimeOverride = fadeTime > 0f ? fadeTime : fadeOutTimeOverride;
        
        EndCurrentCoroutines();
        
        currentFadeCoroutine = StartCoroutine(FadeCo(fadeOutTargetVol, () => {
            SoftStop();
            if (cb != null)
                cb();
        }, _fadeOutTimeOverride));
    }

    public void FadeInPlay(Action cb = null, float fadeTime = 0f)
    {
        float _fadeInTimeOverride = fadeTime > 0f ? fadeTime : fadeInTimeOverride;
        
        EndCurrentCoroutines();
        
        // Then we know this is an initialization.
        if (!Source.isPlaying)
        {
            Source.volume = fadeOutTargetVol;
            this.gameObject.SetActive(true);
        }
        
        currentFadeCoroutine = StartCoroutine(FadeCo(fadeInTargetVol, () => {
            if (cb != null)
                cb();
        }, _fadeInTimeOverride));   

        currentWaiToPlayCoroutine = StartCoroutine(WaitNextFramePlay());
        
        // To avoid ripping sound.
        IEnumerator WaitNextFramePlay()
        {
            yield return null;
            Play();
        }
    }

    private IEnumerator FadeCo(float targetVol, Action cb, float fadeTime = 0f)
    {
        float fadeOutTime = fadeTime > 0f
            ? fadeTime
            : Script_AudioEffectsManager.GetFadeTime(fadeSpeed);
        float newVol = Source.volume;
        float volumeDiff = newVol - targetVol;
        
        while (Source.volume != targetVol)
        {
            newVol -= volumeDiff * (Time.deltaTime / fadeOutTime);

            newVol = Mathf.Clamp(newVol, fadeOutTargetVol, fadeInTargetVol);

            Source.volume = newVol;

            yield return null;
        }

        if (cb != null)
            cb();
    }

    public void Play()
    {
        gameObject.SetActive(true);
        Source.Play();
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
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_BgThemePlayer))]
public class Script_BgThemePlayerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_BgThemePlayer t = (Script_BgThemePlayer)target;
        if (GUILayout.Button("Soft Stop"))
        {
            t.SoftStop();
        }
        
        if (GUILayout.Button("Play"))
        {
            t.Play();
        }

        if (GUILayout.Button("Play Fade In"))
        {
            t.FadeInPlay();
        }

        if (GUILayout.Button("Stop Fade Out"))
        {
            t.FadeOutStop();
        }

        if (GUILayout.Button("Pause"))
        {
            t.Pause();
        }
        
        if (GUILayout.Button("Print IsPlaying"))
        {
            Dev_Logger.Debug($"IsPlaying {t.IsPlaying}");
        }
    }
}
#endif