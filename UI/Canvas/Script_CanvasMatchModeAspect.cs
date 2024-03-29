using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

/// <summary>
/// For use on Screen Space Overlay canvases, to explicitly define whether to match width or height.
/// For Letterboxed screens, match the width.
/// For Pillarboxed screens, match the height.
/// </summary>
[RequireComponent(typeof(CanvasScaler))]
public class Script_CanvasMatchModeAspect : MonoBehaviour
{
    [SerializeField] private PixelPerfectCamera pixelPerfectCamera;

    private double screenAspect;
    private double scaleHeight;

    private CanvasScaler canvasScaler;

    
    void Awake()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        SetMatchModeFromAspect();
    }

    void LateUpdate()
    {
        SetMatchModeFromAspect();
    }

    private void SetMatchModeFromAspect()
    {
        // Get current screen Aspect Ratio
        screenAspect = (double)Screen.width / (double)Screen.height;
        scaleHeight = screenAspect / pixelPerfectCamera.TargetAspect;

        // Letterboxed, set to match width
        if (scaleHeight < 1.0d)
        {
            canvasScaler.matchWidthOrHeight = 0f;
        }
        // Pillarboxed or no framing, set to match height
        else
        {
            canvasScaler.matchWidthOrHeight = 1f;
        }
    }
}
