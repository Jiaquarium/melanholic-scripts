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
}
