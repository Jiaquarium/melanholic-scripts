﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Entry point to easily control canvas groups
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Script_CanvasGroupFadeInOut))]
public class Script_CanvasGroupController : MonoBehaviour
{
    public const float DefaultFadeTime = 0.5f;
    
    // Can define the first button to select if an Event System needs this.
    public Button firstToSelect;

    [SerializeField] Script_CanvasGroupFadeInterval intervalFader;
    
    [Tooltip("Enable using a maximum alpha")]
    [SerializeField] bool isUseMaxAlpha;
    [SerializeField] float maxAlpha;

    private Coroutine fadeOutCoroutine;
    private Coroutine fadeInCoroutine;

    public bool IsUseMaxAlpha
    {
        get => isUseMaxAlpha;
        set => isUseMaxAlpha = value;
    }

    public float MaxAlpha
    {
        get => maxAlpha;
        set => maxAlpha = value;
    }

    public Coroutine FadeOutCoroutine
    {
        get => fadeOutCoroutine;
        set => fadeOutCoroutine = value;
    }

    public Coroutine FadeInCoroutine
    {
        get => fadeInCoroutine;
        set => fadeInCoroutine = value;
    }

    public bool IsFadingIn
    {
        get => fadeInCoroutine != null;
    }

    public bool IsFadingOut
    {
        get => fadeOutCoroutine != null;
    }

    public bool IsFadeIntervalEnabled
    {
        get => intervalFader != null ? intervalFader.enabled : false;
        set
        {
            if (intervalFader != null)
                intervalFader.enabled = value;
        }
    }

    public CanvasGroup MyCanvasGroup { get => GetComponent<CanvasGroup>(); }

    public Script_Canvas canvasChild
    {
        get => GetComponentInChildren<Script_Canvas>(true);
    }

    public float Alpha
    {
        get => MyCanvasGroup.alpha;
        set => MyCanvasGroup.alpha = value;
    }

    public virtual void Open()
    {
        CanvasGroup c = GetComponent<CanvasGroup>();
        c.alpha = 1f;
        c.gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        CanvasGroup c = GetComponent<CanvasGroup>();
        c.alpha = 0f;
        c.gameObject.SetActive(false);

        StopCoroutines();
    }

    public virtual void FadeIn(
        float t = DefaultFadeTime,
        Action onFadedIn = null,
        bool isUnscaledTime = false,
        bool isForceMaxAlpha = false,
        float fadeToAlpha = 0f
    )
    {
        Script_CanvasGroupFadeInOut fader = GetComponent<Script_CanvasGroupFadeInOut>();
        CanvasGroup c = GetComponent<CanvasGroup>();
        
        if (fader == null)
            return;
        
        fader.gameObject.SetActive(true);

        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = null;
        }
        
        if (fadeInCoroutine != null)
            return;

        fadeInCoroutine = StartCoroutine(fader.FadeInCo(t, () => {
                fadeInCoroutine = null;
                
                if (onFadedIn != null)
                    onFadedIn();
            },
            maxAlpha: (isUseMaxAlpha || isForceMaxAlpha)
                ? (fadeToAlpha > 0f ? fadeToAlpha : maxAlpha)
                : 1f,
            isUnscaledTime: isUnscaledTime
        ));
    }

    /// <summary>
    /// NOTE: will close canvas group afterwards
    /// </summary>
    public virtual void FadeOut(
        float t = DefaultFadeTime,
        Action onFadedOut = null,
        bool isUnscaledTime = false,
        bool isForceCanvasRemainOpen = false
    )
    {
        Script_CanvasGroupFadeInOut fader = GetComponent<Script_CanvasGroupFadeInOut>();
        CanvasGroup c = GetComponent<CanvasGroup>();
        
        if (fader == null)
            return;
        
        if (fadeInCoroutine != null)
        {
            StopCoroutine(fadeInCoroutine);
            fadeInCoroutine = null;
        }
        
        if (fadeOutCoroutine != null)
            return;

        fadeOutCoroutine = StartCoroutine(fader.FadeOutCo(t, () => {
                if (!isForceCanvasRemainOpen)
                    fader.gameObject.SetActive(false);

                fadeOutCoroutine = null;
                
                if (onFadedOut != null)
                    onFadedOut();
            },
            isUnscaledTime: isUnscaledTime
        ));
    }

    // Use this in conjuntion with isManualStart set to true on interval fader
    // to specify when to start intervaling.
    public void StartIntervalFader(bool isFadeIn)
    {
        if (intervalFader == null)
            return;
        
        if (isFadeIn)
            intervalFader.FadeIn();
        else
            intervalFader.FadeOut();
    }

    public virtual void InitialState()
    {
        Script_CanvasGroupFadeInOut fader = GetComponent<Script_CanvasGroupFadeInOut>();
        fader.Initialize();

        StopCoroutines();
    }

    private void StopCoroutines()
    {
        if (fadeInCoroutine != null)
        {
            Dev_Logger.Debug($"Stopping fadeInCoroutine {fadeInCoroutine}");
            
            StopCoroutine(fadeInCoroutine);
            fadeInCoroutine = null;
        }

        if (fadeOutCoroutine != null)
        {
            Dev_Logger.Debug($"Stopping fadeOutCoroutine {fadeOutCoroutine}");
            
            StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = null;
        }
    }

    public virtual void Setup()
    {
        InitialState();
    } 
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_CanvasGroupController))]
public class Script_CanvasGroupControllerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_CanvasGroupController t = (Script_CanvasGroupController)target;
        if (GUILayout.Button("FadeIn()"))
        {
            t.FadeIn();
        }
    }
}
#endif