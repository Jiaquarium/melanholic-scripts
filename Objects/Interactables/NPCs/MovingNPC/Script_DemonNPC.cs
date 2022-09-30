using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Urselk NPCs 
/// Have Psychic Nodes mechanic
/// Psychic Nodes will reset on OnEnable
/// </summary>
public class Script_DemonNPC : Script_MovingNPC
{
    // Used if we specifically need to define a Talked state. Otherwise just leave as None.
    // When state is changed to Talked, it will switch out Psychic Nodes. 
    public enum DialogueState
    {
        None        = 0,
        Talked      = 1
    }

    // Quest state to be for more complex quests where we don't want the Level Behavior to handle state.
    public enum PastQuestState
    {
        None        = 0,
        Done        = 2,
    }

    public DialogueState _dialogueState;
    public PastQuestState _pastQuestState;

    [SerializeField] private Script_DialogueNode[] psychicNodes;
    
    [Tooltip("If specified, the NPC will use these nodes if DialogueState is set to Talked")]
    [SerializeField] private Script_DialogueNode[] talkedPsychicNodes;
    
    [SerializeField] Script_PsychicNodesController psychicNodesController;
    
    [Tooltip("Initial Action on first Psychic interaction. Reset On Disable, so will fire multiple times.")]
    [SerializeField] private UnityEvent onInitialPsychicTalkAction;
    
    // ------------------------------------------------------------------
    // Intro Psychic Node
    [SerializeField] bool _isIntroPsychicNode;
    [Tooltip("Shows an intro node on first interaction")]
    [SerializeField] private Script_DialogueNode _introPsychicNode;
    [Tooltip("Make the intro node's child the first Psychic Node")]
    // This is useful if you want to show the Psychic Node no matter what
    [SerializeField] bool _shouldPrependIntroNode;
    [Tooltip("Specify a different node to append psychic nodes on intro to (e.g. when intro node has children)")]
    [SerializeField] Script_DialogueNode customPrependIntroNode;
    
    private Script_DialogueNode[] defaultNodes;
    
    // Tracks just talked Psychic.
    private bool didLastTalkPsychic;
    private bool didTalkPsychicIntro;
    private bool didTalkPrependedIntroNode;

    // ------------------------------------------------------------------
    // Refreshed Local State
    
    // Tracks if has ever talked Psychic, not just last.
    private bool hasTalkedPsychicLocal;

    public DialogueState MyDialogueState
    {
        get => _dialogueState;
        set
        {
            switch (value)
            {
                // Switch psychic nodes with Talked state ones
                case (DialogueState.Talked):
                    if (value != _dialogueState)
                        OnPsychicDialogueTalked();
                    break;
            }

            _dialogueState = value;
        }
    }
    
    public PastQuestState MyPastQuestState
    {
        get => _pastQuestState;
        set
        {
            _pastQuestState = value;

            switch (value)
            {
                // Switch psychic nodes with Done state ones
                case (PastQuestState.Done):
                    OnPastQuestDone();
                    break;
            }   
        }
    }
    
    public Script_DialogueNode[] PsychicNodes
    {
        get => psychicNodes;
        set => psychicNodes = value;
    }

    public Script_DialogueNode IntroPsychicNode => _introPsychicNode;

    public Script_DialogueNode CustomPrependIntroNode => customPrependIntroNode;

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
        base.OnEnable();
        
        if (psychicNodesController != null)
        {
            PsychicNodes = psychicNodesController.Nodes;
        }

        if (defaultNodes == null || defaultNodes.Length == 0)
        {
            defaultNodes = dialogueNodes;
            
            // Set all default nodes to glitch text
            foreach (var node in defaultNodes)
            {
                if (node != null && node.data != null)
                    node.data.isZalgofy = true;
            }
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        hasTalkedPsychicLocal = false;
    }
    
    protected override void TriggerDialogue()
    {
        HandlePsychicDuck();
        base.TriggerDialogue();
    }

    private void HandlePsychicDuck()
    {
        var psychicNPC = GetComponent<Script_PsychicNPC>();
        bool isPsychicNPC = psychicNPC != null && psychicNPC.IsAlwaysPsychic;
        
        bool isPsychicDuckActive = Script_ActiveStickerManager.Control.IsActiveSticker(Const_Items.PsychicDuckId)
            || isPsychicNPC
            || Script_Game.Game.IsPsychicRoom;
        Dev_Logger.Debug($"{name}: HandlePsychicDuck() isPsychicDuckActive: {isPsychicDuckActive}");

        if (isPsychicDuckActive)
        {
            // Switch to specified Psychic nodes if not already done.
            if (IsIntroPsychicNodes && IntroPsychicNode != null)
            {
                UseTemporaryIntroPsychicNode();
                
                // Set flag to skip the first prepended Psychic Node on next interaction.
                didTalkPrependedIntroNode = ShouldPrependIntroNode;
                didTalkPsychicIntro = true;
            }
            else if (didTalkPsychicIntro || !didLastTalkPsychic)
            {
                SwitchNewPsychicNodes();

                didTalkPsychicIntro = false;
            }
            
            if (!hasTalkedPsychicLocal)    OnInitialPsychicTalkAction();
            
            IsIntroPsychicNodes             = false;
            didLastTalkPsychic              = true;
            hasTalkedPsychicLocal           = true;
        }
        else
        {
            Script_DialogueManager.DialogueManager.HandleFirstPsychicInteractionNoDuck();
            
            // if previously talked psychic, then need to switch and reset idx
            if (didLastTalkPsychic)
            {
                Dev_Logger.Debug($"No Psychic Duck; resetting defaultNode");
                SwitchDialogueNodes(defaultNodes, isReset: true);
                didLastTalkPsychic = false;
            }
            else
            {
                // don't reset idx if staying in defaultNodes
                Dev_Logger.Debug($"No Psychic Duck; using defaultNode[{dialogueIndex}]: {defaultNodes[dialogueIndex]}");
                SwitchDialogueNodes(defaultNodes, false);
            }
        }

        // After intro node, first Psychic interaction OR just talked default
        // then need to switch out Dialogue Nodes with Psychic and reset idx.
        void SwitchNewPsychicNodes()
        {
            SwitchDialogueNodes(PsychicNodes, isReset: true);

            // Skip the first Psychic node if we prepended it already to the Intro Node.
            if (didTalkPrependedIntroNode)
            {
                Dev_Logger.Debug($"didTalkPrependedIntroNode, incrementing dialogue index from {dialogueIndex} to {dialogueIndex + 1}");

                HandleIncrementDialogueNodeIndex();
                didTalkPrependedIntroNode = false;
            }
        }

        void OnInitialPsychicTalkAction()
        {
            if (onInitialPsychicTalkAction.CheckUnityEventAction())
                onInitialPsychicTalkAction.Invoke();
        }
    }

    public void SwitchPsychicNodes(Script_DialogueNode[] nodes)
    {
        PsychicNodes = nodes;
        didLastTalkPsychic = false;
    }

    public void SwitchTalkedPsychicNodes(Script_DialogueNode[] nodes)
    {
        talkedPsychicNodes = nodes;
        didLastTalkPsychic = false;
    }

    /// <summary>
    /// Use to introduce NPC name
    /// </summary>
    private void UseTemporaryIntroPsychicNode()
    {
        Script_DialogueNode[] introNodeChild = { PsychicNodes[0] };
        
        if (ShouldPrependIntroNode)
        {
            // Attach psychic nodes to a different intro node if specified (e.g. the intro node has children
            // or links to another node via next node action)
            if (CustomPrependIntroNode != null)
                CustomPrependIntroNode.data.children = introNodeChild;
            else
                IntroPsychicNode.data.children = introNodeChild;
        }
        
        // Switch the current dialogue node to the intro Psychic one 
        SwitchDialogueNodes(new Script_DialogueNode[]{IntroPsychicNode}, isReset: true);
    }

    private void OnPsychicDialogueTalked()
    {
        if (talkedPsychicNodes?.Length > 0)
        {
            psychicNodes = talkedPsychicNodes;
            SwitchPsychicNodes(psychicNodes);
        }
    }
    
    private void OnPastQuestDone()
    {
        Dev_Logger.Debug($"{name} doing OnQuestStateDone() actions");

        // specify quest state done behavior
    }

    protected override void InitialState()
    {
        didLastTalkPsychic = false;
        
        base.InitialState();
    }
    
    /// For now, just start a convo if is hurt
    public override void Setup()
    {
        base.Setup();
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Script_DemonNPC))]
    public class Script_DemonNPCEditor : Editor
    {
        // SerializedProperty _introPsychicNodeProperty;
        // private const string introPsychicNodePropertyName = "_introPsychicNode";
        
        // void OnEnable()
        // {
        //     _introPsychicNodeProperty = serializedObject.FindProperty(introPsychicNodePropertyName);
        // }

        // public override void OnInspectorGUI()
        // {
        //     base.OnInspectorGUI();
        //     var t = target as Script_DemonNPC;

        //     if (t.IsIntroPsychicNodes)
        //     {
        //         t._shouldPrependIntroNode   = EditorGUILayout.Toggle("Should Prepend Intro Node", t._shouldPrependIntroNode);
                
        //         EditorGUILayout.PropertyField(_introPsychicNodeProperty);
        //     }

        //     // Apply changes to the serializedProperty - always do this at the end of OnInspectorGUI.
        //     serializedObject.ApplyModifiedProperties();
        // }
    }
    #endif
}