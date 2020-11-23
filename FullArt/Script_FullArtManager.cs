using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Handles fullArts
/// Uses a global transparent bg
/// </summary>
public class Script_FullArtManager : MonoBehaviour
{
    public static Script_FullArtManager Control;
    public FullArtState state;
    /// <summary>
    /// Track who is controlling the fullArt canvases to prevent stacking calls
    /// </summary>
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
    [SerializeField] private Script_FullArtBgCanvasGroup[] bgs;
    public Script_FullArt activeFullArt { get; private set; }
    public CanvasGroup fullArtCanvas;
    public Canvas fullArtParent;
    public CanvasGroup fullArtBgCanvasGroup;
    public Image fullArtImage;
    private bool shouldSetNativeSize;
    [SerializeField] private float bgAlpha;
    [SerializeField] private Transform[] fullArtParentsToSetActive;
    
    void Awake()
    {
        if (Control == null)
        {
            Control = this;
        }
        else if (Control != this)
        {
            Destroy(this.gameObject);
        }
    }
    
    /// <summary>
    /// fades out fullArt canvases and does callback after finishing fadeOut
    /// </summary>
    public void Hide(FadeSpeeds fadeOutMode, Action cb)
    {
        float fadeOutTime = Script_GraphicsManager.GetFadeTime(fadeOutMode); 
        StartCoroutine(
            fullArtCanvas
                .GetComponent<Script_CanvasGroupFadeInOut>()
                .FadeOutCo(fadeOutTime, () =>
                {
                    fullArtCanvas.gameObject.SetActive(false);
                    
                    foreach (var bg in bgs) bg.gameObject.SetActive(false);

                    fullArtImage.gameObject.SetActive(false);
                    fullArtImage.sprite = null;
                    if (cb != null)     cb();
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
        // inventory is the master non-interactive state for player
        // player should only be able to go to lastState from inventory state
        // DialogueManager doesn't set the player to viewing
        if (
            Script_Game.Game.GetPlayer().GetIsInteract()
            && state != FullArtState.DialogueManager
        )
        {
            Script_Game.Game.GetPlayer().SetIsViewing();
        }
        
        fullArtCanvas.alpha = 1;
        fullArtCanvas.gameObject.SetActive(true);
        
        fullArt.Setup();
        fullArt.gameObject.SetActive(true);

        // fade in
        activeFullArt = fullArt;
        fullArt.FadeIn(fadeInSpeed, cb);
        
        // fade in global bg
        Script_FullArtBgCanvasGroup bg = bgs[(int)fullArt.bg];
        bg.gameObject.SetActive(true);
        bg.FadeIn(fadeInSpeed, null, bgAlpha);
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
        Debug.Log("HideFullArt() called");
        // fade out global bg
        Script_FullArtBgCanvasGroup bg = bgs[(int)fullArt.bg];
        bg.FadeOut(fadeOutSpeed, () => bg.gameObject.SetActive(false));
        
        fullArt.FadeOut(fadeOutSpeed, () =>
        {
            fullArtCanvas.alpha = 0f;
            fullArtCanvas.gameObject.SetActive(false);
            if (Script_Game.Game.GetPlayer().GetIsViewing())
                Script_Game.Game.GetPlayer().SetIsInteract();

            activeFullArt = null;
            if (cb != null)     cb();
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

    /// <summary>
    /// Use to prepare full art for right entrance
    /// </summary>
    public void EntranceFromRight(
        Script_FullArt fullArt
    )
    {
        /// First have gameObject active because state machine resets when is turned on/off
        /// but still hidden
        fullArtCanvas.alpha = 0f;
        fullArtCanvas.gameObject.SetActive(true);
        fullArt.gameObject.SetActive(true);

        fullArt.EntranceFromRight(fullArtCanvas);
    }
    
    public void Setup()
    {
        Hide(FadeSpeeds.None, null);
        fullArtBgCanvasGroup.gameObject.SetActive(true);
        
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
