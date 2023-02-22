using System.Collections;
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
        Sea
    }
    
    public static Script_PRCSManager Control;
    [SerializeField] private CanvasGroup PRCSCanvasGroup;
    [SerializeField] private Canvas PRCSCanvas;
    [SerializeField] private Script_PRCS wellsJustOpenedPRCS;

    [UnityEngine.Serialization.FormerlySerializedAs("TimelinePRCSCanvasGroup")]
    [SerializeField] private Script_CanvasGroupController KingsIntroCanvasGroup;
    [SerializeField] private Script_AwakeningPortraitsController AwakeningPortraitsController;
    [SerializeField] private Script_CanvasGroupController ElleniaStabsCanvasGroup;
    [SerializeField] private Script_AwakeningCanvasGroupController AwakeningCanvasGroup;
    [SerializeField] private Script_AwakeningCanvasGroupController AwakeningFinalCanvasGroup;
    [SerializeField] private Script_FaceOffCanvasGroupController[] FaceOffCanvasGroups;
    [SerializeField] private float glitchFadeInTime = 5f;
    [SerializeField] private float glitchFadeOutTime = 2f;
    
    // ------------------------------------------------------------------
    // Face Off Only
    
    [SerializeField] private Script_BgThemePlayer faceOffBgThemePlayer;
    
    // ------------------------------------------------------------------
    
    [SerializeField] private Transform customCanvasesParent;
    [SerializeField] private Canvas[] customCanvases;
    
    [SerializeField] private Script_CanvasGroupController MynesMirrorCanvasGroup;
    [SerializeField] private Script_CanvasGroupController MynesMirrorBg;
    [SerializeField] private Script_PRCS MynesMirrorPRCS;
    [SerializeField] private Script_PRCS ElleniasHandPRCS;
    [SerializeField] private Script_PRCS toWeekendPRCS;
    [SerializeField] private Script_PRCS IdsDeadPRCS;
    [SerializeField] private Script_PRCS seaVignettePRCS;

    [SerializeField] private Script_GlitchFXManager glitchManager;
    [SerializeField] private Script_MynesMirrorManager mynesMirrorManager;

    [SerializeField] private Script_TimelineController faceOffTimelineController;
    [SerializeField] private Script_TimelineController awakeningFinalTimelineController;
    [SerializeField] private float waitTimeAfterFaceOffHold;
    [SerializeField] private float waitTimeAfterFaceOff;
    
    [Space][Header("Final Awakening")][Space]
    [SerializeField] private Script_BgThemePlayer seaFinalAwakeningBgThemePlayer;
    [SerializeField] private float finalAwakeningBgmFadeInTime;
    [SerializeField] private float waitTimeAfterAwakeningFinal;
    [SerializeField] private Script_DialogueNode myMaskReceiveNode;
    [SerializeField] private float waitTimeAfterMyMaskReceive;
    [Tooltip("Note this should be less than waitTimeAfterMyMaskReceive")]
    [SerializeField] private float finalAwakeningBgmFadeOutTime;
    [SerializeField] private float waitTimeAfterMyMaskTeleport;
    

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Script_Game game;

    private Action onFaceOffTimelineDoneAction;
    private Coroutine fullArtCoroutine;

    public int FaceOffCounter
    {
        get => game.faceOffCounter;
        set => game.faceOffCounter = value;
    }

    public float GlitchFadeInTime
    {
        get => glitchFadeInTime;
    }

    public float WaitTimeAfterMyMaskTeleport
    {
        get => waitTimeAfterMyMaskTeleport;
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
        PRCS.FadeIn(out fullArtCoroutine, fadeInSpeed, cb);
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

    public void OpenPRCSNoFade(
        Script_PRCS PRCS,
        bool isCustom = false,
        RenderMode renderMode = RenderMode.ScreenSpaceOverlay
    )
    {
        // Handle the PRCS Canvas if need to change in context
        if (!isCustom)
            HandlePRCSCanvasRenderMode(renderMode);
        
        PRCSCanvasGroup.alpha = 1f;
        PRCSCanvasGroup.gameObject.SetActive(true);

        PRCS.Setup();
        PRCS.Open();
    }

    public void InitializePRCSCanvasRenderMode()
    {
        HandlePRCSCanvasRenderMode(RenderMode.ScreenSpaceOverlay);
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
                
                var introTimelineIdx = 0;
                
                // Init and bind Myne Portrait
                mynesMirrorManager.BindIntroTimeline(
                    MynesMirrorPRCS.Director,
                    MynesMirrorPRCS.Timelines[introTimelineIdx]
                );

                MynesMirrorBg.Close();
                MynesMirrorCanvasGroup.Open();
                
                MynesMirrorPRCS.Setup();
                MynesMirrorPRCS.Open();
                MynesMirrorPRCS.PlayTimeline(introTimelineIdx);
                // MynesMirror Timeline controls Fading in Bg
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
            
            case CustomTypes.Sea:
                PRCSCanvasGroup.alpha = 1f;
                PRCSCanvasGroup.gameObject.SetActive(true);
                
                seaVignettePRCS.Setup();
                seaVignettePRCS.Open();
                seaVignettePRCS.PlayTimeline(0);
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
                MynesMirrorCanvasGroup.FadeOut(FadeSpeeds.Slow.ToFadeTime(), cb);
                break;
            
            case CustomTypes.ElleniasHand:
                HidePRCS(ElleniasHandPRCS, FadeSpeeds.XFast, cb);
                break;
            
            case CustomTypes.ToWeekend:
                HidePRCS(toWeekendPRCS, FadeSpeeds.None, cb);
                break;
            
            case CustomTypes.IdsDead:
                HidePRCS(IdsDeadPRCS, FadeSpeeds.None, cb);
                break;
            
            case CustomTypes.Sea:
                HidePRCS(seaVignettePRCS, FadeSpeeds.None, cb);
                break;

            default:
                break;
        }
    }

    public void CloseMynesMirrorNoFade()
    {
        ClosePRCSNoFade(MynesMirrorPRCS);
    }

    public void SetAwakeningActive(bool isActive)
    {
        if (isActive)
            AwakeningCanvasGroup.Open();
        else
            AwakeningCanvasGroup.Close();
    }

    public void SetAwakeningFinalActive(bool isActive)
    {
        if (isActive)
            AwakeningFinalCanvasGroup.Open();
        else
            AwakeningFinalCanvasGroup.Close();
    }

    public void SetFaceOffActive(int i, bool isActive)
    {
        if (i > FaceOffCanvasGroups.Length - 1)
            return;
        
        if (isActive)
            FaceOffCanvasGroups[i].Open();
        else
            FaceOffCanvasGroups[i].Close();
    }

    // Gives option to either use callback at beginning of fading out
    // or when fading is completely done.
    public void TalkingSelfSequence(
        Action cb,
        Action doneFadingCb = null,
        bool isFaceOff = false
    )
    {
        glitchManager.SetHigh();
        
        var bgm = Script_BackgroundMusicManager.Control;

        // For FaceOff, fade out BGM in 5 sec
        if (isFaceOff)
        {
            bgm.FadeOut(
                () => {
                    game.PauseBgMusic();
                    game.PauseBgThemeSpeakers();
                },
                glitchFadeInTime,
                Const_AudioMixerParams.ExposedBGVolume
            );
        }
        
        // 5 sec of glitch
        glitchManager.BlendTo(
            1f,
            glitchFadeInTime,
            () => {
                // 2 sec to Fade In Black
                Script_TransitionManager.Control.FadeInCoroutine(
                    Script_TransitionManager.FadeTimeSlow,
                    () => FadeInStartTimeline()
                );
            }
        );

        void FadeInStartTimeline()
        {
            Dev_Logger.Debug("Setting glitch manager to default, blend 0");
            glitchManager.SetDefault();
            glitchManager.SetBlend(0f);
            
            // Fade out Black for 2 sec
            Script_TransitionManager.Control.FadeOutCoroutine(
                Script_TransitionManager.FadeTimeSlow,
                // Optional callback when fading is done
                doneFadingCb    
            );
            
            // Optional callback when fading starts
            if (cb != null)
                cb();
            
            // Myne's Mirror Bg Player comes in at beginning of Face Off Timeline
            if (isFaceOff)
            {
                bgm.SetVolume(1f, Const_AudioMixerParams.ExposedBGVolume);
                faceOffBgThemePlayer.gameObject.SetActive(true);
            }
        }
    }

    // Pass an action that will be called at the end of the faceOff Timeline.
    public void PlayFaceOffTimeline(Action onTimelineDoneAction)
    {
        onFaceOffTimelineDoneAction = onTimelineDoneAction;

        if (FaceOffCounter > faceOffTimelineController.timelines.Count - 1)
        {
            if (onTimelineDoneAction != null)
                onTimelineDoneAction();
            return;
        }
        
        SetFaceOffActive(FaceOffCounter, true);
        faceOffTimelineController.PlayableDirectorPlayFromTimelines(0, FaceOffCounter);
    }

    public void PlayAwakeningFinalTimeline()
    {
        awakeningFinalTimelineController.PlayableDirectorPlayFromTimelines(0, 0);
    }

    private void SwitchLastElevatorMaskBackground()
    {
        game.EquipMaskBackground(Const_Items.LastElevatorId, isGive: false);
    }

    private void HandlePRCSCanvasRenderMode(RenderMode renderMode = RenderMode.ScreenSpaceOverlay)
    {
        PRCSCanvas.renderMode = renderMode;

        switch (renderMode)
        {
            case RenderMode.ScreenSpaceCamera:
                PRCSCanvas.worldCamera = mainCamera;
                PRCSCanvas.planeDistance = Script_GraphicsManager.CamCanvasPlaneDistance;
                break;
        }
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
            // Wait 1 sec before fading out bgm
            yield return new WaitForSeconds(waitTimeAfterFaceOffHold);
            
            // Fade out Myne's Mirror FaceOff bgThemePlayer while waiting to fade out Black Screen
            Script_BackgroundMusicManager.Control.FadeOut(
                () => faceOffBgThemePlayer.gameObject.SetActive(false),
                waitTimeAfterFaceOff,
                Const_AudioMixerParams.ExposedBGVolume
            );
            
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

    // Final Awakening Timeline
    public void FinalAwakeningFadeInBgmPlayer()
    {
        seaFinalAwakeningBgThemePlayer.FadeInPlay(null, finalAwakeningBgmFadeInTime);
    }
    
    public void OnAwakeningFinalTimelineDone()
    {
        game.ChangeStateCutScene();
        
        // Unequip the current mask & move all prepped masks to inventory. Also prevents effects from
        // carrying over to Grand Mirror like lantern light on.
        game.GetPlayer().DefaultStickerState();
        game.UnequipAll();
        
        Script_TransitionManager.Control.TimelineBlackScreen();
        SetAwakeningFinalActive(false);
        
        StartCoroutine(WaitMyMaskReceiveDialogue());

        IEnumerator WaitMyMaskReceiveDialogue()
        {
            yield return new WaitForSeconds(waitTimeAfterAwakeningFinal);

            Script_DialogueManager.DialogueManager.StartDialogueNode(myMaskReceiveNode);
        }
    }

    // ----------------------------------------------------------------------
    // Unity Events
    
    // myMaskReceiveNode
    public void MyMaskReceiveSFX()
    {
        Script_SFXManager.SFX.PlayItemPickUp();
    }
    
    // myMaskReceiveNode1
    public void OnMyMaskReceiveDone()
    {
        game.AddMyMaskBackground();

        glitchManager.SetLow();
        glitchManager.SetBlend(1f);
        
        StartCoroutine(WaitToFadeOut());

        // Fade Out Bgm before waiting in black with sheep bleats
        seaFinalAwakeningBgThemePlayer.FadeOutStop(null, finalAwakeningBgmFadeOutTime);

        IEnumerator WaitToFadeOut()
        {
            yield return new WaitForSeconds(waitTimeAfterMyMaskReceive);
            
            game.TeleportToGrandMirrorBackgroundR2();

            // On Initiate Level, Game will call transitionManager.InitialStateExcludingLevelFader()
            // which will reinit the Timeline Black Under Dialogue, so need to recover that canvas state.
            Script_TransitionManager transitionManager = Script_TransitionManager.Control;
            transitionManager.TimelineBlackScreen();

            // Here will be waiting in black screen while hearing sheep bleats
            yield return new WaitForSeconds(waitTimeAfterMyMaskTeleport);

            transitionManager.TimelineFadeOut(
                Script_TransitionManager.FadeTimeSlow,
                () => {
                    // Don't do anything to glitch setting here, since Grand Mirror R2 will
                    // modify it via Glitch Zone call to IncreaseGlitchBlend.
                }
            );
        }
    }

    // ----------------------------------------------------------------------

    public void Initialize()
    {
        /// Hide CanvasGroup but ensure the PRCS canvas is ready to use
        PRCSCanvasGroup.gameObject.SetActive(false);
        PRCSCanvasGroup.alpha = 0f;
        PRCSCanvas.gameObject.SetActive(true);
        InitializePRCSCanvasRenderMode();

        wellsJustOpenedPRCS.gameObject.SetActive(false);
        wellsJustOpenedPRCS.Setup();

        customCanvasesParent.gameObject.SetActive(true);
        
        // Ensure Custom Canvases are showing
        foreach (Canvas c in customCanvases)
            c.gameObject.SetActive(true);

        Script_PRCS []allPRCS = PRCSCanvasGroup.GetComponentsInChildren<Script_PRCS>(true);
        foreach (Script_PRCS prcs in allPRCS)
        {
            prcs.gameObject.SetActive(false);
        }

        MynesMirrorCanvasGroup.InitialState();
        MynesMirrorCanvasGroup.gameObject.SetActive(false);
        
        MynesMirrorBg.InitialState();
        MynesMirrorBg.gameObject.SetActive(false);

        // Initialize purely customized PRCS
        KingsIntroCanvasGroup.InitialState();
        KingsIntroCanvasGroup.gameObject.SetActive(true);

        AwakeningPortraitsController.Setup();

        ElleniaStabsCanvasGroup.InitialState();
        ElleniaStabsCanvasGroup.gameObject.SetActive(false);

        AwakeningCanvasGroup.InitialState();
        AwakeningCanvasGroup.gameObject.SetActive(false);

        AwakeningFinalCanvasGroup.InitialState();
        AwakeningFinalCanvasGroup.gameObject.SetActive(false);

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

            if (GUILayout.Button("On Awakening Final Timeline Done"))
            {
                t.OnAwakeningFinalTimelineDone();
            }
        }
    }
    #endif
}
