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

    [SerializeField] private PixelPerfectCamera pixelPerfectCamera;
    
    [Header("Calculated Camera Properties")]
    [Tooltip("An upscaled zoom that ensures the image scaled only so much that it will remain completely on screen.")]
    [SerializeField] private int zoom = 1;

    [SerializeField] private CinemachineBrain cinemachineBrain;

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

    public Vector2Int PixelScreenSize
    {
        get => pixelPerfectCamera.pixelScreenSize;
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
