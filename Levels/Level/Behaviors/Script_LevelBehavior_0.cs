using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;

/// <summary>
/// Play Ids intro run timeline either after cut scene is done or upon initiation on Mon & Wed.
/// </summary>

[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_0 : Script_LevelBehavior
{
    // =======================================================================
    //  STATE DATA
    public bool didStartThought;
    public bool[] demonSpawns;
    public bool isDone;
    // =======================================================================

    private Script_PlayerMovementAnimator playerMovementAnimator;
    [SerializeField] private string hintTriggerId;
    
    [SerializeField] private Script_Hint hint; 
    
    [SerializeField] private Script_PRCS wellJustOpened; 

    [Space]
    [Header("Ids Cut Scenes")]
    [Space]
    [SerializeField] private Script_DemonNPC Ids;

    [SerializeField] private float waitBeforeIdsTalksDay1Time;

    [SerializeField] private Script_DemonNPC IdsDay2;
    [SerializeField] private Script_DialogueNode IdsDay1Node;
    [SerializeField] private Script_DialogueNode[] IdsDay2Nodes;
    [SerializeField] private Script_Trigger IdsRun2Trigger;
    [SerializeField] private Script_VCamera IdsDay2VCam;

    [SerializeField] private float pauseBeforeNewNodeTime;
    
    [Space]
    [Header("Timeline Controlled Environment")]
    [Space]
    [SerializeField] private Transform defaultTrees;
    [SerializeField] private Transform finalTrees;

    private bool didIdsRun;

    private void Start()
    {
        Dev_Logger.Debug($"{name} didStartThought: {didStartThought}");
        
        if (!didStartThought)
        {
            // On Initial Opening, pause BGM, fade in after Dialogue.
            // Note: Must set volume to 0 before Stop, or will pop on Play again
            Script_BackgroundMusicManager.Control.SetVolume(0f, Const_AudioMixerParams.ExposedBGVolume);
            game.StopBgMusic();
            
            Dev_Logger.Debug($"**** {name} starting wells cut scene ****");
            Script_PRCSManager.Control.OpenPRCSNoFade(wellJustOpened);
        }
    }
    
    protected override void OnEnable()
    {
    }

    protected override void OnDisable()
    {
    }

    public override void OnLevelInitComplete()
    {
        if (!didStartThought)
        {
            game.ChangeStateCutScene();
            /// Start Timeline fading in the well light
            wellJustOpened.PlayMyTimeline();
        }
        else
        {
            HandleIdsMonWedIntro();   
        }
    }
    
    public override bool ActivateTrigger(string Id){
        if (Id == hintTriggerId && !isDone)
        {
            hint.Show();
            return true;
        }        

        return false;
    }

    // ------------------------------------------------------------------
    /// Next Node Actions
    public void OnWellOpeningDialogueDone()
    {
        // BG Param previously set to 0f 
        game.StartBgMusicNoFade();
        var bgmManager = Script_BackgroundMusicManager.Control;
        bgmManager.SetVolume(0f, Const_AudioMixerParams.ExposedBGVolume);
        bgmManager.FadeInXSlow(null, Const_AudioMixerParams.ExposedBGVolume);
    }
    
    public void OnWellCutSceneDone()
    {
        
    }

    // IdsDay1Node
    // Level Init
    public void PlayIdsDay1RunTimeline()
    {
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(1, 1);
    }

    public void UpdateIdsName()
    {
        Script_Names.UpdateIds();
    }

    public void IdsDialogueDay2Node1()
    {
        StartCoroutine(WaitToTalk());   
        
        IEnumerator WaitToTalk()
        {
            yield return new WaitForSeconds(pauseBeforeNewNodeTime);
            Script_DialogueManager.DialogueManager.StartDialogueNode(IdsDay2Nodes[1]);
        }
    }

    public void IdsDialogueDay2Node2()
    {
        // Full Art
        Script_DialogueManager.DialogueManager.StartDialogueNode(IdsDay2Nodes[2]);
    }

    public void IdsDialogueDay2Node3()
    {
        StartCoroutine(WaitToTalk());   
        
        IEnumerator WaitToTalk()
        {
            yield return new WaitForSeconds(pauseBeforeNewNodeTime);
            Script_DialogueManager.DialogueManager.StartDialogueNode(IdsDay2Nodes[3]);
        }
    }

    public void IdsDialogueDay2Node4()
    {
        StartCoroutine(WaitToTalk());   
        
        IEnumerator WaitToTalk()
        {
            yield return new WaitForSeconds(pauseBeforeNewNodeTime);
            // No Full Art
            Script_DialogueManager.DialogueManager.StartDialogueNode(IdsDay2Nodes[4]);
        }
    }

    public void OnDay2IdsDialogueDone()
    {
        // Ids runs back & switch VCam back to Main
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(1, 3);
        Script_VCamManager.VCamMain.SwitchToMainVCam(IdsDay2VCam);
    }

    // ------------------------------------------------------------------
    // Unity Events
    
    // Trigger Day2
    public void PlayIdsWoodsRunDay2Timeline()
    {
        // On Day 2 Tues, Ids approaches Rin from behind
        if (game.IsFirstTuesday)
        {
            game.ChangeStateCutScene();
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(1, 2);
        }
    }
    
    // ------------------------------------------------------------------
    // Timeline Signals
    public void OnWellJustOpenedDone()
    {
        Script_PRCSManager.Control.HidePRCS(wellJustOpened, FadeSpeeds.Slow, () => {
            game.ChangeStateInteract();
            didStartThought = true;

            HandleIdsMonWedIntro();
        });
    }

    // Ids Intro Woods Run
    public void OnIdsTimelineDone()
    {
        game.ChangeStateInteract();
    }

    public void IdsDialogueDay2()
    {
        Script_DialogueManager.DialogueManager.StartDialogueNode(IdsDay2Nodes[0]);

        Script_VCamManager.VCamMain.SetNewVCam(IdsDay2VCam);
    }

    public void IdsDay2RunsOutTimelineDone()
    {
        game.ChangeStateInteract();
    }

    // ------------------------------------------------------------------

    // On Day 1, Ids will open with dialogue.
    // On Mon (Tutorial Run) or Wed, Ids should lead you into the Mansion.
    private void HandleIdsMonWedIntro()
    {
        if (ShouldPlayIdsIntro())
        {
            game.ChangeStateCutScene();

            if (game.IsFirstMonday)
                StartCoroutine(WaitForIdsDialogue());
            else
                PlayIdsDay1RunTimeline();
            
            didIdsRun = true;
        }

        IEnumerator WaitForIdsDialogue()
        {
            yield return new WaitForSeconds(waitBeforeIdsTalksDay1Time);

            Script_DialogueManager.DialogueManager.StartDialogueNode(IdsDay1Node);
        }
    }

    private bool ShouldPlayIdsIntro()
    {
        return !didIdsRun
            && game.RunCycle == Script_RunsManager.Cycle.Weekday
            && !game.IsFirstTuesday;
    }

    private void HandleFirstDayEmphasizeWalk()
    {
        if (game.IsFirstMonday)
            game.GetPlayer().IsEmphasizeWalk = true;
    }

    public override void Setup()
    {
        playerMovementAnimator = game.GetPlayer().MyAnimator.GetComponent<Script_PlayerMovementAnimator>();

        Ids.gameObject.SetActive(ShouldPlayIdsIntro());
        
        IdsDay2.gameObject.SetActive(false);
        IdsRun2Trigger.gameObject.SetActive(game.IsFirstTuesday);
        
        defaultTrees.gameObject.SetActive(true);
        finalTrees.gameObject.SetActive(false);

        HandleFirstDayEmphasizeWalk();
    }
}
