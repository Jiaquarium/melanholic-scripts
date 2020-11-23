using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach this to MovingNPC; use to control NPC from Unity Events (e.g. NextDialogueNodeAction)
/// </summary>
[RequireComponent(typeof(Script_MovingNPC))]
public class Script_MovingNPCFaceDirectionController : MonoBehaviour
{
    public void FaceLeft()
    {
        GetComponent<Script_MovingNPC>().FaceDirection(Directions.Left);
    }

    public void FaceRight()
    {
        GetComponent<Script_MovingNPC>().FaceDirection(Directions.Right);
    }

    public void FaceUp()
    {
        GetComponent<Script_MovingNPC>().FaceDirection(Directions.Up);
    }
    
    public void FaceDown()
    {
        GetComponent<Script_MovingNPC>().FaceDirection(Directions.Down);
    }

    public void FacePlayer()
    {
        Script_Player player = Script_Game.Game.GetPlayer();
        Directions dir = Script_Utils.GetDirectionToTarget(
            transform.position, player.transform.position
        );
        GetComponent<Script_MovingNPC>().FaceDirection(dir);
    } 
}
