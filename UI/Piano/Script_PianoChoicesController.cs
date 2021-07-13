using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_PianoChoicesInputManager))]
public class Script_PianoChoicesController : MonoBehaviour
{
    private Script_PianoChoicesInputManager inputManager;
    
    void Awake()
    {
        inputManager = GetComponent<Script_PianoChoicesInputManager>();
    }
    
    void Update()
    {
        inputManager.HandleExitInput();
    }
}
