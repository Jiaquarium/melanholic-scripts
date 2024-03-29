﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_InteractionBoxController))]
[RequireComponent(typeof(Script_PlayerAttacks))]
[RequireComponent(typeof(Script_Player))]
public class Script_PlayerAction : MonoBehaviour
{
    protected Script_Game game;
    protected Script_Player player;
    private Dictionary<Directions, Vector3> directions;
    private Script_InteractionBoxController interactionBoxController;
    [SerializeField] private AudioSource itemStashAudioSource;
    [SerializeField] private AudioSource pickUpErrorAudioSource;
    [SerializeField] private Script_StickerEffectsController stickerEffectsController;
    [SerializeField] private Script_Item _itemShown;
    
    public Script_Item itemShown
    {
        get => _itemShown;
        set => _itemShown = value;
    }

    public bool IsLanternLightOn
    {
        get => stickerEffectsController?.IsLanternLightOn ?? false;
    }

    public bool IsPuppeteerEffectHoldOn
    {
        get => stickerEffectsController?.IsPuppeteerEffectHoldOn ?? false;
    }

    public virtual void HandleActionInput(Directions facingDirection, Vector3 location)
    {   
        /// <summary>
        /// interact state available actions
        /// </summary>
        switch (player.State)
        {
            case Const_States_Player.Interact:
                HandleInteraction(facingDirection, location);
                break;
            case Const_States_Player.Dialogue:
                HandleDefaultAction(facingDirection, location);
                break;
            case Const_States_Player.Viewing:
                HandleDefaultAction(facingDirection, location);
                break;
            case Const_States_Player.PickingUp:
                HandlePickingUp();
                break;
            case Const_States_Player.Puppeteer:
                // Control is given to Puppet Master.
                break;
            case Const_States_Player.PuppeteerNull:
                HandleExitPuppeteerNull(facingDirection);
                break;
            case Const_States_Player.LastElevatorEffect:
                HandleLastElevatorActions(facingDirection, location);
                break;
            case Const_States_Player.MelancholyPiano:
                HandleMelancholyPianoActions(facingDirection, location);
                break;
        }
    }

    protected virtual void HandleInteraction(Directions facingDirection, Vector3 location)
    {
        var rewiredInput = player.RewiredInput;
        
        if (rewiredInput.GetButtonDown(Const_KeyCodes.RWInteract))
        {
            HandleDefaultAction(facingDirection, location);
        }
        else if (rewiredInput.GetButtonDown(Const_KeyCodes.RWMaskCommand))
        {
            stickerEffectsController.Effect(facingDirection);
        }
        else if (rewiredInput.GetButtonDown(Const_KeyCodes.RWInventory))
        {
            OpenInventory();
        }
        else if (rewiredInput.GetButtonDown(Const_KeyCodes.RWUICancel))
        {
            Dev_Logger.Debug("Open Settings");
            OpenSettings();
        }
        else if (rewiredInput.GetButtonDown(Const_KeyCodes.RWUnknownControllerSettings))
        {
            Dev_Logger.Debug("Open Settings via Unknown Controller Settings");
            OpenSettings();
        }
        else
        {
            HandleStickerSwitch(facingDirection, location);
        }
    }

    private void HandleLastElevatorActions(Directions facingDirection, Vector3 location)
    {
        if (player.RewiredInput.GetButtonDown(Const_KeyCodes.RWMaskCommand))
        {
            stickerEffectsController.Effect(facingDirection);
        }
        else if (player.RewiredInput.GetButtonDown(Const_KeyCodes.RWInteract))
        {
            HandleNoInteractions();
        }
        else
        {
            HandleStickerSwitch(facingDirection, location);
        }
    }

    private void HandleMelancholyPianoActions(Directions facingDirection, Vector3 location)
    {
        if (player.RewiredInput.GetButtonDown(Const_KeyCodes.RWMaskCommand))
        {
            stickerEffectsController.Effect(facingDirection);
        }
        else if (player.RewiredInput.GetButtonDown(Const_KeyCodes.RWInteract))
        {
            HandleNoInteractions();
        }
        else
        {
            HandleStickerSwitch(facingDirection, location);
        }
    }

    public void HandleDefaultStickerState()
    {
        stickerEffectsController.DefaultStateNoEffect();
    }

    public void HandleForceStickerSwitchBackground(int i)
    {
        stickerEffectsController.Switch(i, true);
    }
    
    private void HandleStickerSwitch(Directions facingDirection, Vector3 location)
    {
        if (
            player.RewiredInput.GetButtonDown(Const_KeyCodes.RWMask1)
            || player.RewiredInput.GetButtonDown(Const_KeyCodes.RWUnknownControllerMaskVert)
        )
        {
            Dev_Logger.Debug($"Switch to {Const_KeyCodes.RWMask1}");
            stickerEffectsController.Switch(0);
        }
        else if (
            player.RewiredInput.GetButtonDown(Const_KeyCodes.RWMask2)
            || player.RewiredInput.GetButtonDown(Const_KeyCodes.RWUnknownControllerMaskHz)
        )
        {
            Dev_Logger.Debug($"Switch to {Const_KeyCodes.RWMask2}");
            stickerEffectsController.Switch(1);
        }
        else if (
            player.RewiredInput.GetButtonDown(Const_KeyCodes.RWMask3)
            || player.RewiredInput.GetNegativeButtonDown(Const_KeyCodes.RWUnknownControllerMaskVert)
        )
        {
            Dev_Logger.Debug($"Switch to {Const_KeyCodes.RWMask3}");
            stickerEffectsController.Switch(2);
        }
        else if (
            player.RewiredInput.GetButtonDown(Const_KeyCodes.RWMask4)
            || player.RewiredInput.GetNegativeButtonDown(Const_KeyCodes.RWUnknownControllerMaskHz)
        )
        {
            Dev_Logger.Debug($"Switch to {Const_KeyCodes.RWMask4}");
            stickerEffectsController.Switch(3);
        }
    }

    private void HandleDefaultAction(Directions facingDirection, Vector3 location)
    {
        if (player.RewiredInput.GetButtonDown(Const_KeyCodes.RWInteract))
        {
            if (!HandleDialogue(facingDirection))
                HandlePickUpItem(facingDirection);
        }
    }

    private bool HandleDialogue(Directions facingDirection)
    {
        if (!DetectNPC(Const_KeyCodes.InteractAction, facingDirection))
            if (!DetectInteractableObject(Const_KeyCodes.InteractAction, facingDirection))
                return DetectSavePoint(Const_KeyCodes.InteractAction, facingDirection);

        return true;
    }

    private void HandleNoInteractions()
    {
        Script_SFXManager.SFX.PlayDullError();
    }

    /// <summary>
    /// Handles the whole picking up flow including the subtext
    /// </summary>
    private bool HandlePickUpItem(Directions facingDirection)
    {
        Script_Item item;
        Script_ItemObject itemObject = DetectAndOutItem(
            Const_KeyCodes.InteractAction, facingDirection, out item
        );

        /// itemObject will be null if item failed to pick up (e.g. inventory was full)
        if (itemObject != null && item != null)
        {
            if (itemObject.initialDescription)
            {
                player.SetIsPickingUp(item);
                itemShown = item;
                
                Script_Game.Game.dialogueManager.StartDialogueNode(
                    itemObject.GetComponent<Script_DialogueNode>()
                );
                
                // Handle itemObject.showTyping here. Currently not implemented.

                player.ItemPickUpEffect(true, item);
            }
            else
            {
                // no description, then immediately stash item
                StashItem(item);
            }

            return true;
        }
        
        return false;
    }

    public bool HandlePickingUp()
    {
        if (player.RewiredInput.GetButtonDown(Const_KeyCodes.RWInteract))
        {
            HandleEndItemDescriptionDialogue(itemShown); // current item being held above Player's head
            return true;
        }

        return false;
    }

    public bool HandleEndItemDescriptionDialogue(Script_Item item)
    {
        bool? isContinuingDialogue = Script_Game.Game.dialogueManager.ContinueDialogue();

        // as long as it is a valid dialogue continuation, ContinueDialogue()
        // will return null on invalid requests
        if (isContinuingDialogue != null)
        {
            if (isContinuingDialogue == false)
            {
                Dev_Logger.Debug("Stashing Item after pickup dialogue.");

                player.ItemPickUpEffect(false, null);
                
                StashItem(item);    // will fire Stash event
                
                itemShown = null;
                
                /// state may have already been modified away from PickingUp
                /// in that case, allow that state to override here
                /// e.g. may want to react to the Stash Event and change to cut scene
                if (player.State == Const_States_Player.PickingUp)
                {
                    player.SetIsInteract();
                    Dev_Logger.Debug("player state set to Interact from PlayerActions HandleEndItemDescriptionDialogue()");
                }
            }
            else
            {
                Dev_Logger.Debug("Continuing item pick up dialogue.");
            }
            
            return true;
        }

        return false;
    }

    /// Pushes only the first pushable in the list
    public void TryPushPushable(Directions dir)
    {
        List<Script_Pushable> pushables = interactionBoxController.GetPushables(dir);
        
        if (pushables.Count > 0)
            pushables[0].Push(dir);
    }

    public bool UseUsableKey(Script_UsableKey key, Directions dir)
    {
        // check for key
        Dev_Logger.Debug($"UseUsableKey() called with key: {key}");
        Script_UsableTarget usableTarget = interactionBoxController.GetUsableTarget(dir);
        
        if (usableTarget is Script_UsableKeyTarget)
        {
            Script_UsableKeyTarget keyTarget = (Script_UsableKeyTarget)usableTarget;
            return keyTarget.Unlock(key);
        }
        
        Dev_Logger.Debug($"No usable target available for usable key {key}");
        
        return false;
    }

    /// <summary>
    /// Called from Player Movement.
    /// Action handled on movement into the exit object.
    /// </summary>
    /// <param name="dir"></param>
    /// <returns>True, only if there is an exit detected and it matches the current facingDirection</returns>
    public virtual bool DetectDoorExit(Directions dir)
    {
        Script_DoorExit exit = interactionBoxController.GetDoorExit(dir);
        if (exit == null)    return false;

        return exit.TryExit(dir);
    }

    private bool DetectNPC(string action, Directions dir)
    {
        Script_StaticNPC NPC = interactionBoxController.GetNPC(dir);
        if (NPC == null)    return false;
        
        NPC.HandleAction(action);
        return true;
    }

    private bool DetectInteractableObject(string action, Directions dir)
    {
        Script_InteractableObject[] objs = interactionBoxController.GetInteractableObject(dir);
        if (objs.Length == 0)    return false;

        foreach (Script_InteractableObject obj in objs) obj.HandleAction(action);
        return true;
    }

    private bool DetectSavePoint(string action, Directions dir)
    {
        Script_SavePoint savePoint = interactionBoxController.GetSavePoint(dir);
        if (savePoint == null)      return false;
        
        savePoint.HandleAction(action);
        return true;
    }

    private Script_ItemObject DetectAndOutItem(string action, Directions dir, out Script_Item outItem)
    {
        Script_ItemObject itemObject = interactionBoxController.GetItem(dir);
        if (itemObject == null)
        {
            outItem = null;
            return null;
        }
        
        /// Able to add item to inventory
        if (game.AddItem(itemObject.Item))
        {
            outItem = itemObject.Item;
            itemObject.HandleAction(action);
            
            // send Event of what item was picked up successfully
            Script_ItemsEventsManager.ItemPickUp(outItem.id);
            
            return itemObject;
        }

        /// Inventory is full
        pickUpErrorAudioSource.PlayOneShot(
            Script_SFXManager.SFX.ItemPickUpError,
            Script_SFXManager.SFX.ItemPickUpErrorVol
        );
        outItem = null;        
        return null;
    }

    private void OpenInventory()
    {
        game.OpenInventory();
        player.SetIsInventory();
    }

    private void StashItem(Script_Item item)
    {
        Script_ItemsEventsManager.ItemStash(item.id);
        
        itemStashAudioSource.PlayOneShot(
            Script_SFXManager.SFX.PlayerStashItem,
            Script_SFXManager.SFX.PlayerStashItemVol
        );
    }

    private void HandleExitPuppeteerNull(Directions facingDirection)
    {
        if (player.RewiredInput.GetButtonDown(Const_KeyCodes.RWMaskCommand))
        {
            stickerEffectsController.Effect(facingDirection);
        }
    }

    public void PuppetMasterEffect(Directions dir)
    {
        stickerEffectsController.Effect(dir);
    }

    public void MyMaskEquipEffectTimeline()
    {
        stickerEffectsController.MyMaskEquipEffectTimeline();
    }

    public void SetIceSpikeDepthMasksEnabled(bool isEnabled)
    {
        stickerEffectsController.SetIceSpikeDepthMasksEnabled(isEnabled);
    }

    public void SwitchMaskSFX()
    {
        stickerEffectsController.SwitchSFX();
    }

    private void OpenSettings()
    {
        game.OpenSettings();
    }

    public void Setup(Script_Game _game)
    {
        game = _game;
        
        player = GetComponent<Script_Player>(); 
        interactionBoxController = GetComponent<Script_InteractionBoxController>();
        directions = Script_Utils.GetDirectionToVectorDict();
    }
}
