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
        Script_Game.Game.Exit(
            playerTeleportPos.data.level,
            playerTeleportPos.data.playerSpawn,
            playerTeleportPos.data.facingDirection,
            isExit: true,
            isSilent: false,
            exitType: Script_Exits.ExitType.Default
        );
    }

    public void BuildSetup()
    {
        Script_Game.LevelsInactivate();
    }

    public void SetAllQuestsDoneToday()
    {
        Script_Game.Game.IdsRoomBehavior.isCurrentPuzzleComplete            = true;
        Script_Game.Game.KTVRoom2Behavior.IsCurrentPuzzleComplete           = true;
        Script_Game.Game.ElleniasRoomBehavior.isCurrentPuzzleComplete       = true;
        Script_Game.Game.EileensMindBehavior.isCurrentPuzzleComplete        = true;
        Script_Game.Game.WellsWorldBehavior.isCurrentMooseQuestComplete     = true;
        Script_Game.Game.GardenLabyrinthBehavior.isCurrentPuzzleComplete    = true;
    }

    public void SolveAllMynesMirrors()
    {
        Script_ScarletCipherManager.Control.Dev_ForceSolveAllMirrors();
    }
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
        
        if (GUILayout.Button("ExitToLevel()"))
        {
            t.ExitToLevel();
        }

        if (GUILayout.Button("All Quests Done Today"))
        {
            t.SetAllQuestsDoneToday();
        }

        if (GUILayout.Button("Solve All Mirrors"))
        {
            t.SolveAllMynesMirrors();
        }

        GUILayout.Space(12);

        if (GUILayout.Button("Build Setup"))
        {
            t.BuildSetup();
        }
    }
}
#endif