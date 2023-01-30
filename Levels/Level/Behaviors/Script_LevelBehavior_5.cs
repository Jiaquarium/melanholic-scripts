using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LevelBehavior_5 : Script_LevelBehavior
{
    private const string MapName = Script_Names.MirrorHalls;
    
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool didOnEntranceDialogue;
    /* ======================================================================= */

    public bool isInitialized;
    
    // Because this will ALWAYS be accompanied by map notification, wait a bit longer.
    [SerializeField] private float beforeInternalThoughtWaitTime;
    [SerializeField] private Script_DialogueNode onEntranceDialogue;
    public Transform[] textParents;

    private bool didMapNotification;
    private bool didMapNotificationDoneEvent;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        
        Script_GameEventsManager.OnLevelInitComplete    += OnLevelInitCompleteEvent;
        Script_GameEventsManager.OnLevelBlackScreenDone += OnLevelBlackScreenDone;

        Script_TransitionsEventsManager.OnMapNotificationDefaultDone += HandleEntranceDialogue;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        Script_GameEventsManager.OnLevelInitComplete    -= OnLevelInitCompleteEvent;
        Script_GameEventsManager.OnLevelBlackScreenDone -= OnLevelBlackScreenDone;

        Script_TransitionsEventsManager.OnMapNotificationDefaultDone -= HandleEntranceDialogue;
    }

    // ------------------------------------------------------------------
    // Next Node Actions

    public void OnEntranceDialogueDone()
    {
        game.ChangeStateInteract();
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
        // Also handles case where map notification finishes up before level init event (not possible currently)
        if (!didOnEntranceDialogue && !didMapNotificationDoneEvent)
            game.ChangeStateCutScene();
    }

    /// TBD: Need to standardize this wait time after Map Notification
    private void HandleEntranceDialogue()
    {
        didMapNotificationDoneEvent = true;
        
        if (didOnEntranceDialogue)
            return;
        
        game.ChangeStateCutScene();

        StartCoroutine(WaitForPlayerThought());

        didOnEntranceDialogue = true;

        IEnumerator WaitForPlayerThought()
        {
            yield return new WaitForSeconds(beforeInternalThoughtWaitTime);

            Script_DialogueManager.DialogueManager.StartDialogueNode(onEntranceDialogue);
        }
    }
    
    public override void Setup()
    {
        foreach(Transform t in textParents)
            game.SetupInteractableObjectsText(t, !isInitialized);
    }
}
