using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Either hook up to Game which calls LB or use UnityEvent
/// </summary>
public class Script_TriggerPlayerEnter : Script_Trigger
{
    public Script_Game game;
    [SerializeField] private UnityEvent action;
    [SerializeField] private UnityEvent exitAction;

    void OnTriggerEnter(Collider other)
    {
        if (isColliding)    return;
        isColliding = true;

        if (other.tag == Const_Tags.Player)
        {
            if (action.CheckUnityEventAction())
            {
                action.Invoke();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (isColliding)    return;
        isColliding = true;

        if (other.tag == Const_Tags.Player)
        {
            if (exitAction.CheckUnityEventAction())
            {
                exitAction.Invoke();
            }
        }
    }
}
