using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Urselk NPCs 
/// Have Psychic Nodes mechanic
/// </summary>
public class Script_DemonNPC : Script_MovingNPC
{
    public Script_DialogueNode[] psychicNodes;
    private Script_DialogueNode[] defaultNodes;
    private bool isPsychicNodes;

    public override void TriggerDialogue()
    {
        HandlePsychicDuck();
        base.TriggerDialogue();
    }

    private void HandlePsychicDuck()
    {
        bool isPsychicDuckActive = Script_ActiveStickerManager.Control.GetSticker()?.id == Const_Items.PsychicDuckId;
        Debug.Log($"isPsychicDuckActive: {isPsychicDuckActive}");

        if (isPsychicDuckActive)
        {
            // if previously talked default, then need to switch and reset idx
            if (!isPsychicNodes)
            {
                SwitchDialogueNodes(psychicNodes, isReset: true);
            }
            
            isPsychicNodes = true;
        }
        else
        {
            // if previously talked psychic, then need to switch and reset idx
            if (isPsychicNodes)
            {
                SwitchDialogueNodes(defaultNodes, isReset: true);
                isPsychicNodes = false;
                return;
            }
            
            // don't reset idx if staying in defaultNodes
            Debug.Log($"using defaultNode[{dialogueIndex}]: {defaultNodes[dialogueIndex]}");
            SwitchDialogueNodes(defaultNodes, false);
        }
    }

    public void SwitchPsychicNodes(Script_DialogueNode[] nodes)
    {
        psychicNodes = nodes;
        isPsychicNodes = false;
    }

    /// For now, just start a convo if is hurt

    public override void Setup()
    {
        base.Setup();
        defaultNodes = dialogueNodes;
    }
}
