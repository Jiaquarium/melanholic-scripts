using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activate this as Activation Track from timeline
/// </summary>
public class Script_StartDialogueNode : MonoBehaviour
{
    [SerializeField] private Script_DialogueNode node;
    [SerializeField] private bool SFXOn = false;
    
    public void Start()
    {
        Script_DialogueManager.DialogueManager.StartDialogueNode(node, SFXOn);
    }
}
