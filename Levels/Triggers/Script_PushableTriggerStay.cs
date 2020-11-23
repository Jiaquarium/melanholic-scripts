using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PushableTriggerStay : Script_Trigger
{
    public bool isOn;
    [SerializeField] private Script_Game game;
    [SerializeField] private Script_TriggerPuzzleController triggerPuzzleController;
    
    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == Const_Tags.Pushable && !isOn)
        {
            isOn = true;
            
            if (!isInitializing)   triggerPuzzleController.TriggerActivated(Id, other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == Const_Tags.Pushable)
        {
            isOn = false;
        }
    }
}
