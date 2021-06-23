using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Script_TimelineSequenceController : MonoBehaviour, INotificationReceiver
{
    [SerializeField] protected PlayableDirector director;
    
    private bool isListening;

    void Update()
    {
        if (isListening && Input.GetButtonDown(Const_KeyCodes.Submit))
        {
            Debug.Log($"{name} Playing timeline on input");

            Play();

            isListening = false;
        }
    }
    
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        Script_TimelineControlMarker tm = notification as Script_TimelineControlMarker;

        if (tm != null)
        {
            if (tm.isPauseTimelineWaitForInput)
            {
                Debug.Log($"{name} Pausing timeline at TimelineControlMarker");
                
                Pause();
                
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

    public virtual void Pause()
    {
        director.Pause();
    }
}
