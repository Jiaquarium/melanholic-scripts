using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_43 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_43 LB43;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_43 lvlModel = new Model_LevelBehavior_43(
            _didIntro : LB43.didIntro
        );
        
        data.levelsData.LB43 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        Model_LevelBehavior_43 lvlModel = data.levelsData.LB43;
        LB43.didIntro = lvlModel.didIntro;

        Dev_Logger.Debug($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(lvlModel);
    }
}
