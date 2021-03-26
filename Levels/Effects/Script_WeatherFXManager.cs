using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_WeatherFXManager : MonoBehaviour
{
    public static Script_Run.DayId SnowDayId = Script_Run.DayId.thu;
    
    public static Script_WeatherFXManager Control;
    
    [SerializeField] private Script_Game game;

    public bool IsSnowDay
    {
        get => game.Run.dayId == SnowDayId;
    }
    
    public void SnowDayEffect()
    {
        game.levelBehavior.HandleSnowFallStart(IsSnowDay);
    }

    public void Setup()
    {
        if (Control == null)
        {
            Control = this;
        }
        else if (Control != this)
        {
            Destroy(this.gameObject);
        }
    }
}
