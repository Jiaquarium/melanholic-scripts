using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_30 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_30 LB30;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_30 lvlModel = new Model_LevelBehavior_30(
            _isDone           : LB30.isDone
        );
        
        data.levelsData.LB30 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        if (data.levelsData == null)
        {
            Dev_Logger.Debug("There is no levels state data to load.");
            return;
        }

        if (data.levelsData.LB30 == null)
        {
            Dev_Logger.Debug("There is no LB30 state data to load.");
            return;
        }

        Model_LevelBehavior_30 lvlModel     = data.levelsData.LB30;
        LB30.isDone                         = lvlModel.isDone;

        Dev_Logger.Debug($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(lvlModel);
    }
}
