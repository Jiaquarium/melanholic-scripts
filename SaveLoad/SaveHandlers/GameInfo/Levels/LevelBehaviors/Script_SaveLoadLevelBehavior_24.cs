using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_24 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_24 LB24;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_24 lvlModel = new Model_LevelBehavior_24(
            LB24.IsPuzzleComplete,
            LB24.didPickUpSpringStone,
            LB24.didPlayFaceOff
        );
        
        data.levelsData.LB24 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        if (data.levelsData == null)
        {
            Dev_Logger.Debug("There is no levels state data to load.");
            return;
        }

        if (data.levelsData.LB24 == null)
        {
            Dev_Logger.Debug("There is no LB24 state data to load.");
            return;
        }

        Model_LevelBehavior_24 lvlModel     = data.levelsData.LB24;
        LB24.IsPuzzleComplete               = lvlModel.isPuzzleComplete;
        LB24.didPickUpSpringStone           = lvlModel.didPickUpSpringStone;
        LB24.didPlayFaceOff                 = lvlModel.didPlayFaceOff;

        Dev_Logger.Debug($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(lvlModel);
    }
}
