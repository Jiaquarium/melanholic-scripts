﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Audio;
using System;
using Cinemachine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_25 : Script_LevelBehavior
{
    private const string BGMParam = Const_AudioMixerParams.ExposedBGVolume;
    
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool isPuzzleComplete;
    
    // Tracks if need to do the Ellenia intro.
    public bool spokenWithEllenia;

    /* ======================================================================= */
    
    // To track if the puzzle is completed, to be reset on new Day
    public bool isCurrentPuzzleComplete;
    
    [SerializeField] private Script_DemonNPC Ellenia;
    [SerializeField] private Script_DemonNPC ElleniaHurt;
    [SerializeField] private Script_DialogueNode[] weekdayTalkedInitialElleniaPsychicNodes;
    [SerializeField] private Script_DialogueNode[] weekendDidntTalkElleniaPsychicNodes;
    [SerializeField] private Script_DialogueNode[] weekendTalkedElleniaPsychicNodes;
    [SerializeField] private Script_DialogueNode[] weekendTalkedElleniaTalkedStatePsychicNodes;

    [SerializeField] private Script_VCamera followElleniaVCam;
    [SerializeField] private PlayableDirector ElleniaDirector;
    
    [SerializeField] private Script_DialogueNode[] cutSceneNodes;
    [SerializeField] private Script_DialogueNode cutSmallTalkNode;
    [SerializeField] private Script_DialogueNode onCorrectDoneNode;
    [SerializeField] private Script_DialogueNode onCorrectDonePastQuestDoneNode;
    
    [SerializeField] private Script_DialogueNode onCorrectWeekendNode;
    [SerializeField] private float toRealizePlayerHasStickerWaitTime;
    [SerializeField] private Script_DialogueNode realizePlayerHasStickerNode;
    
    [SerializeField] private Script_DialogueNode introContinuationNode;
    [SerializeField] private Script_DialogueNode beforeExitNode;
    
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Script_BgThemePlayer ElleniaBgThemePlayer;
    
    [SerializeField] private float bgMusicFadeOutTime;
    [SerializeField] private float bgMusicEndIntroFadeOutTime;
    [SerializeField] private float waitToTurnTime; // should match with music
    [SerializeField] private float waitToCutSmallTalkTime;
    
    [SerializeField] private Script_StickerObject AnimalWithinSticker;
    [SerializeField] private Script_DialogueNode onItemDescriptionDoneNode;
    [SerializeField] private Transform textParent;
    [SerializeField] private Transform fullArtParent;
    
    [SerializeField] private Script_Interactable easle;
    [SerializeField] private Script_InteractableFullArt easleFullArt;
    [SerializeField] private Script_InteractableObjectText easleYellAtPlayerIOText;
    
    [SerializeField] private Script_InteractableFullArt dirtyMagazine;

    [SerializeField] private Script_PRCSPlayer ElleniasHandPRCSPlayer;
    [SerializeField] private Script_DialogueNode onElleniasPRCSDoneNode;
    [SerializeField] private Script_VCamera followElleniaHurtVCam;
    [SerializeField] private float onStartElleniaHurtCutSceneWaitTime;
    [SerializeField] private float cutSceneFadeInTime;
    [SerializeField] private float elleniaHurtCutSceneWaitToFadeInTime;
    [SerializeField] private Script_Marker playerTeleportPos;
    
    [SerializeField] private float waitBeforeSelfRealizationTime;
    [SerializeField] private Script_DialogueNode selfRealizationDialogue;

    // ------------------------------------------------------------------
    // Painting Entrances
    [SerializeField] private Script_InteractablePaintingEntrance paintingEntranceMid;
    
    [SerializeField] private string devPasswordDisplay; // FOR TESTING ONLY
    public Script_LevelBehavior_21 devLB21; // FOR TESTING ONLY
    
    private bool isElleniaHurtToday;
    private bool isElleniaHurtCutSceneActivated;
    private bool isCheckingPsychicDuckElleniaHurtCutScene;

    private bool isInitialization = true;
    private bool shouldChangeGameStateToInteract;

    [SerializeField] private bool isInitialPuzzleCompletion;
    
    /// <summary>
    /// Note: ensure to update this value after speaking with Ellenia
    /// Currently, every node either ends with Correct or Wrong nodes
    /// </summary>
    private bool didTalkWithElleniaToday = false;

    private Action onSelfRealizationDoneAction;

    public bool DidTalkWithElleniaToday
    {
        get => didTalkWithElleniaToday;
    }
    
    protected override void OnEnable()
    {
        ElleniaDirector.stopped                         += OnElleniaPlayableDone;    
        Script_PRCSEventsManager.OnPRCSDone             += OnElleniasHandPRCSDone;
        
        if (Script_EventCycleManager.Control.IsElleniaHurt())
        {
            Script_GameEventsManager.OnLevelInitComplete    += HandleStartCheckingElleniaHurtCutScene;
            isElleniaHurtToday = true;
        }
    }

    protected override void OnDisable()
    {
        ElleniaDirector.stopped                         -= OnElleniaPlayableDone;    
        Script_PRCSEventsManager.OnPRCSDone             -= OnElleniasHandPRCSDone;
        Script_GameEventsManager.OnLevelInitComplete    -= HandleStartCheckingElleniaHurtCutScene;
    }

    protected override void Update()
    {
        if (isCheckingPsychicDuckElleniaHurtCutScene && !isCurrentPuzzleComplete)
        {
            HandleElleniaHurtCutScene();
        }

        devPasswordDisplay = Script_Names.ElleniaPassword; // FOR TESTING PURPOSES ONLY
    }

    private void LateUpdate()
    {
        if (shouldChangeGameStateToInteract)
        {
            game.ChangeStateInteract();
            shouldChangeGameStateToInteract = false;
        }
    }

    public override int OnSubmit(string s)
    {
        if (CheckPassword(s))   return 1;
        else                    return 0;
    }

    private bool CheckPassword(string pw)
    {
        string Password = Script_Names.ElleniaPassword;

        return !string.IsNullOrEmpty(Password) && Password.ToUpper() == pw.ToUpper();
    }

    public void ElleniaIntroDoneDialogueNodes()
    {
        Ellenia.MyDialogueState = Script_DemonNPC.DialogueState.Talked;
    }
    /* ===========================================================================================
        CUTSCENE
    =========================================================================================== */
    
    /// <summary>
    /// when director is finished, start the next dialogue node 
    /// </summary>
    public void OnElleniaPlayableDone(PlayableDirector aDirector)
    {
        // walked to first painting
        if (aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[0])
        {
            Script_DialogueManager.DialogueManager.StartDialogueNode(cutSceneNodes[0], false);
        }
        // walked to middle painting
        else if (aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[1])
        {
            Script_DialogueManager.DialogueManager.StartDialogueNode(cutSceneNodes[1], false);
        }
        // walked to last painting
        else if (aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[2])
        {
            Script_DialogueManager.DialogueManager.StartDialogueNode(cutSceneNodes[2], false);
        }
        // walked to room center
        else if (aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[3])
        {
            Script_DialogueManager.DialogueManager.StartDialogueNode(cutSceneNodes[3], false);
        }
        // return to easle
        else if (aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[4])
        {
            OnReturnedToEasle();
        }
        /// OnCorrect Timeline
        else if (aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[5])
        {
            Script_DialogueNode onSubmitCorrectNode = Ellenia.MyPastQuestState == Script_DemonNPC.PastQuestState.Done
                ? onCorrectDonePastQuestDoneNode
                : onCorrectDoneNode;
            
            // Start dialogue & fade out music.
            // If it's Weekend, use already done nodes.
            if (game.RunCycle == Script_RunsManager.Cycle.Weekend)
            {
                Script_DialogueManager.DialogueManager.StartDialogueNode(onCorrectWeekendNode, false);
            }
            else
            {
                Script_DialogueManager.DialogueManager.StartDialogueNode(onSubmitCorrectNode, false);
            }
            
            StartCoroutine(
                Script_AudioMixerFader.Fade(
                    audioMixer,
                    BGMParam,
                    bgMusicEndIntroFadeOutTime,
                    0f,
                    () => {
                        AudioSource ElleniaAudio = ElleniaBgThemePlayer.GetComponent<AudioSource>();
                        if (ElleniaAudio.isPlaying)
                        {
                            ElleniaAudio.volume = 0f;
                            ElleniaAudio.Stop();
                            ElleniaAudio.gameObject.SetActive(false);
                        }
                        if (game.BGMManager.GetIsPlaying())
                            game.PauseBgMusic();
                    }
                )
            );
        }
        // Ellenia walked to the Exit and will brag to Player to look at her painting.
        else if (aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[6])
        {
            Script_DialogueManager.DialogueManager.StartDialogueNode(beforeExitNode, SFXOn: true);    
        }
        // Ellenia actually exits.
        else if (aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[7])
        {
            OnElleniaExitsDone();
        }

        void OnReturnedToEasle()
        {
            var VCamManager = Script_VCamManager.VCamMain;
            
            VCamManager.SwitchToMainVCam(followElleniaVCam);
            VCamManager.SetDefaultCinemachineBlendUpdateMethod();

            Script_DialogueManager.DialogueManager.StartDialogueNode(cutSceneNodes[4], false);
        }
    }

    public void CutSmallTalk()
    {
        StartCoroutine(WaitToTalk());   
        
        IEnumerator WaitToTalk()
        {
            yield return new WaitForSeconds(waitToCutSmallTalkTime);
            
            Script_DialogueManager.DialogueManager.StartDialogueNode(cutSmallTalkNode);
        }
    }

    private void HandleStartCheckingElleniaHurtCutScene()
    {
        isCheckingPsychicDuckElleniaHurtCutScene = true;
    }
    
    private void HandleElleniaHurtCutScene()
    {
        // Ensure Psychic Duck is the active sticker and we haven't already played this cut scene.
        bool isPsychicDuckActive = Script_ActiveStickerManager.Control.IsActiveSticker(Const_Items.PsychicDuckId);
        
        if (!isPsychicDuckActive || isElleniaHurtCutSceneActivated)
            return;
        
        isElleniaHurtCutSceneActivated                  = true;
        isCheckingPsychicDuckElleniaHurtCutScene        = false;

        game.ChangeStateCutScene();
        
        Script_BackgroundMusicManager bgm = Script_BackgroundMusicManager.Control;

        bgm.FadeOutFast(() => {
            bgm.Stop();
            bgm.SetVolume(1f, BGMParam);
        }, BGMParam);

        StartCoroutine(WaitForElleniaHurtCutScene());

        IEnumerator WaitForElleniaHurtCutScene()
        {
            yield return new WaitForSeconds(onStartElleniaHurtCutSceneWaitTime);

            StartCoroutine(game.TransitionFadeIn(cutSceneFadeInTime, () => {
                // Teleport player.
                game.GetPlayer().Teleport(playerTeleportPos.transform.position);
                game.GetPlayer().FaceDirection(Directions.Left);

                // Face camera to Ellenia (Hurt).
                SwitchVCamElleniaHurt();
                
                StartCoroutine(WaitForElleniaDialogue());
            }));
        }

        IEnumerator WaitForElleniaDialogue()
        {
            yield return new WaitForSeconds(elleniaHurtCutSceneWaitToFadeInTime);

            StartCoroutine(game.TransitionFadeOut(cutSceneFadeInTime, () => {
                PlayElleniasHandPRCS();
            }));
        }

        void PlayElleniasHandPRCS()
        {
            game.ChangeStateCutScene();
            
            Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
                isOpen: true,
                framing: Script_UIAspectRatioEnforcerFrame.Framing.ElleniasHand,
                cb: OnFramingAnimationDone
            );

            void OnFramingAnimationDone()
            {
                ElleniasHandPRCSPlayer.PlayCustom(Script_PRCSManager.CustomTypes.ElleniasHand);
            }
        }
    }

    private void OnElleniasHandPRCSDone(Script_PRCSPlayer prcs)
    {
        if (prcs == ElleniasHandPRCSPlayer)
        {
            // Remove ElleniasHandPRCS
            ElleniasHandPRCSPlayer.CloseCustom(Script_PRCSManager.CustomTypes.ElleniasHand, () => {
                Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
                    isOpen: false,
                    framing: Script_UIAspectRatioEnforcerFrame.Framing.ElleniasHand,
                    cb: () => Script_DialogueManager.DialogueManager.StartDialogueNode(onElleniasPRCSDoneNode)
                );
            });
        }
    }

    /// NextNodeAction START =====================================================================
    
    // Wrong PW (if player gets it wrong, it means they've already done a prompt, so default to short)
    public void SetShortPrompt()
    {
        didTalkWithElleniaToday = true;
    }
    
    public void OnWeekdayTalkedInitialDialogueDone()
    {
        Ellenia.MyDialogueState = Script_DemonNPC.DialogueState.Talked;
    }
    
    /// Node: "i'm part of the..."
    public void PlayOminousMusic()
    {
        game.ChangeStateCutScene();
        Ellenia.FaceDirection(Directions.Left);
        // fade out music and pause
        StartCoroutine(
            Script_AudioMixerFader.Fade(
                audioMixer,
                Const_AudioMixerParams.ExposedGameVolume,
                bgMusicFadeOutTime,
                0f,
                () => {
                    // after faded out, pause bg music and start theme music where Ellenia
                    // should turn on beat
                    game.PauseBgMusic();
                    Script_AudioMixerVolume
                        .SetVolume(audioMixer, Const_AudioMixerParams.ExposedGameVolume, 1f);
                    ElleniaBgThemePlayer.gameObject.SetActive(true);

                    StartCoroutine(StartDialogueOnBeat());
                }
            )
        );

        IEnumerator StartDialogueOnBeat()
        {
            yield return new WaitForSeconds(waitToTurnTime);

            Directions directionToPlayer = Script_Utils.GetDirectionToTarget(
                Ellenia.transform.position, 
                Script_Game.Game.GetPlayer().transform.position
            );
            Ellenia.FaceDirection(directionToPlayer);
            // then play dialogueNode
            Script_DialogueManager.DialogueManager.StartDialogueNode(introContinuationNode, false);          
        }
    }
    
    /// Node: "hehehe..."
    public void ElleniaWalksToPaintingsCutScene()
    {
        Dev_Logger.Debug("Ellenia walking to paintings cut scene");
        game.ChangeStateCutScene();

        var VCamManager = Script_VCamManager.VCamMain;
        
        VCamManager.SetNewVCam(followElleniaVCam);

        // Ellenia is controlled by Timeline which is on Update clock.
        // Jittery camera movement if Update Blend Method is left on Fixed.
        VCamManager.SetCinemachineBlendUpdateMethod(CinemachineBrain.BrainUpdateMethod.LateUpdate);

        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);
    }
    
    public void ElleniaFacesPainting()
    {
        Ellenia.FaceDirection(Directions.Up);
    }
    
    public void ElleniaWalksToMiddlePaintingCutScene()
    {
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 1);
    }

    public void ElleniaWalksToLastPaintingCutScene()
    {
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 2);
    }

    public void ElleniaFacesPlayer()
    {
        Directions directionOfPlayer = Script_Utils.GetDirectionToTarget(
            Ellenia.transform.position, Script_Game.Game.GetPlayer().transform.position
        );
        Ellenia.FaceDirection(directionOfPlayer);
    }

    public void ElleniaWalksToRoomCenterCutScene()
    {
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 3);
    }

    public void ElleniaWalksBackToEasleCutScene()
    {
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 4);
    }

    public void IntroDoneWrong()
    {
        if (game.state == Const_States_Game.CutScene)
        {
            StartCoroutine(
                Script_AudioMixerFader.Fade(
                    audioMixer,
                    Const_AudioMixerParams.ExposedGameVolume,
                    bgMusicEndIntroFadeOutTime,
                    0f,
                    () => {
                        Script_AudioMixerVolume.SetVolume(
                            audioMixer,
                            Const_AudioMixerParams.ExposedGameVolume,
                            1f
                        );
                        ElleniaBgThemePlayer.GetComponent<AudioSource>().volume = 0f;
                        ElleniaBgThemePlayer.gameObject.SetActive(false);

                        /// Need to wait for music to fade out before return facing easle
                        /// otherwise, would use controller/NextNodeAction
                        Ellenia.FaceDirection(Directions.Left);
                        
                        // need to change state in LateUpdate so NPC doesn't read the button event
                        shouldChangeGameStateToInteract = true;
                        game.UnPauseBgMusic();
                    }
                )
            );
        }
        else
        {
            Ellenia.FaceDirection(Directions.Left);
        }
        
        ElleniaIntroDoneDialogueNodes();
        spokenWithEllenia = true;
    }

    public void ElleniaOnCorrect()
    {
        // jump animation; when finished, triggers correct dialogue
        game.ChangeStateCutScene();
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 5);
        
        if (!isPuzzleComplete)
            isInitialPuzzleCompletion = true;
        
        isPuzzleComplete = true;
        isCurrentPuzzleComplete = true;
        didTalkWithElleniaToday = true;
    }

    public void GiveSticker()
    {
        game.HandleItemReceive(AnimalWithinSticker);
    }

    public void GiveStickerPlayerHas()
    {
        game.ChangeStateCutScene();
        StartCoroutine(WaitToContinueDialogue());
        
        // Wait for Ellenia to "realize" Player already has sticker.
        IEnumerator WaitToContinueDialogue()
        {
            yield return new WaitForSeconds(toRealizePlayerHasStickerWaitTime);

            Script_DialogueManager.DialogueManager.StartDialogueNode(realizePlayerHasStickerNode, false);    
        }
    }

    public void OnAnimalWitinDescriptionDone()
    {
        Script_DialogueManager.DialogueManager.StartDialogueNode(onItemDescriptionDoneNode, false);
    }

    public void CorrectDone()
    {
        ElleniaIntroDoneDialogueNodes();

        spokenWithEllenia = true;
        
        // Ellenia walks to the Exit
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 6);
    }

    public void ElleniaExit()
    {
        // Ellenia actually exits
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 7);
    }

    // BeforeNodeAction
    public void BeforeYellAtPlayer()
    {
        Ellenia.FacePlayer();
    }

    public void OnYellAtPlayerDone()
    {
        Ellenia.FaceDefaultDirection();
    }
    
    public void OnElleniaDidTalk()
    {
        Dev_Logger.Debug($"{name} OnElleniaDidTalk()");
        Script_EventCycleManager.Control.SetElleniaDidTalkCountdownMax();
    }

    public void SwitchVCamElleniaHurt()
    {
        Script_VCamManager.VCamMain.SetNewVCam(followElleniaHurtVCam);
    }

    public void SwitchVCamPlayer()
    {
        Script_VCamManager.VCamMain.SwitchToMainVCam(followElleniaHurtVCam);        
    }

    public void OnEndElleniaHurtCutScene()
    {
        SwitchVCamPlayer();
        game.ChangeStateInteract();
    }

    private void OnElleniaExitsDone()
    {
        Script_VCamManager.VCamMain.SwitchToMainVCam(followElleniaVCam);
        
        // Play Painting Done Cut Scene.
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(1, 8);
    }

    public void OnSelfRealizationDone()
    {
        onSelfRealizationDoneAction();
    }

    /// NextNodeAction END ==================================================
    // ----------------------------------------------------------------------
    // Timeline Signals

    public void HidePlayer()
    {
        game.GetPlayer().SetInvisible(true, 0f);
    }

    public void UnhidePlayer()
    {
        game.GetPlayer().SetInvisible(false, 0f);
    }
    
    public void OnElleniaPaintingTimelineDone()
    {
        game.UnPauseBgMusic();
        easleYellAtPlayerIOText.gameObject.SetActive(false);

        var transitionManager = Script_TransitionManager.Control;
        transitionManager.OnCurrentQuestDone(
            allQuestsDoneCb: () =>
            {
                HandleFadeInBgm(() => {
                    transitionManager.FinalCutSceneAwakening();
                });
            }, 
            defaultCb: () =>
            {
                HandleDefault();
            },
            Script_TransitionManager.FinalNotifications.Ellenia
        );

        void HandleFadeInBgm(Action cb)
        {
            Script_BackgroundMusicManager.Control.FadeInXSlow(() =>
            {
                easleFullArt.gameObject.SetActive(true);
                cb();
            }, BGMParam);
        }

        void HandleDefault()
        {
            if (isInitialPuzzleCompletion)
                StartCoroutine(WaitForSelfRealizationDialogue());
            else
                HandleFadeInBgm(game.ChangeStateInteract);
        }
        
        IEnumerator WaitForSelfRealizationDialogue()
        {
            onSelfRealizationDoneAction = () => HandleFadeInBgm(game.ChangeStateInteract);
            
            yield return new WaitForSeconds(waitBeforeSelfRealizationTime);
            
            Script_DialogueManager.DialogueManager.StartDialogueNode(selfRealizationDialogue);
        }
    }

    // ----------------------------------------------------------------------
    
    public override void Setup()
    {
        // Ellenia should always be there on new runs. We'll save states to skip Ellenia's intro
        // and to skip the giving Animal Within dialogue.
        game.SetupMovingNPC(Ellenia, isInitialization);
        
        // isElleniaHurtToday will also be set in OnEnable when it checks IsElleniaHurt,
        // which should happen on same frame as this check.
        if (isElleniaHurtToday || Script_EventCycleManager.Control.IsElleniaHurt())
        {
            if (!isCurrentPuzzleComplete)
                paintingEntranceMid.State = Script_InteractableObject.States.Disabled;
            
            ElleniaHurt.gameObject.SetActive(!isCurrentPuzzleComplete);
            Ellenia.gameObject.SetActive(false);

            isElleniaHurtToday = true;
        }
        else
        {
            paintingEntranceMid.State = Script_InteractableObject.States.Active;
            
            HandleElleniaDialogueState();
            
            if (isPuzzleComplete)
                Ellenia.MyPastQuestState = Script_DemonNPC.PastQuestState.Done;

            Ellenia.gameObject.SetActive(!isCurrentPuzzleComplete);
            ElleniaHurt.gameObject.SetActive(false);
        }

        easle.gameObject.SetActive(true);
        easleYellAtPlayerIOText.gameObject.SetActive(!isCurrentPuzzleComplete);
        easleFullArt.gameObject.SetActive(isCurrentPuzzleComplete);
        
        game.SetupInteractableObjectsText(textParent, isInitialization);
        game.SetupInteractableFullArt(fullArtParent, isInitialization);
        
        // PG Version
        if (Const_Dev.IsPGVersion)  dirtyMagazine.gameObject.SetActive(false);

        isInitialization = false;

        void HandleElleniaDialogueState()
        {
            // On Weekend Cycle, you can now talk to Ellenia from the start (no requirement to talk a previous day).
            // No Intro here at all because already done in Weekday cycle, so need Dialogue States.
            if (game.RunCycle == Script_RunsManager.Cycle.Weekend)
            {
                if (!didTalkWithElleniaToday)
                {
                    Ellenia.SwitchPsychicNodes(weekendTalkedElleniaPsychicNodes);
                    Ellenia.SwitchTalkedPsychicNodes(weekendTalkedElleniaTalkedStatePsychicNodes);

                    Ellenia.MyDialogueState = Script_DemonNPC.DialogueState.None;
                }
                else
                {
                    Ellenia.MyDialogueState = Script_DemonNPC.DialogueState.Talked;
                }
            }
            // On Weekday: Skip Ellenia's intro if already done.
            else if (spokenWithEllenia)
            {
                if (didTalkWithElleniaToday)
                {
                    Ellenia.MyDialogueState = Script_DemonNPC.DialogueState.Talked;
                }
                else
                {
                    // If spoken with Ellenia already on a previous day, but is first time talking with
                    // her this day, need to use Nodes that show Ellenia has forgotten Player
                    Ellenia.SwitchPsychicNodes(weekdayTalkedInitialElleniaPsychicNodes);   
                }
            }
        }
    }

    public void GetDirection()
    {
        Script_Utils.GetDirectionToTarget(
            Ellenia.transform.position,
            Script_Game.Game.GetPlayer().transform.position
        );
    }

#if UNITY_EDITOR
    private void DevElleniaTimeline()
    {
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 9);
    }

    [CustomEditor(typeof(Script_LevelBehavior_25))]
    public class Script_LevelBehavior_25Tester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_LevelBehavior_25 lb = (Script_LevelBehavior_25)target;
            
            if (GUILayout.Button("Ellenia Tour"))
            {
                lb.ElleniaWalksToPaintingsCutScene();
            }

            GUILayout.Space(12);
            
            if (GUILayout.Button("Set New Ellenia Password"))
            {
                lb.devLB21.SetNewElleniaPassword();
            }
            
            if (GUILayout.Button("Ellenia Intro Done DialogueNodes"))
            {
                lb.ElleniaIntroDoneDialogueNodes();
            }

            GUILayout.Space(12);

            if (GUILayout.Button("Ellenia On Correct"))
            {
                lb.ElleniaOnCorrect();
            }

            GUILayout.Space(12);

            if (GUILayout.Button("Hide Player"))
            {
                lb.HidePlayer();
            }
            
            if (GUILayout.Button("Unhide Player"))
            {
                lb.UnhidePlayer();
            }

            if (GUILayout.Button("Unhide Player"))
            {
                lb.UnhidePlayer();
            }

            if (GUILayout.Button("Test Ellenia Timeline"))
            {
                lb.DevElleniaTimeline();
            }
        }
    }
#endif
}

