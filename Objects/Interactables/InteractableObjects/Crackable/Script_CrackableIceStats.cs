using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For Frozen Well
public class Script_CrackableIceStats : Script_CrackableStats
{
    protected override void Die(Script_GameOverController.DeathTypes deathType)
    {
        Debug.Log("**** **** CRACKABLE ICE DIE() **** ****");
        
        Script_InteractableObjectEventsManager.FrozenWellDie(this);
        
        gameObject.SetActive(false);
    }
}
