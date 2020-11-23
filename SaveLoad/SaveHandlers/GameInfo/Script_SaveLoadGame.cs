using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadGame : MonoBehaviour
{
    [SerializeField]
    private Script_Game game;

    public void SaveGameData(Model_SaveData data)
    {
        data.gameData = new Model_GameData(
            game.level,
            game.totalPlayTime
        );
    }

    public void LoadGameData(Model_SaveData data)
    {
        game.level = data.gameData.level;
        game.totalPlayTime = data.gameData.totalPlayTime;
    }
}
