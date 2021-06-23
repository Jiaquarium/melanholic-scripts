using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Script_TimelineSequenceController : MonoBehaviour
{
    [SerializeField] protected PlayableDirector director;
    
    public virtual void Play()
    {
        director.Play();
    }

    public virtual void Pause()
    {
        director.Pause();
    }
}
