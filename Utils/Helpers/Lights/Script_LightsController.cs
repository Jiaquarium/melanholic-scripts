using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Script_LightsController : MonoBehaviour
{
    public enum RenderMode
    {
        None,
        Important
    }
    [Tooltip("Updates lights in Update() instead of just in the editor")]
    [SerializeField] private bool shouldUpdate;
    [SerializeField] private float intensity;
    [SerializeField] private float maxIntensity;
    [SerializeField] private float minIntensity;
    [SerializeField] private float range;
    [SerializeField] private Light[] lights;
    [SerializeField] private RenderMode mode;
    private Coroutine fadeOutCoroutine;
    private Coroutine fadeInCoroutine;

    public bool ShouldUpdate
    {
        get => shouldUpdate;
        set => shouldUpdate = value;
    }
    
    public float Intensity
    {
        get => intensity;
        set => intensity = value;
    }

    void OnDisable()
    {
        ForceFinishLightFadeOut();   
    }

    void OnValidate()
    {
        lights = transform.GetComponentsInChildren<Light>(true);
        UpdateLights();
    }

    void Update()
    {
        if (shouldUpdate)
            UpdateLights();
    }

    private void UpdateLights()
    {
        foreach (Light l in lights)
        {
            l.intensity = intensity;
            l.range = range;
            switch (mode)
            {
                case RenderMode.Important:
                    l.renderMode = LightRenderMode.ForcePixel;
                    break;
                default:
                    break;
            }
        }
    }

    private void ForceFinishLightFadeOut()
    {
        if (fadeOutCoroutine != null)
        {
            Debug.Log("Finishing light fade out, since coroutine didn't finish on disable");
            intensity = minIntensity;
        }
        if (fadeInCoroutine != null)
        {
            Debug.Log("Finishing light fade in, since coroutine didn't finish on disable");
            intensity = maxIntensity;
        }

        UpdateLights();
    }

    public void FadeOut(float t, Action cb)
    {
        if (t == 0)
        {
            intensity = minIntensity;
            UpdateLights();
            return;
        }
        
        if (fadeInCoroutine != null)
        {
            StopCoroutine(fadeInCoroutine);
            fadeInCoroutine = null;
        }
        
        fadeOutCoroutine = StartCoroutine(FadeOutCo());

        IEnumerator FadeOutCo()
        {
            float newIntensity = intensity;
            float intensityDelta = newIntensity - minIntensity;

            while (intensity > minIntensity)
            {
                newIntensity -= (Time.deltaTime / t) * intensityDelta;

                if (newIntensity < minIntensity)    newIntensity = 0;
                intensity = newIntensity;
                UpdateLights();

                yield return null;
            }

            if (cb != null)     cb();
        }
    }

    public void FadeIn(float t, Action cb)
    {
        if (t == 0)
        {
            intensity = maxIntensity;
            UpdateLights();
            return;
        }
        
        if (fadeInCoroutine != null)
        {
            StopCoroutine(fadeInCoroutine);
            fadeInCoroutine = null;
        }
        
        fadeInCoroutine = StartCoroutine(FadeInCo());

        IEnumerator FadeInCo()
        {
            float newIntensity = intensity;
            float intensityDelta = maxIntensity - newIntensity;

            while (intensity < maxIntensity)
            {
                newIntensity += (Time.deltaTime / t) * intensityDelta;

                if (newIntensity > maxIntensity)    newIntensity = maxIntensity;
                intensity = newIntensity;
                UpdateLights();

                yield return null;
            }

            if (cb != null)     cb();
        }
    }

    void Awake()
    {
        if (maxIntensity == 0) maxIntensity = intensity;
    }
}
