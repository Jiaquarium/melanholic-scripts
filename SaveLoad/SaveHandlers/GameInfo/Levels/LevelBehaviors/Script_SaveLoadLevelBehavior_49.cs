using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_49 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_49 LB49;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_49 lvlModel = new Model_LevelBehavior_49(
            _didActivateDoubts : LB49.didActivateDoubts
        );
        
        data.levelsData.LB49 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        Model_LevelBehavior_49 lvlModel = data.levelsData.LB49;
        LB49.didActivateDoubts = lvlModel.didActivateDoubts;

        Debug.Log($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(lvlModel);
    }
}
