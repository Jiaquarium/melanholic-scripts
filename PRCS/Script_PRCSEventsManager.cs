using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PRCSEventsManager : MonoBehaviour
{
    public delegate void PRCSDoneAction(Script_PRCSPlayer PRCSPlayer);
    public static event PRCSDoneAction OnPRCSDone;
    public static void PRCSDone(Script_PRCSPlayer PRCSPlayer)
    {
        if (OnPRCSDone != null) OnPRCSDone(PRCSPlayer);
    }    
}
