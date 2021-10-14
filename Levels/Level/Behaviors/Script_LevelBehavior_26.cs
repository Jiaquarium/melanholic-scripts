﻿using System.Collections;
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
    [SerializeField] private Script_UrselkAttacks urselkAttacks;
    
    // Must be >1 because because the spike animation lasts 1 sec (30 frames)
    // 1.9s allows for a very tight 7 moves.
    // E.g. time of 2s gives 1s of movement time, 1s of animation where hitbox is exposed.
    [Range(1.80f, 2.00f)][SerializeField] private float attackInterval;
    
    [SerializeField] private float timer;
    [SerializeField] private float dramaActuallyCompleteWaitTime;
    [SerializeField] private Script_Switch puzzleSwitch;
    
    [SerializeField] private PlayableDirector spikeCageDirector;
    [SerializeField] private PlayableDirector dramaticThoughtsDirector;
    [SerializeField] Script_TriggerEnterOnce dramaticThoughtsCutSceneTrigger;
    [SerializeField] Script_TriggerEnterOnce dramaDoneTrigger;
    
    [SerializeField] Script_StickerObject iceSpike;
    [SerializeField] Transform spikeCage;
    
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private FadeSpeeds musicFadeOutSpeed;
    [SerializeField] private Script_BgThemePlayer bgThemePlayer;
    
    [SerializeField] private Script_LightsController lightsToVictoryController;

    [SerializeField] private float beforePaintingDoneCutSceneWaitTime;

    private Script_LBSwitchHandler switchHandler;
    private bool isPauseSpikes;

    private bool isDramaActuallyDone;
    
    private bool didMapNotification;
    
    private bool isInitialize = true;
    private bool isTimelineControlled = false;
    
    protected override void OnEnable()
    {
        Script_GameEventsManager.OnLevelInitComplete        += OnLevelInitCompleteEvent;

        Script_InteractableObjectEventsManager.OnSwitchOff  += OnSwitchOff;
        dramaticThoughtsDirector.stopped                    += OnDramaticThoughtsDone;
        Script_HurtBoxEventsManager.OnHurt                  += OnPlayerRestartHandleBgm;
        Script_ItemsEventsManager.OnItemPickUp              += OnItemPickUp;
    }

    protected override void OnDisable()
    {
        Script_GameEventsManager.OnLevelInitComplete        -= OnLevelInitCompleteEvent;
        
        Script_InteractableObjectEventsManager.OnSwitchOff  -= OnSwitchOff;
        dramaticThoughtsDirector.stopped                    -= OnDramaticThoughtsDone;
        Script_HurtBoxEventsManager.OnHurt                  -= OnPlayerRestartHandleBgm;
        Script_ItemsEventsManager.OnItemPickUp              -= OnItemPickUp;

        bgThemePlayer.gameObject.SetActive(false);

        if (!isTimelineControlled)
        {
            DefaultBgMusicLevels();
        }
    }

    private void Awake()
    {
        switchHandler = GetComponent<Script_LBSwitchHandler>();
        switchHandler.Setup(game);

        if (didActivateDramaticThoughts)
        {
            dramaticThoughtsCutSceneTrigger.gameObject.SetActive(false);
        }
        else
        {
            dramaticThoughtsCutSceneTrigger.gameObject.SetActive(true);
        }

        if (isCurrentPuzzleComplete)    spikeCage.gameObject.SetActive(false);
        else                            spikeCage.gameObject.SetActive(true);

        if (gotIceSpikeSticker)         iceSpike.gameObject.SetActive(false);
        else                            iceSpike.gameObject.SetActive(true);
    }

    protected override void Update()
    {
        AttackTimer();
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
            yield return new WaitForSeconds(attackInterval);
            
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

    private void AttackTimer()
    {
        if (timer == 0)
            timer = attackInterval;

        // Match Player Movement deltaTime.
        timer -= Time.smoothDeltaTime;

        if (timer <= 0 && !isCurrentPuzzleComplete)
        {
            if (!isPauseSpikes)
                Attack();
            
            timer = 0;
        }
    }
    
    public void Attack()
    {
        urselkAttacks.AlternatingSpikesAttack();
    }

    public override bool ActivateTrigger(string Id)
    {
        if (Id == "dramatic-thoughts")
        {
            game.ChangeStateCutScene();
            isPauseSpikes = true;
            return true;
        }
        // On DramaDone trigger, give extra time before
        // actually turning off Drama music because Player may have activated
        // trigger at same time as being struck by Needle HitBox.
        // isDramaActuallyDone is reset (via OnPlayerRestartHandleBgm)
        // when player is hit by Spike.
        else if (Id == "drama-done")
        {
            if (!isCurrentPuzzleComplete)
            {
                isDramaActuallyDone = true;
                StartCoroutine(WaitCheckDramaActuallyComplete());
            }

            return true;
        }

        return false;
    }

    private IEnumerator WaitCheckDramaActuallyComplete()
    {
        yield return new WaitForSeconds(dramaActuallyCompleteWaitTime);

        if (isDramaActuallyDone)
        {
            Debug.Log("Drama is actually done");

            FadeOutDramaticMusic();
            // Need to also fade out these lights for the 8 max light count
            FadeOutDramaticLights();
        }
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

        if (!isDramaActuallyDone)
        {
            Debug.Log($"{name} Ignore trying to restart BGM on Hurt because not checking for drama done");
            return;
        }
        
        Debug.Log($"OnPlayerRestartHandleBgm() hurtbox tag: {tag}, hitBox tag: {hitBox.tag}");
        
        if (tag == Const_Tags.Player)
        {
            Debug.Log($"Resetting drama done timer & trigger");
            isDramaActuallyDone = false;
            dramaDoneTrigger.Reactivate();

            // If FadeOutDramaticMusic was previously called to stop
            // the dramatic music, restart the Bgm speaker.
            if (!bgThemePlayer.gameObject.activeSelf)
            {
                Script_BackgroundMusicManager.Control.SetVolume(1f, BGMParam);
                bgThemePlayer.Play();
            }
        }
    }
    
    private void FadeOutDramaticMusic()
    {
        StartCoroutine(
            Script_AudioMixerFader.Fade(
                audioMixer,
                BGMParam,
                Script_AudioEffectsManager.GetFadeTime(musicFadeOutSpeed),
                0f,
                () => {
                    bgThemePlayer.SoftStop();
                    bgThemePlayer.gameObject.SetActive(false);
                    Script_BackgroundMusicManager.Control.SetVolume(1f, BGMParam);
                }
            )
        );
    }

    private void FadeOutDramaticLights()
    {
        Debug.Log("Fading out lights to victory!!!");
        lightsToVictoryController.ShouldUpdate = true;
    }

    private void HandleDramaticThoughtsCutScene()
    {
        if (isPauseSpikes && timer == 0 && !didActivateDramaticThoughts)
        {
            didActivateDramaticThoughts = true;
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(1, 1);
            
            StartCoroutine(
                Script_AudioMixerFader.Fade(
                    audioMixer,
                    BGMParam,
                    Script_AudioEffectsManager.GetFadeTime(musicFadeOutSpeed),
                    0f,
                    () => game.StopBgMusic()
                )
            );
        }
    }

    private void OnDramaticThoughtsDone(PlayableDirector aDirector)
    {
        GetComponent<AudioSource>().PlayOneShot(Script_SFXManager.SFX.ThoughtsDone, Script_SFXManager.SFX.ThoughtsDoneVol);
        
        game.ChangeStateInteract();
        bgThemePlayer.gameObject.SetActive(true);
        Script_AudioMixerVolume.SetVolume(
            audioMixer, BGMParam, 1f
        );
        isPauseSpikes = false;
        timer = 0.001f; // make Attack instantly after
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

    public void OnEileensMindPaintingTimelineDone()
    {
        isTimelineControlled = true;
        Script_TransitionManager.Control.OnCurrentQuestDone(() => {
            game.ChangeStateInteract();
            isTimelineControlled = false;
        });
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
    
    public override void Setup()
    {
        switchesState = switchHandler.SetupSwitchesState(
            switchParent,
            switchesState,
            isInitialize
        );
        
        timer = attackInterval;
        
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
    }
}
#endif