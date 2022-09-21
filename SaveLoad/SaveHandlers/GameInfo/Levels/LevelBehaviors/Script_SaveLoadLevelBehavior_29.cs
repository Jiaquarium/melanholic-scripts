using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_29 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_29 LB29;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_29 lvlModel = new Model_LevelBehavior_29();
        
        data.levelsData.LB29 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        Model_LevelBehavior_29 lvlModel     = data.levelsData.LB29;

        Dev_Logger.Debug($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(lvlModel);
    }
}
