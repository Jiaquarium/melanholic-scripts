using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_48 : Script_LevelBehavior
{
    // ==================================================================
    // State Data
    [SerializeField] private bool isDone;
    // ==================================================================

    [SerializeField] private float panicTime;
    
    [SerializeField] private Script_Snow snowEffectAlways;

    [SerializeField] private PlayableDirector newWorldPaintingsDirector;

    [SerializeField] private Script_CrackableStats iceBlockStatsLeft;
    [SerializeField] private Script_CrackableStats iceBlockStatsMid;
    [SerializeField] private Script_CrackableStats iceBlockStatsRight;

    [SerializeField] private bool isIceBlockLeftCracked;
    [SerializeField] private bool isIceBlockMidCracked;
    [SerializeField] private bool isIceBlockRightCracked;

    [SerializeField] private List<GameObject> iceBlockTimelineObjects;
    [SerializeField] private List<CinemachineVirtualCamera> virtualCameras;
    [SerializeField] private List<Script_InteractablePaintingEntrance> worldPaintings;

    [SerializeField] private TimelineAsset newWorldPaintingRevealTimeline;

    [SerializeField] private Script_LevelBehavior_20 ballroomBehavior;

    private Script_TimelineController timelineController;
    private Script_CrackableStats currentIceBlockStats;

    public bool IsDone
    {
        get => isDone;
        set => isDone = value;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        Script_InteractableObjectEventsManager.OnIceCrackingTimelineDone += PlayRevealNewWorldTimeline;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        Script_InteractableObjectEventsManager.OnIceCrackingTimelineDone -= PlayRevealNewWorldTimeline;
    }
    
    void Awake()
    {
        timelineController = GetComponent<Script_TimelineController>();
        
        snowEffectAlways.gameObject.SetActive(true);
        
        // Initialize World Painting States for Reveal Cut Scenes.
        ballroomBehavior.SetPaintingEntrancesActive(false);
    }

    // ------------------------------------------------------------------
    // Timeline Signals
    public void SetWorldPaintingSketch()
    {
        if (currentIceBlockStats == iceBlockStatsLeft)
        {
            worldPaintings[0].SetSketchAnimation();   
        }
        else if (currentIceBlockStats == iceBlockStatsMid)
        {
            worldPaintings[1].SetSketchAnimation();   
        }
        else if (currentIceBlockStats == iceBlockStatsRight)
        {
            worldPaintings[2].SetSketchAnimation();
        }
    }

    public void SetWorldPaintingStateActive()
    {
        if (currentIceBlockStats == iceBlockStatsLeft)
        {
            worldPaintings[0].SetStateActive();   
        }
        else if (currentIceBlockStats == iceBlockStatsMid)
        {
            worldPaintings[1].SetStateActive();   
        }
        else if (currentIceBlockStats == iceBlockStatsRight)
        {
            worldPaintings[2].SetStateActive();
        }
    }

    // Called from end of NewWorldRevealPaintingTimeline
    public void OnNewWorldRevealPaintingDone()
    {
        // If all three ice blocks have been destroyed, start the Awakening scene.
        if (IsAllIceBlocksCracked())
        {
            AwakeningCutscene();
        }
        else
        {
            game.ChangeStateInteract();
        }
        
        void AwakeningCutscene()
        {
            // Start heavy glitch effect if was third ice stat.
            var glitchManager = Script_GlitchFXManager.Control;
            
            glitchManager.SetPanic();
            glitchManager.BlendTo(1f, 5f, () => Script_TransitionManager.Control.FadeInCoroutine(
                Script_TransitionManager.FadeTimeSlow,
                () => FadeInStartTimeline()
            ));

            void FadeInStartTimeline()
            {
                // Show Awakening canvas immediately (fader transition will handle fading).
                Script_PRCSManager.Control.SetAwakeningActive(true);

                Script_TransitionManager.Control.FadeOutCoroutine(Script_TransitionManager.FadeTimeSlow);
                
                // Awakening Timeline's TimelineTeletypeReveal's UnityEvents will enable/disable
                // Eye's animator during Pause/Play states (Timeline loses control when paused).
                timelineController.PlayableDirectorPlayFromTimelines(1, 1);
            }
        }
    }

    public void OnAwakeningTimelineDone()
    {
        // Set Fade Canvas Active
        Script_TransitionManager.Control.TimelineBlackScreen();

        Script_PRCSManager.Control.SetAwakeningActive(false);
        
        Debug.Log("Waiting for next part in sequence!!!");

        // Wait a few seconds for black screen to stay up and then fade out.
        // Do all inventory calls here.
        
        // Unequip all Prepped Masks
        game.UnequipAll();

        // Give Last Elevator mask in background

        // Equip it in background (without SFX)
    }
    
    // ------------------------------------------------------------------
    // Unity Events

    public void OnIceBlockCracked(Script_CrackableStats ice)
    {
        Debug.Log($"Cracked Left Ice Block <{ice}>");

        game.ChangeStateCutScene();
    }

    public void PlayRevealNewWorldTimeline(Script_CrackableStats iceStats)
    {
        Debug.Log($"Reacting to IceCrackingTimelineDone event with iceStats <{iceStats}>");
        
        currentIceBlockStats = iceStats;

        // Bind the proper objects depending on which crackable stats.
        if (currentIceBlockStats == iceBlockStatsLeft)
        {
            isIceBlockLeftCracked = true;
            newWorldPaintingsDirector.BindTimelineTracks(newWorldPaintingRevealTimeline, iceBlockTimelineObjects);
            timelineController.BindVirtualCameraAndPlayFromDirector(0, 0, virtualCameras[0]);
        }
        else if (currentIceBlockStats == iceBlockStatsMid)
        {
            isIceBlockMidCracked = true;
            newWorldPaintingsDirector.BindTimelineTracks(newWorldPaintingRevealTimeline, iceBlockTimelineObjects);
            timelineController.BindVirtualCameraAndPlayFromDirector(0, 0, virtualCameras[1]);
        }
        else if (currentIceBlockStats == iceBlockStatsRight)
        {
            isIceBlockRightCracked = true;
            newWorldPaintingsDirector.BindTimelineTracks(newWorldPaintingRevealTimeline, iceBlockTimelineObjects);
            timelineController.BindVirtualCameraAndPlayFromDirector(0, 0, virtualCameras[2]);
        }
    }

    private bool IsAllIceBlocksCracked()
    {
        return isIceBlockLeftCracked && isIceBlockMidCracked && isIceBlockRightCracked;
    }
}