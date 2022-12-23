using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadAchievements : MonoBehaviour
{
    public Script_AchievementsManager achievementsManager;
    
    public void SaveAchievements(Model_SaveData data)
    {
        data.achievements = achievementsManager.achievementsState;
    }

    public void LoadAchievements(Model_SaveData data)
    {
        if (data.achievements == null)
        {
            Dev_Logger.Debug($"{name} There is no Achivements state data to load.");
            return;   
        }
        
        Model_Achievements achievements = data.achievements;
        achievementsManager.achievementsState = achievements;

        Dev_Logger.Debug($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(achievements);
    }    
}
