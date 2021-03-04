using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Differs from Player in that PlayerCopy should only handle movement.
/// </summary>
public class Script_PlayerCopy : Script_Player
{
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

        if (game.state == Const_States_Game.Interact)
        {
            if (IsNotMovingState())     StopMovingAnimations();
            else                        playerMovementHandler.HandleMoveInput();
        }
        else
        {
            StopMovingAnimations();
        }
    }    
}
