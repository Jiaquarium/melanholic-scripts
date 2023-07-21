using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Adds a frame over the empty black rect space of the camera to cover overflowing UI elements (e.g. Full Art)
/// Resizing in the editor window will freeze these stretching borders, but in a build, the Update Loop
/// still runs, thus upsizing will stretch the borders properly.
/// 
/// For PRCS Framing
/// The border will be as wide as the specified minBorderHeight of Screen Rect.
/// </summary>
[ExecuteInEditMode]
public class Script_UIAspectRatioEnforcerFrame : MonoBehaviour
{
    public enum Framing
    {
        Ending = 0,
        MaskReveal = 1,
        ElderIntro = 2,
        ElleniasHand = 3,
        IdsDead = 4,
        ConstantThin = 5,
        TheSealing = 6,
        ConstantDefault = 7,
        SeaVignette = 8,
        AwakeningPortraits = 9
    }
    
    public static Script_UIAspectRatioEnforcerFrame Control;
    
    [SerializeField] private RectTransform topRect;
    [SerializeField] private RectTransform bottomRect;
    [SerializeField] private RectTransform leftRect;
    [SerializeField] private RectTransform rightRect;
    [SerializeField] private Camera cam;

    [Header("Pre-Rendered Cut Scene Framing")]
    [SerializeField] private bool isAnimatingLetterBox;
    [SerializeField] private bool isCutSceneLetterBox;
    [Tooltip("Reference resolution of the art asset")]
    [SerializeField] private Vector2Int refResolution;
    [SerializeField] private Script_GraphicsManager graphics;
    
    [SerializeField] private Script_CanvasConstantPixelScaler endingsCanvasScaler;
    [SerializeField] private Script_CanvasConstantPixelScaler maskRevealCanvasScaler;
    [SerializeField] private Script_CanvasConstantPixelScaler elderIntroCanvasScaler;
    [SerializeField] private Script_CanvasConstantPixelScaler theSealingCanvasScaler;
    [SerializeField] private Script_CanvasConstantPixelScaler ElleniasHandCanvasScaler;
    [SerializeField] private Script_CanvasConstantPixelScaler IdsDeadCanvasScaler;
    [SerializeField] private Script_CanvasConstantPixelScaler seaVignetteCanvasScaler;
    [SerializeField] private Script_CanvasConstantPixelScaler awakeningPortraitsCanvasScaler;
    
    [Tooltip("Border heights as a percent of Rect")]
    [SerializeField] private float minBorderHeight;
    [SerializeField] private float minBorderHeightDefault;
    [UnityEngine.Serialization.FormerlySerializedAs("minBorderHeightMaskReveal")]
    [SerializeField] private float minBorderHeightThin;
    [SerializeField] private float letterBoxTime;

    [SerializeField] private Script_CanvasConstantPixelScaler currentCanvasScaler;

    // Editor only
    [SerializeField] private Color devColor = new Color(0f, .47f, 0f);
    [SerializeField] private Color prodColor = new Color(0f, 0f, 0f);
    
    public float BorderWidth { get => leftRect.sizeDelta.x; }
    public float BorderHeight { get => bottomRect.sizeDelta.y; } 
    
    void Start()
    {
        MatchBorders();
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        if (isAnimatingLetterBox)
            return;
        
        if (isCutSceneLetterBox)
            CutSceneLetterBox(currentCanvasScaler);
        else
            MatchBorders();
    }

    public void EndingsLetterBox(
        bool isOpen,
        Framing framing,
        Action cb = null,
        float t = 0f,
        bool isUnscaledTime = false,
        bool isNoAnimation = false
    )
    {
        if (t <= 0f)
            t = letterBoxTime;
        
        Script_CanvasConstantPixelScaler canvasScaler;
        
        switch (framing)
        {
            case (Framing.Ending):
                minBorderHeight = minBorderHeightDefault;
                canvasScaler = endingsCanvasScaler;
                break;
            case (Framing.MaskReveal):
                minBorderHeight = minBorderHeightThin;
                canvasScaler = maskRevealCanvasScaler;
                break;
            case (Framing.ElderIntro):
                minBorderHeight = minBorderHeightDefault;
                canvasScaler = elderIntroCanvasScaler;
                break;
            case (Framing.ElleniasHand):
                minBorderHeight = minBorderHeightDefault;
                canvasScaler = ElleniasHandCanvasScaler;
                break;
            case (Framing.IdsDead):
                minBorderHeight = minBorderHeightDefault;
                canvasScaler = IdsDeadCanvasScaler;
                break;
            case (Framing.TheSealing):
                minBorderHeight = minBorderHeightDefault;
                canvasScaler = theSealingCanvasScaler;
                break;
            case (Framing.SeaVignette):
                minBorderHeight = minBorderHeightDefault;
                canvasScaler = seaVignetteCanvasScaler;
                break;
            case (Framing.AwakeningPortraits):
                minBorderHeight = minBorderHeightDefault;
                canvasScaler = awakeningPortraitsCanvasScaler;
                break;
            
            // No dynamic sizing needed for framing
            // Useful for non-Full Screen Full Art where it's unnecessary to match up to an implied
            // Full Art viewport
            case (Framing.ConstantThin):
                minBorderHeight = minBorderHeightThin;
                canvasScaler = null;
                break;
            case (Framing.ConstantDefault):
                minBorderHeight = minBorderHeightDefault;
                canvasScaler = null;
                break;

            default:
                minBorderHeight = minBorderHeightDefault;
                canvasScaler = endingsCanvasScaler;
                break;
        }

        if (isNoAnimation)
        {
            if (isOpen)
            {
                isAnimatingLetterBox = false;
                isCutSceneLetterBox = true;
                CutSceneLetterBox(canvasScaler);
            }
            else
            {
                MatchBorders();
            }

            return;
        }
        
        if (isOpen)
            StartCoroutine(OpenLetterBoxCo(canvasScaler, t, cb, isUnscaledTime));
        else
            StartCoroutine(CloseLetterBoxCo(canvasScaler, t, cb, isUnscaledTime));
    }

    /// <summary>
    /// Match the Camera Rect
    /// </summary>
    public void MatchBorders()
    {
        isAnimatingLetterBox = false;
        isCutSceneLetterBox = false;
        
        SetBorders(0f, 0f);
    }

    private void ChangeBordersColor(bool isProd)
    {
        Image[] borders = GetComponentsInChildren<Image>();
        foreach (var border in borders)
        {
            border.color = isProd ? prodColor : devColor;
        }

        ForceUpdate();
    }

    // Set border width in pixels
    private void SetBorders(float width, float height)
    {
        Vector2 topBorderHeight = new Vector2(0f, cam.pixelRect.y + height);
        Vector2 sideBorderWidth = new Vector2(cam.pixelRect.x + width, 0f);
        
        topRect.sizeDelta = topBorderHeight;
        bottomRect.sizeDelta = topBorderHeight;
        leftRect.sizeDelta = sideBorderWidth;
        rightRect.sizeDelta = sideBorderWidth;

        ForceUpdate();
    }
    
    private void ForceUpdate()
    {
        topRect.ForceUpdateRectTransforms();
        bottomRect.ForceUpdateRectTransforms();
        leftRect.ForceUpdateRectTransforms();
        rightRect.ForceUpdateRectTransforms();
    }

    private void CutSceneLetterBox(Script_CanvasConstantPixelScaler canvasScaler)
    {
        
        Vector2 letterBox = GetLetterBox(canvasScaler);
        
        SetBorders(0f, letterBox.y);
    }

    private IEnumerator OpenLetterBoxCo(
        Script_CanvasConstantPixelScaler canvasScaler,
        float t,
        Action cb,
        bool isUnscaledTime
    )
    {
        Vector2 letterBox = GetLetterBox(canvasScaler);
        float height = 0f;

        isAnimatingLetterBox = true;

        while (height < letterBox.y)
        {
            var deltaTime = isUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            height += (Time.deltaTime / t) * letterBox.y;

            if (height > letterBox.y)
                height = letterBox.y;
            
            SetBorders(0f, height);

            yield return null;
        }

        isAnimatingLetterBox = false;
        isCutSceneLetterBox = true;
        
        if (cb != null)
            cb();
    }

    private IEnumerator CloseLetterBoxCo(
        Script_CanvasConstantPixelScaler canvasScaler,
        float t,
        Action cb,
        bool isUnscaledTime
    )
    {
        Vector2 letterBox = GetLetterBox(canvasScaler);
        float height = letterBox.y;

        isAnimatingLetterBox = true;
        isCutSceneLetterBox = false;

        while (height > 0f)
        {
            var deltaTime = isUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            height -= (Time.deltaTime / t) * letterBox.y;

            if (height < 0f)
                height = 0f;
            
            SetBorders(0f, height);

            yield return null;
        }

        isAnimatingLetterBox = false;
        
        if (cb != null)
            cb();   
    }

    /// <summary>
    /// Return the LetterBox that is the max between minHeight & verticalSpace where
    /// minHeight is an arbitrary border height and
    /// verticalSpace is the gap between the camera Rect and the reference Image
    /// (e.g. if the space between the ref Image and the bottom of camera Rect is
    /// greater than minHeight, this border will grow until it reaches the ref Image; otherwise,
    /// it will grow by minHeight)
    /// </summary>
    /// <param name="canvasScaler">Canvas Scaler to get the reference height and scale of the Image</param>
    /// <returns>Extra letterbox needed to letterbox the ref Image.</returns>
    private Vector2 GetLetterBox(Script_CanvasConstantPixelScaler canvasScaler)
    {
        currentCanvasScaler = canvasScaler;

        float letterBoxBorderHeight;
        float minHeight = minBorderHeight * graphics.PixelScreenSize.y;
        
        if (canvasScaler == null)
            letterBoxBorderHeight = minHeight;
        else
        {
            float imageSizeY = refResolution.y * canvasScaler.ScaleFactor;
            float verticalSpace = Mathf.Max((graphics.PixelScreenSize.y - imageSizeY) / 2, 0f);
            letterBoxBorderHeight = Application.isPlaying
                ? Mathf.CeilToInt(Mathf.Max(minHeight, verticalSpace))
                : minHeight;
        }

        return new Vector2(0f, letterBoxBorderHeight);
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
        
        if (!Debug.isDebugBuild)
            ChangeBordersColor(true);
        
        gameObject.SetActive(true);
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_UIAspectRatioEnforcerFrame))]
    public class Script_UIAspectRatioEnforcerFrameTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_UIAspectRatioEnforcerFrame t = (Script_UIAspectRatioEnforcerFrame)target;
            
            if (GUILayout.Button("Match Borders"))
            {
                t.MatchBorders();
            }

            if (GUILayout.Button("Borders Black"))
            {
                t.ChangeBordersColor(isProd: true);
            }

            if (GUILayout.Button("Borders Dev"))
            {
                t.ChangeBordersColor(isProd: false);
            }

            GUILayout.Label("Dynamic Framing Size", EditorStyles.boldLabel);

            if (GUILayout.Button("Letter Box (The Sealing Canvas)"))
            {
                t.EndingsLetterBox(true, framing: Framing.TheSealing, isNoAnimation: true);
            }

            if (GUILayout.Button("Open Letter Box (Endings Canvas)"))
            {
                if (!Application.isPlaying)
                    return;
                
                t.EndingsLetterBox(true, framing: Framing.Ending);
            }

            if (GUILayout.Button("Close Letter Box (Endings Canvas)"))
            {
                if (!Application.isPlaying)
                    return;
                
                t.EndingsLetterBox(false, framing: Framing.Ending);
            }

            GUILayout.Label("Constant Framing Size", EditorStyles.boldLabel);

            if (GUILayout.Button("Letter Box No Anim (Constant Thin)"))
            {
                t.EndingsLetterBox(true, framing: Framing.ConstantThin, isNoAnimation: true);
            }

            if (GUILayout.Button("Open Letter Box (Constant Thin)"))
            {
                if (!Application.isPlaying)
                    return;
                
                t.EndingsLetterBox(true, framing: Framing.ConstantThin);
            }

            if (GUILayout.Button("Open Letter Box (Constant Default)"))
            {
                if (!Application.isPlaying)
                    return;
                
                t.EndingsLetterBox(true, framing: Framing.ConstantDefault);
            }

            if (GUILayout.Button("Close Letter Box (Constant Default)"))
            {
                if (!Application.isPlaying)
                    return;
                
                t.EndingsLetterBox(false, framing: Framing.ConstantThin);
            }
        }
    }
#endif
}
