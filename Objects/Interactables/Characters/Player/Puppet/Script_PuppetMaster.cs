using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// There should always be 1 puppet master when using Puppets.
/// This will handle Action inputs.
/// 
/// On Activate will store this object in Game so Player can check if it has passed control.
/// </summary>
public class Script_PuppetMaster : Script_Puppet
{
    protected override void Update()
    {
        // ------------------------------------------------------------------
        // Visuals
        HandleIsMoving();
        // ------------------------------------------------------------------

        playerMovementHandler.HandleMoveTransform();
        
        if (game.state == Const_States_Game.Interact && puppetState == PuppetStates.Active)
        {
            // Puppet Masters have their specified action override.
            playerActionHandler.HandleActionInput(FacingDirection, location);
            
            if (IsNotMovingState())
            {
                StopMovingAnimations();
            }
            else
            {
                playerMovementHandler.HandleMoveInput(isReversed);
            }
        }
        else
        {
            StopMovingAnimations();
        }
    }

    protected override void OnPuppeteerActivate()
    {
        base.OnPuppeteerActivate();

        game.PuppetMaster = this;
    }

    protected override void OnPuppeteerDeactivate()
    {
        base.OnPuppeteerDeactivate();

        if (game.PuppetMaster != null && game.PuppetMaster == this)
        {
            game.PuppetMaster = null;
        }
    }
}
