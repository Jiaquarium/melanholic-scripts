using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.Playables;
using Cinemachine;
using UnityEngine.EventSystems;

/// <summary>
/// Entry point for Game scene
/// </summary>
public class Script_Game : MonoBehaviour
{
    
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public int level;
    public float totalPlayTime;
    
    /* -----------------------------------------------------------------------
        RUNTIME SYSTEM OPTIONS
    ----------------------------------------------------------------------- */
    public int targetFrameRate;
    /* -------------------------------------------------------------------- */

    public Model_Levels Levels;
    public string state;
    public Model_PlayerState playerState;
    public Model_PlayerThoughts thoughts;
    public Script_PlayerThoughtsInventoryButton[] thoughtSlots;
    [SerializeField] private Script_ThoughtSlotHolder thoughtSlotHolder;
    public Vector3 levelZeroCameraPosition;
    
    /* ======================================================================= */

    public static Script_Game Game;
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
    public Script_HealthManager healthManager;
    public Script_TransitionManager transitionManager;
    public PlayableDirector dieTimelineDirector;
    public Script_CutSceneManager cutSceneManager;
    public Script_Exits exitsHandler;
    public Script_DialogueManager dialogueManager;
    public Script_EntryManager entryManager;
    public Script_ThoughtManager thoughtManager;
    public Script_HintManager hintManager;
    public Script_BackgroundMusicManager bgMusicManager;
    public Script_FullArtManager fullArtManager;
    [SerializeField] private Script_InventoryAudioSettings canvasesAudioSource;
    [SerializeField] private Script_MenuController menuController;
    [SerializeField] private Script_CutSceneActionHandler cutSceneActionHandler;
    [SerializeField] private Script_TimeManager timeManager;
    [SerializeField] private Script_AllCanvasGroupsParent canvasGroupsParent;
    [SerializeField] private Script_PersistentDropsContainer persistentDropsContainer;
    [SerializeField] private Transform actsContainer;
    [SerializeField] private Script_LevelContainer[] levelsContainers;

    public Script_Player PlayerPrefab;
    public Script_AudioOneShotSource AudioOneShotSourcePrefab;

    public Font[] fonts;
    public Script_Camera camera;
    public Script_VCamera VCam;
    public Script_VCamera VCamDramaticZoom;
    public Transform world;
    public Transform playerContainer;
    public Transform bgThemeSpeakersContainer;
    public Transform tmpTargetsContainer;


    public Script_Entry[] entries = new Script_Entry[0];
    public GameObject grid;

    private Tilemap tileMap;
    private Tilemap[] exitsTileMaps;
    private Tilemap entrancesTileMap;
    [SerializeField] private Tilemap pushableTileMap;


    [SerializeField] private Script_Player player;
    [SerializeField] private Script_SavePoint savePoint; // max 1 per Level
    public List<Script_StaticNPC> NPCs = new List<Script_StaticNPC>();
    public List<Script_MovingNPC> movingNPCs = new List<Script_MovingNPC>();
    public List<Script_CutSceneNPC> cutSceneNPCs = new List<Script_CutSceneNPC>();
    public List<Script_InteractableObject> interactableObjects = new List<Script_InteractableObject>();
    public List<Script_Switch> switches = new List<Script_Switch>();
    public List<Script_Pushable> pushables = new List<Script_Pushable>();
    public List<Script_Demon> demons = new List<Script_Demon>();
    public List<Script_AudioOneShotSource> audioOneShotSources = new List<Script_AudioOneShotSource>();
    private Script_Demon DemonPrefab;
    private AudioSource backgroundMusicAudioSource;
    public Script_BgThemePlayer npcBgThemePlayer;
    private Script_LevelBehavior levelBehavior;
    public Script_LevelBehavior lastLevelBehavior { get; private set; }
    private Vector3 worldOffset;
    public string lastState;
    private bool isLoadedGame = false;
    private Script_GameOverController.DeathTypes deathType;
    public CinemachineBrain cinemachineBrain;

    [SerializeField] private int tutorialEndLevel;
    [SerializeField] private Transform newGameSpawnDestination;

    void OnEnable()
    {
        dieTimelineDirector.stopped += OnDiePlayableDone;
    }

    void OnDisable()
    {
        dieTimelineDirector.stopped -= OnDiePlayableDone;
    }

    void OnValidate()
    {
        thoughtSlots = thoughtSlotHolder.transform
            .GetChildren<Script_PlayerThoughtsInventoryButton>(true);
        thoughtSlots.SetExplicitListNav();

        levelsContainers = actsContainer.GetComponentsInChildren<Script_LevelContainer>();
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
        
        Script_SystemSettings.TargetFrameRate();
        Script_SystemSettings.FullScreen();
        PlayerPrefs.DeleteAll();
        Script_SystemSettings.DisableMouse();

        cinemachineBrain = GetComponent<CinemachineBrain>();

        Script_Utils.MakeFontsCrispy(fonts);
        
        /*
            set up handlers that affect state
        */
        ChangeStateToInitiateLevel();
        exitsHandler.Setup(this);

        // setup canvases and Managers
        canvasesAudioSource.gameObject.SetActive(true);
        dialogueManager.HideDialogue();
        thoughtManager.HideThought();
        DDRManager.Deactivate();
        SetupMenu();

        worldOffset = world.position;
        camera = Camera.main.GetComponent<Script_Camera>();
        camera.Setup(levelZeroCameraPosition);
        backgroundMusicAudioSource = bgMusicManager.GetComponent<AudioSource>();
        transitionManager.Setup();
        cutSceneManager.Setup();
        canvasGroupsParent.Setup();
    }

    // Load Save Data and Initiate level
    void Start()
    {
        Load();
        OnLoadTasks();
        // player creation must happen before level creation as LB needs reference to player
        CreatePlayer();
        healthManager.Setup(player);
        InitiateLevel();

        exitsHandler.canvas.alpha = 1.0f;
        exitsHandler.StartFadeIn();

        timeManager.Setup();
    }

    private void DevCleanup()
    {
        ClearEntries(); // clear entry null objects left when exitting Play Mode
        totalPlayTime = 0;
        LevelsInactivate();

        void LevelsInactivate()
        {
            Debug.Log("Disabling all level grids");

            foreach (Script_LevelContainer levelsContainer in levelsContainers)
            {
                Script_LevelGrid[] children = levelsContainer.transform.GetComponentsInChildren<Script_LevelGrid>();
                foreach (Script_LevelGrid c in children)   c.gameObject.SetActive(false);
            }
        }
    }

    private void Load()
    {
        if (!Debug.isDebugBuild || Const_Dev.IsPersisting)
        {
            isLoadedGame = Script_SaveGameControl.control.Load();

            if (isLoadedGame)   OnDidLoad();
        }
        
        // TODO: REMOVE (only for dev)
        if (!isLoadedGame && (Debug.isDebugBuild || Const_Dev.IsDevMode))
        {
            Dev_GameHelper gameHelper = GetComponent<Dev_GameHelper>();
            level = gameHelper.level;
            Debug.Log("DEV/Setting level to "+ level);
            
            Tilemap tileMap = Levels.levelsData[level].tileMap;
            Vector3 tileLocation = tileMap.CellToWorld(gameHelper.playerSpawn);
            Debug.Log($"player dev spawn tileLocation: {tileLocation}");

            SetPlayerState(new Model_PlayerState(
                    (int)tileLocation.x,
                    (int)tileLocation.y,
                    (int)tileLocation.z,
                    gameHelper.facingDirection
                )
            );
        }
        /// If not dev mode and no loaded game data, it's a new game
        else if (!isLoadedGame)
        {
            NewGamePlayerSpawn();
        }

        void NewGamePlayerSpawn()
        {
            level = 0;
            Vector3Int newGameSpawn = newGameSpawnDestination.position.ToVector3Int();
            SetPlayerState(new Model_PlayerState(
                newGameSpawn.x, newGameSpawn.y, newGameSpawn.z, Directions.Up
            ));
        }
    }

    private void OnDidLoad()
    {

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
        CameraTargetToPlayer();
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

    // public void ChangeStateToConvo()
    // {
    //     Debug.Log($"Game.state changed to: {Const_States_Game.DDR}; Game.lastState before this = {lastState}");
    //     lastState = state;
    //     state = Const_States_Game.Convo;
    // }

    public void InitiateLevel()
    {
        SetLevelBehavior();
        
        StartBgMusic();
        SetTileMaps();
        ActivateLevel();    /// Unity startup lifeCycle events are called here
        // CreatePlayer();
        SetupPlayerOnLevel();
        CameraMoveToTarget();

        SetupDialogueManager();
        SetupThoughtManager();
        SetupHintManager();

        // must occur last to have references set
        InitLevelBehavior();
    }

    private void SetLevelBehavior()
    {
        lastLevelBehavior = levelBehavior;
        levelBehavior = Levels.levelsData[level].behavior;
    }

    void InitLevelBehavior()
    {
        persistentDropsContainer.ActivatePersistentDropsForLevel(level);
        
        if (levelBehavior == null)  return;
        print($"level: {level}; levelBehavior: {levelBehavior}... lastLevelBehavior: {lastLevelBehavior}");
        levelBehavior.Setup();
    }

    public void DestroyLevel()
    {
        levelBehavior.Cleanup();
        
        ClearNPCs();
        ClearInteractableObjects();
        ClearSavePoint();
        ClearDemons();
        DestroyTmpTargets();
        DestroyAudioOneShotSources();
        ClearTilemaps();
        ClearDrops();
        
        StopMovingNPCThemes();
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

    /* =======================================================================
        _LEVEL BEHAVIOR_
    ======================================================================= */
    public bool ActivateTrigger(string Id)
    {
        return levelBehavior.ActivateTrigger(Id);
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
        grid = Levels.levelsData[level].grid;
        tileMap = Levels.levelsData[level].tileMap;
        exitsTileMaps = Levels.levelsData[level].exitsTileMaps;
        entrancesTileMap = Levels.levelsData[level].entrancesTileMap;
        pushableTileMap = Levels.levelsData[level].pushableTileMap;
    }

    private void ActivateLevel()
    {
        grid.SetActive(true);
    }

    void ClearTilemaps()
    {
        grid.SetActive(false);
    }

    public void SetNewTileMapGround(Tilemap _tileMap)
    {   
        tileMap = _tileMap;   
    }

    public Tilemap GetTileMap()
    {
        return tileMap;
    }

    public Tilemap GetEntrancesTileMap()
    {
        return entrancesTileMap;
    }

    public Tilemap[] GetExitsTileMaps()
    {
        return exitsTileMaps;
    }

    public Tilemap GetPushablesTileMap()
    {
        return pushableTileMap;
    }

    /* =======================================================================
        _PLAYER_
    ======================================================================= */

    void CreatePlayer()
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
            playerState,
            playerData.isLightOn
        );
        player.transform.SetParent(playerContainer, false);
        // camera tracking
        camera.target = player.transform;
        
        SetVCamsFollowPlayer();
    }

    public void SetupPlayerOnLevel()
    {
        Model_Level levelData = Levels.levelsData[level];
        Model_Player playerData = levelData.playerData;   

        player.InitializeOnLevel(
            playerState,
            playerData.isLightOn,
            grid.transform
        );

        PlayerForceSortingLayer(
            playerData.isForceSortingLayer,
            playerData.isForceSortingLayerAxisZ
        );
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
            player.facingDirection
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

    public Script_PlayerGhost GetPlayerGhost()
    {
        return player.GetPlayerGhost();
    }

    public Script_PlayerMovementAnimator GetPlayerMovementAnimator()
    {
        return player.GetPlayerMovementAnimator();
    }

    public void AddPlayerThought(Model_Thought thought)
    {
        playerThoughtHandler.AddPlayerThought(thought, thoughts);
        
        int thoughtCount = playerThoughtHandler.GetThoughtsCount(thoughts);
        
        playerThoughtsInventoryManager.UpdatePlayerThoughts(thought, thoughts, thoughtSlots);
    }

    public void RemovePlayerThought(Model_Thought thought)
    {
        playerThoughtHandler.RemovePlayerThought(thought, thoughts);

        playerThoughtsInventoryManager.UpdatePlayerThoughts(thought, thoughts, thoughtSlots);
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

    public Transform GetPlayerTransform()
    {
        return player.GetComponent<Transform>();
    }

    public bool GetPlayerIsTalking()
    {
        return player.GetIsTalking();
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

    public void DieEffects(Script_GameOverController.DeathTypes _deathType)
    {
        deathType = _deathType;
        
        ChangeStateCutScene();

        // fade screen to black
        Time.timeScale = timeManager.dieTimeScale;
        transitionManager.GetComponent<Script_TimelineController>()
            .PlayableDirectorPlayFromTimelines(0, 0);
    }
    void OnDiePlayableDone(PlayableDirector aDirector)
    {
        if (
            aDirector.playableAsset == transitionManager
                .GetComponent<Script_TimelineController>().timelines[0]
        )
        {
            // return timeScale to normal
            Time.timeScale = 1.0f;
            
            
            Script_SceneManager.ToGameOver(deathType); 
            // Script_SceneManager.ToTitleScene();
        }
    }
    public int PlayerFullHeal()
    {
        return GetPlayer().FullHeal();
    }

    public int PlayerHurt(int dmg, Script_HitBox hitBox)
    {
        return GetPlayer().Hurt(dmg, hitBox);
    }

    public int PlayerHurtFromThought(int dmg, Model_Thought thought)
    {
        return GetPlayer().HurtThoughts(dmg, thought);
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
    
    public void OpenInventory()
    {
        lastState = state;
        ChangeStateToInventory();
        playerThoughtsInventoryManager.OpenInventory();
    }

    public void CloseInventory()
    {
        ChangeStateLastState(Const_States_Game.Inventory);
        player.SetLastState();
        playerThoughtsInventoryManager.CloseInventory();
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

    public void HighlightItem(int id, bool isOn, bool showDesc)
    {
        menuController.HighlightItem(id, isOn, showDesc);
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

    public Script_Item[] GetInventoryItems()
    {
        return menuController.GetInventoryItems();
    }

    public Script_Sticker[] GetEquipmentItems()
    {
        return menuController.GetEquipmentItems();
    }

    /* =======================================================================
        _HEALTH_
    ======================================================================= */
    public void HideHealth()
    {
        healthManager.Close();
    }

    public void ShowHealth()
    {
        healthManager.Open();
    }

    /* =======================================================================
        _CANVASES_
    ======================================================================= */
    public void ShowHint(string s)
    {
        hintManager.ShowTextHint(s);
    }

    public void HideHint()
    {
        hintManager.HideTextHint();
    }

    public void SetupHintManager()
    {
        hintManager.Setup();
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
            HideHealth();
        }

        if (type == Const_States_PlayerViews.Health)
        {
            ShowHealth();
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
        // if (!bgMusicManager.GetIsPlaying())    UnPauseBgMusic();
        
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
    
    // must give world offset bc IOs in Scene are parented by world
    public void SetupInteractableObjectsText(
        Transform textObjectParent,
        bool isInitialize
    )
    {
        interactableObjectCreator.SetupInteractableObjectsText(
            textObjectParent,
            interactableObjects,
            GetRotationToFaceCamera(),
            dialogueManager,
            player,
            worldOffset,
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
            GetRotationToFaceCamera(),
            dialogueManager,
            player,
            worldOffset,
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
            GetRotationToFaceCamera(),
            dialogueManager,
            player,
            worldOffset,
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
            GetRotationToFaceCamera(),
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

    // public void CreateInteractableObjects(
    //     bool[] switchesState,
    //     bool isForceSortingLayer,
    //     bool isSortingLayerAxisZ = true,
    //     int offset = 0
    // )
    // {
    //     interactableObjectCreator.CreateInteractableObjects(
    //         Levels.levelsData[level].InteractableObjectsData,
    //         interactableObjects,
    //         switches,
    //         GetRotationToFaceCamera(),
    //         dialogueManager,
    //         player,
    //         switchesState,
    //         isForceSortingLayer,
    //         isSortingLayerAxisZ,
    //         offset
    //     );
    // }

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

    void SetupDialogueManager()
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

    void StartBgMusic()
    {
        // TODO: make this a general theme player, not just for Ero
        if (npcBgThemePlayer != null && GetNPCThemeMusicIsPlaying())   return;
        
        int i = Levels.levelsData[level].bgMusicAudioClipIndex;
        
        bgMusicManager.Play(i);
    }

    public void SwitchBgMusic(int i)
    {
        bgMusicManager.Play(i, forcePlay: true);
    }

    public void StopBgMusic()
    {
        bgMusicManager.Stop();
    }

    public void PauseBgMusic()
    {
        bgMusicManager.Pause();
    }

    public void UnPauseBgMusic()
    {
        if (bgMusicManager != null)
            bgMusicManager.UnPause();
    }

    public void PlayNPCBgTheme(Script_BgThemePlayer bgThemePlayerPrefab)
    {
        npcBgThemePlayer = Instantiate(
            bgThemePlayerPrefab,
            player.transform.position,
            Quaternion.identity
        );
        npcBgThemePlayer.transform.SetParent(bgThemeSpeakersContainer, false);
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
    // disable and then enable the Vcam after the Follow Target teleports will snap this
    public CinemachineVirtualCamera SnapToPlayer(Vector3 prevPlayerPos)
    {
        CinemachineVirtualCamera activeVCam = cinemachineBrain
            .ActiveVirtualCamera.VirtualCameraGameObject
            .GetComponent<CinemachineVirtualCamera>();
        
        Debug.Log($"Snap activeVCam to player: {activeVCam}");
        Script_Player p = GetPlayer();
        // https://forum.unity.com/threads/cameras-no-longer-snapping-after-being-disabled-enabled.729242/#post-6276506
        activeVCam.OnTargetObjectWarped(p.transform, p.transform.position - prevPlayerPos);
        // https://forum.unity.com/threads/proper-way-to-reset-follow-camera.747101/
        activeVCam.PreviousStateIsValid = false;

        return activeVCam;

        /// Snapping via disabling / enabling VCam, doesn't seem to work
        // activeVCam.enabled = false;
        // StartCoroutine(ReenableActiveVCam());
        // https://forum.unity.com/threads/how-to-snap-a-virtual-camera-to-a-target.481142/
        // IEnumerator ReenableActiveVCam()
        // {
        //     yield return new WaitForEndOfFrame();
        //     activeVCam.enabled = true;
        // }
    }

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

    public void SetVCamsFollowPlayer()
    {
        VCam.FollowTarget(player.transform);
        VCamDramaticZoom.FollowTarget(player.transform);
    }

    /* =========================================================================
        _CAMERA_
    ========================================================================= */
    /// <summary>
    /// TODO: REMOVE MOST THESE METHODS BC USING VCAM NOW
    /// </summary>
    public void ChangeCameraTargetToNPC(int i)
    {
        camera.SetTarget(NPCs[i].transform);
        // move camera fast
        CameraMoveToTarget();
    }

    public void ChangeCameraTargetToGameObject(GameObject obj)
    {
        camera.SetTarget(obj.transform);
        CameraMoveToTarget();
    }

    public void CameraTargetToPlayer()
    {
        camera.target = player.transform;
        CameraMoveToTarget();
    }

    public void CameraMoveToTarget()
    {
        camera.MoveToTarget();
    }

    public void SetOrthographicSizeDefault()
    {
        camera.SetOrthographicSizeDefault();
    }

    public void SetOrthographicSize(float size)
    {
        camera.SetOrthographicSize(size);
    }

    public void CameraZoomSmooth(float size, float time, Vector3 loc, Action cb)
    {
        camera.ZoomSmooth(size, time, loc, cb);
    }

    public void CameraMoveToTargetSmooth(float time, Vector3 loc, Action cb)
    {
        camera.MoveToTargetSmooth(time, loc, cb);
    }

    public void CameraSetIsTrackingTarget(bool isTracking)
    {
        camera.SetIsTrackingTarget(isTracking);
    }

    public void SetCameraOffset(Vector3 offset)
    {
        camera.SetOffset(offset);
    }

    public void SetCameraOffsetDefault()
    {
        camera.SetOffsetToDefault();
    }
    
    public void CameraInstantMoveSpeed()
    {
        camera.InstantTrackSpeed();
    }

    public void CameraDefaultMoveSpeed()
    {
        camera.DefaultSpeed();
    }

    public Vector3 GetRotationToFaceCamera()
    {
        return camera.GetRotationAdjustment();
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
        bool isSilent = false
    )
    {
        exitsHandler.Exit(
            level,
            playerNextSpawnPosition,
            playerFacingDirection,
            isExit,
            isSilent
        );
    }

    public void HandleExitCutSceneLevelBehavior()
    {
        levelBehavior.HandleExitCutScene();
    }

    public void OnDoorLockUnlock(int id)
    {
        levelBehavior.OnDoorLockUnlock(id);
    }

    /* =========================================================================
        _CUTSCENES_
    ========================================================================= */    
    public void MelanholicTitleCutScene()
    {
        cutSceneManager.MelanholicTitleCutScene();
    }

    /* =========================================================================
        _SAVELOAD_
    ========================================================================= */
    public Model_PersistentDrop[] GetPersistentDrops()
    {
        return persistentDropsContainer.GetPersistentDropModels();
    }
}
