using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Used to create separate speaker for BG music so we can persist
/// if no need to persist, use game.SwitchBgMusic(int) instead
/// 
/// Default will give Script_Game reference to it once activated
/// Can disable this if we want this just to be a one-off bg music
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Script_BgThemePlayer : Script_Speaker
{
    public bool isUntrackedSource = false;
    [SerializeField] private float fadeOutTargetVol;
    [SerializeField] private FadeSpeeds fadeOutSpeed;
    

    void OnEnable()
    {
        if (isUntrackedSource)  return;
        Script_Game.Game.npcBgThemePlayer = this;
    }

    void OnDisable() {
        
    }

    public void SoftStop()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.volume = 0f;
        source.Stop();
        source.volume = 1f;
        this.gameObject.SetActive(false);
    }

    public void FadeOutStop(Action doneCallback = null)
    {
        StartCoroutine(FadeOutCo(() => {
            SoftStop();
            if (doneCallback != null)     doneCallback();
        }));
        
        IEnumerator FadeOutCo(Action cb)
        {
            AudioSource source = GetComponent<AudioSource>();
            float fadeOutTime = Script_AudioEffectsManager.GetFadeTime(fadeOutSpeed);
            float newVol = source.volume;
            
            while (source.volume > fadeOutTargetVol)
            {
                newVol -= Time.deltaTime / fadeOutTime;

                if (newVol < fadeOutTargetVol) newVol = fadeOutTargetVol;

                source.volume = newVol;

                yield return null;
            }

            if (cb != null)     cb();
        }
    }

    public void Play()
    {
        gameObject.SetActive(true);
        GetComponent<AudioSource>().Play();
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
    }
}
#endif