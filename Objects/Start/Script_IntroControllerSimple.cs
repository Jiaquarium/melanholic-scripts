using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_TimelineController))]
public class Script_IntroControllerSimple : MonoBehaviour
{
    private Script_TimelineController timelineController;

    private Script_TimelineController TimelineController
    {
        get
        {
            if (timelineController == null)
                timelineController = GetComponent<Script_TimelineController>();

            return timelineController;        
        }
    }
    
    void Awake()
    {
        timelineController = GetComponent<Script_TimelineController>();
    }
    
    public void Play()
    {
        gameObject.SetActive(true);
        TimelineController.PlayableDirectorPlayFromTimelines(0, 0);
    }   
}
