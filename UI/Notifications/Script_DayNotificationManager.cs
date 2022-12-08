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

    [SerializeField] private Script_Game game;
    [SerializeField] private Script_GlitchFXManager glitchManager;

    private Action onTimelineDoneAction;
    private Action onBeforeFadeOutAction;
    private bool isInteractAfter = true;
    private Script_TimelineController timelineController;
    private bool isShortDayNotificationDay1;
    private bool didOnDayNotificationEnd;

    public PlayableDirector playableDirector => GetComponent<PlayableDirector>();

    public void PlayDayNotification(
        Action cb = null,
        bool _isInteractAfter = true,
        bool isFirstDay = false,
        Action beforeFadeOutCb = null
    )
    {
        isShortDayNotificationDay1 = isFirstDay;
        didOnDayNotificationEnd = false;
        
        game.ChangeStateCutScene();
        int directorIdx = 0;
        int timelineIdx = 0;

        if (isFirstDay)
        {
            timelineIdx = 0;
        }
        else if (game.RunCycle == Script_RunsManager.Cycle.Weekday)
        {
            timelineIdx = 2;
        }
        else if (game.IsFirstThursday)
        {
            timelineIdx = 3;

            // Inject old Text; timeline will switch it out with zalgofied
            saturdayFirstR2NotificationTexts[0].UpdateTextId(DefaultSatDayNotificationTimeId);
            saturdayFirstR2NotificationTexts[1].UpdateTextId(DefaultSatDayNotificationTitleId);
            saturdayFirstR2NotificationTexts[2].UpdateTextId(DefaultSatDayNotificationSubtitleId);
        }
        else if (game.RunCycle == Script_RunsManager.Cycle.Weekend)
        {
            timelineIdx = 4;
        }
        else if (game.RunCycle == Script_RunsManager.Cycle.Sunday)
        {
            timelineIdx = 5;
        }
        
        onBeforeFadeOutAction = beforeFadeOutCb;
        onTimelineDoneAction = cb;
        
        isInteractAfter = _isInteractAfter;
        
        timelineController.PlayableDirectorPlayFromTimelines(directorIdx, timelineIdx);
    }

    public void PlayFadeOutDay1()
    {
        timelineController.PlayableDirectorPlayFromTimelines(0, 1);
    }

    // ----------------------------------------------------------------------
    // Timeline Signals

    // From Day Notification Timeline.
    public void PlayDawnSFX()
    {
        var SFXManager = Script_SFXManager.SFX;
        
        switch (game.RunCycle)
        {
            case Script_RunsManager.Cycle.Weekday:
                SFXManager.PlayDawn();
                break;
            case Script_RunsManager.Cycle.Weekend:
                SFXManager.PlayDawnWeekend();
                break;
            case Script_RunsManager.Cycle.Sunday:
                // TBD
                break;
        }
    }

    // Call 0.5 sec (15 frames) before fully fading out.
    // Play typewriter Map Notification here.
    public void OnBeforeFadeOut()
    {
        if (onBeforeFadeOutAction != null)
        {
            onBeforeFadeOutAction();
            onBeforeFadeOutAction = null;
        }
    }

    public void OnDayNotificationDone()
    {
        if (didOnDayNotificationEnd)
            return;
        
        if (isInteractAfter)
            game.ChangeStateInteract();

        isInteractAfter = true;

        if (onTimelineDoneAction != null)
        {
            onTimelineDoneAction();
            onTimelineDoneAction = null;
        }

        didOnDayNotificationEnd = true;
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
        isShortDayNotificationDay1 = false;
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