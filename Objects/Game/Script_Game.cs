using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.Playables;
using Cinemachine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.Universal;
using System.Linq;

/// <summary>
/// Entry point for Game scene
/// </summary>
public class Script_Game : MonoBehaviour
{
    public static Script_Game Game;
    private const int SpawnLevelNo = 32;

    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public int level;
    public float totalPlayTime;
    
    // Private to avoid ending staying non-None in Editor after Dev'ing.
    [SerializeField] private Script_TransitionManager.Endings activeEnding;

    public int faceOffCounter;
    /* ======================================================================= */

    public Model_Levels Levels;
    public string state;
    public Model_PlayerState playerState;
    public Model_PlayerThoughts thoughts;
    public Script_PlayerThoughtsInventoryButton[] thoughtSlots;
    [SerializeField] private Script_ThoughtSlotHolder thoughtSlotHolder;
    public Vector3 levelZeroCameraPosition;
    
    public Script_DDRManager DDRManager;
    public Script_DDRHandler DDRHandler;
    
    public Script_InteractableObjectHandler interactableObjectHandler;
    public Script_InteractableObjectCreator interactableObjectCreator;
    public Script_DemonHandler demonHandler;
    public Script_DemonCreator demonCreator;
    public Script_MovingNPCCreator movingNPCCreator;
    public Script_CutSceneNPCCreator cutSceneNPCCreator;
    public Script_SavePointCreator savePointCreator;
    public Script_ReflectionCreator reflectionCreator;
    
    public Script_PlayerThoughtHandler playerThoughtHandler;
    public Script_PlayerThoughtsInventoryManager playerThoughtsInventoryManager;
    
    public PlayableDirector dieTimelineDirector;
    public Script_Exits exitsHandler;
    
    // ------------------------------------------------------------------
    // Specific Level Behaviors for state
    public Script_LevelBehavior_10 IdsRoomBehavior;
    public Script_LevelBehavior_20 BallroomBehavior;
    public Script_LevelBehavior_21 EileensRoomBehavior;
    public Script_LevelBehavior_22 SaloonBehavior;
    public Script_LevelBehavior_24 KTVRoom2Behavior;
    public Script_LevelBehavior_25 ElleniasRoomBehavior;
    public Script_LevelBehavior_26 EileensMindBehavior;
    public Script_LevelBehavior_33 bayV1Behavior;
    public Script_LevelBehavior_34 bayV2Behavior;
    public Script_LevelBehavior_42 WellsWorldBehavior;
    public Script_LevelBehavior_46 GardenLabyrinthBehavior;
    public Script_LevelBehavior_48 grandMirrorRoomBehavior;
    
    // ------------------------------------------------------------------
    // Level Behavior Exits & Entrances
    [SerializeField] private Script_ExitMetadataObject XXXWorldSaloonExit;
    [SerializeField] private Script_ExitMetadataObject grandMirrorRoomEntrance;

    // ------------------------------------------------------------------
    // Managers
    
    [SerializeField] private Script_SystemSettings systemSettings;
    public Script_DialogueManager dialogueManager;
    [SerializeField] private Script_SaveViewManager saveManager;
    [SerializeField] private Script_MenuController menuController;
    [SerializeField] private Script_InventoryAudioSettings canvasesAudioSource;
    [SerializeField] private Script_TransitionManager transitionManager;
    public Script_EntryManager entryManager;
    public Script_ThoughtManager thoughtManager;
    public Script_HintManager hintManager;
    public Script_FullArtManager fullArtManager;
    public Script_CutSceneManager cutSceneManager;
    [SerializeField] private Script_PRCSManager PRCSManager;
    [SerializeField] private Script_ArtFrameManager artFrameManager;
    [SerializeField] private Script_GraphicsManager graphicsManager;
    
    [SerializeField] private Script_ElevatorManager elevatorManager;
    [SerializeField] private Script_ClockManager clockManager;
    [SerializeField] private Script_TimeManager timeManager;
    
    [SerializeField] private Script_CutSceneActionHandler cutSceneActionHandler;
    [SerializeField] private Script_ScarletCipherManager scarletCipherManager;
    [SerializeField] private Script_StickerHolsterManager stickerHolsterManager;
    [SerializeField] private Script_ActiveStickerManager activeStickerManager;
    
    public Script_BackgroundMusicManager BGMManager;
    [SerializeField] private Script_SFXManager SFXManager;
    
    [SerializeField] private Script_VCamManager VCamManager;
    
    [SerializeField] private Script_HUDManager HUDManager;
    [SerializeField] private Script_RunsManager runsManager;
    
    [SerializeField] private Script_ItemPickUpTheatricsManager itemPickUpTheatricsManager;
    [SerializeField] private Script_Names namesManager;
    [SerializeField] private Script_MynesMirrorManager mynesMirrorManager;
    [SerializeField] private Script_WeatherFXManager weatherFXManager;
    
    [SerializeField] private Script_HitBoxDictionary hitBoxDictionary;
    
    [SerializeField] private Script_SaveGameControl saveGameControl;
    [SerializeField] private Script_EventCycleManager eventCycleManager;

    [SerializeField] private Script_MapNotificationsManager mapNotificationsManager;
    [SerializeField] private Script_DayNotificationManager dayNotificationManager;
    [SerializeField] private Script_TeletypeNotificationManager teletypeNotificationManager;
    [SerializeField] private Script_LightFXManager lightFXManager;
    [SerializeField] private Script_GlitchFXManager glitchFXManager;
    [SerializeField] private Script_WindManager windManager;

    [SerializeField] private Script_PianoManager pianoManager;
    [SerializeField] private Script_NotesTallyTracker notesTallyTracker;

    // ------------------------------------------------------------------
    // Canvases
    [SerializeField] private Script_AllCanvasGroupsParent canvasGroupsParent;
    [SerializeField] private Script_PersistentDropsContainer persistentDropsContainer;

    public Script_Player PlayerPrefab;
    public Script_AudioOneShotSource AudioOneShotSourcePrefab;

    public Font[] fonts;
    public Script_VCamera VCam;
    public Script_VCamera VCamDramaticZoom;
    public Transform playerContainer;
    public Transform bgThemeSpeakersContainer;
    public Transform tmpTargetsContainer;

    public Script_Entry[] entries = new Script_Entry[0];
    public GameObject grid;

    private Tilemap tileMap;
    private Tilemap[] extraTileMaps;
    private Script_StairsTilemap[] stairsTileMaps;
    private Script_WorldTile[] worldTiles;
    private Tilemap[] exitsTileMaps;
    private Tilemap entrancesTileMap;
    [SerializeField] private Tilemap pushableTileMap;
    [SerializeField] private Script_UIAspectRatioEnforcerFrame UIAspectRatioEnforcerFrame;

    [SerializeField] private Script_SettingsController settingsController;

    // ------------------------------------------------------------------
    // Levels
    public List<Script_StaticNPC> NPCs = new List<Script_StaticNPC>();
    public List<Script_MovingNPC> movingNPCs = new List<Script_MovingNPC>();
    public List<Script_CutSceneNPC> cutSceneNPCs = new List<Script_CutSceneNPC>();
    public List<Script_InteractableObject> interactableObjects = new List<Script_InteractableObject>();
    public List<Script_Switch> switches = new List<Script_Switch>();
    public List<Script_Pushable> pushables = new List<Script_Pushable>();
    public List<Script_Demon> demons = new List<Script_Demon>();
    public List<Script_AudioOneShotSource> audioOneShotSources = new List<Script_AudioOneShotSource>();
    
    [SerializeField] private int tutorialEndLevel;
    [SerializeField] private Script_LevelBehavior[] hotelLevelBehaviors;
    [SerializeField] private Script_LevelBehavior[] disabledPianoLevels;
    [SerializeField] private Script_LevelBehavior[] psychicLevels;
    
    // ------------------------------------------------------------------
    // Spawn points
    [SerializeField] private Transform newGameSpawnDestination;
    [SerializeField] private Script_ExitMetadataObject playerSpawn;
    [SerializeField] private List<Script_ExitMetadataObject> pianoSpawns;
    [SerializeField] private Script_ExitMetadataObject lastElevatorExit;
    
    // ------------------------------------------------------------------
    // Instance Data
    public Script_BgThemePlayer npcBgThemePlayer;
    
    public Script_LevelBehavior levelBehavior { get; private set; }
    
    public string lastState;
    public CinemachineBrain cinemachineBrain;
    public Vector3 gridOffset;
    
    private bool isLoadedGame = false;
    
    [SerializeField] private Script_Player player;
    [SerializeField] private Script_LanternFollower lanternFollower;
    [SerializeField] private Script_PlayerCameraTargetFollower cameraTargetFollower;
    private Script_PuppetMaster puppetMaster;

    [SerializeField] private Script_SavePoint savePoint; // max 1 per Level

    [SerializeField] private bool isHideHUD;
    
    // ------------------------------------------------------------------
    // State Properties
    public Script_TransitionManager.Endings ActiveEnding
    {
        get
        {
            if (IsRunDay(Script_Run.DayId.sun))
                activeEnding = Script_TransitionManager.Endings.True;
            
            return activeEnding;
        }
        set => activeEnding = value;
    }

    public bool IsSettingsOpen { get; set; }
    
    // ------------------------------------------------------------------
    // Run Properties
    public Script_Run Run
    {
        get => runsManager.Run;
    }
    
    public int RunIdx
    {
        get => runsManager.RunIdx;
    }

    public Script_RunsManager.Cycle RunCycle
    {
        get => runsManager.RunCycle;
    }

    public int CycleCount
    {
        get => runsManager.CycleCount;
    }

    public bool IsFirstMonday { get => CycleCount == 0 && Run.dayId == Script_Run.DayId.mon; }
    public bool IsFirstTuesday { get => CycleCount == 0 && Run.dayId == Script_Run.DayId.tue; }
    public bool IsFirstThursday { get => CycleCount == 0 && Run.dayId == Script_Run.DayId.thu; }

    public string GetPlayerDisplayDayName
    {
        get => runsManager.GetPlayerDisplayDayName(Run);
    }

    // ------------------------------------------------------------------
    // Tilemap Properties
    public Tilemap TileMap
    {
        get => tileMap;
    }

    public Tilemap[] ExtraTileMaps
    {
        get => extraTileMaps;
    }

    public Script_StairsTilemap[] StairsTileMaps
    {
        get => stairsTileMaps;
    }

    public Script_WorldTile[] WorldTiles
    {
        get => worldTiles;
    }

    public Tilemap EntranceTileMap
    {
        get => entrancesTileMap;
    }

    public Tilemap[] ExitTileMaps
    {
        get => exitsTileMaps;
    }

    public Tilemap PushableTileMap
    {
        get => pushableTileMap;
    }

    // ------------------------------------------------------------------
    // Level Properties
    public Script_LevelBehavior LastLevelBehavior { get; private set; }

    public List<Script_ExitMetadataObject> PianoSpawns
    {
        get => pianoSpawns;
    }

    public Script_LevelBehavior[] HotelLevelBehaviors
    {
        get => hotelLevelBehaviors;
    }

    public Script_ExitMetadataObject LastElevatorExit
    {
        get => lastElevatorExit;
    }

    public Script_LanternFollower LanternFollower
    {
        get => lanternFollower;
    }

    public Script_PlayerCameraTargetFollower CameraTargetFollower
    {
        get => cameraTargetFollower;
    }

    public Script_PuppetMaster PuppetMaster
    {
        get => puppetMaster;
        set => puppetMaster = value;
    }

    public bool IsUrselkSistersQuestsDone
    {
        get => ElleniasRoomBehavior.isCurrentPuzzleComplete && EileensMindBehavior.isCurrentPuzzleComplete;
    }

    public bool IsEileensMindQuestDone
    {
        get => EileensMindBehavior.isCurrentPuzzleComplete;
    }

    public bool IsInEileensRoom
    {
        get => levelBehavior == EileensRoomBehavior;
    }

    public bool IsInIdsRoom
    {
        get => levelBehavior == IdsRoomBehavior;
    }

    public bool IsInElleniasRoom
    {
        get => levelBehavior == ElleniasRoomBehavior;
    }

    public bool IsInBallroom
    {
        get => levelBehavior == BallroomBehavior;
    }

    public bool IsMelancholyPianoDisabled
    {
        get => disabledPianoLevels.FirstOrDefault(lvl => lvl == levelBehavior) != null;
    }

    public bool IsPsychicRoom
    {
        get => psychicLevels.FirstOrDefault(lvl => lvl == levelBehavior) != null;
    }

    /// <summary>
    /// Force stickers and clock to be disabled as if were in Hotel.
    /// </summary>
    public bool IsHideHUD
    {
        get => isHideHUD;
        set => isHideHUD = value;
    }

    // ------------------------------------------------------------------
    // Camera Properties
    public PixelPerfectCamera PixelPerfectCamera
    {
        get => GetComponent<PixelPerfectCamera>();
    }

    public float CameraSize
    {
        get => GetComponent<Camera>().orthographicSize;
    } 

    /// <summary>
    /// (DEV): Sets all levels to inactive from Dev'ing
    /// to avoid errors when a level is active on load where their GameObjects's
    /// OnEnable and Awake functions will be called before Singletons are set
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        Debug.Log("~~~~~~~~ TASKS RIGHT AFTER GAME SCENE LOAD ~~~~~~~~");
        LevelsInactivate();
    }

    public static void LevelsInactivate(bool isOnBeforeScene = true)
    {
        Debug.Log("~~~~~~~~ Disabling all level grids; ensure these are inactive in prod ~~~~~~~~");

        Script_LevelGrid[] lvls = (Script_LevelGrid[])GameObject.FindObjectsOfType(typeof(Script_LevelGrid));
        foreach (Script_LevelGrid lvl in lvls)
        {
            // Only show error messaging if we're inactivating before Scene setup. 
            if (lvl.gameObject.activeSelf && isOnBeforeScene)
            {
                string s = $"{lvl.name} is active. You need to set this inactive at game load for prod.";
                if (Const_Dev.IsDevMode)    Debug.Log(s);
                else                        Debug.LogWarning($"<color=red>{s}</color>");
            }
            
            lvl.gameObject.SetActive(false);
        }
    }
    
    void OnEnable()
    {
        Script_ClockEventsManager.OnTimesUp += TimesUpEffects;
        dieTimelineDirector.stopped += OnDiePlayableDone;
    }

    void OnDisable()
    {
        Script_ClockEventsManager.OnTimesUp -= TimesUpEffects;
        dieTimelineDirector.stopped -= OnDiePlayableDone;
    }

    void OnValidate()
    {
        thoughtSlots = thoughtSlotHolder.transform
            .GetChildren<Script_PlayerThoughtsInventoryButton>(true);
        thoughtSlots.SetExplicitListNav();
    }

    /// All managers are setup as singletons in their respective Awake()
    /// NOTE: no refs to Game or other Managers in Awake, only allowed in Start()
    void Awake()
    {
        DevCleanup();

        if (Game == null)
        {
            Game = this;
        }
        else if (Game != this)
        {
            Destroy(this.gameObject);
        }
        
        saveGameControl.Setup();

        Script_SystemSettings.DisableMouse();
        systemSettings.TargetFrameRate();
        systemSettings.SetScreenSettings();

        cinemachineBrain = GetComponent<CinemachineBrain>();

        Script_Utils.MakeFontsCrispy(fonts);
        
        ChangeStateToInitiateLevel();

        // Setup Singletons, Dicts, Managers and Canvases
        graphicsManager.Setup();
        
        scarletCipherManager.Setup();
        namesManager.Setup();
        exitsHandler.Setup(this);
        
        SFXManager.Setup();
        BGMManager.Setup();
        
        hitBoxDictionary.Setup();
        hintManager.Setup();
        VCamManager.Setup();
        fullArtManager.Setup();
        PRCSManager.Setup();
        artFrameManager.Setup();
        itemPickUpTheatricsManager.Setup();
        
        runsManager.Setup();
        weatherFXManager.Setup();
        eventCycleManager.Setup();
        mapNotificationsManager.Setup();
        dayNotificationManager.Setup();
        teletypeNotificationManager.Setup();
        lightFXManager.Setup();
        glitchFXManager.Setup();
        windManager.Setup();

        canvasesAudioSource.gameObject.SetActive(true);
        
        dialogueManager.Initialize();
        thoughtManager.HideThought();
        DDRManager.Setup();
        SetupMenu();

        transitionManager.Setup();
        cutSceneManager.Setup();
        canvasGroupsParent.Setup();
        UIAspectRatioEnforcerFrame.Setup();
        elevatorManager.Setup();
        stickerHolsterManager.Setup();
        activeStickerManager.Setup();
        mynesMirrorManager.Setup();
        HUDManager.Setup();
        saveManager.Setup();
        pianoManager.Setup();
        notesTallyTracker.Setup();

        settingsController.Setup();
    }

    // Load Save Data and Initiate level
    void Start()
    {
        Script_PlayerInputManager.Instance.Setup();
        LoadGame();
        
        OnLoadTasks();
        
        // player creation must happen before level creation as LB needs reference to player
        CreatePlayer();
        
        lanternFollower.Setup();

        InitiateLevel();

        exitsHandler.canvas.alpha = 1.0f;
        exitsHandler.StartFadeIn();

        timeManager.Setup();
        clockManager.Setup(); // needs to happen after level is set so we know if we're in lobby or not
    }

    private void DevCleanup()
    {
        GetComponent<CinemachineBrain>().enabled = true;
        ClearEntries(); // clear entry null objects left when exitting Play Mode
        totalPlayTime = 0;
    }

    private void LoadGame()
    {
        if (!Debug.isDebugBuild || Const_Dev.IsPersisting)
        {
            isLoadedGame = Script_SaveGameControl.control.Load();

            if (isLoadedGame)
                OnDidLoad();
        }
        
        // TBD TODO: REMOVE (only for dev)
        if (Const_Dev.IsDevSpawn && !isLoadedGame)
        {
            OnNewGameDev();

            Dev_GameHelper gameHelper = GetComponent<Dev_GameHelper>();
            level = gameHelper.level;
            Debug.Log("DEV/Setting level to "+ level);
            
            Tilemap tileMap = Levels.levelsData[level].tileMap;
            GameObject grid = Levels.levelsData[level].grid;
            gridOffset = grid.transform.position;
            Vector3 tileLocation = gameHelper.playerSpawn + gridOffset;
            
            // Vector3 tileLocation = tileMap.CellToWorld(gameHelper.playerSpawn);
            Debug.Log($"player dev spawn tileLocation: {tileLocation} on Tilemap: {tileMap}");

            SetPlayerState(new Model_PlayerState(
                    (int)tileLocation.x,
                    (int)tileLocation.y,
                    (int)tileLocation.z,
                    gameHelper.facingDirection
                )
            );
        }
        else if (!isLoadedGame)
        {
            OnNewGame();
        }
    }

    private void OnDidLoad() { }

    private void OnNewGame()
    {
        NewGame();
        NewGamePlayerSpawn();

        void NewGamePlayerSpawn()
        {
            var newGamePlayerSpawn = new Model_PlayerState(
                (int)playerSpawn.data.playerSpawn.x,
                (int)playerSpawn.data.playerSpawn.y,
                (int)playerSpawn.data.playerSpawn.z,
                playerSpawn.data.facingDirection
            );

            level = playerSpawn.data.level;
            SetPlayerState(newGamePlayerSpawn);
        }
    }

    private void OnNewGameDev()
    {
        NewGame();
    }

    private void NewGame()
    {
        scarletCipherManager.Initialize();
        runsManager.Initialize();
    }

    /// <summary>
    /// Tasks to run after initial load but before initialization of level
    /// </summary>
    private void OnLoadTasks()
    {
        if (level > tutorialEndLevel)
        {
            Debug.Log($"Enabling S-book because we loaded after level {tutorialEndLevel}");
            EnableSBook(true);
        }
        else
        {
            /// Always allow SBook for now (some people like to check out inventory at start of game)
            EnableSBook(true); // EnableSBook(false);
        }

        IdsRoomBehavior.InitializeBGMOnRun();
    }

    public void ChangeStateToInitiateLevel()
    {
        lastState = state;
        state = Const_States_Game.InitiateLevel;
    }

    public void ChangeStateCutScene()
    {
        Debug.Log($"Game.state changed to: {Const_States_Game.CutScene}");
        lastState = state;
        
        if (lastState == Const_States_Game.Inventory)
        {
            Script_MenuEventsManager.ExitSubmenu();
            Script_MenuEventsManager.ExitMenu();
        }

        state = Const_States_Game.CutScene;
    }

    public void ChangeStateCutSceneNPCMoving()
    {
        Debug.Log($"Game.state changed to: {Const_States_Game.CutSceneNPCMoving}; Game.lastState before this = {lastState}");
        lastState = state;
        state = Const_States_Game.CutSceneNPCMoving;
    }

    public void ChangeStateInteract()
    {
        Debug.Log($"Game.state changed to: {Const_States_Game.Interact}; Game.lastState before this = {lastState}");
        lastState = state;
        state = Const_States_Game.Interact;
    }

    public void NextFrameChangeStateInteract()
    {
        StartCoroutine(WaitToChangeState());
        
        IEnumerator WaitToChangeState()
        {
            yield return null;
            
            Debug.Log($"NEXT FRAME Game.state changed to: {Const_States_Game.Interact}; Game.lastState before this = {lastState}");
            
            lastState = state;
            state = Const_States_Game.Interact;
        }
    }

    public void ChangeStateToInventory()
    {
        Debug.Log($"Game.state changed to: {Const_States_Game.Inventory}; Game.lastState before this = {lastState}");
        lastState = state;
        state = Const_States_Game.Inventory;
    }

    public void ChangeStateDDR()
    {
        Debug.Log($"Game.state changed to: {Const_States_Game.DDR}; Game.lastState before this = {lastState}");
        lastState = state;
        state = Const_States_Game.DDR;
    }

    /// <summary>
    /// Pass in null to make new state and last state the same
    /// Otherwise, go to last state; define this state
    /// </summary>
    /// <param name="newLastState"></param>
    public void ChangeStateLastState(string newLastState)
    {
        string prevLastState = newLastState ?? lastState;
        state = lastState;
        lastState = prevLastState;
        Debug.Log($"ChangeStateLastState(): Game.state changed to: {state}; Game.lastState now = {lastState}");
    }

    public void InitiateLevel()
    {
        SetLevelBehavior();
        weatherFXManager.SnowDayEffect();
        
        // Note: StartBgMusic should be before Grid Activation and Level Setup, because BGM changes
        // on Awake/OnEnable/Start/Setup should be able to override this default BGM Player.
        StartBgMusic();
        
        SetTileMaps();
        
        // Unity startup lifeCycle events
        grid.SetActive(true);
        
        SetupPlayerOnLevel();

        SetupDialogueManagerOnLevel();
        SetupThoughtManager();
        InitializeHintManager();

        // Level Behavior Setups (must occur last for references to be set)
        InitLevelBehavior();
    }

    private void SetLevelBehavior()
    {
        LastLevelBehavior = levelBehavior;
        levelBehavior = Levels.levelsData[level].behavior;
    }

    void InitLevelBehavior()
    {
        persistentDropsContainer.ActivatePersistentDropsForLevel(level);
        
        if (levelBehavior == null)
            return;
        
        print($"level: {level}; levelBehavior: {levelBehavior}... LastLevelBehavior: {LastLevelBehavior}");
        levelBehavior.Setup();

        // Set BoundingVolume
        VCamManager.BoundingVolume = levelBehavior.BoundingVolume?.BoundingVolumeCollider;
        
        VCamManager.ConfineScreenEdges = VCamManager.BoundingVolume != null
            && levelBehavior.BoundingVolume.ConfineScreenEdges;

        VCamManager.VCamera.InvalidateConfinerCache();
    }

    /// <summary>
    /// Tearing down Level after a an Exit.
    /// </summary>
    public void DestroyLevel()
    {
        levelBehavior.Cleanup();
        GetPlayer().ClearLevelState();
        
        ClearNPCs();
        ClearInteractableObjects();
        ClearSavePoint();
        ClearDemons();
        DestroyTmpTargets();
        DestroyAudioOneShotSources();
        grid.SetActive(false);
        ClearDrops();
        
        StopMovingNPCThemes();
        BGMManager.SetDefault(Const_AudioMixerParams.ExposedBGVolume);
        BGMManager.SetDefault(Const_AudioMixerParams.ExposedSFXVolume);
    }

    public void OnSaveTasks()
    {
        UpdateTime();
    } 

    private float UpdateTime()
    {
        return timeManager.UpdateTotalPlayTime();
    }

    private void ClearDrops()
    {
        persistentDropsContainer.DeactivatePersistentDrops();
    }

    public void LoadRun(int runIdx, int cycleCount)
    {
        runsManager.Load(runIdx, cycleCount);
    }

    public bool IsRunDay(Script_Run.DayId dayId)
    {
        return runsManager.Run.dayId == dayId;
    }

    // ----------------------------------------------------------------------
    // Timeline Setup Signals
    // 
    // Especially useful when a Level is needing to activate another Level.
    public void TimelineSetupBallroom()
    {
        BallroomBehavior.TimelineSetup();
    }

    /* =======================================================================
        _ENDING SEQENCES_DEATH_TIMESUP_TRANSITIONS
    ======================================================================= */
    public void OnEndingSequenceHideAll()
    {
        LevelsInactivate(isOnBeforeScene: false);
        SetPlayerActive(false);
    }
    
    public void DieEffects(Script_GameOverController.DeathTypes deathType)
    {
        transitionManager.DieEffects(deathType);
    }
    
    private void OnDiePlayableDone(PlayableDirector aDirector)
    {
        transitionManager.OnDiePlayableDone(aDirector);
    }

    

    /// ------------------------------------------------------------------
    /// Times Up
    /// <summary>
    /// Called on TimesUp Event Firing
    /// </summary>
    public void TimesUpEffects()
    {
        transitionManager.TimesUpEffects();
    }

    // ------------------------------------
    // Times Up: Timeline Signals
    public void OnTimesUpPlayableDone()
    {
        transitionManager.OnTimesUpPlayableDone();
    }

    /// ------------------------------------------------------------------
    /// Ending
    
    public void EndingCutScene(Script_TransitionManager.Endings ending)
    {
        transitionManager.StartEndingSequence(ending);
    }
    
    /// ------------------------------------------------------------------
    /// To Title
    public void ToTitle()
    {
        StartCoroutine(WaitToTitleScreen());

        IEnumerator WaitToTitleScreen()
        {
            yield return new WaitForSeconds(transitionManager.ToTitleWaitTime);

            transitionManager.PlayToTitleTimeline();
        }
    }

    private void ToTitleScene()
    {
        Script_SceneManager.ToTitleScene();
    }

    // ------------------------------------
    // To Title: Timeline Signals
    public void ToTitleFromTimeline()
    {
        ToTitleScene();
    }

    
    /* =======================================================================
        _LEVEL BEHAVIOR_
    ======================================================================= */
    public bool ActivateTrigger(string Id)
    {
        return levelBehavior.ActivateTrigger(Id);
    }

    public bool IsInHotel()
    {
        if (HotelLevelBehaviors.Length == 0)
            Debug.LogError("You need to specify hotel behaviors; otherwise, the clock will always be running.");
        
        foreach (Script_LevelBehavior lb in HotelLevelBehaviors)
        {
            if (levelBehavior == lb)
                return true;
        }

        return false;
    }

    public bool IsScreenFXDisabled()
    {
        return IsInHotel() || IsInGrandMirrorRoom();
    }

    public bool IsInGrandMirrorRoom()
    {
        return levelBehavior == grandMirrorRoomBehavior;
    }

    public bool IsGrandMirrorSetup()
    {
        return EileensMindBehavior.isPuzzleComplete && !grandMirrorRoomBehavior.IsDone;
    }

    public bool IsLastElevatorSaveAndStartWeekendCycle()
    {
        return levelBehavior == grandMirrorRoomBehavior;
    }

    /* =======================================================================
        _LEVEL MANAGEMENT_
    ======================================================================= */
    // remove temporary targets (used for camera targeting)
    public void DestroyTmpTargets()
    {
        foreach(Transform t in tmpTargetsContainer)
        {
            Destroy(t.gameObject);
        }
    }

    /* =======================================================================
        _TILEMAPS_
    ======================================================================= */

    /// <summary>
    /// Setting grid to active kicks off the Unity lifecycle in the LevelBehavior
    /// </summary>
    void SetTileMaps()
    {
        grid                = Levels.levelsData[level].grid;
        gridOffset          = grid.transform.position;
        tileMap             = Levels.levelsData[level].tileMap;
        extraTileMaps       = Levels.levelsData[level].extraTileMaps;
        stairsTileMaps      = Levels.levelsData[level].stairsTileMaps;
        worldTiles          = Levels.levelsData[level].worldTiles;
        exitsTileMaps       = Levels.levelsData[level].exitsTileMaps;
        entrancesTileMap    = Levels.levelsData[level].entrancesTileMap;
        pushableTileMap     = Levels.levelsData[level].pushableTileMap;
    }

    public void SetNewTileMapGround(Tilemap _tileMap)
    {   
        tileMap = _tileMap;   
    }

    /* =======================================================================
        _PLAYER_
    ======================================================================= */

    private void CreatePlayer()
    {
        // TODO don't need this, put all player data into PlayerState 
        Model_Level levelData = Levels.levelsData[level];
        Model_Player playerData = levelData.playerData;

        Vector3 spawnLocation = new Vector3(
            playerState.spawnX,
            playerState.spawnY,
            playerState.spawnZ
        );
        player = Instantiate(PlayerPrefab, spawnLocation, Quaternion.identity);
        player.Setup(
            playerState.faceDirection,
            playerState
        );
        player.transform.SetParent(playerContainer, false);
        
        // CameraTargetFollower.MatchPlayer();
        // CameraTargetFollower.IsFollowing = true;
        
        VCam.SetFollow();
        VCamDramaticZoom.SetFollow();
        VCamDramaticZoom.gameObject.SetActive(true);
    }

    public void SetupPlayerOnLevel()
    {
        Model_Level levelData = Levels.levelsData[level];
        Model_Player playerData = levelData.playerData;   

        player.InitializeOnLevel(
            playerState,
            grid.transform
        );

        PlayerForceSortingLayer(
            playerData.isForceSortingLayer,
            playerData.isForceSortingLayerAxisZ
        );

        CameraTargetFollower.MatchPlayer();

        Debug.Log("---- ---- PLAYER SETUP ON LEVEL EVENT ---- ----");
        Script_GameEventsManager.PlayerSetupOnLevel();   
    }

    public void DestroyPlayer()
    {
        Destroy(player.gameObject);
    }

    public void SetPlayerState(Model_PlayerState state)
    {
        playerState.faceDirection = state.faceDirection;
        playerState.spawnX = state.spawnX;
        playerState.spawnY = state.spawnY;
        playerState.spawnZ = state.spawnZ;
    }

    public void UpdatePlayerStateToCurrent()
    {
        Model_PlayerState p = new Model_PlayerState(
            (int)Mathf.Round(player.transform.position.x),
            (int)Mathf.Round(player.transform.position.y),
            (int)Mathf.Round(player.transform.position.z),
            player.FacingDirection
        );
        SetPlayerState(p);
    }

    public Model_PlayerState GetPlayerState()
    {
        return playerState;
    }

    public Script_Player GetPlayer()
    {
        return player;
    }

    public int GetThoughtsCount()
    {
        return playerThoughtHandler.GetThoughtsCount(thoughts);
    }

    /// <summary>
    /// Handle dialogue during cut scenes; implement in level behavior
    /// </summary>
    /// <param name="action"></param>
    public void HandleContinuingDialogueActions(string action)
    {
        cutSceneActionHandler.HandleContinuingDialogueActions(action, player);
    }

    public void HandleItemReceive(Script_ItemObject itemObject)
    {
        cutSceneActionHandler.HandleItemReceive(itemObject, player);
    }

    public void CreatePlayerReflection(Vector3 axis)
    {
        player.CreatePlayerReflection(axis);
    }
    
    public void RemovePlayerReflection()
    {
        player.RemoveReflection();
    }

    public void PlayerForceSortingLayer(bool isForceSortingLayer, bool isAxisZ)
    {
        player.ForceSortingLayer(isForceSortingLayer, isAxisZ);
    }

    public Vector3 GetPlayerLocation()
    {
        return player.GetComponent<Transform>().position;
    }

    public bool GetPlayerIsTalking()
    {
        return player.State == Const_States_Player.Dialogue;
    }

    public void PlayerFaceDirection(Directions direction)
    {
        player.FaceDirection(direction);
    }

    public bool GetPlayerIsSpawned()
    {
        return player != null;
    }

    public void PlayerEffectQuestion(bool isShow)
    {
        player.QuestionMark(isShow);
    }

    public int PlayerFullHeal()
    {
        return GetPlayer().FullHeal();
    }

    public int PlayerHurt(int dmg, Script_HitBox hitBox)
    {
        return GetPlayer().Hurt(dmg, hitBox);
    }

    public void HidePlayer()
    {
        GetPlayer().SetInvisible(true, 0f);
    }

    public void UnhidePlayer()
    {
        GetPlayer().SetInvisible(false, 0f);
    }

    public void SetPlayerActive(bool isActive)
    {
        GetPlayer().gameObject.SetActive(isActive);
    }

    /* =======================================================================
        _REFLECTION_
    ======================================================================= */
    public void SetupPlayerReflection(Transform r)
    {
        reflectionCreator.SetupPlayerReflection(r);
    }


    /* =======================================================================
        _INVENTORY_
    ======================================================================= */
    public void SetupMenu()
    {
        playerThoughtsInventoryManager.Setup();
    }

    public void InitializeMenuState(EventSystem eventSystem)
    {
        menuController.InitializeState(eventSystem);
    }
    
    public void OpenInventory(bool noSFX = false)
    {
        lastState = state;
        ChangeStateToInventory();
        playerThoughtsInventoryManager.OpenInventory(noSFX);
    }

    public void CloseInventory(bool noSFX = false)
    {
        ChangeStateLastState(Const_States_Game.Inventory);
        player.SetLastState();
        playerThoughtsInventoryManager.CloseInventory(noSFX);
        levelBehavior.OnCloseInventory();
    }

    public void EnableSBook(bool isActive)
    {
        playerThoughtsInventoryManager.EnableSBook(isActive);
    }

    public void UpdateEntries(Script_Entry[] e)
    {
        entries = e;
    }

    public void ClearEntries()
    {
        foreach (Script_Entry e in entries)
        {
            if (!Application.isEditor) Destroy(e.gameObject);
        }
        entries = new Script_Entry[0];
    }

    public void HighlightItem(
        int id,
        bool isOn,
        bool showDesc,
        Script_InventoryManager.Types type
    )
    {
        menuController.HighlightItem(id, isOn, showDesc, type);
    }

    public bool AddItem(Script_Item item)
    {
        return menuController.AddItem(item);
    }

    public bool AddItemInSlotById(string itemId, int i)
    {
        return menuController.AddItemInSlotById(itemId, i);
    }

    public bool AddItemById(string itemId)
    {
        return menuController.AddItemById(itemId);
    }

    public bool AddEquippedItemInSlotById(string equipmentId, int i)
    {
        return menuController.AddEquippedItemInSlotById(equipmentId, i);
    }

    // Remove all prepped masks. Place them back in inventory.
    public bool UnequipAll(string[] excludes = null)
    {
        return menuController.UnequipAll(excludes);
    }

    public void EquipLastElevatorMaskBackground(bool isGive = false)
    {
        UnequipAll(new string[]{ Const_Items.LastElevatorId });

        if (isGive)
            AddItemById(Const_Items.LastElevatorId);

        UnequipAll();
        
        // Set as Prepped Mask in background.
        StickStickerBackground(Const_Items.LastElevatorId);

        // Set as active in background.
        GetPlayer().ForceStickerSwitchBackground(0);
    }

    public void AddMyMaskBackground()
    {
        AddItemById(Const_Items.MyMaskId);
    }

    public bool StickStickerBackground(string stickerId)
    {
        return menuController.StickStickerBackground(stickerId);
    }

    public bool RemoveItemFromInventory(Script_Item item)
    {
        return menuController.RemoveItem(item);
    }

    public Script_ItemObject InstantiateDropById(string itemId, Vector3 location, int LB)
    {
        return menuController.InstantiateDropById(itemId, location, LB);
    }

    public bool CheckStickerEquipped(Script_Sticker sticker)
    {
        return menuController.CheckStickerEquipped(sticker);
    }

    public bool CheckStickerEquippedById(string stickerId)
    {
        return menuController.CheckStickerEquippedById(stickerId);
    }

    public Script_Item[] GetStickers()
    {
        return menuController.GetStickers();
    }

    public Script_Item[] GetItems()
    {
        return menuController.GetItems();
    }

    public Script_Sticker[] GetEquipmentItems()
    {
        return menuController.GetEquipmentItems();
    }

    /// <summary>
    /// To be called from the key target
    /// </summary>
    public bool TryUseKey(Script_UsableKey key)
    {
        return menuController.TryUseKey(key);
    }

    public Script_Item GetItemsInventoryItem(string itemId, out int slot)
    {
        return menuController.GetItemsInventoryItem(itemId, out slot);
    }

    public Script_Item GetItemsStickerItem(string itemId, out int slot)
    {
        return menuController.GetItemsStickerItem(itemId, out slot);
    }

    public void OrganizeInventory()
    {
        menuController.Organize();
    }

    /* =======================================================================
        _SETTINGS_
    ======================================================================= */

    public void OpenSettings()
    {
        if (settingsController.IsThrottledInGame)
            return;
        
        settingsController.OpenOverview(0);
        Time.timeScale = 0f;
        IsSettingsOpen = true;
    }

    public void BackToGame()
    {
        Time.timeScale = 1f;
        IsSettingsOpen = false;
    }
    
    /* =======================================================================
        _CANVASES_CUTSCENES_THEATRICS
    ======================================================================= */
    public void CanvasesInitialState()
    {
        transitionManager.InitialState();
        PRCSManager.Initialize();
    }
    
    public void ShowHint(string s)
    {
        hintManager.ShowTextHint(s);
    }

    public void HideHint()
    {
        hintManager.HideTextHint();
    }

    public void InitializeHintManager()
    {
        hintManager.Initialize();
    }

    // separate vs. default level fader
    public IEnumerator TransitionFadeIn(float t, Action action)
    {
        return transitionManager.FadeIn(t, action);
    }
    
    public IEnumerator TransitionFadeOut(float t, Action action)
    {
        return transitionManager.FadeOut(t, action);
    }

    public void ManagePlayerViews(string type)
    {
        if (type == Const_States_PlayerViews.DDR)
        {
            /// TODO: Handle Runs Canvas on DDR   
        }

        if (type == Const_States_PlayerViews.Health)
        {
            /// TODO: Handle Runs Canvas on DDR   
        }
    }

    /* =======================================================================
        _NPCs_
    ======================================================================= */

    public void SetupCutSceneNPC(Script_CutSceneNPC cutSceneNPC)
    {
        cutSceneNPCCreator.SetupCutSceneNPC(cutSceneNPC, NPCs, cutSceneNPCs);
    }

    public void SetupMovingNPC(Script_MovingNPC movingNPC, bool isInitialize)
    {
        movingNPCCreator.SetupMovingNPC(movingNPC, NPCs, movingNPCs, isInitialize);
    }

    public void AutoSetupMovingNPC(Script_MovingNPC npc)
    {
        movingNPCCreator.AutoSetup(npc, NPCs, movingNPCs);
    }

    public void DestroyNPCs()
    {
        foreach(Script_StaticNPC NPC in NPCs)
        {
            if (NPC != null)    Destroy(NPC.gameObject);
        }

        NPCs.Clear();
        movingNPCs.Clear();
        cutSceneNPCs.Clear();
    }

    public void ClearNPCs()
    {
        NPCs.Clear();
        movingNPCs.Clear();
        cutSceneNPCs.Clear();
    }

    public void DestroyMovingNPC(int Id)
    {
        for (int i = 0; i < movingNPCs.Count; i++)
        {
            if (movingNPCs[i].StaticNPCId == Id)
            {
                Destroy(movingNPCs[i].gameObject);
                movingNPCs.RemoveAt(i);
            }
        }
    }

    public void DestroyCutSceneNPC(int Id)
    {
        for (int i = 0; i < cutSceneNPCs.Count; i++)
        {
            if (cutSceneNPCs[i].StaticNPCId == Id)
            {
                Destroy(cutSceneNPCs[i].gameObject);
                cutSceneNPCs.RemoveAt(i);
            }
        }
    }

    public Vector3[] GetNPCLocations()
    {
        Vector3[] NPCLocations = new Vector3[NPCs.Count];
        bool isAllDestroyed = true;
        
        if (NPCLocations.Length == 0)    return new Vector3[0];

        for (int i = 0; i < NPCs.Count; i++)
        {
            if (NPCs[i] != null)
            {
                NPCLocations[i] = NPCs[i].transform.position;
                isAllDestroyed = false;
            }
        }

        if (isAllDestroyed)     return new Vector3[0];
        else return NPCLocations;
    }

    public Script_StaticNPC GetNPC(int i)
    {
        return NPCs[i];
    }
    
    public Vector3[] GetMovingNPCLocations()
    {
        Vector3[] MovingNPCLocations = new Vector3[movingNPCs.Count];
        
        if (MovingNPCLocations.Length == 0)    return new Vector3[0];

        for (int i = 0; i < movingNPCs.Count; i++)
        {
            MovingNPCLocations[i] = movingNPCs[i].transform.position;
        }

        return MovingNPCLocations;
    }

    public void TriggerMovingNPCMove(int i)
    {
        movingNPCs[i].Move();
    }

    public void MovingNPCActuallyMove(int i)
    {
        movingNPCs[i].ActuallyMove();
    }

    public void SetMovingNPCExit(int i, bool shouldExit)
    {
        movingNPCs[i].shouldExit = shouldExit;
    }

    public Script_MovingNPC GetMovingNPC(int i)
    {
        return movingNPCs[i];
    }

    public void ChangeMovingNPCSpeed(int i, float speed)
    {
        movingNPCs[i].ChangeSpeed(speed);
    }

    public void CurrentMovesDoneAction()
    {
        Levels.levelsData[level].behavior.HandleMovingNPCCurrentMovesDone();
        // if (!BGMManager.GetIsPlaying())    UnPauseBgMusic();
        
        // if (eroBgThemePlayer != null)
        // {
        //     PauseEroTheme();
        // }
    }

    public void AllMovesDoneAction(int i)
    {
        Levels.levelsData[level].behavior.HandleMovingNPCAllMovesDone();
    }

    public void OnApproachedTarget(int i)
    {
        Levels.levelsData[level].behavior.HandleMovingNPCOnApproachedTarget(i);
    }

    public void NPCFaceDirection(int Id, Directions direction)
    {
        foreach(Script_MovingNPC NPC in movingNPCs)
        {
            if (NPC.MovingNPCId == Id)   NPC.FaceDirection(direction);
        }
    }

    /* =======================================================================
        _SAVEPOINTS_
    ======================================================================= */

    // only max of 1 in a Level
    public void SetupSavePoint(Script_SavePoint sp, bool isInitialize)
    {
        savePoint = sp;
        print($"savePoint: {sp}");
        savePointCreator.SetupSavePoint(sp, isInitialize);
    }

    void ClearSavePoint()
    {
        savePoint = null;
    }
    
    public Model_SavePointData GetSavePointData()
    {
        return savePoint.GetData();
    }

    /* =======================================================================
        _INTERACTABLE OBJECTS_
    ======================================================================= */
    
    /// Better way to hook up interactables to Game
    public void AddInteractableObject(Script_InteractableObject interactableObject)
    {
        interactableObjects.Add(interactableObject);
    }
    
    // must give world offset bc IOs in Scene are parented by world
    public void SetupInteractableObjectsText(
        Transform textObjectParent,
        bool isInitialize
    )
    {
        interactableObjectCreator.SetupInteractableObjectsText(
            textObjectParent,
            interactableObjects,
            dialogueManager,
            player,
            isInitialize
        );
    }

    /// <summary>
    /// Setup text elements when we need to specify particular objects (no common parent)
    /// </summary>
    /// <param name="fullArtParent"></param>
    /// <param name="isInitialize"></param>
    public void SetupInteractableObjectsTextManually(
        Script_InteractableObjectText[] textObjs,
        bool isInitialize
    )
    {
        interactableObjectCreator.SetupInteractableObjectsTextManually(
            textObjs,
            interactableObjects,
            dialogueManager,
            player,
            isInitialize
        );
    }

    public void SetupInteractableFullArt(
        Transform fullArtParent,
        bool isInitialize
    )
    {
        interactableObjectCreator.SetupInteractableFullArt(
            fullArtParent,
            interactableObjects,
            dialogueManager,
            player,
            isInitialize
        );
    }
    
    public bool[] SetupSwitches(
        Transform switchesParent,
        bool[] switchesState,
        bool isInitialize
    )
    {
        return interactableObjectCreator.SetupSwitches(
            switchesParent,
            interactableObjects,
            switches,
            switchesState,
            isInitialize
        );
    }

    public void SetupPushables(
        Transform parent,
        bool isInitialize
    )
    {
        interactableObjectCreator.SetupPushables(
            parent,
            interactableObjects,
            pushables,
            isInitialize
        );
    }

    public void SetupInteractableObjectsExit(
        Transform parent,
        bool isInit
    )
    {
        interactableObjectCreator.SetupInteractableObjectsExit(
            parent,
            interactableObjects,
            isInit
        );
    }

    void DestroyInteractableObjects()
    {
        interactableObjectCreator.DestroyInteractableObjects(
            interactableObjects, 
            switches,
            pushables
        );    
    }

    void ClearInteractableObjects()
    {
        interactableObjectCreator.ClearInteractableObjects(
            interactableObjects, 
            switches,
            pushables
        );
    }

    public Vector3[] GetInteractableObjectLocations()
    {
        return interactableObjectHandler.GetLocations(interactableObjects);
    }

    public List<Script_InteractableObject> GetInteractableObjects()
    {
        return interactableObjects;
    }

    public int GetSwitchesCount()
    {
        return switches.Count;
    }

    public void SetSwitchesState(bool[] switchesState)
    {
        for (int i = 0; i < switches.Count; i++)
        {
            switches[i].isOn = switchesState[i];
        }
    }

    public void SetSwitchState(int Id, bool isOn)
    {
        print($"setting switches state Id: {Id}, isOn: {isOn}");
        levelBehavior.SetSwitchState(Id, isOn);
    }

    public Script_Switch GetSwitch(int Id)
    {
        return switches[Id];
    }

    /* =======================================================================
        _DEMONS_
    ======================================================================= */

    public void CreateDemons(bool[] spawnState)
    {
        demonCreator.CreateDemons(
            spawnState,
            Levels.levelsData[level].DemonsData,
            demons
        );
    }

    public void SetupDemons(Transform demonsParent, bool[] demonsState)
    {
        demonCreator.SetupDemons(demonsParent, demonsState, demons);
    }

    void DestroyDemons()
    {
        foreach(Script_Demon d in demons)
        {
            if (d)    Destroy(d.gameObject);
        }

        demons.Clear();
    }

    void ClearDemons()
    {
        demons.Clear();
    }

    public List<Script_Demon> GetDemons()
    {
        return demons;
    }

    public void EatDemon(int Id)
    {
        demonHandler.EatDemon(Id, demons);
        levelBehavior.EatDemon(Id);
    }

    public Vector3[] GetDemonLocations()
    {
        Vector3[] DemonLocations = new Vector3[demons.Count];
        
        if (DemonLocations.Length == 0)    return new Vector3[0];

        for (int i = 0; i < demons.Count; i++)
        {
            DemonLocations[i] = demons[i].transform.position;
        }

        return DemonLocations;
    }

    public int GetDemonsCount()
    {
        return demons.Count;
    }

    /* =========================================================================
        _TIMELINE_
    ========================================================================= */
    public void HandlePlayableDirectorStopped(PlayableDirector aDirector)
    {
        levelBehavior.HandlePlayableDirectorStopped(aDirector);
    }
    
    /* =========================================================================
        _DIALOGUE & THOUGHTS_
    ========================================================================= */    

    void SetupDialogueManagerOnLevel()
    {
        dialogueManager.Setup();
    }

    void SetupThoughtManager()
    {
        thoughtManager.Setup();
    }

    public void ShowAndCloseThought(Model_Thought thought)
    {
        thoughtManager.ShowThought(thought);
        thoughtManager.CloseThought(thought);
    }

    public void HandleDialogueNodeAction(string action)
    {
        levelBehavior.HandleDialogueNodeAction(action);
    }

    public void HandleDialogueNodeUpdateAction(string action)
    {
        levelBehavior.HandleDialogueNodeUpdateAction(action);
    }

    /* =========================================================================
        _INPUT_
    ========================================================================= */
    public int HandleSubmit(string text)
    {
        return levelBehavior.OnSubmit(text);
    }

    /* =========================================================================
        _DDR_
    ========================================================================= */

    public void HandleDDRArrowClick(int tier)
    {
        DDRHandler.HandleArrowClick(tier, levelBehavior);
    }
    
    /* =========================================================================
        _MUSIC_
    ========================================================================= */

    public void StartBgMusic()
    {
        // TODO: make this a general theme player, not just for Ero
        if (npcBgThemePlayer != null && GetNPCThemeMusicIsPlaying())   return;
        
        int i = Levels.levelsData[level].bgMusicAudioClipIndex;

        if (Levels.levelsData[level].isBgmPaused)
        {
            Debug.Log($"Level {Levels.levelsData[level]} starting with PAUSED Bgm");
            i = -1;
        }
        
        BGMManager.Play(i);
    }

    public void SwitchBgMusic(int i)
    {
        BGMManager.Play(i, forcePlay: true);
    }

    public void StopBgMusic()
    {
        BGMManager.Stop();
    }

    public void PauseBgMusic()
    {
        BGMManager.Pause();
    }

    public void UnPauseBgMusic()
    {
        if (BGMManager != null)
            BGMManager.UnPause();
    }

    public void PauseBgThemeSpeakers()
    {
        BGMManager.PauseBgThemeSpeakers();
    }

    public void UnPauseBgThemeSpeakers()
    {
        BGMManager.UnPauseBgThemeSpeakers();
    }

    public Script_BgThemePlayer PlayNPCBgTheme(Script_BgThemePlayer bgThemePlayerPrefab)
    {
        npcBgThemePlayer = Instantiate(
            bgThemePlayerPrefab,
            player.transform.position,
            Quaternion.identity
        );
        npcBgThemePlayer.transform.SetParent(bgThemeSpeakersContainer, false);
        return npcBgThemePlayer;
    }

    public void PauseNPCBgTheme()
    {
        if (npcBgThemePlayer == null)   return;
        npcBgThemePlayer.GetComponent<AudioSource>().Pause();
    }

    public Script_BgThemePlayer UnPauseNPCBgTheme()
    {
        if (npcBgThemePlayer == null)
        {
            Debug.Log("No npcBgThemePlayer object exists to UnPause.");
            return null;
        }
        npcBgThemePlayer.GetComponent<AudioSource>().UnPause();
        return npcBgThemePlayer;
    }

    public void StopBgTheme()
    {
        npcBgThemePlayer.SoftStop();
        npcBgThemePlayer.gameObject.SetActive(false);

        npcBgThemePlayer = null;
    }

    public bool GetNPCBgThemeActive()
    {
        return  npcBgThemePlayer != null;
    }

    public bool GetNPCThemeMusicIsPlaying()
    {
        return npcBgThemePlayer.GetComponent<AudioSource>().isPlaying;
    }

    public void StopMovingNPCThemes()
    {
        if (npcBgThemePlayer == null)   return;

        if (Levels.levelsData[level].shouldPersistBgThemes)    PauseNPCBgTheme();
        
        StopBgTheme();
    }

        public Script_AudioOneShotSource CreateAudioOneShotSource(Vector3 position)
    {
        Script_AudioOneShotSource a = Instantiate(
            AudioOneShotSourcePrefab,
            position,
            Quaternion.identity
        );

        audioOneShotSources.Add(a);

        return a;
    }

    void DestroyAudioOneShotSources()
    {
        foreach(Script_AudioOneShotSource a in audioOneShotSources)
        {
            if (a)    Destroy(a.gameObject);
        }

        audioOneShotSources.Clear();
    }

    /* =========================================================================
        _VCAMERA_
    ========================================================================= */
    
    public CinemachineVirtualCamera SnapActiveCam(Vector3 prevPos, Transform t = null)
    {
        CinemachineVirtualCamera activeVCam = cinemachineBrain
            .ActiveVirtualCamera.VirtualCameraGameObject
            .GetComponent<CinemachineVirtualCamera>();
        
        var target = t ?? GetPlayer().transform;
        
        Debug.Log($"Snapping activeVCam <{activeVCam}> to <{target}>");
        
        // https://forum.unity.com/threads/cameras-no-longer-snapping-after-being-disabled-enabled.729242/#post-6276506
        activeVCam.OnTargetObjectWarped(target.transform, target.transform.position - prevPos);
        
        // https://forum.unity.com/threads/proper-way-to-reset-follow-camera.747101/
        activeVCam.PreviousStateIsValid = false;

        // Match the Camera Follower
        cameraTargetFollower.MatchPlayer();
        
        return activeVCam;
    }

    /// <summary>
    /// Will snap the camera instead of blending. Useful if want to cut the Blend on the fly
    /// and not have to define in Brain.
    /// </summary>
    /// <param name="prevPos">Previous VCam position</param>
    /// <param name="t">New VCam Transform</param>
    /// <param name="vCam"New VCam></param>
    public CinemachineVirtualCamera SnapCam(Vector3 prevPos, Transform t, CinemachineVirtualCamera vCam)
    {
        vCam.OnTargetObjectWarped(t.transform, t.transform.position - prevPos);
        vCam.PreviousStateIsValid = false;

        return vCam;
    }

    // Forces a cut on the current Active Cam.
    public void ForceCutBlend()
    {
        CinemachineVirtualCamera activeVCam = cinemachineBrain
            .ActiveVirtualCamera?.VirtualCameraGameObject
            .GetComponent<CinemachineVirtualCamera>();

        if (activeVCam == null || cinemachineBrain == null)     return; // to prevent erroring on quitting

        activeVCam?.gameObject.SetActive(false);
        cinemachineBrain.enabled = false;

        Vector3 activeCamPosition = activeVCam?.transform.position ?? Vector3.zero;
        cinemachineBrain.transform.position = activeCamPosition;

        activeVCam?.gameObject.SetActive(true);
        cinemachineBrain.enabled = true;
    }

    public void PixelPerfectEnable(bool isEnable)
    {
        GetComponent<PixelPerfectCamera>().enabled = isEnable;
    }

    /* =========================================================================
        _EXITS_
    ========================================================================= */
    public void DisableAllExitsEntrances(bool isDisabled)
    {
        exitsHandler.DisableAllExitsEntrances(isDisabled);
    }
    
    public void DisableExits(bool isDisabled, int i)
    {
        print("game.DisableExits()=================");
        exitsHandler.DisableExits(isDisabled, i);
    }

    public bool GetIsExitsDisabled()
    {
        return exitsHandler.GetIsExitsDisabled();
    }

    public void Exit(
        int level,
        Vector3 playerNextSpawnPosition,
        Directions playerFacingDirection,
        bool isExit,
        bool isSilent = false,
        Script_Exits.ExitType exitType = Script_Exits.ExitType.Default
    )
    {
        Script_Exits.FollowUp followUp; 
        
        switch(exitType)
        {
            case (Script_Exits.ExitType.SaveAndRestart):
                followUp = Script_Exits.FollowUp.SaveAndRestart;
                break;
            case (Script_Exits.ExitType.Elevator):
                followUp = Script_Exits.FollowUp.CutSceneNoFade;
                break;
            case (Script_Exits.ExitType.StairsUp):
                followUp = Script_Exits.FollowUp.Default;
                break;
            case (Script_Exits.ExitType.Piano):
                followUp = Script_Exits.FollowUp.Piano;
                break;
            case (Script_Exits.ExitType.SaveAndStartWeekendCycle):
                followUp = Script_Exits.FollowUp.SaveAndStartWeekendCycle;
                break;
            case (Script_Exits.ExitType.SaveAndRestartOnLevel):
                followUp = Script_Exits.FollowUp.SaveAndRestartOnLevel;
                break;
            default:
                followUp = Script_Exits.FollowUp.Default;
                break;
        }
        
        Debug.Log($"Follow up passed to ExitsHandler is {followUp}");
        
        exitsHandler.Exit(
            level,
            playerNextSpawnPosition,
            playerFacingDirection,
            isExit,
            isSilent,
            followUp
        );
    }

    private void Teleport(Script_ExitMetadata exit)
    {
        Exit(
            exit.data.level,
            exit.data.playerSpawn,
            exit.data.facingDirection,
            isExit: true,
            isSilent: false,
            exitType: Script_Exits.ExitType.Default
        );
    }

    private void TeleportBackground(Script_ExitMetadata exit)
    {
        exitsHandler.Exit(
            exit.data.level,
            exit.data.playerSpawn,
            exit.data.facingDirection,
            isExit: true,
            isSilent: true,
            followUp: Script_Exits.FollowUp.CutSceneNoFade
        );
    }

    public void TeleportToXXXWorldSaloonExit()
    {
        Teleport(XXXWorldSaloonExit);
    }

    public void HandleExitCutSceneLevelBehavior()
    {
        levelBehavior.HandleExitCutScene();
    }

    public void TeleportToGrandMirrorBackgroundR2()
    {
        grandMirrorRoomBehavior.IsFinalRound = true;
        TeleportBackground(grandMirrorRoomEntrance);
    }

    public void SetBayV1ToSaveState(Script_LevelBehavior_33.State saveState)
    {
        bayV1Behavior.Behavior = saveState;
    }

    /* =========================================================================
        _CUTSCENES_
    ========================================================================= */    
    public void MelanholicTitleCutScene()
    {
        cutSceneManager.MelanholicTitleCutScene();
    }

    public void ElevatorCloseDoorsCutScene(
        Script_ExitMetadataObject exit,
        Script_ElevatorBehavior exitBehavior,
        Script_Elevator.Types type,
        Model_Exit exitOverrideData = null,
        Script_Exits.ExitType? exitTypeOverride = null
    )
    {
        elevatorManager.CloseDoorsCutScene(exit, exitBehavior, type, exitOverrideData, exitTypeOverride);
    }

    /* =========================================================================
        _SAVELOAD_
    ========================================================================= */
    public Model_PersistentDrop[] GetPersistentDrops()
    {
        return persistentDropsContainer.GetPersistentDropModels();
    }

    /* =========================================================================
        _RESTARTING_
    ========================================================================= */
    
    /// <summary>
    /// -------- Saving & Restarting --------
    /// 1. Moving to the next run
    /// 2. Restarting the day
    /// </summary>
    
    /// ------------------------------------------------------------------
    /// 1. Move to Next Run
    /// <summary>
    /// Exiting the Elevator Bay after exiting via a Last Elevator
    /// (TBD: Upgraded Elevator Sticker)
    /// </summary>
    /// <param name="playerStateOverride"></param>
    public void NextRunSaveInitialize(bool isLobbySpawn = true, Script_Run.DayId dayId = Script_Run.DayId.none)
    {
        SetActiveEnding();
        
        if (dayId == Script_Run.DayId.none)
            runsManager.IncrementRun();
        else
            runsManager.SetRun(dayId);
        
        CleanRun();
        
        SaveWaitRestart(isLobbySpawn);
    }

    public void StartWeekendCycleSaveInitialize(bool isLobbySpawn = true)
    {
        SetActiveEnding();
        
        runsManager.StartWeekendCycle();
        CleanRun();

        // Set LB48 isDone to update that we've done the Myne Grand Mirror Cut Scene
        grandMirrorRoomBehavior.IsDone = true;

        SaveWaitRestart(isLobbySpawn);
    }

    public void StartSundayCycleSaveInitialize(bool isLobbySpawn = true)
    {
        SetActiveEnding();
        
        runsManager.StartSundayCycle();
        CleanRun();

        SaveWaitRestart(isLobbySpawn);
    }

    /// <summary>
    /// Setting the Active Ending will trigger the final cut scene.
    /// </summary>
    private void SetActiveEnding()
    {
        // Set ActiveEnding in state so on Load we can play an ending.
        // More important/harder endings override lesser ones.
        if (IsAllQuestsDoneToday())
        {
            activeEnding = Script_TransitionManager.Endings.True;
        }
        
        Debug.Log($"@@@@@@@@@@@@@@@ ACTIVE ENDING CHANGED TO {activeEnding} @@@@@@@@@@@@@@@");
    }

    private void SaveWaitRestart(bool isLobbySpawn = true)
    {
        Model_PlayerState lobbySpawnPlayer = new Model_PlayerState(
            (int)playerSpawn.data.playerSpawn.x,
            (int)playerSpawn.data.playerSpawn.y,
            (int)playerSpawn.data.playerSpawn.z,
            playerSpawn.data.facingDirection
        );

        Model_GameData gameData = new Model_GameData(
            RunIdx,
            SpawnLevelNo,
            CycleCount,
            totalPlayTime,
            activeEnding,
            faceOffCounter
        );
        
        saveGameControl.Save(
            isLobbySpawn ? lobbySpawnPlayer : null,
            isLobbySpawn ? gameData : null 
        );
        
        StartCoroutine(WaitToRestartGame());

        IEnumerator WaitToRestartGame()
        {
            yield return new WaitForSeconds(transitionManager.RestartGameTimeOnSave);
            RestartGame();
        }
    }

    public bool IsAllQuestsDoneToday()
    {
        Debug.Log($"Ids Done: {IdsRoomBehavior.isCurrentPuzzleComplete}");
        Debug.Log($"Ursie Done: {KTVRoom2Behavior.IsCurrentPuzzleComplete}");
        Debug.Log($"Ellenia Done: {ElleniasRoomBehavior.isCurrentPuzzleComplete}");
        Debug.Log($"Eileen Done: {EileensMindBehavior.isCurrentPuzzleComplete}");
        Debug.Log($"Moose Done: {WellsWorldBehavior.isCurrentMooseQuestComplete}");
        Debug.Log($"Kaffe Latte Done: {GardenLabyrinthBehavior.isCurrentPuzzleComplete}");
        
        return IdsRoomBehavior.isCurrentPuzzleComplete
            && KTVRoom2Behavior.IsCurrentPuzzleComplete
            && ElleniasRoomBehavior.isCurrentPuzzleComplete
            && EileensMindBehavior.isCurrentPuzzleComplete
            && WellsWorldBehavior.isCurrentMooseQuestComplete
            && GardenLabyrinthBehavior.isCurrentPuzzleComplete;
    }

    public void ShowSaveAndRestartMessageDefault()
    {
        saveManager.ShowSaveAndRestarMessage();
    }

    public void ShowSaveAndStartWeekendMessage()
    {
        saveManager.ShowSaveAndStartWeekendMessage();
    }

    /// <summary>
    /// ------------------------------------------------------------------
    /// 2. Restarting
    ///
    /// Restarts from the lobby of the current day (last save).
    /// </summary>
    
    public void Restart()
    {
        StartCoroutine(WaitToRestartGame());

        IEnumerator WaitToRestartGame()
        {
            yield return new WaitForSeconds(transitionManager.RestartGameTimeOnBadEnding);
            
            // Timeline will call RestartGameFromTimeline.
            transitionManager.PlayRestartGameTimeline();
        }
    }

    // ------------------------------------
    // Restarting: Timeline Signal
    // Called from Transition Manager's RestartTimeline 
    public void RestartGameFromTimeline()
    {
        RestartGame();   
    }

    // ------------------------------------------------------------------

    private void CleanRun()
    {
        clockManager.InitialState();
        scarletCipherManager.InitialState();
    }
    
    private void RestartGame()
    {
        Script_SceneManager.RestartGame();
    }
}
