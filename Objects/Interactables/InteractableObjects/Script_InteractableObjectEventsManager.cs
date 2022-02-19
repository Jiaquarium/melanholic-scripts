using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_InteractableObjectEventsManager : MonoBehaviour
{
    public delegate void SwitchOnDelegate(string switchId);
    public static event SwitchOnDelegate OnSwitchOn;
    public static void SwitchOn(string switchId)
    {
        if (OnSwitchOn != null)
            OnSwitchOn(switchId);
    }

    public delegate void SwitchOffDelegate(string switchId);
    public static event SwitchOffDelegate OnSwitchOff;
    public static void SwitchOff(string switchId)
    {
        if (OnSwitchOff != null)
            OnSwitchOff(switchId);
    }

    public delegate void WellInteraction(Script_Well well);
    public static event WellInteraction OnWellInteraction;
    public static void WellInteract(Script_Well well)
    {
        if (OnWellInteraction != null)
            OnWellInteraction(well);
    }

    public delegate void FrozenWellDieDelegate(Script_FrozenWellCrackableStats iceStats);
    public static event FrozenWellDieDelegate OnFrozenWellDie;
    public static void FrozenWellDie(Script_FrozenWellCrackableStats iceStats)
    {
        if (OnFrozenWellDie != null)
            OnFrozenWellDie(iceStats);
    }

    public delegate void IceCrackingTimelineDoneDelegate(Script_CrackableStats iceStats);
    public static event IceCrackingTimelineDoneDelegate OnIceCrackingTimelineDone;
    public static void IceCrackingTimelineDone(Script_CrackableStats iceStats)
    {
        if (OnIceCrackingTimelineDone != null)
            OnIceCrackingTimelineDone(iceStats);
    }

    public delegate void ShatterDelegate(Script_CrackableStats iceStats);
    public static event ShatterDelegate OnShatter;
    public static void Shatter(Script_CrackableStats iceStats)
    {
        if (OnShatter != null)
            OnShatter(iceStats);
    }
}
