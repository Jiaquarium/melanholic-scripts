using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;
using System.Linq;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Player cracks the 3 Ice Blocks covering Myne's Grand Mirror.
/// Each respective Ice Stats plays their timeline on their "Die"
/// to reveal the respective world painting.
/// 
/// Note on Bound Volumes:
/// - Bounding Volume shouldn't bound bottom because want camera not to show too much of what
/// is ahead here
/// - Bounding Volumes switched out depending on R1 & R2
/// </summary>
[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_48 : Script_LevelBehavior
{
    // ==================================================================
    // State Data
    [SerializeField] private bool isDone;
    
    // Only set for Dev purposes
    [SerializeField] private bool isFinalRound;
    // ==================================================================

    [SerializeField] private List<GameObject> R1Objects;
    [SerializeField] private List<GameObject> R2Objects;

    // Default mirror theme to play on "softer" mood cut scenes.
    [SerializeField] private int mynesMirrorBgm;
    
    [SerializeField] private int glitchZoneSteps;
    [SerializeField] private float waitBeforeAwakeningCutTime;
    [SerializeField] private float waitTimeAfterAwakening;
    
    [SerializeField] private Script_Snow snowEffectAlways;

    [SerializeField] private PlayableDirector newWorldPaintingsDirector;

    [SerializeField] private Script_BgThemePlayer northWindPlayer;

    [SerializeField] private Script_MynesGrandMirror mynesGrandMirror;
    
    [Space][Header("R1 Only")][Space]
    [SerializeField] private float fadeInMynesMirrorVibesTime;
    
    [SerializeField] private Script_CrackableStats iceBlockStatsLeft;
    [SerializeField] private Script_CrackableStats iceBlockStatsMid;
    [SerializeField] private Script_CrackableStats iceBlockStatsRight;

    [SerializeField] private bool isIceBlockLeftCracked;
    [SerializeField] private bool isIceBlockMidCracked;
    [SerializeField] private bool isIceBlockRightCracked;

    [SerializeField] private float waitOnDiagonalCutToUnpause;
    [SerializeField] private float waitOnDiagonalCutToFrameTime;

    [SerializeField] private Renderer leftBody;
    [SerializeField] private Renderer middleBody;
    [SerializeField] private Renderer rightBody;

    [SerializeField] private Material wavyUnlitStoneMaterial;

    [SerializeField] private List<GameObject> iceBlockTimelineObjects;
    [SerializeField] private List<CinemachineVirtualCamera> virtualCameras;
    [SerializeField] private List<Script_InteractablePaintingEntrance> worldPaintings;

    [SerializeField] private TimelineAsset newWorldPaintingRevealTimeline;

    [SerializeField] private Script_LevelBehavior_20 ballroomBehavior;

    [SerializeField] private Script_ItemDialogueNode lastElevatorMaskDialogue;

    [SerializeField] private List<Collider> grandMirrorColliders;

    [SerializeField] private GameObject lastSectionSnowParent;
    [SerializeField] private GameObject nonLastSectionSnowParent;

    [SerializeField] private GameObject boundingVolumeR1Parent;
    [SerializeField] private Script_BoundingVolume boundingVolumeR1;
    
    // ------------------------------------------------------------------
    // Awakening Portraits

    [SerializeField] private Script_AwakeningPortraitsController awakeningPortraitsController;
    
    // Note: Source from BGM 2 so fading will work.
    [SerializeField] private Script_BgThemePlayer scribbleBgThemePlayer;
    // Note: Used by Timeline
    [SerializeField] private Script_BgThemePlayer scribbleIntenseBgThemePlayer;
    [SerializeField] private float scribbleFadeInTime;
    [SerializeField] private Script_Marker teleportLocationAfterAwakening;

    // ------------------------------------------------------------------

    [Space][Header("R2 Only")][Space]
    [SerializeField] private float fadeToBlackTime;
    [SerializeField] private Script_PlayerMutation playerMutation;
    [SerializeField] private Transform playerReflectionMyne;
    [SerializeField] private Script_BgThemePlayer drumBuildUpBgPlayer;
    
    [SerializeField] private float awakeningR2EntranceExtraWaitTime;
    [SerializeField] private Script_DialogueNode onEntranceR2Node;
    [SerializeField] private Script_DialogueNode onPushBackDoneR2Node;
    [SerializeField] private float onPushBackDoneR2WaitTime;
    [SerializeField] private GameObject IdsSpeakersParent;
    [SerializeField] private List<Script_SFXLooperParentController> sfxLooperParents;
    [SerializeField] private float sheepBleatsFadeOutTime;
 
    [SerializeField] private Script_LevelBehavior_45 Underworld;

    [SerializeField] private float finaleLightsIntensity;

    [SerializeField] private Script_ElevatorManager elevatorManager;

    [SerializeField] private Script_LevelBehavior_0 Woods;
    [SerializeField] private Script_ClockManager clockManager;

    [SerializeField] private float WaitBeforeFinalSaveTime;

    [SerializeField] private GameObject boundingVolumeR2Parent;
    [SerializeField] private Script_BoundingVolume boundingVolumeR2;

    private Script_TimelineController timelineController;
    private Script_CrackableStats currentIceBlockStats;
    private Script_GlitchFXManager glitchManager;
    private Script_WindManager windManager;

    private float currentTargetBlend;
    private bool isExitingWindZone;
    private bool didR2PushBackDoneDialogue;

    // ------------------------------------------------------------------
    // Trailer Only
    
    [SerializeField] private Script_VCamera trailerVCam;
    private bool isTrailerVCam;
    
    // ------------------------------------------------------------------


    public bool IsDone
    {
        get => isDone;
        set => isDone = value;
    }

    public bool IsFinalRound
    {
        get => isFinalRound;
        set => isFinalRound = value;
    }

    public Script_PlayerMutation PlayerMutation { get => playerMutation; }

    protected override void OnEnable()
    {
        base.OnEnable();

        Script_GameEventsManager.OnLevelInitComplete += OnLevelInitCompleteEvent;
        Script_InteractableObjectEventsManager.OnIceCrackingTimelineDone += PlayRevealNewWorldTimeline;
        Script_InteractableObjectEventsManager.OnUnfreezeEffect += ActivateGrandMirrorBreathing;
        Script_InteractableObjectEventsManager.OnDiagonalCut += OnDiagonalCut;
        Script_StickerEffectEventsManager.OnMyMaskForceFaceDir += StopSheepBleats;
        Script_StickerEffectEventsManager.OnMyMaskStopFaceDir += ResumeSheepBleats;

        currentTargetBlend = 0f;
        glitchManager.InitialState();

        if (!isFinalRound)
        {
            // Only R1 has the entrance from the Last Elevator.
            PauseBgmForElevator();
        }
        else
        {
            // In Final Round set the time to 5:59:55am so in sync with
            // MaskRevealTimeline.
            clockManager.SetFinalRoundGrandMirrorTime();
            game.IsDisableMasksOnly = true;
        }

        Script_Game.IsRunningDisabled = true;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        Script_GameEventsManager.OnLevelInitComplete -= OnLevelInitCompleteEvent;
        Script_InteractableObjectEventsManager.OnIceCrackingTimelineDone -= PlayRevealNewWorldTimeline;
        Script_InteractableObjectEventsManager.OnUnfreezeEffect -= ActivateGrandMirrorBreathing;
        Script_InteractableObjectEventsManager.OnDiagonalCut -= OnDiagonalCut;
        Script_StickerEffectEventsManager.OnMyMaskForceFaceDir -= StopSheepBleats;
        Script_StickerEffectEventsManager.OnMyMaskStopFaceDir -= ResumeSheepBleats;

        InitialState();

        Script_Game.IsRunningDisabled = false;
    }

    void OnValidate()
    {
        PopulateIdsSpeakers();
    }

    void Awake()
    {
        glitchManager = Script_GlitchFXManager.Control;
        windManager = Script_WindManager.Control;
        
        timelineController = GetComponent<Script_TimelineController>();
        
        snowEffectAlways.gameObject.SetActive(true);
        
        // Initialize World Painting States for Reveal Cut Scenes.
        ballroomBehavior.SetPaintingEntrancesActive(false);

        PopulateIdsSpeakers();
    }

    protected override void Update()
    {
        if (Const_Dev.IsTrailerMode)
            HandleTrailerPan();
    }

    // ------------------------------------------------------------------
    // Timeline Signals
    public void SetWorldPaintingSketch()
    {
        if (currentIceBlockStats == iceBlockStatsLeft)
        {
            worldPaintings[0].SetSketchAnimation();   
        }
        else if (currentIceBlockStats == iceBlockStatsMid)
        {
            worldPaintings[1].SetSketchAnimation();   
        }
        else if (currentIceBlockStats == iceBlockStatsRight)
        {
            worldPaintings[2].SetSketchAnimation();
        }
    }

    public void SetWorldPaintingStateActive()
    {
        if (currentIceBlockStats == iceBlockStatsLeft)
        {
            worldPaintings[0].SetStateActive();
        }
        else if (currentIceBlockStats == iceBlockStatsMid)
        {
            worldPaintings[1].SetStateActive();   
        }
        else if (currentIceBlockStats == iceBlockStatsRight)
        {
            worldPaintings[2].SetStateActive();
        }
    }

    // Called from end of NewWorldRevealPaintingTimeline
    public void OnNewWorldRevealPaintingDone()
    {
        grandMirrorColliders.ForEach(part => {
            // Turn back on Box Colliders
            List<Collider> cols = part.GetComponents<Collider>().ToList();
            cols?.ForEach(col => col.enabled = true);

            // Turn off Mesh Colliders
            var meshCol = part.GetComponent<MeshCollider>();
            if (meshCol != null)
                meshCol.enabled = false;
        });
        
        // If all three ice blocks have been destroyed, start the Awakening scene.
        if (IsAllIceBlocksCracked())
        {
            var PRCSManager = Script_PRCSManager.Control;

            // Fade Out Bgm
            var bgm = Script_BackgroundMusicManager.Control;
            bgm.FadeOut(null, fadeTime: PRCSManager.GlitchFadeInTime, Const_AudioMixerParams.ExposedBGVolume);
            
            PRCSManager.TalkingSelfSequence(() => {
                // Show Awakening canvas immediately (fader transition will handle fading).
                PRCSManager.SetAwakeningActive(true);
                
                // Awakening Timeline's TimelineTeletypeReveal's UnityEvents will enable/disable
                // Eye's animator during Pause/Play states (Timeline loses control when paused).
                timelineController.PlayableDirectorPlayFromTimelines(1, 1);

                PlayMynesMirrorAtTime(0f);
            });
        }
        else
        {
            Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
                isOpen: false,
                framing: Script_UIAspectRatioEnforcerFrame.Framing.ConstantThin,
                cb: game.ChangeStateInteract
            );
        }

        void PlayMynesMirrorAtTime(float time)
        {
            var bgm = Script_BackgroundMusicManager.Control;

            bgm.SetVolume(0f, Const_AudioMixerParams.ExposedBGVolume);
            bgm.PlayFadeIn(
                mynesMirrorBgm,
                cb: null,
                forcePlay: true,
                fadeInMynesMirrorVibesTime,
                Const_AudioMixerParams.ExposedBGVolume,
                bgm.r1AwakeningMynesMirrorStartTime
            );
        }
    }

    /// <summary>
    /// Pause Timeline manually with signal. Fire TimelineTeletypeReveal's onTypingDoneAction to
    /// resume the timeline, since we need to care for differing type speeds based on frame rates.
    /// </summary>
    public void PauseAwakeningTimeline()
    {
        var awakeningDirector = timelineController.playableDirectors[1];
        awakeningDirector.Pause();
    }

    // Awakening Portraits Timeline
    // Fade out Myne's Mirror theme and fade in Scribble Bgm simultaneously
    public void FadeInScribbleBgm()
    {
        var bgm = Script_BackgroundMusicManager.Control;
        var bgmParam = Const_AudioMixerParams.ExposedBGVolume;

        bgm.FadeOut(() => {
            bgm.SetVolume(0f, bgmParam);
            bgm.Stop();
            bgm.SetVolume(1f, bgmParam);
        }, scribbleFadeInTime, bgmParam);

        // On Source BG2
        scribbleBgThemePlayer.FadeInPlay(null, scribbleFadeInTime);
    }
    
    // Last Timeline Teletype Reveal of Awakening Timeline
    public void OnAwakeningTimelineDone()
    {
        Dev_Logger.Debug("OnAwakeningTimelineDone Event called from Teletype Reveal Done Typing handler");
        
        game.ChangeStateCutScene();

        StartCoroutine(WaitToCut());        
        
        IEnumerator WaitToCut()
        {
            yield return new WaitForSeconds(waitBeforeAwakeningCutTime);
            
            // Set Fade Canvas Active
            Script_TransitionManager.Control.TimelineUnderHUDBlackScreenOpen();

            // Stop the previously paused Awakening Timeline
            var awakeningDirector = timelineController.playableDirectors[1];
            awakeningDirector.Stop();
            
            Script_PRCSManager.Control.SetAwakeningActive(false);
            
            Dev_Logger.Debug("Waiting for next part in sequence!!!");

            // Wait a few seconds in black screen before Awakening Portraits
            yield return new WaitForSeconds(waitTimeAfterAwakening);

            PlayAwakeningPortraitsTimeline();
        }
    }

    private void PlayAwakeningPortraitsTimeline()
    {
        // For editor call
        game.ChangeStateCutScene();

        EquipLastElevatorMaskBackground();
        
        var player = game.GetPlayer();
        player.Teleport(teleportLocationAfterAwakening.Position);
        player.FaceDirection(Directions.Down);
        
        Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
            isOpen: true,
            framing: Script_UIAspectRatioEnforcerFrame.Framing.AwakeningPortraits,
            isNoAnimation: true
        );
        
        // Play AwakeningPortraitsTimeline
        awakeningPortraitsController.OpenAwakeningPortraits();
        Script_TransitionManager.Control.TimelineUnderHUDBlackScreenClose();
        timelineController.PlayableDirectorPlayFromTimelines(3, 3);
    }

    // Awakening Portraits Timeline: StopScribbleBgm when canvas goes black on last portrait
    public void StopScribbleBgm()
    {
        scribbleBgThemePlayer.SoftStop();
    }

    // AwakeningPortraitsTimeline Done
    public void OnAwakeningPortraitsTimelineDone()
    {
        Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
            isOpen: false,
            framing: Script_UIAspectRatioEnforcerFrame.Framing.AwakeningPortraits,
            isNoAnimation: true
        );
        
        // Leave glitch setting on for remainder of day
        glitchManager.SetLow();
        glitchManager.SetBlend(1f);
        
        Script_TransitionManager.Control.TimelineBlackScreen();
        awakeningPortraitsController.CloseAwakeningPortraits();

        Script_BackgroundMusicManager.Control.Play(
            i: mynesMirrorBgm,
            forcePlay: true,
            forceNewStartTime: true
        );

        Script_TransitionManager.Control.TimelineFadeOut(
            Script_TransitionManager.FadeTimeSlow,
            () =>  Script_DialogueManager.DialogueManager.StartDialogueNode(lastElevatorMaskDialogue)
        );
    }

    private void EquipLastElevatorMaskBackground()
    {
        game.EquipMaskBackground(Const_Items.LastElevatorId, true);
    }
    
    // ------------------------------------------------------------------
    // Unity Events & Timeline Signals
    
    /// <summary>
    /// When Crackable begins cracking sequence, hide the Base & Side's Box Colliders
    /// because the Ice Pieces get stuck inside.
    /// Turn on Mesh Colliders so Ice Pieces can still interact with the environment.
    /// </summary>
    public void OnDiagonalCut(Script_CrackableStats ice)
    {
        // Pause timeline for UI Frame
        ice.CrackingDirector.Pause();

        StartCoroutine(WaitToUnpause());
        StartCoroutine(WaitToFrame());
        
        grandMirrorColliders.ForEach(part => {
            // Turn off Box Colliders
            List<BoxCollider> cols = part.GetComponents<BoxCollider>().ToList();
            cols?.ForEach(col => col.enabled = false);

            // Turn on Mesh Colliders
            var meshCol = part.GetComponent<MeshCollider>();
            if (meshCol != null)
                meshCol.enabled = true;
        });

        // For trailer mode make all ice explode in unison.
        if (Const_Dev.IsTrailerMode)
        {
            if (ice != iceBlockStatsLeft)
                iceBlockStatsLeft.Hurt(1, null, null);
            
            if (ice != iceBlockStatsMid)
                iceBlockStatsMid.Hurt(1, null, null);
            
            if (ice != iceBlockStatsRight)
                iceBlockStatsRight.Hurt(1, null, null);
        }

        // Unpause timeline before framing, since the wait from after framing to shatter
        // feels too long
        IEnumerator WaitToUnpause()
        {
            yield return new WaitForSeconds(waitOnDiagonalCutToUnpause);

            ice.CrackingDirector.Play();
        }
        
        IEnumerator WaitToFrame()
        {
            yield return new WaitForSeconds(waitOnDiagonalCutToFrameTime);

            Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
                isOpen: true,
                framing: Script_UIAspectRatioEnforcerFrame.Framing.ConstantThin,
                cb: null
            );
        }
    }
    
    public void OnIceBlockCracked(Script_CrackableStats ice)
    {
        game.ChangeStateCutScene();
        
        Dev_Logger.Debug($"Cracked Ice Block <{ice}>");

        if (ice == iceBlockStatsLeft)
            mynesGrandMirror.SetMirrorGraphics(false, 0);   
        else if (ice == iceBlockStatsMid)
            mynesGrandMirror.SetMirrorGraphics(false, 1);
        else if (ice == iceBlockStatsRight)
            mynesGrandMirror.SetMirrorGraphics(false, 2);
    }

    public void ActivateGrandMirrorBreathing(Script_CrackableStats ice)
    {
        if (ice == iceBlockStatsLeft)
        {
            leftBody.material = wavyUnlitStoneMaterial;
            ActivateScroller(leftBody.GetComponent<Script_TextureScroller>());
        }
        else if (ice == iceBlockStatsMid)
        {
            middleBody.material = wavyUnlitStoneMaterial;
            ActivateScroller(middleBody.GetComponent<Script_TextureScroller>());
        }
        else if (ice == iceBlockStatsRight)
        {
            rightBody.material = wavyUnlitStoneMaterial;
            ActivateScroller(rightBody.GetComponent<Script_TextureScroller>());
        }

        void ActivateScroller(Script_TextureScroller scroller)
        {
            if (scroller != null)
            {
                scroller.enabled = true;
                scroller.UpdateMaterial();
            }
        }
    }

    public void PlayRevealNewWorldTimeline(Script_CrackableStats iceStats)
    {
        Dev_Logger.Debug($"Reacting to IceCrackingTimelineDone event with iceStats <{iceStats}>");
        
        currentIceBlockStats = iceStats;

        // Bind the proper objects depending on which crackable stats.
        if (currentIceBlockStats == iceBlockStatsLeft)
        {
            isIceBlockLeftCracked = true;
            newWorldPaintingsDirector.BindTimelineTracks(newWorldPaintingRevealTimeline, iceBlockTimelineObjects);
            timelineController.BindVirtualCameraAndPlayFromDirector(0, 0, virtualCameras[0]);
        }
        else if (currentIceBlockStats == iceBlockStatsMid)
        {
            isIceBlockMidCracked = true;
            newWorldPaintingsDirector.BindTimelineTracks(newWorldPaintingRevealTimeline, iceBlockTimelineObjects);
            timelineController.BindVirtualCameraAndPlayFromDirector(0, 0, virtualCameras[1]);
        }
        else if (currentIceBlockStats == iceBlockStatsRight)
        {
            isIceBlockRightCracked = true;
            newWorldPaintingsDirector.BindTimelineTracks(newWorldPaintingRevealTimeline, iceBlockTimelineObjects);
            timelineController.BindVirtualCameraAndPlayFromDirector(0, 0, virtualCameras[2]);
        }
    }

    // Zone trigger events
    public void SetNorthWindActive(bool isActive)
    {
        game.GetPlayer().IsNorthWind = isActive;

        if (isActive)
            northWindPlayer.FadeInPlay();
        else
        {
            // Abrubtly stop music when player clears the top of wind zone.
            Directions lastMove = game.GetPlayer().LastMove;
            
            if (lastMove == Directions.Up)
                northWindPlayer.SoftStop();
            else
                northWindPlayer.FadeOutStop();
        }
    }

    public void IncreaseGlitchBlend()
    {
        if (Const_Dev.IsTrailerMode)
        {
            // Immediately set blend to high for trailer, so it's obvious we're using
            // a glitch effect.
            glitchManager.SetHigh(useCurrentBlend: true);
            glitchManager.BlendTo(1f);
        }
        else
        {
            float blendStep = 1f / glitchZoneSteps;
            currentTargetBlend += blendStep;

            // Ensure current settings is Default.
            glitchManager.SetDefault(useCurrentBlend: true);
            glitchManager.BlendTo(currentTargetBlend);
        }
    }

    public void DecreaseGlitchBlend()
    {
        // Prevent error when closing game while on glitch zone.
        if (glitchManager == null)
            return;
        
        // Once player clears the top of the wind zone, immediately set Blend for abrubt effect.
        Directions lastMove = game.GetPlayer().LastMove;
        
        // For trailer, always have glitching, unless exiting from the top.
        if (Const_Dev.IsTrailerMode)
        {
            if (lastMove == Directions.Up)
                glitchManager.SetBlend(0f);
        }
        else
        {
            float blendStep = 1f / glitchZoneSteps;
            currentTargetBlend -= blendStep;

            // Ensure current settings is Default.
            glitchManager.SetDefault(useCurrentBlend: true);
            
            if (lastMove == Directions.Up)
                glitchManager.SetBlend(currentTargetBlend);
            else
                glitchManager.BlendTo(currentTargetBlend);
        }
    }

    /// <summary>
    /// Wind pushes player back to the beginning of the Wind Zone.
    /// This signals to Player they must do something else rather than
    /// wasting time brute forcing through the wind.
    /// </summary>
    public void WindPushBackEnter()
    {
        HandleWindPushBack();
    }

    /// <summary>
    /// Wind Zone Trigger Stay Action (not Wind Push Back)
    /// If Player switches to My Mask, they should be able to move again immediately.
    /// </summary>
    public void WindZoneStayHandleMove()
    {
        var player = game.GetPlayer();
        var activeMaskId = Script_ActiveStickerManager.Control.ActiveSticker?.id ?? string.Empty;
        var isWearingMyMask = activeMaskId == Const_Items.MyMaskId;
        
        if (player.IsPassive && isWearingMyMask)
        {
            player.IsPassive = false;
        }
    }

    /// <summary>
    /// Wind Push Back Trigger Stay Action (not Wind Zone)
    /// If Player switches off from My Mask anywhere inside the Push Trigger,
    /// should push Player back.
    /// </summary>
    public void WindPushBackStayHandlePush()
    {
        HandleWindPushBack();        
    }

    /// <summary>
    /// Done Triggers, should ensure exitting of Passive state.
    /// </summary>
    public void WindPushBackDone()
    {
        var player = game.GetPlayer();
        
        if (player.IsPassive)
        {
            player.IsPassive = false;

            if (game.IsDisableMasksOnly && !didR2PushBackDoneDialogue)
            {
                didR2PushBackDoneDialogue = true;

                game.ChangeStateCutScene();

                // Wait before yelling at Player
                StartCoroutine(WaitToReactToPushBackDone());
            }
        }

        IEnumerator WaitToReactToPushBackDone()
        {
            yield return new WaitForSeconds(onPushBackDoneR2WaitTime);

            Script_DialogueManager.DialogueManager.StartDialogueNode(onPushBackDoneR2Node);
        }
    }

    /// <summary>
    /// R2 Glitch Zone 2 Trigger
    /// Set by trigger nearest final torii, to use MyMaskClose values, speeding up player
    /// </summary>
    public void SetMyMaskIsClose(bool isR2Close)
    {
        windManager.IsR2Close = isR2Close;
    }
    
    public void SetOnlyLastSectionSnow(bool isOnlyLastSectionSnow)
    {
        Script_Snow[] snowsLast = lastSectionSnowParent.GetComponentsInChildren<Script_Snow>(true);
        Script_Snow[] snowsNonLast = nonLastSectionSnowParent.GetComponentsInChildren<Script_Snow>(true);
        
        if (isOnlyLastSectionSnow)
        {
            foreach (var snow in snowsLast)
                snow.SetEmissionEnabled(true);
            
            foreach (var snow in snowsNonLast)
                snow.SetEmissionEnabled(false);
        }
        else
        {
            foreach (var snow in snowsLast)
                snow.SetEmissionEnabled(true);
            
            foreach (var snow in snowsNonLast)
                snow.SetEmissionEnabled(true);
        }
    }

    // From interaction with Myne
    public void HandleMaskRevealSequence()
    {
        var activeMaskId = Script_ActiveStickerManager.Control.ActiveSticker?.id ?? string.Empty;
        var isWearingMyMask = activeMaskId == Const_Items.MyMaskId;

        if (!isWearingMyMask)
        {
            Dev_Logger.Debug("Not wearing My Mask, cannot interact.");
            return;
        }

        PlayMaskRevealTimeline();
    }

    public void DevForcePlayMaskRevealTimeline() => PlayMaskRevealTimeline();
    
    private void PlayMaskRevealTimeline()
    {
        Dev_Logger.Debug("Playing Mask Reveal Timeline!!!");
        
        game.ChangeStateCutScene();

        // Force Lights Intensity for panning cut scenes.
        var lightsManager = Script_LightFXManager.Control;
        lightsManager.IsPaused = true;
        lightsManager.SetDirectionalLightsIntensity(finaleLightsIntensity);

        // Animate in Frame
        Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
            isOpen: true,
            framing: Script_UIAspectRatioEnforcerFrame.Framing.MaskReveal,
            cb: () => { timelineController.PlayableDirectorPlayFromTimelines(2, 2); }
        );

        // Setup Woods in case WellJustOpened cut scene didn't play.
        Woods.didStartThought = true;
    }

    public void HandleExitWindZone()
    {
        var exittingDir = game.GetPlayer().FacingDirection;
        
        Dev_Logger.Debug($"HandleExitWindZone exittingDir {exittingDir}");
        
        // Only trigger this reaction if exiting with forward direction
        if (exittingDir == Directions.Up && !isExitingWindZone)
        {
            var sfx = Script_SFXManager.SFX;
            sfx.PlayWindZoneExit();

            // Prevent multiple calls
            isExitingWindZone = true;

            StartCoroutine(WaitEndFrameSetFlag());
        }

        IEnumerator WaitEndFrameSetFlag()
        {
            yield return new WaitForEndOfFrame();
            isExitingWindZone = false;
        }
    }

    // ------------------------------------------------------------------
    // Mask Reveal Timeline Signals

    // On Play of Mask Reveal Timeline
    // Note: Ensure this matches the length of Timeline fade to Black.
    public void FadeOutMynesLairExaggTheme()
    {
        var bgm = Script_BackgroundMusicManager.Control;
        bgm.FadeOut(null, fadeTime: fadeToBlackTime, Const_AudioMixerParams.ExposedBGVolume);
    }

    // After Fading out Exagg Theme when Myne Standing appears
    public void PlayMynesMirrorTheme()
    {
        var bgm = Script_BackgroundMusicManager.Control;
        bgm.SetVolume(1f, Const_AudioMixerParams.ExposedBGVolume);

        bgm.Play(mynesMirrorBgm, forcePlay: true);
    }

    // R2 On Myne Mask Reveal 2
    public void StopDrumBuildUp()
    {
        drumBuildUpBgPlayer.SoftStop();
    }

    /// <summary>
    /// Set Borders back to default
    /// Before countdown nested timeline (so HUD is placed where it should be in-game)
    /// </summary>
    public void RemoveUIFraming()
    {
        Script_UIAspectRatioEnforcerFrame.Control.MatchBorders();
    }

    // Save Game and start on Sunday.
    public void OnMaskRevealDone()
    {
        StartCoroutine(WaitToStartSunday());
        
        IEnumerator WaitToStartSunday()
        {
            yield return new WaitForSeconds(WaitBeforeFinalSaveTime);
            
            game.ShowSaveAndRestartMessageDefault();
            game.StartSundayCycleSaveInitialize();
        }   
    }
    
    // True Ending Mask Reveal Timeline: True Ending Underworld Pan Timeline
    public void SetUnderworldFinalTrueEndingTimeline()
    {
        Dev_Logger.Debug($"Setting Underworld.IsFinalTrueEndingTimeline {Underworld.IsFinalTrueEndingTimeline} to true");
        Underworld.IsFinalTrueEndingTimeline = true;
    }

    // ------------------------------------------------------------------
    // Next Node Actions

    // LastElevatorNode
    public void OnLastElevatorDialogueDone()
    {
        game.ChangeStateInteract();
    }

    // R2 Mask Reveal Timeline when Pauses on Rin Closeup after Myne talks
    public void OnRinNo()
    {
        var bgm = Script_BackgroundMusicManager.Control;
        bgm.FadeOutFast(() => {
            bgm.Pause();
            PlayDrumBuildUp();
        }, Const_AudioMixerParams.ExposedBGVolume);
    }

    // R2 Mask Reveal Timeline when Pauses on Rin Closeup after Myne talks
    public void PlayDrumBuildUp()
    {
        Script_BackgroundMusicManager.Control.SetVolume(1f, Const_AudioMixerParams.ExposedBGVolume);
        drumBuildUpBgPlayer.Play();
    }

    // Unused
    public void OnRinLastDialogue()
    {
        var sfx = Script_SFXManager.SFX;
        sfx.Play(sfx.RhythmicXBeat, sfx.RhythmicXBeatVol);
    }

    // onEntranceR2Node
    public void OnEntranceR2DialogueDone()
    {
        // Remove framing set from TransitionManager:OnCurrentQuestDone for Final Awakening
        Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
            isOpen: false,
            framing: Script_UIAspectRatioEnforcerFrame.Framing.ConstantThin,
            cb: game.ChangeStateInteract
        );   
    }

    public void OnPushBackDoneR2DialogueDone()
    {
        game.ChangeStateInteract();
        game.IsDisableMasksOnly = false;
    }

    // ------------------------------------------------------------------

    private void HandleWindPushBack()
    {
        var player = game.GetPlayer();
        var activeMaskId = Script_ActiveStickerManager.Control.ActiveSticker?.id ?? string.Empty;
        var isWearingMyMask = activeMaskId == Const_Items.MyMaskId;
        
        if (!player.IsPassive && !isWearingMyMask)
        {
            player.IsPassive = true;
            player.PassiveNotificationSFX = Script_SFXManager.SFX.WindPushBack;
        }
    }
    
    private bool IsAllIceBlocksCracked()
    {
        return isIceBlockLeftCracked && isIceBlockMidCracked && isIceBlockRightCracked;
    }

    private void PauseBgmForElevator()
    {
        Dev_Logger.Debug($"PauseBgmForElevator elevatorManager.IsBgmOn {elevatorManager.IsBgmOn}");

        // Only stop Bgm if the elevator manager hasn't already restarted it.
        // This happens on same frame but after Bgm Start on InitLevel.
        if (!elevatorManager.IsBgmOn)
            Script_BackgroundMusicManager.Control.Stop();
    }

    public void StopSheepBleats(Directions dir)
    {
        sfxLooperParents.ForEach(parent => parent.StopChildren());
    }

    private void ResumeSheepBleats()
    {
        SetPlayerMutationActive(true);
        sfxLooperParents.ForEach(parent => parent.PlayChildren());
    }

    public void SetPlayerMutationActive(bool isActive) => PlayerMutation.gameObject.SetActive(isActive);

    private void OnLevelInitCompleteEvent()
    {
        // Start in cut scene in R2
        if (IsFinalRound)
        {
            game.ChangeStateCutScene();

            StartCoroutine(WaitOnEntranceR2Dialogue());
        }

        IEnumerator WaitOnEntranceR2Dialogue()
        {
            // Wait >3s (time after teleport waiting for total black screen fade out time)
            var waitTime = Script_PRCSManager.Control.WaitTimeAfterMyMaskTeleport
                + Script_TransitionManager.FadeTimeSlow
                + awakeningR2EntranceExtraWaitTime;
            
            yield return new WaitForSeconds(waitTime);

            Script_DialogueManager.DialogueManager.StartDialogueNode(onEntranceR2Node);
        }
    }

    // ------------------------------------------------------------------
    // Trailer Only

    private void HandleTrailerPan()
    {
        if (Input.GetButtonDown(Const_KeyCodes.TrailerCam))
        {
            if (!isTrailerVCam)
                Script_VCamManager.VCamMain.SetNewVCam(trailerVCam);
            else
                Script_VCamManager.VCamMain.SwitchToMainVCam(trailerVCam);
            
            isTrailerVCam = !isTrailerVCam;
        }
    }

    // ------------------------------------------------------------------

    // Switch environment based on R1 or R2
    private void HandleEnvironment(bool isR2)
    {
        R2Objects.ForEach(obj => obj.SetActive(isR2));
        R1Objects.ForEach(obj => obj.SetActive(!isR2));
    }

    // R2's bounding volume is shorter, since its tilemap is shorter than R1.
    private void HandleBoundingVolumes(bool isR2)
    {
        if (isR2)
        {
            boundingVolumeR1Parent.SetActive(false);
            boundingVolumeR2Parent.SetActive(true);
            BoundingVolume = boundingVolumeR2;
        }
        else
        {
            boundingVolumeR1Parent.SetActive(true);
            boundingVolumeR2Parent.SetActive(false);
            BoundingVolume = boundingVolumeR1;
        }
    }

    private void PopulateIdsSpeakers()
    {
        var sfxLooperParentsArray = IdsSpeakersParent.GetComponentsInChildren<Script_SFXLooperParentController>(true);
        sfxLooperParents = sfxLooperParentsArray.ToList();
    }
    
    public override void InitialState()
    {
        base.InitialState();

        glitchManager.InitialState();
        windManager.InitialState();
        isFinalRound = false;
        game.GetPlayer().IsFinalRound = false;
    }
    
    public override void Setup()
    {
        base.Setup();

        HandleEnvironment(IsFinalRound);
        HandleBoundingVolumes(IsFinalRound);
        
        windManager.IsFinalRound = IsFinalRound;
        game.GetPlayer().IsFinalRound = IsFinalRound;
        
        // Awakening Portraits Cut Scene Setup
        scribbleBgThemePlayer.gameObject.SetActive(false);
        scribbleIntenseBgThemePlayer.gameObject.SetActive(false);

        if (isFinalRound)
        {
            game.SetupPlayerReflection(playerReflectionMyne);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_LevelBehavior_48))]
    public class Script_LevelBehavior_48Tester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_LevelBehavior_48 t = (Script_LevelBehavior_48)target;
            
            GUILayout.Space(8);
            
            if (GUILayout.Button("R1 Environment"))
            {
                t.HandleEnvironment(false);
                t.HandleBoundingVolumes(false);
            }

            if (GUILayout.Button("R2 Environment"))
            {
                t.HandleEnvironment(true);
                t.HandleBoundingVolumes(true);
            }

            GUILayout.Space(8);
            
            if (GUILayout.Button("Awakening Timeline"))
            {
                t.isIceBlockLeftCracked = true;
                t.isIceBlockMidCracked = true;
                t.isIceBlockRightCracked = true;
                t.OnNewWorldRevealPaintingDone();
            }

            if (GUILayout.Button("Play Mask Reveal Timeline (Force)"))
            {
                t.PlayMaskRevealTimeline();
            }
            
            if (GUILayout.Button("Switch to Last Elevator Background"))
            {
                t.EquipLastElevatorMaskBackground();
            }

            if (GUILayout.Button("On Awakening Timeline Done"))
            {
                t.OnAwakeningTimelineDone();
            }

            if (GUILayout.Button("Play Awakening Portraits Timeline"))
            {
                t.PlayAwakeningPortraitsTimeline();
            }

            if (GUILayout.Button("On Awakening Portraits Timeline Done"))
            {
                t.OnAwakeningPortraitsTimelineDone();
            }

            if (GUILayout.Button("Set Woods Well PRCS Done"))
            {
                t.Woods.didStartThought = true;
            }

            if (GUILayout.Button("On Mask Reveal Timeline Done"))
            {
                t.OnMaskRevealDone();
            }
        }
    }
#endif   
}