using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PuppeteerEffect : Script_StickerEffect
{
    [SerializeField] private Script_Player player;
    
    private bool isActive;
    private string playerLastState;
    
    public override void Effect()
    {
        if (isActive)
        {
            // Event to switch back to default view.
            Script_PlayerEventsManager.PuppeteerDeactivate();
            
            player.SetState(playerLastState);
            playerLastState = string.Empty;

            isActive = false;

            player.AnimatorEffectHold = false;
        }
        else
        {
            // Event to switch to Puppeteer View.
            Script_PlayerEventsManager.PuppeteerActivate();

            playerLastState = player.State;
            if (Script_Game.Game.PuppetMaster == null)
            {
                // Should we do this on the next frame? Will cause movement to not stop immediately if we do.
                player.SetIsPuppeteerNull();
            }
            else
            {
                player.SetIsPuppeteer();
            }

            isActive = true;

            player.AnimatorEffectHold = true;

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
    }
}