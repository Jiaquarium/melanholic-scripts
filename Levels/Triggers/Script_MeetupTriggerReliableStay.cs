using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_MeetupTriggerReliableStay : Script_Trigger
{
    private enum DetectTags
    {
        Puppet      = 0,
        Player      = 1,
    }
    
    [SerializeField] private DetectTags detectTag;
    [SerializeField] private Script_MeetupPuzzleController meetupPuzzleController;
    
    void OnTriggerEnter(Collider other)
    {
        Script_ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);
        
        if (other.tag == HandleDetectionTag(detectTag))
        {
            meetupPuzzleController.playersOnTrigger.Add(
                other.transform.parent.GetComponent<Script_Player>()
            );
        }
    }

    void OnTriggerExit(Collider other)
    {
        Script_ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);
        
        if (other.tag == HandleDetectionTag(detectTag))
        {
            meetupPuzzleController.playersOnTrigger.Remove(
                other.transform.parent.GetComponent<Script_Player>()
            );
        }
    }

    private string HandleDetectionTag(DetectTags tag)
    {
        switch (tag)
        {
            case (DetectTags.Puppet):
                return Const_Tags.Puppet;
            case (DetectTags.Player):
                return Const_Tags.Player;
            default:
                return Const_Tags.Player;
        }
    }
}
