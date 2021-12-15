using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When Player switches to a specified Mask, their Animator is forced to match
/// the target animator's normalized time.
/// </summary>
public class Script_ForcePlayerSyncAnimator : MonoBehaviour
{
    [SerializeField] private Script_Sticker syncSticker;
    [SerializeField] private Script_Game game;

    [SerializeField] private Animator animator;
    
    void OnEnable()
    {
        Script_StickerEffectEventsManager.OnEquip += HandleForceSyncAnimator;
    }

    void OnDisable()
    {
        Script_StickerEffectEventsManager.OnEquip -= HandleForceSyncAnimator;
    }

    private void HandleForceSyncAnimator(Script_Sticker sticker)
    {
        if (syncSticker.id == sticker.id)
        {
            AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(Script_PlayerMovement.Layer);
            
            game.GetPlayer().SyncAnimatorState(animatorStateInfo);
        }
    }
}
