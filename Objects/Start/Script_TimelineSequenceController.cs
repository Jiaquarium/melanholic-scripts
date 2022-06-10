using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

/// <summary>
/// Because Pausing timeline at Playhead doesn't gurantee a precise frame,
/// give 8 frames of buffer after a Pause (in the Timeline) to ensure the Timeline
/// doesn't scrub into the next event.
/// </summary>
public class Script_TimelineSequenceController : MonoBehaviour, INotificationReceiver
{
    [SerializeField] protected PlayableDirector director;

    [SerializeField] private List<UnityEvent> pauseActions;
    [SerializeField] private List<UnityEvent> resumeActions;
    
    private bool isListening;

    private int markerActionIndex;

    public double Time
    {
        get => director.time;
    }
    
    // For Dialogue Markers.
    void Update()
    {
        if (
            isListening
            && Script_PlayerInputManager.Instance.MyPlayerInput.actions[Const_KeyCodes.UISubmit].WasPressedThisFrame()
        )
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
        director.Play();
    }

    public virtual void Pause(double time)
    {
        director.Pause();
    }
}
