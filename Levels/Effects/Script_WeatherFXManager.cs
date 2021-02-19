using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_WeatherFXManager : MonoBehaviour
{
    public static Script_Run.DayId SnowDayId = Script_Run.DayId.thu;
    
    [SerializeField] private Script_Game game;

    public void SnowDayEffect()
    {
        if (game.Run.dayId == SnowDayId)
        {
            game.levelBehavior.SnowFallStart();
        }
    }
}
