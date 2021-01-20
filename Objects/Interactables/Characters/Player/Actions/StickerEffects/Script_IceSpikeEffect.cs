using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_IceSpikeEffect : Script_StickerEffect
{
    [SerializeField] private Script_IceSpikeAttack iceSpikeAttack;
    [SerializeField] private Script_Player player; 
    
    public override void Effect()
    {
        Debug.Log($"{name} Effect()");
        iceSpikeAttack.Spike(player.FacingDirection);
    }
}
