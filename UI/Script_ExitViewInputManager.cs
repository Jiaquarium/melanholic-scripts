using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Use this as a placeholder if you handle exiting slots somewhere else.
/// </summary>
public class Script_ExitViewInputManager : MonoBehaviour
{
    [SerializeField] protected Script_UIState masterUIState;
    
    public virtual void HandleExitInput()
    {
        if (masterUIState != null && masterUIState.state == UIState.Disabled)
            return;
        
        // if (Input.GetButtonDown("Inventory") || Input.GetButtonDown("Cancel"))
        // {
        //     print("HandleExitInput()");
        // }
    }
}
