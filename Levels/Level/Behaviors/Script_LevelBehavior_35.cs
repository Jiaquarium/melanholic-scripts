using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LevelBehavior_35 : Script_LevelBehavior
{
    public const string MapName = "The Parlor";
    
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool MynesMirrorIsActivated;
    /* ======================================================================= */
    
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
    
    /// Need for Myne's Mirror
    protected override void HandleAction()
    {
        base.HandleDialogueAction();
    }

    public override void Setup()
    {
        
    }        
}