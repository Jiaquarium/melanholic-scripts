﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Either hook up to Game which calls LB or use UnityEvent
/// </summary>
public class Script_TriggerEnterOnce : Script_Trigger
{
    public Script_Game game;
    [SerializeField] private UnityEvent action;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == Const_Tags.Player)
        {
            if (action.CheckUnityEventAction())
            {
                action.Invoke();
            }
            else if (game.ActivateTrigger(Id))
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
