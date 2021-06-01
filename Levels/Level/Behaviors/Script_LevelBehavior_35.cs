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
    
    [SerializeField] private Script_MeshFadeController meshFadeController;
    [SerializeField] private float meshFadeTime = .25f;
    private bool didMapNotification;

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