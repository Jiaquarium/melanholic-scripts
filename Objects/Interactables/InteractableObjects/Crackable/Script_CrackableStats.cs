using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_CrackableStats : Script_CharacterStats
{
    protected override void Die(Script_GameOverController.DeathTypes deathType)
    {
        Debug.Log("**** CRACKABLE DIE() ****");
        gameObject.SetActive(false);
    }
}
