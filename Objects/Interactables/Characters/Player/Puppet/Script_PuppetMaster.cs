using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PuppetMaster : Script_PlayerCopy
{
    private enum PuppetMasterStates
    {
        Inactive        = 0,
        Active          = 1
    }

    [SerializeField] private PuppetMasterStates puppetMasterState;

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

        puppetMasterState = PuppetMasterStates.Inactive;
    }
    
    protected override void Update()
    {
        // ------------------------------------------------------------------
        // Visuals
        if (isPlayerGhostMatchSortingLayer)
        {
            playerMovementHandler.PlayerGhostSortOrder(
                Script_Utils.FindComponentInChildWithTag<SpriteRenderer>(
                    this.gameObject, Const_Tags.PlayerAnimator
                ).sortingOrder
            );
        }
        playerMovementHandler.TrackPlayerGhost();
        // ------------------------------------------------------------------

        if (game.state == Const_States_Game.Interact && puppetMasterState == PuppetMasterStates.Active)
        {
            playerActionHandler.HandleActionInput(FacingDirection, location);
            
            if (IsNotMovingState())     StopMovingAnimations();
            else                        playerMovementHandler.HandleMoveInput();
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
            
            puppetMasterState = PuppetMasterStates.Active;
        }
    }

    private void OnPuppeteerDeactivate()
    {
        puppetMasterState = PuppetMasterStates.Inactive;
    }
}
