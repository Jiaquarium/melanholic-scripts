using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Script_DoorExit : Script_InteractableObjectExit
{
    [SerializeField] protected List<Directions> exitDirections;
    
    [Tooltip("Define an action instead of exiting")]
    [SerializeField] private UnityEvent customExclusiveExitAction;

    private Directions facingDirection;
    
    public bool TryExit(Directions dir)
    {
        if (State == States.Active)
        {
            // check if dir is in exitDirections
            foreach (var exitDirection in exitDirections)
            {
                if (dir == exitDirection)
                {
                    if (customExclusiveExitAction.CheckUnityEventAction())
                    {
                        customExclusiveExitAction.Invoke();
                    }
                    else
                    {
                        facingDirection = dir;
                        Exit();
                    }

                    return true;
                }
            }
        }

        return false;
    }

    protected override void Exit()
    {
        Script_Game.Game.GetPlayer().OnExitAnimations(facingDirection);
        facingDirection = Directions.None;
        
        base.Exit();
    }

    protected override void ActionDefault()
    {
        Dev_Logger.Debug($"No action defined for {name} by interaction. Move into {name} for interaction.");
    }
    
    protected override void SetActive()
    {
        base.SetActive();
    }

    protected override void SetDisabled()
    {
        base.SetDisabled();
    }        
}
