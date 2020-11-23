using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activate this as Activation Track from timeline to turn NPC direction
/// </summary>
public class Script_MovingNPCFaceDirectionOnAwake : MonoBehaviour
{
    [SerializeField] private Directions faceDirection;
    [SerializeField] private Script_MovingNPC[] movingNPCs;

    private void Awake()
    {
        foreach (Script_MovingNPC npc in movingNPCs)
        {
            print($"NPC {npc}facing direction: {faceDirection}");
            npc.FaceDirection(faceDirection);
        }
        this.gameObject.SetActive(false);
    }
}
