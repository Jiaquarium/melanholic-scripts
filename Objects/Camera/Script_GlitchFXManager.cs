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

    [Header("---- Initial State Settings ----")]
    
    [SerializeField] private GlitchImageEffect.GlitchType type = GlitchImageEffect.GlitchType.Type1;
    
    [Range(0, 1)]
    [SerializeField] private float blend;

    [Header("Parameters of Type1")]
    [Range(0, 10)]
    [SerializeField] private float frequency;

    [Range(0, 500)]
    [SerializeField] private float interference;

    [Range(0, 5)]
    [SerializeField] private float noise;

    [Range(0, 20)]
    [SerializeField] private float scanLine;

    [Range(0, 1)]
    [SerializeField] private float colored;

    [Header("Parameters of Type3")]
    [Range(0, 30)]
    [SerializeField] private float intensityType3;

    [Header("Parameters of Type4")]
    [Range(100, 500)]
    [SerializeField] private float lines;

    [Range(1, 6)]
    [SerializeField] private float scanSpeed;

    [Range(0.1f, 0.9f)]
    [SerializeField] private float linesThreshold;

    [Range(0, 0.8f)]
    [SerializeField] private float exposure;
    
    [SerializeField] private Texture2D noiseTex;
    [SerializeField] private Material material;
    
    [SerializeField] private GlitchImageEffect glitchFeature;

    private Coroutine blendCoroutine;
    private float timer;

    private GlitchImageEffect.GlitchImageEffectSettings defaultSettings;
    
    void OnValidate()
    {
        SaveSettings(glitchFeature.settings);
    }
    
    void OnDisable()
    {
        SaveSettings(glitchFeature.settings, defaultSettings);
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
            float originalBlend = glitchFeature.settings.blend;
            float blendDifference = newBlendValue - originalBlend;
            
            while (timer > 0f)
            {
                yield return null;
                
                float timeSinceLastFrame = Time.unscaledDeltaTime;
                timer = Mathf.Max(timer - timeSinceLastFrame, 0f);

                float percentDone = 1 - (timer / time);
                float newBlend = originalBlend + (percentDone * blendDifference);
                
                blend = Mathf.Clamp(newBlend, 0f, 1f);
                SaveSettings(glitchFeature.settings);
            }

            if (cb != null)
                cb();
        }
    }

    public void SetBlend(float val)
    {
        blend = val;
        SaveSettings(glitchFeature.settings);
    }
    
    // Inject the settings either defined on this manager or another settings object.
    private void SaveSettings(
        GlitchImageEffect.GlitchImageEffectSettings _settings,
        GlitchImageEffect.GlitchImageEffectSettings fromSettings = null
    )
    {
        _settings.type              = fromSettings != null ? fromSettings.type : type;
        
        _settings.blend             = fromSettings != null ? fromSettings.blend : blend;
        
        _settings.frequency         = fromSettings != null ? fromSettings.frequency : frequency;
        _settings.interference      = fromSettings != null ? fromSettings.interference : interference;
        _settings.noise             = fromSettings != null ? fromSettings.noise : noise;
        _settings.scanLine          = fromSettings != null ? fromSettings.scanLine : scanLine;
        _settings.colored           = fromSettings != null ? fromSettings.colored : colored;
        
        _settings.intensityType3    = fromSettings != null ? fromSettings.intensityType3 : intensityType3;
        
        _settings.lines             = fromSettings != null ? fromSettings.lines : lines;
        _settings.scanSpeed         = fromSettings != null ? fromSettings.scanSpeed : scanSpeed;
        _settings.linesThreshold    = fromSettings != null ? fromSettings.linesThreshold : linesThreshold;
        _settings.exposure          = fromSettings != null ? fromSettings.exposure : exposure;

        _settings.noiseTex          = fromSettings != null ? fromSettings.noiseTex : noiseTex;
        _settings.material          = fromSettings != null ? fromSettings.material : material;      
    }
    
    // Save a copy of the default settings;
    private void InitialState()
    {
        defaultSettings = new GlitchImageEffect.GlitchImageEffectSettings();
        
        SaveSettings(defaultSettings);
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