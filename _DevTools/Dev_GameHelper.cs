using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Dev_GameHelper : MonoBehaviour
{
    public static readonly string[] ScriptingDefineSymbolsDev = {
        "UNITY_POST_PROCESSING_STACK_V2",
        "ENABLE_LOGS"
    };

    public static readonly string[] ScriptingDefineSymbolsProd = {
        "UNITY_POST_PROCESSING_STACK_V2",
    };
    
    private static float notificationDuration = 1f;
    
    [SerializeField] private bool isDisableHUD;
    [SerializeField] private bool isStartAwareTime;
    [SerializeField] private bool isStartWarningTime;
    [SerializeField] private bool isStartDangerTime;
    
    public Vector3Int playerSpawn;
    public int level;
    public Directions facingDirection;
    
    [SerializeField] private Script_ExitMetadataObject playerDefaultSpawn;
    [SerializeField] private Script_ExitMetadataObject elevatorBayV2Entrance;
    
    [SerializeField] private Script_ExitMetadataObject woodsEntrance;
    [SerializeField] private Script_ExitMetadataObject DiningEntrance;
    [SerializeField] private Script_ExitMetadataObject HallwayWithSecretEntrance;
    [SerializeField] private Script_ExitMetadataObject MirrorHallsPuzzleEntrance;
    [SerializeField] private Script_ExitMetadataObject HallwayToBasementEntrance;
    [SerializeField] private Script_ExitMetadataObject IdsRoomEntrance;
    [SerializeField] private Script_ExitMetadataObject DarkCorridorEntrance;
    [SerializeField] private Script_ExitMetadataObject EileensRoomEntrance;
    [SerializeField] private Script_ExitMetadataObject ElleniasRoomEntrance;
    [SerializeField] private Script_ExitMetadataObject BallroomEntranceFromHMSHall;
    [SerializeField] private Script_ExitMetadataObject LastElevatorEntrance;
    [SerializeField] private Script_ExitMetadataObject EileensMindEntrance;
    [SerializeField] private Script_ExitMetadataObject EileensMindDoneLoc;
    [SerializeField] private Script_ExitMetadataObject WellsWorldEntrance;
    [SerializeField] private Script_ExitMetadataObject XXXWorldEntrance;
    [SerializeField] private Script_ExitMetadataObject UrselksSaloonHallwayEntrance;
    [SerializeField] private Script_ExitMetadataObject UrselksSaloonEntrance;
    [SerializeField] private Script_ExitMetadataObject KTV2Entrance;
    [SerializeField] private Script_ExitMetadataObject FireplacePuzzleEntrance;
    [SerializeField] private Script_ExitMetadataObject FireplaceTraining1Entrance;
    [SerializeField] private Script_ExitMetadataObject FireplaceTraining2Entrance;
    [SerializeField] private Script_ExitMetadataObject CatWalkEntrance;
    [SerializeField] private Script_ExitMetadataObject UnderworldEntrance;
    [SerializeField] private Script_ExitMetadataObject GrandMirrorEntrance;
    [SerializeField] private Script_ExitMetadataObject GrandMirrorFrontOfMirror;
    [SerializeField] private Script_ExitMetadataObject CelestialGardensEntrance;
    [SerializeField] private Script_ExitMetadataObject RockGardenEntrance;
    [SerializeField] private Script_ExitMetadataObject CatWalk2Entrance;
    [SerializeField] private Script_ExitMetadataObject FountainEntrance;
    [SerializeField] private Script_ExitMetadataObject LabyrinthEntrance;

    [SerializeField] private Dev_InventoryTester inventoryTester;

    [SerializeField] private Script_RunsManager runsManager;

    // ----------------------------------------------------------------------
    // Level Behaviors
    [SerializeField] private Script_LevelBehavior_0 woodsBehavior;
    [SerializeField] private Script_LevelBehavior_4 hallwayWithSecretBehavior;
    [SerializeField] private Script_LevelBehavior_10 IdsRoomBehavior;
    [SerializeField] private Script_LevelBehavior_21 EileensRoomBehavior;
    [SerializeField] private Script_LevelBehavior_25 ElleniasRoomBehavior;
    [SerializeField] private Script_LevelBehavior_26 EileensMindBehavior;
    [SerializeField] private Script_LevelBehavior_27 LastElevatorBehavior;
    [SerializeField] private Script_LevelBehavior_48 MynesGrandMirrorRoomBehavior;
    [SerializeField] private List<Script_WorldTile> WellsWorldTiles;
    [SerializeField] private List<Script_WorldTile> CelestialGardensWorldTiles;
    [SerializeField] private List<Script_WorldTile> XXXWorldTiles;

    // ----------------------------------------------------------------------
    // Quests Done Dynamic Settings
    [SerializeField] private bool ElleniaPuzzleDone;
    [SerializeField] private bool IdsPuzzleDone;
    [SerializeField] private bool EileenPuzzleDone;
    [SerializeField] private bool WellsWorldPuzzleDone;
    [SerializeField] private bool CelestialGardensPuzzleDone;
    [SerializeField] private bool XXXWorldPuzzleDone;

    // ----------------------------------------------------------------------
    // Level Grids
    [SerializeField] private List<Grid> bakedLightingGrids;

    // ----------------------------------------------------------------------
    // Game Objects for Activation / Deactivation
    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject endings;
    [SerializeField] private CanvasGroup fullArtCanvasGroup;
    [SerializeField] private CanvasGroup menuCanvasGroup;
    [SerializeField] private List<CanvasGroup> faceOffCanvasGroups;
    [SerializeField] private CanvasGroup DDRCanvasGroup;

    // ----------------------------------------------------------------------
    // Dev Canvases
    [SerializeField] private Script_CanvasGroupController saveDevCanvas;

    private bool didSetWeekend;

    void Awake()
    {
        saveDevCanvas.Close();
    }
    
    void Start()
    {
        if (isDisableHUD && Debug.isDebugBuild)
            SetUIActive(false);
        
        var clock = Script_ClockManager.Control;
        
        if (isStartAwareTime)
            clock.AwareTime();
        else if (isStartWarningTime)
            clock.WarningTime();
        else if (isStartDangerTime)
            clock.DangerTime();
    }
    
    public void DefaultPlayerSpawnPos()
    {
        Teleport(playerDefaultSpawn);
    }

    public void ExitToElevatorBayV2()
    {
        Teleport(elevatorBayV2Entrance);
    }

    public void ExitToWoods()
    {
        Teleport(woodsEntrance);
    }
    
    public void ExitToDining()
    {
        Teleport(DiningEntrance);
    }
    
    public void ExitToHallWithSecret()
    {
        Teleport(HallwayWithSecretEntrance);
    }

    public void ExitToMirrorHallsPuzzle()
    {
        Teleport(MirrorHallsPuzzleEntrance);
    }
    
    public void ExitToHallwayToBasement()
    {
        Teleport(HallwayToBasementEntrance);
    }
    
    public void ExitToIdsRoom()
    {
        Teleport(IdsRoomEntrance);
    }

    public void ExitToDarkCorridor()
    {
        Teleport(DarkCorridorEntrance);
    }

    public void ExitToEileensRoom()
    {
        Teleport(EileensRoomEntrance);
    }
    
    public void ExitToElleniasRoom()
    {
        Teleport(ElleniasRoomEntrance);
    }

    public void ExitToLastElevator()
    {
        Teleport(LastElevatorEntrance);
    }

    public void ExitToBallroomFromHMSHall()
    {
        Teleport(BallroomEntranceFromHMSHall);
    }

    public void ExitToEileensMind()
    {
        Teleport(EileensMindEntrance);
    }

    public void ExitToEileensMindDone()
    {
        Teleport(EileensMindDoneLoc);
    }

    public void ExitToWellsWorld()
    {
        Teleport(WellsWorldEntrance);
    }

    public void ExitToXXXWorld()
    {
        Teleport(XXXWorldEntrance);
    }

    public void ExitToUrselksSaloonHallway()
    {
        Teleport(UrselksSaloonHallwayEntrance);
    }

    public void ExitToUrselksSaloon()
    {
        Teleport(UrselksSaloonEntrance);
    }

    public void ExitToKTV2()
    {
        Teleport(KTV2Entrance);
    }

    public void ExitToFireplaceTraining1()
    {
        Teleport(FireplaceTraining1Entrance);
    }

    public void ExitToFireplaceTraining2()
    {
        Teleport(FireplaceTraining2Entrance);
    }

    public void ExitToFireplacePuzzle()
    {
        Teleport(FireplacePuzzleEntrance);
    }

    public void ExitToCatWalk()
    {
        Teleport(CatWalkEntrance);
    }

    public void ExitToUnderworld()
    {
        Teleport(UnderworldEntrance);
    }
    
    public void ExitToGrandMirror()
    {
        Teleport(GrandMirrorEntrance);
    }

    public void ExitToGrandMirrorFrontOfMirror()
    {
        Teleport(GrandMirrorFrontOfMirror);
    }

    public void ExitToCelestialGardens()
    {
        Teleport(CelestialGardensEntrance);
    }
    
    public void ExitToRockGarden()
    {
        Teleport(RockGardenEntrance);
    }

    public void ExitToCatWalk2()
    {
        Teleport(CatWalk2Entrance);
    }

    public void ExitToFountain()
    {
        Teleport(FountainEntrance);
    }
    
    public void ExitToLabyrinth()
    {
        Teleport(LabyrinthEntrance);
    }

#if UNITY_EDITOR
    public void AddLogs()
    {
        PlayerSettings.SetScriptingDefineSymbols(
            UnityEditor.Build.NamedBuildTarget.Standalone,
            ScriptingDefineSymbolsDev
        );
    }

    public void RemoveLogs()
    {
        PlayerSettings.SetScriptingDefineSymbols(
            UnityEditor.Build.NamedBuildTarget.Standalone,
            ScriptingDefineSymbolsProd
        );
    }
#endif

    public void BuildSetup()
    {
        runsManager.StartWeekdayCycle();
        Script_Game.LevelsInactivate();

        Dev_Logger.Debug("Build Setup: Setting GameObjects active states now!");
        
        SetCanvasStates();

        // Notify on state of World Tiles
        bool isAllWellWorldActive = true;
        WellsWorldTiles.ForEach(worldTile => {
            if (!worldTile.gameObject.activeSelf)
                isAllWellWorldActive = false;
        });

        bool isAllCelGardensWorldActive = true;
        CelestialGardensWorldTiles.ForEach(worldTile => {
            if (!worldTile.gameObject.activeSelf)
                isAllCelGardensWorldActive = false;
        });

        bool isAllXXXWorldActive = true;
        XXXWorldTiles.ForEach(worldTile => {
            if (!worldTile.gameObject.activeSelf)
                isAllXXXWorldActive = false;
        });

        if (!isAllWellWorldActive)
            Debug.LogWarning($"<color=orange>NOT ALL WELLS WORLD TILES ARE ACTIVE</color>");
        
        if (!isAllCelGardensWorldActive)
            Debug.LogWarning($"<color=orange>NOT ALL CEL GARDENS WORLD TILES ARE ACTIVE</color>");
        
        if (!isAllXXXWorldActive)
            Debug.LogWarning($"<color=orange>NOT ALL XXX WORLD TILES ARE ACTIVE</color>");

        void SetCanvasStates()
        {
            if (!settings.activeInHierarchy)
            {
                Debug.Log($"<color=red>SETTINGS should be ACTIVE</color>");
                settings.gameObject.SetActive(true);
            }
            
            if (HUD.activeInHierarchy)
            {
                Debug.Log($"<color=red>HUD should be INACTIVE</color>");
                HUD.gameObject.SetActive(false);
            }

            if (endings.activeInHierarchy)
            {
                Debug.Log($"<color=red>ENDINGS should be INACTIVE</color>");
                endings.gameObject.SetActive(false);
            }

            if (saveDevCanvas.gameObject.activeInHierarchy)
            {
                Debug.Log($"<color=red>SAVE DEV CANVAS should be INACTIVE</color>");
                saveDevCanvas.gameObject.SetActive(false);
            }

            if (!fullArtCanvasGroup.gameObject.activeInHierarchy)
            {
                Debug.Log($"<color=red>FULL CANVASGROUP should be ACTIVE</color>");
                fullArtCanvasGroup.gameObject.SetActive(true);
            }

            if (menuCanvasGroup.gameObject.activeInHierarchy)
            {
                Debug.Log($"<color=red>MENU CANVASGROUP should be INACTIVE</color>");
                menuCanvasGroup.gameObject.SetActive(false);
            }

            faceOffCanvasGroups.ForEach(faceOffCanvasGroup => {
                if (faceOffCanvasGroup.gameObject.activeInHierarchy)
                {
                    Debug.Log($"<color=red>{faceOffCanvasGroup.name} should be INACTIVE</color>");
                    faceOffCanvasGroup.gameObject.SetActive(false);
                }
            });

            if (DDRCanvasGroup.gameObject.activeInHierarchy)
            {
                Debug.Log($"<color=red>{DDRCanvasGroup.name} should be INACTIVE</color>");
                DDRCanvasGroup.gameObject.SetActive(false);
            }
        }
    }

    public void BuildDevExploreSetup()
    {
        runsManager.StartWeekendCycle();
        Script_Game.LevelsInactivate();
    }

    public void SetAllQuestsDoneToday()
    {
        Script_Game.Game.ElleniasRoomBehavior.isCurrentPuzzleComplete       = true;
        Script_Game.Game.IdsRoomBehavior.isCurrentPuzzleComplete            = true;
        Script_Game.Game.EileensMindBehavior.isCurrentPuzzleComplete        = true;
        Script_Game.Game.WellsWorldBehavior.isCurrentMooseQuestComplete     = true;
        Script_Game.Game.GardenLabyrinthBehavior.isCurrentPuzzleComplete    = true;
        Script_Game.Game.KTVRoom2Behavior.IsCurrentPuzzleComplete           = true;
    }

    public void SetQuestsDoneDynamic()
    {
        Script_Game.Game.ElleniasRoomBehavior.isCurrentPuzzleComplete       = ElleniaPuzzleDone;
        Script_Game.Game.IdsRoomBehavior.isCurrentPuzzleComplete            = IdsPuzzleDone;
        Script_Game.Game.EileensMindBehavior.isCurrentPuzzleComplete        = EileenPuzzleDone;
        Script_Game.Game.WellsWorldBehavior.isCurrentMooseQuestComplete     = WellsWorldPuzzleDone;
        Script_Game.Game.GardenLabyrinthBehavior.isCurrentPuzzleComplete    = CelestialGardensPuzzleDone;
        Script_Game.Game.KTVRoom2Behavior.IsCurrentPuzzleComplete           = XXXWorldPuzzleDone;
    }

    public void SolveAllMynesMirrors()
    {
        Script_ScarletCipherManager.Control.Dev_ForceSolveAllMirrors();
    }

    public void SaveCurrent()
    {
        Script_SaveGameControl.control.Save(); 
    }

    private void Teleport(Script_ExitMetadata exit)
    {
        Script_Game.Game.Exit(
            exit.data.level,
            exit.data.playerSpawn,
            exit.data.facingDirection,
            isExit: true,
            isSilent: false,
            exitType: Script_Exits.ExitType.Default
        );   
    }

    // ----------------------------------------------------------------------
    // Game States

    // Set current save state to the Weekend Cycle.
    public void WeekendCycle()
    {
        if (didSetWeekend)
            return;
        
        didSetWeekend = true;
        
        // Set Items for Weekend Cycle.
        inventoryTester.WeekendCycle();
        
        ToGrandMirrorState();

        MynesGrandMirrorRoomBehavior.IsDone = true;

        Script_Game.Game.StartWeekendCycleSaveInitialize();
    }

    public void StartSundayCycle()
    {
        Script_Game.Game.StartSundayCycleSaveInitialize();
    }

    // Set to Last Elevator to go to Grand Mirror Room as if just completed Eileen's Mind Quest. 
    public void BeforeGrandMirror()
    {
        ToGrandMirrorState();
        MynesGrandMirrorRoomBehavior.IsDone = false;

        // Set Items for Weekend Cycle.
        inventoryTester.GrandMirror();

        Teleport(LastElevatorEntrance);
    }

    public void WeekendWithLantern()
    {
        Script_Game.Game.AddItemById(Const_Items.LetThereBeLightId);   
        WeekendCycle();
    }

    private void ToGrandMirrorState()
    {
        // Set State at this point in game.
        woodsBehavior.didStartThought                               = true;
        
        hallwayWithSecretBehavior.didPickUpMelancholyPianoSticker   = true;

        IdsRoomBehavior.gotBoarNeedle                               = true;

        EileensRoomBehavior.spokenWithEileen                        = true;

        ElleniasRoomBehavior.isPuzzleComplete                       = true;
        ElleniasRoomBehavior.spokenWithEllenia                      = true;

        EileensMindBehavior.isPuzzleComplete                        = true;
        EileensMindBehavior.didActivateDramaticThoughts             = true;
        EileensMindBehavior.gotIceSpikeSticker                      = true;

        LastElevatorBehavior.GotPsychicDuck                         = true;
    }

    public void SetElleniaPuzzleDone()
    {
        ElleniasRoomBehavior.isPuzzleComplete                       = true;
        ElleniasRoomBehavior.spokenWithEllenia                      = true;
    }

    public void ElleniaComfortableState()
    {
        // Must be either Fri or Sat because of Ellenia's condition.
        if (Script_Game.Game.Run.dayId == Script_Run.DayId.thu)
            runsManager.IncrementRun();
        
        Script_EventCycleManager.Control.SetElleniaDidTalkCountdownMax();
        Script_EventCycleManager.Control.DidTalkToEllenia--;
    }

    // ----------------------------------------------------------------------
    // Lighting

    public void SetBakedLightingGridsActive(bool isActive)
    {
        bakedLightingGrids.ForEach(grid => grid.gameObject.SetActive(isActive));
    }

    // ----------------------------------------------------------------------
    // UI

    public void SetUIActive(bool isActive)
    {
        Script_Game.Game.IsHideHUD = !isActive;
    }

    public void ShowSaveDevCanvas()
    {
        saveDevCanvas.Open();

        StartCoroutine(WaitToClose());

        IEnumerator WaitToClose()
        {
            yield return new WaitForSeconds(notificationDuration);

            saveDevCanvas.Close();
        }
    }

    // ----------------------------------------------------------------------
    // Checks
    
    private void CheckAnimatorsNotRootMotion()
    {
        Debug.Log("Looking for animators...");

        List<Animator> animators = FindObjectsOfType<Animator>(true)
            .Where(animator => animator.applyRootMotion).ToList();

        if (animators.Count == 0)
        {
            Debug.Log("<color=lime>All good!</color>");
            return;
        }
        
        animators.ForEach(animator => {
            if (animator.applyRootMotion)
                Debug.Log(animator);
        });
    }

    // ----------------------------------------------------------------------

    #if UNITY_EDITOR
    [CustomEditor(typeof(Dev_GameHelper))]
    public class Dev_GameHelperTester : Editor
    {
        private static bool showSpawns;
        private static bool showPaintingQuests;
        private static bool showStates;
        private static bool showBuildSettings;
        private static bool showLightSettings;
        private static bool showUISettings;
        private static bool showQAChecks;
        
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Dev_GameHelper t = target as Dev_GameHelper;

            var style = EditorStyles.foldoutHeader;

            showSpawns = EditorGUILayout.Foldout(showSpawns, "Spawns", style);
            if (showSpawns)
            {
                EditorGUILayout.LabelField("Hotel", EditorStyles.miniLabel);
                
                if (GUILayout.Button("Go To: Hotel Lobby"))
                {
                    t.DefaultPlayerSpawnPos();
                }

                if (GUILayout.Button("Go To: Bay V2"))
                {
                    t.ExitToElevatorBayV2();
                }

                if (GUILayout.Button("Go To: Last Elevator"))
                {
                    t.ExitToLastElevator();
                }
                
                if (GUILayout.Button("Go To: Grand Mirror"))
                {
                    t.ExitToGrandMirror();
                }

                if (GUILayout.Button("Go To: Grand Mirror (Front Of Mirror)"))
                {
                    t.ExitToGrandMirrorFrontOfMirror();
                }

                if (GUILayout.Button("Go To: Grand Mirror R2"))
                {
                    Script_Game.Game.TeleportToGrandMirrorBackgroundR2();
                }
                
                EditorGUILayout.LabelField("Intro Rooms", EditorStyles.miniLabel);
                
                if (GUILayout.Button("Go To: Woods"))
                {
                    t.ExitToWoods();
                }
                
                if (GUILayout.Button("Go To: Dining"))
                {
                    t.ExitToDining();
                }
                
                if (GUILayout.Button("Go To: Hallway with Secret"))
                {
                    t.ExitToHallWithSecret();
                }

                if (GUILayout.Button("Go To: Mirror Halls Puzzle"))
                {
                    t.ExitToMirrorHallsPuzzle();
                }
                
                if (GUILayout.Button("Go To: Hallway to Basement"))
                {
                    t.ExitToHallwayToBasement();
                }
                
                if (GUILayout.Button("Go To: Ids Room"))
                {
                    t.ExitToIdsRoom();
                }

                if (GUILayout.Button("Go To: Dark Corridor"))
                {
                    t.ExitToDarkCorridor();
                }

                if (GUILayout.Button("Go To: Eileen's Room"))
                {
                    t.ExitToEileensRoom();
                }
                
                if (GUILayout.Button("Go To: Ellenia's Room"))
                {
                    t.ExitToElleniasRoom();
                }

                if (GUILayout.Button("Go To: Ballroom (HMS Hall Entrance)"))
                {
                    t.ExitToBallroomFromHMSHall();
                }

                if (GUILayout.Button("Go To: Eileen's Mind"))
                {
                    t.ExitToEileensMind();
                }

                if (GUILayout.Button("Go To: Eileen's Mind Done"))
                {
                    t.ExitToEileensMindDone();
                }

                EditorGUILayout.LabelField("Wells World", EditorStyles.miniLabel);
                
                if (GUILayout.Button("Go To: Wells World"))
                {
                    t.ExitToWellsWorld();
                }

                if (GUILayout.Button("Go To: Fireplace Training 1"))
                {
                    t.ExitToFireplaceTraining1();
                }

                if (GUILayout.Button("Go To: Fireplace Training 2"))
                {
                    t.ExitToFireplaceTraining2();
                }

                if (GUILayout.Button("Go To: Fireplace Puzzle"))
                {
                    t.ExitToFireplacePuzzle();
                }

                if (GUILayout.Button("Go To: Catwalk"))
                {
                    t.ExitToCatWalk();
                }

                if (GUILayout.Button("Go To: Underworld"))
                {
                    t.ExitToUnderworld();
                }
                
                EditorGUILayout.LabelField("XXX World", EditorStyles.miniLabel);
                
                if (GUILayout.Button("Go To: XXX World"))
                {
                    t.ExitToXXXWorld();
                }

                if (GUILayout.Button("Go To: Urselks Saloon Hallway"))
                {
                    t.ExitToUrselksSaloonHallway();
                }

                if (GUILayout.Button("Go To: Urselks Saloon"))
                {
                    t.ExitToUrselksSaloon();
                }

                if (GUILayout.Button("Go To: KTV2"))
                {
                    t.ExitToKTV2();
                }

                EditorGUILayout.LabelField("Celestial Gardens World", EditorStyles.miniLabel);
                
                if (GUILayout.Button("Go To: Celestial Gardens"))
                {
                    t.ExitToCelestialGardens();
                }
                
                if (GUILayout.Button("Go To: Rock Garden"))
                {
                    t.ExitToRockGarden();
                }
                
                if (GUILayout.Button("Go To: CatWalk2"))
                {
                    t.ExitToCatWalk2();
                }    
                
                if (GUILayout.Button("Go To: Fountain"))
                {
                    t.ExitToFountain();
                }

                
                if (GUILayout.Button("Go To: Labyrinth"))
                {
                    t.ExitToLabyrinth();
                }
            }

            showPaintingQuests = EditorGUILayout.Foldout(showPaintingQuests, "Painting Quests", style);
            if (showPaintingQuests)
            {
                if (GUILayout.Button("All Quests Done Today"))
                {
                    t.SetAllQuestsDoneToday();
                }

                if (GUILayout.Button("Quests Done Dynamic Today"))
                {
                    t.SetQuestsDoneDynamic();
                }
            }

            showStates = EditorGUILayout.Foldout(showStates, "Game States", style);
            if (showStates)
            {
                if (GUILayout.Button("Save Current"))
                {
                    t.SaveCurrent();
                }

                if (GUILayout.Button("Increment Day (via Last Elevator)"))
                {
                    Script_Game game = Script_Game.Game;
                    
                    Model_Exit exitData = new Model_Exit(
                        game.level,
                        game.GetPlayer().transform.position,
                        game.GetPlayer().FacingDirection
                    );
                    
                    game.ElevatorCloseDoorsCutScene(
                        null,
                        null,
                        Script_Elevator.Types.Last,
                        exitData,
                        Script_Exits.ExitType.SaveAndRestartOnLevel
                    );
                }

                if (GUILayout.Button("Last Elevator Before Grand Mirror"))
                {
                    t.BeforeGrandMirror();
                }
                
                if (GUILayout.Button("Weekend Start"))
                {
                    t.WeekendCycle();
                }

                if (GUILayout.Button("Weekend Start with Lantern"))
                {
                    t.WeekendWithLantern();
                }

                if (GUILayout.Button("Ellenia Quest Done"))
                {
                    t.SetElleniaPuzzleDone();
                }

                if (GUILayout.Button("Ellenia Comfortable State"))
                {
                    t.ElleniaComfortableState();
                }

                EditorGUILayout.LabelField("Sunday (Stateless)", EditorStyles.miniLabel);

                if (GUILayout.Button("Start Sunday Cycle"))
                {
                    t.StartSundayCycle();
                }
            }

            showLightSettings = EditorGUILayout.Foldout(showLightSettings, "Lighting Settings", style);
            if (showLightSettings)
            {
                if (GUILayout.Button("Set Baked Grids Active"))
                {
                    t.SetBakedLightingGridsActive(true);

                    if (GUI.changed)
                    {
                        EditorUtility.SetDirty(t);
                        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(t.gameObject.scene);
                    }
                }

                if (GUILayout.Button("Set Baked Grids Inactive"))
                {
                    t.SetBakedLightingGridsActive(false);

                    if (GUI.changed)
                    {
                        EditorUtility.SetDirty(t);
                        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(t.gameObject.scene);
                    }
                }
            }

            showBuildSettings = EditorGUILayout.Foldout(showBuildSettings, "Build Settings", style);
            if (showBuildSettings)
            {
                if (GUILayout.Button("Build Setup", GUILayout.Height(32)))
                {
                    t.BuildSetup();

                    if (GUI.changed)
                    {
                        EditorUtility.SetDirty(t);
                        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(t.gameObject.scene);
                    }
                }

                if (GUILayout.Button("Add Logs (Add ENABLE_LOGS)", GUILayout.Height(32)))
                {
                    t.AddLogs();

                    if (GUI.changed)
                    {
                        EditorUtility.SetDirty(t);
                        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(t.gameObject.scene);
                    }
                }

                if (GUILayout.Button("Remove Logs (Remove ENABLE_LOGS)", GUILayout.Height(32)))
                {
                    t.RemoveLogs();

                    if (GUI.changed)
                    {
                        EditorUtility.SetDirty(t);
                        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(t.gameObject.scene);
                    }
                }

                if (GUILayout.Button("Delete Player Prefs", GUILayout.Height(24)))
                {
                    PlayerPrefs.DeleteAll();
                    Debug.Log("<color=lime>Deleted Player Prefs</color>");
                }

                if (GUILayout.Button("Build Dev Explore Setup"))
                {
                    t.BuildDevExploreSetup();

                    if (GUI.changed)
                    {
                        EditorUtility.SetDirty(t);
                        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(t.gameObject.scene);
                    }
                }
            }

            showUISettings = EditorGUILayout.Foldout(showUISettings, "UI Settings", style);
            if (showUISettings)
            {
                if (GUILayout.Button("Hide UI / Pause Clock"))
                {
                    t.SetUIActive(false);
                }

                if (GUILayout.Button("Unhide UI / Unpause Clock"))
                {
                    t.SetUIActive(true);
                }
            }

            showQAChecks = EditorGUILayout.Foldout(showQAChecks, "Final QA Checks", style);
            if (showQAChecks)
            {
                if (GUILayout.Button("Check Animators Root Motion"))
                {
                    t.CheckAnimatorsNotRootMotion();
                }
            }
        }
    }
    #endif
}
