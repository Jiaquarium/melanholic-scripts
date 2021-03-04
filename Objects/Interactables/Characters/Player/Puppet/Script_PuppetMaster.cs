using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// There should always be 1 puppet master when using Puppets.
/// This will handle Action inputs.
/// </summary>
public class Script_PuppetMaster : Script_Puppet
{
    [SerializeField] private Script_VCamera puppeteerVCam;
    
    protected override void Update()
    {
        // ------------------------------------------------------------------
        // Visuals
        HandleGhostGraphics();
        // ------------------------------------------------------------------

        if (game.state == Const_States_Game.Interact && puppetState == PuppetStates.Active)
        {
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

        Script_VCamManager.VCamMain.SetNewVCam(puppeteerVCam);
    }

    protected override void OnPuppeteerDeactivate()
    {
        base.OnPuppeteerDeactivate();

        Script_VCamManager.VCamMain.SwitchToMainVCam(puppeteerVCam);
    }
}
