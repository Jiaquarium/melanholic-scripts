﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Ordering:
/// 
/// 1. Intro() from prompt (BG Music is faded out here) -> yes node NextNodeAction
/// 2. StartDialogue() from Timeline End Signal
/// 3. End() from Dialogue End NextNodeAction
/// </summary>
public class Script_MynesMirror : Script_InteractableObjectText
{
    [Tooltip("Specifies which mirror to save as")]
    public int MynesMirrorId;
    
    [SerializeField] private Script_DialogueNode _MynesConversationNode;
    [SerializeField] private Script_MynesMirrorNodesController dialogueController;

    [SerializeField] protected Script_BgThemePlayer bgThemePlayer;

    [SerializeField] protected SpriteRenderer mirrorGraphics;

    [SerializeField] private Sprite defaultMirrorSprite;
    [SerializeField] protected Sprite brokenMirrorSprite;

    private bool isSolved;
    
    protected Script_DialogueNode MynesConversationNode
    {
        get => dialogueController?.Nodes?.Length > 0
                ? dialogueController.Nodes[0]
                : _MynesConversationNode;
        set => _MynesConversationNode = value;
    }

    void OnValidate()
    {
        if (Id != MynesMirrorId)
        {
            Debug.LogError($"This Mynes Mirror {name} Id and MynesMirrorId were not matching");
        }
        Id = MynesMirrorId;
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        
        Script_MynesMirrorEventsManager.OnEndTimeline += StartDialogue;

        HandleIsSolvedGraphics(isSolved);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        Script_MynesMirrorEventsManager.OnEndTimeline -= StartDialogue;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public override void ActionDefault()
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

    protected override bool CheckDisabled()
    {
        bool isActivated = Script_ScarletCipherManager.Control.MynesMirrorsActivationStates[MynesMirrorId];
        return isActivated || base.CheckDisabled();
    }

    /// <summary>
    /// The relevant choice node calls this to check its Id upon selecting
    /// </summary>
    public bool CheckCipher(int choiceIdx)
    {
        bool isSolved = Script_ScarletCipherManager.Control.HandleCipherSlot(MynesMirrorId, choiceIdx);

        HandleIsSolvedGraphics(isSolved);
        
        return isSolved;
    }

    protected virtual void HandleIsSolvedGraphics(bool isSolved)
    {
        if (isSolved)   mirrorGraphics.sprite = brokenMirrorSprite;
        else            mirrorGraphics.sprite = defaultMirrorSprite;
    }

    // ------------------------------------------------------------------
    // Signal Reactions START
    /// <summary>
    /// Begin Myne's dialogue, the end of Timeline calls MynesMirrorManager to fire this event
    /// </summary>
    public virtual void StartDialogue()
    {
        Script_DialogueManager.DialogueManager.StartDialogueNode(MynesConversationNode);
    }
    // Signal Reactions END
    // ------------------------------------------------------------------
    
    // ------------------------------------------------------------------
    // Next Node Actions START
    /// <summary>
    /// Shows Myne's dramatic entrance
    /// </summary>
    public void Intro()
    {
        game.ChangeStateCutScene();
        Script_PRCSManager.Control.OpenPRCSCustom(Script_PRCSManager.CustomTypes.MynesMirror);

        // BGM coroutine may still be running, so ensure to Pause in case it was stopped prematurely
        // and never called its callback to pause BGM
        game.PauseBgMusic();
        game.PauseBgThemeSpeakers();
        
        Script_BackgroundMusicManager.Control.SetVolume(1f, Const_AudioMixerParams.ExposedBGVolume);
        bgThemePlayer.gameObject.SetActive(true);
    }
    
    /// <summary>
    /// Remove cut scene
    /// Called from last Dialogue Node
    /// </summary>
    public virtual void End()
    {
        // Fade out BG Theme Player and Fade in Game BGM
        Script_BackgroundMusicManager.Control.FadeOutMed(() => {
                bgThemePlayer.gameObject.SetActive(false);
                FadeInBGMusic();
            },
            Const_AudioMixerParams.ExposedBGVolume
        );
        
        Script_PRCSManager.Control.ClosePRCSCustom(Script_PRCSManager.CustomTypes.MynesMirror, () => {
            game.ChangeStateInteract();
            Script_ScarletCipherManager.Control.MynesMirrorsActivationStates[MynesMirrorId] = true;
        });
    }

    /// <summary>
    /// Note! Call this on end dialogue node No because music will be faded out
    /// on interaction with the mirror.
    /// </summary>
    public void FadeInBGMusic()
    {
        game.UnPauseBgMusic();
        game.UnPauseBgThemeSpeakers();
        Script_BackgroundMusicManager.Control.FadeInSlow(null, Const_AudioMixerParams.ExposedBGVolume);
    }
    // Next Node Actions END
    // ------------------------------------------------------------------
}
