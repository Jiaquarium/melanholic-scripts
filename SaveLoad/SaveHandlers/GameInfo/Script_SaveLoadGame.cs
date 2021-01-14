using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadGame : MonoBehaviour
{
    [SerializeField] private Script_Game game;

    public void SaveGameData(Model_SaveData data, Model_GameData gameDataOverride)
    {
        data.gameData = new Model_GameData(
            gameDataOverride?.run           ?? game.Run,
            gameDataOverride?.level         ?? game.level,
            gameDataOverride?.totalPlayTime ?? game.totalPlayTime
        );
    }

    public void LoadGameData(Model_SaveData data)
    {
        game.Run = data.gameData.run;
        game.level = data.gameData.level;
        game.totalPlayTime = data.gameData.totalPlayTime;
    }

    public void UpdatePlayTime(Model_SaveData data)
    {
        data.gameData = new Model_GameData(
            data.gameData.run,
            data.gameData.level,
            game.totalPlayTime
        );
    }
}
