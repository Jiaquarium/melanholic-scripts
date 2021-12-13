using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;
using UnityEngine.Events;

/// <summary>
/// Because Pausing timeline at Playhead doesn't gurantee a precise frame,
/// give 8 frames of buffer after a Pause (in the Timeline) to ensure the Timeline
/// doesn't scrub into the next event.
/// </summary>
public class Script_TimelineSequenceController : MonoBehaviour, INotificationReceiver
{
    [SerializeField] protected PlayableDirector director;
    [SerializeField] private TimelineAsset timeline;
    
    [SerializeField] private List<double> pauseTimes;

    [SerializeField] private List<UnityEvent> pauseActions;
    [SerializeField] private List<UnityEvent> resumeActions;
    
    private bool isListening;
    
    private double originalDuration;
    private DirectorWrapMode originalWrapMode;

    private int markerActionIndex;

    public double Time
    {
        get => director.time;
    }
    
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
    
    // For Dialogue Markers.
    void Update()
    {
        if (isListening && Input.GetButtonDown(Const_KeyCodes.Submit))
        {
            Debug.Log($"{name} Playing timeline on input");

            if (markerActionIndex > -1)
            {
                Debug.Log("Invoking Resume Acton.");
                resumeActions[markerActionIndex].SafeInvoke();
                markerActionIndex = -1;
            }

            Play();

            isListening = false;
        }
    }
    
    // For Dialogue Markers.
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        Script_TimelineControlMarker tm = notification as Script_TimelineControlMarker;

        markerActionIndex = -1;

        if (tm != null)
        {
            if (tm.isPauseTimelineWaitForInput)
            {
                Debug.Log($"{name} Pausing timeline at TimelineControlMarker");
                
                if (tm.pauseActionIndex > -1 && tm.isAction)
                {
                    markerActionIndex = tm.pauseActionIndex;
                    pauseActions[markerActionIndex].SafeInvoke();
                }

                Pause(tm.time);
                
                StartCoroutine(ListenOnNextFrame());
            }
        }

        IEnumerator ListenOnNextFrame()
        {
            yield return null;
            
            isListening = true;
        }
    }
    
    public virtual void Play()
    {
        // director.playableGraph.GetRootPlayable(0).SetDuration(originalDuration);

        // StartCoroutine(NextFramePlay());
        // IEnumerator NextFramePlay()
        // {
        //     yield return null;
        //     director.Play();
        // }

        director.Play();
    }

    // Must adjust timeline duration and use Hold mode to accurately stop
    // at markers, regardless of frame rate.
    // https://forum.unity.com/threads/playdirector-pause-not-immediate.827373/
    public virtual void Pause(double time)
    {
        // director.playableGraph.GetRootPlayable(0).SetDuration(time);
        director.Pause();
    }

    private void Initialize(PlayableDirector aDirector)
    {
        originalDuration = director.playableGraph.GetRootPlayable(0).GetDuration();
    }
}
