using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Script_MemoriesController : Script_CanvasGroupController
{
    [SerializeField] private Script_MemoriesViewController memoriesViewController;
    
    public override void Setup()
    {
        memoriesViewController.InitializeState();
    }    
}
