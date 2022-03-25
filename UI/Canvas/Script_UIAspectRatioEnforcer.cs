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
        Bottom,
        Left,
        Right
    }

    [Tooltip("Offset that scales with scaleFactor. Based on scaling factor of 1")]
    [SerializeField] private Vector3 refResScalingOffset;
    [UnityEngine.Serialization.FormerlySerializedAs("offset")]
    [SerializeField] private Vector3 constantPixelSizeOffset;
    [SerializeField] private UIPosition _UIPosition;
    [SerializeField] private CanvasScaler canvasScaler;
    [SerializeField] private Camera cam;
    [SerializeField] private Script_GraphicsManager graphics;

    [SerializeField] private Script_CanvasAdjuster canvasAdjuster;

    [Header("Viewport Overlap - Center Aligned UI")]
    [Tooltip("Set to true for center aligned UI to always at least reach bottom of viewport")]
    [SerializeField] private bool isCenterImageAtBottom;

    [Header("Pre-Rendered Cut Scenes")]
    [Tooltip("Use UI Aspect Ratio Enforcer Frame instead of Rect to stick to")]
    [SerializeField] private bool isStickToUIFrame;
    [SerializeField] private Script_UIAspectRatioEnforcerFrame UIAspectRatioEnforcerFrame;
    
    private Vector3 position;
    private Vector2 canvasBottomWorldPoint = new Vector2();
    private RectTransform rect;
    private Vector3[] worldCorners;
    
    void Awake()
    {
        rect = GetComponent<RectTransform>();

        Canvas canvas = canvasScaler.GetComponent<Canvas>();
        
        if (canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            Debug.LogError($"{name}'s Canvas Scaler needs to be Screen Space Overlay for UI Enforcer to work properly");
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
        float topBorderHeight;
        float sideBorderWidth;
        Vector3 _offset;
        
        if (isStickToUIFrame)
        {
            // Set border height and width based on UI Frame
            topBorderHeight = UIAspectRatioEnforcerFrame.BorderHeight;
            sideBorderWidth = UIAspectRatioEnforcerFrame.BorderWidth;
        }
        else
        {
            topBorderHeight = cam.rect.y * (float)Screen.height;
            sideBorderWidth = cam.rect.x * (float)Screen.width;
        }

        switch (canvasScaler.uiScaleMode)
        {
            case CanvasScaler.ScaleMode.ConstantPixelSize:
                _offset = constantPixelSizeOffset / canvasScaler.scaleFactor + refResScalingOffset;
                position.y = topBorderHeight / canvasScaler.scaleFactor;
                position.x = sideBorderWidth / canvasScaler.scaleFactor;
                break;
            case CanvasScaler.ScaleMode.ScaleWithScreenSize:
                float scaleFactor = graphics.PixelScreenSize.y / canvasScaler.referenceResolution.y;
                
                _offset = constantPixelSizeOffset / scaleFactor + refResScalingOffset;
                position.y = topBorderHeight / scaleFactor;
                position.x = sideBorderWidth / scaleFactor;
                break;
            default:
                Debug.LogWarning($"Canvas Scaler set to an unsupported UI Scale Mode: {canvasScaler.uiScaleMode}");
                _offset = refResScalingOffset;
                position.y = topBorderHeight;
                position.x = sideBorderWidth;
                break;
        }
        
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
            case (UIPosition.Left):
                position.y = 0f;
                break;
            case (UIPosition.Right):
                position.y = 0f;
                position.x *= -1;
                break;
            default:
                break;
        }
        
        rect.anchoredPosition = position + _offset;

        if (canvasAdjuster != null)
        {
            canvasAdjuster.AdjustPosition(
                Mathf.RoundToInt(canvasScaler.scaleFactor),
                graphics.PixelScreenSize.y,
                Vector3.zero,
                false
            );
        }
    }

    /// <summary>
    /// Make a center aligned canvas AT LEAST touch the bottom of viewport (e.g. Fullart that needs
    /// to be grounded always). If it doesn't need to be grounded it will be unaffected.
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
        
        // Make any final adjustments to canvas.
        if (canvasAdjuster != null)
            canvasAdjuster.AdjustPosition(
                Mathf.RoundToInt(canvasScaler.scaleFactor),
                graphics.PixelScreenSize.y,
                rect.anchoredPosition,
                true
            );

        void SetCenterCanvasToBottom()
        {
            double windowAspect = (double)Screen.width / (double)Screen.height;
            
            double scaleHeight = windowAspect / graphics.TargetAspect;
            float newYPosition = scaleHeight >= 1.0d
                // Pillarbox, adjust y position by the full amount.
                ? rect.anchoredPosition.y - (canvasBottomWorldPoint.y / canvasScaler.scaleFactor)
                // Letterbox, adjust y position less the pixels of letterbox border.
                : rect.anchoredPosition.y - ((canvasBottomWorldPoint.y - cam.pixelRect.y) / canvasScaler.scaleFactor);
            
            position = new Vector3(
                rect.anchoredPosition.x,
                Mathf.FloorToInt(newYPosition),
                0f
            );
            
            rect.anchoredPosition = position;
        }

        void SetDefaultPosition()
        {
            position = Vector3.zero + (constantPixelSizeOffset / canvasScaler.scaleFactor) + refResScalingOffset;
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
