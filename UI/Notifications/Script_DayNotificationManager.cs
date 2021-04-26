using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Script_TimelineController))]
public class Script_DayNotificationManager : MonoBehaviour
{
    public static Script_DayNotificationManager Control;
    
    [SerializeField] private Script_RunsManager runsManager;    
    [SerializeField] private Script_DayNotification dayNotification;

    [SerializeField] private Script_Game game;

    private Action onTimelineDoneAction;
    private bool isInteractAfter = true;
    private Script_TimelineController timelineController;
    
    public void PlayDayNotification(
        Action cb = null,
        bool _isInteractAfter = true
    )
    {
        string todayName = runsManager.Run.dayName;
        dayNotification.DayText = todayName;
        
        game.ChangeStateCutScene();
        
        if (cb != null) onTimelineDoneAction = cb;
        isInteractAfter = _isInteractAfter;
        
        timelineController.PlayableDirectorPlayFromTimelines(0, 0);
    }

    // ----------------------------------------------------------------------
    // Timeline Signals
    
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

        dayNotification.Setup();

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
        if (GUILayout.Button("Play Map Notification"))
        {
            t.PlayDayNotification();
        }
    }
}
#endif