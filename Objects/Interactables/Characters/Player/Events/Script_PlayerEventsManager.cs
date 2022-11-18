using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerEventsManager : MonoBehaviour
{
    public delegate void OnEnteredElevatorDelegate();
    public static event OnEnteredElevatorDelegate OnEnteredElevator;
    public static void EnteredElevator()
    {
        if (OnEnteredElevator != null)
            OnEnteredElevator();
    }

    public delegate void OnPuppeteerActivateDelegate();
    public static event OnPuppeteerActivateDelegate OnPuppeteerActivate;
    public static void PuppeteerActivate()
    {
        if (OnPuppeteerActivate != null)
            OnPuppeteerActivate();
    }

    public delegate void OnPuppeteerDeactivateDelegate();
    public static event OnPuppeteerDeactivateDelegate OnPuppeteerDeactivate;
    public static void PuppeteerDeactivate()
    {
        if (OnPuppeteerDeactivate != null)
            OnPuppeteerDeactivate();
    }

    public delegate void OnPuppeteerSwitchAnimatorDelegate();
    public static event OnPuppeteerSwitchAnimatorDelegate OnPuppeteerSwitchAnimator;
    public static void PuppeteerSwitchAnimator()
    {
        if (OnPuppeteerSwitchAnimator != null)
            OnPuppeteerSwitchAnimator();
    }
}
