using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shows as a lag animation to Player to give illusion of moving smoothly
/// Doing it this way so changing direction is incredibly crisp
/// 
/// NOTE: Ensure the Graphics offset here is same as Player
/// </summary>
public class Script_PlayerGhost : MonoBehaviour
{
    public AnimationCurve progressCurve;
    public SpriteRenderer spriteRenderer;
    
    [SerializeField] private Animator animator;
    
    [SerializeField] private float _speed;

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

    public Vector3 startLocation;
    public Vector3 location;
    public float progress;
    public Directions facingDirection;

    [SerializeField] private Light l;

    private bool isMoving;

    void Update()
    {
        if (isMoving)   ActuallyMove();
    }

    public void Move(Directions dir)
    {
        SetIsMoving();
        progress = 0f;
    }

    public void SetIsNotMoving()
    {
        isMoving = false;
        spriteRenderer.enabled = false;

        StopMoveAnimation();
    }

    void SetIsMoving()
    {
        isMoving = true;
        spriteRenderer.enabled = true;
    }

    void ActuallyMove()
    {
        progress += _speed * Time.deltaTime;
        transform.position = Vector3.Lerp(
            startLocation,
            location,
            progressCurve.Evaluate(progress)
        );

        if (progress >= 1f)
        {
            progress = 1f;
        }
    }

    public void AnimatorSetDirection(Directions dir)
    {
        facingDirection = dir;
        
        if (dir == Directions.Up)
        {
            animator.SetFloat(Script_PlayerMovement.LastMoveXAnimatorParam, 0f);
            animator.SetFloat(Script_PlayerMovement.LastMoveZAnimatorParam, 1f);
            animator.SetFloat(Script_PlayerMovement.MoveXAnimatorParam,     0f);
            animator.SetFloat(Script_PlayerMovement.MoveZAnimatorParam,     1f);
        }
        else if (dir == Directions.Down)
        {
            animator.SetFloat(Script_PlayerMovement.LastMoveXAnimatorParam, 0f);
            animator.SetFloat(Script_PlayerMovement.LastMoveZAnimatorParam, -1f);
            animator.SetFloat(Script_PlayerMovement.MoveXAnimatorParam,     0f);
            animator.SetFloat(Script_PlayerMovement.MoveZAnimatorParam,     -1f);
        }
        else if (dir == Directions.Left)
        {
            animator.SetFloat(Script_PlayerMovement.LastMoveXAnimatorParam, -1f);
            animator.SetFloat(Script_PlayerMovement.LastMoveZAnimatorParam, 0f);
            animator.SetFloat(Script_PlayerMovement.MoveXAnimatorParam,     -1f);
            animator.SetFloat(Script_PlayerMovement.MoveZAnimatorParam,     0f);
        }
        else if (dir == Directions.Right)
        {
            animator.SetFloat(Script_PlayerMovement.LastMoveXAnimatorParam, 1f);
            animator.SetFloat(Script_PlayerMovement.LastMoveZAnimatorParam, 0f);
            animator.SetFloat(Script_PlayerMovement.MoveXAnimatorParam,     1f);
            animator.SetFloat(Script_PlayerMovement.MoveZAnimatorParam,     0f);
        }
    }

    public void SetMoveAnimation()
    {
        animator.SetBool(
            Script_PlayerMovement.PlayerMovingAnimatorParam,
            Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f
        );
    }

    public void StopMoveAnimation()
    {
        animator.SetBool(Script_PlayerMovement.PlayerMovingAnimatorParam, false);
    }
    
    public void SwitchLight(bool isOn)
    {
        l.enabled = isOn;
    }

    public void UpdateLocation(Vector3 updatedPlayerLoc)
    {
        location = updatedPlayerLoc;
        startLocation = location;
    }

    public void Setup(Vector3 loc)
    {
        progress = 1f;
        transform.position = loc;
        UpdateLocation(loc);
        spriteRenderer.enabled = false;
    }
}
