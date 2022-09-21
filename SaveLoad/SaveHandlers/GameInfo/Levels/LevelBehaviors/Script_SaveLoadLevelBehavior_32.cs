using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_32 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_32 LB32;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_32 lvlModel = new Model_LevelBehavior_32(
            _didStartThought           : LB32.didStartThought
        );
        
        data.levelsData.LB32 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        if (data.levelsData == null)
        {
            Dev_Logger.Debug("There is no levels state data to load.");
            return;
        }

        if (data.levelsData.LB32 == null)
        {
            Dev_Logger.Debug("There is no LB32 state data to load.");
            return;
        }

        Model_LevelBehavior_32 lvlModel     = data.levelsData.LB32;
        LB32.didStartThought                = lvlModel.didStartThought;

        Dev_Logger.Debug($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(lvlModel);
    }
}
