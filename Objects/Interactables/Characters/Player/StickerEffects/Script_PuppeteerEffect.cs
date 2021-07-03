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
        }
        else
        {
            // Event to switch to Puppeteer View.
            Script_PlayerEventsManager.PuppeteerActivate();

            playerLastState = player.State;
            if (Script_Game.Game.PuppetMaster == null)  StartCoroutine(WaitNextFrameSwitchState());
            else                                        player.SetIsPuppeteer();

            isActive = true;
        }

        IEnumerator WaitNextFrameSwitchState()
        {
            yield return null;
            
            player.SetIsPuppeteerNull();
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
