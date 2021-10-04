using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerTrueReflectionMovement : Script_PlayerReflectionMovement
{
    private const string IsMovingBool = "PlayerMoving";

    protected override void SetIsMoving(bool isMoving)
    {
        animator.SetBool(IsMovingBool, isMoving);
    }
}
