using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Script_TimelineController))]
public class Script_IntroControllerSimple : MonoBehaviour
{
    [SerializeField] private Script_CanvasGroupController simpleIntroCanvasGroup;
    [SerializeField] private float simpleIntroSkipFadeOutTime;
    [SerializeField] private float waitAfterSkipTime;
    
    [SerializeField] private Script_StartOverviewController startOverviewController;
    private Script_TimelineController timelineController;
    [SerializeField] private Script_CanvasGroupController freedomCanvasGroup;
    [SerializeField] private Script_CanvasGroupController authorsCanvasGroup;
    [SerializeField] private Script_CanvasGroupController contentWarningCanvasGroup;
    [SerializeField] private Image contentWarningContainer;

    public bool isSkipContentWarning { get; set; }
    public bool isDonePlaying { get; set; }
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
        HandleContentWarning();

        isDonePlaying = false;
        gameObject.SetActive(true);
        TimelineController.PlayableDirectorPlayFromTimelines(0, 0);
    }

    public void Stop(bool isForceStop = false)
    {
        TimelineController.StopAllPlayables(isForceStop);
    }

    public void HandleSkip()
    {
        // Pause Timeline & immediately prevent other Timeline Signals from firing with isDonePlaying flag
        TimelineController.PausePlayableDirector(0);
        isDonePlaying = true;
        
        // Fade out CanvasGroup
        simpleIntroCanvasGroup.FadeOut(
            simpleIntroSkipFadeOutTime,
            () => {
                // Must force stop since timeline will be paused here
                Stop(isForceStop: true);
                StartCoroutine(WaitToFadeInTitleScreen());
            },
            isUnscaledTime: true
        );

        IEnumerator WaitToFadeInTitleScreen()
        {
            yield return new WaitForSecondsRealtime(waitAfterSkipTime);
            startOverviewController.FadeInTitleScreen(withCTA: true);
        }
    }

    // ----------------------------------------------------------------------
    // Timeline Signals

    // Intro Simple timeline: When Content Warning starts to fade out
    public void SetContentWarningSeen()
    {
        PlayerPrefs.SetInt(Const_PlayerPrefs.ContentWarningSeen, 1);
        Dev_Logger.Debug($"Set ContentWarningSeen to 1; Now current ContentWarningSeen: {PlayerPrefs.GetInt(Const_PlayerPrefs.ContentWarningSeen)}");
    } 
    
    // Intro Simple timeline: End
    public void OnTimelineDone()
    {
        // If isDonePlaying: true, it means we're already fading in title from a different caller
        if (!isDonePlaying)
            startOverviewController.FadeInTitleScreen(true);
    }

    /// <summary>
    /// Called from Intro Simple timeline (on Content Warning fade-in)
    /// </summary>
    public void FinishTimelineSkipContentWarning()
    {
        // If isDonePlaying: true, it means we're already fading in title from a different caller
        if (isSkipContentWarning && !isDonePlaying)
        {
            Stop();
            startOverviewController.FadeInTitleScreen(true);
            contentWarningCanvasGroup.Close();
        }
    }

    // ----------------------------------------------------------------------

    /// <summary>
    /// Hide the content warning object so the timeline ends without showing content warning at all even if the
    /// signal is processed on the next few frames.
    /// </summary>
    private void HandleContentWarning()
    {
        Dev_Logger.Debug($"HandleContentWarning Current ContentWarningSeen: {PlayerPrefs.GetInt(Const_PlayerPrefs.ContentWarningSeen)}");

        if (
            PlayerPrefs.HasKey(Const_PlayerPrefs.ContentWarningSeen)
            && PlayerPrefs.GetInt(Const_PlayerPrefs.ContentWarningSeen) > 0
        )
        {
            isSkipContentWarning = true;
        }

        // Always show content warning in editor & dev builds.
        if (Debug.isDebugBuild)
            isSkipContentWarning = false;

        contentWarningContainer.gameObject.SetActive(!isSkipContentWarning);
    }

    public void Setup()
    {
        freedomCanvasGroup.Close();
        authorsCanvasGroup.Close();
        contentWarningCanvasGroup.Close();
    }
}
