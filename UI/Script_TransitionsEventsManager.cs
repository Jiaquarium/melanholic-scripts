using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_TransitionsEventsManager : MonoBehaviour
{
    public delegate void StandaloneCutOutZoomOutDoneAction();
    public static event StandaloneCutOutZoomOutDoneAction OnStandaloneCutOutZoomOutDone;
    public static void StandaloneCutOutZoomOutDone()
    {
        if (OnStandaloneCutOutZoomOutDone != null)
            OnStandaloneCutOutZoomOutDone(); 
    }
}
