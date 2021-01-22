using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Audio;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_25 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool isPuzzleComplete;
    public bool spokenWithEllenia;

    /* ======================================================================= */
    [SerializeField] private Script_DemonNPC Ellenia;
    [SerializeField] private Script_DialogueNode[] NoIntroElleniaNodes;
    [SerializeField] private Script_VCamera followElleniaVCam;
    [SerializeField] private PlayableDirector ElleniaDirector;
    
    [SerializeField] private Script_DialogueNode[] cutSceneNodes;
    [SerializeField] private Script_DialogueNode introContinuationNode;
    [SerializeField] private Script_DialogueNode beforeExitNode;
    
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Script_BgThemePlayer ElleniaBgThemePlayer;
    
    [SerializeField] private float bgMusicFadeOutTime;
    [SerializeField] private float bgMusicEndIntroFadeOutTime;
    [SerializeField] private float waitToTurnTime; // should match with music
    
    [SerializeField] private Script_StickerObject AnimalWithinSticker;
    [SerializeField] private Script_DialogueNode onItemDescriptionDoneNode;
    [SerializeField] private Transform textParent;
    [SerializeField] private Transform fullArtParent;
    [SerializeField] private Script_InteractableFullArt easleFullArt;
    [SerializeField] private Script_InteractableObjectText easleYellAtPlayerIOText;
    [SerializeField] private Script_InteractableFullArt dirtyMagazine;
    
    [SerializeField] private string devPasswordDisplay; // FOR TESTING ONLY
    public Script_LevelBehavior_21 devLB21; // FOR TESTING ONLY
    
    private bool isInitialization = true;
    private bool shouldChangeGameStateToInteract;

    protected override void OnEnable()
    {
        ElleniaDirector.stopped += OnElleniaPlayableDone;    
    }

    protected override void OnDisable()
    {
        ElleniaDirector.stopped -= OnElleniaPlayableDone;    
    }

    protected override void Update()
    {
        base.HandleDialogueAction();
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

        return !string.IsNullOrEmpty(Password) && Password == pw;
    }

    public void ElleniaIntroDoneDialogueNodes()
    {
        Ellenia.SwitchPsychicNodes(NoIntroElleniaNodes);
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
            // start dialogue & fade out music
            Script_DialogueManager.DialogueManager.StartDialogueNode(cutSceneNodes[5], false);
            StartCoroutine(
                Script_AudioMixerFader.Fade(
                    audioMixer,
                    Const_AudioMixerParams.ExposedBGVolume,
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
        else if (aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[6])
        {
            Script_DialogueManager.DialogueManager.StartDialogueNode(beforeExitNode, SFXOn: true);    
        }
        else if (aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[7])
        {
            OnElleniaExitsDone();
        }

        void OnReturnedToEasle()
        {
            Script_VCamManager.VCamMain.SwitchToMainVCam(followElleniaVCam);
            Script_DialogueManager.DialogueManager.StartDialogueNode(cutSceneNodes[4], false);
        }
    }
    /// <summary>
    /// NextNodeAction() START =====================================================================
    /// </summary>
    
    /// Node: "i'm part of the..."
    public void PlayOminousMusic()
    {
        game.ChangeStateCutScene();
        Ellenia.FaceDirection(Directions.Left);
        // fade out music and pause
        StartCoroutine(
            Script_AudioMixerFader.Fade(
                audioMixer,
                Const_AudioMixerParams.ExposedMasterVolume,
                bgMusicFadeOutTime,
                0f,
                () => {
                    // after faded out, pause bg music and start theme music where Ellenia
                    // should turn on beat
                    game.PauseBgMusic();
                    Script_AudioMixerVolume
                        .SetVolume(audioMixer, Const_AudioMixerParams.ExposedMasterVolume, 1f);
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
        print("Ellenia walking to paintings cut scene");
        game.ChangeStateCutScene();

        Script_VCamManager.VCamMain.SetNewVCam(followElleniaVCam);
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
                    Const_AudioMixerParams.ExposedMasterVolume,
                    bgMusicEndIntroFadeOutTime,
                    0f,
                    () => {
                        Script_AudioMixerVolume.SetVolume(
                            audioMixer,
                            Const_AudioMixerParams.ExposedMasterVolume,
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
        
        if (!spokenWithEllenia)     ElleniaIntroDoneDialogueNodes();
        spokenWithEllenia = true;
    }

    public void ElleniaOnCorrect()
    {
        // jump animation; when finished, triggers correct dialogue
        game.ChangeStateCutScene();
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 5);
        isPuzzleComplete = true;
    }

    public void GiveSticker()
    {
        game.HandleItemReceive(AnimalWithinSticker);
    }

    public void OnAnimalWitinDescriptionDone()
    {
        Script_DialogueManager.DialogueManager.StartDialogueNode(onItemDescriptionDoneNode, false);
    }

    public void CorrectDone()
    {
        if (!spokenWithEllenia)     ElleniaIntroDoneDialogueNodes();
        spokenWithEllenia = true;
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 6);
        // fade music back in
    }

    public void ElleniaExit()
    {
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 7);
    }

    /// Called from BeforeNodeAction
    public void BeforeYellAtPlayer()
    {
        Ellenia.FacePlayer();
    }
    public void OnYellAtPlayerDone()
    {
        Ellenia.FaceDefaultDirection();
    }
    /// <summary>
    /// NextNodeAction() END =====================================================================
    /// </summary>
    
    private void OnElleniaExitsDone()
    {
        Script_VCamManager.VCamMain.SwitchToMainVCam(followElleniaVCam);
        game.UnPauseBgMusic();
        easleYellAtPlayerIOText.gameObject.SetActive(false);
        StartCoroutine(
            Script_AudioMixerFader.Fade(
                audioMixer,
                Const_AudioMixerParams.ExposedBGVolume,
                bgMusicEndIntroFadeOutTime,
                1f,
                () => {
                    easleFullArt.gameObject.SetActive(true);
                    game.ChangeStateInteract();
                }
            )
        );   
    }
    /* =========================================================================================== */
    
    public override void Setup()
    {
        if (isPuzzleComplete)
        {
            Ellenia.gameObject.SetActive(false);
            easleYellAtPlayerIOText.gameObject.SetActive(false);
            easleFullArt.gameObject.SetActive(true);
        }
        else
        {
            game.SetupMovingNPC(Ellenia, isInitialization);
            easleYellAtPlayerIOText.gameObject.SetActive(true);
            easleFullArt.gameObject.SetActive(false);

            if (spokenWithEllenia)
            {
                ElleniaIntroDoneDialogueNodes();
            }
        }
        
        game.SetupInteractableObjectsText(textParent, isInitialization);
        game.SetupInteractableFullArt(fullArtParent, isInitialization);
        
        // PG Version
        if (Const_Dev.IsPGVersion)  dirtyMagazine.gameObject.SetActive(false);

        isInitialization = false;
    }

    public void GetDirection()
    {
        Script_Utils.GetDirectionToTarget(
            Ellenia.transform.position,
            Script_Game.Game.GetPlayer().transform.position
        );
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_LevelBehavior_25))]
public class Script_LevelBehavior_25Tester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_LevelBehavior_25 lb = (Script_LevelBehavior_25)target;
        if (GUILayout.Button("SetNewElleniaPassword()"))
        {
            lb.devLB21.SetNewElleniaPassword();
        }
        if (GUILayout.Button("ElleniaIntroDoneDialogueNodes()"))
        {
            lb.ElleniaIntroDoneDialogueNodes();
        }
    }
}
#endif