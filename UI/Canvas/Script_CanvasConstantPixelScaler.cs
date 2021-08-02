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

    [SerializeField] private PixelPerfectCamera pixelPerfectCamera;
    
    [SerializeField] private int targetScaleFactor;
    [SerializeField] private int hiddenScaleFactor;

    private bool isScaling;

    void OnEnable()
    {
        SetScaleFactor();
    }

    void Start()
    {
        SetScaleFactor();
    }

    void Update()
    {
        if (!isScaling)
            SetScaleFactor();
    }

    // Animate frame to appear by moving from a scale factor that is outside the viewport to one that is inside.
    public void AnimateOpen(Action cb)
    {
        gameObject.SetActive(true);
        
        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
        float scaleDecrease = hiddenScaleFactor - targetScaleFactor;
        
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
                cb();
        }
    }

    public void AnimateClose(Action cb)
    {
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
            if (pixelPerfectCamera == null)
                Setup();
            
            targetScaleFactor = pixelPerfectCamera.pixelRatio;
            
            // Will result in the canvas being hidden from view.
            hiddenScaleFactor = targetScaleFactor + 1;
            
            GetComponent<CanvasScaler>().scaleFactor = Mathf.Max(targetScaleFactor, 1);
        }
        catch (System.Exception error)
        {
            Debug.LogWarning(error);
        }
    }

    public void Setup()
    {
        pixelPerfectCamera = Script_Game.Game.PixelPerfectCamera;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_CanvasConstantPixelScaler))]
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