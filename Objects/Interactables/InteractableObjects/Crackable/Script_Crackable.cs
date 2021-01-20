using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_CrackableStats))]
public class Script_Crackable : Script_InteractableObject
{
    protected override void AutoSetup()
    {
        base.AutoSetup();
        GetComponent<Script_CrackableStats>().Setup();
    }
}
