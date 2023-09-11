using System.Collections;
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
    public bool didStabCutScene;

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
    [SerializeField] private float waitAfterItemDescriptionDoneTime;
    [SerializeField] private Transform textParent;
    [SerializeField] private Transform fullArtParent;
    
    [SerializeField] private Script_Interactable easle;
    [SerializeField] private Script_InteractableFullArt easleFullArt;
    [SerializeField] private Script_InteractableObjectText easleYellAtPlayerIOText;
    
    [SerializeField] private Script_InteractableFullArt dirtyMagazine;

    [SerializeField] private float waitBeforeSelfRealizationTime;
    [SerializeField] private Script_DialogueNode selfRealizationDialogue;

    // ------------------------------------------------------------------
    // Painting Entrances

    [SerializeField] private Script_InteractablePaintingEntrance paintingEntranceMid;
    
    // ------------------------------------------------------------------
    // Hurt Ellenia
    [Header("Hurt Ellenia")]
    [SerializeField] private Script_TriggerPlayerStay elleniaHurtTrigger;

    [SerializeField] private float ElleniasHurtBgmTargetVol;
    [SerializeField] private float ElleniasHurtBgmTargetVolFadeTime;
    [SerializeField] private float blackScreenBeforeHurtSceneTime;
    [SerializeField] private Script_PRCSPlayer ElleniasArtPRCSPlayer;
    [SerializeField] private Script_DialogueNode cutSceneStartNode;
    [SerializeField] private Script_DialogueNode onElleniasPRCSDoneNode;
    [SerializeField] private Script_VCamera followElleniaHurtVCam;
    [SerializeField] private float onStartElleniaHurtCutSceneWaitTime;
    [SerializeField] private float cutSceneFadeInTime;
    [SerializeField] private Script_Marker playerTeleportPos;
    
    [SerializeField] private Script_FullArt ElleniaCenterFullArt;
    [SerializeField] private Script_DialogueNode[] cursedNodes;
    [SerializeField] private float[] waitForCursedDialogueTimes;

    [SerializeField] private Script_BgThemePlayer creepyBgmPlayer;
    [SerializeField] private Script_BgThemePlayer creepyIntenseBgmPlayer;
    [SerializeField] private float creepyIntenseFadeInTime;
    [SerializeField] private Script_BgThemePlayer heartbeatsPlayer;
    [SerializeField] private int ElleniasArtHeartBeatsSettings;
    [SerializeField] private int intensestHeartBeatsSettings;

    [SerializeField] private Script_FullArt ElleniaCloseUpMadFullArt;
    [SerializeField] private PlayableDirector ElleniaStabsDirector;
    [SerializeField] private Script_CanvasGroupController ElleniaStabsCanvasGroup;
    [SerializeField] private float waitAfterInSilenceTime;

    [SerializeField] private float blackScreenBeforeStabTimeShortScene;

    // ------------------------------------------------------------------
    
    [SerializeField] private string devPasswordDisplay; // FOR TESTING ONLY
    public Script_LevelBehavior_21 devLB21; // FOR TESTING ONLY
    
    private bool isElleniaTourWalkRight;
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
        Script_PRCSEventsManager.OnPRCSDone             += OnElleniasArtPRCSDone;
        
        if (Script_EventCycleManager.Control.IsElleniaHurt())
        {
            elleniaHurtTrigger.gameObject.SetActive(true);
            Script_GameEventsManager.OnLevelInitComplete    += HandleStartCheckingElleniaHurtCutScene;
            isElleniaHurtToday = true;
        }
        else
            elleniaHurtTrigger.gameObject.SetActive(false);
        
        Script_GraphicsManager.Control.SetSpikeRoomPhysics();
    }

    protected override void OnDisable()
    {
        ElleniaDirector.stopped                         -= OnElleniaPlayableDone;    
        Script_PRCSEventsManager.OnPRCSDone             -= OnElleniasArtPRCSDone;
        Script_GameEventsManager.OnLevelInitComplete    -= HandleStartCheckingElleniaHurtCutScene;

        Script_GraphicsManager.Control.SetDefaultPhysics();
    }

    protected override void Update()
    {
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
        if (
            aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[0]
            || aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[10]
        )
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
        else if (
            aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[4]
            || aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[12]
        )
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
        else if (
            aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[6]
            || aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[11]
        )
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
    
    // ----------------------------------------------------------------------
    // Ellenia Hurt Cut Scene
    
    /// <summary>
    /// Five Sections to Cut Scene
    /// (Short Version cuts to section #5 Stab Cut Scene)
    /// 
    /// 1. Ellenia talks about a word
    ///     0.5vol heartbeat, fade in black BG after each node
    /// 2. Show ElleniasArt PRCS
    ///     1.0vol heartbeat, 1.0vol creepy BGM start
    /// 3. Ellenia debates whether to talk about Eileen's condition
    ///     0.5-1.0vol 1.0-1.8pitch heartbeat, 1.0vol creepy BGM
    /// 4. Ellenia madly talks about pain
    ///     1.0vol 1.8pitch heartbeat, 1.0vol INTENSE creepy BGM
    /// 5. Stab Cut Scene
    ///     Stop all music on stab
    /// </summary>
    
    // Trigger
    public void HandleElleniaHurtCutScene()
    {
        if (
            isCheckingPsychicDuckElleniaHurtCutScene
            && !isCurrentPuzzleComplete
        )
        {
            // Ensure Psychic Duck is the active sticker and we haven't already played this cut scene.
            bool isPsychicDuckActive = Script_ActiveStickerManager.Control.IsActiveSticker(Const_Items.PsychicDuckId);
            
            if (!isPsychicDuckActive || isElleniaHurtCutSceneActivated)
                return;
            
            StartElleniaHurtCutScene();
        }
    }
    
    private void StartElleniaHurtCutScene()
    {
        isElleniaHurtCutSceneActivated                  = true;
        isCheckingPsychicDuckElleniaHurtCutScene        = false;

        game.ChangeStateCutScene();

        var bgm = Script_BackgroundMusicManager.Control;
        
        // If already did cut scene, only show the stabbing portion.
        if (didStabCutScene)
        {
            StartCoroutine(StartStabCutSceneShort());
            return;
        }
        
        // Fade out bgm a bit so can hear heartbeats clearly.
        // BGM will be reset to 0f when Creepy Music starts on PRCS.
        bgm.FadeOut(null, ElleniasHurtBgmTargetVolFadeTime, BGMParam, ElleniasHurtBgmTargetVol);
        
        StartCoroutine(WaitForElleniaHurtCutScene());

        IEnumerator WaitForElleniaHurtCutScene()
        {
            yield return new WaitForSeconds(onStartElleniaHurtCutSceneWaitTime);

            // Fade to Black and focus camera on Ellenia (Hurt)
            StartCoroutine(game.TransitionFadeIn(cutSceneFadeInTime, () => {
                // Teleport player.
                game.GetPlayer().Teleport(playerTeleportPos.transform.position);
                game.GetPlayer().FaceDirection(Directions.Left);

                // Face camera to Ellenia (Hurt).
                SwitchVCamElleniaHurt();
                
                ShowElleniaFullArtInBg();
            }));
        }

        // Do Framing and show Ellenia FA in BG
        void ShowElleniaFullArtInBg()
        {
            Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
                isOpen: true,
                framing: Script_UIAspectRatioEnforcerFrame.Framing.ElleniasHand,
                cb: () => Script_FullArtManager.Control.ShowFullArt(
                    ElleniaCenterFullArt,
                    FadeSpeeds.None,
                    () => StartCoroutine(StartElleniaHurtCutSceneDialogue()),
                    Script_FullArtManager.FullArtState.DialogueManager
                ),
                t: 0.01f
            );
        }

        IEnumerator StartElleniaHurtCutSceneDialogue()
        {
            // Ensure cam has enough time to cut
            yield return new WaitForSeconds(blackScreenBeforeHurtSceneTime);
            
            // Fade out fader to reveal just Ellenia and start Dialogue
            StartCoroutine(game.TransitionFadeOut(FadeSpeeds.XFast.ToFadeTime(), () => {
                Script_DialogueManager.DialogueManager.StartDialogueNode(
                    cutSceneStartNode,
                    SFXOn: false
                );
            }));
        }

        // Short Cut Scene
        // Note: The fading in transition will be exactly the same as default cut scene EXCEPT
        // the blackScreenBeforeStabTimeShortScene which should be longer in the short version
        // because the transition needs to be more dramatic.
        IEnumerator StartStabCutSceneShort()
        {
            bgm.FadeOut(null, onStartElleniaHurtCutSceneWaitTime, Const_AudioMixerParams.ExposedBGVolume);
            
            yield return new WaitForSeconds(onStartElleniaHurtCutSceneWaitTime);

            // Fade to Black and focus camera on Ellenia (Hurt)
            StartCoroutine(game.TransitionFadeIn(cutSceneFadeInTime, () => {
                StartCoroutine(WaitToStartStabCutSceneShort());
            }));
        }

        // Short Cut Scene
        IEnumerator WaitToStartStabCutSceneShort()
        {
            yield return new WaitForSeconds(blackScreenBeforeStabTimeShortScene);
            
            StartCoroutine(game.TransitionFadeOut(0f, () => {
                // Skip to most intense heartbeat settings
                var pitchShifter = heartbeatsPlayer.GetComponent<Script_AudioSourcePitchShifter>();
                
                // Must explicitly change starting vol; otherwise, starts at 0.5f (for default cut scene)
                pitchShifter.StartingVolume = 1f;
                pitchShifter.SwitchMySettings(intensestHeartBeatsSettings);
                heartbeatsPlayer.gameObject.SetActive(true);

                creepyIntenseBgmPlayer.gameObject.SetActive(true);
                creepyIntenseBgmPlayer.Play();
                
                PlayElleniaStabsHandTimeline();
            }));
        }
    }

    // Alone7 Node
    public void ShowElleniasArtPRCSInBg()
    {
        // Remove framing same frame
        Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
            isOpen: false,
            framing: Script_UIAspectRatioEnforcerFrame.Framing.ElleniasHand,
            isNoAnimation: true
        );

        // Open Fader (Under HUD) to ensure we have a black BG (since it's not guaranteed if not using frame)
        Script_TransitionManager.Control.TimelineUnderHUDBlackScreenOpen();

        ElleniasArtPRCSPlayer.gameObject.SetActive(true);
        ElleniasArtPRCSPlayer.PlayCustom(Script_PRCSManager.CustomTypes.ElleniasHand);
    }

    // Load Ellenia Center FA behind ElleniasHandPRCS and wait to reveal by removing ElleniasHandPRCS
    private void OnElleniasArtPRCSDone(Script_PRCSPlayer prcs)
    {
        if (prcs == ElleniasArtPRCSPlayer)
        {
            // Set heartbeats back to default
            heartbeatsPlayer.GetComponent<Script_AudioSourcePitchShifter>().SwitchMySettings(0);
            
            // Open Ellenia FullArt behind ElleniasHandPRCS
            Script_FullArt firstFullArt = onElleniasPRCSDoneNode.data.FullArt;
            Script_FullArtManager.Control.ShowFullArt(
                firstFullArt,
                FadeSpeeds.None,
                null,
                Script_FullArtManager.FullArtState.DialogueManager
            );
            
            // Remove ElleniasHandPRCS and with same speed reopen framing
            ElleniasArtPRCSPlayer.CloseCustom(Script_PRCSManager.CustomTypes.ElleniasHand, null);
            Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
                isOpen: true,
                framing: Script_UIAspectRatioEnforcerFrame.Framing.ElleniasHand,
                cb: () => {
                    Script_DialogueManager.DialogueManager.StartDialogueNode(
                        onElleniasPRCSDoneNode,
                        SFXOn: false
                    );

                    // Once framing is present, remove the black BG underneath
                    Script_TransitionManager.Control.TimelineUnderHUDBlackScreenClose();
                },
                t: FadeSpeeds.XFast.GetFadeTime()
            );
        }
    }

    // Last Node in Cut Scene
    public void PlayElleniaStabsHandTimeline()
    {
        // Framing for short version
        Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
            isOpen: true,
            framing: Script_UIAspectRatioEnforcerFrame.Framing.ElleniasHand,
            isNoAnimation: true
        );
        
        // Remove Dialogue Box & FA in the full cut scene version, if short version,
        // must skip since Full Art would be inactive.
        if (ElleniaCloseUpMadFullArt.gameObject.activeInHierarchy)
        {
            Script_DialogueManager.DialogueManager.InitialState();
            Script_FullArtManager.Control.HideFullArt(ElleniaCloseUpMadFullArt, FadeSpeeds.None, null);
        }

        ElleniaStabsCanvasGroup.Open();
        ElleniaStabsDirector.Play();
    }
    
    public void OnStab()
    {
        creepyBgmPlayer.SoftStop();
        creepyIntenseBgmPlayer.SoftStop();
        heartbeatsPlayer.SoftStop();
    }
    
    public void OnStabDone()
    {
        // Put up black screen
        StartCoroutine(game.TransitionFadeIn(0, () => {
            SwitchVCamPlayer();
            ElleniaStabsCanvasGroup.Close();
            Script_FullArtManager.Control.CloseBgForceBlack();

            Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
                isOpen: false,
                framing: Script_UIAspectRatioEnforcerFrame.Framing.ElleniasHand,
                isNoAnimation: true
            );
        }));
    }
    
    // Ellenia Stabs Timeline Done
    public void OnEndElleniaHurtCutScene()
    {
        Dev_Logger.Debug("OnEndElleniaHurtCutScene");

        didStabCutScene = true;
        
        // Set volume before level fades in Bgm to avoid click (Bgm audio mixer should not be used at this point)
        Script_BackgroundMusicManager.Control.SetVolume(0f, BGMParam);

        // Wait in silence before teleporting to Hallway and BGM starting again
        StartCoroutine(WaitToTeleport());
        
        IEnumerator WaitToTeleport()
        {
            yield return new WaitForSeconds(waitAfterInSilenceTime);
            game.ElleniaHurtEndTransition();
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

        // If Player is blocking Ellenia (standing Up from her), she'll walk right first.
        if (GetElleniaToPlayerDirection() == Directions.Up)
        {
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 10);
            isElleniaTourWalkRight = true;
        }
        else
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
        if (isElleniaTourWalkRight)
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 12);
        else
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
        StartCoroutine(WaitToContinueDialogue());

        IEnumerator WaitToContinueDialogue()
        {
            yield return new WaitForSeconds(waitAfterItemDescriptionDoneTime);

            Script_DialogueManager.DialogueManager.StartDialogueNode(onItemDescriptionDoneNode, false);
        }
    }

    public void CorrectDone()
    {
        ElleniaIntroDoneDialogueNodes();

        spokenWithEllenia = true;
        
        // Ellenia walks to the Exit. If player standing to Right, Ellenia should walk down first.
        if (GetElleniaToPlayerDirection() == Directions.Right)
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 11);
        else
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

    // After Hurt First Node
    public void StartHeartbeats()
    {
        heartbeatsPlayer.GetComponent<Script_AudioSourcePitchShifter>().SwitchMySettings(0);
        heartbeatsPlayer.gameObject.SetActive(true);
    }

    // On ElleniasArtPRCS Start
    public void StartIntenseHeartBeatsOnElleniasArt()
    {
        heartbeatsPlayer.GetComponent<Script_AudioSourcePitchShifter>().SwitchMySettings(ElleniasArtHeartBeatsSettings);

        Script_BackgroundMusicManager bgm = Script_BackgroundMusicManager.Control;
        float fadeTime = FadeSpeeds.Fast.GetFadeTime();
        bgm.FadeOut(() => {
                bgm.SetVolume(0f, BGMParam);
                bgm.Stop();
                bgm.SetVolume(1f, BGMParam);
            },
            fadeTime,
            BGMParam
        );

        StartCreepyMusic(fadeTime);
    }
    
    // Cursed Nodes
    public void StartCursedDialogue(int i)
    {
        float waitTime = waitForCursedDialogueTimes[i];
        
        StartCoroutine(WaitToTalk());

        IEnumerator WaitToTalk()
        {
            yield return new WaitForSeconds(waitTime);

            Script_DialogueManager.DialogueManager.StartDialogueNode(cursedNodes[i], SFXOn: false);
        }
    }

    // Cursed Node
    public void FadeInFullArtBg(float alpha)
    {
        Script_FullArtManager.Control.SetForceBlackAlpha(alpha);
    }

    // Cursed Node
    public void StartCreepyMusic(float fadeTime)
    {
        creepyBgmPlayer.gameObject.SetActive(true);
        creepyBgmPlayer.FadeInPlay(null, fadeTime);
    }

    // Cursed Node
    public void SwitchHeartbeatPitchSettings(int i)
    {
        heartbeatsPlayer.GetComponent<Script_AudioSourcePitchShifter>().SwitchMySettings(i);
    }

    // Cursed Node
    public void StartCreepyMusicIntense()
    {
        creepyBgmPlayer.FadeOutStop(null, creepyIntenseFadeInTime);
        creepyIntenseBgmPlayer.gameObject.SetActive(true);
        creepyIntenseBgmPlayer.FadeInPlay(null, creepyIntenseFadeInTime);
    }

    // Cursed Node Alone0 when Bg FA is faded in all the way
    public void OpenBgForceBlack()
    {
        Script_FullArtManager.Control.OpenBgForceBlack();
        Script_FullArtManager.Control.SetForceBlackAlpha(0f);
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
            onSelfRealizationDoneAction = () => HandleFadeInBgm(() => {
                game.ChangeStateInteract();
                
                // Word achievement
                Script_AchievementsManager.Instance.UnlockWord();
            });
            
            yield return new WaitForSeconds(waitBeforeSelfRealizationTime);
            
            Script_DialogueManager.DialogueManager.StartDialogueNode(selfRealizationDialogue);
        }
    }

    // ----------------------------------------------------------------------
    
    private Directions GetElleniaToPlayerDirection(bool isCursedR2Ellenia = false)
    {
        Vector3 ElleniaPosition = isCursedR2Ellenia 
            ? ElleniaHurt.transform.position
            : Ellenia.transform.position;
        Vector3 playerPosition = game.GetPlayer().transform.position;

        return ElleniaPosition.GetDirectionToTarget(playerPosition);
    }
    
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
        
        ElleniasArtPRCSPlayer.gameObject.SetActive(false);
        
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

            if (GUILayout.Button("Print Direction (Ellenia to Player)"))
            {
                Dev_Logger.Debug($"{lb.GetElleniaToPlayerDirection()}");
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

            GUILayout.Space(12);

            if (GUILayout.Button("Ellenia Hurt Cut Scene"))
            {
                lb.StartElleniaHurtCutScene();
            }

            if (GUILayout.Button("Ellenia Stabs Timeline"))
            {
                lb.PlayElleniaStabsHandTimeline();
            }
        }
    }
#endif
}

