using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_InteractionBoxController))]
[RequireComponent(typeof(Script_PlayerAttacks))]
[RequireComponent(typeof(Script_Player))]
public class Script_PlayerAction : MonoBehaviour
{
    private Script_Game game;
    private Script_Player player;
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
        if (Input.GetButtonDown(Const_KeyCodes.Action1))
        {
            HandleDefaultAction(facingDirection, location);
        }
        else if (Input.GetButtonDown(Const_KeyCodes.Action2))
        {
            stickerEffectsController.Effect(facingDirection);
        }
        else if (Input.GetButtonDown(Const_KeyCodes.Inventory))
        {
            OpenInventory();
        }
        else
        {
            HandleStickerSwitch(facingDirection, location);
        }
    }

    private void HandleLastElevatorActions(Directions facingDirection, Vector3 location)
    {
        if (Input.GetButtonDown(Const_KeyCodes.Action2))
        {
            stickerEffectsController.Effect(facingDirection);
        }
        else
        {
            HandleStickerSwitch(facingDirection, location);
        }
    }

    private void HandleMelancholyPianoActions(Directions facingDirection, Vector3 location)
    {
        if (Input.GetButtonDown(Const_KeyCodes.Action2))
        {
            stickerEffectsController.Effect(facingDirection);
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
    
    private void HandleStickerSwitch(Directions facingDirection, Vector3 location)
    {
        if (Input.GetButtonDown(Const_KeyCodes.Effect1))
        {
            Debug.Log($"Switch to {Const_KeyCodes.Effect1}");
            stickerEffectsController.Switch(0);
        }
        else if (Input.GetButtonDown(Const_KeyCodes.Effect2))
        {
            Debug.Log($"Switch to {Const_KeyCodes.Effect2}");
            stickerEffectsController.Switch(1);
        }
        else if (Input.GetButtonDown(Const_KeyCodes.Effect3))
        {
            Debug.Log($"Switch to {Const_KeyCodes.Effect3}");
            stickerEffectsController.Switch(2);
        }
        else if (Input.GetButtonDown(Const_KeyCodes.Effect4))
        {
            Debug.Log($"Switch to {Const_KeyCodes.Effect4}");
            stickerEffectsController.Switch(3);
        }
    }

    private void HandleDefaultAction(Directions facingDirection, Vector3 location)
    {
        if (Input.GetButtonDown(Const_KeyCodes.Action1))
        {
            if (!HandleDialogue(facingDirection))
                HandlePickUpItem(facingDirection);
        }
    }

    bool HandleDialogue(Directions facingDirection)
    {
        if (!DetectNPC(Const_KeyCodes.Action1, facingDirection))
            if (!DetectInteractableObject(Const_KeyCodes.Action1, facingDirection))
                return DetectSavePoint(Const_KeyCodes.Action1, facingDirection);

        return true;
    }

    /// <summary>
    /// Handles the whole picking up flow including the subtext
    /// </summary>
    bool HandlePickUpItem(Directions facingDirection)
    {
        Script_Item item;
        Script_ItemObject itemObject = DetectAndOutItem(
            Const_KeyCodes.Action1, facingDirection, out item
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
                
                if (!itemObject.showTyping)
                {
                    Debug.Log("Skipping typing item initial description");
                    Script_Game.Game.dialogueManager.SkipTypingSentence();
                }
                
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
        if (Input.GetButtonDown(Const_KeyCodes.Action1))
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
                Debug.Log("Stashing Item after pickup dialogue.");

                player.ItemPickUpEffect(false, null);
                
                StashItem(item);    // will fire Stash event
                
                itemShown = null;
                
                /// state may have already been modified away from PickingUp
                /// in that case, allow that state to override here
                /// e.g. may want to react to the Stash Event and change to cut scene
                if (player.State == Const_States_Player.PickingUp)
                {
                    player.SetIsInteract();
                    Debug.Log("player state set to Interact from PlayerActions HandleEndItemDescriptionDialogue()");
                }
            }
            else
            {
                Debug.Log("Continuing item pick up dialogue.");
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
        Debug.Log($"UseUsableKey() called with key: {key}");
        Script_UsableTarget usableTarget = interactionBoxController.GetUsableTarget(dir);
        
        if (usableTarget is Script_UsableKeyTarget)
        {
            Script_UsableKeyTarget keyTarget = (Script_UsableKeyTarget)usableTarget;
            return keyTarget.Unlock(key);
        }
        
        Debug.Log($"No usable target available for usable key {key}");
        
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

    void OpenInventory()
    {
        game.OpenInventory();
        player.SetIsInventory();
    }

    void StashItem(Script_Item item)
    {
        Script_ItemsEventsManager.ItemStash(item.id);
        
        itemStashAudioSource.PlayOneShot(
            Script_SFXManager.SFX.PlayerStashItem,
            Script_SFXManager.SFX.PlayerStashItemVol
        );
    }

    private void HandleExitPuppeteerNull(Directions facingDirection)
    {
        if (Input.GetButtonDown(Const_KeyCodes.Action2))
        {
            stickerEffectsController.Effect(facingDirection);
        }
    }

    public void PuppetMasterEffect(Directions dir)
    {
        stickerEffectsController.Effect(dir);
    }

    public void Setup(Script_Game _game)
    {
        game = _game;
        
        player = GetComponent<Script_Player>(); 
        interactionBoxController = GetComponent<Script_InteractionBoxController>();
        directions = Script_Utils.GetDirectionToVectorDict();
    }
}
