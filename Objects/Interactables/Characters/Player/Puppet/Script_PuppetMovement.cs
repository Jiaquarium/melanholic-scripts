using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PuppetMovement : Script_PlayerMovement
{
    public bool isBlockedByUnique;
    
    // Puppets should not be able to trigger exits.
    protected override bool HandleExitTile(Directions dir)
    {
        return false;
    }

    protected override void HandleAnimations()
    {
        // First check if active interaction box has unique tag blocking, if so,
        // prevent animations.
        if (isBlockedByUnique)
        {
            animator.SetBool(PlayerMovingAnimatorParam, false);
            return;
        }

        base.HandleAnimations();
    }
}
