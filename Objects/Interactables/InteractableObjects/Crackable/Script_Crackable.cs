using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_CrackableStats))]
public class Script_Crackable : Script_InteractableObject
{
    [SerializeField] private Script_InteractableObjectText textObject;
    
    protected override void AutoSetup()
    {
        base.AutoSetup();
    }

    protected override void ActionDefault()
    {
        if (textObject != null)
            textObject.HandleAction(Const_KeyCodes.InteractAction);
    }
}
