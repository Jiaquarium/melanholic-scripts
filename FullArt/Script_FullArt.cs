using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Should only be called from manager as it will set the BG
/// Serves as a Canvas Group Controller
/// </summary>
[RequireComponent(typeof(Script_CanvasGroupFadeInOut))]
[RequireComponent(typeof(CanvasGroup))]
public class Script_FullArt : MonoBehaviour
{
    private static int IsOffScreen = Animator.StringToHash("IsOffScreen");
    private static int EntranceFromRightTrigger = Animator.StringToHash("EntranceFromRight");
    
    public Script_FullArt nextFullArt;
    public FadeSpeeds fadeInSpeed; /// used for Examine
    public FadeSpeeds nextFadeSpeed; /// used for Examine
    public Script_FullArtManager.Bgs bg;
    [SerializeField] private Script_CanvasGroupController myBg;
    [SerializeField] private Animator animator;

    [Tooltip("Option to specify a custom bounds for this UI object. Can be Set by parent with FullArtBoundsParent.")]
    [SerializeField] private Script_ScalingBounds customBounds;
    private Script_CanvasConstantPixelScaler canvasScaler;
    
    void OnEnable()
    {
        if (myBg != null)
            myBg.Close();
    }
    
    void Awake()
    {
        canvasScaler = GetComponentInParent<Script_CanvasConstantPixelScaler>();
    }
    
    /// <summary>
    /// Show without fade and no callbacks
    /// </summary>
    public void Open()
    {
        CanvasGroup c = GetComponent<CanvasGroup>();
        c.alpha = 1f;
        c.gameObject.SetActive(true);
    }
    /// <summary>
    /// Close without fade and no callbacks
    /// </summary>
    public virtual void Close()
    {
        CanvasGroup c = GetComponent<CanvasGroup>();
        c.alpha = 0f;
        c.gameObject.SetActive(false);
    }
    
    public void FadeIn(FadeSpeeds fadeSpeed, Action cb)
    {
        float fadeInTime = Script_Utils.GetFadeTime(fadeSpeed);
        StartCoroutine(
            GetComponent<Script_CanvasGroupFadeInOut>()
                .FadeInCo(fadeInTime, () =>
                {
                    if (cb != null)     cb();
                }
            )
        );
    }

    public void FadeOut(FadeSpeeds fadeSpeed, Action cb)
    {
        float fadeOutTime = Script_Utils.GetFadeTime(fadeSpeed);
        StartCoroutine(
            GetComponent<Script_CanvasGroupFadeInOut>()
                .FadeOutCo(fadeOutTime, () =>
                {
                    if (cb != null)     cb();
                }
            )
        );
    }

    public void EntranceFromRight(CanvasGroup fullArtCanvasGroup)
    {
        /// Manager handles making this full art active
        
        animator.SetBool(IsOffScreen, true);
        animator.SetTrigger(EntranceFromRightTrigger);
        
        /// Reveal the canvas group after animation already underway
        fullArtCanvasGroup.alpha = 1f;

        StartCoroutine(WaitToResetAnimator());

        IEnumerator WaitToResetAnimator()
        {
            yield return null;
            animator.SetBool(IsOffScreen, false);
        }
    }

    public void FadeInMyBg(float t, float alpha = 1f)
    {
        // Ensure Image is fully opaque before fading CanvasGroup
        Image myBgImg = myBg.GetComponent<Image>();
        var myBgColor = myBgImg.color;
        myBgImg.color = new Color(myBgColor.r, myBgColor.g, myBgColor.b, 1f);
        
        myBg.FadeIn(t, isForceMaxAlpha: true, fadeToAlpha: alpha);
    }

    public virtual void Setup()
    {
        GetComponent<Script_CanvasGroupFadeInOut>().Initialize();
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_FullArt))]
    public class Script_FullArtTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_FullArt t = (Script_FullArt)target;
            
            if (GUILayout.Button("Fade In My BG"))
            {
                t.FadeInMyBg(2f);
            }
        }
    }
#endif
}
