using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;

[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_0 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool didStartThought;
    public bool[] demonSpawns;
    public bool isDone;
    /* ======================================================================= */
    // TODO: put into state if we end up needing to track state for TUTORIAL
    /* ======================================================================= */

    [SerializeField] private AudioMixer audioMixer;
    public Script_ProximityFader fader;
    [SerializeField] private SpriteRenderer fadingPlayer;
    [SerializeField] private SpriteRenderer playerGhostToFade;
    private Script_PlayerMovementAnimator playerMovementAnimator;
    private Script_PlayerGhost playerGhost;
    [SerializeField] private Script_DialogueNode eroPanicNode;
    [SerializeField] private Transform demonsParent;
    [SerializeField] private string triggerId;
    [SerializeField] private string hintTriggerId;
    [SerializeField] private Script_MovingNPC Ero;
    [SerializeField] private FadeSpeeds fadeOutBgmSpeed;
    [SerializeField] private PlayableDirector woodsIntroDirector;
    [SerializeField] private Script_VCamera eroVCam;
    [SerializeField] private float afterBadSpecterFadeInWaitTime;
    // set equal to VCam0 cut time to VCam Main so player doesn't see Ero vanish
    [SerializeField] private float eroExitWaitTime;
    [SerializeField] private Script_Hint hint; 
    [SerializeField] private Script_PRCS wellJustOpened; 

    private bool isInit = true;

    private void Awake()
    {
        if (!didStartThought)
        {
            Script_PRCSManager.Control.OpenPRCSNoFade(wellJustOpened);
        }
    }
    
    protected override void OnEnable()
    {
        woodsIntroDirector.stopped                      += OnBadSpectersIntroDone;
    }

    protected override void OnDisable()
    {
        woodsIntroDirector.stopped                      -= OnBadSpectersIntroDone;
    }

    public override void OnLevelInitComplete()
    {
        if (!didStartThought)
        {
            game.ChangeStateCutScene();
            /// Start Timeline fading in the well light
            wellJustOpened.PlayMyTimeline();
        }
    }
    
    public override bool ActivateTrigger(string Id){
        if (Id == triggerId && !isDone)
        {
            game.ChangeStateCutScene();
            
            // fade out BG music and switch to CONFLICT music
            float fadeTime = Script_AudioEffectsManager.GetFadeTime(fadeOutBgmSpeed);
            StartCoroutine(
                Script_AudioMixerFader.Fade(
                    audioMixer,
                    Const_AudioMixerParams.ExposedBGVolume,
                    fadeTime,
                    0f,
                    () => {
                        game.PauseBgMusic();
                        
                        // TODO: Switch to CONFLICT MUSIC
                        game.SwitchBgMusic(14);

                        Script_AudioMixerVolume.SetVolume(
                            audioMixer,
                            Const_AudioMixerParams.ExposedBGVolume,
                            1f
                        );

                        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);
                    }
                )
            );
            
            return true;
        }
        else if (Id == hintTriggerId && !isDone)
        {
            hint.Show();
            return true;
        }        

        return false;
    }

    private void OnBadSpectersIntroDone(PlayableDirector aDirector)
    {
        Ero.gameObject.SetActive(true);
        StartCoroutine(WaitToCutToEro());

        IEnumerator WaitToCutToEro()
        {
            yield return new WaitForSeconds(afterBadSpecterFadeInWaitTime);
            
            // switch VCam to Ero
            Script_VCamManager.VCamMain.SetNewVCam(eroVCam);

            Script_DialogueManager.DialogueManager.StartDialogueNode(eroPanicNode);
        }
    }
    
    /// <summary>
    /// NextNodeAction START ===============================================================
    /// </summary>
    public void OnWellCutSceneDone()
    {
        
    }
    public void OnEndEroPanic()
    {
        // switch VCam back to player
        Script_VCamManager.VCamMain.SwitchToMainVCam(eroVCam);

        StartCoroutine(WaitForEroExit());

        IEnumerator WaitForEroExit()
        {
            yield return new WaitForSeconds(eroExitWaitTime);
            Ero.gameObject.SetActive(false);

            game.ChangeStateInteract();
            isDone = true;
        }
    }
    /// <summary>
    /// NextNodeAction END ===============================================================
    /// </summary>
    /// ===========================================================================================
    /// Signal Reactions START 
    /// ===========================================================================================
    public void OnWellJustOpenedDone()
    {
        Script_PRCSManager.Control.HidePRCS(wellJustOpened, FadeSpeeds.Slow, () => {
            game.ChangeStateInteract();
            didStartThought = true;
        });
    }
    /// Signal Reactions END ========================================================================


    protected override void HandleAction()
    {
        // for cutScene dialogue
        base.HandleDialogueAction();
    }

    public override void Setup()
    {
        playerMovementAnimator = game.GetPlayerMovementAnimator();
        playerGhost = game.GetPlayerGhost();
        fadingPlayer = playerMovementAnimator.GetComponent<SpriteRenderer>();
        playerGhostToFade = playerGhost.spriteRenderer;

        SpriteRenderer[] toFade = new SpriteRenderer[]{
            fadingPlayer,
            playerGhostToFade
        };
        fader.Setup(fadingPlayer, toFade);

        /// <summary> 
        /// Specter Setup START
        /// </summary> 
        if (isInit)
        {
            demonSpawns = new bool[demonsParent.childCount];
            for (int i = 0; i < demonSpawns.Length; i++)
                demonSpawns[i] = true;
        }
        game.SetupDemons(demonsParent, demonSpawns);
        /// Specter Setup END

        game.SetupMovingNPC(Ero, isInit);
        Ero.gameObject.SetActive(false);

        if (isDone)
        {
            demonsParent.gameObject.SetActive(false);
            game.SwitchBgMusic(14);
        }

        isInit = false;
    }
}
