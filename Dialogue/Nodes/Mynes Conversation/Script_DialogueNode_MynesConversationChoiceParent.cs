using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will populate children's choiceIdx
/// </summary>
public class Script_DialogueNode_MynesConversationChoiceParent : Script_DialogueNode
{
    void OnValidate()
    {
        SetChildren();
    }
    
    /// <summary>
    /// Don't do in OnValidate since with Prefab Mode doesn't read myMirror properly
    /// </summary>
    void Awake()
    {
        SetChildren();
    }

    private void SetChildren()
    {
        Script_DialogueNode_MynesConversationChoice[] choices =
            GetComponentsInChildren<Script_DialogueNode_MynesConversationChoice>(true);
        
        data.children = (Script_DialogueNode[])choices;

        for (int i = 0; i < choices.Length; i++)
        {
            choices[i].choiceIdx = i;
        }
    }
}
