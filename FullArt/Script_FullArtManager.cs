using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Handles fullArts
/// Uses a global transparent bg
/// </summary>
public enum FullArtPortrait
{
    None = 0,
    Moose = 1
}

public class Script_FullArtManager : MonoBehaviour
{
    public enum FullArtState
    {
        DialogueManager,
        InteractableObject,
        Inventory,
        Timeline
    }
    
    public enum Bgs
    {
        Default = 0,
        Black = 1
    }
    
    public static Script_FullArtManager Control;
    
    [SerializeField] private int DefaultSortingOrder;
    [SerializeField] private int ExamineSortingOrder;

    public FullArtState state;
    /// <summary>
    /// Track who is controlling the fullArt canvases to prevent stacking calls
    /// </summary>
    
    [SerializeField] private Script_FullArtBgCanvasGroup[] bgs;
    public Script_FullArt activeFullArt { get; private set; }
    
    public CanvasGroup fullArtCanvasGroup;
    public Canvas fullArtParent;
    public CanvasGroup fullArtBgCanvasGroup;
    
    public Image fullArtImage;
    
    [SerializeField] private float bgAlpha;
    [SerializeField] private Transform[] fullArtParentsToSetActive;
    [SerializeField] private Script_CanvasGroupController bgForceBlack;

    /// <summary>
    /// Alternate way to reference Full Arts via Dialogue Nodes
    /// </summary>
    [Space][Header("Full Art Overrides")][Space]
    [SerializeField] private Script_FullArt MooseFullArt;

    private Coroutine fullArtCoroutine;
    private Coroutine bgCoroutine;
    
    void Awake()
    {
        var canvas = fullArtParent.GetComponent<Canvas>();
        if (canvas != null && canvas.sortingOrder != DefaultSortingOrder)
            Debug.LogWarning($"FullArtCanvas Sorting Order <{canvas.sortingOrder}> != DefaulSortingOrder");
    }
    
    public Script_FullArt GetFullArt(FullArtPortrait fullArtType) => fullArtType switch
    {
        FullArtPortrait.Moose => MooseFullArt,
        _ => null,
    };
    
    /// <summary>
    /// fades out fullArt canvases and does callback after finishing fadeOut
    /// </summary>
    public void Hide(FadeSpeeds fadeOutMode, Action cb)
    {
        float fadeOutTime = Script_Utils.GetFadeTime(fadeOutMode); 
        StartCoroutine(
            fullArtCanvasGroup
                .GetComponent<Script_CanvasGroupFadeInOut>()
                .FadeOutCo(fadeOutTime, () =>
                {
                    fullArtCanvasGroup.gameObject.SetActive(false);
                    
                    foreach (var bg in bgs)
                        bg.gameObject.SetActive(false);

                    fullArtImage.gameObject.SetActive(false);
                    fullArtImage.sprite = null;
                    
                    if (cb != null)
                        cb();
                }
            )
        );
    }

    public void ShowFullArt(
        Script_FullArt fullArt,
        FadeSpeeds fadeInSpeed,
        Action cb,
        FullArtState _state
    )
    {   
        state = _state;
        
        HandleFullArtSortingOrder();
        
        fullArtCanvasGroup.alpha = 1;
        fullArtCanvasGroup.gameObject.SetActive(true);
        
        fullArt.Setup();
        fullArt.gameObject.SetActive(true);

        // fade in
        activeFullArt = fullArt;
        fullArt.FadeIn(out fullArtCoroutine, fadeInSpeed, cb);
        
        // fade in global bg
        Script_FullArtBgCanvasGroup bg = bgs[(int)fullArt.bg];
        bg.gameObject.SetActive(true);
        bg.FadeIn(out bgCoroutine, fadeInSpeed, null, bgAlpha);
    }
    
    /// <summary>
    /// fade out the designated prefab canvas
    /// this ends the viewing state,
    /// use TransitionOutFullArt instead for transitioning to next 
    /// </summary>
    public void HideFullArt(
        Script_FullArt fullArt,
        FadeSpeeds fadeOutSpeed,
        Action cb
    )
    {
        Dev_Logger.Debug("HideFullArt() called");
        // fade out global bg
        Script_FullArtBgCanvasGroup bg = bgs[(int)fullArt.bg];
        bg.FadeOut(fadeOutSpeed, () => bg.gameObject.SetActive(false));
        
        fullArt.FadeOut(fadeOutSpeed, () =>
        {
            CloseCanvasGroup(fullArtCanvasGroup);
            
            if (Script_Game.Game.GetPlayer().State == Const_States_Player.Viewing)
                Script_Game.Game.GetPlayer().SetIsInteract();

            activeFullArt = null;
            
            if (cb != null)
                cb();
        });
    }

    public void TransitionOutFullArt(
        Script_FullArt fullArt,
        FadeSpeeds fadeOutSpeed,
        Action cb
    )
    {
        fullArt.FadeOut(fadeOutSpeed, () =>
        {
            if (cb != null)     cb();
        });
    }

    public void OpenBgForceBlack()
    {
        bgForceBlack.Open();
    }

    public void CloseBgForceBlack()
    {
        bgForceBlack.Close();
    }

    public void SetForceBlackAlpha(float alpha)
    {
        bgForceBlack.Alpha = alpha;
    }

    /// <summary>
    /// Use to prepare full art for right entrance
    /// </summary>
    public void EntranceFromRight(
        Script_FullArt fullArt
    )
    {
        /// First have gameObject active because state machine resets when is turned on/off
        /// but still hidden
        fullArtCanvasGroup.alpha = 0f;
        fullArtCanvasGroup.gameObject.SetActive(true);
        fullArt.gameObject.SetActive(true);

        fullArt.EntranceFromRight(fullArtCanvasGroup);
    }

    /// <summary>
    /// Change Sorting Order depending on where the Full Art is being displayed (e.g. if it is
    /// during an examine within Inventory it needs to show on top of Menu)
    /// </summary>
    private void HandleFullArtSortingOrder()
    {
        switch (state)
        {
            case (FullArtState.Inventory):
                fullArtParent.sortingOrder = ExamineSortingOrder;
                break;
            default: 
                fullArtParent.sortingOrder = DefaultSortingOrder;
                break;
        }
    }

    private void CloseCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(false);
    }
    
    // Note: after initialization, full arts are never closed again, just their alphas are adjusted.
    public void InitialState(Script_FullArt fullArt)
    {
        Script_FullArtBgCanvasGroup bg = bgs[(int)fullArt.bg];
        StopFullArtCoroutines(fullArt);
        
        bg.Initialize();
        bg.gameObject.SetActive(false);

        fullArt.Close();
        CloseCanvasGroup(fullArtCanvasGroup);
        activeFullArt = null;

        void StopFullArtCoroutines(Script_FullArt fullArt)
        {
            fullArt.StopMyCoroutineRef(ref fullArtCoroutine);
            bg.StopMyCoroutineRef(ref bgCoroutine);
        }
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
        
        Hide(FadeSpeeds.None, null);
        fullArtBgCanvasGroup.gameObject.SetActive(true);
        bgForceBlack.Close();
        
        foreach (Script_FullArtBgCanvasGroup bg in bgs)
        {
            bg.gameObject.SetActive(true);
            bg.InitializeState();
        }

        foreach (Transform t in fullArtParent.GetComponentsInChildren<Transform>(true))
        {
            Script_FullArt fa = t.GetComponent<Script_FullArt>();
            if (fa != null)
            {
                fa.gameObject.SetActive(false);
            }
        }

        foreach (Transform t in fullArtParentsToSetActive)
        {
            t.gameObject.SetActive(true);
        }
    }
}
