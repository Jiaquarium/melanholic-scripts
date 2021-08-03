using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System;

/// <summary>
/// For sprites & canvases
/// </summary>
public class Script_GraphicsManager : MonoBehaviour
{
    public static Script_GraphicsManager Control;
    [SerializeField] private PixelPerfectCamera pixelPerfectCamera;

    [SerializeField] private int zoom;

    public int PixelRatio
    {
        get => pixelPerfectCamera.pixelRatio;
    }

    public int UpscaledRTPixelRatio
    {
        get => zoom;
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
        int verticalZoom = Screen.height / pixelPerfectCamera.refResolutionY;
        int horizontalZoom = Screen.width / pixelPerfectCamera.refResolutionX;
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
