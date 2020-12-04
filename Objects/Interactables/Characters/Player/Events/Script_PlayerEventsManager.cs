using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerEventsManager : MonoBehaviour
{
    public delegate void OnEnteredElevatorDelegate();
    public static event OnEnteredElevatorDelegate OnEnteredElevator;
    public static void EnteredElevator()
    {
        if (OnEnteredElevator != null)   OnEnteredElevator();
    }
}
