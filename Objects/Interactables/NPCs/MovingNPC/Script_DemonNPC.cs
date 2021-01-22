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
    [SerializeField] private bool didTalkPsychic;
    [SerializeField] private bool didTalkPrependedIntroNode;

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
            // If is an Intro, we replace the current Dialogue Nodes with the Intro Node
            if (IsIntroPsychicNodes && IntroPsychicNode != null)
            {
                HandleIntroPsychicNode();
                IsIntroPsychicNodes = false;
                // Set flag so on next interaction we know we need to skip the first Psychic Node
                didTalkPrependedIntroNode = ShouldPrependIntroNode;
                return;
            }
            
            // If previously talked default or intro Psychic node, then need to switch and reset idx.
            if (!didTalkPsychic)
            {
                SwitchDialogueNodes(PsychicNodes, isReset: true);

                // Skip the first Psychic node if we prepended it already to the Intro Node.
                if (didTalkPrependedIntroNode)
                {
                    HandleIncrementDialogueNodeIndex();
                    didTalkPrependedIntroNode = false;
                }
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
        
        if (ShouldPrependIntroNode)
        {
            IntroPsychicNode.data.children = introNodeChild;
        }
        
        // Switch the current dialogue node to the intro Psychic one 
        SwitchDialogueNodes(new Script_DialogueNode[]{IntroPsychicNode}, isReset: true);
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
