using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_StartScreenController : MonoBehaviour
{
    void Update()
    {
        GetComponent<Script_StartScreenInputManager>().HandleEnterInput();
    }
}
