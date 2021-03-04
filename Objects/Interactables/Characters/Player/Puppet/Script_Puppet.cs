using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Puppet : Script_PlayerCopy
{
    protected enum PuppetStates
    {
        Inactive        = 0,
        Active          = 1
    }

    [SerializeField] protected PuppetStates puppetState;
    [SerializeField] protected bool isReversed;

    void OnEnable()
    {
        Script_PlayerEventsManager.OnPuppeteerActivate      += OnPuppeteerActivate;
        Script_PlayerEventsManager.OnPuppeteerDeactivate    += OnPuppeteerDeactivate;
    }

    protected override void OnDisable()
    {
        Script_PlayerEventsManager.OnPuppeteerActivate      -= OnPuppeteerActivate;
        Script_PlayerEventsManager.OnPuppeteerDeactivate    -= OnPuppeteerDeactivate;
    }

    protected override void Awake()
    {
        base.Awake();

        puppetState = PuppetStates.Inactive;
    }
    
    protected override void Update()
    {
        // ------------------------------------------------------------------
        // Visuals
        HandleGhostGraphics();
        // ------------------------------------------------------------------

        if (game.state == Const_States_Game.Interact && puppetState == PuppetStates.Active)
        {
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

    private void OnPuppeteerActivate()
    {
        // Wait to set active on next frame, so we don't take in the input for the current frame
        // (e.g. the input for switching to Puppeteer mode) and call PlayerStickerEffect() on same frame.
        StartCoroutine(WaitNextFrameSwitchState());
        
        IEnumerator WaitNextFrameSwitchState()
        {
            yield return null;
            
            puppetState = PuppetStates.Active;
        }
    }

    private void OnPuppeteerDeactivate()
    {
        puppetState = PuppetStates.Inactive;
    }
}
