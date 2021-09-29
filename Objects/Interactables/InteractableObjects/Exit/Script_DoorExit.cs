using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_DoorExit : Script_InteractableObjectExit
{
    [SerializeField] protected List<Directions> exitDirections;
    
    public bool TryExit(Directions dir)
    {
        if (State == States.Active)
        {
            // check if dir is in exitDirections
            foreach (var exitDirection in exitDirections)
            {
                if (dir == exitDirection)
                {
                    Exit();
                    return true;
                }
            }
        }

        return false;
    }

    protected override void ActionDefault()
    {
        Debug.Log($"No action defined for {name} by interaction. Move into {name} for interaction.");
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
