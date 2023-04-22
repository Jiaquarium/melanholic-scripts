using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LastElevatorEffectController : MonoBehaviour
{
    [SerializeField] private Script_ElevatorManager elevatorManager;
    
    void Update()
    {
        HandleExitInput();
    }

    public void HandleExitInput()
    {
        var rewiredInput = Script_PlayerInputManager.Instance.RewiredInput;
        
        if (rewiredInput.GetButtonDown(Const_KeyCodes.RWUICancel))
        {
            if (!elevatorManager.IsFinishingLastElevatorTimeline)
                elevatorManager.LastElevatorCanceledTimeline();
        }
    }
}
