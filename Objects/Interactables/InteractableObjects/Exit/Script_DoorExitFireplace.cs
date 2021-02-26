using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fireplace exit when disabled should show the Flame to signify it is blocking the exit.
/// </summary>
public class Script_DoorExitFireplace : Script_DoorExit
{
    [SerializeField] private Script_Flame flame;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        
        HandleFlame();
    }
    
    protected override void SetActive()
    {
        base.SetActive();
        HandleFlame();
    }

    protected override void SetDisabled()
    {
        base.SetDisabled();
        HandleFlame();
    }

    private void HandleFlame()
    {
        if (State == States.Active)     flame.gameObject.SetActive(false);
        else                            flame.gameObject.SetActive(true);
    }
}
