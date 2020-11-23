using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_PlayerReflectionInteractiveMovement))]
[RequireComponent(typeof(Script_InteractionBoxController))]
public class Script_PlayerReflectionInteractive : Script_PlayerReflection
{
    public bool CanMove()
    {
        return GetComponent<Script_PlayerReflectionInteractiveMovement>().CanMove();
    }

    /// Will only push first pushable currently
    public void TryPushPushable(Directions dir)
    {
        Directions myFaceDirection = ToOppositeDirectionZ(dir);
        
        List<Script_Pushable> pushables = GetComponent<Script_InteractionBoxController>()
            .GetPushables(myFaceDirection);
        if (pushables.Count > 0) pushables[0].Push(myFaceDirection);
    }
}
