using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_20 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_20 LB20;

    public override void Save(Model_SaveData data)
    {
        Model_LevelBehavior_20 lvlModel = new Model_LevelBehavior_20(
            LB20.season,
            LB20.entranceCutSceneDone,
            LB20.isPuzzleComplete,
            LB20.didPickUpMasterKey,
            LB20.didUnlockMasterLock
        );
        
        data.levelsData.LB20 = lvlModel;
    }

    public override void Load(Model_SaveData data)
    {
        if (data.levelsData == null)
        {
            Debug.Log("There is no levels state data to load.");
            return;
        }

        if (data.levelsData.LB20 == null)
        {
            Debug.Log("There is no LB20 state data to load.");
            return;
        }

        Model_LevelBehavior_20 lvlModel = data.levelsData.LB20;
        LB20.season                 = lvlModel.season;
        LB20.entranceCutSceneDone   = lvlModel.entranceCutSceneDone;
        LB20.isPuzzleComplete       = lvlModel.isPuzzleComplete;
        LB20.didPickUpMasterKey     = lvlModel.didPickUpMasterKey;
        LB20.didUnlockMasterLock    = lvlModel.didUnlockMasterLock;
    }
}
