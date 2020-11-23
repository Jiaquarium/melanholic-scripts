using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SavePoint : Script_Interactable
{
    public Script_Game game;
    public SpriteRenderer spriteRenderer;
    public Script_DialogueManager dm;
    public Script_DialogueNode dialogueNode;
    public Model_SavePointData data;

    public void HandleAction(string action)
    {
        if (action == Const_KeyCodes.Action1)
        {
            if (isDialogueCoolDown)     return;
            SaveDialogue();
        }
    }

    public Model_SavePointData GetData()
    {
        return data;
    }

    void SaveDialogue()
    {
        if (!game.GetPlayerIsTalking())
        {
            dm.StartDialogueNode(dialogueNode, SFXOn: true, null, this);
        }
        else
        {
            if (dm.IsDialogueSkippable())   dm.SkipTypingSentence();
            else                            dm.ContinueDialogue();
        }
    }

    void AdjustRotation()
    {
        spriteRenderer.transform.forward = Camera.main.transform.forward;
    }

    public void Setup()
    {
        AdjustRotation();
    }
}
