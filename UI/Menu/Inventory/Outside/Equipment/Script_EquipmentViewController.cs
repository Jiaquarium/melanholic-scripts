using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Should only be active when focused in on equipment view
/// </summary>
public class Script_EquipmentViewController : Script_SBookViewController
{
    public override void Setup()
    {
        base.Setup();
        gameObject.SetActive(false);
    }
}
