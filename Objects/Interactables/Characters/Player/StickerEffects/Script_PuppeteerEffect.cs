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

    void OnEnable()
    {
        Script_GameEventsManager.OnLevelBeforeDestroy += ForceStopEffectHold;
    }

    void OnDisable()
    {
        Script_GameEventsManager.OnLevelBeforeDestroy -= ForceStopEffectHold;
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

    /// <summary>
    /// On level tear down event, remove the Puppeteer effect if it's active. Ideally, this won't
    /// be called, but in the case it does, this will prevent the game from breaking.
    /// </summary>
    public void ForceStopEffectHold()
    {
        if (IsEffectHoldActive)
        {
            SetToInteract();
            isEffectHoldActive = false;
            Script_Game.Game.GetPlayer().SetBuffEffectActive(false);
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
        var game = Script_Game.Game;
        bool inAOE = true;
        
        player.AnimatorEffectHold = true;
        
        // Check AOE for specified Level Behaviors
        if (game.levelBehavior == game.UrsaSaloonHallwayBehavior)
            inAOE = game.UrsaSaloonHallwayBehavior.CheckInsidePuppeteerAreaOfEffect();

        // Puppet Master will react to this event and set itself as Script_Game.Game.PuppetMaster.
        // PuzzlePuppetController reacts to start timeline.
        if (inAOE)
            Script_PlayerEventsManager.PuppeteerActivate();
        
        if (game.PuppetMaster == null || !inAOE)
            player.SetIsPuppeteerNull();
        else
            player.SetIsPuppeteer();

        IsEffectHoldActive = true;
    }
    
    private void StopEffectHold()
    {
        var game = Script_Game.Game;
        bool inAOE = true;
        
        // Check AOE for specified Level Behaviors
        if (game.levelBehavior == game.UrsaSaloonHallwayBehavior)
            inAOE = game.UrsaSaloonHallwayBehavior.CheckInsidePuppeteerAreaOfEffect();

        // Puppet Master will react to this and reset Script_Game.Game.Puppeteer.
        if (inAOE)
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
    }

    private void SetToInteract()
    {
        player.AnimatorEffectHold = false;
        player.SetIsInteract();
    }
}