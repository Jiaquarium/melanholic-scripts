using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;
using UnityEngine.Playables;

/// <summary>
/// Ordering:
/// 
/// 1. Intro() from prompt (BG Music is faded out here) -> yes node NextNodeAction
///
/// 2. StartDialogue() from Timeline End Signal
///     Dialogue Flow:
///     A) Default
///         1) Interaction Node? (Myne's Mirror Manager gives this node based on # of interactions with Mirror)
///         2) Hint Node? Check cases in MynesMirrorNodesController_Mirror0 (based on Day)
///         3) Default Node (Default END)
///     
///     B) Special Conditions
///         1) Specially defined override Nodes, completely define a custom flow
/// 
/// 3. End() from Dialogue End NextNodeAction
/// 
/// Most of the cut scene will be handled by calls to PRCS Manager (Custom).
/// This director handles the shatter timeline.
/// </summary>
public class Script_MynesMirror : Script_InteractableObjectText
{
    [Tooltip("Specifies which mirror to save as")]
    public int MynesMirrorId;
    
    [SerializeField] private Script_DialogueNode defaultNode;
    [SerializeField] private Script_MynesMirrorNodesController dialogueController;

    [SerializeField] protected Script_BgThemePlayer bgThemePlayer;

    [SerializeField] protected SpriteRenderer mirrorGraphics;

    [SerializeField] protected Sprite defaultMirrorSprite;
    [SerializeField] protected Sprite brokenMirrorSprite;

    [SerializeField] protected PlayableDirector director;
    [SerializeField] protected TimelineAsset shatterTimeline;

    [SerializeField] private Script_PRCSManager PRCSManager;

    private bool isSolved;
    
    protected Script_DialogueNode HintNode
    {
        get => dialogueController?.Nodes?.Length > 0
                ? dialogueController.Nodes[0]
                : defaultNode;
        set => defaultNode = value;
    }

    private Script_DialogueNode InteractionNode
    {
        get
        {
            var specialConditionNodes = dialogueController?.SpecialConditionNodes;
            var interactionNode = Script_MynesMirrorManager.Control.ActiveNode;
            
            if (specialConditionNodes != null && specialConditionNodes.Length > 0)
                return specialConditionNodes[0];
            else if (interactionNode != null)
                return interactionNode;
            else
                return HintNode;
        }
    }

    protected virtual Sprite MirrorGraphicsSprite
    {
        get => mirrorGraphics.sprite;
        set => mirrorGraphics.sprite = value;
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        
        if (Id != MynesMirrorId)
        {
            Debug.LogError($"This Mynes Mirror {name} Id and MynesMirrorId were not matching");
        }
        Id = MynesMirrorId;
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        
        Script_MynesMirrorEventsManager.OnEndTimeline           += StartDialogue;
        Script_MynesMirrorEventsManager.OnInteractionNodeDone   += OnInteractionDialogueDone;

        var isActivated = Script_ScarletCipherManager.Control.MynesMirrorsActivationStates[MynesMirrorId];
        var isWeekend = game.RunCycle == Script_RunsManager.Cycle.Weekend;
        var isShattered = isActivated || isWeekend;
        
        HandleIsActivatedGraphics(isShattered);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        Script_MynesMirrorEventsManager.OnEndTimeline           -= StartDialogue;
        Script_MynesMirrorEventsManager.OnInteractionNodeDone   -= OnInteractionDialogueDone;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void ActionDefault()
    {
        // Although this check is repeated in base, we need to check before we do other actions.
        if (CheckDisabled())  return;

        // Fade out BG Music
        Script_BackgroundMusicManager.Control.FadeOutMed(
            () => {
                game.PauseBgMusic();
                game.PauseBgThemeSpeakers();
            },
            Const_AudioMixerParams.ExposedBGVolume
        );

        base.ActionDefault();
    }

    /// <summary>
    /// Myne's Mirror should be disabled after interacting once per run.
    /// On Weekend, always disabled.
    /// </summary>
    protected override bool CheckDisabled()
    {
        var isActivated = Script_ScarletCipherManager.Control.MynesMirrorsActivationStates[MynesMirrorId];
        var isWeekend = game.RunCycle == Script_RunsManager.Cycle.Weekend;
        var isDisabled = isActivated || isWeekend || base.CheckDisabled();

        Debug.Log($"{name} isActivated: <{isActivated}>, isWeekend: <{isWeekend}>, Base Disabled? {base.CheckDisabled()}");
        
        return isDisabled;
    }

    protected virtual void HandleIsActivatedGraphics(bool isActivated)
    {
        if (isActivated)
            MirrorGraphicsSprite = brokenMirrorSprite;
        else
            MirrorGraphicsSprite = defaultMirrorSprite;
    }

    // ------------------------------------------------------------------
    // Signal Reactions START
    /// <summary>
    /// Begin Myne's dialogue, the end of Timeline calls MynesMirrorManager to fire this event.
    /// </summary>
    public virtual void StartDialogue()
    {
        Script_DialogueManager.DialogueManager.StartDialogueNode(InteractionNode);
    }

    // Shatter Timeline
    public void RemoveMyne()
    {
        PRCSManager.CloseMynesMirrorNoFade();
        Script_ScarletCipherManager.Control.MynesMirrorsActivationStates[MynesMirrorId] = true;
    }
    
    /// <summary>
    /// Note! Call this on end dialogue node No because music will be faded out
    /// on interaction with the mirror.
    /// </summary>
    /// 
    /// Shatter Timeline
    public void FadeInBGMusic()
    {
        Script_BackgroundMusicManager.Control.UnPauseAll();
        Script_BackgroundMusicManager.Control.FadeInSlow(null, Const_AudioMixerParams.ExposedBGVolume);
    }

    // Shatter Timeline
    public void OnShatterTimelineDone()
    {
        Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
            isOpen: false,
            framing: Script_UIAspectRatioEnforcerFrame.Framing.ConstantThin,
            cb: game.ChangeStateInteract
        );
    }
    
    // ------------------------------------------------------------------
    // Next Node Actions START
    /// <summary>
    /// Shows Myne's dramatic entrance
    /// </summary>
    public void Intro()
    {
        game.ChangeStateCutScene();
        
        Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
            isOpen: true,
            framing: Script_UIAspectRatioEnforcerFrame.Framing.ConstantThin,
            cb: OnFramingAnimationDone
        );
        
        void OnFramingAnimationDone()
        {
            PRCSManager.OpenPRCSCustom(Script_PRCSManager.CustomTypes.MynesMirror);

            // BGM coroutine may still be running, so ensure to Pause in case it was stopped prematurely
            // and never called its callback to pause BGM
            game.PauseBgMusic();
            game.PauseBgThemeSpeakers();
            
            Script_BackgroundMusicManager.Control.SetVolume(1f, Const_AudioMixerParams.ExposedBGVolume);
            bgThemePlayer.gameObject.SetActive(true);
        }
    }
    
    /// <summary>
    /// Remove cut scene
    /// Called from last Dialogue Node
    /// </summary>
    public virtual void End()
    {
        // Fade out BG Theme Player
        Script_BackgroundMusicManager.Control.FadeOutMed(() => {
                bgThemePlayer.gameObject.SetActive(false);
            },
            Const_AudioMixerParams.ExposedBGVolume
        );

        director.Play(shatterTimeline);
        
        HandleIsActivatedGraphics(true);
        
        // Track the interaction count.
        Script_MynesMirrorManager.Control.InteractionCount++;
    }

    // Next Node Actions END
    // ------------------------------------------------------------------

    /// <summary>
    /// Called from Interaction Node Done event if one was used.
    /// </summary>
    public void OnInteractionDialogueDone()
    {
        Script_DialogueManager.DialogueManager.StartDialogueNode(HintNode, SFXOn: false);
    }
}
