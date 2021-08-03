using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

/// <summary>
/// Should only be called from manager as it will set the BG
/// Serves as a Canvas Group Controller
/// </summary>
[RequireComponent(typeof(Script_CanvasGroupFadeInOut))]
[RequireComponent(typeof(CanvasGroup))]
public class Script_FullArt : MonoBehaviour
{
    public Script_FullArt nextFullArt;
    public FadeSpeeds fadeInSpeed; /// used for Examine
    public FadeSpeeds nextFadeSpeed; /// used for Examine
    public Script_FullArtManager.Bgs bg;
    [SerializeField] private Animator animator;
    
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
        
        animator.SetBool("IsOffScreen", true);
        animator.SetTrigger("EntranceFromRight");
        
        /// Reveal the canvas group after animation already underway
        fullArtCanvasGroup.alpha = 1f;

        StartCoroutine(WaitToResetAnimator());

        IEnumerator WaitToResetAnimator()
        {
            yield return null;
            animator.SetBool("IsOffScreen", false);
        }
    }

    public virtual void Setup()
    {
        GetComponent<Script_CanvasGroupFadeInOut>().Initialize();
    }
}
