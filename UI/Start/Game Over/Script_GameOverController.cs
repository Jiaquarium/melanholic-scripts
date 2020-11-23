using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_GameOverController : MonoBehaviour
{
    public enum DeathTypes
    {
        Default,
        ThoughtsOverload,
        Impaled,
        DemoOver
    }
    
    void Update()
    {
        GetComponent<Script_GameOverInputManager>().HandleEnterInput();
    }
}
