using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Script_SaveSavedGameTitleData : MonoBehaviour
{
    public Script_Game game;
    
    public Model_SavedGameTitleData Create()
    {
        int run                 = game.Run;
        float clockTime         = Script_ClockManager.Control.ClockTime;
        
        string name             = Script_Names.Player;
        
        Script_Entry lastEntry  = game.entryManager.GetLastEntry();
        string headline         = lastEntry ? lastEntry.headline : string.Empty;
        
        long date               = lastEntry ? lastEntry.timestamp.ToBinary() : DateTime.Now.ToBinary();
        float playTime          = game.totalPlayTime;

        return new Model_SavedGameTitleData(
            run,
            clockTime,
            name,
            headline,
            date,
            playTime
        );
    }
}
