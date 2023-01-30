using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_TransitionsEventsManager : MonoBehaviour
{
    public delegate void DayNotificationStandaloneFadeOutDoneAction();
    public static event DayNotificationStandaloneFadeOutDoneAction OnDayNotificationStandaloneFadeOutDone;
    public static void DayNotificationStandaloneFadeOutDone()
    {
        if (OnDayNotificationStandaloneFadeOutDone != null)
            OnDayNotificationStandaloneFadeOutDone(); 
    }

    public delegate void MapNotificationTeletypeDoneDelegate(bool isWorldPaintingIntro);
    public static event MapNotificationTeletypeDoneDelegate OnMapNotificationTeletypeDone;
    public static void MapNotificationTeletypeDone(bool isWorldPaintingIntro)
    {
        if (OnMapNotificationTeletypeDone != null)
            OnMapNotificationTeletypeDone(isWorldPaintingIntro); 
    }

    public delegate void MapNotificationDefaultDoneDelegate();
    public static event MapNotificationDefaultDoneDelegate OnMapNotificationDefaultDone;
    public static void MapNotificationDefaultDone()
    {
        if (OnMapNotificationDefaultDone != null)
            OnMapNotificationDefaultDone(); 
    }
}
