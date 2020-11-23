using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// unskippable: true
/// type: "read"
/// </summary>
[RequireComponent(typeof(Script_ItemObject))]
public class Script_ItemDialogueNode : Script_DialogueNode
{
    void Start()
    {
        // Model_DialogueSection[] sections = data.dialogue.sections;
        
        // for (int i = 0; i < sections.Length; i++)
        // {
        //     for (int j = 0; j < sections[i].lines.Length; j++)
        //     {
        //         if (string.IsNullOrEmpty(sections[i].lines[j]))     return;
                
        //         string itemName = $"<b><i>{GetComponent<Script_ItemObject>().name}</b></i>";
        //         string newString = Script_Utils.ReplaceParams(sections[i].lines[j], ItemNameParam, itemName);
        //         sections[i].lines[j] = newString;
        //     }    
        // }
    }
}
