using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_12 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_12 LB12;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_12 lvlModel = new Model_LevelBehavior_12(
            LB12.isDone,
            LB12.isCutSceneDone
        );
        
        data.levelsData.LB12 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        if (data.levelsData == null)
        {
            Debug.Log("There is no levels state data to load.");
            return;
        }

        if (data.levelsData.LB12 == null)
        {
            Debug.Log("There is no LB12 state data to load.");
            return;
        }

        Model_LevelBehavior_12 lvlModel = data.levelsData.LB12;
        LB12.isDone             = lvlModel.isDone;
        LB12.isCutSceneDone     = lvlModel.isCutSceneDone;
    }
}
