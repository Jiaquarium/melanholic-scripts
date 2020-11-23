using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// responds to if Pushable is pushed into it 
/// </summary>
public class Script_PushableTriggerEnter : Script_Trigger
{
    [SerializeField] private Script_Game game;
    [SerializeField] private Script_TriggerPuzzleController triggerController;
    [SerializeField] bool isDeactivatePushable;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == Const_Tags.Pushable)
        {
            Debug.Log($"trigger activated: {Id}, other col: {other.transform.parent}");
            triggerController.TriggerActivated(Id, other);

            if (isDeactivatePushable)
                other.transform.parent.GetComponent<Script_Pushable>().HideAfterMove();  
        }
    }
}
