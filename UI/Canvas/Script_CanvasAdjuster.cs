using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manually adjust canvas position depending on zoom level.
/// </summary>
public class Script_CanvasAdjuster : MonoBehaviour
{
    [Tooltip("adjustment(x, y) for resolutions z to w (z & w inclusive)")]
    [SerializeField] private Vector4[] positionDeltas;
    
    [SerializeField] private RectTransform rect;

    private Vector3 originalPosition;

    public void AdjustPosition(
        int scaleFactor,
        int height,
        Vector3 positionOverride,
        bool usePositionOverride
    )
    {
        Vector3 position;
        
        if (usePositionOverride)
            position = positionOverride;
        else
            position = originalPosition;
        
        // Scale Factor determines which adjustment to use.
        int scaleIndex = scaleFactor - 1;
        Vector4 positionDelta = Vector4.zero;
        Vector3 adjustment = Vector3.zero;

        if (scaleIndex < positionDeltas.Length)
            positionDelta = positionDeltas[scaleIndex];
        
        // If screen height falls in range, make the adjustment for the
        // current Scale Factor.
        if (
            positionDelta != null
            && (height >= positionDelta.z && height < positionDelta.w)
        )
        {
            adjustment.x = positionDelta.x;
            adjustment.y = positionDelta.y;
        }

        // Adjustment is scale independent because the adjustment
        // only applies to a predefined scale factor.
        rect.anchoredPosition = position + adjustment;
    }

    public void Setup()
    {
        originalPosition = rect.anchoredPosition;
    }
}
