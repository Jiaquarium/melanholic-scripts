using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Quest completion: talk to Ursie after finishing LB24 puzzle
/// </summary>
[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_29 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool gotPsychicDuck;
    public bool updatedUrsieName;
    public bool questActive;
    public bool questComplete;
    public bool isSaloonLocked = true;

    /* ======================================================================= */
    [SerializeField] private Script_MovingNPC Ursie;
    [SerializeField] private Script_StickerObject PsychicDuck;
    [SerializeField] private Script_DialogueNode[] psychicNodesTalked;
    [SerializeField] private Script_DialogueNode[] psychicNodesQuestActive;
    [SerializeField] private Script_DialogueNode[] psychicNodesOnQuestDone;
    [SerializeField] private Script_DialogueNode[] psychicNodesEndState;
    [SerializeField] private Script_DialogueNode onUnlockedSaloonNode;
    [SerializeField] private Script_DialogueNode onUnlockedBallroomNode;
    [SerializeField] private PlayableDirector UrsieDirector;
    [SerializeField] private float unlockSaloonWaitTime;
    [SerializeField] private float unlockBallroomWaitTime;
    [SerializeField] private Script_LevelBehavior_24 LB24;
    [SerializeField] private Script_DoorLock doorCage;
    private bool spokenWithUrsie;
    private bool isInit = true;

    protected override void OnEnable()
    {
        UrsieDirector.stopped += OnUrsieDirectorDone;    
    }

    protected override void OnDisable()
    {
        UrsieDirector.stopped -= OnUrsieDirectorDone;    
    }

    protected override void Update()
    {
        base.HandleDialogueAction();
    }

    /// <summary>
    /// Called from NextNodeAction()
    /// </summary>
    public void GivePsychicDuck()
    {
        if (gotPsychicDuck)     return;

        game.HandleItemReceive(PsychicDuck);
        gotPsychicDuck = true;
    }

    /// <summary>
    /// Called from BeforeNodeAction()
    /// </summary>
    public void UpdateUrsieName()
    {
        if (updatedUrsieName)    return;
        
        Debug.Log("Updating Ursie name");
        Script_Names.UpdateUrsie();
        updatedUrsieName = true;
    }

    private void SwitchUrsieDialogueQuestActive()
    {
        Script_DemonNPC demonNPC = (Script_DemonNPC)Ursie;
        demonNPC.SwitchPsychicNodes(psychicNodesQuestActive);
    }

    private void SwitchUrsieDialogueOnQuestDone()
    {
        Script_DemonNPC demonNPC = (Script_DemonNPC)Ursie;
        demonNPC.SwitchPsychicNodes(psychicNodesOnQuestDone);
    }

    private void SwitchUrsieDialogueEndState()
    {
        Script_DemonNPC demonNPC = (Script_DemonNPC)Ursie;
        demonNPC.SwitchPsychicNodes(psychicNodesEndState);
    }

    /// <summary>
    /// Called from NextNodeAction()
    /// after dialogue is done
    /// </summary>
    public void OnFinishedTalking()
    {
        if (spokenWithUrsie)    return;
        
        Debug.Log("OnFinishedTalking() switching out Ursie's dialogue nodes now...");
        Script_DemonNPC specter = (Script_DemonNPC)Ursie;
        specter.SwitchPsychicNodes(psychicNodesTalked);
        
        spokenWithUrsie = true;
    }

    /// <summary>
    /// Called from NextNodeAction() from "yes" node after prompt
    /// </summary>
    public void UnlockSaloonCutScene()
    {
        game.ChangeStateCutScene();
        StartCoroutine(WaitToUnlockSaloon());

        IEnumerator WaitToUnlockSaloon()
        {
            yield return new WaitForSeconds(unlockSaloonWaitTime);
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);
        }
    }

    /// <summary>
    /// Called from NextNodeAction() from end quest nodes
    /// </summary>
    public void UnlockBallroomCutScene()
    {
        game.ChangeStateCutScene();
        StartCoroutine(WaitToUnlockBallroom());

        IEnumerator WaitToUnlockBallroom()
        {
            yield return new WaitForSeconds(unlockBallroomWaitTime);
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 1);
        }
    }

    private void OnUrsieDirectorDone(PlayableDirector aDirector)
    {
        if (aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[0])
            Script_DialogueManager.DialogueManager.StartDialogueNode(onUnlockedSaloonNode, false);

        else if (aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[1])
            Script_DialogueManager.DialogueManager.StartDialogueNode(onUnlockedBallroomNode, false);
    }

    /// <summary>
    /// NextNodeAction START ===========================================================================
    /// </summary>
    /// Called from NextNodeAction() from onUnlockedSaloonNode
    /// </summary>
    public void OnEndUnlockSaloonScene()
    {
        game.ChangeStateInteract();
        Ursie.FaceDefaultDirection();
        StartQuest();
    }

    /// <summary>
    /// Called from NextNodeAction() from onUnlockedBallroomNode
    /// </summary>
    public void OnEndUnlockBallroomScene()
    {
        game.ChangeStateInteract();
        Ursie.FaceDefaultDirection();
    }
    /// <summary>
    /// NextNodeAction END ===========================================================================
    /// </summary>

    private void StartQuest()
    {
        SwitchUrsieDialogueQuestActive();
        questActive = true;
    }

    /// <summary>
    /// After interacting with Ursie after finishing LB24 puzzle,
    /// quest is then done
    /// </summary>
    private void UrsieDialogueEndQuest()
    {
        SwitchUrsieDialogueOnQuestDone();
        questActive = false;
    }

    /// <summary>
    /// After quest is done, use these nodes
    /// 1) Called on NextNodeAction() after unlocking sequence
    /// 2) Also on entrance if quest is done
    /// </summary>
    public void UrsieDialogueEndState()
    {
        SwitchUrsieDialogueEndState();
        questComplete = true;
    }

    public override void Setup()
    {
        if (questActive)
        {
            StartQuest();
        }
        
        if (!questComplete)
        {
            if (LB24.IsCurrentPuzzleComplete)  UrsieDialogueEndQuest();
        }
        else
        {
            UrsieDialogueEndState();
        }

        game.SetupMovingNPC(Ursie, isInit);
        doorCage.Setup();
    }
}