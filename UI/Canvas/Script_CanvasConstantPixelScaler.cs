using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

// Note that when we adjust the screen size, the PixelRatio is unchanged unless we
// disable and reenable Pixel Perfect Camera component.
[RequireComponent(typeof(CanvasScaler))]
public class Script_CanvasConstantPixelScaler : MonoBehaviour
{
    private static float scaleAnimateTime = 0.5f;
    private static float onScaleDonePauseTime = 0.25f;

    /// <summary>
    /// Use Pixel Ratio that will always fit canvases to smaller than screen size.
    /// </summary>
    [SerializeField] private bool isUpscaledRTScaleFactor;
    
    [SerializeField] private Script_GraphicsManager graphics;
    
    [SerializeField] private int targetScaleFactor;
    [SerializeField] private int hiddenScaleFactor;

    private bool isScaling;

    void OnEnable()
    {
        if (!isScaling)
            SetScaleFactor();
    }

    void Start()
    {
        if (!isScaling)
            SetScaleFactor();
    }

    void Update()
    {
        if (!isScaling)
            SetScaleFactor();
    }

    // Animate frame to appear by moving from a scale factor that is outside the viewport to one that is inside.
    public void AnimateOpen(Action cb, bool shouldPauseAfter = true)
    {
        SetScaleFactor();
        
        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
        float scaleDecrease = hiddenScaleFactor - targetScaleFactor;
        
        gameObject.SetActive(true);
        
        isScaling = true;
        
        StartCoroutine(ScaleDown());
        
        IEnumerator ScaleDown()
        {
            // Start out hiding canvas.
            canvasScaler.scaleFactor = hiddenScaleFactor;
            
            while (canvasScaler.scaleFactor > targetScaleFactor)
            {
                yield return null;

                float scaleFactor = canvasScaler.scaleFactor;
                scaleFactor -= (Time.deltaTime / scaleAnimateTime) * scaleDecrease;
                
                if (scaleFactor < targetScaleFactor)
                    scaleFactor = targetScaleFactor;
                
                canvasScaler.scaleFactor = scaleFactor;
            }

            isScaling = false;

            if (cb != null)
            {
                if (shouldPauseAfter)
                    yield return new WaitForSeconds(onScaleDonePauseTime); 
                
                cb();
            }
        }
    }

    public void AnimateClose(Action cb)
    {
        SetScaleFactor();
        
        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
        float scaleIncrease = hiddenScaleFactor - targetScaleFactor;

        gameObject.SetActive(true);
        
        isScaling = true;

        StartCoroutine(ScaleUp());
        
        IEnumerator ScaleUp()
        {
            // Start out showing canvas.
            canvasScaler.scaleFactor = targetScaleFactor;

            while (canvasScaler.scaleFactor < hiddenScaleFactor)
            {
                yield return null;
                
                float scaleFactor = canvasScaler.scaleFactor;
                scaleFactor += (Time.deltaTime / scaleAnimateTime) * scaleIncrease;
                
                if (scaleFactor > hiddenScaleFactor)
                    scaleFactor = hiddenScaleFactor;
                
                canvasScaler.scaleFactor = scaleFactor;
            }

            isScaling = false;

            gameObject.SetActive(false);

            if (cb != null)
                cb();
        }
    }
    
    private void SetScaleFactor()
    {
        // Catch when Singletons aren't set yet.
        try
        {
            targetScaleFactor = isUpscaledRTScaleFactor ? graphics.UpscaledRTPixelRatio : graphics.PixelRatio;
            
            // Will result in the canvas being hidden from view.
            hiddenScaleFactor = targetScaleFactor + 1;
            
            GetComponent<CanvasScaler>().scaleFactor = Mathf.Max(targetScaleFactor, 1);
        }
        catch (System.Exception error)
        {
            Debug.LogWarning(error);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_CanvasConstantPixelScaler))]
[CanEditMultipleObjects]
public class Script_CanvasConstantPixelScalerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_CanvasConstantPixelScaler t = (Script_CanvasConstantPixelScaler)target;
        if (GUILayout.Button("AnimateOpen()"))
        {
            t.AnimateOpen(null);
        }

        if (GUILayout.Button("AnimateClose()"))
        {
            t.AnimateClose(null);
        }
    }
}
#endif