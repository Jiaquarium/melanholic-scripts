using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LetThereBeLightEffect : Script_StickerEffect
{
    [SerializeField] private RuntimeAnimatorController lanternOnAnimatorController;
    [SerializeField] private AudioSource audioSource;
    
    private bool isLanternOn;

    public bool IsLanternOn
    {
        get => isLanternOn;
    }

    public override void Effect()
    {
        if (isLanternOn)
        {
            OnEquipControllerSynced();   
            
            player.SwitchLight(false);
            isLanternOn = false;

            var level = Script_Game.Game.levelBehavior;
            
            // Give control of SFX to the level if there's an event when lights turn off.
            if (!level.OnLanternEffectOff())
            {
                var sfx = Script_SFXManager.SFX;
                audioSource.PlayOneShot(sfx.LanternOff, sfx.LanternOffVol);
            }
        }
        else
        {
            LanternOnController();
            
            player.SwitchLight(true);
            isLanternOn = true;

            var level = Script_Game.Game.levelBehavior;

            // Give control of SFX to the level if there's an event when lights turn on.
            if (!level.OnLanternEffectOn())
            {
                var sfx = Script_SFXManager.SFX;
                audioSource.PlayOneShot(sfx.LanternOn, sfx.LanternOnVol);
            }
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

    protected override void OnUnequipState()
    {
        base.OnUnequipState();

        player.SwitchLight(false);
        isLanternOn = false;
    }

    private void LanternOnController()
    {
        // Save the current animation state so we can start the new controller at the same frame.
        AnimatorStateInfo animatorStateInfo = playerMovement.MyAnimator.GetCurrentAnimatorStateInfo(Script_PlayerMovement.Layer);

        playerMovement.MyAnimator.runtimeAnimatorController = lanternOnAnimatorController;

        playerMovement.SyncAnimatorState(animatorStateInfo);
        
        playerMovement.MyAnimator.AnimatorSetDirection(playerMovement.FacingDirection);

        Script_StickerEffectEventsManager.Equip(sticker);
    }
}
