using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_42 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_42 LB42;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_42 lvlModel = new Model_LevelBehavior_42(
            _didPickUpLastWellMap           : LB42.didPickUpLastWellMap,
            _didPickUpSpeedSeal             : LB42.didPickUpSpeedSeal,            
            _isMooseQuestDone               : LB42.isMooseQuestDone,
            _didPlayFaceOff                 : LB42.didPlayFaceOff,
            _didSpecialIntro                : LB42.didSpecialIntro
        );
        
        data.levelsData.LB42 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        Model_LevelBehavior_42 lvlModel         = data.levelsData.LB42;
        
        LB42.didPickUpLastWellMap               = lvlModel.didPickUpLastWellMap;
        LB42.didPickUpSpeedSeal                 = lvlModel.didPickUpSpeedSeal;
        LB42.isMooseQuestDone                   = lvlModel.isMooseQuestDone;
        LB42.didPlayFaceOff                     = lvlModel.didPlayFaceOff;
        LB42.didSpecialIntro                    = lvlModel.didSpecialIntro;

        Dev_Logger.Debug($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(lvlModel);
    }
}
