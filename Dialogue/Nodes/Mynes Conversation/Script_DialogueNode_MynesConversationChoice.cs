using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NOTE: Ensure to call CheckCipher from this NextNodeAction here
/// </summary>
public class Script_DialogueNode_MynesConversationChoice : Script_DialogueNode
{
    /// Set by MynesConversationChoiceParent
    public int choiceIdx;
    public Script_MynesMirror myMirror;

    /// <summary>
    /// Don't do in OnValidate since with Prefab Mode doesn't read myMirror properly
    /// </summary>
    void Awake()
    {
        if (myMirror == null)
        {
            Debug.LogError($"{name} needs a MynesMirror reference");
        }
    }

    /// ===========================================================================================
    /// NextNodeAction START 
    /// ===========================================================================================
    
    public void CheckCipher()
    {
        
    } 

    /// NextNodeAction END ========================================================================
}
