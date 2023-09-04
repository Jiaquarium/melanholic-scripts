using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Cinemachine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// For sprites & canvases
/// </summary>
public class Script_GraphicsManager : MonoBehaviour
{
    /// <summary>
    /// Note: Singleton GraphicsManager should only be set for Game.
    /// </summary>
    public static Script_GraphicsManager Control;
    public const int AssetsPPU = 36;
    public const int PhysicsSolverIterationsDefault = 6;
    public const int PhysicsSolverIterationsWellsWorld = 3;
    public const int PhysicsSolverIterationsSpikeRoom = 3;
    public const int CamCanvasPlaneDistance = 20;
    private const float DefaultShadowDistance = 100f;
    private const float UnderworldShadowDistance = 150f;

    private const float WellsWorldSpecialIntroShadowDistance = 120f;
    // Allows top right shadow on walls to be showing but not be pitch black
    private const float CelestialGardensSpecialIntroShadowDistance = 150f;
    // Allows totem shadows to show but not be pitch black
    private const float XXXWorldSpecialIntroShadowDistance = 105f;
    
    [SerializeField] private float TargetOrthoSize = 7.5f;

    [SerializeField] private float minOrthoSizeCameraConfinement = 5.67f;

    [SerializeField] private PixelPerfectCamera pixelPerfectCamera;
    
    [Header("Calculated Camera Properties")]
    [Tooltip("An upscaled zoom that ensures the image scaled only so much that it will remain completely on screen.")]
    [SerializeField] private int zoom = 1;

    // The current zoom multiplier used after Cinemachine adjustment.
    public int PixelRatio
    {
        get => pixelPerfectCamera.pixelRatio;
    }

    public int Zoom
    {
        get => zoom;
    }

    public int PPU
    {
        get => AssetsPPU;
    }

    // Scale UI to largest possible for screen size.
    public int UIDefaultScaleFactor
    {
        get => Mathf.Max(PixelRatio, Zoom);
    }

    /// <summary>
    /// Scale factor that reflects the default Main VCam Ortho Size Setting.
    /// For UI elements that need to always scale by the default zoom regardless of VCam Zoom.
    /// This is the same as UIDefaultScaleFactor in that PixelRatio == cinemachineVCamZoom,
    /// except here uses a constant Target Ortho Size.
    /// </summary>
    public int UIDefaultScaleFactorAdjustedZoomCam
    {
        get
        {
            int cinemachineVCamZoom = Math.Max(1, Mathf.RoundToInt(Zoom * DefaultOrthoSize / TargetOrthoSize));
            
            float correctedOrthoSize = Zoom * DefaultOrthoSize / cinemachineVCamZoom;

            // Ensure this matches the custom Bounds in
            // Graphics/com.unity.render-pipelines.universal/Runtime/2D/PixelPerfectCameraInternal
            switch (cinemachineVCamZoom)
            {
                case 1:
                    if (correctedOrthoSize > 9.0f)
                        cinemachineVCamZoom++;
                    break;
                case 2:
                    if (correctedOrthoSize > 8.5f)
                        cinemachineVCamZoom++;
                    break;
                case 3:
                    if (correctedOrthoSize > 8f)
                        cinemachineVCamZoom++;
                    break;
                case 4:
                    if (correctedOrthoSize > 8f)
                        cinemachineVCamZoom++;
                    break;
                default:
                    if (correctedOrthoSize > 8f)
                        cinemachineVCamZoom++;
                    break;
            }

            return Mathf.Max(cinemachineVCamZoom, Zoom);
        }
    }

    /// <summary>
    /// Screen size in pixels based on the Camera Rect (after Aspect Ratio adjustments are made)
    /// </summary>
    public Vector2Int PixelScreenSize
    {
        get => pixelPerfectCamera.pixelScreenSize;
    }

    public double TargetAspect
    {
        get => pixelPerfectCamera.TargetAspect;
    }

    public float MinOrthoSizeCameraConfinement
    {
        get => minOrthoSizeCameraConfinement;
    }

    /// <summary>
    /// The implied ortho target for pixel perfectness with current zoom.
    /// </summary>
    public float DefaultOrthoSize
    {
        get => (PixelScreenSize.y * 0.5f) / (Zoom * AssetsPPU);
    }
    
    void OnEnable()
    {
        CalculateZoom();
        InitialState();
    }

    void OnDisable()
    {
        InitialState();
    }
    
    void LateUpdate()
    {
        CalculateZoom();
    }

    public float GetShadowDistance()
    {
        UniversalRenderPipelineAsset urp = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
        return urp.shadowDistance;
    }
    
    public float SetDefaultShadowDistance() => SetShadowDistance(DefaultShadowDistance);
    public float SetUnderworldShadowDistance() => SetShadowDistance(UnderworldShadowDistance);
    public float SetWellsWorldSpecialIntroShadowDistance() => SetShadowDistance(WellsWorldSpecialIntroShadowDistance);
    public float SetCelestialGardensSpecialIntroShadowDistance() => SetShadowDistance(CelestialGardensSpecialIntroShadowDistance);
    public float SetXXXWorldSpecialIntroShadowDistance() => SetShadowDistance(XXXWorldSpecialIntroShadowDistance);
    
    private float SetShadowDistance(float shadowDistance)
    {
        UniversalRenderPipelineAsset urp = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
        urp.shadowDistance = shadowDistance;

        return urp.shadowDistance;
    }

    public void SetDefaultPhysics()
    {
        Physics.defaultSolverIterations = PhysicsSolverIterationsDefault;
    }

    public void SetWellsWorldPhysics()
    {
        Physics.defaultSolverIterations = PhysicsSolverIterationsWellsWorld;
    }

    public void SetSpikeRoomPhysics()
    {
        Physics.defaultSolverIterations = PhysicsSolverIterationsSpikeRoom;
    }
    
    private void CalculateZoom()
    {
        // https://github.com/Unity-Technologies/Graphics/blob/c93f57ef7c3f23a377dcd970a604d47448eb2250/com.unity.render-pipelines.universal/Runtime/2D/PixelPerfectCameraInternal.cs#L75
        int horizontalZoom = pixelPerfectCamera.pixelScreenSize.x / pixelPerfectCamera.refResolutionX;
        int verticalZoom = pixelPerfectCamera.pixelScreenSize.y / pixelPerfectCamera.refResolutionY;
        zoom = Math.Max(1, Math.Min(verticalZoom, horizontalZoom));
    }

    private void InitialState()
    {
        SetDefaultShadowDistance();
    }
    
    /// <summary>
    /// Note: Setup should ONLY be called in Game Scene (Start Scene must manually ref necessary fields).
    /// </summary>
    public void Setup()
    {
        if (Control == null)
        {
            Control = this;
        }
        else if (Control != this)
        {
            Destroy(this.gameObject);
        }

        pixelPerfectCamera = Script_Game.Game.PixelPerfectCamera;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_GraphicsManager))]
    public class Script_GraphicsManagerTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_GraphicsManager t = (Script_GraphicsManager)target;
            
            if (GUILayout.Button("Get Shadow Distance"))
            {
                t.GetShadowDistance();
            }

            if (GUILayout.Button("Set Shadow Distance"))
            {
                t.SetShadowDistance(150f);
            }
        }
    }
#endif
}