using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_35 : Script_LevelBehavior
{
    public const string MapName = "The Parlor";
    
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool MynesMirrorIsActivated;
    /* ======================================================================= */
    
    [SerializeField] private Script_MeshFadeController meshFadeController;
    [SerializeField] private float meshFadeTime = .25f;

    [SerializeField] private Script_DemonNPC Ids;

    private bool didMapNotification;
    private bool didIdsRun;

    protected override void OnEnable()
    {
        base.OnEnable();
        
        Script_GameEventsManager.OnLevelInitComplete    += OnLevelInitCompleteEvent;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        Script_GameEventsManager.OnLevelInitComplete    -= OnLevelInitCompleteEvent;
    }

    // ------------------------------------------------------------------
    // Unity Events (Triggers)
    public void FadeInBookshelvesSouth()
    {
        meshFadeController.FadeIn(meshFadeTime);
    }

    public void FadeOutBookshelvesSouth()
    {
        meshFadeController.FadeOut(meshFadeTime);
    }
    // ------------------------------------------------------------------
    // Timeline Signal Reactions
    public void OnIdsRunAwayTimelineDone()
    {
        game.ChangeStateInteract();
        didIdsRun = true;
    }

    // ------------------------------------------------------------------

    private void OnLevelInitCompleteEvent()
    {
        if (!didMapNotification)
        {
            Script_MapNotificationsManager.Control.PlayMapNotification(MapName, () => {
                HandlePlayIdsTimeline();
            });
            didMapNotification = true;
        }
        else
        {
            HandlePlayIdsTimeline();
        }
    }
    
    // After Map Notification, Ids should lead the way on Tutorial Run.
    private void HandlePlayIdsTimeline()
    {
        if (ShouldPlayIdsIntro())
        {
            game.ChangeStateCutScene();
            
            if (Script_EventCycleManager.Control.IsLastElevatorTutorialRun())
                GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);
            else if (Script_EventCycleManager.Control.IsIdsRoomIntroDay())
                GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 1);
        }
    }
    
    private bool ShouldPlayIdsIntro()
    {
        return !didIdsRun && (
            Script_EventCycleManager.Control.IsLastElevatorTutorialRun()
            || Script_EventCycleManager.Control.IsIdsRoomIntroDay()
        );
    }

    public override void Setup()
    {
        if (ShouldPlayIdsIntro())
            Ids.gameObject.SetActive(true);
        else
            Ids.gameObject.SetActive(false);        
    }        
}