using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Overlay canvases.
/// </summary>
[RequireComponent(typeof(Image))]
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

    [Header("Viewport Overlap - Center Aligned UI")]
    [Tooltip("Set to true for center aligned UI to always at least reach bottom of viewport")]
    [SerializeField] private bool isCenterImageAtBottom;
    
    private Vector2 canvasBottomWorldPoint = new Vector2();
    private RectTransform rect;
    private Vector3[] worldCorners;
    
    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        if (isCenterImageAtBottom)
            StickCenterImageToBottom();
        else
            StickImageToBorder();
    }

    void LateUpdate()
    {
        if (isCenterImageAtBottom)
            StickCenterImageToBottom();
        else
            StickImageToBorder();
    }

    private void StickImageToBorder()
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

    /// <summary>
    /// Make a center aligned canvas touch the bottom of viewport. (e.g. Fullart that needs
    /// to be grounded always)
    /// </summary>
    private void StickCenterImageToBottom()
    {
        SetDefaultPosition();
        
        // Get BL, TL, TR, BR corners
        Vector3[] v = new Vector3[4];
        rect.GetWorldCorners(v);
        worldCorners = v;

        // Get BL point; for Overlay canvases world space == screen space.
        canvasBottomWorldPoint = worldCorners[0];
        
        // Check viewport (Screen Space) contains the overlay canvas point.
        if (cam.pixelRect.Contains(canvasBottomWorldPoint))
            SetCenterCanvasToBottom();

        void SetCenterCanvasToBottom()
        {
            float windowAspect = (float)Screen.width / (float)Screen.height;
            float scaleHeight = windowAspect / graphics.TargetAspect;
            float newYPosition = scaleHeight >= 1.0f
                // Pillarbox, adjust y position by the full amount.
                ? rect.anchoredPosition.y - (canvasBottomWorldPoint.y / canvasScaler.scaleFactor)
                // Letterbox, adjust y position less the pixels of letterbox border.
                : rect.anchoredPosition.y - ((canvasBottomWorldPoint.y - cam.pixelRect.y) / canvasScaler.scaleFactor);
            
            position = new Vector3(
                rect.anchoredPosition.x,
                newYPosition,
                0f
            );
            
            rect.anchoredPosition = position;
        }

        void SetDefaultPosition()
        {
            position = Vector3.zero + (offset / canvasScaler.scaleFactor);
            rect.anchoredPosition = position;
            rect.ForceUpdateRectTransforms();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_UIAspectRatioEnforcer))]
    public class Script_UIAspectRatioEnforcerTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_UIAspectRatioEnforcer t = (Script_UIAspectRatioEnforcer)target;
            
            if (GUILayout.Button("Stick To Bottom"))
            {
                t.StickCenterImageToBottom();
            }
        }
    }
#endif    
}
