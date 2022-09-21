using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_DemonNPC))]
public class Script_DemonNPCStats : Script_DemonStats
{
    [SerializeField] private Script_DialogueNode reactionNode;
    
    public override int Hurt(int dmg, Script_HitBox hitBox)
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
            Script_DialogueManager.DialogueManager.StartDialogueNode(
                reactionNode,
                SFXOn: true,
                null,
                GetComponent<Script_DemonNPC>()
            );
        }
    }
}
