using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_4 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_4 LB4;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_4 lvlModel = new Model_LevelBehavior_4(
            LB4.didPickUpMelancholyPianoSticker
        );
        
        data.levelsData.LB4 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        Model_LevelBehavior_4 lvlModel  = data.levelsData.LB4;
        LB4.didPickUpMelancholyPianoSticker             = lvlModel.didPickUpMelancholyPianoSticker;

        Dev_Logger.Debug($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(lvlModel);
    }
}
