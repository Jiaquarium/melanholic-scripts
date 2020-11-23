using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_TriggerEnterOnce : Script_Trigger
{
    public Script_Game game;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == Const_Tags.Player)
        {
            if (game.ActivateTrigger(Id))
                this.gameObject.SetActive(false);
        }
    }
}
