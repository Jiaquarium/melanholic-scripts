using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_InteractableObjectExit : Script_InteractableObject
{
    [SerializeField] protected Script_ExitMetadataObject exit;
    [SerializeField] private bool isExitSFXSilent = true;
    [SerializeField] private Script_Exits.ExitType exitType;
    
    public override void ActionDefault()
    {
        Debug.Log("Exit Object Interaction!");
        Exit();
    }

    protected void Exit()
    {
        Script_Game.Game.Exit(
            exit.data.level,
            exit.data.playerSpawn,
            exit.data.facingDirection,
            true,
            isExitSFXSilent,
            exitType
        );
    }
}
