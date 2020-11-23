using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Dialogue Canvas behavior ONLY FOR DIALOGUE purposes
/// </summary>
public class Script_Canvas : MonoBehaviour
{
    public Script_DialogueContinuationIcon ContinuationIcon;
    [SerializeField] private Script_DialogueBox dialogueBox;
    
    void OnValidate() {
        Script_DialogueBox[] dialogueBoxes = transform.GetComponentsInChildren<Script_DialogueBox>();
        if (dialogueBoxes.Length != 1)  Debug.LogError("You need exactly 1 dialogue box per dialogue Script_Canvas");
        
        if (dialogueBoxes.Length > 0)
            dialogueBox = dialogueBoxes[0];
    }
    
    public void Clear()
    {
        dialogueBox.Clear();
    }

    public bool IsTextEmpty()
    {
        return string.IsNullOrEmpty(GetText());
    }

    public string GetText()
    {
        return dialogueBox.GetText();
    }
    
    public void Setup()
    {
        if (ContinuationIcon != null)   ContinuationIcon.Setup();
    }
}
