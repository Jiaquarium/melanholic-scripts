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
    Moose = 1,
    Ellenia = 10,
    ElleniaAngry = 11,
    ElleniaAngrySmile = 12,
    ElleniaHmph = 13,
    ElleniaSmug = 14,
    ElleniaPensive = 15,
    ElleniaPensiveAdmit = 16,
    ElleniaPensiveAngry = 17,
    ElleniaPensiveTearingUp1 = 18,
    ElleniaPensiveTearingUp2 = 19,
    Eileen = 20,
    EileenUneasy = 21,
    EileenFocused = 22,
    Ids = 30,
    IdsSerious = 31,
    IdsPensive = 32,
    IdsTroubled = 33,
    IdsPossessed = 34,
    IdsHesitant = 35,
    IdsGladBashful = 36,
    IdsSuperHappy = 37,
    IdsSuperHappyBashful = 38,
    IdsSusPositive = 39,
    IdsPossessedPositive = 40,
    Myne = 50,
    MyneGlare = 51,
    MyneFeignedConcern = 52,
    MyneConfident = 53,
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
    /// Alternate way to reference Full Arts via Dialogue Nodes of using enum FullArtPortrait instead
    /// of referencing the object (useful for scriptable data e.g. prefabs, scriptable objects)
    /// </summary>
    [Space][Header("Full Art Overrides")][Space]
    [SerializeField] private Script_FullArt MooseFullArt;

    [SerializeField] private Script_FullArt ElleniaFullArt;
    [SerializeField] private Script_FullArt ElleniaAngryFullArt;
    [SerializeField] private Script_FullArt ElleniaAngrySmileFullArt;
    [SerializeField] private Script_FullArt ElleniaHmphFullArt;
    [SerializeField] private Script_FullArt ElleniaSmugFullArt;
    [SerializeField] private Script_FullArt ElleniaPensiveFullArt;
    [SerializeField] private Script_FullArt ElleniaPensiveAdmitFullArt;
    [SerializeField] private Script_FullArt ElleniaPensiveAngryFullArt;
    [SerializeField] private Script_FullArt ElleniaPensiveTearingUp1FullArt;
    [SerializeField] private Script_FullArt ElleniaPensiveTearingUp2FullArt;

    [SerializeField] private Script_FullArt EileenFullArt;
    [SerializeField] private Script_FullArt EileenUneasyFullArt;
    [SerializeField] private Script_FullArt EileenFocusedFullArt;

    [SerializeField] private Script_FullArt IdsFullArt;
    [SerializeField] private Script_FullArt IdsSeriousFullArt;
    [SerializeField] private Script_FullArt IdsPensiveFullArt;
    [SerializeField] private Script_FullArt IdsTroubledFullArt;
    [SerializeField] private Script_FullArt IdsPossessedFullArt;
    [SerializeField] private Script_FullArt IdsHesitant;
    [SerializeField] private Script_FullArt IdsGladBashfulFullArt;
    [SerializeField] private Script_FullArt IdsSuperHappyFullArt;
    [SerializeField] private Script_FullArt IdsSuperHappyBashfulFullArt;
    [SerializeField] private Script_FullArt IdsSusPositiveFullArt;
    [SerializeField] private Script_FullArt IdsPossessedPositiveFullArt;

    private Coroutine fullArtCoroutine;
    private Coroutine bgCoroutine;
    
    void Awake()
    {
        var canvas = fullArtParent.GetComponent<Canvas>();
        if (canvas != null && canvas.sortingOrder != DefaultSortingOrder)
            Debug.LogWarning($"FullArtCanvas Sorting Order <{canvas.sortingOrder}> != DefaulSortingOrder");
    }
    
    public Script_FullArt GetFullArt(FullArtPortrait fullArtType, out bool isMynesMirrorNode)
    {
        switch (fullArtType)
        {
            case FullArtPortrait.Myne:
            case FullArtPortrait.MyneGlare:
            case FullArtPortrait.MyneFeignedConcern:
            case FullArtPortrait.MyneConfident:
                isMynesMirrorNode = true;
                return null;
            default:
                isMynesMirrorNode = false;
                return GetDefaultFullArt(fullArtType);
        }
        
        Script_FullArt GetDefaultFullArt(FullArtPortrait fullArtType) => fullArtType switch
        {
            FullArtPortrait.Moose => MooseFullArt,
            FullArtPortrait.Ellenia => ElleniaFullArt,
            FullArtPortrait.ElleniaAngry => ElleniaAngryFullArt,
            FullArtPortrait.ElleniaAngrySmile => ElleniaAngrySmileFullArt,
            FullArtPortrait.ElleniaHmph => ElleniaHmphFullArt,
            FullArtPortrait.ElleniaSmug => ElleniaSmugFullArt,
            FullArtPortrait.ElleniaPensive => ElleniaPensiveFullArt,
            FullArtPortrait.ElleniaPensiveAdmit => ElleniaPensiveAdmitFullArt,
            FullArtPortrait.ElleniaPensiveAngry => ElleniaPensiveAngryFullArt,
            FullArtPortrait.ElleniaPensiveTearingUp1 => ElleniaPensiveTearingUp1FullArt,
            FullArtPortrait.ElleniaPensiveTearingUp2 => ElleniaPensiveTearingUp2FullArt,
            
            FullArtPortrait.Eileen => EileenFullArt,
            FullArtPortrait.EileenUneasy => EileenUneasyFullArt,
            FullArtPortrait.EileenFocused => EileenFocusedFullArt,

            FullArtPortrait.Ids => IdsFullArt,
            FullArtPortrait.IdsSerious => IdsSeriousFullArt,
            FullArtPortrait.IdsPensive => IdsPensiveFullArt,
            FullArtPortrait.IdsTroubled => IdsTroubledFullArt,
            FullArtPortrait.IdsPossessed => IdsPossessedFullArt,
            FullArtPortrait.IdsHesitant => IdsHesitant,
            FullArtPortrait.IdsGladBashful => IdsGladBashfulFullArt,
            FullArtPortrait.IdsSuperHappy => IdsSuperHappyFullArt,
            FullArtPortrait.IdsSuperHappyBashful => IdsSuperHappyBashfulFullArt,
            FullArtPortrait.IdsSusPositive => IdsSusPositiveFullArt,
            FullArtPortrait.IdsPossessedPositive => IdsPossessedPositiveFullArt,

            _ => null,
        };
    }
    
    
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
        
        fullArtCanvasGroup.alpha = 1f;
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

    public void OpenFullArt(Script_FullArt fullArt, FullArtState _state)
    {   
        state = _state;
        
        HandleFullArtSortingOrder();
        
        fullArtCanvasGroup.alpha = 1f;
        fullArtCanvasGroup.gameObject.SetActive(true);
        
        fullArt.Setup();
        fullArt.gameObject.SetActive(true);

        // Show full art
        activeFullArt = fullArt;
        fullArt.Open();
        
        // Show global bg
        Script_FullArtBgCanvasGroup bg = bgs[(int)fullArt.bg];
        bg.Open();
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

    /// <summary>
    /// Note: this will not handle Player state like default Hide
    /// </summary>
    public void CloseFullArt(Script_FullArt fullArt)
    {
        // Close global bg
        Script_FullArtBgCanvasGroup bg = bgs[(int)fullArt.bg];
        bg.Close();
        
        fullArt.Close();
        CloseCanvasGroup(fullArtCanvasGroup);

        activeFullArt = null;
    }

    /// <summary>
    /// Used to fade out a Full Art, accompanied by a call to transition in a new Full Art.
    /// Note: (When used syncronously) fadeOutSpeed must match fadeInSpeed of new Full Art being transitioned to.
    /// </summary>
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
