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
    
    [SerializeField] private GlitchImageEffect glitchFeature;

    private Coroutine blendCoroutine;
    private float timer;
    
    void OnDisable()
    {
        InitialState();
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
                
                glitchFeature.settings.blend = Mathf.Clamp(newBlend, 0f, 1f);
            }

            if (cb != null)
                cb();
        }
    }
    
    public void SetBlend(float value)
    {
        glitchFeature.settings.blend = value;
    }

    private void InitialState()
    {
        SetBlend(0f);
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