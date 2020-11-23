using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_ExitViewInputManager))]
public class Script_SavedGameSubmenuController : MonoBehaviour
{
    void Update()
    {
        GetComponent<Script_ExitViewInputManager>().HandleExitInput();
    }
}
