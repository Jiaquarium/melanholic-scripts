using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_LevelBehavior_39 : Script_LevelBehavior
{
    public const string MapName = "Urselks Hall";
    
    /* =======================================================================
        STATE DATA
    ======================================================================= */

    /* ======================================================================= */
    
    [SerializeField] private Script_DemonNPC Flan;
    [SerializeField] private Script_Interactable barrier;
    [SerializeField] private Script_Trigger trigger;

    private bool didGuardConfirm;
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

    // Need for Guard OnTrigger Cut Scene
    protected override void HandleAction()
    {
        base.HandleDialogueAction();
    }

    // ----------------------------------------------------------------------
    // Unity Events START
    
    // called from Trigger in front of Guard
    public void StartGuardDialogue()
    {
        bool isPsychicDuckActive = Script_ActiveStickerManager.Control.IsActiveSticker(Const_Items.PsychicDuckId);
        
        Vector3 playerPos       = game.GetPlayer().transform.position;
        float xDistanceFromFlan = Flan.transform.position.x - playerPos.x;
        bool isExiting          = xDistanceFromFlan < 0;

        // If already spoken with guard by either interacting with or activating trigger
        // then don't activate trigger anymore and let Vx pass automatically 
        // except on Sunday, where it's always blocking
        if (isPsychicDuckActive && didGuardConfirm && game.Run.dayId != Script_Run.DayId.sun)
            return;

        // If player x position > Flan's, then is entering Trigger from right side (exiting rooms)
        if (isExiting)
        {
            Debug.Log($"Player x distance from Flan: {xDistanceFromFlan}, isExiting: {isExiting}");
            OnEndGuardDialogueUnblock();
            return;
        }
        
        Debug.Log("Triggering Flan convo");
        game.ChangeStateCutScene();
        
        Flan.FaceDirection(Flan.transform.position.GetDirectionToTarget(playerPos));
        Flan.TriggerDialogue();

        // NOTE Flan's last dialogue node needs to call EndGuardDialogue()
    }

    public void OnEndGuardDialogueBlock()
    {
        /// Reinstate barrier
        barrier.gameObject.SetActive(true);
        
        game.ChangeStateInteract();

        // must set back to nonconfirm state to force Guard to speak up next time
        // you walk past trigger with Psychic Duck (applicable if get confirmed 
        // once but come back without the sticker attached)
        didGuardConfirm = false;
    }

    public void OnEndGuardDialogueUnblock()
    {
        /// Remove barrier
        barrier.gameObject.SetActive(false);
        
        game.ChangeStateInteract();

        didGuardConfirm = true;
    }
    // ----------------------------------------------------------------------

    public override void Setup()
    {
        if (game.IsUrselkSistersQuestsDone)
        {
            Flan.gameObject.SetActive(false);
            barrier.gameObject.SetActive(false);
            trigger.gameObject.SetActive(false);
        }
    }        
}

#if UNITY_EDITOR
#endif