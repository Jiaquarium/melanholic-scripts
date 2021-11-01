using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_46 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_46 LB46;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_46 lvlModel = new Model_LevelBehavior_46(
            _isPuzzleComplete               : LB46.isPuzzleComplete,
            _didPlayFaceOff                 : LB46.didPlayFaceOff
        );
        
        data.levelsData.LB46 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        Model_LevelBehavior_46 lvlModel         = data.levelsData.LB46;
        
        LB46.isPuzzleComplete                   = lvlModel.isPuzzleComplete;
        LB46.didPlayFaceOff                     = lvlModel.didPlayFaceOff;

        Debug.Log($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(lvlModel);
    }
}
