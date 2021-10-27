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

    [SerializeField] private Script_Snow snowEffectAlways;

    [SerializeField] private PlayableDirector newWorldPaintingsDirector;

    [SerializeField] private Script_CrackableStats iceBlockStatsLeft;
    [SerializeField] private Script_CrackableStats iceBlockStatsMid;
    [SerializeField] private Script_CrackableStats iceBlockStatsRight;

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

    public void OnNewWorldRevealPaintingDone()
    {
        game.ChangeStateInteract();
    }
    
    // ------------------------------------------------------------------
    // Unity Events

    public void OnIceBlockCrackedLeft(Script_CrackableStats ice)
    {
        Debug.Log($"Cracked Left Ice Block <{ice}>");

        game.ChangeStateCutScene();
    }

    public void OnIceBlockCrackedMid(Script_CrackableStats ice)
    {
        Debug.Log($"Cracked Mid Ice Block <{ice}>");

        game.ChangeStateCutScene();
    }

    public void OnIceBlockCrackedRight(Script_CrackableStats ice)
    {
        Debug.Log($"Cracked Right Ice Block <{ice}>");

        game.ChangeStateCutScene();
    }

    public void PlayRevealNewWorldTimeline(Script_CrackableStats iceStats)
    {
        Debug.Log($"Reacting to IceCrackingTimelineDone event with iceStats <{iceStats}>");
        
        currentIceBlockStats = iceStats;

        // Bind the proper objects depending on which crackable stats.
        if (currentIceBlockStats == iceBlockStatsLeft)
        {
            newWorldPaintingsDirector.BindTimelineTracks(newWorldPaintingRevealTimeline, iceBlockTimelineObjects);
            timelineController.BindVirtualCameraAndPlayFromDirector(0, 0, virtualCameras[0]);
        }
        else if (currentIceBlockStats == iceBlockStatsMid)
        {
            newWorldPaintingsDirector.BindTimelineTracks(newWorldPaintingRevealTimeline, iceBlockTimelineObjects);
            timelineController.BindVirtualCameraAndPlayFromDirector(0, 0, virtualCameras[1]);
        }
        else if (currentIceBlockStats == iceBlockStatsRight)
        {
            newWorldPaintingsDirector.BindTimelineTracks(newWorldPaintingRevealTimeline, iceBlockTimelineObjects);
            timelineController.BindVirtualCameraAndPlayFromDirector(0, 0, virtualCameras[2]);
        }
    }
}