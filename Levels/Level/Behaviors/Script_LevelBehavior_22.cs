using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_22 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool isUrsieCutsceneDone;
    /* ======================================================================= */
    
    [SerializeField] private Script_TrackedPushablesTriggerPuzzleController puzzleController;
    [SerializeField] private Script_LevelBehavior_24 LB24;

    [SerializeField] private Script_TileMapExitEntrance ktvRoomExit;

    [SerializeField] private Script_DemonNPC Ursie;
    [SerializeField] private Script_DialogueNode UrsieAfterUnlockNode;
    [SerializeField] private Script_DialogueNode[] psychicNodesQuestActive;
    [SerializeField] private Script_DialogueNode[] psychicNodesTalked;

    [SerializeField] private Script_DemonNPC PecheMelba;

    private bool spokenWithUrsie;
    private bool isInit = true;

    protected override void OnEnable()
    {
        if (FinishedQuest())
        {
            PuzzleCompleteState();   
        }

        bool FinishedQuest()
        {
            return LB24.IsCurrentPuzzleComplete && !isUrsieCutsceneDone;
        }
    }

    private void Awake()
    {
        ktvRoomExit.IsDisabled = true;
        
        // Setup puzzle rooms
        puzzleController.Setup();
        
        /// Set completion state after set up to ensure no race conditions and we're not
        /// resetting trackables to default position in their Setup
        if (LB24.IsCurrentPuzzleComplete)   LB24.PuzzleFinishedState();
    }
    
    // If you complete the quest Ursie and MelbaPeche will have left, leaving a note. 
    public void PuzzleCompleteState()
    {
        Ursie.gameObject.SetActive(false);
        PecheMelba.gameObject.SetActive(false);
    }

    protected override void HandleAction()
    {
        // for cutScene dialogue
        base.HandleDialogueAction();
    }

    // ------------------------------------------------------------------
    // Next Node Actions START

    public void OnDeclinedUrsieQuest()
    {
        if (spokenWithUrsie)    return;
        
        Debug.Log("OnDeclinedUrsieQuest() switching out Ursie's dialogue nodes now**********");
        Ursie.SwitchPsychicNodes(psychicNodesTalked);

        spokenWithUrsie = true;
    }
    
    public void UnlockKTVRoom()
    {
        Debug.Log("!!! PLAY UNLOCK KTV ROOM TIMELINE !!!");
        
        ktvRoomExit.IsDisabled = false;
        
        game.ChangeStateCutScene();
        // Play Timeline to unnlock KTV Room2

        // TBD: TO DELETE
        StartCoroutine(WaitForUrsieDialogue());

        IEnumerator WaitForUrsieDialogue()
        {
            yield return new WaitForSeconds(2);
            Script_DialogueManager.DialogueManager.StartDialogueNode(UrsieAfterUnlockNode);
        }
    }

    public void OnUnlockCutSceneDone()
    {
        game.ChangeStateInteract();
        Ursie.SwitchPsychicNodes(psychicNodesQuestActive);
    }
    
    // ------------------------------------------------------------------
    
    public override void Setup()
    {
        isInit = false;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_LevelBehavior_22))]
public class Script_LevelBehavior_22Tester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_LevelBehavior_22 lb = (Script_LevelBehavior_22)target;
        if (GUILayout.Button("PuzzleCompleteState()"))
        {
            lb.PuzzleCompleteState();
        }
    }
}
#endif