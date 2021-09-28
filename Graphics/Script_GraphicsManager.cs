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
    
    [Tooltip("[BROKEN] Pixel Ratio as defined on Pixel Perfect Camera.")]
    [SerializeField] private int pixelPerfectCameraPixelRatio;
    
    
    [Tooltip("The size of current live Virtual Camera.")]
    [SerializeField] private float targetOrthoSize;
    
    [Tooltip("Current ortho size dictated by Pixel Perfect Cam.")]
    [SerializeField] private float correctedOrthoSize;

    [SerializeField] private int screenHeight;
    [SerializeField] private int screenWidth;

    [Header("Calculated Camera Properties")]
    [Tooltip("Manual calculation of the current Pixel Ratio.")]
    [SerializeField] private int calcedPixelPerfectCameraPixelRatio;
    
    [Tooltip("An upscaled zoom that ensures the image scaled only so much that it will remain completely on screen.")]
    [SerializeField] private int zoom = 1;

    public int PixelRatio
    {
        get => calcedPixelPerfectCameraPixelRatio;
    }

    public int UpscaledRTPixelRatio
    {
        get => zoom;
    }

    public int PPU
    {
        get => AssetsPPU;
    }
    
    void OnEnable()
    {
        CalculateZoom();    
    }
    
    void LateUpdate()
    {
        UpdateForCameraProperties();
        CalculatePixelPerfectCameraPixelRatio();
        CalculateZoom();
    }

    private void CalculatePixelPerfectCameraPixelRatio()
    {
        // Orthographic Size calculation equation:
        // ortho = (screenHeight * 0.5) / (zoom * PPU)
        // https://github.com/Unity-Technologies/Graphics/blob/5bd0e48534298b93d06c99088a8d06aacf26bfdb/com.unity.render-pipelines.universal/Runtime/2D/PixelPerfectCameraInternal.cs#L156
        float pixelRatio = (screenHeight * 0.5f) / (correctedOrthoSize * PPU);
        calcedPixelPerfectCameraPixelRatio = Math.Max(1, Mathf.RoundToInt(pixelRatio));
    }

    private void CalculateZoom()
    {
        // https://github.com/Unity-Technologies/Graphics/blob/c93f57ef7c3f23a377dcd970a604d47448eb2250/com.unity.render-pipelines.universal/Runtime/2D/PixelPerfectCameraInternal.cs#L75
        int verticalZoom = screenHeight / pixelPerfectCamera.refResolutionY;
        int horizontalZoom = screenWidth / pixelPerfectCamera.refResolutionX;
        zoom = Math.Max(1, Math.Min(verticalZoom, horizontalZoom));
    }

    private void UpdateForCameraProperties()
    {
        pixelPerfectCameraPixelRatio = pixelPerfectCamera.pixelRatio;
        
        CinemachineVirtualCamera liveVirtualCamera = Script_Game.Game.GetComponent<CinemachineBrain>()
            .ActiveVirtualCamera as CinemachineVirtualCamera;
        targetOrthoSize = liveVirtualCamera.m_Lens.OrthographicSize;
        correctedOrthoSize = pixelPerfectCamera.CorrectCinemachineOrthoSize(targetOrthoSize);

        screenHeight = Screen.height;
        screenWidth = Screen.width;
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
