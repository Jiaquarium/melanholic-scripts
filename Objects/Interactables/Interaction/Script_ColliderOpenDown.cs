using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NOTE: Script Execution Order: Set before Default time, before Pixel Perfect Cinemachine
/// </summary>
[RequireComponent(typeof(Script_PhysicsBox))]
public class Script_ColliderOpenDown : MonoBehaviour
{
    [SerializeField] private Collider myCollider;
    [SerializeField] private Script_PhysicsBox blockingArea;

    private Script_Player Player
    {
        get
        {
            if (player == null)
                player = Script_Game.Game.GetPlayer();
            
            return player;
        }
        set => player = value;
    }
    private Script_Player player;
    
    void Start()
    {
        Player = Script_Game.Game.GetPlayer();
    }
    
    void Update()
    {
        HandleCollisionOnDirection();
        HandlePlayerColliding();
    }

    void HandleCollisionOnDirection()
    {
        // If Player is above this, disable the collision
        if (transform.position.GetDirectionToTarget(Player.transform.position) == Directions.Up)
            myCollider.enabled = false;
        // If Player is below this, always enable collision
        else if (transform.position.GetDirectionToTarget(Player.transform.position) == Directions.Down)
            myCollider.enabled = true;
    }

    // As a safeguard, if Player is overlapping this collider, always remove the collider to allow Player to move
    void HandlePlayerColliding()
    {
        if (CheckPlayerOverlap())
            myCollider.enabled = false;

        bool CheckPlayerOverlap()
        {
            // Check if Player is within the Physics Box
            List<Script_Player> players = new List<Script_Player>();
            blockingArea.ExposeBox();

            foreach (Collider col in blockingArea.Colliders)
            {
                if (col != null && col.transform.GetParentRecursive<Script_Player>() != null)
                    players.Add(col.transform.GetParentRecursive<Script_Player>());
            }

            return players.Count > 0;
        }
    }
}