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
    // Reference a Marker Util to specify simple automatic fading behavior.
    [SerializeField] private Transform target;
    [SerializeField] private bool isCompareX;
    [SerializeField] private bool isCompareZ;
    [Tooltip("True to show when meeting Target, default is to hide when reaching Target")]
    [SerializeField] private bool isShowOnTargetReached;
    
    private const float DefaultFadeTime = 0.5f;
    
    [SerializeField] private float maxAlpha = 1f;
    [SerializeField] private float minAlpha = 0f;

    [SerializeField] private float compareTime = 0.1f;
    [SerializeField] private float timer;

    [SerializeField] private Vector3 playerLoc;
    [SerializeField] private Vector3 targetLoc;
    
    private Coroutine fadeOutCoroutine;
    private Coroutine fadeInCoroutine;

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

    void LateUpdate()
    {
        if (target == null)
            return;
        
        timer -= Time.deltaTime;
        
        if (timer <= 0f)
        {
            timer = compareTime;
            
            if (CheckShouldFadeIn())
                FadeIn();
            else
                FadeOut();
        }
    }
    
    /// <summary>
    /// Note: This gameobject must be active to call this method (to run coroutine).
    /// </summary>
    public virtual void FadeIn(float t = DefaultFadeTime, Action a = null)
    {
        if (fader == null)  return;

        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = null;
        }
        if (fadeInCoroutine != null)     return;

        fadeInCoroutine = StartCoroutine(fader.FadeInCo(() => {
            if (a != null) a();
        }, t, maxAlpha));
    }

    /// <summary>
    /// Note: This gameobject must be active to call this method (to run coroutine).
    /// </summary>
    public virtual void FadeOut(float t = DefaultFadeTime, Action a = null)
    {
        if (fader == null)  return;

        if (fadeInCoroutine != null)
        {
            StopCoroutine(fadeInCoroutine);
            fadeInCoroutine = null;
        }
        if (fadeOutCoroutine != null)     return;

        fadeOutCoroutine = StartCoroutine(fader.FadeOutCo(() => {
            if (a != null) a();
        }, t, minAlpha));
    }

    /// <summary>
    /// Check target vs. player location.
    /// </summary>
    /// <returns>True, to fade in (default); False to fade out</returns>
    private bool CheckShouldFadeIn()
    {
        var player = Script_Game.Game.GetPlayer();

        if (player == null)
            return true;

        playerLoc = player.transform.position;
        targetLoc = target.transform.position;

        bool ZShow = isShowOnTargetReached ? playerLoc.z >= targetLoc.z : playerLoc.z < targetLoc.z;
        bool XShow = isShowOnTargetReached ? playerLoc.x >= targetLoc.x : playerLoc.x < targetLoc.x;

        if (isCompareX && isCompareZ)
            return XShow && ZShow;
        else if (isCompareX)
            return XShow;
        else if (isCompareZ)
            return ZShow;
        // If no comparison is checked, always show.
        else
            return true;
    }

    public void SetVisibility(bool isVisible)
    {
        fader.SetVisibility(isVisible, maxAlpha, minAlpha);
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