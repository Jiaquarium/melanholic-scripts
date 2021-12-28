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
        BottomRight
    }

    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 offset;
    [SerializeField] private UIPosition _UIPosition;
    [SerializeField] private CanvasScaler canvasScaler;
    [SerializeField] private Camera cam;
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

        switch (_UIPosition)
        {
            case (UIPosition.TopLeft):
                topBorderHeight *= -1;
                break;
            case (UIPosition.TopRight):
                topBorderHeight *= -1;
                sideBorderWidth *= -1;
                break;
            case (UIPosition.BottomLeft):
                break;
            case (UIPosition.BottomRight):
                sideBorderWidth *= -1;
                break;
            default:
                break;
        }

        position.y = topBorderHeight / canvasScaler.scaleFactor;
        position.x = sideBorderWidth / canvasScaler.scaleFactor;
        
        rect.anchoredPosition = position + (offset / canvasScaler.scaleFactor);
    }
}
