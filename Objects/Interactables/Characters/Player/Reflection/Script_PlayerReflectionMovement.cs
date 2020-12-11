using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerReflectionMovement : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float xOffset;
    [SerializeField] private float zOffset;
    protected Script_Player player;
    private Script_PlayerReflection playerReflection;
    private Script_PlayerGhost playerGhost;
    private Vector3 axis;
    private Vector3 lastPosition;
    
    public void HandleMove()
    {
        ActuallyMove();
        MoveAnimation(player.facingDirection);
        // if (Script_Game.Game.state != Const_States_Game.Interact)   SetNotMoving();
    }
    protected virtual void ActuallyMove()
    {
        if (playerGhost == null)    return;
        
        lastPosition = transform.position;
        
        // allows to reflect player when first spawned on level and not moving
        if (!playerGhost.spriteRenderer.enabled)
            transform.position = GetReflectionPosition(player.transform.position);
        else
            transform.position = GetReflectionPosition(playerGhost.transform.position);
    }

    void MoveAnimation(Directions dir)
    {
        Directions myFaceDirection = ToOppositeDirectionZ(dir);
        Script_Utils.AnimatorSetDirection(animator, myFaceDirection);
        
        bool isMoving = Vector3.Distance(transform.position, lastPosition) != 0;
        
        // if game is in interact mode, even if reflection is not moving position
        // it should still show movement animation like the player (e.g. walking into wall)
        if (
            (
                Script_Game.Game.state == Const_States_Game.Interact
                && player.State == Const_States_Player.Interact
            )
            && (
                Input.GetButton(Const_KeyCodes.Up)
                || Input.GetButton(Const_KeyCodes.Right)
                || Input.GetButton(Const_KeyCodes.Down)
                || Input.GetButton(Const_KeyCodes.Left)
            )
        )
        {
            isMoving = true;
        }
        animator.SetBool("NPCMoving", isMoving);
    }

    void SetNotMoving()
    {
        animator.SetBool("NPCMoving", false);
    }

    protected Directions ToOppositeDirectionZ(Directions desiredDir)
    {
        return playerReflection.ToOppositeDirectionZ(desiredDir);   
    }
    
    public Vector3 GetReflectionPosition(Vector3 loc)
    {
        // TODO: currently only works for reflection to be on top and to the right
        float reflectedZ = axis.z + Mathf.Abs(axis.z - loc.z) + zOffset;
        float reflectedX = axis.x + Mathf.Abs(axis.x - loc.x) + xOffset;

        return new Vector3(reflectedX, loc.y, reflectedZ);
    }

    public void Setup(
        Script_PlayerReflection _playerReflection,
        Script_PlayerGhost _playerGhost,
        Script_Player _player,
        Vector3 _axis
    )
    {
        playerGhost = _playerGhost;
        player = _player;
        axis = _axis;
        playerReflection = _playerReflection;
    }
}
