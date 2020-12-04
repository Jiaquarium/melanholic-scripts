using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.EventSystems;


[RequireComponent(typeof(Script_TimelineController))]
[RequireComponent(typeof(AudioSource))]
public class Script_LevelBehavior_27 : Script_LevelBehavior
{
    [SerializeField] private Transform exitParent;
    [SerializeField] private Script_Elevator elevator; /// Ref'ed by ElevatorManager
    private bool isInit = true;
    

    public override void Setup()
    {
        game.SetupInteractableObjectsExit(exitParent, isInit);
        
        isInit = false;
    }
}