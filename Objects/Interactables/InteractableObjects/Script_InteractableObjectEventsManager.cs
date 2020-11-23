using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_InteractableObjectEventsManager : MonoBehaviour
{
    public delegate void SwitchOnDelegate(string switchId);
    public static event SwitchOnDelegate OnSwitchOn;
    public static void SwitchOn(string switchId)
    {
        if (OnSwitchOn != null)   OnSwitchOn(switchId);
    }

    public delegate void SwitchOffDelegate(string switchId);
    public static event SwitchOffDelegate OnSwitchOff;
    public static void SwitchOff(string switchId)
    {
        if (OnSwitchOff != null)   OnSwitchOff(switchId);
    }
}
