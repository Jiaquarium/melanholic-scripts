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

    public Script_Item itemShown { get; set; }
    
    public void HandleActionInput(Directions facingDirection, Vector3 location)
    {   
        /// <summary>
        /// interact state available actions
        /// </summary>
        switch (player.State)
        {
            case Const_States_Player.Interact:
                HandleInteraction();
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
        }

        void HandleInteraction()
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
            else if (Input.GetButtonDown(Const_KeyCodes.Effect1))
            {
                Debug.Log($"Player action for {Const_KeyCodes.Effect1}");
                stickerEffectsController.Switch(0);
            }
            else if (Input.GetButtonDown(Const_KeyCodes.Effect2))
            {
                Debug.Log($"Player action for {Const_KeyCodes.Effect2}");
                stickerEffectsController.Switch(1);
            }
            else if (Input.GetButtonDown(Const_KeyCodes.Effect3))
            {
                Debug.Log($"Player action for {Const_KeyCodes.Effect3}");
                stickerEffectsController.Switch(2);
            }
            else if (Input.GetButtonDown(Const_KeyCodes.Effect4))
            {
                Debug.Log($"Player action for {Const_KeyCodes.Effect4}");
                stickerEffectsController.Switch(3);
            }
            else if (Input.GetButtonDown(Const_KeyCodes.Effect5))
            {
                Debug.Log($"Player action for {Const_KeyCodes.Effect5}");
                stickerEffectsController.Switch(4);
            }
            else if (Input.GetButtonDown(Const_KeyCodes.Effect6))
            {
                Debug.Log($"Player action for {Const_KeyCodes.Effect6}");
                stickerEffectsController.Switch(5);
            }
            else if (Input.GetButtonDown(Const_KeyCodes.Effect7))
            {
                Debug.Log($"Player action for {Const_KeyCodes.Effect7}");
                stickerEffectsController.Switch(6);
            }
            else if (Input.GetButtonDown(Const_KeyCodes.Effect8))
            {
                Debug.Log($"Player action for {Const_KeyCodes.Effect8}");
                stickerEffectsController.Switch(7);
            }
            else if (Input.GetButtonDown(Const_KeyCodes.Effect9))
            {
                Debug.Log($"Player action for {Const_KeyCodes.Effect9}");
                stickerEffectsController.Switch(8);
            }
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
                
                // TODO: IMPLEMENT ITEM THEATRICS PICKUP

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
                player.ItemPickUpEffect(false, null);
                StashItem(item);
                itemShown = null;
                /// state may have already been modified away from PickingUp
                /// in that case, allow that state to override here
                if (player.State == Const_States_Player.PickingUp)
                {
                    player.SetIsInteract();
                    print("player state set to Interact from PlayerActions HandleEndItemDescriptionDialogue()");
                }
            }
            else
            {
                Debug.LogError("Need to implement continuing item pick up dialogue");
            }
            
            return true;
        }

        return false;
    }

    /// Pushes only the first pushable in the list
    public void TryPushPushable(Directions dir)
    {
        List<Script_Pushable> pushables = interactionBoxController.GetPushables(dir);
        if (pushables.Count > 0) pushables[0].Push(dir);
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

    bool DetectNPC(string action, Directions dir)
    {
        Script_StaticNPC NPC = interactionBoxController.GetNPC(dir);
        if (NPC == null)    return false;
        
        NPC.HandleAction(action);
        return true;
    }

    bool DetectInteractableObject(string action, Directions dir)
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
        if (game.AddItem(itemObject.GetItem()))
        {
            outItem = itemObject.GetItem();
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

    public void Setup(Script_Game _game)
    {
        player = GetComponent<Script_Player>(); 
        interactionBoxController = GetComponent<Script_InteractionBoxController>();
        game = _game;
        directions = Script_Utils.GetDirectionToVectorDict();
    }
}
