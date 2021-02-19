using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// Standardized Pre Rendered Cut Scenes
/// Allows for interaction with dialogue nodes
/// </summary>
public class Script_PRCS : Script_FullArt
{
    [SerializeField] private List<TimelineAsset> timelines;
    
    /// <summary>
    /// Simplest of PRCS where they are only played once and have a single timeline
    /// </summary>
    public void PlayMyTimeline()
    {
        PlayableDirector playable = GetComponent<PlayableDirector>();
        
        if (playable != null)
        {
            if (playable.playableAsset == null)
            {
                Debug.LogError("You are trying to play a PRCS without a timeline");
                return;
            }
            playable.Play();
        }
    }

    public void PlayTimeline(int i)
    {
        PlayableDirector playable = GetComponent<PlayableDirector>();
        
        if (playable != null)
        {
            if (timelines?.Count > 0 && timelines[i] != null)
            {
                playable.Play(timelines[i]);
                return;
            }
            
            Debug.LogError("You are trying to play a PRCS without a timeline");
        }
    }
}
