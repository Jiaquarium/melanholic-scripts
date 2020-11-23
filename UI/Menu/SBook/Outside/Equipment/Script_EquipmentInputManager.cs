using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_EquipmentInputManager : Script_SBookViewInputManager
{
    protected override void ExitView()
    {
        Debug.Log("ExitView caught from equipment input manager");
        
        sBookController.ExitEquipmentView();
        base.ExitView();
    }
}
