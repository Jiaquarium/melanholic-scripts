using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_44 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_44 LB44;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_44 lvlModel = new Model_LevelBehavior_44(
            _didIntro : LB44.didIntro,
            _didDontKnowMeThought : LB44.didDontKnowMeThought
        );
        
        data.levelsData.LB44 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        Model_LevelBehavior_44 lvlModel = data.levelsData.LB44;
        
        LB44.didIntro = lvlModel.didIntro;
        LB44.didDontKnowMeThought = lvlModel.didDontKnowMeThought;

        Dev_Logger.Debug($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(lvlModel);
    }
}
