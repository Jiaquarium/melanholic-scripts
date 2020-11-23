using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activate this as Activation Track from timeline to make NPC face player
/// </summary>
public class Script_MovingNPCFacePlayerOnAwake : MonoBehaviour
{
    [SerializeField] private Script_MovingNPC[] movingNPCs;

    private void Awake()
    {
        foreach (Script_MovingNPC npc in movingNPCs)
        {
            Directions faceDirection = Script_Utils.GetDirectionToTarget(
                npc.transform.position,
                Script_Game.Game.GetPlayer().transform.position
            );
            
            Debug.Log($"npc {npc.name} face {faceDirection}");
            npc.FaceDirection(faceDirection);
        }
        this.gameObject.SetActive(false);
    }
}
