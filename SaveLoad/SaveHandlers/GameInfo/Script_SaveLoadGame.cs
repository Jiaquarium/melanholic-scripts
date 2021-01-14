using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadGame : MonoBehaviour
{
    [SerializeField] private Script_Game game;

    public void SaveGameData(Model_SaveData data, Model_GameData gameDataOverride)
    {
        data.gameData = new Model_GameData(
            gameDataOverride?.runIdx        ?? game.RunIdx,
            gameDataOverride?.level         ?? game.level,
            gameDataOverride?.totalPlayTime ?? game.totalPlayTime
        );
    }

    public void LoadGameData(Model_SaveData data)
    {
        game.LoadRun(data.gameData.runIdx);
        game.level = data.gameData.level;
        game.totalPlayTime = data.gameData.totalPlayTime;
    }

    public void UpdatePlayTime(Model_SaveData data)
    {
        data.gameData = new Model_GameData(
            data.gameData.runIdx,
            data.gameData.level,
            game.totalPlayTime
        );
    }
}
