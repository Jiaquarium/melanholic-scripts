using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PuppeteerEffect : Script_StickerEffect
{
    [Tooltip("Ensure this matches the PuppeteerDeactivate timeline length.")]
    [SerializeField] private float StopEffectHoldAnimationWaitTime;
    
    private bool isEffectHoldActive;
    
    public bool IsEffectHoldActive
    {
        get => isEffectHoldActive;
        private set => isEffectHoldActive = value;
    }

    public override void Effect()
    {
        if (!IsEffectHoldActive)
        {
            StartEffectHold();
        }
        else
        {
            StopEffectHold();
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

    private void StartEffectHold()
    {
        // Puppet Master will react to this event and set itself as Script_Game.Game.PuppetMaster.
        Script_PlayerEventsManager.PuppeteerActivate();
        
        player.AnimatorEffectHold = true;

        if (Script_Game.Game.PuppetMaster == null)
            player.SetIsPuppeteerNull();
        else
            player.SetIsPuppeteer();

        IsEffectHoldActive = true;
    }
    
    private void StopEffectHold()
    {
        // Puppet Master will react to this and reset Script_Game.Game.Puppeteer.
        Script_PlayerEventsManager.PuppeteerDeactivate();
        
        // If we are coming from the Puppeteer state, we want to wait until the PuppeteerDeactivate
        // Timeline is done before stopping the Player's Effect Hold animation (arms in the air).
        if (player.State == Const_States_Player.Puppeteer)
            StartCoroutine(WaitToStopEffectHoldAnimation());
        else
            SetToInteract();

        IsEffectHoldActive = false;

        IEnumerator WaitToStopEffectHoldAnimation()
        {
            yield return new WaitForSeconds(StopEffectHoldAnimationWaitTime);

            SetToInteract();
        }

        void SetToInteract()
        {
            player.AnimatorEffectHold = false;
            player.SetIsInteract();
        }
    }
}