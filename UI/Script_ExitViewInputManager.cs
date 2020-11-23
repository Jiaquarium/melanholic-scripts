using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
