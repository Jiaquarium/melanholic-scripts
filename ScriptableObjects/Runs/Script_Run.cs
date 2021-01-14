using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Run", menuName = "Runs/Run")]
public class Script_Run : ScriptableObject
{
    public enum DayId
    {
        mon     = 0,
        tue     = 1,
        wed     = 2,
        thu     = 3,
        fri     = 4,
        sat     = 5,
        sun     = 6,
        none    = 7, // use for testing / disabling events
    }
    public DayId dayId;
    public string dayName;
}
