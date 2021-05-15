using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Dev_GameHelper : MonoBehaviour
{
    public Vector3Int playerSpawn;
    public int level;
    public Directions facingDirection;
    
    [SerializeField] private Script_ExitMetadataObject playerDefaultSpawn;
    [SerializeField] private Script_ExitMetadataObject playerTeleportPos;
    [SerializeField] private Script_ExitMetadataObject IdsRoomEntrance;
    [SerializeField] private Script_ExitMetadataObject BallroomEntranceFromHMSHall;
    [SerializeField] private Script_ExitMetadataObject LastElevatorEntrance;
    [SerializeField] private Script_ExitMetadataObject WellsWorldEntrance;

    [SerializeField] private Dev_InventoryTester inventoryTester;

    [SerializeField] private Script_RunsManager runsManager;

    // ----------------------------------------------------------------------
    // Weekday Cycle State
    [SerializeField] private Script_LevelBehavior_0 woodsBehavior;
    [SerializeField] private Script_LevelBehavior_4 hallwayWithSecretBehavior;
    [SerializeField] private Script_LevelBehavior_10 IdsRoomBehavior;
    [SerializeField] private Script_LevelBehavior_21 EileensRoomBehavior;
    [SerializeField] private Script_LevelBehavior_25 ElleniasRoomBehavior;
    [SerializeField] private Script_LevelBehavior_27 LastElevatorBehavior;
    [SerializeField] private Script_LevelBehavior_26 EileensMindBehavior;
    [SerializeField] private Script_LevelBehavior_48 MynesGrandMirrorRoomBehavior;

    // ----------------------------------------------------------------------
    // Quests Done Dynamic Settings
    [SerializeField] private bool ElleniaPuzzleDone;
    [SerializeField] private bool IdsPuzzleDone;
    [SerializeField] private bool EileenPuzzleDone;
    [SerializeField] private bool WellsWorldPuzzleDone;
    [SerializeField] private bool CelestialGardensPuzzleDone;
    [SerializeField] private bool XXXWorldPuzzleDone;

    private bool didSetWeekend;

    public void DefaultPlayerSpawnPos()
    {
        playerSpawn = new Vector3Int(
            (int)playerDefaultSpawn.data.playerSpawn.x,
            (int)playerDefaultSpawn.data.playerSpawn.y,
            (int)playerDefaultSpawn.data.playerSpawn.z
        );
        level           = playerDefaultSpawn.data.level;
        facingDirection = playerDefaultSpawn.data.facingDirection;
    }

    public void ExitToLevel()
    {
        Teleport(playerTeleportPos);
    }

    public void ExitToIdsRoom()
    {
        Teleport(IdsRoomEntrance);
    }

    public void ExitToLastElevator()
    {
        Teleport(LastElevatorEntrance);
    }

    public void ExitToBallroomFromHMSHall()
    {
        Teleport(BallroomEntranceFromHMSHall);
    }

    public void ExitToWellsWorld()
    {
        Teleport(WellsWorldEntrance);
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

    // Set current save state to the Weekend Cycle.
    public void WeekendCycle()
    {
        if (didSetWeekend)  return;
        didSetWeekend = true;
        
        // Set Items for Weekend Cycle.
        inventoryTester.WeekendCycle();
        
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

        MynesGrandMirrorRoomBehavior.IsDone                         = true;

        // Set Run and Cycle data to Weekend Thursday.
        Script_Game.Game.NextRunSaveInitialize(false, Script_Run.DayId.thu);
    }

    public void SaveCurrent()
    {
        Script_SaveGameControl.control.Save(Script_SaveGameControl.Saves.Initialize); 
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

    #if UNITY_EDITOR
    [CustomEditor(typeof(Dev_GameHelper))]
    public class Dev_GameHelperTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Dev_GameHelper t = (Dev_GameHelper)target;
            if (GUILayout.Button("DefaultPlayerSpawnPos()"))
            {
                t.DefaultPlayerSpawnPos();
            }
            
            if (GUILayout.Button("Go To:"))
            {
                t.ExitToLevel();
            }

            if (GUILayout.Button("Go To: Ids Room"))
            {
                t.ExitToIdsRoom();
            }

            if (GUILayout.Button("Go To: Last Elevator"))
            {
                t.ExitToLastElevator();
            }

            if (GUILayout.Button("Go To: Ballroom (HMS Hall Entrance)"))
            {
                t.ExitToBallroomFromHMSHall();
            }

            if (GUILayout.Button("Go To: Wells World"))
            {
                t.ExitToWellsWorld();
            }

            GUILayout.Space(12);

            if (GUILayout.Button("All Quests Done Today"))
            {
                t.SetAllQuestsDoneToday();
            }

            if (GUILayout.Button("Quests Done Dynamic Today"))
            {
                t.SetQuestsDoneDynamic();
            }

            GUILayout.Space(12);

            if (GUILayout.Button("Save Current"))
            {
                t.SaveCurrent();
            }

            if (GUILayout.Button("Weekend Start"))
            {
                t.WeekendCycle();
            }

            GUILayout.Space(12);

            if (GUILayout.Button("Build Setup", GUILayout.Height(32)))
            {
                t.BuildSetup();
            }

            if (GUILayout.Button("Build Dev Explore Setup"))
            {
                t.BuildDevExploreSetup();
            }
        }
    }
    #endif
}
