using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Script_TimelineSequenceControllerSafe : Script_TimelineSequenceController
{
    [SerializeField] protected TimelineAsset timeline;
    [SerializeField] private List<double> pauseTimes;

    private double originalDuration;
    private DirectorWrapMode originalWrapMode;

    void OnValidate()
    {
        if (timeline != null)
        {
            List<double> times = new List<double>();
            
            foreach (var track in timeline.GetOutputTracks())
            {
                var markers = track.GetMarkers();
                foreach (var marker in markers)
                {
                    if (marker is Script_TimelineControlMarker)
                    {
                        times.Add(marker.time);
                    }
                }
            }

            // Sort from smallest to largest values.
            times.Sort((a, b) => a.CompareTo(b));
            pauseTimes = times;
        }
    }
    
    void OnEnable()
    {
        director.played += Initialize;
        
        originalWrapMode = director.extrapolationMode;
        // director.extrapolationMode = DirectorWrapMode.Hold;
    }

    void OnDisable()
    {
        director.played -= Initialize;
        
        if (director != null)
            director.extrapolationMode = originalWrapMode;
    }

    public override void Play()
    {
        // director.playableGraph.GetRootPlayable(0).SetDuration(originalDuration);

        // StartCoroutine(NextFramePlay());
        // IEnumerator NextFramePlay()
        // {
        //     yield return null;
        //     director.Play();
        // }
    }

    // Must adjust timeline duration and use Hold mode to accurately stop
    // at markers, regardless of frame rate.
    // https://forum.unity.com/threads/playdirector-pause-not-immediate.827373/
    public override void Pause(double time)
    {
        // director.playableGraph.GetRootPlayable(0).SetDuration(time);
    }

    private void Initialize(PlayableDirector aDirector)
    {
        originalDuration = director.playableGraph.GetRootPlayable(0).GetDuration();
    }
}
