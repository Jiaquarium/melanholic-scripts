using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Use with DialogueStartMarker for Dialogue Nodes in between cut scenes.
/// </summary>
public class Script_DialogueStartReceiver : MonoBehaviour, INotificationReceiver
{
    [SerializeField] private Script_DialogueNode[] nodes;
    
    private PlayableDirector director;

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        Script_DialogueStartMarker dm = notification as Script_DialogueStartMarker;
        
        if (dm != null)
        {
            double time = origin.IsValid() ? origin.GetTime() : 0.0;
            Debug.LogFormat("Received dialogue start notification of type {0} at time {1}", dm.GetType(), time);

            Script_DialogueNode node = nodes[dm.dialogueNodeIndex];
            bool isSFXOn = !dm.isSilent;
            
            Script_DialogueManager dialogueManager = Script_DialogueManager.DialogueManager;
            dialogueManager.StartDialogueNode(node, isSFXOn);

            if (dm.isPauseTimeline)
            {
                director = (origin.GetGraph().GetResolver() as PlayableDirector);
                director.Pause();
            }
        }
    }

    // Can call from Next Node Action or from Timeline
    public void UnpauseTimeline()
    {
        Script_DialogueManager dialogueManager = Script_DialogueManager.DialogueManager;
        dialogueManager.SetActive(true);

        director.Play();
        director = null;
    }
}
