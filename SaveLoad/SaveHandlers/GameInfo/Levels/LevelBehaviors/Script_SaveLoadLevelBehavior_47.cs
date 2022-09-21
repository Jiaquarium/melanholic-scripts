using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_47 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_47 LB47;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_47 lvlModel = new Model_LevelBehavior_47(
            _didPickUpPuppeteerSticker               : LB47.didPickUpPuppeteerSticker
        );
        
        data.levelsData.LB47 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        Model_LevelBehavior_47 lvlModel         = data.levelsData.LB47;
        
        LB47.didPickUpPuppeteerSticker                   = lvlModel.didPickUpPuppeteerSticker;

        Dev_Logger.Debug($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(lvlModel);
    }
}
