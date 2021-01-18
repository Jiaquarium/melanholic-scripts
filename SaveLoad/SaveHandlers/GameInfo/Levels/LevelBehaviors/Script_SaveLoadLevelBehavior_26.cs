using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_26 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_26 LB26;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_26 lvlModel = new Model_LevelBehavior_26(
            _switchesState                  : LB26.switchesState,
            _isPuzzleComplete               : LB26.isPuzzleComplete,
            _didActivateDramaticThoughts    : LB26.didActivateDramaticThoughts,
            _didPickUpWinterStone           : LB26.didPickUpWinterStone
        );
        
        data.levelsData.LB26 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        if (data.levelsData == null)
        {
            Debug.Log("There is no levels state data to load.");
            return;
        }

        if (data.levelsData.LB26 == null)
        {
            Debug.Log("There is no LB26 state data to load.");
            return;
        }

        Model_LevelBehavior_26 lvlModel     = data.levelsData.LB26;
        LB26.switchesState                  = lvlModel.switchesState;
        LB26.isPuzzleComplete               = lvlModel.isPuzzleComplete;
        LB26.didActivateDramaticThoughts    = lvlModel.didActivateDramaticThoughts;
        LB26.didPickUpWinterStone           = lvlModel.didPickUpWinterStone;

        Debug.Log($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(lvlModel);
    }
}
