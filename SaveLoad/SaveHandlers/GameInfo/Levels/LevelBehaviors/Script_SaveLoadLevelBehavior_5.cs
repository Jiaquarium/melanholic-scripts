using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_5 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_5 LB5;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_5 lvlModel = new Model_LevelBehavior_5(
            LB5.didOnEntranceDialogue
        );
        
        data.levelsData.LB5 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        Model_LevelBehavior_5 lvlModel = data.levelsData.LB5;
        LB5.didOnEntranceDialogue = lvlModel.didOnEntranceDialogue;

        Dev_Logger.Debug($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(lvlModel);
    }
}
