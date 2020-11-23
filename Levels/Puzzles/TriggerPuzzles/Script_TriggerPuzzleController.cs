using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// handles resetting triggers
/// </summary>
public class Script_TriggerPuzzleController : Script_PuzzleController
{
    [SerializeField] protected Script_Game game;
    [SerializeField] protected Script_Trigger[] triggers;

    protected virtual void OnValidate() {
        triggers = transform.GetChildren<Script_Trigger>();
        Dictionary<string, string> Ids = new Dictionary<string, string>{};
        foreach (Script_Trigger trigger in triggers)
        {
            string Id;
            if (Ids.TryGetValue(trigger.Id, out Id))
                Debug.LogWarning("Repeated trigger Id: " + Id);
            else
                Ids[trigger.Id] = trigger.Id;
        }
    }
    
    public virtual void TriggerActivated(string Id, Collider other)
    {

    }

    /// <summary>
    /// Trigger activated with object on initializing; used mostly for trigger stays
    /// </summary>
    public virtual void TriggerReactivated(string Id, Collider other)
    {

    }

    public virtual void TriggerDeactivated(string Id, Collider other)
    {

    }

    protected virtual bool CheckTriggersAllOccupied() { return false; }
}
