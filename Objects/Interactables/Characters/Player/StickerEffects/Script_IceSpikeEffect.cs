using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// aka Ice Princess
public class Script_IceSpikeEffect : Script_StickerEffect
{
    [SerializeField] private Script_IceSpikeAttack iceSpikeAttack;
    public static float ShakeTime = 0.6f; 
    public static float ShakeAmp = 2f; 
    public static float ShakeFreq = 1f;
    
    public override void Effect()
    {
        Dev_Logger.Debug($"{name} Effect()");
        iceSpikeAttack.Spike(player.FacingDirection);
        
        // NOTE Ensure the triggered Effect animation time <= time we remain in Effect state.
        player.AnimatorEffectTrigger();
    }

    protected override void OnEquip()
    {
        base.OnEquip();
        OnEquipControllerSynced();
    }

    protected override void OnUnequip()
    {
        base.OnEquip();
        OnUnequipControllerSynced();
    }
}
