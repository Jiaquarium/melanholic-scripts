﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_13 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_13 LB13;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_13 lvlModel = new Model_LevelBehavior_13(
            LB13.didPickUpLightSticker
        );
        
        data.levelsData.LB13 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        if (data.levelsData == null)
        {
            Dev_Logger.Debug("There is no levels state data to load.");
            return;
        }

        if (data.levelsData.LB13 == null)
        {
            Dev_Logger.Debug("There is no LB13 state data to load.");
            return;
        }

        Model_LevelBehavior_13 lvlModel     = data.levelsData.LB13;
        LB13.didPickUpLightSticker          = lvlModel.didPickUpLightSticker;

        Dev_Logger.Debug($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(lvlModel);
    }
}
