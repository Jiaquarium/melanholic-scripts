using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_MatchPlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Script_Game game;
    
    [Tooltip("Use to specify the Reflection Object if using one")]
    [SerializeField] private Script_PlayerReflection playerReflection;
    
    void OnEnable()
    {
        Script_StickerEffectEventsManager.OnEquip                   += MatchPlayerAnimator;
        Script_StickerEffectEventsManager.OnUnequip                 += HandleReturnDefault;
        Script_StickerEffectEventsManager.OnAnimatorEffectTrigger   += AnimatorEffectTrigger;
        Script_StickerEffectEventsManager.OnAnimatorEffectHold      += AnimatorEffectHold;
        
        MatchPlayerAnimator(null);
    }

    void OnDisable()
    {
        Script_StickerEffectEventsManager.OnEquip                   -= MatchPlayerAnimator;
        Script_StickerEffectEventsManager.OnUnequip                 -= HandleReturnDefault;
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

    private void HandleReturnDefault(Script_Sticker sticker, int prevHash, float prevNormalizedTime)
    {
        int Layer = Script_PlayerMovement.Layer;

        Script_Player player = game.GetPlayer();
        animator.runtimeAnimatorController = player.DefaultAnimator;

        Dev_Logger.Debug($"{name} prevNormalizedTime {prevNormalizedTime}");
        animator.Play(prevHash, Layer, prevNormalizedTime);
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
