using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LevelBehavior_5 : Script_LevelBehavior
{
    public const string MapName = "Mirror Halls";
    
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool didOnEntranceDialogue;
    /* ======================================================================= */

    public bool isInitialized;
    [SerializeField] private float beforeInternalThoughtWaitTime;
    [SerializeField] private Script_DialogueNode onEntranceDialogue;
    public Transform[] textParents;

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
    // Next Node Actions

    public void OnEntranceDialogueDone()
    {
        game.ChangeStateInteract();
    }
    
    // ------------------------------------------------------------------

    private void OnLevelInitCompleteEvent()
    {
        if (!didMapNotification)
        {
            Script_MapNotificationsManager.Control.PlayMapNotification(MapName, () => {
                HandleEntranceDialogue(onEntranceDialogue);       
            });
            didMapNotification = true;
        }
        else
        {
            HandleEntranceDialogue(onEntranceDialogue);
        }
    }

    private void HandleEntranceDialogue(Script_DialogueNode dialogueNode)
    {
        if (didOnEntranceDialogue)
            return;
        
        game.ChangeStateCutScene();

        StartCoroutine(WaitForPlayerThought());

        didOnEntranceDialogue = true;

        IEnumerator WaitForPlayerThought()
        {
            yield return new WaitForSeconds(beforeInternalThoughtWaitTime);

            Script_DialogueManager.DialogueManager.StartDialogueNode(dialogueNode);
        }
    }
    
    public override void Setup()
    {
        foreach(Transform t in textParents)
            game.SetupInteractableObjectsText(t, !isInitialized);
    }
}
