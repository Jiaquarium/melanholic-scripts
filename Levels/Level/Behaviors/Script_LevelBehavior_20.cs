using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Playables;
using UnityEngine.Timeline;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Add Cinemachine track to Timeline to control VCams via Timeline
/// Timeline: Use a "controller" PlayableDirector if controlling multiple things
/// </summary>
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_20 : Script_LevelBehavior
{
    public const string MapName = "Ballroom";
    
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool isKingIntroCutSceneDone;
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
    [SerializeField] private Script_DemonNPC PecheMelba;
    [SerializeField] private Script_DemonNPC Suzette;
    [SerializeField] private Script_DemonNPC Moose;
    [SerializeField] private Script_DemonNPC Ursie;
    [SerializeField] private Script_DemonNPC KingEclaire;

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

        SetupCycleConditions();

        if (
            game.IsEileensMindQuestDone
            && game.RunCycle == Script_RunsManager.Cycle.Weekday
        )
        {
            Script_GlitchFXManager.Control.SetBlend(1f);
            isGlitched = true;
            
            // Only change Bgm to "disturbing" when actually in this room, not from outside cut scene
            // (e.g. Grand Mirror Cut Scene Timeline)
            if (game.levelBehavior == this)
                Script_BackgroundMusicManager.Control.Play(isGlitchedBGM);
        }
    }

    protected override void OnDisable()
    {
        Script_GameEventsManager.OnLevelInitComplete    -= OnLevelInitCompleteEvent;

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

    public void OnLevelInitCompleteEvent()
    {
        if (!didMapNotification)
        {
            Script_MapNotificationsManager.Control.PlayMapNotification(MapName, () => {
                HandlePlayIdsRunAwayTimeline();
            });
            didMapNotification = true;
        }
        else
        {
            HandlePlayIdsRunAwayTimeline();
        }

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

    // ----------------------------------------------------------------------
    // King's Intro Unity Events, Next Node Actions
    
    public void KingsIntroTimeline()
    {
        // Pause King's walking Timeline.

        // Check for Psychic Duck
        bool isPsychicDuckActive = Script_ActiveStickerManager.Control.IsActiveSticker(Const_Items.PsychicDuckId);
        if (isPsychicDuckActive && !isKingIntroCutSceneDone && !didStartKingsIntro)
        {
            didStartKingsIntro = true;

            game.ChangeStateCutScene();

            KingEclaire.State = Script_StaticNPC.States.Dialogue;

            Script_BackgroundMusicManager.Control.FadeOutMed(null, Const_AudioMixerParams.ExposedBGVolume);

            Script_TransitionManager.Control.TimelineFadeIn(timelineFaderFadeInTime, () => {
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
            ((TimelineAsset)KingEclaire.MyDirector.playableAsset).editorSettings.fps;
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
            bgm.PlayFadeIn(bgm.CurrentClipIndex, outputMixer: Const_AudioMixerParams.ExposedBGVolume);
        });

        void OnRemoveLetterBoxDone()
        {
            game.ChangeStateInteract();
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
        Debug.Log($"{name} Setup from Timeline");
        
        Ids.gameObject.SetActive(false);
    }

    // ----------------------------------------------------------------------

    private void SetDynamicSpectersActive(bool isActive)
    {
        Kaffe.gameObject.SetActive(isActive);
        Latte.gameObject.SetActive(isActive);
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
        // Ero reports of missing Ids.
        if (game.RunCycle == Script_RunsManager.Cycle.Weekend)
        {
            // Handle Ero Event Cycle
            if (Script_EventCycleManager.Control.IsIdsDead())
                Ero.gameObject.SetActive(true);
            else
                Ero.gameObject.SetActive(false);

            // Remove all NPCs except for King, they've gone to their respective rooms.
            SetDynamicSpectersActive(false);
            
            // Open Painting Entrances.
            SetPaintingEntrancesActive(true);
        }
        else
        {
            Ero.gameObject.SetActive(false);
            SetDynamicSpectersActive(true);
            
            // Ignore setting entrances to disabled if we're in Grand Mirror room, meaning
            // we're in the middle of the World Painting Reveal cut scene.
            if (!game.IsInGrandMirrorRoom())
                SetPaintingEntrancesActive(false);
        }
    }
    
    public override void Setup()
    {
        // Ids runs away on tutorial run.
        if (ShouldPlayIdsIntro())
            Ids.gameObject.SetActive(true);
        else
            Ids.gameObject.SetActive(false);
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
    }
}
#endif