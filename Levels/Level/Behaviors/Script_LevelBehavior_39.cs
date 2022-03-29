﻿using System.Collections;
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
    [SerializeField] private Script_Trigger onEnterTrigger;

    [SerializeField] private Script_Trigger stayTrigger;
    [SerializeField] private Directions checkDirection;

    [SerializeField] private Script_Marker noBarrierOnTalkLocation;

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

    // ----------------------------------------------------------------------
    // Unity Events START
    
    // called from Trigger in front of Guard
    public void StartGuardDialogue()
    {
        if (Flan.State == Script_StaticNPC.States.Dialogue)
            return;

        bool isPsychicDuckActive = Script_ActiveStickerManager.Control.IsActiveSticker(Const_Items.PsychicDuckId);
        
        Vector3 playerPos       = game.GetPlayer().transform.position;
        float xDistanceFromFlan = Flan.transform.position.x - playerPos.x;
        bool isExiting          = xDistanceFromFlan < 0;

        // If already spoken with guard by either interacting with or activating trigger
        // then don't activate trigger anymore and let Vx pass automatically 
        // except on Wednesday, where it's always blocking
        if (isPsychicDuckActive && didGuardConfirm && game.Run.dayId != Script_Run.DayId.wed)
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
        Flan.ForceHandleAction(Const_KeyCodes.Action1);

        // NOTE Flan's last dialogue node needs to call EndGuardDialogue()
    }

    public void OnEndGuardDialogueBlock()
    {
        /// Reinstate barrier unless Player is talking from the right side of guard.
        if (game.GetPlayer().location != noBarrierOnTalkLocation.Position)
            barrier.gameObject.SetActive(true);
        
        Debug.Log("OnEndGuardDialogueBlock: Set game to Interact.");
        game.ChangeStateInteract();

        // must set back to nonconfirm state to force Guard to speak up next time
        // you walk past trigger with Psychic Duck (applicable if get confirmed 
        // once but come back without the sticker attached)
        didGuardConfirm = false;
    }

    // Happens when Flan unblocks hallway.
    public void UpdateFlan()
    {
        Script_Names.UpdateFlan();
    }
    
    public void OnEndGuardDialogueUnblock()
    {
        /// Remove barrier
        barrier.gameObject.SetActive(false);
        
        game.ChangeStateInteract();

        didGuardConfirm = true;
    }

    // Called from OnStayTrigger
    // If player is trying to continue to move right, then react with
    // Flan's dialogue.
    public void CheckPlayerMoveDirection()
    {
        var player = game.GetPlayer();
        
        if (
            Input.GetButton(checkDirection.DirectionToKeyCode())
            && player.FacingDirection == checkDirection
        )
        {
            if (player.State == Const_States_Player.Interact)
                StartGuardDialogue();
            else
                Debug.Log("Trying to trigger Flan Dialogue but Player not in Interact");
        }
    }

    // ----------------------------------------------------------------------

    public override void Setup()
    {
        if (game.IsUrselkSistersQuestsDone)
        {
            Flan.gameObject.SetActive(false);
            barrier.gameObject.SetActive(false);
            onEnterTrigger.gameObject.SetActive(false);
            stayTrigger.gameObject.SetActive(false);
        }
    }        
}

#if UNITY_EDITOR
#endif