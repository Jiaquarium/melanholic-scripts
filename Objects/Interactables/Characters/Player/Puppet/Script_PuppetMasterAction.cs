using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PuppetMasterAction : Script_PlayerAction
{
    public override void HandleActionInput(Directions facingDirection, Vector3 location)
    {   
        HandleInteraction(facingDirection, location);
    }

    protected override void HandleInteraction(Directions facingDirection, Vector3 location)
    {
        if (game.GetPlayer().MyPlayerInput.actions[Const_KeyCodes.MaskEffect].WasPressedThisFrame())
        {
            PlayerStickerEffect();
        }
    }

    public override bool DetectDoorExit(Directions dir)
    {
        return false;
    }    

    /// <summary>
    /// Call the Player's Sticker Effects Controller
    /// </summary>
    private void PlayerStickerEffect()
    {
        game?.GetPlayer()?.HandlePuppeteerEffectHold();
    }
}
