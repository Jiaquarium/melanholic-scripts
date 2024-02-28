using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Dev_GameHelper : MonoBehaviour
{
    public static readonly string[] ScriptingDefineSymbolsDev = {
        "UNITY_POST_PROCESSING_STACK_V2",
        "STEAMWORKS_NET",
        "ENABLE_LOGS"
    };

    public static readonly string[] ScriptingDefineSymbolsProd = {
        "UNITY_POST_PROCESSING_STACK_V2",
        "STEAMWORKS_NET"
    };

    public static readonly string[] ScriptingDefineSymbolsDevDisableSteamworks = {
        "UNITY_POST_PROCESSING_STACK_V2",
        "STEAMWORKS_NET",
        "ENABLE_LOGS",
        "FORCE_DISABLE_STEAMWORKS"
    };

    public static readonly string[] ScriptingDefineSymbolsProdDisableSteamworks = {
        "UNITY_POST_PROCESSING_STACK_V2",
        "STEAMWORKS_NET",
        "FORCE_DISABLE_STEAMWORKS"
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
    [SerializeField] private Script_ExitMetadataObject BallroomEntranceFromLastElevator;
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
    [SerializeField] private Script_ScreenFXManager screenFXManager;
    [SerializeField] private Script_GraphicsManager graphicsManager;

    // ----------------------------------------------------------------------
    // Level Behaviors
    [SerializeField] private Script_LevelBehavior_0 woodsBehavior;
    [SerializeField] private Script_LevelBehavior_4 hallwayWithSecretBehavior;
    [SerializeField] private Script_LevelBehavior_10 IdsRoomBehavior;
    [SerializeField] private Script_LevelBehavior_13 CatWalk1Behavior;
    [SerializeField] private Script_LevelBehavior_20 BallroomBehavior;
    [SerializeField] private Script_LevelBehavior_21 EileensRoomBehavior;
    [SerializeField] private Script_LevelBehavior_22 SaloonBehavior;
    [SerializeField] private Script_LevelBehavior_24 KTV2Behavior;
    [SerializeField] private Script_LevelBehavior_25 ElleniasRoomBehavior;
    [SerializeField] private Script_LevelBehavior_26 EileensMindBehavior;
    [SerializeField] private Script_LevelBehavior_27 LastElevatorBehavior;
    [SerializeField] private Script_LevelBehavior_32 hotelLobbyBehavior;
    [SerializeField] private Script_LevelBehavior_42 WellsWorldBehavior;
    [SerializeField] private Script_LevelBehavior_43 CelGardensBehavior;
    [SerializeField] private Script_LevelBehavior_44 XXXWorldBehavior;
    [SerializeField] private Script_LevelBehavior_46 LabyrinthBehavior;
    [SerializeField] private Script_LevelBehavior_47 RockGardenBehavior;
    [SerializeField] private Script_LevelBehavior_48 MynesGrandMirrorRoomBehavior;
    [SerializeField] private Script_LevelBehavior_49 CatWalk2Behavior;
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
    [SerializeField] private CanvasGroup EndingsCanvasGroup;
    [SerializeField] private CanvasGroup KingsIntroCanvasGroup;
    [SerializeField] private CanvasGroup ElleniaStabCanvasGroup;
    [SerializeField] private CanvasGroup AwakeningPortraitsCanvasGroup;
    [SerializeField] private CanvasGroup ScarletCipherLastOnesCanvasGroup;
    [SerializeField] private CanvasGroup ElevatorCanvasGroup;

    // ----------------------------------------------------------------------
    // Dev Canvases
    [SerializeField] private Script_CanvasGroupController saveDevCanvas;
    [SerializeField] private TextMeshProUGUI saveDevCanvasText;

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

    void Update()
    {
        if (Const_Dev.IsDevHelperOn || Debug.isDebugBuild)
		    HandleDevInput();

        if (Const_Dev.IsTrailerMode)
			HandleTrailerDevInput();
    }

    private void HandleDevInput()
	{
		if (Input.GetKey(KeyCode.V) && Input.GetKeyDown(KeyCode.Alpha1))
            TestCaseKey1();
        else if (Input.GetKey(KeyCode.V) && Input.GetKeyDown(KeyCode.Alpha2))
            TestCaseKey2();
        else if (Input.GetKey(KeyCode.V) && Input.GetKeyDown(KeyCode.Alpha3))
            TestCaseKey3();
        else if (Input.GetKey(KeyCode.V) && Input.GetKeyDown(KeyCode.Alpha4))
            TestCaseKey4();
        else if (Input.GetKey(KeyCode.V) && Input.GetKeyDown(KeyCode.Alpha5))
            TestCaseKey5();
        else if (Input.GetKey(KeyCode.V) && Input.GetKeyDown(KeyCode.F))
            Act2NewState();
        else if (Input.GetKey(KeyCode.V) && Input.GetKeyDown(KeyCode.G))
            GoodEndingNewState();
        else if (Input.GetKey(KeyCode.V) && Input.GetKeyDown(KeyCode.H))
            TrueEndingNewState();
        else if (Input.GetKey(KeyCode.V) && Input.GetKeyDown(KeyCode.J))
            RollCredits();
        else if (Input.GetKey(KeyCode.V) && Input.GetKeyDown(KeyCode.B))
		{
			if (Script_SaveGameControl.control != null)
			{
				Script_SaveGameControl.control.Save();
				ShowSaveDevCanvas();
			}
		}
		else if (Input.GetKey(KeyCode.V) && Input.GetKeyDown(KeyCode.N))
		{
			SaveRestartOnLevel();
		}
        else if (Input.GetKey(KeyCode.V) && Input.GetKeyDown(KeyCode.L))
            BeforeGrandMirror();
        else if (Input.GetKey(KeyCode.Comma) && Input.GetKeyDown(KeyCode.E))
        {
            EileensRoomBehavior.SetNewElleniaPassword();
            ShowSaveDevCanvas($"SETUP TRUE ENDING: ELLENIA ({Script_Names.ElleniaPassword})");
            SetQuestsDoneExplicit(0);
        }
        else if (Input.GetKey(KeyCode.Comma) && Input.GetKeyDown(KeyCode.I))
        {
            ShowSaveDevCanvas($"SETUP TRUE ENDING: IDS");
            SetQuestsDoneExplicit(1);
        }
        else if (Input.GetKey(KeyCode.Comma) && Input.GetKeyDown(KeyCode.L))
        {
            ShowSaveDevCanvas($"SETUP TRUE ENDING: EILEEN");
            SetQuestsDoneExplicit(2);
        }
        else if (Input.GetKey(KeyCode.Comma) && Input.GetKeyDown(KeyCode.W))
        {
            ShowSaveDevCanvas($"SETUP TRUE ENDING: Wells");
            SetQuestsDoneExplicit(3);
        }
        else if (Input.GetKey(KeyCode.Comma) && Input.GetKeyDown(KeyCode.C))
        {
            ShowSaveDevCanvas($"SETUP TRUE ENDING: Cel Gardens");
            SetQuestsDoneExplicit(4);
        }
        else if (Input.GetKey(KeyCode.Comma) && Input.GetKeyDown(KeyCode.X))
        {
            ShowSaveDevCanvas($"SETUP TRUE ENDING: XXX");
            SetQuestsDoneExplicit(5);
        }
        else if (Input.GetKey(KeyCode.V) && Input.GetKeyDown(KeyCode.X))
            XXXWorldNewState();
        else if (Input.GetKey(KeyCode.V) && Input.GetKeyDown(KeyCode.D))
        {
            ShowSaveDevCanvas($"SET DDR MAX MISTAKES ALLOWED");
            IdsRoomBehavior.IsForceSetMistakesAllowed = true;
        }
        else if (Input.GetKey(KeyCode.V) && Input.GetKeyDown(KeyCode.S))
        {
            screenFXManager.ShakeScreen();
        }
        else if (Input.GetKey(KeyCode.Period) && Input.GetKey(KeyCode.O))
        {
            // Full screen for Macs needs to get the exact 16:9 rect
            Script_GraphicsManager gManager = Script_GraphicsManager.Control;
            Script_Utils.SetFullScreenOnMac(Script_GraphicsManager.TargetAspectStatic);
            
            string msg = $"{Screen.width}x{Screen.height} {Screen.fullScreenMode}";
            ShowSaveDevCanvas(msg);
            Debug.Log(msg);
        }
        else if (Input.GetKey(KeyCode.Period) && Input.GetKey(KeyCode.W))
        {
            // Full screen for Windows can just use displayInfo
            Script_Utils.SetFullScreenOnWindows();
            
            DisplayInfo displayInfo = Screen.mainWindowDisplayInfo;
            string msg = $"{displayInfo.width}x{displayInfo.height} {Screen.fullScreenMode}";
            ShowSaveDevCanvas(msg);
            Debug.Log(msg);
        }
        // Localization
        else if (Input.GetKey(KeyCode.RightShift) && Input.GetKey(KeyCode.Alpha0))
        {
            Script_Game.ChangeLangToEN();
            string msg = $"LANG -> {Script_Game.Lang}";
            ShowSaveDevCanvas(msg);
        }
        else if (Input.GetKey(KeyCode.RightShift) && Input.GetKey(KeyCode.Alpha1))
        {
            Script_Game.ChangeLangToCN();
            string msg = $"LANG -> {Script_Game.Lang}";
            ShowSaveDevCanvas(msg);
        }
        else if (Input.GetKey(KeyCode.RightShift) && Input.GetKey(KeyCode.Alpha2))
        {
            Script_Game.ChangeLangToJP();
            string msg = $"LANG -> {Script_Game.Lang}";
            ShowSaveDevCanvas(msg);
        }

        //  Replace these with necessary milestone cases
        void TestCaseKey1() => TeleportGrandMirrorR2();
        void TestCaseKey2() => PlayMaskRevealTimeline();
        void TestCaseKey3() => PlayGoodEnding();
        void TestCaseKey4() => FastForwardGoodEnding();
        void TestCaseKey5() => TeleportBallroomDidGoodEnding();
	}

    private void SaveRestartOnLevel()
    {
        Script_Game game = Script_Game.Game;

        game.CleanRun();
                
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

    private void HandleTrailerDevInput()
	{
		if (Input.GetButtonDown(Const_KeyCodes.PlayerVisibility))
		{
			if (Script_Game.Game != null)
			{
				var isInvisible = Script_Game.Game.GetPlayer().isInvisible;
				Script_Game.Game.GetPlayer().SetInvisible(!isInvisible);
			}			
		}

        if (Input.GetButtonDown(Const_KeyCodes.UIVisibility))
		{
			if (Script_Game.Game != null)
				Script_Game.Game.IsHideHUD = !Script_Game.Game.IsHideHUD;
		}
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

    public void ExitToGrandMirrorFrontOfMirrorR2()
    {
        Script_Game.Game.grandMirrorRoomBehavior.IsFinalRound = true;
        Teleport(GrandMirrorFrontOfMirror);
    }

    public void HandleGrandMirrorR2Mask()
    {
        var game = Script_Game.Game;
        int slot;
        
        game.IsDisableMasksOnly = false;
        game.IsHideHUD = false;
        var hasMyMask = game.GetItemsStickerItem(Const_Items.MyMaskId, out slot) != null;
        game.EquipMaskBackground(Const_Items.MyMaskId, !hasMyMask);
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
    
    public void AddLogsDisableSteamworks()
    {
        PlayerSettings.SetScriptingDefineSymbols(
            UnityEditor.Build.NamedBuildTarget.Standalone,
            ScriptingDefineSymbolsDevDisableSteamworks
        );
    }

    public void RemoveLogsDisableSteamworks()
    {
        PlayerSettings.SetScriptingDefineSymbols(
            UnityEditor.Build.NamedBuildTarget.Standalone,
            ScriptingDefineSymbolsProdDisableSteamworks
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

        // Check Target Aspect Ratios are in sync
        bool isTargetAspectRatiosSynced = true;
        if (graphicsManager.TargetAspect != Script_GraphicsManager.TargetAspectStatic)
            isTargetAspectRatiosSynced = false;

        if (!isAllWellWorldActive)
            Debug.LogWarning($"<color=orange>NOT ALL WELLS WORLD TILES ARE ACTIVE</color>");
        
        if (!isAllCelGardensWorldActive)
            Debug.LogWarning($"<color=orange>NOT ALL CEL GARDENS WORLD TILES ARE ACTIVE</color>");
        
        if (!isAllXXXWorldActive)
            Debug.LogWarning($"<color=orange>NOT ALL XXX WORLD TILES ARE ACTIVE</color>");
        
        if (!isTargetAspectRatiosSynced)
            Debug.LogWarning($"<color=orange>TARGET ASPECT RATIOS DO NOT MATCH {graphicsManager.TargetAspect} vs. {Script_GraphicsManager.TargetAspectStatic}</color>");

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

            if (EndingsCanvasGroup.gameObject.activeInHierarchy)
            {
                Debug.Log($"<color=red>{EndingsCanvasGroup.name} should be INACTIVE</color>");
                EndingsCanvasGroup.gameObject.SetActive(false);
            }

            if (!KingsIntroCanvasGroup.gameObject.activeInHierarchy)
            {
                Debug.Log($"<color=red>{KingsIntroCanvasGroup.name} should be ACTIVE</color>");
                KingsIntroCanvasGroup.gameObject.SetActive(true);
            }

            if (ElleniaStabCanvasGroup.gameObject.activeInHierarchy)
            {
                Debug.Log($"<color=red>{ElleniaStabCanvasGroup.name} should be INACTIVE</color>");
                ElleniaStabCanvasGroup.gameObject.SetActive(false);
            }

            if (AwakeningPortraitsCanvasGroup.gameObject.activeInHierarchy)
            {
                Debug.Log($"<color=red>{AwakeningPortraitsCanvasGroup.name} should be INACTIVE</color>");
                AwakeningPortraitsCanvasGroup.gameObject.SetActive(false);
            }

            if (ScarletCipherLastOnesCanvasGroup.gameObject.activeInHierarchy)
            {
                Debug.Log($"<color=red>{ScarletCipherLastOnesCanvasGroup.name} should be INACTIVE</color>");
                ScarletCipherLastOnesCanvasGroup.gameObject.SetActive(false);
            }

            if (ElevatorCanvasGroup.gameObject.activeInHierarchy)
            {
                Debug.Log($"<color=red>{ElevatorCanvasGroup.name} should be INACTIVE</color>");
                ElevatorCanvasGroup.gameObject.SetActive(false);
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

    public void SetQuestsDoneExplicit(int questIdx)
    {
        Script_Game.Game.ElleniasRoomBehavior.isCurrentPuzzleComplete       = questIdx != 0;
        Script_Game.Game.IdsRoomBehavior.isCurrentPuzzleComplete            = questIdx != 1;
        Script_Game.Game.EileensMindBehavior.isCurrentPuzzleComplete        = questIdx != 2;
        Script_Game.Game.WellsWorldBehavior.isCurrentMooseQuestComplete     = questIdx != 3;
        Script_Game.Game.GardenLabyrinthBehavior.isCurrentPuzzleComplete    = questIdx != 4;
        Script_Game.Game.KTVRoom2Behavior.IsCurrentPuzzleComplete           = questIdx != 5;
    }

    public void SetWellsWorldSolved()
    {
        var game = Script_Game.Game;
        game.WellsWorldBehavior.isCurrentMooseQuestComplete = true;
        game.WellsWorldBehavior.isMooseQuestDone = true;
        
        game.faceOffCounter++;
        game.WellsWorldBehavior.didPlayFaceOff = true;
    }

    public void SetGardenLabyrinthSolved()
    {
        var game = Script_Game.Game;
        game.GardenLabyrinthBehavior.isCurrentPuzzleComplete = true;
        game.GardenLabyrinthBehavior.isPuzzleComplete = true;
        
        game.faceOffCounter++;
        game.GardenLabyrinthBehavior.didPlayFaceOff = true;
    }

    public void SetKTV2Solved()
    {
        var game = Script_Game.Game;
        game.KTVRoom2Behavior.IsCurrentPuzzleComplete = true;
        game.KTVRoom2Behavior.IsPuzzleComplete = true;
        
        game.faceOffCounter++;
        game.KTVRoom2Behavior.didPlayFaceOff = true;
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
        
        // Set Level states
        ToGrandMirrorState();

        // Set Scarlet Cipher visibility
        Act1ScarletCipherState();

        // Set Piano states
        Act1PianoStates();

        // Update names
        Script_Names.UpdateR1Names();

        MynesGrandMirrorRoomBehavior.IsDone = true;

        Script_Game.Game.StartWeekendCycleSaveInitialize();
    }

    public void Act2NewState()
    {
        Script_TransitionManager.Control.TimelineBlackScreen();
        ShowSaveDevCanvas($"{name} GO TO: ACT2 START");

        inventoryTester.WeekendCycle();
        ToGrandMirrorState();
        Act1ScarletCipherState();
        Act1PianoStates();
        Script_Names.UpdateR1Names();
        MynesGrandMirrorRoomBehavior.IsDone = true;

        Script_Game.Game.StartWeekendCycleSaveInitialize();
    }

    public void GoodEndingNewState()
    {
        Script_TransitionManager.Control.TimelineBlackScreen();
        ShowSaveDevCanvas($"{name} GO TO: CCTV ENDING");

        inventoryTester.WeekendCycle();
        ToGrandMirrorState();
        Act1ScarletCipherState();
        Act1PianoStates();
        Script_Names.UpdateR1Names();
        MynesGrandMirrorRoomBehavior.IsDone = true;

        AfterGrandMirrorToGoodEndingState();
        Act2ScarletCipherState();
        Act2PianoStates();
        Act2Items();

        // Set most likely finish run (4th day weekend)
        runsManager.StartWeekendCycle();
        runsManager.IncrementRun();
        runsManager.IncrementRun();
        
        Script_Game.Game.NextRunSaveInitialize();
    }

    public void TrueEndingNewState()
    {
        Script_TransitionManager.Control.TimelineBlackScreen();
        ShowSaveDevCanvas($"{name} GO TO: TRUE ENDING");
        
        inventoryTester.WeekendCycle();
        ToGrandMirrorState();
        Act1ScarletCipherState();
        Act1PianoStates();
        Script_Names.UpdateR1Names();
        MynesGrandMirrorRoomBehavior.IsDone = true;

        AfterGrandMirrorToGoodEndingState();
        Act2ScarletCipherState();
        Act2PianoStates();
        Act2Items();
        inventoryTester.AddMyMask();

        // Set most likely finish run (4th day weekend)
        runsManager.StartWeekendCycle();
        runsManager.IncrementRun();
        runsManager.IncrementRun();

        Script_Game.Game.StartSundayCycleSaveInitialize();
    }

    public void XXXWorldNewState()
    {
        Script_TransitionManager.Control.TimelineBlackScreen();
        ShowSaveDevCanvas($"{name} GO TO: XXX WORLD START");        
        
        inventoryTester.WeekendCycle();
        inventoryTester.AddLetThereBeLight();
        ToXXXWorldStartAfterAct2State();
        
        Act1ScarletCipherState();
        var scarletCipherManager = Script_ScarletCipherManager.Control;
        scarletCipherManager.RevealScarletCipherSlot(3);
        scarletCipherManager.RevealScarletCipherSlot(4);
        scarletCipherManager.RevealScarletCipherSlot(5);
        scarletCipherManager.RevealScarletCipherSlot(6);
        scarletCipherManager.RevealScarletCipherSlot(7);

        Act1PianoStates();
        Script_PianoManager.Control.Pianos[3].IsRemembered = true;
        
        Script_Names.UpdateR1Names();
        MynesGrandMirrorRoomBehavior.IsDone = true;

        // Set runs manager to weekend, cyclecount 0
        runsManager.StartWeekendCycle();
        runsManager.IncrementRun();
        
        Script_Game.Game.faceOffCounter = 2;

        Script_Game.Game.NextRunSaveInitialize();
    }

    public void RollCredits()
    {
        ShowSaveDevCanvas($"{name} GO TO: CREDITS");
        Script_TransitionManager.Control.RollCredits();
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
        hotelLobbyBehavior.didStartThought                          = true;
        hotelLobbyBehavior.didCantSwimDialogue                      = true;
        
        woodsBehavior.didStartThought                               = true;
        
        BallroomBehavior.isKingIntroCutSceneDone                    = true;

        hallwayWithSecretBehavior.didPickUpMelancholyPianoSticker   = true;

        IdsRoomBehavior.gotBoarNeedle                               = true;

        EileensRoomBehavior.spokenWithEileen                        = true;
        EileensRoomBehavior.didOnEntranceAttack                     = true;

        ElleniasRoomBehavior.isPuzzleComplete                       = true;
        ElleniasRoomBehavior.spokenWithEllenia                      = true;

        EileensMindBehavior.isPuzzleComplete                        = true;
        EileensMindBehavior.didActivateDramaticThoughts             = true;
        EileensMindBehavior.gotIceSpikeSticker                      = true;

        LastElevatorBehavior.GotPsychicDuck                         = true;
    }

    private void AfterGrandMirrorToGoodEndingState()
    {
        woodsBehavior.didStartThoughtSea = true;
        
        hotelLobbyBehavior.didCantSwimDialogue = true;
        hotelLobbyBehavior.didOpeningThoughtFaceOff0 = true;
        hotelLobbyBehavior.didOpeningThoughtFaceOff1 = true;
        hotelLobbyBehavior.didOpeningThoughtCodeRemains0 = false;
        hotelLobbyBehavior.didOpeningThoughtCodeRemains1 = true;
        
        MynesGrandMirrorRoomBehavior.IsDone = true;

        WellsWorldBehavior.didPickUpLastWellMap = true;
        WellsWorldBehavior.didPickUpSpeedSeal = true;
        WellsWorldBehavior.isMooseQuestDone = true;
        WellsWorldBehavior.didPlayFaceOff = true;
        WellsWorldBehavior.didSpecialIntro = true;
        WellsWorldBehavior.didWellTalkInitialDialogue = true;
        CatWalk1Behavior.didPickUpLightSticker = true;

        CelGardensBehavior.didIntro = true;
        RockGardenBehavior.didPickUpPuppeteerSticker = true;
        CatWalk2Behavior.didActivateDoubts = true;
        LabyrinthBehavior.isPuzzleComplete = true;
        LabyrinthBehavior.didPlayFaceOff = true;

        XXXWorldBehavior.didIntro = true;
        XXXWorldBehavior.didDontKnowMeThought = true;
        // SaloonBehavior.isUrsieCutsceneDone = true;
        KTV2Behavior.IsPuzzleComplete = true;
        KTV2Behavior.didPlayFaceOff = true;
    }

    private void ToXXXWorldStartAfterAct2State()
    {
        woodsBehavior.didStartThoughtSea = true;
        
        hotelLobbyBehavior.didCantSwimDialogue = true;
        hotelLobbyBehavior.didOpeningThoughtFaceOff0 = true;
        hotelLobbyBehavior.didOpeningThoughtFaceOff1 = false;
        hotelLobbyBehavior.didOpeningThoughtCodeRemains0 = false;
        hotelLobbyBehavior.didOpeningThoughtCodeRemains1 = false;
        
        MynesGrandMirrorRoomBehavior.IsDone = true;

        WellsWorldBehavior.didPickUpLastWellMap = true;
        WellsWorldBehavior.didPickUpSpeedSeal = true;
        WellsWorldBehavior.isMooseQuestDone = true;
        WellsWorldBehavior.didPlayFaceOff = true;
        WellsWorldBehavior.didSpecialIntro = true;
        WellsWorldBehavior.didWellTalkInitialDialogue = true;
        CatWalk1Behavior.didPickUpLightSticker = true;

        CelGardensBehavior.didIntro = true;
        RockGardenBehavior.didPickUpPuppeteerSticker = true;
        CatWalk2Behavior.didActivateDoubts = true;
        LabyrinthBehavior.isPuzzleComplete = true;
        LabyrinthBehavior.didPlayFaceOff = true;
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

    private void Act1ScarletCipherState()
    {
        var scarletCipherManager = Script_ScarletCipherManager.Control;
        scarletCipherManager.RevealScarletCipherSlot(0);
        scarletCipherManager.RevealScarletCipherSlot(1);
        scarletCipherManager.RevealScarletCipherSlot(2);
    }

    private void Act2ScarletCipherState()
    {
        var scarletCipherManager = Script_ScarletCipherManager.Control;
        scarletCipherManager.RevealScarletCipherSlot(3);
        scarletCipherManager.RevealScarletCipherSlot(4);
        scarletCipherManager.RevealScarletCipherSlot(5);
        scarletCipherManager.RevealScarletCipherSlot(6);
        scarletCipherManager.RevealScarletCipherSlot(7);
        scarletCipherManager.RevealScarletCipherSlot(8);
        scarletCipherManager.RevealScarletCipherSlot(9);
    }

    private void Act1PianoStates()
    {
        Script_PianoManager.Control.Pianos[0].IsRemembered = true;
        Script_PianoManager.Control.Pianos[1].IsRemembered = true;
        Script_PianoManager.Control.Pianos[2].IsRemembered = true;
    }

    private void Act2PianoStates()
    {
        Script_PianoManager.Control.Pianos[3].IsRemembered = true;
        Script_PianoManager.Control.Pianos[4].IsRemembered = true;
    }

    private void Act2Items()
    {
        var inventory = Dev_InventoryTester.Control;
        inventory.AddLetThereBeLight();
        inventory.AddPuppeteer();

        inventory.AddLastWellMap();
        inventory.AddSpeedSeal();
    }

    // ----------------------------------------------------------------------
    // Freedom Milestone States
    // These should not save state.
    
    /// <summary>
    /// Milestone AUGUST
    /// </summary>
    public void PlayGoodEnding() => hotelLobbyBehavior.DevPlayGoodEnding();

    public void FastForwardGoodEnding() => hotelLobbyBehavior.DevFastForwardGoodEndingTheEnd();

    // Must fast forward to CCTV Ending first before calling
    public void TeleportBallroomDidGoodEnding()
    {
        Script_Game.Game.didGoodEnding = true;
        ExitToBallroomFromHMSHall();
    }

    /// <summary>
    /// Milestone MAY
    /// </summary>
    public void TeleportGrandMirrorR2()
    {
        ExitToGrandMirrorFrontOfMirrorR2();
        HandleGrandMirrorR2Mask();
    }

    public void PlayMaskRevealTimeline()
    {
        MynesGrandMirrorRoomBehavior.DevForcePlayMaskRevealTimeline();
    }
    
    /// <summary>
    /// Milestone FEBRUARY
    /// </summary>
    public void Day2SaveAndRestart(int testCaseIdx)
    {
        var game = Script_Game.Game;
        
        game.ChangeStateCutScene();

        // Put up canvas
        ShowSaveDevCanvas(@$"TEST CASE {testCaseIdx}");

        inventoryTester.Day2();
        woodsBehavior.didStartThought = true;
        
        game.NextRunSaveInitialize();
    }
    
    /// <summary>
    /// Milestone JANUARY
    /// </summary>
    public void Act1EndingFrontOfMirror()
    {
        inventoryTester.GrandMirror();
        ToGrandMirrorState();
        Act1ScarletCipherState();
        Act1PianoStates();
        Script_Names.UpdateR1Names();

        Teleport(GrandMirrorFrontOfMirror);
    }

    public void Act2SeaVignette()
    {
        inventoryTester.WeekendCycle();
        ToGrandMirrorState();
        Act1ScarletCipherState();
        Act1PianoStates();
        Script_Names.UpdateR1Names();
        MynesGrandMirrorRoomBehavior.IsDone = true;

        // Set runs manager to weekend, cyclecount 0
        runsManager.StartWeekendCycle();
    }

    public void SpecialIntroWellsWorld()
    {
        inventoryTester.WeekendCycle();
        ToGrandMirrorState();
        Act1ScarletCipherState();
        Act1PianoStates();
        Script_Names.UpdateR1Names();
        MynesGrandMirrorRoomBehavior.IsDone = true;

        // Set runs manager to weekend, cyclecount 0
        runsManager.StartWeekendCycle();
        
        Teleport(BallroomEntranceFromLastElevator);
    }

    public void SpecialIntroCelestialGardens()
    {
        inventoryTester.WeekendCycle();
        ToGrandMirrorState();
        Act1ScarletCipherState();
        Act1PianoStates();
        Script_Names.UpdateR1Names();
        MynesGrandMirrorRoomBehavior.IsDone = true;

        // Set runs manager to weekend, cyclecount 0
        runsManager.StartWeekendCycle();
        Script_Game.Game.faceOffCounter = 1;
        
        Teleport(BallroomEntranceFromLastElevator);
    }

    public void SpecialIntroXXXWorld()
    {
        inventoryTester.WeekendCycle();
        ToGrandMirrorState();
        Act1ScarletCipherState();
        Act1PianoStates();
        Script_Names.UpdateR1Names();
        MynesGrandMirrorRoomBehavior.IsDone = true;

        // Set runs manager to weekend, cyclecount 0
        runsManager.StartWeekendCycle();
        Script_Game.Game.faceOffCounter = 2;

        Teleport(BallroomEntranceFromLastElevator);
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

    public void ShowSaveDevCanvas(string msg = "SAVED")
    {
        saveDevCanvasText.text = msg;
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
        private static bool showLocalizationTests;

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

                if (GUILayout.Button("Go To: Grand Mirror R2 (Front of Mirror)"))
                {
                    t.ExitToGrandMirrorFrontOfMirrorR2();
                }

                if (GUILayout.Button("Equip MyMask BG R2"))
                {
                    t.HandleGrandMirrorR2Mask();
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

                if (GUILayout.Button("All Quests Done Today Exc Ellenia"))
                    t.SetQuestsDoneExplicit(0);

                if (GUILayout.Button("All Quests Done Today Exc Ids"))
                    t.SetQuestsDoneExplicit(1);
                
                if (GUILayout.Button("All Quests Done Today Exc Eileen"))
                    t.SetQuestsDoneExplicit(2);
                
                if (GUILayout.Button("All Quests Done Today Exc Wells World"))
                    t.SetQuestsDoneExplicit(3);
                
                if (GUILayout.Button("All Quests Done Today Exc Cel Gardens"))
                    t.SetQuestsDoneExplicit(4);
                
                if (GUILayout.Button("All Quests Done Today Exc XXX World"))
                    t.SetQuestsDoneExplicit(5);

                if (GUILayout.Button("Solve Wells World & Handle FaceOff"))
                {
                    t.SetWellsWorldSolved();
                }

                if (GUILayout.Button("Solve Labyrinth & Handle FaceOff"))
                {
                    t.SetGardenLabyrinthSolved();
                }

                if (GUILayout.Button("Solve KTV2 & Handle FaceOff"))
                {
                    t.SetKTV2Solved();
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

                EditorGUILayout.LabelField("Steam", EditorStyles.miniLabel);

                if (GUILayout.Button("[DEV] Add ENABLE_LOGS, Remove FORCE_DISABLE_STEAMWORKS", GUILayout.Height(32)))
                {
                    t.AddLogs();

                    if (GUI.changed)
                    {
                        EditorUtility.SetDirty(t);
                        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(t.gameObject.scene);
                    }
                }

                if (GUILayout.Button("[PROD] Remove ENABLE_LOGS, Remove FORCE_DISABLE_STEAMWORKS", GUILayout.Height(56)))
                {
                    t.RemoveLogs();

                    if (GUI.changed)
                    {
                        EditorUtility.SetDirty(t);
                        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(t.gameObject.scene);
                    }
                }

                EditorGUILayout.LabelField("GOG & Epic", EditorStyles.miniLabel);

                if (GUILayout.Button("[DEV] Add ENABLE_LOGS, Add FORCE_DISABLE_STEAMWORKS", GUILayout.Height(32)))
                {
                    t.AddLogsDisableSteamworks();

                    if (GUI.changed)
                    {
                        EditorUtility.SetDirty(t);
                        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(t.gameObject.scene);
                    }
                }

                if (GUILayout.Button("[PROD] Remove ENABLE_LOGS, Add FORCE_DISABLE_STEAMWORKS", GUILayout.Height(56)))
                {
                    t.RemoveLogsDisableSteamworks();

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

            showLocalizationTests = EditorGUILayout.Foldout(showLocalizationTests, "Localization", style);
            if (showLocalizationTests)
            {
                if (GUILayout.Button("Change Language to EN"))
                {
                    Script_Game.ChangeLangToEN();
                }

                if (GUILayout.Button("Change Language to CN"))
                {
                    Script_Game.ChangeLangToCN();
                }

                if (GUILayout.Button("Change Language to JP"))
                {
                    Script_Game.ChangeLangToJP();
                }
                
                if (GUILayout.Button("Build Item Strings - EN"))
                {
                    Script_ItemStringBuilder.BuildParams(Const_Languages.EN);
                }

                if (GUILayout.Button("Build Item Strings - CN"))
                {
                    Script_ItemStringBuilder.BuildParams(Const_Languages.CN);
                }
            }
        }
    }
    #endif
}
