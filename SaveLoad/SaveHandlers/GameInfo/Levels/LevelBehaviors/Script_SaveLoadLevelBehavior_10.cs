using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_10 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_10 LB10;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_10 lvlModel = new Model_LevelBehavior_10(
            LB10.gotBoarNeedle
        );
        
        data.levelsData.LB10 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        Model_LevelBehavior_10 lvlModel     = data.levelsData.LB10;

        LB10.gotBoarNeedle                  = lvlModel.gotBoarNeedle;

        Dev_Logger.Debug($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(lvlModel);
    }
}
