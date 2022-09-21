using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ElevatorBehavior : MonoBehaviour
{
    public virtual void Effect()
    {
        Dev_Logger.Debug($"{name} Effect()");
    }
}
