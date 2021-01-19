using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Urselk NPCs 
/// Have Psychic Nodes mechanic
/// Psychic Nodes will reset on OnEnable
/// </summary>
public class Script_DemonNPC : Script_MovingNPC
{
    [SerializeField] private Script_DialogueNode[] psychicNodes;
    [SerializeField] Script_PsychicNodesController psychicNodesController;
    private Script_DialogueNode[] defaultNodes;
    private bool didTalkPsychic;

    public Script_DialogueNode[] PsychicNodes
    {
        get => psychicNodes;
        set => psychicNodes = value;
    }

    protected override void OnEnable()
    {
        InitialDialogueState();
        
        if (psychicNodesController != null)     PsychicNodes = psychicNodesController.Nodes;

        defaultNodes = dialogueNodes;
        base.OnEnable();
    }
    
    public override void TriggerDialogue()
    {
        HandlePsychicDuck();
        base.TriggerDialogue();
    }

    private void HandlePsychicDuck()
    {
        bool isPsychicDuckActive = Script_ActiveStickerManager.Control.IsActiveSticker(Const_Items.PsychicDuckId);
        Debug.Log($"isPsychicDuckActive: {isPsychicDuckActive}");

        if (isPsychicDuckActive)
        {
            // if previously talked default, then need to switch and reset idx
            if (!didTalkPsychic)
            {
                SwitchDialogueNodes(PsychicNodes, isReset: true);
            }
            
            didTalkPsychic = true;
        }
        else
        {
            // if previously talked psychic, then need to switch and reset idx
            if (didTalkPsychic)
            {
                SwitchDialogueNodes(defaultNodes, isReset: true);
                didTalkPsychic = false;
            }
            else
            {
                // don't reset idx if staying in defaultNodes
                Debug.Log($"using defaultNode[{dialogueIndex}]: {defaultNodes[dialogueIndex]}");
                SwitchDialogueNodes(defaultNodes, false);
            }
        }
    }

    public void SwitchPsychicNodes(Script_DialogueNode[] nodes)
    {
        PsychicNodes = nodes;
        didTalkPsychic = false;
    }

    private void InitialDialogueState()
    {
        didTalkPsychic = false;
    }

    /// For now, just start a convo if is hurt
    public override void Setup()
    {
        base.Setup();
    }
}
