using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Script_StickerEffect : MonoBehaviour
{
    public enum EquipType
    {
        Equip               = 0,
        
        // Unequipping by pressing the current active slot.
        Unequip             = 1,
        
        // Unequipping by pressing a different active slot.
        UnequipSwitch       = 2,
        
        // Use only if need to switch back to Player animator without triggering any effect.
        UnequipState        = 3,
    }
    
    protected const int Layer = 0;
    
    [SerializeField] protected Script_PlayerMovement playerMovement;
    [SerializeField] protected RuntimeAnimatorController stickerAnimatorController;
    
    public abstract void Effect();

    public virtual void EquipEffect(EquipType type)
    {
        switch (type)
        {
            case EquipType.Equip:
                OnEquip();
                break;
            case EquipType.Unequip:
                OnUnequip();
                break;
            case EquipType.UnequipSwitch:
                OnUnequipSwitch();
                break;
            case EquipType.UnequipState:
                OnUnequipState();
                break;
        }
    }

    protected virtual void OnEquip()
    {
        Debug.Log($"{name} OnEquip()");
    }

    protected virtual void OnUnequip()
    {
        Debug.Log($"{name} OnUnequip()");
    }

    // Unequip when switching to another Sticker. The main difference with this and OnUnequip is that
    // this should not call OnUnequipControllerSynced() to resync to the default controller.
    // The reason for this is when switching the new controller overrides the previous so we don't
    // need to unecessarily override with the Default controller before switching to the new one.
    protected virtual void OnUnequipSwitch()
    {
        Debug.Log($"{name} OnUnequipSwitch()");
    }

    protected virtual void OnUnequipState()
    {
        Debug.Log($"{name} OnUnequipState()");
        
        OnUnequipControllerSynced();
    }

    // Switches to a controller that is a "mask" transformation, keeping the same state.
    protected void OnEquipControllerSynced()
    {
        // Save the current animation state so we can start the new controller at the same frame.
        AnimatorStateInfo animatorStateInfo = playerMovement.MyAnimator.GetCurrentAnimatorStateInfo(Layer);

        playerMovement.MyAnimator.runtimeAnimatorController = stickerAnimatorController;

        SyncAnimatorState(animatorStateInfo);
        
        playerMovement.MyAnimator.AnimatorSetDirection(playerMovement.FacingDirection);
    }

    // Handle unequipping the active sticker to return to the default controller.
    protected void OnUnequipControllerSynced()
    {
        AnimatorStateInfo animatorStateInfo = playerMovement.MyAnimator.GetCurrentAnimatorStateInfo(Layer);

        playerMovement.MyAnimator.runtimeAnimatorController = playerMovement.DefaultAnimatorController;

        SyncAnimatorState(animatorStateInfo);

        playerMovement.MyAnimator.AnimatorSetDirection(playerMovement.FacingDirection);
    }

    // Play the new controller at the saved state time.
    protected void SyncAnimatorState(AnimatorStateInfo animatorStateInfo)
    {
        playerMovement.MyAnimator.Play(animatorStateInfo.fullPathHash, Layer, animatorStateInfo.normalizedTime);
    }
}
