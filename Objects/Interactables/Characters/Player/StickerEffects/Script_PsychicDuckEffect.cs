using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Script_PsychicDuckEffect : Script_StickerEffect
{
    public override void Effect()
    {
        AudioClip clip  = Script_SFXManager.SFX.psychicDuckQuack;
        float vol       = Script_SFXManager.SFX.psychicDuckQuackVol;
        GetComponent<AudioSource>().PlayOneShot(clip, vol);
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
