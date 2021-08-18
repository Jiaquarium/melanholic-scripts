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

        // Save the current animation state so we can start the new controller at the same frame.
        AnimatorStateInfo animatorStateInfo = playerMovement.MyAnimator.GetCurrentAnimatorStateInfo(Layer);

        playerMovement.MyAnimator.runtimeAnimatorController = stickerAnimatorController;
        playerMovement.PlayerGhost.MyAnimator.runtimeAnimatorController = stickerAnimatorController;

        SyncAnimationFrame(animatorStateInfo);
        
        playerMovement.MyAnimator.AnimatorSetDirection(playerMovement.LastMove);
        playerMovement.PlayerGhost.MyAnimator.AnimatorSetDirection(playerMovement.LastMove);
    }

    protected override void OnUnequip()
    {
        base.OnUnequip();

        AnimatorStateInfo animatorStateInfo = playerMovement.MyAnimator.GetCurrentAnimatorStateInfo(Layer);

        playerMovement.MyAnimator.runtimeAnimatorController = playerMovement.DefaultAnimatorController;
        playerMovement.PlayerGhost.MyAnimator.runtimeAnimatorController = playerMovement.DefaultAnimatorController;

        SyncAnimationFrame(animatorStateInfo);

        playerMovement.MyAnimator.AnimatorSetDirection(playerMovement.LastMove);
        playerMovement.PlayerGhost.MyAnimator.AnimatorSetDirection(playerMovement.LastMove);
    }
}
