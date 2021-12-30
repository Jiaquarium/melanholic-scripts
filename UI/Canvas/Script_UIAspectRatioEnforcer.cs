using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Script_UIAspectRatioEnforcer : MonoBehaviour
{
    private enum UIPosition
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Top,
        Bottom
    }

    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 offset;
    [SerializeField] private UIPosition _UIPosition;
    [SerializeField] private CanvasScaler canvasScaler;
    [SerializeField] private Camera cam;
    [SerializeField] private Script_GraphicsManager graphics;

    [SerializeField] private Script_CanvasAdjuster canvasAdjuster;
    
    private RectTransform rect;
    
    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        // Get Y based on Target Aspect Ratio
        float topBorderHeight = cam.rect.y * (float)Screen.height;
        float sideBorderWidth = cam.rect.x * (float)Screen.width;

        position.y = topBorderHeight / canvasScaler.scaleFactor;
        position.x = sideBorderWidth / canvasScaler.scaleFactor;

        switch (_UIPosition)
        {
            case (UIPosition.TopLeft):
                position.y *= -1;
                break;
            case (UIPosition.TopRight):
                position.y *= -1;
                position.x *= -1;
                break;
            case (UIPosition.BottomLeft):
                break;
            case (UIPosition.BottomRight):
                position.x *= -1;
                break;
            case (UIPosition.Top):
                position.y *= -1;
                position.x = 0f;
                break;
            case (UIPosition.Bottom):
                position.x = 0f;
                break;
            default:
                break;
        }
        
        rect.anchoredPosition = position + (offset / canvasScaler.scaleFactor);

        if (canvasAdjuster != null)
            canvasAdjuster.AdjustPosition(Mathf.RoundToInt(canvasScaler.scaleFactor), graphics.PixelScreenSize.y);
    }
}
