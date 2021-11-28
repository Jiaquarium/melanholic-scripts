using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Audio;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Script_LBSwitchHandler))]
[RequireComponent(typeof(Script_TimelineController))]
[RequireComponent(typeof(AudioSource))]
public class Script_LevelBehavior_26 : Script_LevelBehavior
{
    public const string MapName = "Inside a Painting";
    private const string BGMParam = Const_AudioMixerParams.ExposedBGVolume;
    
    private const string MyneChallenge0 = "myne-challenge_0";
    private const string MyneChallenge1 = "myne-challenge_1";
    private const string MyneChallengePassive0 = "myne-challenge_passive_0";
    private const string MyneChallengePassive1 = "myne-challenge_passive_1";
    private const string MyneChallengePassive2 = "myne-challenge_passive_2";
    private const string MyneChallengePassive3 = "myne-challenge_passive_3";
    
    private const string DramaticThoughts = "dramatic-thoughts";
    private const string DramaDone = "drama-done";
    
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool isPuzzleComplete;
    public bool didActivateDramaticThoughts;
    public bool gotIceSpikeSticker;
    /* ======================================================================= */
    
    public bool[] switchesState;
    public bool isCurrentPuzzleComplete;
    [SerializeField] private Transform switchParent;
    
    [SerializeField] private Script_LevelAttackController attackController;
    
    [SerializeField] private Script_Switch puzzleSwitch;
    
    [SerializeField] Script_StickerObject iceSpike;
    [SerializeField] Transform spikeCage;
    
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float musicFadeOutSpeed;
    [SerializeField] private Script_BgThemePlayer bgThemePlayer;
    
    [SerializeField] private float beforePaintingDoneCutSceneWaitTime;

    
    [Header("Myne's Challenge Settings")]
    [SerializeField] private Script_DialogueNode[] mynesStopDialogue;
    [SerializeField] private Script_DialogueNode dramaDoneRepeatDialogue;

    // Dev Only
    [SerializeField] private Script_Marker devHalfTriggerHalfSpikesLocation;
    
    private Script_LBSwitchHandler switchHandler;
    private bool isPauseSpikes;

    private bool isDramaDoneTriggerOff = false;
    private bool isDramaCutSceneActivated = false;
    
    private bool didMapNotification;

    private Coroutine fadingOutMusicCoroutine;

    private bool isTimelineControlled = false;
    
    private bool isInitialize = true;

    protected override void OnEnable()
    {
        Script_GameEventsManager.OnLevelInitComplete        += OnLevelInitCompleteEvent;

        Script_InteractableObjectEventsManager.OnSwitchOff  += OnSwitchOff;
        Script_HurtBoxEventsManager.OnHurt                  += OnPlayerRestartHandleBgm;
        Script_ItemsEventsManager.OnItemPickUp              += OnItemPickUp;
    }

    protected override void OnDisable()
    {
        Script_GameEventsManager.OnLevelInitComplete        -= OnLevelInitCompleteEvent;
        
        Script_InteractableObjectEventsManager.OnSwitchOff  -= OnSwitchOff;
        Script_HurtBoxEventsManager.OnHurt                  -= OnPlayerRestartHandleBgm;
        Script_ItemsEventsManager.OnItemPickUp              -= OnItemPickUp;

        bgThemePlayer.gameObject.SetActive(false);

        if (!isTimelineControlled)
        {
            DefaultBgMusicLevels();
        }

        Script_TeletypeNotificationManager.Control.InitialState();
    }

    private void Awake()
    {
        switchHandler = GetComponent<Script_LBSwitchHandler>();
        switchHandler.Setup(game);

        if (isCurrentPuzzleComplete)    spikeCage.gameObject.SetActive(false);
        else                            spikeCage.gameObject.SetActive(true);

        if (gotIceSpikeSticker)         iceSpike.gameObject.SetActive(false);
        else                            iceSpike.gameObject.SetActive(true);
    }

    protected override void Update()
    {
        attackController.AttackTimer(isPauseSpikes || isCurrentPuzzleComplete);
        HandleDramaticThoughtsCutScene();
    }

    public void PuzzleSuccess()
    {
        game.ChangeStateCutScene();
        StartCoroutine(WaitSpikeCageDown());
        FadeOutDramaticMusic();

        IEnumerator WaitSpikeCageDown()
        {
            isPuzzleComplete = true;
            isCurrentPuzzleComplete = true;
            
            // Ensure spikes are done.
            yield return new WaitForSeconds(attackController.AttackInterval);
            
            // Spike cage down Timeline
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);

            yield return new WaitForSeconds(beforePaintingDoneCutSceneWaitTime);

            // Painting Done Cut Scene Timeline
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(2, 2);
        }
    }

    private void OnLevelInitCompleteEvent()
    {
        if (!didMapNotification)
        {
            Script_MapNotificationsManager.Control.PlayMapNotification(MapName);
            didMapNotification = true;
        }
    }

    private void OnItemPickUp(string itemId)
    {
        if (itemId == iceSpike.Item.id)
        {
            gotIceSpikeSticker = true;
        }
    }

    public override bool ActivateTrigger(string Id)
    {
        if (
            Id == DramaticThoughts
            && !isDramaCutSceneActivated
            && game.state != Const_States_Game.CutScene
        )
        {
            game.ChangeStateCutScene();
            
            // This will trigger the cut scene.
            isPauseSpikes = true;
            
            // Leave trigger active, isDramaCutSceneActivated flag to control state.
            return false;
        }
        // On DramaDone trigger, never inactivate the trigger.
        // Allow it to continue to detect collisions, handle its state here.
        else if (Id == DramaDone)
        {
            if (!isCurrentPuzzleComplete && !isDramaDoneTriggerOff)
            {
                isDramaDoneTriggerOff = true;
                FadeOutDramaticMusic();
            }

            return false;
        }
        else if (Id == MyneChallenge0)
        {
            if (!didActivateDramaticThoughts)
                PlayMynesStopDialogue(0);
            
            return true;
        }
        else if (Id == MyneChallenge1)
        {
            if (!didActivateDramaticThoughts)
                PlayMynesStopDialogue(1);
            
            return true;
        }
        else if (Id == MyneChallengePassive0)
        {
            if (!didActivateDramaticThoughts)
                Script_TeletypeNotificationManager.Control.ShowEileensMindDialogue(0);
            
            return true;
        }
        else if (Id == MyneChallengePassive1)
        {
            if (!didActivateDramaticThoughts)
                Script_TeletypeNotificationManager.Control.ShowEileensMindDialogue(1);
            
            return true;
        }
        else if (Id == MyneChallengePassive2)
        {
            if (!didActivateDramaticThoughts)
                Script_TeletypeNotificationManager.Control.ShowEileensMindDialogue(2);
            
            return true;
        }
        else if (Id == MyneChallengePassive3)
        {
            if (!didActivateDramaticThoughts)
                Script_TeletypeNotificationManager.Control.ShowEileensMindDialogue(3);
            
            return true;
        }

        return false;
    }

    private void PlayMynesStopDialogue(int i)
    {
        game.ChangeStateCutScene();
        Script_DialogueManager.DialogueManager.StartDialogueNode(mynesStopDialogue[i]);
    }

    // Only need to handle Bgm after activating the "drama done" Trigger.
    // If the player gets hit when checking for drama done, stop checking
    // and reset the Enter Once Trigger.
    private void OnPlayerRestartHandleBgm(string tag, Script_HitBox hitBox)
    {
        // Ignore if this Hurt Event caused Time to run out or if we're not checking for
        // drama actually done.
        if (Script_ClockManager.Control.ClockState == Script_Clock.States.Done)
        {
            Debug.Log($"{name} Ignore trying to restart BGM on Hurt because Time has run out");
            return;
        }

        Debug.Log($"OnPlayerRestartHandleBgm() hurtbox tag: {tag}, hitBox tag: {hitBox.tag}");
        
        if (tag == Const_Tags.Player)
        {
            // Allow drama cut scene to be played On Trigger again.
            isDramaCutSceneActivated = false;
            isDramaDoneTriggerOff = false;
            
            // If BgThemePlayer was fading / done fading but player hits a spike.
            // And also we're in the didActivateDramaticThoughts phase.
            if (
                didActivateDramaticThoughts
                && (
                    !bgThemePlayer.gameObject.activeSelf
                    || fadingOutMusicCoroutine != null
                )
            )
            {
                // During fading out music.
                if (fadingOutMusicCoroutine != null)
                {
                    StopCoroutine(fadingOutMusicCoroutine);
                    fadingOutMusicCoroutine = null;
                }

                Script_BackgroundMusicManager.Control.SetVolume(1f, BGMParam);
                
                // If Done fading out music.
                if (!bgThemePlayer.IsPlaying)
                    bgThemePlayer.Play();
            }
        }
    }
    
    private void FadeOutDramaticMusic()
    {
        fadingOutMusicCoroutine = StartCoroutine(
            Script_AudioMixerFader.Fade(
                audioMixer,
                BGMParam,
                musicFadeOutSpeed,
                0f,
                () => {
                    bgThemePlayer.SoftStop();
                    bgThemePlayer.gameObject.SetActive(false);
                    Script_BackgroundMusicManager.Control.SetVolume(1f, BGMParam);
                    
                    fadingOutMusicCoroutine = null;
                }
            )
        );
    }

    private void HandleDramaticThoughtsCutScene()
    {
        var isDramaticThoughts = isPauseSpikes
            && attackController.Timer == 0
            && !isDramaCutSceneActivated;
        
        if (isDramaticThoughts)
        {
            isDramaCutSceneActivated = true;
            
            if (!didActivateDramaticThoughts)
                FullCutScene();
            else
                RepeatCutScene();
        }

        void FullCutScene()
        {
            didActivateDramaticThoughts = true;
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(1, 1);
            
            StartCoroutine(
                Script_AudioMixerFader.Fade(
                    audioMixer,
                    BGMParam,
                    musicFadeOutSpeed,
                    0f,
                    () => game.StopBgMusic()
                )
            );

            Script_TeletypeNotificationManager.Control.InitialState();
        }

        void RepeatCutScene()
        {
            Script_DialogueManager.DialogueManager.StartDialogueNode(dramaDoneRepeatDialogue);
        }
    }

    private void OnSwitchOff(string switchId)
    {
        if (switchId == puzzleSwitch.nameId)
        {
            PuzzleSuccess();
        }
    }

    public override void SetSwitchState(int Id, bool isOn)
    {
        print("override SetSwitchState()");
        switchHandler.SetSwitchState(switchesState, Id, isOn);
    }

    private void DefaultBgMusicLevels()
    {
        Script_AudioMixerVolume.SetVolume(
            audioMixer,
            BGMParam,
            1f
        );   
    }

    // ----------------------------------------------------------------------
    // Timeline Signals

    // 1) Dramatic Thoughts Timeline
    // 2) Repeat Drama Node Action
    public void OnDramaticThoughtsCutsceneDone()
    {
        GetComponent<AudioSource>().PlayOneShot(Script_SFXManager.SFX.ThoughtsDone, Script_SFXManager.SFX.ThoughtsDoneVol);
        
        game.ChangeStateInteract();
        
        bgThemePlayer.gameObject.SetActive(true);
        Script_AudioMixerVolume.SetVolume(
            audioMixer, BGMParam, 1f
        );
        
        isPauseSpikes = false;
        
        // Make Attack instantly after cut scene is done.
        attackController.Timer = 0.001f;
    }
    
    public void OnEileensMindPaintingTimelineDone()
    {
        isTimelineControlled = true;
        
        Script_TransitionManager.Control.OnCurrentQuestDone(
            allQuestsDoneCb: () =>
            {
                isTimelineControlled = false;
                
                // Final Cut Scene Sequence
            
            }, 
            defaultCb: () =>
            {
                isTimelineControlled = false;
                game.ChangeStateInteract();
            }
        );
    }

    public void HidePlayer()
    {
        Debug.Log($"Hiding player {game.GetPlayer()}");
        game.GetPlayer().SetInvisible(true, 0f);
    }

    public void UnhidePlayer()
    {
        game.GetPlayer().SetInvisible(false, 0f);
    }

    // ----------------------------------------------------------------------
    // Next Node Actions

    public void OnMyneChallengeDialogueDone()
    {
        game.ChangeStateInteract();
    }

    // ----------------------------------------------------------------------
    
    public override void Setup()
    {
        switchesState = switchHandler.SetupSwitchesState(
            switchParent,
            switchesState,
            isInitialize
        );
        
        // If player activated the dramatic thoughts, and came back later
        // start off with dramatic music
        if (didActivateDramaticThoughts && !isCurrentPuzzleComplete)
        {
            bgThemePlayer.gameObject.SetActive(true);
            game.PauseBgMusic();
        }
        // If puzzle is done just use default music.
        else
        {
            bgThemePlayer.gameObject.SetActive(false);
        }

        isInitialize = false;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_LevelBehavior_26))]
    public class Script_LevelBehavior_26Tester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_LevelBehavior_26 t = (Script_LevelBehavior_26)target;
            if (GUILayout.Button("Puzzle Success"))
            {
                t.PuzzleSuccess();
            }

            if (GUILayout.Button("Hide Player"))
            {
                t.HidePlayer();
            }

            if (GUILayout.Button("Unhide Player"))
            {
                t.UnhidePlayer();
            }

            if (GUILayout.Button("Move Player 1/2 Spikes 1/2 Trigger"))
            {
                Script_Game.Game.GetPlayer().Teleport(t.devHalfTriggerHalfSpikesLocation.Position);
            }
        }
    }
#endif
}
