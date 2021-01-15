using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_EatableStats : Script_CharacterStats
{
    protected override void Die(Script_GameOverController.DeathTypes deathType)
    {
        Debug.Log("**** EATABLE DIE() ****");
        gameObject.SetActive(false);
    }
}
