using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_EntriesController : Script_CanvasGroupController
{
    public Script_EntriesCanvas SBookInsideCanvas;
    
    void OnEnable()
    {
        SBookInsideCanvas.gameObject.SetActive(true);
    }
}
