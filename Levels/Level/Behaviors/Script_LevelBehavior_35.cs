﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_35 : Script_LevelBehavior
{
    public const string MapName = Script_Names.DiningRoom;
    
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool MynesMirrorIsActivated;
    /* ======================================================================= */
    
    [SerializeField] private Script_MeshFadeController meshFadeController;
    [SerializeField] private float meshFadeTime = .25f;

    [SerializeField] private Script_DemonNPC Ids;

    [SerializeField] private Transform MynesMirror;
    [SerializeField] private Transform MynesFinalPainting;

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

    // Called from Node: dining_mynes-mirror_interaction-node_end0
    public void UpdateMyneName()
    {
        Script_Names.UpdateMyne(); 
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
    
    // After Map Notification, Ids should lead the way until Player gets Animal Within mask
    // (by completing Ellenia's quest)
    private void HandlePlayIdsTimeline()
    {
        if (ShouldPlayIdsIntro())
        {
            game.ChangeStateCutScene();
            
            // Ids should lead you to his room (wrong way) on the First Tuesday to get Player acquainted with mansion.
            // Afterwards, he leads you on the critical path.
            if (game.IsFirstTuesday || game.ElleniasRoomBehavior.isPuzzleComplete)
                GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 1);
            else
                GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);
        }
    }

    private bool ShouldPlayIdsIntro()
    {
        return !didIdsRun && game.RunCycle == Script_RunsManager.Cycle.Weekday; 
    }

    public override void Setup()
    {
        if (ShouldPlayIdsIntro())
            Ids.gameObject.SetActive(true);
        else
            Ids.gameObject.SetActive(false);
        
        MynesMirror.gameObject.SetActive(true);
        MynesFinalPainting.gameObject.SetActive(false);
    }        
}