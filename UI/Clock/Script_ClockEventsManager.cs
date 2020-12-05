using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ClockEventsManager : MonoBehaviour
{
    public delegate void OnTimesUpDelegate();
    public static event OnTimesUpDelegate OnTimesUp;
    public static void TimesUp()
    {
        if (OnTimesUp != null)   OnTimesUp();
    }
}
