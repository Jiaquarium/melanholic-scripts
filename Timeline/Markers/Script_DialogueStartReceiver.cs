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
    [SerializeField] private bool isControlAnimators;
    [SerializeField] private Animator[] animators;
    
    private PlayableDirector director;

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        Script_DialogueStartMarker dm = notification as Script_DialogueStartMarker;
        
        if (dm != null)
        {
            double time = origin.IsValid() ? origin.GetTime() : 0.0;
            // Dev_Logger.Debug($"Received dialogue start notification of type {dm.GetType()} at time {time}");

            Script_DialogueNode node = nodes[dm.dialogueNodeIndex];
            bool isSFXOn = !dm.isSilent;
            
            Script_DialogueManager dialogueManager = Script_DialogueManager.DialogueManager;
            dialogueManager.StartDialogueNode(node, isSFXOn);

            if (dm.isPauseTimeline)
            {
                if (isControlAnimators)
                {
                    foreach (var animator in animators)
                        animator.enabled = false;
                }
                
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

        if (isControlAnimators)
        {
            foreach (var animator in animators)
                animator.enabled = true;
        }
    }
}
