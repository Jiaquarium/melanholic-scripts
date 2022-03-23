using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Dev_GameHelper : MonoBehaviour
{
    [SerializeField] private bool isDisableHUD;
    
    public Vector3Int playerSpawn;
    public int level;
    public Directions facingDirection;
    
    [SerializeField] private Script_ExitMetadataObject playerDefaultSpawn;
    [SerializeField] private Script_ExitMetadataObject woodsEntrance;
    [SerializeField] private Script_ExitMetadataObject DiningEntrance;
    [SerializeField] private Script_ExitMetadataObject HallwayWithSecretEntrance;
    [SerializeField] private Script_ExitMetadataObject MirrorHallsPuzzleEntrance;
    [SerializeField] private Script_ExitMetadataObject IdsRoomEntrance;
    [SerializeField] private Script_ExitMetadataObject ElleniasRoomEntrance;
    [SerializeField] private Script_ExitMetadataObject BallroomEntranceFromHMSHall;
    [SerializeField] private Script_ExitMetadataObject LastElevatorEntrance;
    [SerializeField] private Script_ExitMetadataObject EileensMindEntrance;
    [SerializeField] private Script_ExitMetadataObject WellsWorldEntrance;
    [SerializeField] private Script_ExitMetadataObject XXXWorldEntrance;
    [SerializeField] private Script_ExitMetadataObject UrselksSaloonHallwayEntrance;
    [SerializeField] private Script_ExitMetadataObject UrselksSaloonEntrance;
    [SerializeField] private Script_ExitMetadataObject KTV2Entrance;
    [SerializeField] private Script_ExitMetadataObject FireplacePuzzleEntrance;
    [SerializeField] private Script_ExitMetadataObject FireplaceTraining1Entrance;
    [SerializeField] private Script_ExitMetadataObject CatWalkEntrance;
    [SerializeField] private Script_ExitMetadataObject UnderworldEntrance;
    [SerializeField] private Script_ExitMetadataObject GrandMirrorEntrance;
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

    private bool didSetWeekend;

    void Start()
    {
        if (isDisableHUD && Debug.isDebugBuild)
            SetUIActive(false);
    }
    
    public void DefaultPlayerSpawnPos()
    {
        Teleport(playerDefaultSpawn);
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
    
    public void ExitToIdsRoom()
    {
        Teleport(IdsRoomEntrance);
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

    public void BuildSetup()
    {
        runsManager.StartWeekdayCycle();
        Script_Game.LevelsInactivate();
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

        // Set Run and Cycle data to Weekend Thursday.
        Script_Game.Game.NextRunSaveInitialize(false, Script_Run.DayId.thu);
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
        
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Dev_GameHelper t = target as Dev_GameHelper;

            var style = EditorStyles.foldoutHeader;

            showSpawns = EditorGUILayout.Foldout(showSpawns, "Spawns", style);
            if (showSpawns)
            {
                if (GUILayout.Button("Go To: Hotel Lobby"))
                {
                    t.DefaultPlayerSpawnPos();
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
                
                if (GUILayout.Button("Go To: Ids Room"))
                {
                    t.ExitToIdsRoom();
                }

                if (GUILayout.Button("Go To: Ellenia's Room"))
                {
                    t.ExitToElleniasRoom();
                }

                if (GUILayout.Button("Go To: Last Elevator"))
                {
                    t.ExitToLastElevator();
                }

                if (GUILayout.Button("Go To: Ballroom (HMS Hall Entrance)"))
                {
                    t.ExitToBallroomFromHMSHall();
                }

                if (GUILayout.Button("Go To: Eileen's Mind"))
                {
                    t.ExitToEileensMind();
                }

                EditorGUILayout.LabelField("Myne's Lair", EditorStyles.miniLabel);

                if (GUILayout.Button("Go To: Grand Mirror"))
                {
                    t.ExitToGrandMirror();
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
                }

                if (GUILayout.Button("Set Baked Grids Inactive"))
                {
                    t.SetBakedLightingGridsActive(false);
                }
            }

            showBuildSettings = EditorGUILayout.Foldout(showBuildSettings, "Build Settings", style);
            if (showBuildSettings)
            {
                if (GUILayout.Button("Build Setup", GUILayout.Height(32)))
                {
                    t.BuildSetup();
                }

                if (GUILayout.Button("Build Dev Explore Setup"))
                {
                    t.BuildDevExploreSetup();
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
        }
    }
    #endif
}
