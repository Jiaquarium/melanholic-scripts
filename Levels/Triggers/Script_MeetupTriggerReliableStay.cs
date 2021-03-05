using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_MeetupTriggerReliableStay : Script_TriggerReliableStay
{   
    [SerializeField] private Script_MeetupPuzzleController meetupPuzzleController;
    
    protected override void OnEnter(Collider other)
    {
        meetupPuzzleController.playersOnTrigger.Add(
            other.transform.parent.GetComponent<Script_Player>()
        );
    }

    protected override void OnExit(Collider other)
    {
        meetupPuzzleController.playersOnTrigger.Remove(
            other.transform.parent.GetComponent<Script_Player>()
        );
    }
}
