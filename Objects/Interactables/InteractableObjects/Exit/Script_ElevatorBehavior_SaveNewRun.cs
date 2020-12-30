using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ElevatorBehavior_SaveNewRun : Script_ElevatorBehavior
{
    [SerializeField] private Script_ExitMetadataObject playerSpawn;
    
    public override void Effect()
    {
        Model_PlayerState playerSavePos = new Model_PlayerState(
            (int)playerSpawn.data.playerSpawn.x,
            (int)playerSpawn.data.playerSpawn.y,
            (int)playerSpawn.data.playerSpawn.z,
            playerSpawn.data.facingDirection
        );
        
        Script_Game.Game.NextRunSaveInitialize(playerSavePos);
    }
}
