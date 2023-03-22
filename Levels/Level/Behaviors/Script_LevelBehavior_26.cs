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
    public const string MapName = Script_Names.InsideAPainting;
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
    
    // ----------------------------------------------------------------------
    // Auto Saves
    public int spikeRoomTryCounterAutoUpdate;
    /* ======================================================================= */
    
    public bool[] switchesState;
    public bool isCurrentPuzzleComplete;
    
    [SerializeField] private Script_SpritesChildrenController lightsParentSpritesController;
    [SerializeField] private Transform switchParent;
    
    [SerializeField] private Script_LevelAttackController attackController;
    
    [SerializeField] private Script_Switch puzzleSwitch;
    
    [SerializeField] Script_StickerObject iceSpike;
    [SerializeField] Transform spikeCage;
    
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float musicFadeOutSpeed;
    [SerializeField] private Script_BgThemePlayer bgThemePlayer;
    
    [SerializeField] private float beforePaintingDoneCutSceneWaitTime;

    [SerializeField] private Script_DialogueManager dialogueManager;
    
    [Space][Header("Level Attack Controller Settings")][Space]
    
    // Must be >1 because because the spike animation lasts 1 sec (30 frames)
    // 1.9s is very tight 8 moves; 1.95s is reasonable
    // 23 frames at 30 fps, the hitbox is exposed (0.7666 sec) meaning 1.95 - 0.7666 = 1.1833
    // So Player has 1.1833s to move 8 spaces (with default speed 8 spaces takes 1.0666 sec)
    // They have .11666 margin for error (~7 frames)
    // We can make it easier by increasing it by .125 (+.1 is still semi-tight), up to a max of 2.25.
    [Range(1.80f, 2.50f)][SerializeField] private float initialAttackInterval;
    [Range(0.10f, 0.125f)][SerializeField] private float attackIntervalDifficultyInterval;
    [SerializeField] private float attackIntervalMax;
    [SerializeField] private int hitCounter;
    
    [Space][Header("Myne's Challenge Settings")][Space]
    [SerializeField] private Script_DialogueNode[] mynesStopDialogue;
    [SerializeField] private Script_DialogueNode dramaDoneRepeatDialogue;
    
    [Space][Header("Demo")][Space]
    [SerializeField] private Script_DemoNoteController demoNoteController;

    [Space][Header("Other")][Space]
    [SerializeField] private Script_CrackableStats giantFinalIce;
    
    // Dev Only
    [SerializeField] private Script_Marker devHalfTriggerHalfSpikesLocation;
    [SerializeField] private Script_Trigger dramaticThoughtsTrigger;
    
    private Script_LBSwitchHandler switchHandler;
    private bool isPauseSpikes;

    private bool isDramaDoneTriggerOff = false;
    private bool isDramaCutSceneActivated = false;
    
    private bool didMapNotification;

    private Coroutine fadingOutMusicCoroutine;

    private bool isTimelineControlled = false;
    
    private bool isInitialize = true;
    private bool isDemoEnd;

    // Difficulty Flags
    // Flag to track if crossed drama trigger regardless if short or full cut scene
    private bool didActivateDramaTriggerInRoom;

    public bool IsDemoEnd
    {
        get => isDemoEnd;
        set => isDemoEnd = value;
    }

    public int HitCounter
    {
        get => hitCounter;
        set => hitCounter = value;
    }

    public bool DidDieFromSpike { get; set; }

    protected override void OnEnable()
    {
        Script_GameEventsManager.OnLevelInitComplete        += OnLevelInitCompleteEvent;
        Script_GameEventsManager.OnLevelBlackScreenDone     += OnLevelBlackScreenDone;

        Script_InteractableObjectEventsManager.OnSwitchOff  += OnSwitchOff;
        Script_HurtBoxEventsManager.OnPlayerRestart         += OnPlayerRestartHandleState;
        Script_HurtBoxEventsManager.OnPlayerRestartTeleport += OnPlayerRestartTeleport;
        Script_HurtBoxEventsManager.OnHurt                  += OnPlayerRestartHandleBgm;
        Script_ItemsEventsManager.OnItemPickUp              += OnItemPickUp;
        Script_InteractableObjectEventsManager.OnInteractAfterShatter += OnInteractAfterShatter;
        Script_ClockEventsManager.OnFastForwardTimesUp      += OnFastForwardTimesUp;
    }

    protected override void OnDisable()
    {
        Script_GameEventsManager.OnLevelInitComplete        -= OnLevelInitCompleteEvent;
        Script_GameEventsManager.OnLevelBlackScreenDone     -= OnLevelBlackScreenDone;
        
        Script_InteractableObjectEventsManager.OnSwitchOff  -= OnSwitchOff;
        Script_HurtBoxEventsManager.OnPlayerRestart         -= OnPlayerRestartHandleState;
        Script_HurtBoxEventsManager.OnPlayerRestartTeleport -= OnPlayerRestartTeleport;
        Script_HurtBoxEventsManager.OnHurt                  -= OnPlayerRestartHandleBgm;
        Script_ItemsEventsManager.OnItemPickUp              -= OnItemPickUp;
        Script_InteractableObjectEventsManager.OnInteractAfterShatter -= OnInteractAfterShatter;
        Script_ClockEventsManager.OnFastForwardTimesUp      -= OnFastForwardTimesUp;

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
        attackController.AttackTimer(isPauseSpikes || isCurrentPuzzleComplete || IsDemoEnd);
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

    private void OnLevelBlackScreenDone()
    {
        if (!didMapNotification)
        {
            Script_MapNotificationsManager.Control.PlayMapNotification(MapName);
            didMapNotification = true;
        }
    }
    
    private void OnLevelInitCompleteEvent()
    {
        
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

                HandleSpikePerfectAchievement();                
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

        return false;

        void HandleSpikePerfectAchievement()
        {
            if (HitCounter == 0)
                Script_AchievementsManager.Instance.UnlockSpikePerfect();
        }
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
        // Ignore if this Hurt Event caused Time to run out
        if (Script_ClockManager.Control.ClockState == Script_Clock.States.Done)
        {
            Dev_Logger.Debug($"{name} Ignore trying to restart BGM on Hurt because Time has run out");
            return;
        }

        // Only need to handle this on DramaDone trigger
        if (!isDramaDoneTriggerOff)
            return;

        Dev_Logger.Debug($"OnPlayerRestartHandleBgm() hurtbox tag: {tag}, hitBox tag: {hitBox.tag}");
        
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

    // Event when Player is hit by Spike and the screen has finished fading to black (same frame where
    // player teleports)
    private void OnPlayerRestartTeleport(Collider col)
    {
        HitCounter++;

        // If have gotten to very end on this run, then set new difficulty level
        if (didActivateDramaTriggerInRoom)
        {
            float rawAttackInterval = initialAttackInterval + (attackIntervalDifficultyInterval * spikeRoomTryCounterAutoUpdate);
            float newAttackInterval = Mathf.Min(rawAttackInterval, attackIntervalMax);
            attackController.AttackInterval = newAttackInterval;

            Dev_Logger.Debug($"{name} Setting new newAttackInterval {newAttackInterval}");
        }
    }
    
    /// <summary>
    /// Handle the case where player is left in Talking state from Myne Dialogue and is
    /// hit in during the Dialogue.
    /// </summary>
    private void OnPlayerRestartHandleState(Collider col)
    {
        var isCurrentNodeInMynes = false;
        foreach (Script_DialogueNode node in mynesStopDialogue)
        {
            if (node == dialogueManager.currentNode)
            {
                isCurrentNodeInMynes = true;
                break;
            }
        }
        
        Dev_Logger.Debug($"OnPlayerRestartHandleState, player.State: {game.GetPlayer().State}; isCurrentNodeInMynes: {isCurrentNodeInMynes}");

        // Check if player is in dialogue state and currentNode is any of the Myne Nodes
        if (game.GetPlayer().State == Const_States_Player.Dialogue && isCurrentNodeInMynes)
        {
            Dev_Logger.Debug($"Handling Restart during Dialogue, Current Node {dialogueManager.currentNode}");
            game.ChangeStateCutScene();
        }

        // Must set this flag, so the drama trigger can react with the repeat cutscene
        isDramaCutSceneActivated = false;
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
            didActivateDramaTriggerInRoom = true;

            if (!didActivateDramaticThoughts)
                FullCutScene();
            else
                RepeatCutScene();
        }

        void FullCutScene()
        {
            didActivateDramaticThoughts = true;

            // Dramatic Cut Scene timeline should change camera AND call signal 1 frame before
            // timeline is done for a more seamless transition.
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
        Dev_Logger.Debug("override SetSwitchState()");
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

    // Reaction to times up from Fast Forwarding time (Spike Hit)
    private void OnFastForwardTimesUp()
    {
        if (!DidDieFromSpike)
        {
            spikeRoomTryCounterAutoUpdate++;
            game.SaveAutoSaves();
        }
        
        DidDieFromSpike = true;
    }

    // ----------------------------------------------------------------------
    // Timeline Signals

    // 1) Dramatic Thoughts Timeline
    // 2) Repeat Drama Node Action
    public void OnDramaticThoughtsCutsceneDone()
    {
        GetComponent<AudioSource>().PlayOneShot(Script_SFXManager.SFX.ThoughtsDone, Script_SFXManager.SFX.ThoughtsDoneVol);
        
        game.ChangeStateInteract();
        
        PlayDramaticMusic();

        Script_AudioMixerVolume.SetVolume(
            audioMixer, BGMParam, 1f
        );
        
        isPauseSpikes = false;
        
        // Make Attack instantly after cut scene is done.
        attackController.Timer = 0.001f;
        lightsParentSpritesController.SetAlpha = 1f;
    }
    
    public void OnEileensMindPaintingTimelineDone()
    {
        isTimelineControlled = true;
        
        var transitionManager = Script_TransitionManager.Control;
        transitionManager.OnCurrentQuestDone(
            allQuestsDoneCb: () =>
            {
                isTimelineControlled = false;
                
                transitionManager.FinalCutSceneAwakening();
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
        Dev_Logger.Debug($"Hiding player {game.GetPlayer()}");
        game.GetPlayer().SetInvisible(true, 0f);
    }

    public void UnhidePlayer()
    {
        game.GetPlayer().SetInvisible(false, 0f);
    }

    private void PlayDramaticMusic()
    {
        bgThemePlayer.gameObject.SetActive(true);
        game.PauseBgMusic();
    }

    private void PlayDefaultMusic()
    {
        bgThemePlayer.gameObject.SetActive(false);
        bgThemePlayer.SoftStop();
        game.StartBgMusicNoFade();
    }

    private void OnInteractAfterShatter(Script_CrackableStats ice)
    {
        if (Const_Dev.IsDemo && ice == giantFinalIce)
        {
            giantFinalIce.IsIcePersists = true;
            demoNoteController.ActivateDemoText(Script_DemoNoteController.Levels.SpikeRoom);
        }

        // Break first ice achievement (this happens a few moments after diagonal cut and ice shatter)
        Script_AchievementsManager.Instance.UnlockBreakIce();
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

        attackController.AttackInterval = initialAttackInterval;
        didActivateDramaTriggerInRoom = false;
        
        // Should always start with Default music.
        // If the player comes back, they would have already triggered FadeOutDramaticMusic
        // from the final trigger, leaving with the room being silent.
        PlayDefaultMusic();

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

            if (GUILayout.Button("Dramatic Thoughts Cut Scene"))
            {
                var dramaticThoughtsLocation = t.dramaticThoughtsTrigger.transform.position;
                Script_Game.Game.GetPlayer().Teleport(dramaticThoughtsLocation);
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

            if (GUILayout.Button("Fade Out Dramatic Music"))
            {
                t.FadeOutDramaticMusic();
            }
        }
    }
#endif
}
