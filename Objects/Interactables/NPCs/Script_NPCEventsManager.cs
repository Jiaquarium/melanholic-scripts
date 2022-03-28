using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_NPCEventsManager : MonoBehaviour
{
    public delegate void NPCMovesSetsDoneDelegate();
    public static event NPCMovesSetsDoneDelegate OnNPCMovesSetsDone;
    public static void NPCMoveSetsDone()
    {
        if (OnNPCMovesSetsDone != null)
            OnNPCMovesSetsDone();
    }
}
