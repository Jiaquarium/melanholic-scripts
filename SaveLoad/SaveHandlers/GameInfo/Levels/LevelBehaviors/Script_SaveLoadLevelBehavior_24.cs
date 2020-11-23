using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_24 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_24 LB24;

    public override void Save(Model_SaveData data)
    {
        Model_LevelBehavior_24 lvlModel = new Model_LevelBehavior_24(
            LB24.isPuzzleComplete,
            LB24.didPickUpSpringStone
        );
        
        data.levelsData.LB24 = lvlModel;
    }

    public override void Load(Model_SaveData data)
    {
        if (data.levelsData == null)
        {
            Debug.Log("There is no levels state data to load.");
            return;
        }

        if (data.levelsData.LB24 == null)
        {
            Debug.Log("There is no LB24 state data to load.");
            return;
        }

        Model_LevelBehavior_24 lvlModel = data.levelsData.LB24;
        LB24.isPuzzleComplete       = lvlModel.isPuzzleComplete;
        LB24.didPickUpSpringStone         = lvlModel.didPickUpSpringStone;
    }
}
