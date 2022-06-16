using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Timeline;
using UnityEngine.Playables;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Script_TimelineController))]
public class Script_DayNotificationManager : MonoBehaviour
{
    public static Script_DayNotificationManager Control;
    
    [SerializeField] private Script_RunsManager runsManager;    
    [SerializeField] private Script_DayNotification saturdayNotification;
    [SerializeField] private Script_DayNotification saturdayR2Notification;
    [SerializeField] private Script_DayNotification sundayNotification;

    [SerializeField] private List<GameObject> saturdayNotificationObjectsToBind;
    [SerializeField] private List<GameObject> saturdayR2NotificationObjectsToBind;
    [SerializeField] private List<GameObject> sundayNotificationObjectsToBind;

    [SerializeField] private Script_Game game;

    private Action onTimelineDoneAction;
    private bool isInteractAfter = true;
    private Script_TimelineController timelineController;

    public void PlayDayNotification(
        Action cb = null,
        bool _isInteractAfter = true
    )
    {
        game.ChangeStateCutScene();

        int timelineIdx = 0;
        int directorIdx = 0;
        TimelineAsset timeline = timelineController.timelines[timelineIdx];
        PlayableDirector director = timelineController.playableDirectors[directorIdx];

        List<GameObject> objectsToBind = runsManager.RunCycle switch
        {
            Script_RunsManager.Cycle.Weekday => saturdayNotificationObjectsToBind,
            Script_RunsManager.Cycle.Weekend => saturdayR2NotificationObjectsToBind,
            Script_RunsManager.Cycle.Sunday => sundayNotificationObjectsToBind,
            _ => saturdayNotificationObjectsToBind
        };
        
        director.BindTimelineTracks(timeline, objectsToBind);
        
        if (cb != null)
            onTimelineDoneAction = cb;
        
        isInteractAfter = _isInteractAfter;
        
        timelineController.PlayableDirectorPlayFromTimelines(directorIdx, timelineIdx);
    }

    // ----------------------------------------------------------------------
    // Timeline Signals

    // From Day Notification Timeline.
    public void PlayDawnSFX()
    {
        var SFXManager = Script_SFXManager.SFX;
        var isWeekend = game.RunCycle == Script_RunsManager.Cycle.Weekend;
        
        if (isWeekend)
            SFXManager.PlayDawnWeekend();
        else
            SFXManager.PlayDawn();
    }
    
    public void OnDayNotificationDone()
    {
        if (isInteractAfter)
            game.ChangeStateInteract();

        isInteractAfter = true;

        if (onTimelineDoneAction != null)
        {
            onTimelineDoneAction();
            onTimelineDoneAction = null;
        }
    }
    
    // ----------------------------------------------------------------------

    public void Setup()
    {
        if (Control == null)
            Control = this;
        else if (Control != this)
            Destroy(this.gameObject);

        timelineController = GetComponent<Script_TimelineController>();

        saturdayNotification.Setup();
        saturdayR2Notification.Setup();
        sundayNotification.Setup();

        isInteractAfter = true;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_DayNotificationManager))]
public class Script_DayNotificationManagerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_DayNotificationManager t = (Script_DayNotificationManager)target;
        if (GUILayout.Button("Play Day Notification"))
        {
            t.PlayDayNotification();
        }
    }
}
#endif