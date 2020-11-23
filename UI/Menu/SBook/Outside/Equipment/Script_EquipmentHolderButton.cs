﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_EquipmentHolderButton : MonoBehaviour
{
    public Script_SBookOverviewController sBookController;

    /// <summary>
    /// called from OnClick handler
    /// </summary>
    public void OnEnter()
    {
        sBookController.EnterEquipmentView();
    }    
}
