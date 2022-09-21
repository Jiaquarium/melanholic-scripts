using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_22 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_22 LB22;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_22 lvlModel = new Model_LevelBehavior_22(
            LB22.isUrsieCutsceneDone
        );
        
        data.levelsData.LB22 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        if (data.levelsData == null)
        {
            Dev_Logger.Debug("There is no levels state data to load.");
            return;
        }

        if (data.levelsData.LB22 == null)
        {
            Dev_Logger.Debug("There is no LB22 state data to load.");
            return;
        }

        Model_LevelBehavior_22 lvlModel = data.levelsData.LB22;
        LB22.isUrsieCutsceneDone        = lvlModel.isUrsieCutsceneDone;

        Dev_Logger.Debug($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(lvlModel);
    }
}
