using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_19 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_19 LB19;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_19 lvlModel = new Model_LevelBehavior_19();
        
        data.levelsData.LB19 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        if (data.levelsData == null)
        {
            Debug.Log("There is no levels state data to load.");
            return;
        }

        if (data.levelsData.LB19 == null)
        {
            Debug.Log("There is no LB19 state data to load.");
            return;
        }

        Model_LevelBehavior_19 lvlModel = data.levelsData.LB19;

        Debug.Log($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(lvlModel);
    }
}
