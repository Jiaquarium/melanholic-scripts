using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SBookViewInputManager : Script_ExitViewInputManager
{
    [SerializeField] protected Script_EventSystemLastSelected eventSystemLastSelected;
    protected Script_SBookOverviewController sBookController;

    protected virtual void OnEnable()
    {
        Script_MenuEventsManager.OnExitSubmenu += ExitView;    
    }

    protected virtual void OnDisable()
    {
        Script_MenuEventsManager.OnExitSubmenu -= ExitView;
    }
    
    public override void HandleExitInput()
    {
        if (masterUIState != null && masterUIState.state == UIState.Disabled)
            return;

        if (Input.GetButtonDown("Inventory") || Input.GetButtonDown("Cancel"))
        {
            print("HandleExitInput()");
            Script_MenuEventsManager.ExitSubmenu();
        }
    }

    protected virtual void ExitView()
    {
        eventSystemLastSelected.UpdateCurrentSelected();   
    }

    public void Setup(
        Script_SBookOverviewController _SBookController
    )
    {
        sBookController = _SBookController;
    }
}
