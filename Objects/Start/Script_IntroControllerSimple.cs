using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_TimelineController))]
public class Script_IntroControllerSimple : MonoBehaviour
{
    private Script_TimelineController timelineController;
    
    void Awake()
    {
        timelineController = GetComponent<Script_TimelineController>();
    }
    
    public void Play()
    {
        timelineController.PlayableDirectorPlayFromTimelines(0, 0);
    }   
}
