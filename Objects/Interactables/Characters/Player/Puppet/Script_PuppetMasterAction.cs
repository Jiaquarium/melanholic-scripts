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
        if (Input.GetButtonDown(Const_KeyCodes.Action2))
        {
            PlayerStickerEffect(facingDirection, location);
        }
    }

    /// <summary>
    /// Call the Player's Sticker Effects Controller
    /// </summary>
    private void PlayerStickerEffect(Directions facingDirection, Vector3 location)
    {
        Script_Game.Game?.GetPlayer()?.GetComponent<Script_PlayerAction>()?
                .StickerEffectsController.Effect(facingDirection);
    }
}
