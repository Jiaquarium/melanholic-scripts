using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LevelBehavior_33 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    
    /* ======================================================================= */
    
    [SerializeField] private Transform exitParent;
    [SerializeField] private Script_Elevator elevator; /// Ref'ed by ElevatorManager

    private bool isInit = true;

    public override void Setup()
    {
        game.SetupInteractableObjectsExit(exitParent, isInit);

        isInit = false;
    }        
}