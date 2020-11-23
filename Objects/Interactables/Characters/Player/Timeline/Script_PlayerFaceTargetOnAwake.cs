using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activate this as Activation Track from timeline to make NPC face player
/// </summary>
public class Script_PlayerFaceTargetOnAwake : MonoBehaviour
{
    public Transform target;
    
    private void Awake()
    {
        Script_Player player = Script_Game.Game.GetPlayer();
        Directions dir = Script_Utils.GetDirectionToTarget(
            player.transform.position, target.position
        );
        player.FaceDirection(dir);
    }
}
