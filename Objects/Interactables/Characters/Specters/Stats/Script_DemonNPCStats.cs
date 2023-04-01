using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_DemonNPC))]
public class Script_DemonNPCStats : Script_DemonStats
{
    [SerializeField] private Script_DialogueNode reactionNode;
    
    public override int Hurt(int dmg, Script_HitBox hitBox, Script_HitBoxBehavior hitBoxBehavior)
    {
        // give player a callback to do after eating
        Script_Player player = hitBox.transform.GetParentRecursive<Script_Player>();
        if (player != null)
        {
            Dev_Logger.Debug($"Player trying to attack {this.gameObject.name}");
            // recursively find the Script_Attack
            player.onAttackDone = () => AttackedByPlayerReaction();
        }

        return 0;
    }

    private void AttackedByPlayerReaction()
    {
        if (reactionNode != null)
        {
            var dialogueManager = Script_DialogueManager.DialogueManager;
            var demonNPC = GetComponent<Script_DemonNPC>();
            
            dialogueManager.IsHandlingNPCOnHit = true;
            
            Script_DialogueManager.DialogueManager.StartDialogueNode(
                reactionNode,
                SFXOn: true,
                null,
                demonNPC
            );
        }
    }
}
