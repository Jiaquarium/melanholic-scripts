using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// aka Ice Princess
public class Script_IceSpikeEffect : Script_StickerEffect
{
    [SerializeField] private Script_IceSpikeAttack iceSpikeAttack;
    [SerializeField] private Script_Player player; 
    [SerializeField] private float shakeTime; 
    [SerializeField] private float shakeAmp; 
    [SerializeField] private float shakeFreq;
    
    public override void Effect()
    {
        Debug.Log($"{name} Effect()");
        iceSpikeAttack.Spike(player.FacingDirection);
        
        // NOTE Ensure the triggered Effect animation time <= time we remain in Effect state.
        player.TriggerEffect();

        Script_VCamManager.VCamMain.GetComponent<Script_CameraShake>().Shake(
            shakeTime,
            shakeAmp,
            shakeFreq,
            null
        );
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
