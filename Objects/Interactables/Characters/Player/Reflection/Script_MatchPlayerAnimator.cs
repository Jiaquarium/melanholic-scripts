using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_MatchPlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Script_Game game;
    
    void OnEnable()
    {
        Script_StickerEffectEventsManager.OnEquip                   += MatchPlayerAnimator;
        Script_StickerEffectEventsManager.OnAnimatorEffectTrigger   += AnimatorEffectTrigger;
        Script_StickerEffectEventsManager.OnAnimatorEffectHold      += AnimatorEffectHold;
        
        MatchPlayerAnimator(null);
    }

    void OnDisable()
    {
        Script_StickerEffectEventsManager.OnEquip                   -= MatchPlayerAnimator;
        Script_StickerEffectEventsManager.OnAnimatorEffectTrigger   -= AnimatorEffectTrigger;
        Script_StickerEffectEventsManager.OnAnimatorEffectHold      -= AnimatorEffectHold;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    private void MatchPlayerAnimator(Script_Sticker sticker)
    {
        int Layer = Script_PlayerMovement.Layer;
        
        RuntimeAnimatorController playerAnimatorController = game?.GetPlayer()?.MyAnimator.runtimeAnimatorController;
        
        if (playerAnimatorController == null)
            return;
        
        animator.runtimeAnimatorController = playerAnimatorController;

        AnimatorStateInfo animatorStateInfo = game.GetPlayer().MyAnimator
            .GetCurrentAnimatorStateInfo(Layer);
        
        animator.Play(animatorStateInfo.fullPathHash, Layer, animatorStateInfo.normalizedTime);       
    }

    private void AnimatorEffectTrigger()
    {
        animator.SetTrigger(Script_Player.IsEffectTrigger);
    }

    private void AnimatorEffectHold(bool isActive)
    {
        animator.SetBool(Script_Player.IsEffectHoldBool, isActive);
    }
}
