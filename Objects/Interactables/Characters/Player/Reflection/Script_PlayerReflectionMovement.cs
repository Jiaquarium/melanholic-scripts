using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerReflectionMovement : MonoBehaviour
{
    protected static readonly int PlayerMovingBool = Animator.StringToHash("PlayerMoving");
    private static readonly int NPCMovingBool = Animator.StringToHash("NPCMoving");
    
    [SerializeField] protected Animator animator;
    [SerializeField] private float xOffset;
    [SerializeField] private float zOffset;

    [SerializeField] private bool isUnsyncX;
    
    [Tooltip("Specify an adjuster if different facing Directions need to change position of Graphics")]
    [SerializeField] private Script_AnimatorPositionAdjuster positionAdjuster;

    protected Script_Player player;
    private Script_PlayerReflection playerReflection;
    private Vector3 axis;
    
    public void HandleMove()
    {
        ActuallyMove();
        MoveAnimation(player.FacingDirection);
    }
    
    protected virtual void ActuallyMove()
    {
        transform.position = GetReflectionPosition(player.transform.position);
    }

    void MoveAnimation(Directions dir)
    {
        Directions myFaceDirection = ToOppositeDirectionZ(dir);
        Script_Utils.AnimatorSetDirection(animator, myFaceDirection);
        
        bool isMoving = Script_Game.Game.GetPlayer().MyAnimator.GetBool(PlayerMovingBool);
        
        SetIsMoving(isMoving);

        if (positionAdjuster != null)
            positionAdjuster.Adjust(myFaceDirection);
    }

    protected virtual void SetIsMoving(bool isMoving)
    {
        animator.SetBool(NPCMovingBool, isMoving);
    }

    protected Directions ToOppositeDirectionZ(Directions desiredDir)
    {
        return playerReflection.ToOppositeDirectionZ(desiredDir);   
    }
    
    public Vector3 GetReflectionPosition(Vector3 loc)
    {
        // TODO: currently only works for reflection to be on top and to the right
        float reflectedZ = axis.z + Mathf.Abs(axis.z - loc.z) + zOffset;
        float reflectedX = isUnsyncX
            ? axis.x + Mathf.Abs(axis.x - loc.x) + xOffset
            : loc.x + xOffset;

        return new Vector3(reflectedX, loc.y, reflectedZ);
    }

    public void Setup(
        Script_PlayerReflection _playerReflection,
        Script_Player _player,
        Vector3 _axis
    )
    {
        player = _player;
        axis = _axis;
        playerReflection = _playerReflection;
    }
}
