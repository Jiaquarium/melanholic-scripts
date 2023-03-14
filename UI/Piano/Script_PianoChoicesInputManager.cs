using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PianoChoicesInputManager : Script_ExitViewInputManager
{
    [SerializeField] private Script_PianoManager pianoManager;
    
    public override void HandleExitInput()
    {
        if (Script_PlayerInputManager.Instance.RewiredInput.GetButtonDown(Const_KeyCodes.RWUICancel))
        {
            pianoManager.ExitPianoChoices();
        }
    }
}
