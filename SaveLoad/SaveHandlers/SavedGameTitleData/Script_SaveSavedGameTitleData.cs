using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Script_SaveSavedGameTitleData : MonoBehaviour
{
    public Script_Game game;
    
    public Model_SavedGameTitleData Create()
    {
        int maxHp = game.GetPlayer().GetComponent<Script_PlayerStats>().stats.maxHp.GetVal();
        string name = Script_Names.Player;
        
        Script_Entry lastEntry = game.entryManager.GetLastEntry();
        string headline = lastEntry ? lastEntry.headline : string.Empty;
        
        long date = lastEntry ? lastEntry.timestamp.ToBinary() : DateTime.Now.ToBinary();
        float playTime = game.totalPlayTime;

        return new Model_SavedGameTitleData(
            maxHp,
            name,
            headline,
            date,
            playTime
        );
    }
}
