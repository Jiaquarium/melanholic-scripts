using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadGame : MonoBehaviour
{
    [SerializeField] private Script_Game game;

    public void SaveGameData(Model_SaveData data, Model_GameData gameDataOverride)
    {
        Dev_Logger.Debug($"Saving Total Time Played {game.totalPlayTime.FormatTotalPlayTime()}");
        
        data.gameData = new Model_GameData(
            gameDataOverride?.runIdx            ?? game.RunIdx,
            gameDataOverride?.level             ?? game.level,
            gameDataOverride?.cycleCount        ?? game.CycleCount,
            gameDataOverride?.totalPlayTime     ?? game.totalPlayTime,
            gameDataOverride?.activeEnding      ?? game.ActiveEnding,
            gameDataOverride?.faceOffCounter    ?? game.faceOffCounter
        );
    }

    public void LoadGameData(Model_SaveData data)
    {
        game.LoadRun(data.gameData.runIdx, data.gameData.cycleCount);
        game.level          = data.gameData.level;
        game.totalPlayTime  = data.gameData.totalPlayTime;
        game.ActiveEnding   = data.gameData.activeEnding;
        game.faceOffCounter = data.gameData.faceOffCounter;

        Dev_Logger.Debug($"---- LOADED {data.gameData} ----");
        Dev_Logger.Debug($"runIdx:             {data.gameData.runIdx}");
        Dev_Logger.Debug($"level:              {data.gameData.level}");
        Dev_Logger.Debug($"totalPlayTime:      {data.gameData.totalPlayTime}");
        Dev_Logger.Debug($"activeEnding:       {data.gameData.activeEnding}");
        Dev_Logger.Debug($"faceOffCounter:     {data.gameData.faceOffCounter}");
    }

    public void UpdatePlayTime(Model_SaveData data)
    {
        data.gameData = new Model_GameData(
            data.gameData.runIdx,
            data.gameData.level,
            data.gameData.cycleCount,
            game.totalPlayTime,
            data.gameData.activeEnding,
            data.gameData.faceOffCounter
        );
    }
}
