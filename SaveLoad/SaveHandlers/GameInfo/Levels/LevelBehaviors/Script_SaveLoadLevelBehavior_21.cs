using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_21 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_21 LB21;

    public override void Save(Model_SaveData data)
    {
        Model_LevelBehavior_21 lvlModel = new Model_LevelBehavior_21(
            LB21.spokenWithEileen,
            LB21.didOnEntranceAttack
        );
        
        data.levelsData.LB21 = lvlModel;
    }

    public override void Load(Model_SaveData data)
    {
        if (data.levelsData == null)
        {
            Debug.Log("There is no levels state data to load.");
            return;
        }

        if (data.levelsData.LB21 == null)
        {
            Debug.Log("There is no LB21 state data to load.");
            return;
        }

        Model_LevelBehavior_21 lvlModel = data.levelsData.LB21;
        LB21.spokenWithEileen             = lvlModel.spokenWithEileen;
        LB21.didOnEntranceAttack          = lvlModel.didOnEntranceAttack;
    }
}
