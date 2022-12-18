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

    public delegate void NPCRandomAnimatorSwitchDelegate();
    public static event NPCRandomAnimatorSwitchDelegate OnNPCRandomAnimatorSwitch;
    public static void NPCRandomAnimatorSwitch()
    {
        if (OnNPCRandomAnimatorSwitch != null)
            OnNPCRandomAnimatorSwitch();
    }

    public delegate void NPCRandomAnimatorDefaultDelegate();
    public static event NPCRandomAnimatorDefaultDelegate OnNPCRandomAnimatorDefault;
    public static void NPCRandomAnimatorDefault()
    {
        if (OnNPCRandomAnimatorDefault != null)
            OnNPCRandomAnimatorDefault();
    }

    public delegate void NPCRandomAnimatorForceDefaultDelegate();
    public static event NPCRandomAnimatorForceDefaultDelegate OnNPCRandomAnimatorForceDefault;
    public static void NPCRandomAnimatorForceDefault()
    {
        if (OnNPCRandomAnimatorForceDefault != null)
            OnNPCRandomAnimatorForceDefault();
    }

    public delegate void NPCRandomAnimatorStopForceDefaultDelegate();
    public static event NPCRandomAnimatorStopForceDefaultDelegate OnNPCRandomAnimatorStopForceDefault;
    public static void NPCRandomAnimatorStopForceDefault()
    {
        if (OnNPCRandomAnimatorStopForceDefault != null)
            OnNPCRandomAnimatorStopForceDefault();
    }
}
