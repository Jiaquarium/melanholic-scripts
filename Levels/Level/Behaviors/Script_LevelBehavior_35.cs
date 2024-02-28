using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_35 : Script_LevelBehavior
{
    public static string MapName = Script_Names.DiningRoom;
    
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

    [SerializeField] private Script_InteractableObjectText goBoardR1;
    [SerializeField] private Script_InteractableObjectText goBoardR2;

    private bool didMapNotification;
    private bool didIdsRun;

    protected override void OnEnable()
    {
        base.OnEnable();
        
        Script_GameEventsManager.OnLevelInitComplete    += OnLevelInitCompleteEvent;
        Script_GameEventsManager.OnLevelBlackScreenDone += OnLevelBlackScreenDone;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        Script_GameEventsManager.OnLevelInitComplete    -= OnLevelInitCompleteEvent;
        Script_GameEventsManager.OnLevelBlackScreenDone -= OnLevelBlackScreenDone;
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

    private void OnLevelBlackScreenDone()
    {
        if (!didMapNotification)
        {
            Script_MapNotificationsManager.Control.PlayMapNotification(MapName);
            didMapNotification = true;
        }
    }
    
    private void OnLevelInitCompleteEvent()
    {
        HandlePlayIdsTimeline();
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
        
        bool isWeekend = game.RunCycle == Script_RunsManager.Cycle.Weekend;
        goBoardR1.gameObject.SetActive(!isWeekend);
        goBoardR2.gameObject.SetActive(isWeekend);

        MynesMirror.gameObject.SetActive(true);
        MynesFinalPainting.gameObject.SetActive(false);
    }        
}