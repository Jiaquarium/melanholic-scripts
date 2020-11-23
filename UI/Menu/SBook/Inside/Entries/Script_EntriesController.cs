using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_EntriesController : Script_CanvasGroupController
{
    public Script_EntriesCanvas entriesCanvas;
    [SerializeField] private Script_EntriesViewController entriesViewController;
    
    void OnEnable()
    {
        entriesCanvas.gameObject.SetActive(true);
    }

    public override void Setup()
    {
        // start off inactive as we must enter into slots view from overview anyways
        entriesViewController.gameObject.SetActive(false);
    }
}
