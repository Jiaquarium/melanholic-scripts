using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_DDREventsManager : MonoBehaviour
{
    public delegate void DDRDoneDelegate();
    public static event DDRDoneDelegate OnDDRDone;
    public static void DDRDone() {
        if (OnDDRDone != null) OnDDRDone();
    }
}
