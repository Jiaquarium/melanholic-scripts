using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif


[RequireComponent(typeof(Script_MeshFadeInOut))]
public class Script_MeshFadeController : MonoBehaviour
{
    private const float DefaultFadeTime = 0.5f;
    
    [SerializeField] private float maxAlpha = 1f;
    [SerializeField] private float minAlpha = 0f;
    
    private Coroutine fadeOutCoroutine;
    private Coroutine fadeInCoroutine;
    private bool isFadedIn;
    private bool isFadedOut;

    private Script_MeshFadeInOut fader;

    void OnDisable()
    {
        // Handle finishing fade if Player exits before fade completes.
        if (fadeOutCoroutine != null)
        {
            fader.SetVisibility(false, maxAlpha, minAlpha);
            fadeOutCoroutine = null;
        }

        if (fadeInCoroutine != null)
        {
            fader.SetVisibility(true, maxAlpha, minAlpha);
            fadeInCoroutine = null;
        }
    }

    void Awake()
    {
        fader = GetComponent<Script_MeshFadeInOut>();        
    }
    
    public virtual void FadeIn(float t = DefaultFadeTime, Action a = null)
    {
        if (fader == null)  return;

        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = null;
        }
        if (isFadedIn || fadeInCoroutine != null)     return;

        isFadedOut = false;
        fadeInCoroutine = StartCoroutine(fader.FadeInCo(() => {
            if (a != null) a();
            isFadedIn = true;
        }, t, maxAlpha));
    }

    /// <summary>
    /// NOTE: will close canvas group afterwards
    /// </summary>
    public virtual void FadeOut(float t = DefaultFadeTime, Action a = null)
    {
        if (fader == null)  return;

        if (fadeInCoroutine != null)
        {
            StopCoroutine(fadeInCoroutine);
            fadeInCoroutine = null;
        }
        if (isFadedOut || fadeOutCoroutine != null)     return;

        isFadedIn = false;
        fadeOutCoroutine = StartCoroutine(fader.FadeOutCo(() => {
            if (a != null) a();
            isFadedOut = true;
        }, t, minAlpha));
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_MeshFadeController))]
public class Script_MeshFadeControllerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_MeshFadeController t = (Script_MeshFadeController)target;
        if (GUILayout.Button("FadeIn()"))
        {
            t.FadeIn();
        }
        if (GUILayout.Button("FadeOut()"))
        {
            t.FadeOut();
        }
    }
}
#endif