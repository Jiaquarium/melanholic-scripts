using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager for Mynes Mirror Game Objects
/// States will be handled by Scarlet Cipher 
/// </summary>
public class Script_MynesMirrorManager : MonoBehaviour
{
    public static Script_MynesMirrorManager Control;

    [SerializeField] private int interactionCount;

    [SerializeField] private Script_DialogueNode[] interactionNodes;
    
    public int InteractionCount
    {
        get => interactionCount;
        set => interactionCount = value;
    }

    public Script_DialogueNode ActiveNode
    {
        get => interactionNodes[interactionCount];
    }
    
    // ----------------------------------------------------------------------
    // Timeline Signals
    
    /// <summary>
    /// Call this from Mynes Mirror End of Timeline so Mynes Mirrors can react to it
    /// </summary>
    public void OnEndTimeline()
    {
        Script_MynesMirrorEventsManager.EndTimeline();
    }

    // ----------------------------------------------------------------------
    // Next Node Actions

    /// <summary>
    /// Must call this from Interaction Node.
    /// </summary>
    public void EndInteractionDialogue()
    {
        Script_MynesMirrorEventsManager.EndInteractionNode();
    }

    public void Setup()
    {
        if (Control == null)
        {
            Control = this;
        }
        else if (Control != this)
        {
            Destroy(this.gameObject);
        }   
    }
}
