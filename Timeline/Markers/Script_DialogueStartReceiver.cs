using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Script_DialogueStartReceiver : MonoBehaviour, INotificationReceiver
{
    [SerializeField] private Script_DialogueNode[] nodes;
    
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        Script_DialogueStartMarker dm = notification as Script_DialogueStartMarker;
        
        if (dm != null)
        {
            double time = origin.IsValid() ? origin.GetTime() : 0.0;
            Debug.LogFormat("Received dialogue start notification of type {0} at time {1}", dm.GetType(), time);

            Script_DialogueNode node = nodes[dm.dialogueNodeIndex];
            bool isSFXOn = !dm.isSilent;
            
            Script_DialogueManager.DialogueManager.StartDialogueNode(node, isSFXOn);
        }
    }
}
