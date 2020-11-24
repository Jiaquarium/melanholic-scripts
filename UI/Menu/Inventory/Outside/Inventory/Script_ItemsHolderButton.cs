using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ItemsHolderButton : MonoBehaviour
{
    public Script_SBookOverviewController sBookController;

    /// <summary>
    /// called from OnClick handler
    /// </summary>
    public void OnEnter()
    {
        sBookController.EnterInventoryView();
    }
}
