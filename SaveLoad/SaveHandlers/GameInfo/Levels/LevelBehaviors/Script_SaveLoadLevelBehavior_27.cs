using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_27 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_27 LB27;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_27 lvlModel = new Model_LevelBehavior_27(
            _gotPsychicDuck               : LB27.GotPsychicDuck
        );
        
        data.levelsData.LB27 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        Model_LevelBehavior_27 lvlModel     = data.levelsData.LB27;

        LB27.GotPsychicDuck                 = lvlModel.gotPsychicDuck;

        Dev_Logger.Debug($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(lvlModel);
    }
}
