using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Script_StickerEffect : MonoBehaviour
{
    protected const int Layer = 0;
    
    [SerializeField] protected Script_PlayerMovement playerMovement;
    [SerializeField] protected RuntimeAnimatorController stickerAnimatorController;
    
    public abstract void Effect();

    public virtual void EquipEffect(bool isEquip)
    {
        if (isEquip)    OnEquip();
        else            OnUnequip();
    }

    protected virtual void OnEquip()
    {
        Debug.Log($"{name} OnEquip()");
    }

    protected virtual void OnUnequip()
    {
        Debug.Log($"{name} OnUnequip()");
    }

    // Play the new controller at the saved state time.
    // NOTE: can only be used for "mask" transformations that keep the same animations as PlayerMovement.
    protected void SyncAnimationFrame(AnimatorStateInfo animatorStateInfo)
    {
        playerMovement.MyAnimator.Play(animatorStateInfo.fullPathHash, Layer, animatorStateInfo.normalizedTime);
        playerMovement.PlayerGhost.MyAnimator.Play(animatorStateInfo.fullPathHash, Layer, animatorStateInfo.normalizedTime);
    }
}
