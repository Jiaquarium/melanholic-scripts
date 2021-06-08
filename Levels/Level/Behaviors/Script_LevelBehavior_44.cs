using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_LevelBehavior_44 : Script_LevelBehavior
{
    public const string MapName = "XXX World";
    
    // ==================================================================
    // State Data
    
    // ==================================================================

    [SerializeField] private Script_InteractablePaintingEntrance[] paintingEntrances;
    [SerializeField] private Script_InteractablePaintingEntrance ballroomPaintingEntrance;

    private bool didMapNotification;

    protected override void OnEnable()
    {
        Script_GameEventsManager.OnLevelInitComplete    += OnLevelInitCompleteEvent;
    }

    protected override void OnDisable()
    {
        Script_GameEventsManager.OnLevelInitComplete    -= OnLevelInitCompleteEvent;
    }

    private void OnLevelInitCompleteEvent()
    {
        if (!didMapNotification)
        {
            Script_MapNotificationsManager.Control.PlayMapNotification(MapName);
            didMapNotification = true;
        }
    }
    
    // ------------------------------------------------------------------
    // Timeline Signals
    
    public void FinishQuestPaintings()
    {
        foreach (var painting in paintingEntrances)
        {
            painting.DonePainting();
        }

        ballroomPaintingEntrance.DonePainting();
    }
    // ------------------------------------------------------------------
    
    public override void Setup()
    {
        base.Setup();
    }
}