using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_GlitchFXManager : MonoBehaviour
{
    public static Script_GlitchFXManager Control;

    [Header("Glitch Settings")]
    
    [SerializeField] private GlitchImageEffect.GlitchImageEffectSettings currentSettings;
    [SerializeField] private GlitchImageEffect.GlitchImageEffectSettings defaultSettings;
    [SerializeField] private GlitchImageEffect.GlitchImageEffectSettings highSettings;
    [SerializeField] private GlitchImageEffect.GlitchImageEffectSettings lowSettings;

    [SerializeField] private GlitchImageEffect.GlitchImageEffectSettings UIDayNotificationSettings;
    
    [SerializeField] private GlitchImageEffect glitchFeature;

    private Coroutine blendCoroutine;
    private float timer;

    public float CurrentBlend
    {
        get => currentSettings.blend;
    }

    void OnValidate()
    {
        SaveSettings(glitchFeature.settings, currentSettings);
    }
    
    void OnDisable()
    {
        SetDefault();
    }
    
    public void SetDefault(bool useCurrentBlend = false)
    {
        float lastBlend = currentSettings.blend;
        
        SaveSettings(currentSettings, defaultSettings);

        if (useCurrentBlend)
            SetBlend(lastBlend);
        else
            UpdateGlitchFXState();
    }

    public void SetHigh(bool useCurrentBlend = false)
    {
        float lastBlend = currentSettings.blend;

        SaveSettings(currentSettings, highSettings);
        
        if (useCurrentBlend)
            SetBlend(lastBlend);
        else
            UpdateGlitchFXState();
    }

    public void SetLow(bool useCurrentBlend = false)
    {
        float lastBlend = currentSettings.blend;
        
        SaveSettings(currentSettings, lowSettings);
        
        if (useCurrentBlend)
            SetBlend(lastBlend);
        else
            UpdateGlitchFXState();
    }

    public void SetUIDayNotification(bool useCurrentBlend = false)
    {
        float lastBlend = currentSettings.blend;
        
        SaveSettings(currentSettings, UIDayNotificationSettings);
        
        if (useCurrentBlend)
            SetBlend(lastBlend);
        else
            UpdateGlitchFXState();   
    }

    private void UpdateGlitchFXState()
    {
        SaveSettings(glitchFeature.settings, currentSettings);    
    }
    
    public void BlendTo(float blendValue, float time = 0.5f, Action cb = null)
    {
        if (blendCoroutine != null)
        {
            StopCoroutine(blendCoroutine);
            blendCoroutine = null;
        }

        blendCoroutine = StartCoroutine(BlendCoroutine(blendValue, time, cb));
        
        IEnumerator BlendCoroutine(float newBlendValue, float time, Action cb)
        {
            timer = time;
            float startingBlend = currentSettings.blend;
            float blendDifference = newBlendValue - startingBlend;
            
            while (timer > 0f)
            {
                yield return null;
                
                timer = Mathf.Max(timer - Time.unscaledDeltaTime, 0f);

                float percentDone = 1 - (timer / time);
                float newBlend = startingBlend + (percentDone * blendDifference);
                
                newBlend = Mathf.Clamp(newBlend, 0f, 1f);
                
                currentSettings.blend = newBlend;

                UpdateGlitchFXState();
            }

            if (cb != null)
                cb();
        }
    }

    public void SetBlend(float val)
    {
        currentSettings.blend = Mathf.Clamp(val, 0f, 1f);
        UpdateGlitchFXState();
    }
    
    // Inject the settings either defined on this manager or an override settings object.
    private void SaveSettings(
        GlitchImageEffect.GlitchImageEffectSettings _settings,
        GlitchImageEffect.GlitchImageEffectSettings settingsOverride = null
    )
    {
        _settings.type              = settingsOverride?.type ?? defaultSettings.type;
        
        _settings.blend             = settingsOverride?.blend ?? defaultSettings.blend;
        
        _settings.frequency         = settingsOverride?.frequency ?? defaultSettings.frequency;
        _settings.interference      = settingsOverride?.interference ?? defaultSettings.interference;
        _settings.noise             = settingsOverride?.noise ?? defaultSettings.noise;
        _settings.scanLine          = settingsOverride?.scanLine ?? defaultSettings.scanLine;
        _settings.colored           = settingsOverride?.colored ?? defaultSettings.colored;
        
        _settings.intensityType3    = settingsOverride?.intensityType3 ?? defaultSettings.intensityType3;
        
        _settings.lines             = settingsOverride?.lines ?? defaultSettings.lines;
        _settings.scanSpeed         = settingsOverride?.scanSpeed ?? defaultSettings.scanSpeed;
        _settings.linesThreshold    = settingsOverride?.linesThreshold ?? defaultSettings.linesThreshold;
        _settings.exposure          = settingsOverride?.exposure ?? defaultSettings.exposure;

        _settings.noiseTex          = settingsOverride?.noiseTex ?? defaultSettings.noiseTex;
        _settings.material          = settingsOverride?.material ?? defaultSettings.material;      
    }
    
    // Initialize the current settings with default.
    public void InitialState()
    {
        SaveSettings(currentSettings);
        SaveSettings(glitchFeature.settings);
    }

    public void Setup()
    {
        if (Control == null)
            Control = this;
        else if (Control != this)
            Destroy(this.gameObject);

        InitialState();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_GlitchFXManager))]
public class Script_GlitchFXManagerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_GlitchFXManager t = (Script_GlitchFXManager)target;
        
        if (GUILayout.Button("Set Default"))
        {
            t.SetDefault();
        }

        if (GUILayout.Button("Set High"))
        {
            t.SetHigh();
        }

        if (GUILayout.Button("Set Low"))
        {
            t.SetLow();
        }
        
        if (GUILayout.Button("Set Blend 1f"))
        {
            t.SetBlend(1f);
        }

        if (GUILayout.Button("Set Blend 0f"))
        {
            t.SetBlend(0f);
        }

        if (GUILayout.Button("Blend to 1f"))
        {
            t.BlendTo(1f, 5f);
        }
        
        if (GUILayout.Button("Blend to 0f"))
        {
            t.BlendTo(0f, 5f);
        }
    }
}
#endif