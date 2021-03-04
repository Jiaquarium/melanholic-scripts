using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_LevelBehavior_46 : Script_LevelBehavior
{
    // ==================================================================
    // State Data
    
    // ==================================================================

    // Needed to initialize "Player" copy.
    [SerializeField] private Script_LevelGrid levelGrid;
    
    [SerializeField] private Script_Player puppetMaster;
    [SerializeField] private Script_Marker puppetMasterSpawn;

    public override void Setup()
    {
        Model_PlayerState puppetMasterStartState = new Model_PlayerState(
            (int)puppetMasterSpawn.transform.position.x,
            (int)puppetMasterSpawn.transform.position.y,
            (int)puppetMasterSpawn.transform.position.z,
            puppetMasterSpawn.Direction
        );
        puppetMaster.Setup(
            puppetMaster.FacingDirection,
            puppetMasterStartState,
            false
        );
        puppetMaster.InitializeOnLevel(
            puppetMasterStartState,
            false,
            levelGrid.transform
        );
    }
}