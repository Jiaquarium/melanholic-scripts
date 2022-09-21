using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_41 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_41 LB41;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_41 lvlModel = new Model_LevelBehavior_41(
            _didPickUpMelancholyPianoSticker           : LB41.didPickUpMelancholyPianoSticker
        );
        
        data.levelsData.LB41 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        Model_LevelBehavior_41 lvlModel         = data.levelsData.LB41;
        LB41.didPickUpMelancholyPianoSticker    = lvlModel.didPickUpMelancholyPianoSticker;

        Dev_Logger.Debug($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(lvlModel);
    }
}
