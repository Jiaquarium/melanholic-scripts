using System.Collections;
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
    // Can define the first button to select if an Event System needs this.
    public Button firstToSelect;
    
    private const float DefaultFadeTime = 0.5f;
    private Coroutine fadeOutCoroutine;
    private Coroutine fadeInCoroutine;
    private bool isFadedIn;
    private bool isFadedOut;

    public bool IsFadingIn
    {
        get => fadeInCoroutine != null;
    }

    public bool IsFadingOut
    {
        get => fadeOutCoroutine != null;
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
    }

    public virtual void FadeIn(
        float t = DefaultFadeTime,
        Action a = null,
        bool isUnscaledTime = false
    )
    {
        Script_CanvasGroupFadeInOut fader = GetComponent<Script_CanvasGroupFadeInOut>();
        fader.gameObject.SetActive(true);
        if (fader == null)  return;

        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = null;
        }
        if (isFadedIn || fadeInCoroutine != null)     return;

        isFadedOut = false;
        fadeInCoroutine = StartCoroutine(fader.FadeInCo(t, () => {
                if (a != null) a();
                isFadedIn = true;
                fadeInCoroutine = null;
            },
            maxAlpha: 1f,
            isUnscaledTime: isUnscaledTime
        ));
    }

    /// <summary>
    /// NOTE: will close canvas group afterwards
    /// </summary>
    public virtual void FadeOut(
        float t = DefaultFadeTime,
        Action a = null,
        bool isUnscaledTime = false
    )
    {
        Script_CanvasGroupFadeInOut fader = GetComponent<Script_CanvasGroupFadeInOut>();
        if (fader == null)  return;

        if (fadeInCoroutine != null)
        {
            StopCoroutine(fadeInCoroutine);
            fadeInCoroutine = null;
        }
        if (isFadedOut || fadeOutCoroutine != null)     return;

        isFadedIn = false;
        fadeOutCoroutine = StartCoroutine(fader.FadeOutCo(t, () => {
                if (a != null) a();
                isFadedOut = true;
                fader.gameObject.SetActive(false);
                fadeOutCoroutine = null;
            },
            isUnscaledTime: isUnscaledTime
        ));
    }

    public void InitialState()
    {
        Script_CanvasGroupFadeInOut fader = GetComponent<Script_CanvasGroupFadeInOut>();
        fader.Initialize();

        isFadedIn = false;
        isFadedOut = false;
        
        if (fadeInCoroutine != null)
        {
            StopCoroutine(fadeInCoroutine);
            fadeInCoroutine = null;
        }

        if (fadeOutCoroutine != null)
        {
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