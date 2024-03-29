﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class Script_InteractableFullArt : Script_InteractableObjectText
{
    [SerializeField] private Script_FullArt fullArt;
    [SerializeField] private FadeSpeeds fadeInSpeed;
    [SerializeField] private FadeSpeeds fadeOutSpeed;
    
    [SerializeField] private Script_BgThemePlayer bgThemePlayer;
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private bool isFadeFullArtBgThemePlayer;
    [SerializeField] private FadeSpeeds fullArtBgThemeFadeInSpeed;
    [SerializeField] private FadeSpeeds onFullArtCloseBgmFadeInSpeed;
    
    // Note: For prompt to work properly, MUST include an empty dialogue node in dialogueNodes,
    // so we can properly reset isPromptDialogueDone at the end of dialogue.
    [SerializeField] private Script_DialogueNode promptDialogueNode;
    [SerializeField] private UnityEvent _preFullArtAction;

    [Tooltip("Use when there is no dialogue node after Full Art that usually would reset isPromptDialogueDone by calling OnMyDialogueEnd")]
    [SerializeField] private bool forceResetPromptOnFullArtRemoveDone;
    
    private bool isFullArtMode;
    private bool isInputDisabled;
    private Script_FullArt activeFullArt;
    private Coroutine musicFadeCoroutine;
    private bool isPromptDialogueDone;
    private bool didPauseBgm;

    protected UnityEvent PreFullArtAction
    {
        get => _preFullArtAction;
    }

    protected override void OnEnable()
    {
        Script_DialogueEventsManager.OnDialogueEnd += OnMyDialogueEnd;

        base.OnEnable();
    }

    protected override void OnDisable()
    {
        Script_DialogueEventsManager.OnDialogueEnd -= OnMyDialogueEnd;

        base.OnDisable();
    }

    private void OnMyDialogueEnd()
    {
        // check if currentNode we finished on is one of ours and we're in fullArtMode
        if (
            dialogueManager?.currentNode != null && CheckInMyNodes(dialogueManager.currentNode)
        )
        {
            if (isFullArtMode)
            {
                Dev_Logger.Debug($"OnDialogueEnd Event -> OnMyDialogueEnd(): dialogueManager.currentNode: {dialogueManager.currentNode}");
                RemoveFullArt();
            }

            ResetPrompt();
        }
    }
    
    private bool CheckInMyNodes(Script_DialogueNode node)
    {
        foreach (Script_DialogueNode myNode in dialogueNodes)
            if (node == myNode) return true;

        return false;
    }
    
    protected override void ActionDefault()
    {
        if (isDialogueCoolDown)         return;
        if (CheckDisabledDirections())  return;
        
        InvokeFullArtPreAction();
        
        if (promptDialogueNode != null && !isPromptDialogueDone)
        {
           HandlePromptDialogue();
           return;
        }

        HandleFullArtInteraction();
    }

    private void HandleFullArtInteraction()
    {
        if (isInputDisabled)    return;
        // first fade in the full art
        // after fade, allow dialogue input detection
        if (!isFullArtMode)
        {
            HandleFirstInteraction();
        }
        else
        {
            if (activeFullArt.nextFullArt == null)
            {
                if (dialogueNodes.Length == 0)
                {
                    RemoveFullArt();
                    return;
                }
                
                base.ActionDefault();
            }
            else
            {
                ContinueFullArt();
            }
        }
    }

    // ------------------------------------------------------------------
    // Next Node Actions
    
    /// <summary>
    /// Called from "yes" choice in choices prompt if there was a promptDialogueNode
    /// </summary>
    public void PromptDone()
    {
        isPromptDialogueDone = true;
        HandleFullArtInteraction();
    }

    // ------------------------------------------------------------------

    private void HandlePromptDialogue()
    {
        if (Script_Game.Game.GetPlayer().State != Const_States_Player.Dialogue)
        {
            dialogueManager.StartDialogueNode(
                promptDialogueNode,
                SFXOn: true,
                type: Const_DialogueTypes.Type.Read,
                this
            );
        }
        else
            dialogueManager.ContinueDialogue();
    }

    private void HandleFirstInteraction()
    {
        isInputDisabled = true;
        // fade in via fullartmanager
        
        Script_Game.Game.GetPlayer().SetIsViewing();
        
        Script_Game.Game.fullArtManager.ShowFullArt(
            activeFullArt,
            fadeInSpeed, () => {    // Use iobject fadeIn speed so fullArt can be extensible
                isFullArtMode = true;
                isInputDisabled = false;
            },
            Script_FullArtManager.FullArtState.InteractableObject
        );
        
        if (bgThemePlayer)
        {
            if (audioMixer == null)
            {
                Debug.LogError("You need to ref AudioMixer if you want to play from fullArt obj.");
                return;
            }
            HandleMusicFadeIn();
        }

        void HandleMusicFadeIn()
        {
            // fade out bg volume, pause any bg songs, play my bgThemePlayer and set volume
            StopMyCoroutines();
            musicFadeCoroutine = StartCoroutine(
                Script_AudioMixerFader.Fade(
                    audioMixer,
                    Const_AudioMixerParams.ExposedBGVolume,
                    Script_AudioEffectsManager.GetFadeTime(fadeInSpeed),
                    0f,
                    () => {
                        var bgm = Script_BackgroundMusicManager.Control;
                        
                        if (bgm.IsPlaying)
                        {
                            bgm.Pause();
                            didPauseBgm = true;
                        }
                        
                        Script_Game.Game.PauseNPCBgTheme();
                        
                        PlayBgThemePlayerUntracked();
                        Script_AudioMixerVolume.SetVolume(
                            audioMixer, Const_AudioMixerParams.ExposedBGVolume, 1f
                        );
                    }
                )
            );
        }

        void PlayBgThemePlayerUntracked()
        {
            bgThemePlayer.isUntrackedSource = true;

            if (isFadeFullArtBgThemePlayer)
                bgThemePlayer.FadeInPlay(null, fullArtBgThemeFadeInSpeed.GetFadeTime());
            else
                bgThemePlayer.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Allows to view a series of full arts
    /// </summary>
    private void ContinueFullArt()
    {
        isInputDisabled = true;
        // fade in via fullartmanager
        Script_Game.Game.fullArtManager.TransitionOutFullArt(activeFullArt, fadeInSpeed, null);
        
        activeFullArt = activeFullArt.nextFullArt;
        
        Script_Game.Game.fullArtManager.ShowFullArt(
            activeFullArt,
            fadeInSpeed, () => {
                isFullArtMode = true;
                isInputDisabled = false;
            },
            Script_FullArtManager.FullArtState.InteractableObject
        );
    }

    /// <summary>
    /// Also called from NextNodeAction() when dialogue on ending node
    /// </summary>
    public void RemoveFullArt()
    {
        var game = Script_Game.Game;
        
        Dev_Logger.Debug($"RemoveFullArt called with state {game.fullArtManager.state}");
        
        // whoever opens the fullArt get priority to remove it; otherwise there are multiple calls to remove it
        if (game.fullArtManager.state != Script_FullArtManager.FullArtState.InteractableObject)
            return;
        
        Dev_Logger.Debug("RemoveFullArt(): end dialogue event caught by FullArt");
        
        isInputDisabled = true;
        var player = game.GetPlayer();

        // Cache paused Bgm; the flag may be re-inited if Full Art fades out (calling InitializeState) before music fades
        bool didPauseBgmOnThisFullArt = didPauseBgm;

        // Dialogue Manager OnEndDialogue may have set Player back to Interact.
        // In that case, Player should be set to Viewing until Full Art is completely removed.
        // fullArtManager will set player back to Interact.
        if (player.State == Const_States_Player.Interact)
            player.SetIsViewing();

        game.fullArtManager.HideFullArt(activeFullArt, fadeOutSpeed, () =>
        {
            isFullArtMode = false;
            isInputDisabled = false;
            
            if (forceResetPromptOnFullArtRemoveDone)
                ResetPrompt();

            InitializeState();
        });

        if (bgThemePlayer != null && audioMixer != null)
            HandleMusicFadeOut();

        void HandleMusicFadeOut()
        {
            StopMyCoroutines();
            
            // fade out my bgThemePlayer, stop it and set master vol, unpause songs
            musicFadeCoroutine = StartCoroutine(
                Script_AudioMixerFader.Fade(
                    audioMixer,
                    Const_AudioMixerParams.ExposedBGVolume,
                    Script_AudioEffectsManager.GetFadeTime(fadeOutSpeed),
                    0f,
                    () => {
                        StopBgThemePlayer();
                        
                        // Unpause whatever was playing before
                        game.UnPauseNPCBgTheme();
                        
                        if (didPauseBgmOnThisFullArt)
                        {
                            Dev_Logger.Debug($"{name} Unpause bgm");
                            game.UnPauseBgMusic();
                            didPauseBgm = false;
                        }
                        
                        if (isFadeFullArtBgThemePlayer)
                        {
                            Script_BackgroundMusicManager.Control.FadeInExtra(
                                out musicFadeCoroutine,
                                null,
                                onFullArtCloseBgmFadeInSpeed.GetFadeTime(),
                                Const_AudioMixerParams.ExposedBGVolume
                            );
                        }
                        else
                        {
                            Script_AudioMixerVolume.SetVolume(
                                audioMixer, Const_AudioMixerParams.ExposedBGVolume, 1f
                            );
                        }
                    }
                )
            );
        }
    }

    private void ResetPrompt()
    {
        isPromptDialogueDone = false;
    }

    private void StopBgThemePlayer()
    {
        bgThemePlayer.SoftStop();
    }

    // Invoke an action before displaying the full art.
    private void InvokeFullArtPreAction()
    {
        if (PreFullArtAction.CheckUnityEventAction()) PreFullArtAction.Invoke();
    }

    private void StopMyCoroutines()
    {
        if (musicFadeCoroutine != null)
        {
            StopCoroutine(musicFadeCoroutine);
            musicFadeCoroutine = null;
        }
    }

    public override void InitializeState()
    {
        activeFullArt = fullArt;
        base.InitializeState();

        didPauseBgm = false;
    }
}