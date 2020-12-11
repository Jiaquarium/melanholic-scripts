using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_31 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_31 LB31;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_31 lvlModel = new Model_LevelBehavior_31(
            _isDone           : LB31.isDone
        );
        
        data.levelsData.LB31 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        if (data.levelsData == null)
        {
            Debug.Log("There is no levels state data to load.");
            return;
        }

        if (data.levelsData.LB31 == null)
        {
            Debug.Log("There is no LB31 state data to load.");
            return;
        }

        Model_LevelBehavior_31 lvlModel     = data.levelsData.LB31;
        LB31.isDone                         = lvlModel.isDone;
    }
}
