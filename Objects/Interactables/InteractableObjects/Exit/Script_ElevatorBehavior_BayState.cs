using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ElevatorBehavior_BayState : Script_ElevatorBehavior
{
    [SerializeField] private Script_LevelBehavior_33 bayV1Behavior;
    
    public override void Effect()
    {
        bayV1Behavior.Behavior = Script_LevelBehavior_33.State.Save;
    }
}
