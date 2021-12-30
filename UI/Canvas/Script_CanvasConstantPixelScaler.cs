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
    [SerializeField] private bool isOnlyUpscaled;
    [SerializeField] private bool isCustomScaling;
    
    [SerializeField] private Script_GraphicsManager graphics;
    
    [SerializeField] private int targetScaleFactor;
    [SerializeField] private int hiddenScaleFactor;

    [Header("Custom Scaling Bounds")]
    [SerializeField] private Script_ScalingBounds bounds;
    [SerializeField] private Script_CanvasAdjuster[] canvasAdjusters;

    private bool isScaling;

    
    void Awake()
    {
        SetupCanvasAdjusters();
    }
    
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
            targetScaleFactor = isOnlyUpscaled
                ? graphics.Zoom
                : graphics.UIDefaultScaleFactor;
            
            if (isCustomScaling)
                targetScaleFactor = GetScaleFactorByViewportHeight(graphics.PixelScreenSize.y);
            
            // Will result in the canvas being hidden from view.
            hiddenScaleFactor = targetScaleFactor + 1;
            
            int scaleFactor = Mathf.Max(targetScaleFactor, 1);
            GetComponent<CanvasScaler>().scaleFactor = scaleFactor;

            foreach (var canvasAdjuster in canvasAdjusters)
            {
                // UI Aspect Ratio Enforcer will Call Adjust Position after it sets its position first.
                var UIAspectRatioEnforcer = canvasAdjuster.GetComponent<Script_UIAspectRatioEnforcer>();
                if (UIAspectRatioEnforcer == null)
                    canvasAdjuster.AdjustPosition(scaleFactor, graphics.PixelScreenSize.y);
            }
        }
        catch (System.Exception error)
        {
            Debug.LogWarning(error);
        }
    }

    private int GetScaleFactorByViewportHeight(int height)
    {
        if (bounds == null)
            return targetScaleFactor;
        
        if (height < bounds.Bound1 && bounds.Bound1 > 0)
        {
            return 1;
        }
        else if (
            (height >= bounds.Bound1 && bounds.Bound1 > 0)
            && (height < bounds.Bound2 && bounds.Bound2 > 0)
        )
        {
            return 2;
        }
        else if (
            (height >= bounds.Bound2 && bounds.Bound2 > 0)
            && (height < bounds.Bound3 && bounds.Bound3 > 0)
        )
        {
            return 3;
        }
        else
        {
            return targetScaleFactor;
        }
    }

    private void SetupCanvasAdjusters()
    {
        foreach (var canvasAdjuster in canvasAdjusters)
            canvasAdjuster.Setup();
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