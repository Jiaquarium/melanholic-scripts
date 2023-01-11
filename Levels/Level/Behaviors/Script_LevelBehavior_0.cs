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
    public bool didStartThoughtSea;
    public bool[] demonSpawns;
    public bool isDone;
    // =======================================================================

    private Script_PlayerMovementAnimator playerMovementAnimator;
    [SerializeField] private string hintTriggerId;
    
    [SerializeField] private Script_Hint hint; 
    
    [Space]
    [Header("Opening PRCS")]
    [Space]
    
    [SerializeField] private Script_PRCS wellJustOpened;
    [SerializeField] private Script_PRCS seaVignette;
    [SerializeField] private Script_BgThemePlayer seaBgThemePlayer;
    [SerializeField] private Script_TransitionManager transitionManager;
    [SerializeField] private Script_PostProcessingSettings postProcessingSettings;
    [SerializeField] private Script_GlitchFXManager glitchFXManager;
    [SerializeField] private float seaBgmFadeInTime;
    [SerializeField] private float seaBgmFadeOutTime;
    [SerializeField] private float waitBeforeSeaBgmTime;
    [SerializeField] private float waitBeforeSeaPRCSTime;
    [SerializeField] private float waitAfterSeaVignetteDoneTime;

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
        Dev_Logger.Debug($"{name} didStartThought: {didStartThought}; didStartThoughtSea: {didStartThoughtSea}");
        
        // Handle Well PRCS
        if (
            !didStartThought
            && game.RunCycle == Script_RunsManager.Cycle.Weekday
        )
        {
            // On Initial Opening, pause BGM, fade in after Dialogue.
            // Note: Must set volume to 0 before Stop, or will pop on Play again
            Script_BackgroundMusicManager.Control.SetVolume(0f, Const_AudioMixerParams.ExposedBGVolume);
            game.StopBgMusic();
            
            Dev_Logger.Debug($"**** {name} starting wells cut scene ****");
            Script_PRCSManager.Control.OpenPRCSNoFade(wellJustOpened);
        }
        // Handle Sea PRCS
        else if (
            !didStartThoughtSea
            && game.RunCycle == Script_RunsManager.Cycle.Weekend
        )
        {
            Script_BackgroundMusicManager.Control.SetVolume(0f, Const_AudioMixerParams.ExposedBGVolume);
            game.StopBgMusic();
            
            // Put up frame
            Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
                isOpen: true,
                framing: Script_UIAspectRatioEnforcerFrame.Framing.SeaVignette,
                isNoAnimation: true
            );
            
            // Timeline will take over control of Under Dialogue Fader afterwards
            transitionManager.TimelineBlackScreen(isOver: false);
            
            StartCoroutine(WaitToStartSeaBgm());
            StartCoroutine(WaitToPlaySeaPRCS());
        }
        
        IEnumerator WaitToStartSeaBgm()
        {
            yield return new WaitForSeconds(waitBeforeSeaBgmTime);
            
            seaBgThemePlayer.gameObject.SetActive(false);
            seaBgThemePlayer.FadeInPlay(null, seaBgmFadeInTime);
        }
        
        IEnumerator WaitToPlaySeaPRCS()
        {
            yield return new WaitForSeconds(waitBeforeSeaPRCSTime);

            Script_PRCSManager.Control.OpenPRCSCustom(Script_PRCSManager.CustomTypes.Sea);
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
        if (
            !didStartThought
            && game.RunCycle == Script_RunsManager.Cycle.Weekday
        )
        {
            game.ChangeStateCutScene();
            /// Start Timeline fading in the well light
            wellJustOpened.PlayMyTimeline();
        }
        // After exit canvas fades out in Script_Exits.defaultLevelFadeInTime time
        // begin handling the sea vignette PRCS and black screen under dialogue
        else if (
            !didStartThoughtSea
            && game.RunCycle == Script_RunsManager.Cycle.Weekend
        )
        {
            game.ChangeStateCutScene();

            // seaVignette coroutine should already be running here from Start() so
            // no need to play the PRCS here.
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

    public void OnWellCutSceneDone() {}
    
    public void OnSeaVignetteDialogueDone()
    {
        seaBgThemePlayer.FadeOutStop(null, seaBgmFadeOutTime);
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

    // SeaVignetteTimeline Start
    public void OnSeaVignetteStart()
    {
        // Set Vignette; from testing this should be set on same frame as signal but to be safe,
        // wait 1 frame in timeline to ensure it updates
        postProcessingSettings.Vignette(true);
    }
    
    // SeaVignetteTimeline GlitchOn
    public void StartGlitch()
    {
        glitchFXManager.SetXHigh();
        glitchFXManager.SetBlend(1f);
    }
    
    // SeaVignetteTimeline End
    public void OnSeaVignetteDone()
    {
        Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
            isOpen: false,
            framing: Script_UIAspectRatioEnforcerFrame.Framing.SeaVignette,
            isNoAnimation: true
        );
        
        Script_PRCSManager.Control.ClosePRCSCustom(Script_PRCSManager.CustomTypes.Sea, () => {
            glitchFXManager.SetBlend(0f);
            postProcessingSettings.Vignette(false);

            game.StartBgMusicNoFade();
            var bgmManager = Script_BackgroundMusicManager.Control;
            bgmManager.SetVolume(0f, Const_AudioMixerParams.ExposedBGVolume);
            bgmManager.FadeInXSlow(null, Const_AudioMixerParams.ExposedBGVolume);
            
            StartCoroutine(WaitToInteract());
            
            didStartThoughtSea = true;
        });
        
        IEnumerator WaitToInteract()
        {
            yield return new WaitForSeconds(waitAfterSeaVignetteDoneTime);

            game.ChangeStateInteract();
            HandleIdsMonWedIntro();
        }        
    }

    // Ids Intro Woods Run
    public void OnIdsTimelineDone()
    {
        game.ChangeStateInteract();
    }

    // Ids Day 2 Entrance Timeline Done
    public void IdsDialogueDay2()
    {
        IdsDay2.MyAnimator.ResetAnimator(0);
        
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
