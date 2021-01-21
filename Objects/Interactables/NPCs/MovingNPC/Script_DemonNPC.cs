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
    [SerializeField] private Script_DialogueNode _introPsychicNode;
    [SerializeField] bool _isIntroPsychicNode;
    [Tooltip("Combine the intro node with the first Psychic Node")]
    [SerializeField] bool _shouldPrependIntroNode;
    private Script_DialogueNode[] defaultNodes;
    private bool didTalkPsychic;

    public Script_DialogueNode[] PsychicNodes
    {
        get => psychicNodes;
        set => psychicNodes = value;
    }

    public Script_DialogueNode IntroPsychicNode
    {
        get => _introPsychicNode;
    }

    public bool IsIntroPsychicNodes
    {
        get => _isIntroPsychicNode;
        set => _isIntroPsychicNode = value;
    }

    private bool ShouldPrependIntroNode {
        get => _shouldPrependIntroNode;
    }

    protected override void OnEnable()
    {
        InitialDialogueState();
        
        if (psychicNodesController != null)
        {
            PsychicNodes = psychicNodesController.Nodes;
        }

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
            // If is an intro, we first append the necessary Psychic dialogue as a child of the Intro node
            // and use that node.
            if (IsIntroPsychicNodes && IntroPsychicNode != null)
            {
                HandleIntroPsychicNode();
                return;
            }
            
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

    /// <summary>
    /// Use to introduce NPC name
    /// </summary>
    private void HandleIntroPsychicNode()
    {
        Script_DialogueNode[] introNodeChild = { PsychicNodes[0] };
        
        if (ShouldPrependIntroNode)     IntroPsychicNode.data.children = introNodeChild;
        
        // Switch the current dialogue node to the intro Psychic one 
        SwitchDialogueNodes(new Script_DialogueNode[]{IntroPsychicNode}, isReset: true);
        
        IsIntroPsychicNodes = false;
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
