using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System;
using Cinemachine;

/// <summary>
/// For sprites & canvases
/// </summary>
public class Script_GraphicsManager : MonoBehaviour
{
    public static Script_GraphicsManager Control;
    public const int AssetsPPU = 36;
    
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
            float defaultOrthoSize = (PixelScreenSize.y * 0.5f) / (Zoom * AssetsPPU);
            int cinemachineVCamZoom = Math.Max(1, Mathf.RoundToInt(Zoom * defaultOrthoSize / TargetOrthoSize));
            
            float correctedOrthoSize = Zoom * defaultOrthoSize / cinemachineVCamZoom;

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

    public float TargetAspect
    {
        get => pixelPerfectCamera.targetAspect;
    }

    public float MinOrthoSizeCameraConfinement
    {
        get => minOrthoSizeCameraConfinement;
    }
    
    void OnEnable()
    {
        CalculateZoom();    
    }
    
    void LateUpdate()
    {
        CalculateZoom();
    }

    private void CalculateZoom()
    {
        // https://github.com/Unity-Technologies/Graphics/blob/c93f57ef7c3f23a377dcd970a604d47448eb2250/com.unity.render-pipelines.universal/Runtime/2D/PixelPerfectCameraInternal.cs#L75
        int horizontalZoom = pixelPerfectCamera.pixelScreenSize.x / pixelPerfectCamera.refResolutionX;
        int verticalZoom = pixelPerfectCamera.pixelScreenSize.y / pixelPerfectCamera.refResolutionY;
        zoom = Math.Max(1, Math.Min(verticalZoom, horizontalZoom));
    }
    
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
}
