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

    private const string DefaultSatDayNotificationTimeId = "day-notification_time_sat";
    private const string DefaultSatDayNotificationTitleId = "day-notification_title_sat";
    private const string DefaultSatDayNotificationSubtitleId = "day-notification_subtitle_sat";
    private const string SatR2DayNotificationTimeId = "day-notification_time_sat_R2";
    private const string SatR2DayNotificationTitleId = "day-notification_title_sat_R2";
    private const string SatR2DayNotificationSubtitleId = "day-notification_subtitle_sat_R2";
    
    [SerializeField] private Script_RunsManager runsManager;    
    [SerializeField] private Script_DayNotification saturdayNotification;
    [SerializeField] private Script_DayNotification saturdayR2Notification;
    [SerializeField] private Script_DayNotification saturdayFirstR2Notification;
    [SerializeField] private Script_TMProPopulator[] saturdayFirstR2NotificationTexts;
    [SerializeField] private Script_DayNotification sundayNotification;

    [SerializeField] private List<GameObject> saturdayNotificationObjectsToBind;
    [SerializeField] private List<GameObject> saturdayR2NotificationObjectsToBind;

    [SerializeField] private Script_Game game;
    [SerializeField] private Script_GlitchFXManager glitchManager;

    private Action onTimelineDoneAction;
    private bool isInteractAfter = true;
    private Script_TimelineController timelineController;

    public void PlayDayNotification(
        Action cb = null,
        bool _isInteractAfter = true
    )
    {
        game.ChangeStateCutScene();
        int directorIdx = 0;
        int timelineIdx = 0;

        if (game.IsFirstThursday)
        {
            timelineIdx = 1;

            // Inject old Text; timeline will switch it out with zalgofied
            saturdayFirstR2NotificationTexts[0].UpdateTextId(DefaultSatDayNotificationTimeId);
            saturdayFirstR2NotificationTexts[1].UpdateTextId(DefaultSatDayNotificationTitleId);
            saturdayFirstR2NotificationTexts[2].UpdateTextId(DefaultSatDayNotificationSubtitleId);
        }
        else if (game.RunCycle == Script_RunsManager.Cycle.Sunday)
        {
            timelineIdx = 2;
        }
        else
        {
            TimelineAsset timeline = timelineController.timelines[timelineIdx];
            PlayableDirector director = timelineController.playableDirectors[directorIdx];

            List<GameObject> objectsToBind = runsManager.RunCycle switch
            {
                Script_RunsManager.Cycle.Weekday => saturdayNotificationObjectsToBind,
                Script_RunsManager.Cycle.Weekend => saturdayR2NotificationObjectsToBind,
                _ => saturdayNotificationObjectsToBind
            };
            
            director.BindTimelineTracks(timeline, objectsToBind);
        }
        
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

    // Day Notification First R2 Timeline
    public void SetUIDayNotificationGlitch(bool isOn)
    {
        if (isOn)
        {
            glitchManager.SetUIDayNotification();
            glitchManager.SetBlend(1f);
        }
        else
        {
            glitchManager.SetBlend(0f);
        }
    }

    // Day Notification First R2 Timeline
    // Inject new text.
    public void SwitchFirstR2DayNotificationText()
    {
        saturdayFirstR2NotificationTexts[0].UpdateTextId(SatR2DayNotificationTimeId);
        saturdayFirstR2NotificationTexts[1].UpdateTextId(SatR2DayNotificationTitleId);
        saturdayFirstR2NotificationTexts[2].UpdateTextId(SatR2DayNotificationSubtitleId);
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
        saturdayFirstR2Notification.Setup();
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