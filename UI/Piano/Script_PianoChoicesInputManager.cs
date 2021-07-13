using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PianoChoicesInputManager : Script_ExitViewInputManager
{
    [SerializeField] private Script_PianoManager pianoManager;
    
    public override void HandleExitInput()
    {
        if (Input.GetButtonDown(Const_KeyCodes.Cancel))
        {
            pianoManager.ExitPianoChoices();
        }
    }
}
