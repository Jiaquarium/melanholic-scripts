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

        HandleAction(
            // Puppet Masters have their specified action overrides
            () => playerActionHandler.HandleActionInput(FacingDirection, location),
            () => playerMovementHandler.HandleMoveInput(isReversed),
            StopMoving
        );
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
