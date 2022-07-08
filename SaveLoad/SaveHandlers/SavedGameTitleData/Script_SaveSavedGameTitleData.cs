using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Script_SaveSavedGameTitleData : MonoBehaviour
{
    public Script_Game game;
    
    public Model_SavedGameTitleData Create()
    {
        string run              = game.GetPlayerDisplayDayName;
        float clockTime         = Script_ClockManager.Control.ClockTime;
        
        string name             = Script_Names.Player;
        
        string headline         = string.Empty;

        int maskCount           = game.GetMaskCount();
        int[] scarletCipher     = Script_ScarletCipherManager.Control.ScarletCipherPublic;
        
        long date               = DateTime.Now.ToBinary();
        float playTime          = game.totalPlayTime;

        return new Model_SavedGameTitleData(
            run,
            clockTime,
            name,
            headline,
            maskCount,
            scarletCipher,
            date,
            playTime
        );
    }
}
