using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Trigger : MonoBehaviour
{
    public string Id;
    protected bool isInitializing = true;
    
    // allows triggers to know if an object has entered it on initialization of scene
    void LateUpdate()
    {
        if (isInitializing)     isInitializing = false;
    }
}
