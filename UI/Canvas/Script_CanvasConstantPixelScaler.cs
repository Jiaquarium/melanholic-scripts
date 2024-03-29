using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
using System;
using Cinemachine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

// Note that when we adjust the screen size, the PixelRatio is unchanged unless we
// disable and reenable Pixel Perfect Camera component.
[RequireComponent(typeof(CanvasScaler))]
public class Script_CanvasConstantPixelScaler : MonoBehaviour
{
    public enum MyScenes
    {
        Game = 0,
        Start = 1
    }
    
    private static float scaleAnimateTime = 0.5f;
    private static float onScaleDonePauseTime = 0.25f;

    [Tooltip("Set for UI to always scale by an implied camera with size of GraphicsManager.TargetOrthoSize")]
    [SerializeField] private bool isOnlyUseDefaultVCamScaling;

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
    [SerializeField] private GameObject canvasAdjustersParent;
    [SerializeField] private Script_CanvasAdjuster[] canvasAdjusters;

    [SerializeField] private MyScenes mySceneBehavior;

    private Script_ScalingBounds defaultBounds;
    private bool isScaling;
    private CanvasScaler myCanvasScaler;

    public CanvasScaler MyCanvasScaler
    {
        get 
        {
            if (myCanvasScaler == null)
                myCanvasScaler = GetComponent<CanvasScaler>();
        
            return myCanvasScaler;
        }
    }
    
    public Script_ScalingBounds Bounds
    {
        get => bounds;
        set => bounds = value;
    }

    public bool IsCustomScaling
    {
        get => isCustomScaling;
        set => isCustomScaling = value;
    }

    public float ScaleFactor
    {
        get
        {
            SetScaleFactor();
            
            return MyCanvasScaler.scaleFactor;
        }
        set => MyCanvasScaler.scaleFactor = value;
    }

    private Script_GraphicsManager GraphicsManager
    {
        get
        {
            // For Start Scene Constant Pixel Scalers, the GraphicsManager reference
            // will be lost when going from Title > Game > Title, so always grab Start's reference.
            if (
                mySceneBehavior == MyScenes.Start
                && SceneManager.GetActiveScene().name == Script_SceneManager.TitleScene
            )
            {
                if (graphics == null)
                    graphics = Script_Start.Main.StartGraphicsManager;

                return graphics;
            }
            
            if (graphics == null)
                graphics = Script_GraphicsManager.Control;
            
            return graphics;
        }
    }

    void OnValidate()
    {
        PopulateCanvasAdjusters();
    }
    
    void Awake()
    {
        SetupCanvasAdjusters();
        defaultBounds = bounds;
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

    public void SetDefaultBounds()
    {
        Bounds = defaultBounds;
    }

    // Animate frame to appear by moving from a scale factor that is outside the viewport to one that is inside.
    public void AnimateOpen(Action cb, bool shouldPauseAfter = true)
    {
        SetScaleFactor();
        
        float scaleDecrease = hiddenScaleFactor - targetScaleFactor;
        
        gameObject.SetActive(true);
        
        isScaling = true;
        
        StartCoroutine(ScaleDown());
        
        IEnumerator ScaleDown()
        {
            // Start out hiding canvas.
            MyCanvasScaler.scaleFactor = hiddenScaleFactor;
            
            while (MyCanvasScaler.scaleFactor > targetScaleFactor)
            {
                yield return null;

                float scaleFactor = MyCanvasScaler.scaleFactor;
                scaleFactor -= (Time.deltaTime / scaleAnimateTime) * scaleDecrease;
                
                if (scaleFactor < targetScaleFactor)
                    scaleFactor = targetScaleFactor;
                
                MyCanvasScaler.scaleFactor = scaleFactor;
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
        
        float scaleIncrease = hiddenScaleFactor - targetScaleFactor;

        gameObject.SetActive(true);
        
        isScaling = true;

        StartCoroutine(ScaleUp());
        
        IEnumerator ScaleUp()
        {
            // Start out showing canvas.
            MyCanvasScaler.scaleFactor = targetScaleFactor;

            while (MyCanvasScaler.scaleFactor < hiddenScaleFactor)
            {
                yield return null;
                
                float scaleFactor = MyCanvasScaler.scaleFactor;
                scaleFactor += (Time.deltaTime / scaleAnimateTime) * scaleIncrease;
                
                if (scaleFactor > hiddenScaleFactor)
                    scaleFactor = hiddenScaleFactor;
                
                MyCanvasScaler.scaleFactor = scaleFactor;
            }

            isScaling = false;

            gameObject.SetActive(false);

            if (cb != null)
                cb();
        }
    }
    
    public void SetScaleFactor()
    {
        // Catch when Singletons aren't set yet.
        try
        {
            if (isOnlyUpscaled)
                targetScaleFactor = GraphicsManager.Zoom;
            else
            {
                targetScaleFactor = isOnlyUseDefaultVCamScaling
                    ? GraphicsManager.UIDefaultScaleFactorAdjustedZoomCam
                    : GraphicsManager.UIDefaultScaleFactor;
            }

            if (IsCustomScaling)
                targetScaleFactor = GetScaleFactorByViewportHeight(GraphicsManager.PixelScreenSize.y);
            
            // Will result in the canvas being hidden from view.
            hiddenScaleFactor = targetScaleFactor + 1;
            
            int scaleFactor = Mathf.Max(targetScaleFactor, 1);
            MyCanvasScaler.scaleFactor = scaleFactor;

            foreach (var canvasAdjuster in canvasAdjusters)
            {
                // If UI Aspect Ratio Enforcer is present on adjuster, don't call its AdjustPosition here,
                // UI Aspect Ratio Enforcer will Call Adjust Position after it sets its position first.
                // Otherwise, it will never be adjusted because UI Aspect Ratio Enforcer will override it.
                var UIAspectRatioEnforcer = canvasAdjuster.GetComponent<Script_UIAspectRatioEnforcer>();
                if (UIAspectRatioEnforcer == null)
                    canvasAdjuster.AdjustPosition(scaleFactor, GraphicsManager.PixelScreenSize.y, Vector3.zero, false);
            }
        }
        catch (System.Exception error)
        {
            Debug.LogWarning($"{name}: {error}");
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

    private void PopulateCanvasAdjusters()
    {
        if (canvasAdjustersParent != null)
            canvasAdjusters = canvasAdjustersParent.GetComponentsInChildren<Script_CanvasAdjuster>(true);
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