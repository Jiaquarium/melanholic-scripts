using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_CutSceneActionHandler : MonoBehaviour
{
    [SerializeField] private Script_Game game;
    public void HandleContinuingDialogueActions(string action, Script_Player player)
    {
        if (action == Const_KeyCodes.Action1)
        {
            if (player.State == Const_States_Player.Dialogue)
            {
                if (Script_DialogueManager.DialogueManager.IsDialogueSkippable())
                    Script_DialogueManager.DialogueManager.SkipTypingSentence();
                else
                    game.dialogueManager.ContinueDialogue();
            }
            else if (player.State == Const_States_Player.PickingUp)   HandleEndItemDescriptionDialogue(player);
        }
    }

    /// <summary>
    /// e.g. for showItem sequence, also for use in dialogue
    /// </summary>
    /// <param name="itemObject"></param>
    public void HandleItemReceive(Script_ItemObject itemObject, Script_Player player)
    {
        Script_Item item = itemObject.Item;
        bool pickUpSuccess = game.AddItem(item);

        if (!pickUpSuccess)
        {
            // handle flow to drop an item in inventory
        }
        
        Script_ItemsEventsManager.ItemPickUp(item.id);

        player.SetIsPickingUp(item);
        player.ItemPickUpEffect(true, item);

        /// Item theatrics here
        if (itemObject.pickUpTheatricsPlayer != null)
        {
            itemObject.pickUpTheatricsPlayer.Play();
            return;
        }

        Script_Game.Game.dialogueManager.StartDialogueNode(
            itemObject.GetComponent<Script_DialogueNode>()
        );
        
        if (!itemObject.showTyping)
        {
            Debug.Log("Skipping typing item initial description on receiving item");
            Script_Game.Game.dialogueManager.SkipTypingSentence();
        }
    }

    void HandleEndItemDescriptionDialogue(Script_Player player)
    {
        player.HandleEndItemDescriptionDialogue();
    }
}
