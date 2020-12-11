using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_29 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_29 LB29;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_29 lvlModel = new Model_LevelBehavior_29(
            _gotPsychicDuck             : LB29.gotPsychicDuck,
            _updatedUrsieName           : LB29.updatedUrsieName,
            _questActive                : LB29.questActive,
            _questComplete              : LB29.questComplete,
            _isSaloonLocked             : LB29.isSaloonLocked
        );
        
        data.levelsData.LB29 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        if (data.levelsData == null)
        {
            Debug.Log("There is no levels state data to load.");
            return;
        }

        if (data.levelsData.LB29 == null)
        {
            Debug.Log("There is no LB29 state data to load.");
            return;
        }

        Model_LevelBehavior_29 lvlModel     = data.levelsData.LB29;
        LB29.gotPsychicDuck                 = lvlModel.gotPsychicDuck;
        LB29.updatedUrsieName               = lvlModel.updatedUrsieName;
        LB29.questActive                    = lvlModel.questActive;
        LB29.questComplete                  = lvlModel.questComplete;
        LB29.isSaloonLocked                 = lvlModel.isSaloonLocked;
    }
}
