using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Playables;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Player should react to snow on entrance, hinting at the Fireplace exit.
/// 
/// Special Intro will play once on first entering, and this will be saved into state
/// (will only happen once unless Day isn't saved)
/// </summary>
[RequireComponent(typeof(Script_TimelineController))]
[RequireComponent(typeof(Script_WorldTilesController))]
[RequireComponent(typeof(AudioSource))]
public class Script_LevelBehavior_42 : Script_LevelBehavior
{
    public const string MapName = Script_Names.WellsWorld;
    
    // ==================================================================
    // State Data
    public bool didPickUpLastWellMap;
    public bool didPickUpSpeedSeal;
    public bool isMooseQuestDone;
    public bool didPlayFaceOff;
    public bool didSpecialIntro;
    public bool didWellTalkInitialDialogue;
    // ==================================================================
    
    public bool isCurrentMooseQuestComplete;

    [SerializeField] private float waitTimeBeforeCastMooseFinalSpell;

    [SerializeField] private Script_WellsPuzzleController wellsPuzzleController;
    
    [SerializeField] private Script_FrozenWell[] frozenWells;
    
    [SerializeField] private Script_InteractablePaintingEntrance[] paintingEntrances;
    [SerializeField] private Script_InteractablePaintingEntrance ballroomPaintingEntrance;
    
    [SerializeField] private Script_Item lastSpellRecipeBookItem;
    
    [SerializeField] private Script_DialogueNode OnHasLastSpellRecipeBookNode;
    [SerializeField] private Script_DialogueNode OnMissingLastSpellRecipeBookNode;
    [SerializeField] private Script_DialogueNode OnMooseGiveItemDoneNode;
    
    [SerializeField] private Script_DialogueNode OnSnowReactionNode;
    
    // For Special Intro and on Freeze Well scene if not done on entrance (impossible now)
    [SerializeField] private float beforeSnowReactionShortTime;
    // A bit longer wait time to give Player time to absorb the snow effect
    [SerializeField] private float beforeSnowReactionOpeningTime;
    
    [SerializeField] private Script_LevelBehavior_13 CatWalkBehavior;

    [SerializeField] private Script_Moose[] Mooses;
    [SerializeField] private Script_DemonNPC[] Suzettes;
    
    [SerializeField] private Script_ScarletCipherPiece[] scarletCipherPieces;

    [SerializeField] private Script_WeatherFXManager weatherFXManager;
    [SerializeField] private List<Script_WellsWorldBehavior> wellsWorldBehaviors;

    // ------------------------------------------------------------------
    // Moose Quest
    
    [Space][Header("Moose Quest")][Space]
    // Success time longer than fail wait time to build suspense, a different expectation.
    [SerializeField] private float pauseBeforeNewNodeSuccessTime;
    [SerializeField] private float pauseBeforeNewNodeFailTime;
    [SerializeField] private float pauseBeforeIntroNodeTime;
    [SerializeField] private FadeSpeeds successFullArtFadeOutSpeed;

    // ------------------------------------------------------------------
    // Intro Only
    
    [Space][Header("Intro")][Space]
    [SerializeField] private float specialCaseFadeInTime;
    [SerializeField] private float specialCaseWaitInBlackTime;
    [SerializeField] private float waitToPlayIntroDirectorTime;
    [SerializeField] private Script_BgThemePlayer droneBgmPlayerIntro;
    [SerializeField] private float droneFadeInTimeIntro;
    [SerializeField] private float droneFadeOutTimeIntro;
    [SerializeField] private float fadeInBgmTimeIntro;
    [SerializeField] private float waitToFadeInBlackScreenTime;
    [SerializeField] private float fadeInBlackScreenTimeIntro;
    [SerializeField] private float blackScreenTimeIntro;
    [SerializeField] private float fadeOutBlackScreenTimeIntro;
    [SerializeField] private PlayableDirector introDirector;
    [SerializeField] private Script_GlitchFXManager glitchFXManager;
    [SerializeField] private Script_VCamera introZoomOutGameVCam;
    [SerializeField] private Script_TransitionManager transitionManager;
    [SerializeField] private Script_MapNotification mapNotification;
    [SerializeField] private Script_LevelCustomFadeBehavior levelCustomFadeBehavior;
    public bool IsSpecialIntro => !didSpecialIntro;
    private bool isSpecialIntroFraming;
    
    // ------------------------------------------------------------------
    // Trailer Only
    [Space][Header("For Trailer Only")][Space]
    [SerializeField] private PlayableDirector trailerDirector;
    [SerializeField] private float waitToPlayTrailerDirectorTime;
    
    // ------------------------------------------------------------------

    private bool didMapNotification;
    private bool didSnowReaction;
    private bool didMapNotificationDoneEvent;

    public Script_WellsPuzzleController WellsPuzzleController => wellsPuzzleController;
    public bool IsSnowReaction => !didSnowReaction && !CatWalkBehavior.didPickUpLightSticker;

    protected override void OnEnable()
    {
        Script_GameEventsManager.OnLevelInitComplete                            += OnLevelInitCompleteEvent;
        Script_GameEventsManager.OnLevelBlackScreenDone                         += OnLevelBlackScreenDone;
        
        Script_PuzzlesEventsManager.OnPuzzleSuccess                             += OnPuzzleSuccess;
        Script_ItemsEventsManager.OnItemPickUp                                  += OnItemPickUp;

        Script_ScarletCipherEventsManager.OnScarletCipherPiecePickUp            += OnScarletCipherPickUp;

        Script_InteractableObjectEventsManager.OnFrozenWellDie                  += OnFrozenWellDie;
        Script_InteractableObjectEventsManager.OnShatter                        += OnShatter;

        Script_TransitionsEventsManager.OnMapNotificationTeletypeDone           += OnMapNotificationTeletypeDone;
        Script_TransitionsEventsManager.OnMapNotificationDefaultDone            += HandleOpeningSnowNoSpecial;

        Script_GraphicsManager.Control.SetWellsWorldPhysics();
    }

    protected override void OnDisable()
    {
        Script_GameEventsManager.OnLevelInitComplete                            -= OnLevelInitCompleteEvent;
        Script_GameEventsManager.OnLevelBlackScreenDone                         -= OnLevelBlackScreenDone;
        
        Script_PuzzlesEventsManager.OnPuzzleSuccess                             -= OnPuzzleSuccess;
        Script_ItemsEventsManager.OnItemPickUp                                  -= OnItemPickUp;
        
        Script_ScarletCipherEventsManager.OnScarletCipherPiecePickUp            -= OnScarletCipherPickUp;

        Script_InteractableObjectEventsManager.OnFrozenWellDie                  -= OnFrozenWellDie;
        Script_InteractableObjectEventsManager.OnShatter                        -= OnShatter;

        Script_TransitionsEventsManager.OnMapNotificationTeletypeDone           -= OnMapNotificationTeletypeDone;
        Script_TransitionsEventsManager.OnMapNotificationDefaultDone            -= HandleOpeningSnowNoSpecial;

        Script_GraphicsManager.Control.SetDefaultPhysics();
    }

    void Awake()
    {
        if (weatherFXManager.IsSnowDay)
            SetSmallSnowActive(true);
        else
            SetSmallSnowActive(false);
    }

    void Start()
    {
        if (IsSpecialIntro)
        {
            // Set custom shadow distance for special intro
            Script_GraphicsManager.Control.SetWellsWorldSpecialIntroShadowDistance();

            Script_BackgroundMusicManager.Control.SetVolume(0f, Const_AudioMixerParams.ExposedBGVolume);
            game.StopBgMusic();
            
            // Put up frame
            Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
                isOpen: true,
                framing: Script_UIAspectRatioEnforcerFrame.Framing.ConstantDefault,
                isNoAnimation: true
            );
            isSpecialIntroFraming = true;
            
            // Remove this black screen with timeline signal later
            // Timeline will then use the over canvas (since the Under canvas needs to be controlled
            // by script later)
            transitionManager.TimelineBlackScreen(isOver: false);
        }
    }

    protected override void Update()
    {
        if (Const_Dev.IsTrailerMode)
            HandleTrailerPan();
    }
    
    private void OnLevelBlackScreenDone()
    {
        if (!didMapNotification && !IsSpecialIntro)
        {
            Script_MapNotificationsManager.Control.PlayMapNotification(MapName);
            didMapNotification = true;
        }
    }
    
    private void OnLevelInitCompleteEvent()
    {
        if (IsSpecialIntro)
        {
            game.ChangeStateCutScene();
            PlaySpecialIntro();
        }
        // Set up for HandleOpeningSnowNoSpecial()
        else if (IsSnowReaction && !didMapNotificationDoneEvent)
        {
            game.ChangeStateCutScene();
        }
    }

    /// <summary>
    /// Always give hint to find the fireplace if didn't finish Lantern quest.
    /// This will ALWAYS be accompanied with Map Notification.
    /// 
    /// TBD: Need to standardize this wait time wait after Map Notification (since teletype
    /// time is not standardized)
    /// </summary>
    private bool HandleSnowReaction(float pauseTime)
    {
        if (!IsSnowReaction)
            return false;
        
        game.ChangeStateCutScene();

        StartCoroutine(WaitToReact());

        didSnowReaction = true;
        return true;

        IEnumerator WaitToReact()
        {
            yield return new WaitForSeconds(pauseTime);
            Script_DialogueManager.DialogueManager.StartDialogueNode(OnSnowReactionNode);
        }
    }

    // Entry point for opening snow reaction dialogue on Map Notification done
    private void HandleOpeningSnowNoSpecial()
    {
        didMapNotificationDoneEvent = true;
        
        HandleSnowReaction(beforeSnowReactionOpeningTime);
    }

    private void OnItemPickUp(string itemId)
    {
        if (itemId == wellsWorldBehaviors[0].LastWellMap.Item.id)
        {
            didPickUpLastWellMap = true;
            SetMapsActive(false);
        }

        if (itemId == wellsWorldBehaviors[0].SpeedSeal.Item.id)
        {
            didPickUpSpeedSeal = true;
            SetSpeedSealActive(false);
        }
    }

    private void OnPuzzleSuccess(string puzzleId)
    {
        if (puzzleId == wellsPuzzleController.PuzzleId)
        {
            game.ChangeStateCutScene();

            // Play Freeze Timeline
            var currentWorldTileVCam = GetComponent<Script_WorldTilesController>()
                .OriginWorldTile?.GetComponent<Script_WellsWorldBehavior>()?.VirtualCamera;
            
            GetComponent<Script_TimelineController>().BindVirtualCameraAndPlayFromDirector(
                0, 0, currentWorldTileVCam
            );

            StartHeavySnow();
        }
    }

    private void OnScarletCipherPickUp(int scarletCipherId)
    {
        if (scarletCipherId == scarletCipherPieces[0].ScarletCipherId)
        {
            foreach (var scarletCipherPiece in scarletCipherPieces)
            {
                scarletCipherPiece.UpdateActiveState();
            }
        }
    }

    public void SetFireplaceExitActive(bool isActive)
    {
        wellsWorldBehaviors.ForEach(
            behavior => {
                behavior.FireplaceExit.SetInteractionActive(isActive);
                
                // If the Exit is enabled, then there is no fire, so
                // the text commenting on the fire should be hidden.
                behavior.FireText.gameObject.SetActive(!isActive);
            }
        );
    }

    // Will also open Fireplace Exit.
    public void StartHeavySnow()
    {
        wellsWorldBehaviors.ForEach(
            behavior => behavior.HeavySnow.gameObject.SetActive(true)
        );
    }

    public void StopHeavySnow()
    {
        wellsWorldBehaviors.ForEach(
            behavior => behavior.HeavySnow.gameObject.SetActive(false)
        );
    }

    // Will also affect Fireplace Exit state.
    public void SetSmallSnowActive(bool isActive)
    {
        wellsWorldBehaviors.ForEach(
            behavior => behavior.SmallSnow.gameObject.SetActive(isActive)
        );
    }

    /// <summary>
    /// When Player destroys one Well, hide all others that are currently not visible,
    /// so Tiles remain in sync. The current Well will be handled by its respective Timeline sequence.
    /// </summary>
    public void OnFrozenWellDie(Script_FrozenWellCrackableStats iceStats)
    {
        Script_FrozenWell destroyedFrozenWell = iceStats.GetComponent<Script_FrozenWell>();

        Dev_Logger.Debug($"Frozen Well destroyed: {destroyedFrozenWell}");

        if (destroyedFrozenWell == null)
            return;

        // If one of the frozen wells destroyed is 1 we are tracking, then destroy
        // all frozen wells.
        Script_FrozenWell matchingFrozenWell = frozenWells.FirstOrDefault(
            frozenWell => frozenWell == destroyedFrozenWell
        );

        Dev_Logger.Debug($"Matching Frozen Well destroyed: {matchingFrozenWell}");

        if (matchingFrozenWell != null)
        {
            foreach (Script_FrozenWell frozenWell in frozenWells)
            {
                // Skip the current Well which will be handled by its Timeline.
                if (matchingFrozenWell == frozenWell)
                    continue;

                Dev_Logger.Debug($"Setting frozenWell inactive: {frozenWell.gameObject.name}");
                frozenWell.gameObject.SetActive(false);
            }
        }
    }

    // When an ice block shatters, check which one it is, hide all the rest of them.
    public void OnShatter(Script_CrackableStats crackableStats)
    {
        wellsWorldBehaviors.ForEach(behavior => {
            if (behavior.SpeedSealIceBlock == crackableStats)
            {
                wellsWorldBehaviors.ForEach(_behavior => {
                    if (_behavior.SpeedSealIceBlock != crackableStats)
                        _behavior.SpeedSealIceBlock.gameObject.SetActive(false);
                });
            }
        });

        wellsWorldBehaviors.ForEach(behavior => {
            if (behavior.LastWellMapIceBlock == crackableStats)
            {
                wellsWorldBehaviors.ForEach(_behavior => {
                    if (_behavior.LastWellMapIceBlock != crackableStats)
                        _behavior.LastWellMapIceBlock.gameObject.SetActive(false);
                });
            }
        });
    }

    // ----------------------------------------------------------------------
    // Next Node Action START

    public void SnowReactionDone()
    {
        if (isSpecialIntroFraming)
        {
            Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
                isOpen: false,
                framing: Script_UIAspectRatioEnforcerFrame.Framing.ConstantDefault,
                cb: game.ChangeStateInteract
            );

            isSpecialIntroFraming = false;
        }
        else
        {
            game.ChangeStateInteract();
        }
    }
    
    public void CheckPlayerHasLastSpellRecipeBook()
    {
        game.ChangeStateCutScene();
        
        int slot = -1;
        bool hasBook = game.GetItemsInventoryItem(lastSpellRecipeBookItem.id, out slot) != null;

        if (hasBook)
        {
            // Force fade out of current full art because it is a "Leave Up" node. Success flow has a longer
            // pause where Moose portrait will be removed and then fade back in after the pause.
            var fullArtManager = Script_FullArtManager.Control;
            fullArtManager.HideFullArt(
                fullArtManager.activeFullArt,
                successFullArtFadeOutSpeed,
                () => {
                    StartCoroutine(WaitToReact(pauseBeforeNewNodeSuccessTime, StartSuccessDialogue));
                }
            );
        }
        else
            StartCoroutine(WaitToReact(pauseBeforeNewNodeFailTime, StartFailDialogue));

        IEnumerator WaitToReact(float t, Action cb)
        {
            yield return new WaitForSeconds(t);

            if (cb != null)
                cb();
        }

        void StartSuccessDialogue()
        {
            Script_DialogueManager.DialogueManager.StartDialogueNode(OnHasLastSpellRecipeBookNode);

            game.RemoveItemFromInventory(lastSpellRecipeBookItem);
        }

        void StartFailDialogue()
        {
            Script_DialogueManager.DialogueManager.StartDialogueNode(OnMissingLastSpellRecipeBookNode);
        }
    }

    public void GiveSticker()
    {
        Dev_Logger.Debug("-------- MOOSE GIVES PLAYER STICKER --------");
        
        StartCoroutine(WaitToTalk());

        IEnumerator WaitToTalk()
        {
            yield return new WaitForSeconds(pauseBeforeIntroNodeTime);
            
            Script_DialogueManager.DialogueManager.StartDialogueNode(OnMooseGiveItemDoneNode);
        }
    }

    public void UpdateMooseName()
    {
        Dev_Logger.Debug("-------- UPDATING MOOSE NAME --------");
        Script_Names.UpdateMoose();
    }

    // Moose and Suzette exit. Cut scene of paintings being done.
    public void MooseQuestDone()
    {
        game.ChangeStateCutScene();
        
        isMooseQuestDone                = true;
        isCurrentMooseQuestComplete     = true;
        
        StartCoroutine(WaitPlayPaintingQuestTimeline());

        IEnumerator WaitPlayPaintingQuestTimeline()
        {
            yield return new WaitForSeconds(waitTimeBeforeCastMooseFinalSpell);            
            
            // Play Timeline for finishing Moose's quest
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 1);
        }
    }

    public void OnMooseCheckItemDialogueDone()
    {
        // Also set all Moose to cooldown
        foreach (var Moose in Mooses)
            Moose.StartDialogueCoolDown();
        
        game.NextFrameChangeStateInteract();
    }

    public void WellsInitialState()
    {
        foreach (Script_FrozenWell frozenWell in frozenWells)
            frozenWell.InitialState();
    }
    
    // ----------------------------------------------------------------------
    // Timeline Signals

    public void FreezeWells()
    {
        // Play SFX Once here and disable for the Wells so all don't play at once.
        foreach (Script_FrozenWell frozenWell in frozenWells)
            frozenWell.Freeze(_isSFXOn: false);

        GetComponent<AudioSource>().PlayOneShot(
            Script_SFXManager.SFX.Freeze, Script_SFXManager.SFX.FreezeVol
        );
    }

    /// <summary>
    /// If haven't played the Snow Reaction yet, play it first, and let it handle game state.
    /// </summary>
    public void EndFreezeCutScene()
    {
        if (!HandleSnowReaction(beforeSnowReactionShortTime))
            game.ChangeStateInteract();
    }

    // Called after Moose checks Player has Last Spell Book.
    public void MooseExit()
    {
        foreach (var Moose in Mooses)
            Moose.FinalSpellExit();
    }

    public void HandleScarletCipherPieces()
    {
        foreach (var scarletCipherPiece in scarletCipherPieces)
        {
            scarletCipherPiece?.UpdateActiveState();
        }
    }

    public void FinishQuestPaintings()
    {
        ballroomPaintingEntrance.DonePainting();

        foreach (var paintingEntrance in paintingEntrances)
        {
            paintingEntrance.DonePainting();
        }
    }

    public void OnWellsWorldPaintingQuestDone()
    {
        var transitionManager = Script_TransitionManager.Control;
        transitionManager.OnCurrentQuestDone(
            allQuestsDoneCb: () =>
            {
                transitionManager.FinalCutSceneAwakening();
            }, 
            defaultCb: () =>
            {
                HandlePlayFaceOff(OnSuccessDone);
            },
            waitType: Script_TransitionManager.FinalNotificationWaitTimes.WellsWorld
        );

        // Face Off is not played if Final Cut Scene Sequence should play.
        void HandlePlayFaceOff(Action cb)
        {
            if (!didPlayFaceOff)
            {
                var PRCSManager = Script_PRCSManager.Control;

                PRCSManager.TalkingSelfSequence(() => {
                    PRCSManager.PlayFaceOffTimeline(() => {
                        cb();
                        didPlayFaceOff = true;
                    });
                },
                isFaceOff: true);
            }
            else
            {
                cb();
            }
        }

        void OnSuccessDone()
        {
            // Fade back in level bgm unless it's KTV2 puzzle
            var bgm = Script_BackgroundMusicManager.Control;
            bgm.UnPause();
            bgm.FadeIn(game.ChangeStateInteract, Script_TransitionManager.FadeTimeSlow, Const_AudioMixerParams.ExposedBGVolume);
        }
    }

    // ----------------------------------------------------------------------
    // Timeline Signals - Special Intro Only
    
    // Wells World Intro Timeline
    public void OnFrameAfterStartIntro()
    {
        transitionManager.TimelineRemoveBlackScreen(isOver: false);
    }
    
    // Wells World Intro Timeline
    public void PlayBgmDroneIntro()
    {
        droneBgmPlayerIntro.FadeInPlay(null, droneFadeInTimeIntro);
    }
    
    // Wells World Intro Timeline
    public void SetScanlineTransitionGlitch(bool isOn)
    {
        if (isOn)
        {
            glitchFXManager.SetScanlineTransition();
            glitchFXManager.SetBlend(1f);
        }
        else
        {
            glitchFXManager.SetDefault();
            glitchFXManager.SetBlend(0f);
        }
    }

    // Wells World Intro Timeline
    // Switch camera
    public void PlayCustomMapNotification()
    {
        Script_MapNotificationsManager.Control.PlayMapNotification(
            MapName,
            type: Script_MapNotificationsManager.Type.SpecialIntro,
            isSFXOn: true
        );
        didMapNotification = true;

        // Set new vcam in background for when timeline ends. Required to remain on the zoomed out shot on timeline end.
        // Note: introZoomOutGameVCam's custom blend with Main needs to be set to "cut" for transition to
        // be seamless after timeline
        introZoomOutGameVCam.SetPriority(2);
    }

    // ----------------------------------------------------------------------

    private void SetMapsActive(bool isActive)
    {
        wellsWorldBehaviors.ForEach((behavior) => {
            if (behavior.LastWellMap != null)
                behavior.LastWellMap.gameObject.SetActive(isActive);
        });
    }

    private void SetSpeedSealActive(bool isActive)
    {
        wellsWorldBehaviors.ForEach((behavior) => {
            if (behavior.SpeedSeal != null)
                behavior.SpeedSeal.gameObject.SetActive(isActive);
        });
    }

    private void SetMoosesActive(bool isActive)
    {
        foreach (var Moose in Mooses)
            Moose.gameObject.SetActive(isActive);
    }

    private void SetSuzettesActive(bool isActive)
    {
        foreach (var Suzette in Suzettes)
            Suzette.gameObject.SetActive(isActive);
    }

    // ------------------------------------------------------------------
    // Intro Only
    
    /// <summary>
    /// Fade Special Case, for Intro setup
    /// To mark it a special case, MUST set custom fade behavior's IsSpecialFadeIn & SpecialCaseFadeInTime
    /// </summary>
    public void SpecialCaseFadeIn()
    {
        if (IsSpecialIntro)
        {
            // Note: MUST SET for custom fade behavior to work properly
            levelCustomFadeBehavior.SpecialCaseFadeInTime = specialCaseFadeInTime;
            // Note: MUST SET for custom fade behavior to work properly
            levelCustomFadeBehavior.IsSpecialFadeIn = true;
        }
    }
    
    /// <summary>
    /// Wait In Black Special Case, for Intro setup
    /// To mark it a special case, MUST set custom fade behavior's IsSpecialWaitInBlack & SpecialCaseWaitInBlackTime
    /// </summary>
    public void SpecialCaseWaitInBlack()
    {
        if (IsSpecialIntro)
        {
            // Note: MUST SET for custom fade behavior to work properly
            levelCustomFadeBehavior.SpecialCaseWaitInBlackTime = specialCaseWaitInBlackTime;
            // Note: MUST SET for custom fade behavior to work properly
            levelCustomFadeBehavior.IsSpecialWaitInBlack = true;
        }
    }

    private void PlaySpecialIntro()
    {
        Script_BackgroundMusicManager.Control.SetVolume(1f, Const_AudioMixerParams.ExposedBG2Volume);
        
        StartCoroutine(WaitToPlay());

        IEnumerator WaitToPlay()
        {
            yield return new WaitForSeconds(waitToPlayIntroDirectorTime);

            introDirector.Play();

            Dev_Logger.Debug($"Playing special intro Time: {Time.time}");   
        }
    }

    private void OnMapNotificationTeletypeDone(bool isWorldPaintingIntro)
    {
        Dev_Logger.Debug($"OnMapNotificationTeletypeDone isWorldPaintingIntro {isWorldPaintingIntro}");
        
        if (!isWorldPaintingIntro)
            return;
        
        droneBgmPlayerIntro.FadeOutStop(null, droneFadeOutTimeIntro);
        
        StartCoroutine(WaitToFadeInBlackScreen());

        IEnumerator WaitToFadeInBlackScreen()
        {
            yield return new WaitForSeconds(waitToFadeInBlackScreenTime);
            
            // Fade black screen in
            transitionManager.TimelineFadeIn(
                fadeInBlackScreenTimeIntro,
                () => {
                    StartCoroutine(WaitToFadeOutBlackScreen());
                },
                isOver: false
            );
        }

        IEnumerator WaitToFadeOutBlackScreen()
        {
            // Revert priority that was set during Intro Timeline
            introDirector.Stop();
            introZoomOutGameVCam.SetPriority(0);

            // Revert shadow distance
            Script_GraphicsManager.Control.SetDefaultShadowDistance();
             
             // Immediately fade in bgm
            var bgm = Script_BackgroundMusicManager.Control;
            bgm.PlayFadeIn(
                bgm.WellsWorldTheme,
                null,
                true,
                fadeInBgmTimeIntro,
                Const_AudioMixerParams.ExposedBGVolume,
                startTime: 0f,
                isForceNewStartTime: true
            );
            
            yield return new WaitForSeconds(blackScreenTimeIntro);

            // Fade black screen out & remove map notification
            var mapNotificationManager = Script_MapNotificationsManager.Control;
            transitionManager.TimelineFadeOut(
                fadeOutBlackScreenTimeIntro,
                () => {
                    mapNotification.Close(
                        () => {
                            // Must reinitiate mapNotification for World Paintings because the default
                            // OnTeletypeDone is not called when it's a Special Intro
                            mapNotificationManager.InitialState();
                            
                            game.ChangeStateInteract();
                            didSpecialIntro = true;
                            
                            if (weatherFXManager.IsSnowDay)
                                HandleSnowReaction(beforeSnowReactionShortTime);
                        },
                        mapNotificationManager.SpecialIntroFadeOutTime
                    );
                },
                isOver: false
            );
        }
    }

    // ------------------------------------------------------------------
    // Trailer Only

    private void HandleTrailerPan()
    {
        if (Input.GetButtonDown(Const_KeyCodes.TrailerCam))
        {
            trailerDirector.Stop();

            StartCoroutine(WaitToPlay());
        }

        IEnumerator WaitToPlay()
        {
            yield return new WaitForSeconds(waitToPlayTrailerDirectorTime);

            trailerDirector.Play();
        }
    }

    // ------------------------------------------------------------------

    public override void Setup()
    {
        wellsPuzzleController.InitialState();

        introZoomOutGameVCam.SetPriority(0);

        // Always leave fireplace open
        SetFireplaceExitActive(true);

        // Only Spawn Last Well Map if Player has not picked it up.
        if (didPickUpLastWellMap)
            SetMapsActive(false);
        else
            SetMapsActive(true);
        
        if (didPickUpSpeedSeal)
            SetSpeedSealActive(false);
        else
            SetSpeedSealActive(true);

        if (isCurrentMooseQuestComplete)
        {
            SetMoosesActive(false);
            SetSuzettesActive(false);
        }
        else
        {
            SetMoosesActive(true);
            SetSuzettesActive(true);
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Script_LevelBehavior_42))]
    public class Script_LevelBehavior_42Tester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_LevelBehavior_42 t = (Script_LevelBehavior_42)target;
            if (GUILayout.Button("Activate Fireplace Exit"))
            {
                t.SetFireplaceExitActive(true);
            }

            if (GUILayout.Button("Disable Fireplace Exit"))
            {
                t.SetFireplaceExitActive(false);
            }

            if (GUILayout.Button("Start Heavy Snow"))
            {
                t.StartHeavySnow();
            }

            if (GUILayout.Button("Stop Heavy Snow"))
            {
                t.StopHeavySnow();
            }

            GUILayout.Space(12);

            if (GUILayout.Button("Destroy Frozen Wells"))
            {
                t.OnFrozenWellDie(t.frozenWells[0].GetComponent<Script_FrozenWellCrackableStats>());
            }

            GUILayout.Space(12);

            if (GUILayout.Button("Moose Quest Done"))
            {
                t.MooseQuestDone();
            }

            if (GUILayout.Button("Wells Initial State"))
            {
                t.WellsInitialState();
            }

            GUILayout.Space(12);

            if (GUILayout.Button("Play Intro"))
            {
                t.PlaySpecialIntro();
            }

            GUILayout.Space(12);
            
            if (GUILayout.Button("Setup True Ending On Complete"))
            {
                Dev_GameHelper gameHelper = Script_Game.Game.gameObject.GetComponent<Dev_GameHelper>();
                gameHelper.SetQuestsDoneExplicit(3);
            }
        }
    }
    #endif
}
