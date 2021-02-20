using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_48 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_48 LB48;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_48 lvlModel = new Model_LevelBehavior_48(
            _isDone           : LB48.IsDone
        );
        
        data.levelsData.LB48 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        Model_LevelBehavior_48 lvlModel         = data.levelsData.LB48;
        LB48.IsDone                             = lvlModel.isDone;

        Debug.Log($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(lvlModel);
    }
}
