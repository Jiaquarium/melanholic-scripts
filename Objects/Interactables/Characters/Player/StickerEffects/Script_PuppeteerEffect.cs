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
            player.SetState(playerLastState);
            playerLastState = string.Empty;
            
            Script_PlayerEventsManager.PuppeteerDeactivate();

            isActive = false;
        }
        else
        {
            playerLastState = player.State;
            player.SetIsPuppeteer();

            Script_PlayerEventsManager.PuppeteerActivate();

            isActive = true;
        }
    }

    protected override void OnEquip()
    {
        base.OnEquip();
    }

    protected override void OnUnequip()
    {
        base.OnUnequip();
    }
}
