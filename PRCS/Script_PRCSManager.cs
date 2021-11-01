﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Pre-Rendered Cut Scenes Manager (Singleton)
/// Talking Self Cut Scenes
/// 
/// Custom PRCS's fade in is NOT automatic, and defined in the Timeline instead.
/// Can adjust Sort Order per canvas too.
/// Allows for more freedom if it needs to be adjusted later.
/// </summary>
public class Script_PRCSManager : MonoBehaviour
{
    public static Script_PRCSManager Control;
    [SerializeField] private CanvasGroup PRCSCanvasGroup;
    [SerializeField] private Canvas PRCSCanvas;

    [UnityEngine.Serialization.FormerlySerializedAs("TimelinePRCSCanvasGroup")]
    [SerializeField] private Script_CanvasGroupController KingsIntroCanvasGroup;
    [SerializeField] private Script_AwakeningCanvasGroupController AwakeningCanvasGroup;
    [SerializeField] private Script_FaceOffCanvasGroupController[] FaceOffCanvasGroups;
    [SerializeField] private float glitchFadeInTime = 5f;
    [SerializeField] private float glitchFadeOutTime = 2f;
    
    [SerializeField] private Transform customCanvasesParent;
    [SerializeField] private Canvas[] customCanvases;
    
    [SerializeField] private Script_PRCS MynesMirrorPRCS;
    [SerializeField] private Script_PRCS ElleniasHandPRCS;
    [SerializeField] private Script_PRCS toWeekendPRCS;
    [SerializeField] private Script_PRCS IdsDeadPRCS;

    [SerializeField] private Script_GlitchFXManager glitchManager;

    [SerializeField] private Script_TimelineController faceOffTimelineController;
    [SerializeField] private int targetEquipmentSlot;
    [SerializeField] private float waitTimeAfterFaceOff;

    [SerializeField] private Script_Game game;

    private Action onFaceOffTimelineDoneAction;

    public int FaceOffCounter
    {
        get => game.faceOffCounter;
        set => game.faceOffCounter = value;
    }
    
    /// <summary>
    /// More specific canvases for special behavior
    /// </summary>
    public enum CustomTypes
    {
        None,
        MynesMirror,
        MynesMirrorMidConvo,
        ElleniasHand,
        ToWeekend,
        IdsDead,
    }

    void OnValidate()
    {
        customCanvases = customCanvasesParent.GetComponentsInChildren<Canvas>(true);
    }

    public void Start()
    {
        Initialize();
    }

    public void ShowPRCS(
        Script_PRCS PRCS,
        FadeSpeeds fadeInSpeed,
        Action cb
    )
    {
        PRCSCanvasGroup.alpha = 1f;
        PRCSCanvasGroup.gameObject.SetActive(true);

        PRCS.Setup();
        PRCS.gameObject.SetActive(true);
        PRCS.FadeIn(fadeInSpeed, cb);
    }

    public void HidePRCS(
        Script_PRCS PRCS,
        FadeSpeeds fadeOutSpeed,
        Action cb 
    )
    {
        PRCS.FadeOut(fadeOutSpeed, () =>
        {
            Initialize();
            if (Script_Game.Game.GetPlayer().State == Const_States_Player.Viewing)
                Script_Game.Game.GetPlayer().SetIsInteract();

            if (cb != null)     cb();
        });
    }

    public void OpenPRCSNoFade(Script_PRCS PRCS)
    {
        PRCSCanvasGroup.alpha = 1f;
        PRCSCanvasGroup.gameObject.SetActive(true);

        PRCS.Setup();
        PRCS.Open();   
    }

    public void ClosePRCSNoFade(Script_PRCS PRCS)
    {
        PRCS.Close();
        
        PRCSCanvasGroup.alpha = 0f;
        PRCSCanvasGroup.gameObject.SetActive(false);
    }

    public void OpenPRCSCustom(CustomTypes type)
    {
        switch (type)
        {
            case CustomTypes.MynesMirror:
                PRCSCanvasGroup.alpha = 1f;
                PRCSCanvasGroup.gameObject.SetActive(true);
                
                MynesMirrorPRCS.Setup();
                MynesMirrorPRCS.Open();
                MynesMirrorPRCS.PlayTimeline(0);
                // MynesMirror Timeline controls Fade
                break;
                
            case CustomTypes.MynesMirrorMidConvo:
                PRCSCanvasGroup.alpha = 1f;
                PRCSCanvasGroup.gameObject.SetActive(true);
                
                // Set Myne's Mirror CanvasGroup alpha to avoid flicker
                MynesMirrorPRCS.Setup();
                MynesMirrorPRCS.Open();
                MynesMirrorPRCS.PlayTimeline(1);
                break;
            
            case CustomTypes.ElleniasHand:
                PRCSCanvasGroup.alpha = 1f;
                PRCSCanvasGroup.gameObject.SetActive(true);
                
                ElleniasHandPRCS.Setup();
                ElleniasHandPRCS.Open();
                ElleniasHandPRCS.PlayTimeline(0);
                break;
            
            case CustomTypes.ToWeekend:
                PRCSCanvasGroup.alpha = 1f;
                PRCSCanvasGroup.gameObject.SetActive(true);
                
                toWeekendPRCS.Setup();
                toWeekendPRCS.Open();
                toWeekendPRCS.PlayTimeline(0);
                break;
            
            case CustomTypes.IdsDead:
                PRCSCanvasGroup.alpha = 1f;
                PRCSCanvasGroup.gameObject.SetActive(true);
                
                IdsDeadPRCS.Setup();
                IdsDeadPRCS.Open();
                IdsDeadPRCS.PlayTimeline(0);
                break;

            default:
                break;
        }
    }

    public void ClosePRCSCustom(CustomTypes type, Action cb = null)
    {
        switch (type)
        {
            case CustomTypes.MynesMirror:
                HidePRCS(MynesMirrorPRCS, FadeSpeeds.Slow, cb);
                break;
            
            case CustomTypes.ElleniasHand:
                HidePRCS(ElleniasHandPRCS, FadeSpeeds.Slow, cb);
                break;
            
            case CustomTypes.ToWeekend:
                HidePRCS(toWeekendPRCS, FadeSpeeds.None, cb);
                break;
            
            case CustomTypes.IdsDead:
                HidePRCS(IdsDeadPRCS, FadeSpeeds.Slow, cb);
                break;
                
            default:
                break;
        }
    }

    public void SetAwakeningActive(bool isActive)
    {
        if (isActive)
            AwakeningCanvasGroup.Open();
        else
            AwakeningCanvasGroup.Close();
    }

    public void SetFaceOffActive(int i, bool isActive)
    {
        if (isActive)
            FaceOffCanvasGroups[i].Open();
        else
            FaceOffCanvasGroups[i].Close();
    }

    public void TalkingSelfSequence(Action cb)
    {
        glitchManager.SetHigh();
        glitchManager.BlendTo(1f, glitchFadeInTime, () => Script_TransitionManager.Control.FadeInCoroutine(
            Script_TransitionManager.FadeTimeSlow,
            () => FadeInStartTimeline()
        ));

        void FadeInStartTimeline()
        {
            Debug.Log("Setting glitch manager to default, blend 0");
            glitchManager.SetDefault();
            glitchManager.SetBlend(0f);
            
            Script_TransitionManager.Control.FadeOutCoroutine(Script_TransitionManager.FadeTimeSlow);
            
            if (cb != null)
                cb();
        }
    }

    // Pass an action that will be called at the end of the faceOff Timeline.
    public void PlayFaceOffTimeline(Action onTimelineDoneAction)
    {
        onFaceOffTimelineDoneAction = onTimelineDoneAction;
        
        SetFaceOffActive(FaceOffCounter, true);
        
        var clampedCounter = Mathf.Clamp(FaceOffCounter, 0, faceOffTimelineController.timelines.Count - 1);
        faceOffTimelineController.PlayableDirectorPlayFromTimelines(0, clampedCounter);
    }

    private void SwitchLastElevatorMaskBackground()
    {
        game.EquipLastElevatorMaskBackground(isGive: false);
    }

    // ----------------------------------------------------------------------
    // Timeline Signals

    // FaceOffDirector: FaceOffTimeline0
    // FaceOffDirector: FaceOffTimeline1
    // FaceOffDirector: FaceOffTimeline2
    public void OnFaceOffTimelineDone()
    {
        game.ChangeStateCutScene();
        
        // Set Fade Canvas Active
        Script_TransitionManager.Control.TimelineBlackScreen();

        SetFaceOffActive(FaceOffCounter, false);
        
        // Wait a few seconds for black screen to stay up and then fade out.
        SwitchLastElevatorMaskBackground();

        glitchManager.SetLow();
        glitchManager.SetBlend(1f);

        StartCoroutine(WaitToFadeOut());

        IEnumerator WaitToFadeOut()
        {
            yield return new WaitForSeconds(waitTimeAfterFaceOff);

            Script_TransitionManager.Control.TimelineFadeOut(
                Script_TransitionManager.FadeTimeSlow,
                () => {
                    if (onFaceOffTimelineDoneAction != null)
                        onFaceOffTimelineDoneAction();
                    
                    onFaceOffTimelineDoneAction = null;
                    FaceOffCounter = FaceOffCounter + 1;

                    glitchManager.BlendTo(0f, glitchFadeOutTime);
                }
            );            
        }
    }

    // ----------------------------------------------------------------------

    public void Initialize()
    {
        /// Hide CanvasGroup but ensure the PRCS canvas and ready to use
        PRCSCanvasGroup.gameObject.SetActive(false);
        PRCSCanvasGroup.alpha = 0f;
        PRCSCanvas.gameObject.SetActive(true);

        customCanvasesParent.gameObject.SetActive(true);
        foreach (Canvas c in customCanvases)    c.gameObject.SetActive(true);

        Script_PRCS []allPRCS = PRCSCanvasGroup.GetComponentsInChildren<Script_PRCS>(true);
        foreach (Script_PRCS prcs in allPRCS)
        {
            prcs.gameObject.SetActive(false);
        }

        // Initialize purely customized PRCS
        KingsIntroCanvasGroup.InitialState();
        KingsIntroCanvasGroup.gameObject.SetActive(true);

        AwakeningCanvasGroup.InitialState();
        AwakeningCanvasGroup.gameObject.SetActive(false);

        foreach (var canvasGroup in FaceOffCanvasGroups)
        {
            canvasGroup.InitialState();
            canvasGroup.gameObject.SetActive(false);
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
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Script_PRCSManager))]
    public class Script_PRCSManagerTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_PRCSManager t = (Script_PRCSManager)target;
            if (GUILayout.Button("Play Face Off Timeline"))
            {
                t.PlayFaceOffTimeline(null);
            }

            if (GUILayout.Button("Switch to Last Elevator Background"))
            {
                t.SwitchLastElevatorMaskBackground();
            }
        }
    }
    #endif
}
