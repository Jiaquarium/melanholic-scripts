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
    
    [SerializeField] private Script_PuppetMaster puppetMaster;
    [SerializeField] private Script_Puppet puppet;
    [SerializeField] private Script_Marker puppetMasterSpawn;
    [SerializeField] private Script_Marker puppetSpawn;

    public override void Setup()
    {
        Model_PlayerState puppetMasterStartState = new Model_PlayerState(
            (int)puppetMasterSpawn.transform.position.x,
            (int)puppetMasterSpawn.transform.position.y,
            (int)puppetMasterSpawn.transform.position.z,
            puppetMasterSpawn.Direction
        );
        Model_PlayerState puppetStartState = new Model_PlayerState(
            (int)puppetSpawn.transform.position.x,
            (int)puppetSpawn.transform.position.y,
            (int)puppetSpawn.transform.position.z,
            puppetSpawn.Direction
        );

        puppetMaster.Setup(puppetMasterStartState.faceDirection, puppetMasterStartState, false);
        puppetMaster.InitializeOnLevel(puppetMasterStartState, false, levelGrid.transform);

        puppet.Setup(puppetStartState.faceDirection, puppetStartState, false);
        puppet.InitializeOnLevel(puppetStartState, false, levelGrid.transform);
    }
}