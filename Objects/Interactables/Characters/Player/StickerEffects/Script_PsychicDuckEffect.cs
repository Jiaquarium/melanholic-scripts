using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Script_PsychicDuckEffect : Script_StickerEffect
{
    [SerializeField] private RuntimeAnimatorController psychicDuckAnimatorController;
    [SerializeField] private Script_PlayerMovement playerMovement;
    
    public override void Effect()
    {
        AudioClip clip  = Script_SFXManager.SFX.psychicDuckQuack;
        float vol       = Script_SFXManager.SFX.psychicDuckQuackVol;
        GetComponent<AudioSource>().PlayOneShot(clip, vol);
    }

    protected override void OnEquip()
    {
        base.OnEquip();

        playerMovement.MyAnimator.runtimeAnimatorController = psychicDuckAnimatorController;
        playerMovement.PlayerGhost.MyAnimator.runtimeAnimatorController = psychicDuckAnimatorController;

        playerMovement.MyAnimator.AnimatorSetDirection(playerMovement.LastMove);
        playerMovement.PlayerGhost.MyAnimator.AnimatorSetDirection(playerMovement.LastMove);
    }

    protected override void OnUnequip()
    {
        base.OnUnequip();

        playerMovement.MyAnimator.runtimeAnimatorController = playerMovement.DefaultAnimatorController;
        playerMovement.PlayerGhost.MyAnimator.runtimeAnimatorController = playerMovement.DefaultAnimatorController;

        playerMovement.MyAnimator.AnimatorSetDirection(playerMovement.LastMove);
        playerMovement.PlayerGhost.MyAnimator.AnimatorSetDirection(playerMovement.LastMove);
    }
}
