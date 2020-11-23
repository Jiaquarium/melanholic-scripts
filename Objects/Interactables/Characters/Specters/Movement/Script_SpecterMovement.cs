using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_Demon))]
public class Script_SpecterMovement : MonoBehaviour
{
    public static string LastMoveX  = "LastMoveX";
    public static string LastMoveZ  = "LastMoveZ";
    public static string MoveX      = "MoveX";
    public static string MoveZ      = "MoveZ";
    
    
    [SerializeField] protected Directions defaultFacingDirection;
    [SerializeField] private bool maintainFacingDirection;
    
    
    private Directions lastFacingDirection;
    private Animator myAnimator;
    
    void Awake()
    {
        myAnimator = GetComponent<Script_Demon>().animator;
    }
    
    void OnEnable()
    {
        if (
            maintainFacingDirection
            && lastFacingDirection != Directions.None
            && myAnimator != null)
        {
            FaceLastDirection();
        }
        else
        {
            FaceDefaultDirection();
        }
    }

    public void FaceLastDirection()
    {
        FaceDirection(lastFacingDirection);
    }

    public void FaceDefaultDirection()
    {
        FaceDirection(defaultFacingDirection);
    }

    public void FacePlayer()
    {
        Directions dir = GetDirectionToPlayer();
        FaceDirection(dir);
    }

    public Directions GetDirectionToPlayer()
    {
        return transform.GetMyDirectionToTarget(Script_Game.Game.GetPlayer().transform);
    }

    public void FaceDirection(Directions direction)
    {
        if (direction == Directions.Down)        AnimatorSetDirection(0  , -1f);
        else if (direction == Directions.Up)     AnimatorSetDirection(0  ,  1f);
        else if (direction == Directions.Left)   AnimatorSetDirection(-1f,  0f);
        else if (direction == Directions.Right)  AnimatorSetDirection(1f ,  0f );

        if (direction != Directions.None)
            lastFacingDirection = direction;

        void AnimatorSetDirection(float x, float z)
        {
            myAnimator.SetFloat(LastMoveX, x);
            myAnimator.SetFloat(LastMoveZ, z);
            myAnimator.SetFloat(MoveX, x);
            myAnimator.SetFloat(MoveZ, z);
        }
    }
}
