using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ElevatorBehavior_BayState : Script_ElevatorBehavior
{
    public override void Effect()
    {
        Script_Game.Game.SetBayV1ToSaveState(Script_LevelBehavior_33.State.Save);
    }
}
