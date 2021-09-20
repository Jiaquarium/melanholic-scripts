using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LetThereBeLightEffect : Script_StickerEffect
{
    [SerializeField] private Script_Player player;
    
    [SerializeField] private RuntimeAnimatorController lanternOnAnimatorController;
    
    
    private bool isLanternOn;

    public override void Effect()
    {
        if (isLanternOn)
        {
            OnEquipControllerSynced();   
            
            player.SwitchLight(false);
            isLanternOn = false;
        }
        else
        {
            LanternOnController();
            
            player.SwitchLight(true);
            isLanternOn = true;
        }
    }

    protected override void OnEquip()
    {
        base.OnEquip();
        OnEquipControllerSynced();
    }

    protected override void OnUnequip()
    {
        base.OnUnequip();
        OnUnequipControllerSynced();

        player.SwitchLight(false);
        isLanternOn = false;
    }

    protected override void OnUnequipSwitch()
    {
        base.OnUnequipSwitch();

        player.SwitchLight(false);
        isLanternOn = false;
    }

    private void LanternOnController()
    {
        // Save the current animation state so we can start the new controller at the same frame.
        AnimatorStateInfo animatorStateInfo = playerMovement.MyAnimator.GetCurrentAnimatorStateInfo(Layer);

        playerMovement.MyAnimator.runtimeAnimatorController = lanternOnAnimatorController;

        SyncAnimatorState(animatorStateInfo);
        
        playerMovement.MyAnimator.AnimatorSetDirection(playerMovement.FacingDirection);
    }
}
