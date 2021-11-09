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
    public bool isUntrackedSource = false;
    
    [Range(0f, 1f)]
    [SerializeField] private float fadeOutTargetVol;
    
    [Range(0f, 1f)]
    [SerializeField] private float fadeInTargetVol = 1f;
    [SerializeField] private FadeSpeeds fadeSpeed;

    private Coroutine currentFadeCoroutine;    
    private Coroutine currentWaiToPlayCoroutine;

    private AudioSource source;

    public bool IsPlaying
    {
        get => Source.isPlaying;
    }
    
    private AudioSource Source
    {
        get
        {
            if (source == null)
                source = GetComponent<AudioSource>();
            
            return source;
        }
    }

    void OnEnable()
    {
        if (isUntrackedSource)
            return;
        
        Script_Game.Game.npcBgThemePlayer = this;
    }

    public void SoftStop()
    {
        Source.volume = 0f;
        Source.Stop();
        Source.volume = 1f;
        this.gameObject.SetActive(false);
    }

    public void FadeOutStop(Action cb = null)
    {
        EndCurrentCoroutines();
        
        currentFadeCoroutine = StartCoroutine(FadeCo(fadeOutTargetVol, () => {
            SoftStop();
            if (cb != null)
                cb();
        }));
    }

    public void FadeInPlay(Action cb = null)
    {
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
        }));   

        currentWaiToPlayCoroutine = StartCoroutine(WaitNextFramePlay());
        
        // To avoid ripping sound.
        IEnumerator WaitNextFramePlay()
        {
            yield return null;
            Play();
        }
    }

    private IEnumerator FadeCo(float targetVol, Action cb)
    {
        float fadeOutTime = Script_AudioEffectsManager.GetFadeTime(fadeSpeed);
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
    }
}
#endif