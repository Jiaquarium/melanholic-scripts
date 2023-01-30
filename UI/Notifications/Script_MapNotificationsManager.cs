using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Map notification should type out map name. Should be fully typed out roughly when the level
/// has fully faded in.
/// </summary>
public class Script_MapNotificationsManager : MonoBehaviour
{
    public static Script_MapNotificationsManager Control;

    public enum Type
    {
        Default,
        SpecialIntro,
        HotelLobby
    }

    [SerializeField] private float defaultFadeInTime;
    [SerializeField] private float specialIntroFadeInTime;
    [SerializeField] private float defaultFadeOutTime;
    [SerializeField] private float specialIntroFadeOutTime;
    [SerializeField] private float textCanvasGroupFadeOutTime;
    
    /// <summary>
    /// 0.4 will be standardized because the shortest fade time right now is 0.35f.
    /// </summary>
    [Tooltip("Time before level fully fades in to start teletype")]
    [SerializeField] private float teletypeBufferTime;
    
    [SerializeField] private Script_MapNotification mapNotification;
    [SerializeField] private Canvas mapNotificationCanvas;
    [SerializeField] private int specialIntroSortingOrder;
    
    [SerializeField] private float duration;
    
    [SerializeField] private Script_Game game;

    [SerializeField] private int sortingOrderDefault;
    private Action onCloseAction;
    private float currentDuration;
    private bool isWorldPaintingIntro;
    private Coroutine fadeOutCoroutine;

    public float SpecialIntroFadeOutTime => specialIntroFadeOutTime;
    public float TextCanvasGroupFadeOutTime => textCanvasGroupFadeOutTime;

    /// <summary>
    /// NOTE: Ensure this happens before other cut scenes if any in rooms
    /// in Awake or After Init functions.
    /// </summary>
    public void PlayMapNotification(
        string mapName,
        Action cb = null,
        float customDuration = -1f,
        bool isSFXOn = false,
        AudioClip sfx = null,
        Type type = Type.Default
    )
    {
        float _fadeInTime;
        float _delayTeletypeTime;
        bool _isWorldPaintingIntro;
        
        switch (type)
        {
            case Type.SpecialIntro:
                _fadeInTime = specialIntroFadeInTime;
                _delayTeletypeTime = 0f;
                _isWorldPaintingIntro = true;
                break;
            case Type.HotelLobby:
                _fadeInTime = specialIntroFadeInTime;
                _delayTeletypeTime = 0f;
                _isWorldPaintingIntro = false;
                break;
            default:
                _fadeInTime = defaultFadeInTime;
                _delayTeletypeTime = GetDelayTeletypeTime();
                _isWorldPaintingIntro = false;
                break;
        }
        
        mapNotification.Open(
            text: mapName,
            isSFXOn: isSFXOn,
            sfx: sfx,
            fadeInTime: _fadeInTime,
            delayTeletypeTime: _delayTeletypeTime
        );

        onCloseAction = cb;

        currentDuration = customDuration > 0f ? customDuration : duration;
        isWorldPaintingIntro = _isWorldPaintingIntro;
        
        mapNotificationCanvas.sortingOrder = _isWorldPaintingIntro
            ? specialIntroSortingOrder
            : sortingOrderDefault;
    }

    private float GetDelayTeletypeTime()
    {
        float currentLevelFadeInTime = game.GetCurrentFadeInTime();
        float timeAfterChangeLevel = Mathf.Max(currentLevelFadeInTime - teletypeBufferTime, 0f);
        
        Dev_Logger.Debug($"{name} current delay teletype time for {game.levelBehavior.name}: {timeAfterChangeLevel}");
        
        return timeAfterChangeLevel;
    }

    // ----------------------------------------------------------------------
    // Unity Events

    // Note: NOT used by special intros
    public void OnTeletypeDone()
    {
        Script_TransitionsEventsManager.MapNotificationTeletypeDone(isWorldPaintingIntro);
        
        // OnTeletypeDone will be handled by respective Worlds
        if (isWorldPaintingIntro)
        {
            isWorldPaintingIntro = false;
            return;
        }
        
        fadeOutCoroutine = StartCoroutine(WaitToEndNotification());

        IEnumerator WaitToEndNotification()
        {
            yield return new WaitForSeconds(currentDuration);

            mapNotification.Close(() => {
                    Dev_Logger.Debug($"{name} Setting to initial state");

                    InitialState();
                    Script_TransitionsEventsManager.MapNotificationDefaultDone();
                    
                    if (onCloseAction != null)
                    {
                        Dev_Logger.Debug($"{name} Running onCloseAction callback");

                        onCloseAction();
                        onCloseAction = null;
                    }
                },
                defaultFadeOutTime
            );
        }    
    }

    // ----------------------------------------------------------------------

    private void SetSortingOrderDefault()
    {
        sortingOrderDefault = mapNotificationCanvas.sortingOrder;
    }
    
    public void InitialState()
    {
        StopCoroutines();
        mapNotification.InitialState();
        
        mapNotificationCanvas.sortingOrder = sortingOrderDefault;

        void StopCoroutines()
        {
            if (fadeOutCoroutine != null)
            {
                StopCoroutine(fadeOutCoroutine);
                fadeOutCoroutine = null;
            }
        }
    }
    
    public void Setup()
    {
        if (Control == null)
        {
            Control = this;
        }
        else if (Control != this)
        {
            Destroy(this.gameObject);
        }

        // Ensure nothing sets mapNotificationCanvas.sortingOrder before caching its default value here
        SetSortingOrderDefault();
        mapNotification.Setup();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_MapNotificationsManager))]
public class Script_MapNotificationsManagerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_MapNotificationsManager t = (Script_MapNotificationsManager)target;
        if (GUILayout.Button("Play Map Notification"))
        {
            t.PlayMapNotification("Test Map Notification");
        }

        if (GUILayout.Button("Play Map Notification with SFX"))
        {
            t.PlayMapNotification("Test Map Notification", isSFXOn: true);
        }
    }
}
#endif