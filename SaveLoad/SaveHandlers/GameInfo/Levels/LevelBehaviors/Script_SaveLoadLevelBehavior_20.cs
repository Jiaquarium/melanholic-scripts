using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_20 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_20 LB20;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_20 lvlModel = new Model_LevelBehavior_20(
            _isKingIntroCutSceneDone: LB20.isKingIntroCutSceneDone,
            _isMyneR2CutsceneDone: LB20.isMyneR2CutsceneDone,
            
            // Archive
            _season: LB20.season,
            _entranceCutSceneDone: LB20.entranceCutSceneDone,
            _isPuzzleComplete: LB20.isPuzzleComplete,
            _didPickUpMasterKey: LB20.didPickUpMasterKey,
            _didUnlockMasterLock: LB20.didUnlockMasterLock
        );
        
        data.levelsData.LB20 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        if (data.levelsData == null)
        {
            Dev_Logger.Debug("There is no levels state data to load.");
            return;
        }

        if (data.levelsData.LB20 == null)
        {
            Dev_Logger.Debug("There is no LB20 state data to load.");
            return;
        }

        Model_LevelBehavior_20 lvlModel = data.levelsData.LB20;
        LB20.isKingIntroCutSceneDone    = lvlModel.isKingIntroCutSceneDone;
        LB20.isMyneR2CutsceneDone       = lvlModel.isMyneR2CutsceneDone;
        
        // Archive
        LB20.season                     = lvlModel.season;
        LB20.entranceCutSceneDone       = lvlModel.entranceCutSceneDone;
        LB20.isPuzzleComplete           = lvlModel.isPuzzleComplete;
        LB20.didPickUpMasterKey         = lvlModel.didPickUpMasterKey;
        LB20.didUnlockMasterLock        = lvlModel.didUnlockMasterLock;

        Dev_Logger.Debug($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(lvlModel);
    }
}
