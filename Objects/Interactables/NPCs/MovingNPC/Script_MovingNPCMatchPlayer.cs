using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_MovingNPC))]
public class Script_MovingNPCMatchPlayer : MonoBehaviour
{
    private enum Axis
    {
        x,
        y,
        z
    }
    
    [SerializeField] private Axis axis;
    [SerializeField] private float distanceLimit;
    [SerializeField] private Script_Game game;
    
    private Script_MovingNPC npc;
    private Script_Player player;

    private Vector3 lastPosition;
    private Vector3 origin;

    void Awake()
    {
        npc = GetComponent<Script_MovingNPC>();
        player = game.GetPlayer();

        origin = npc.transform.position;
    }

    void FixedUpdate()
    {
        ActuallyMove();
        MoveAnimations(player.FacingDirection);
    }

    private void ActuallyMove()
    {
        lastPosition = npc.transform.position;
        Vector3 newPosition = lastPosition;
        
        switch (axis)
        {
            case (Axis.x):
                newPosition = new Vector3(
                    player.transform.position.x,
                    npc.transform.position.y, 
                    npc.transform.position.z
                ); 
                if (
                    distanceLimit > 0 &&
                    newPosition.x > origin.x + distanceLimit
                    || newPosition.x < origin.x - distanceLimit
                )
                {
                    return;
                }
                break;
            case (Axis.y):
                newPosition = new Vector3(
                    npc.transform.position.x,
                    player.transform.position.y, 
                    npc.transform.position.z
                ); 
                if (
                    distanceLimit > 0 &&
                    newPosition.y > origin.y + distanceLimit
                    || newPosition.y < origin.y - distanceLimit
                )
                {
                    return;
                }
                break;
            case (Axis.z):
                newPosition = new Vector3(
                    npc.transform.position.x,
                    npc.transform.position.y, 
                    player.transform.position.z
                ); 
                if (
                    distanceLimit > 0 &&
                    newPosition.z > origin.z + distanceLimit
                    || newPosition.z < origin.z - distanceLimit
                )
                {
                    return;
                }
                break;
        }

        npc.transform.position = newPosition;
    }

    private void MoveAnimations(Directions dir)
    {
        // Limit to only facing axis directions when moving.
        switch (axis)
        {
            case (Axis.x):
                if (dir == Directions.Up || dir == Directions.Down)
                    dir = npc.DefaultFacingDirection;
                break;
            case (Axis.y):
                dir = npc.DefaultFacingDirection;
                break;
            case (Axis.z):
                if (dir == Directions.Right || dir == Directions.Left)
                    dir = npc.DefaultFacingDirection;
                break;
        }

        bool isMoving = npc.transform.position != lastPosition;

        if (!isMoving)
            npc.FaceDefaultDirection();
        else
            Script_Utils.AnimatorSetDirection(npc.MyAnimator, dir);
        
        npc.MyAnimator.SetBool(Script_MovingNPC.NPCMoving, isMoving);
    }
}
