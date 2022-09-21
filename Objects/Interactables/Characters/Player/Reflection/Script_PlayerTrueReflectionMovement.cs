using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerTrueReflectionMovement : Script_PlayerReflectionMovement
{
    private static readonly int IsMovingBool = Animator.StringToHash("PlayerMoving");

    protected override void SetIsMoving(bool isMoving)
    {
        animator.SetBool(IsMovingBool, isMoving);
    }
}
