using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        
        Script_VCamManager.VCamMain.GetComponent<Script_CameraShake>().Shake(
            shakeTime,
            shakeAmp,
            shakeFreq,
            null
        );
    }
}
