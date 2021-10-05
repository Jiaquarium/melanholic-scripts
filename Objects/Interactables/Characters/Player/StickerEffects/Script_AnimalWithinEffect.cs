using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AnimalWithinEffect : Script_StickerEffect
{
    [SerializeField] private Script_PlayerAttackEat eatAttack;

    [SerializeField] private float shakeTime; 
    [SerializeField] private float shakeAmp; 
    [SerializeField] private float shakeFreq;
    
    public override void Effect()
    {
        Debug.Log($"{name} Effect()");

        eatAttack.Eat(player.FacingDirection);

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
