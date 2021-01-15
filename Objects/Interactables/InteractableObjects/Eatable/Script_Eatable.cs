using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_EatableStats))]
public class Script_Eatable : Script_InteractableObject
{
    protected override void AutoSetup()
    {
        base.AutoSetup();
        GetComponent<Script_EatableStats>().Setup();
    }
}
