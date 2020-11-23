using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_22 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_22 LB22;

    public override void Save(Model_SaveData data)
    {
        Model_LevelBehavior_22 lvlModel = new Model_LevelBehavior_22(
            LB22.isMyneCutSceneDone
        );
        
        data.levelsData.LB22 = lvlModel;
    }

    public override void Load(Model_SaveData data)
    {
        if (data.levelsData == null)
        {
            Debug.Log("There is no levels state data to load.");
            return;
        }

        if (data.levelsData.LB22 == null)
        {
            Debug.Log("There is no LB22 state data to load.");
            return;
        }

        Model_LevelBehavior_22 lvlModel = data.levelsData.LB22;
        LB22.isMyneCutSceneDone         = lvlModel.isMyneCutSceneDone;
    }
}
