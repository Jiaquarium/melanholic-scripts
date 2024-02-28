using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Add Cinemachine track to Timeline to control VCams via Timeline
/// Timeline: Use a "controller" PlayableDirector if controlling multiple things
/// 
/// NPC Walk Paths:
/// Ursie
///     - walk from Elder to stopping position as if just spoke with Elder about the infestation
///     - timeline should repeat until Ursie is at Default/Resting position, then will spawn there
/// </summary>
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_20 : Script_LevelBehavior
{
    public static string MapName = Script_Names.Ballroom;
    
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool isKingIntroCutSceneDone;
    public bool isMyneR2CutsceneDone;
    public bool isMyneTrueEndingCutsceneDone;
    
    // Archives
    public Seasons season;
    public bool isPuzzleComplete;
    public bool entranceCutSceneDone;
    public bool didPickUpMasterKey;
    public bool didUnlockMasterLock;
    /* ======================================================================= */

    [SerializeField] private int isGlitchedBGM;
    
    [SerializeField] private float timelineFaderFadeInTime;
    [SerializeField] private Script_Marker KingIntroPlayerSpawn;
    [SerializeField] private int KingEclaireTimelineMidpointFrame;

    // -------------------------------------------------------------------------------------
    // NPCs
    [SerializeField] private Script_DemonNPC Ero;
    
    [SerializeField] private Script_DemonNPC Kaffe;
    [SerializeField] private Script_DemonNPC Latte;
    [SerializeField] private GameObject KaffeLatteParent;
    [SerializeField] private Script_DemonNPC PecheMelba;
    [SerializeField] private Script_DemonNPC Suzette;
    [SerializeField] private Script_DemonNPC Moose;
    [SerializeField] private Script_DemonNPC Ursie;
    [SerializeField] private PlayableDirector UrsieDirector;
    [SerializeField] private List<TimelineAsset> UrsieWalkPathTimelines;
    [SerializeField] private Script_DemonNPC KingEclaire;
    [SerializeField] private PlayableDirector MyneDirector;
    [SerializeField] private PlayableDirector MyneTrueEndingDirector;
    [SerializeField] private Animator MyneNPC;
    [SerializeField] private Animator MyneNPCTrueEnding;

    [SerializeField] private Script_DemonNPC Ids;

    // -------------------------------------------------------------------------------------
    // Painting Entrances to the "Worlds"
    [SerializeField] private Script_InteractablePaintingEntrance WellsWorldPaintingEntrance;
    [SerializeField] private Script_InteractablePaintingEntrance CelestialGardensWorldPaintingEntrance;
    [SerializeField] private Script_InteractablePaintingEntrance XXXWorldPaintingEntrance;
    [SerializeField] private Script_InteractableObjectText blankCanvasesText;

    // -------------------------------------------------------------------------------------
    // Elder's Intro
    [SerializeField] private Script_BgThemePlayer eldersTragedyBgThemePlayer;

    // -------------------------------------------------------------------------------------
    // Dialogue
    [SerializeField] private Script_DialogueNode selfPortraitThought;
    [SerializeField] private Script_DialogueNode[] weekendElderPsychicNodes;
    
    // -------------------------------------------------------------------------------------
    // Myne Cut Scene - R2
    [SerializeField] private float waitAfterInitToPlayMyneR2CutsceneTime;
    [Tooltip("Start time to avoid pause at beginning of track")]
    [SerializeField] private float lostMemoriesStartTime;
    [SerializeField] private Script_VCamera myneR2CutsceneVCam;
    [SerializeField] private Script_ExitMetadata celestialGardensExit;
    private bool isOnInitMyneR2Cutscene;

    // -------------------------------------------------------------------------------------
    // Myne Cut Scene - True Ending
    [SerializeField] private float waitAfterInitToPlayMyneTrueEndingCutsceneTime;
    [SerializeField] private Script_VCamera myneTrueEndingCutsceneVCam;
    [SerializeField] private Script_VCamera myneTrueEndingPart2CutsceneVCam;
    
    [SerializeField] private Script_ExitMetadata wellsWorldExit;
    [SerializeField] private Script_ExitMetadata xxxWorldExit;
    [SerializeField] private Script_ExitMetadata urselksHallExit;

    [SerializeField] private float myneLastOnesFadeInTime;
    
    [SerializeField] private Script_Marker myneTourFocalPoint;
    private bool isOnInitMyneTrueEndingCutscene;
    
    // -------------------------------------------------------------------------------------
    // Deprecated
    
    [SerializeField] Script_SeasonsTree[] ManTrees;
    [SerializeField] Script_SeasonsTree[] WomanTrees;
    
    [SerializeField] private Script_VCamera vCamMid;
    [SerializeField] private Script_VCamera vCamEntrance;
    
    [SerializeField] private float waitToChangeCameraTime; // 1s
    [SerializeField] private float waitToChangeSeasonTime; // 1.5s camera + .5s pause
    [SerializeField] private float seasonChangeWaitTime; //  1.25s slow season fade + .75s pause
    [SerializeField] private float revertCameraWaitTime; //  1.5s

    // -------------------------------------------------------------------------------------
    private Script_TimelineController timelineController;

    private bool didMapNotification;
    private bool didIdsRun;
    public bool didStartKingsIntro;

    private float fadeTime;

    private bool isGlitched;
    
    private Dictionary<string, Seasons> SeasonStonesEnums = new Dictionary<string, Seasons>{
        {"collectible_winter-stone",    Seasons.Winter},
        {"collectible_spring-stone",    Seasons.Spring},
        {"collectible_summer-stone",    Seasons.Summer},
        {"collectible_autumn-stone",    Seasons.Autumn},
    };

    private Dictionary<Seasons, int> SeasonInt = new Dictionary<Seasons, int>{
        {Seasons.Winter,            0},
        {Seasons.Spring,            1},
        {Seasons.Summer,            2},
        {Seasons.Autumn,            3},
    };

    protected override void OnEnable()
    {
        Script_GameEventsManager.OnLevelInitComplete    += OnLevelInitCompleteEvent;
        Script_GameEventsManager.OnLevelBlackScreenDone += OnLevelBlackScreenDone;

        SetupCycleConditions();
    }

    protected override void OnDisable()
    {
        Script_GameEventsManager.OnLevelInitComplete    -= OnLevelInitCompleteEvent;
        Script_GameEventsManager.OnLevelBlackScreenDone -= OnLevelBlackScreenDone;

        if (isGlitched)
        {
            Script_GlitchFXManager.Control.SetBlend(0f);
            isGlitched = false;
        }
    }

    void Awake()
    {
        timelineController = GetComponent<Script_TimelineController>();
    }

    protected override void Update()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if(audioSource.isPlaying && audioSource.time > fadeTime)
        {
            audioSource.volume = 0f;
        }

        base.Update();
    }

    private void OnLevelBlackScreenDone()
    {
        if (!didMapNotification)
        {
            Script_MapNotificationsManager.Control.PlayMapNotification(
                MapName
            );
            didMapNotification = true;
        }
    }
    
    public void OnLevelInitCompleteEvent()
    {
        if (isOnInitMyneR2Cutscene)
        {
            game.ChangeStateCutScene();
            
            StartCoroutine(WaitToPlayMyneR2Cutscene());

            IEnumerator WaitToPlayMyneR2Cutscene()
            {
                yield return new WaitForSecondsRealtime(waitAfterInitToPlayMyneR2CutsceneTime);
                MyneDirector.Play();
            }

            isOnInitMyneR2Cutscene = false;
        }
        else if (isOnInitMyneTrueEndingCutscene)
        {
            game.ChangeStateCutScene();

            StartCoroutine(WaitToPlayMyneTrueEndingCutscene());

            IEnumerator WaitToPlayMyneTrueEndingCutscene()
            {
                yield return new WaitForSecondsRealtime(waitAfterInitToPlayMyneTrueEndingCutsceneTime);
                MyneTrueEndingDirector.Play();
            }

            isOnInitMyneTrueEndingCutscene = false;
        }
        else
            HandlePlayIdsRunAwayTimeline();

        // After Map Notification, Ids should lead the way on Tutorial Run.
        void HandlePlayIdsRunAwayTimeline()
        {
            if (ShouldPlayIdsIntro())
            {
                game.ChangeStateCutScene();
                GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(1, 1);
            }
        }
    }

    public void SetKingEclaireActive(bool isActive)
    {
        KingEclaire.gameObject.SetActive(isActive);
    }

    /// <summary> ==============================================================================
    /// NextNodeAction(s) Start 
    /// </summary> =============================================================================
    public void UpdateKingEclaire()
    {
        Script_Names.UpdateKingEclaire();
    }

    public void UpdateUrsie()
    {
        Script_Names.UpdateUrsie();
    }

    public void OnSelfPortraitThoughtDone()
    {
        game.ChangeStateInteract();
    }

    // ----------------------------------------------------------------------
    // Trigger Unity Events, Next Node Actions

    // King's Intro Trigger
    public void KingsIntroTimeline()
    {
        // Check for Psychic Duck
        bool isPsychicDuckActive = Script_ActiveStickerManager.Control.IsActiveSticker(Const_Items.PsychicDuckId);
        if (isPsychicDuckActive && !isKingIntroCutSceneDone && !didStartKingsIntro)
        {
            didStartKingsIntro = true;

            game.ChangeStateCutScene();

            // Pause King's walking Timeline.
            KingEclaire.State = Script_StaticNPC.States.Dialogue;

            Script_BackgroundMusicManager.Control.FadeOutMed(null, Const_AudioMixerParams.ExposedBGVolume);

            Script_TransitionManager.Control.TimelineFadeIn(timelineFaderFadeInTime, () => {
                Dev_Logger.Debug($"Faded in Timeline Under after t {timelineFaderFadeInTime} sec");
                
                Script_Player p = game.GetPlayer();
                
                p.Teleport(KingIntroPlayerSpawn.Position);
                p.FaceDirection(Directions.Up);

                MoveKingEclaireToMidpoint();

                // King's Explanation of Sealing
                timelineController.PlayableDirectorPlayFromTimelines(0, 0);

                // Play Elder's Tragedy Song
                game.PauseBgMusic();
                eldersTragedyBgThemePlayer.Play();
                Script_BackgroundMusicManager.Control.SetVolume(1f, Const_AudioMixerParams.ExposedBGVolume);
            });
        }
    }

    // Self Portrait Thought Trigger
    public void SelfPortraitThought()
    {
        if (game.CycleCount != 0 || game.Run.dayId != Script_Run.DayId.mon)
            return;

        game.ChangeStateCutScene();
        Script_DialogueManager.DialogueManager.StartDialogueNode(selfPortraitThought);
    }

    // ----------------------------------------------------------------------
    // Timeline Signals
    
    // Called from KingsIntro after Black Screen fades out.
    public void OpenArtFrame()
    {
        Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
            isOpen: true,
            framing: Script_UIAspectRatioEnforcerFrame.Framing.ElderIntro
        );
    }

    // Set King's Position to center of Stage frame
    // Frame to Time conversion: https://forum.unity.com/threads/jump-to-frame.500709/    
    public void MoveKingEclaireToMidpoint()
    {
        KingEclaire.MyDirector.Pause();
        
        KingEclaire.MyDirector.time = KingEclaireTimelineMidpointFrame /
            ((TimelineAsset)KingEclaire.MyDirector.playableAsset).editorSettings.frameRate;
        KingEclaire.MyDirector.Evaluate();

        KingEclaire.FacePlayer();

        KingEclaire.State = Script_MovingNPC.States.Dialogue;
    }
    
    public void EndKingEclaireIntro()
    {
        isKingIntroCutSceneDone = true;
        Script_BackgroundMusicManager bgm = Script_BackgroundMusicManager.Control;
        
        Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
            isOpen: false,
            framing: Script_UIAspectRatioEnforcerFrame.Framing.ElderIntro,
            cb: OnRemoveLetterBoxDone
        );

        eldersTragedyBgThemePlayer.FadeOutStop(() => {
            bgm.SetVolume(0f, Const_AudioMixerParams.ExposedBGVolume);
            
            // Wait to fade in Bgm next frame to avoid clicking
            StartCoroutine(WaitNextFrameFadeInBgm());
        });

        IEnumerator WaitNextFrameFadeInBgm()
        {
            yield return null;
            
            bgm.PlayFadeIn(
                bgm.CurrentClipIndex,
                () => {
                    game.ChangeStateInteract();
                    
                    // Track Elder Intro Cursed cut scene for Achievement cut scene finishes up
                    Script_AchievementsManager.Instance.UpdateCursedCutScene(
                        Script_AchievementsManager.CursedCutScenes.ElderEclaire
                    );
                },
                outputMixer: Const_AudioMixerParams.ExposedBGVolume
            );
        }

        void OnRemoveLetterBoxDone()
        {
            game.CanvasesInitialState();
            
            // Face King in the proper direction upon timeline restarting.
            KingEclaire.FaceDirection(Directions.Left);
            KingEclaire.State = Script_StaticNPC.States.Interact;
        }
    }

    // Ids intro run away cut scene on initial tutorial run.
    public void OnIdsRunAwayTimelineDone()
    {
        game.ChangeStateInteract();
        didIdsRun = true;
    }

    public void TimelineSetup()
    {
        Dev_Logger.Debug($"{name} Setup from Timeline");
        
        Ids.gameObject.SetActive(false);
    }

    // MyneR2Cutscene Timeline
    public void PanToMyne()
    {
        // Switch VCam to Myne's R2 VCam
        Script_VCamManager.VCamMain.SetNewVCam(myneR2CutsceneVCam);
    }
    
    // MyneR2Cutscene Timeline
    public void RevertVCamMyneR2Cutscene()
    {
        // Switch back to main VCam (cut)
        Script_VCamManager.VCamMain.SwitchToMainVCam(myneR2CutsceneVCam);
        
        // Only deactivate NPC; we still need Director to be active to finish timeline.
        MyneNPC.gameObject.SetActive(false);
    }

    // MyneR2Cutscene Timeline
    public void PlayFadeInBgm()
    {
        var bgm = Script_BackgroundMusicManager.Control;
        bgm.PlayFadeIn(
            bgm.CurrentClipIndex,
            null,
            forcePlay: true,
            fadeTime: Script_BackgroundMusicManager.DefaultBgmFadeInTime,
            outputMixer: Script_BackgroundMusicManager.DefaultBgmLevelParam,
            startTime: lostMemoriesStartTime,
            isForceNewStartTime: true
        );
    }

    // MyneR2Cutscene Timeline
    public void OnMyneR2CutsceneDone()
    {
        game.ChangeStateInteract();
        MyneDirector.gameObject.SetActive(false);
        MyneDirector.Stop();
    }

    // MyneTrueEndingCutscene Timeline
    public void PanToMyneTrueEndingTour()
    {
        // Set Blend Update Method to avoid jerky movement (default blend cams are set to Fixed Update)
        // Switch VCam to Myne's True Ending VCam
        Script_VCamManager.VCamMain.SetCinemachineBlendUpdateMethod(CinemachineBrain.BrainUpdateMethod.LateUpdate);
        Script_VCamManager.VCamMain.SetNewVCam(myneTrueEndingCutsceneVCam);
    }

    // MyneTrueEndingCutscene Timeline
    public void CutToMyneTrueEndingTourPart2()
    {
        Script_VCamManager.VCamMain.SwitchBetweenVCams(myneTrueEndingCutsceneVCam, myneTrueEndingPart2CutsceneVCam);

        // Snap to Part2 VCam of Final Tour (walking down right side Ballroom) to avoid "catch up" motion at beginning
        game.SnapCam(
            myneTourFocalPoint.transform.position,
            myneTourFocalPoint.transform,
            myneTrueEndingPart2CutsceneVCam.CinemachineVirtualCamera
        );
    }

    // MyneTrueEndingCutscene Timeline
    public void FadeInLastOnesOnMyne()
    {
        var lastOnes = Script_ScarletCipherManager.Control.ScarletCipherLastOnes;

        lastOnes.StartTrackingTarget(MyneNPCTrueEnding.transform);
        lastOnes.FadeInOnPosition(
            MyneNPCTrueEnding.transform.position,
            myneLastOnesFadeInTime
        );
    }

    // MyneTrueEndingCutscene Timeline
    public void RevertVCamMyneTrueEndingTour()
    {
        // Switch back to main VCam (cut)
        Script_VCamManager.VCamMain.SwitchToMainVCam(myneTrueEndingPart2CutsceneVCam);
        Script_VCamManager.VCamMain.SetDefaultCinemachineBlendUpdateMethod();
        
        // Only deactivate NPC; we still need Director to be active to finish timeline.
        MyneNPCTrueEnding.gameObject.SetActive(false);

        // Reset Last Ones
        Script_ScarletCipherManager.Control.ScarletCipherLastOnes.ForceClose();
    }
    
    // MyneTrueEndingCutscene Timeline
    public void OnMyneTrueEndingTourDone()
    {
        game.ChangeStateInteract();
        MyneTrueEndingDirector.gameObject.SetActive(false);
        MyneTrueEndingDirector.Stop();
    }

    // ----------------------------------------------------------------------

    private void SetDynamicSpectersActive(bool isActive)
    {
        Kaffe.gameObject.SetActive(isActive);
        Latte.gameObject.SetActive(isActive);
        KaffeLatteParent.SetActive(isActive);
        PecheMelba.gameObject.SetActive(isActive);
        Suzette.gameObject.SetActive(isActive);
        Moose.gameObject.SetActive(isActive);
        Ursie.gameObject.SetActive(isActive);
    }

    public void SetPaintingEntrancesActive(bool isActive)
    {
        Script_InteractableObject.States paintingState;
        
        if (isActive)
        {
            paintingState = Script_InteractableObject.States.Active;
            blankCanvasesText.gameObject.SetActive(false);
        }
        else
        {
            paintingState = Script_InteractableObject.States.Disabled;
            blankCanvasesText.gameObject.SetActive(true);
        }
        
        WellsWorldPaintingEntrance.State            = paintingState;
        CelestialGardensWorldPaintingEntrance.State = paintingState;
        XXXWorldPaintingEntrance.State              = paintingState;
    }

    private bool ShouldPlayIdsIntro()
    {
        return !didIdsRun && Script_EventCycleManager.Control.IsLastElevatorTutorialRun();
    }

    private void SetupCycleConditions()
    {
        Ero.gameObject.SetActive(false);
        
        if (game.RunCycle == Script_RunsManager.Cycle.Weekend)
        {
            // Remove all NPCs except for King, they've gone to their respective rooms.
            SetDynamicSpectersActive(false);
            
            // Open Painting Entrances.
            SetPaintingEntrancesActive(true);

            SetupElderR2Dialogue();
        }
        else
        {
            // Handle glitch state where after returning to Ballroom after completing spike room
            // forbode something ominous.
            if (game.IsEileensMindQuestDone)
            {
                SetDynamicSpectersActive(false);
                
                Script_GlitchFXManager glitchFXManager = Script_GlitchFXManager.Control;
                glitchFXManager.SetDefault();
                glitchFXManager.SetBlend(1f);
                isGlitched = true;
                
                // Only change Bgm to "disturbing" when actually in this room, not from outside cut scene
                // (e.g. Grand Mirror Cut Scene Timeline)
                if (game.levelBehavior == this)
                    Script_BackgroundMusicManager.Control.Play(isGlitchedBGM);
                
                SetupElderR2Dialogue();
            }
            else
            {
                SetDynamicSpectersActive(true);
            }
            
            // Ignore setting entrances to disabled if we're in Grand Mirror room, meaning
            // we're in the middle of the World Painting Reveal cut scene.
            if (!game.IsInGrandMirrorRoom())
                SetPaintingEntrancesActive(false);
        }

        // Elder dialogue after Spike Room and R2 explains the others have returned to their worlds
        void SetupElderR2Dialogue() => KingEclaire.SwitchPsychicNodes(weekendElderPsychicNodes);
    }

    public override void Setup()
    {
        // Ids runs away on tutorial run.
        if (ShouldPlayIdsIntro())
        {
            Ids.gameObject.SetActive(true);

            // For Ids intro, Ursie should take the longer walk path
            UrsieDirector.playableAsset = UrsieWalkPathTimelines[0];
            UrsieDirector.Play();
        }
        else
        {
            Ids.gameObject.SetActive(false);

            // When Ids is not there, Ursie should take the shorter walk path, so bottom of map isn't too empty
            UrsieDirector.playableAsset = UrsieWalkPathTimelines[1];
            UrsieDirector.Play();
        }

        // If Ursie walk path is done, then start off at spawn position, no walk path
        if (Ursie.IsAutoMoveTimelineDone)
            UrsieDirector.playableAsset = null;
        
        Vector3 fromWellsWorldSpawn = wellsWorldExit.data.playerSpawn;
        Vector3 fromCelestialGardensSpawn = celestialGardensExit.data.playerSpawn;
        Vector3 fromXxxWorldSpawn = xxxWorldExit.data.playerSpawn;
        Vector3 fromUrselksHallSpawn = urselksHallExit.data.playerSpawn;

        Vector3 playerSpawnedPosition = game.GetPlayer().transform.position;
        
        bool fromWellsWorldDefaultEntrace = fromWellsWorldSpawn == playerSpawnedPosition;
        bool fromCelGardensDefaultEntrance = fromCelestialGardensSpawn == playerSpawnedPosition;
        bool fromXxxWorldDefaultEntrance = fromXxxWorldSpawn == playerSpawnedPosition;
        bool fromUrselksHallDefaultEntrance = fromUrselksHallSpawn == playerSpawnedPosition;
        
        Dev_Logger.Debug($"fromCelGardensDefaultEntrance {fromCelGardensDefaultEntrance} (fromCelestialGardensSpawn {fromCelestialGardensSpawn} playerSpawnedPosition: {playerSpawnedPosition})");
        
        if (GetIsImpliedR2Day2() && !fromCelGardensDefaultEntrance)
        {
            Script_BackgroundMusicManager.Control.PauseBgmOnSetup();
            
            MyneDirector.gameObject.SetActive(true);
            MyneTrueEndingDirector.gameObject.SetActive(false);

            MyneNPC.gameObject.SetActive(true);
            isOnInitMyneR2Cutscene = true;
            isMyneR2CutsceneDone = true;
        }
        // Avoid activating Final Tour timeline if player first arrives in Ballroom at any of these entrances, since
        // may appear buggy (Rin shouldn't be able to see Myne)
        else if (
            GetIsTrueEndingStart()
            && !fromWellsWorldDefaultEntrace
            && !fromCelGardensDefaultEntrance
            && !fromXxxWorldDefaultEntrance
            && !fromUrselksHallDefaultEntrance
        )
        {
            Script_BackgroundMusicManager.Control.PauseBgmOnSetup();

            MyneDirector.gameObject.SetActive(false);
            MyneTrueEndingDirector.gameObject.SetActive(true);

            MyneNPCTrueEnding.gameObject.SetActive(true);
            isOnInitMyneTrueEndingCutscene = true;
            isMyneTrueEndingCutsceneDone = true;
        }
        else
        {
            MyneDirector.gameObject.SetActive(false);
            MyneTrueEndingDirector.gameObject.SetActive(false);
        }

        // If Wells World or XXXWorld is done (and did not just complete them that day, to prevent playing the cutscene
        // upon exiting the world after completion) and Cel Gardens is not done ("R2 Day 2").
        // Also, ensure player not on the Cel Gardens entrance either (since Myne approaches the Cel Gardens painting)
        bool GetIsImpliedR2Day2()
        {
            return (
                (game.WellsWorldBehavior.isMooseQuestDone && !game.WellsWorldBehavior.isCurrentMooseQuestComplete)
                || (game.KTVRoom2Behavior.IsPuzzleComplete && !game.KTVRoom2Behavior.IsCurrentPuzzleComplete)
            )
            && !game.GardenLabyrinthBehavior.isPuzzleComplete
            && !isMyneR2CutsceneDone;
        }

        bool GetIsTrueEndingStart()
        {
            return !isMyneTrueEndingCutsceneDone && game.didGoodEnding;
        }
    }

    // -------------------------------------------------------------------------------------
    // Deprecated Old Seasons Utils
    public void ChangeSeason(string seasonStoneId, Action cb = null)
    {   
        if (isPuzzleComplete)   return;

        Seasons lastSeason = season;

        if (!SeasonStonesEnums.TryGetValue(seasonStoneId, out season))
            Debug.LogError($"Script_LevelBehavior_20: There is no season key {seasonStoneId}");
        
        /// CHANGE SEASON ANIMATION & SFX
        StartCoroutine(ChangeSeasonCutScene());

        IEnumerator ChangeSeasonCutScene()
        {
            game.ChangeStateCutScene();
            
            // TODO: could show some animation on drop 
            yield return new WaitForSeconds(waitToChangeCameraTime);
            Script_VCamManager.VCamMain.SetNewVCam(vCamMid);
            yield return new WaitForSeconds(waitToChangeSeasonTime);

            // SFX
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = Script_SFXManager.SFX.Rumble254ms;
            audioSource.volume = Script_SFXManager.SFX.Rumble254msVol;
            audioSource.Play();

            FadeInOutSeason();
            yield return new WaitForSeconds(seasonChangeWaitTime);

            Script_VCamManager.VCamMain.SwitchToMainVCam(vCamMid);
            yield return new WaitForSeconds(revertCameraWaitTime);

            game.ChangeStateInteract();
            if (cb != null)     cb();
        }

        void FadeInOutSeason()
        {
            // if the season is the same, don't change the Sprite
            if (season == lastSeason)   return;
            
            // fade out old season sprite if was one
            if (lastSeason != Seasons.None)
            {
                // get SeasonTree corresponding to it
                int seasonInt;
                SeasonInt.TryGetValue(lastSeason, out seasonInt);
                ManTrees[seasonInt].FadeOut(null);
                WomanTrees[seasonInt].FadeOut(null);
            }

            // fade in new season sprite
            if (season != Seasons.None)
            {
                // get SeasonTree corresponding to it
                int seasonInt;
                SeasonInt.TryGetValue(season, out seasonInt);
                ManTrees[seasonInt].FadeIn(null);
                WomanTrees[seasonInt].FadeIn(null);

                fadeTime = Script_Utils.GetFadeTime(
                    WomanTrees[seasonInt].fadeSpeed
                );
            }
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(Script_LevelBehavior_20))]
    public class Script_LevelBehavior_20Tester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_LevelBehavior_20 lb = (Script_LevelBehavior_20)target;
            if (GUILayout.Button("Move King Eclaire to Midpoint"))
            {
                lb.MoveKingEclaireToMidpoint();
            }
            
            if (GUILayout.Button("King Intro Timeline"))
            {
                lb.KingsIntroTimeline();
            }

            if (GUILayout.Button("Reset Map Notification & is Myne R2 Cutscene"))
            {
                lb.didMapNotification = false;
                lb.isMyneR2CutsceneDone = false;
            }
        }
    }
#endif
}
