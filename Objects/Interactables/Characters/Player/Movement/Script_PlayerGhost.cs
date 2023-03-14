using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Shows as a lag animation to Player to give illusion of moving smoothly
/// Doing it this way so changing direction is incredibly crisp
/// 
/// NOTE: Ensure the Graphics offset here is same as Player
/// </summary>
[RequireComponent(typeof(PlayableDirector))]
public class Script_PlayerGhost : MonoBehaviour
{
    public AnimationCurve progressCurve;
    public SpriteRenderer spriteRenderer;
    
    [SerializeField] private Animator animator;
    
    public Vector3 startLocation;
    public Vector3 location;
    public float progress;
    public Directions facingDirection;

    [SerializeField] private Light l;
    [SerializeField] private Script_Player player;

    [SerializeField] private Script_PlayerGhostGraphics playerGhostGraphics;
    [SerializeField] private PlayableDirector director;
    [SerializeField] private Transform focalPoint;

    public Script_PlayerGhostGraphics PlayerGhostGraphics
    {
        get => playerGhostGraphics;
    }

    public Animator MyAnimator
    {
        get => animator;
        set => animator = value;
    }

    public PlayableDirector Director
    {
        get => director;
    }

    public Transform FocalPoint
    {
        get => focalPoint;
    }

    private Script_Player Player
    {
        get => player;
        set => player = value;
    }

    void Update()
    {
        CopyPlayerColor();
    }

    public void Move(float progress)
    {
        if (startLocation == null || location == null || !progress.HasValue())  return;

        Vector3 newPosition = Vector3.Lerp(
            startLocation,
            location,
            progressCurve.Evaluate(progress)
        );

        transform.position = newPosition;
    }

    public void SetIsNotMoving()
    {
        StopMoveAnimation();
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

    public void SetMoveAnimation(bool isMoving)
    {
        bool isMovingAnimation = isMoving
            || player.IsInputAxesMoving(
                player.RewiredInput.GetAxis(Const_KeyCodes.RWHorizontal),
                player.RewiredInput.GetAxis(Const_KeyCodes.RWVertical)
            );
        
        animator.SetBool(
            Script_PlayerMovement.PlayerMovingAnimatorParam,
            isMovingAnimation
        );
    }

    public void StopMoveAnimation()
    {
        animator.SetBool(
            Script_PlayerMovement.PlayerMovingAnimatorParam,
            false
        );
    }
    
    public void SwitchLight(bool isOn)
    {
        l.enabled = isOn;
    }

    public void UpdateLocation(Vector3 updatedPlayerLoc)
    {
        location = updatedPlayerLoc;
        transform.position = location;
        startLocation = location;
    }

    private void CopyPlayerColor()
    {
        SpriteRenderer playerSpriteRenderer = (SpriteRenderer)player.graphics;
        spriteRenderer.color = playerSpriteRenderer.color;
    }

    public void Setup(Script_Player _player)
    {
        player = _player;
        
        progress = 1f;
        transform.position = player.transform.position;
        UpdateLocation(player.transform.position);
        spriteRenderer.enabled = true;
    }
}
