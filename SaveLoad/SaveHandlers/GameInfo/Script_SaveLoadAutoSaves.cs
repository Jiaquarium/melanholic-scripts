using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadAutoSaves : MonoBehaviour
{
    [SerializeField] private Script_LevelBehavior_26 LB26;
    [SerializeField] private Script_Game game;
    
    public void SaveAutoSaves(Model_SaveData data)
    {
        Model_AutoSaves autoSavesModel = new Model_AutoSaves(
            _spikeRoomTryCounterAutoUpdate: LB26.spikeRoomTryCounterAutoUpdate,
            _gameDidGoodEnding: game.didGoodEnding
        );

        data.autoSaves = autoSavesModel;
    }

    public void LoadAutoSaves(Model_SaveData data)
    {
        if (data.autoSaves == null)
        {
            Dev_Logger.Debug($"{name} There is no AutoSaves state data to load.");
            return;
        }

        Model_AutoSaves autoSavesModel = data.autoSaves;

        LB26.spikeRoomTryCounterAutoUpdate = autoSavesModel.spikeRoomTryCounterAutoUpdate;
        game.didGoodEnding = autoSavesModel.gameDidGoodEnding;

        Dev_Logger.Debug($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(autoSavesModel);
    }
}
