using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PuppetMovement : Script_PlayerMovement
{
    // Puppets should not be able to trigger exits.
    protected override bool HandleExitTile(Directions dir)
    {
        return false;
    }
}
