using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_FaceOffCanvasGroupController : Script_CanvasGroupController
{
    [SerializeField] private Script_TeletypeDialogueContainer[] dialogues;

    void OnValidate()
    {
        dialogues = GetComponentsInChildren<Script_TeletypeDialogueContainer>(true);
    }

    void Awake()
    {
        dialogues = GetComponentsInChildren<Script_TeletypeDialogueContainer>(true);
    }

    public override void InitialState()
    {
        base.InitialState();

        foreach (var dialogue in dialogues)
        {
            dialogue.InitialState();
            dialogue.gameObject.SetActive(true);
        }
    }
}
