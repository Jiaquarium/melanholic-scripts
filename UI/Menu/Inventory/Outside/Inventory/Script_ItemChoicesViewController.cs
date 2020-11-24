using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_ItemChoicesInputManager))]
public class Script_ItemChoicesViewController : MonoBehaviour
{
    void Update()
    {
        GetComponent<Script_ItemChoicesInputManager>().HandleExitInput();
    }
}
