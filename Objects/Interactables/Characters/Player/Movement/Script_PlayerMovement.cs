using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// For movement, uses PlayerGhost to simulate movement. We actually move Player as the pointer
/// immediately and have the visual representation PlayerGhost follow.
/// 
/// If we're not moving in world space (e.g. Timeline)
/// then we don't do this since we don't need the PlayerGhost lag visuals.
/// </summary>
[RequireComponent(typeof(PlayableDirector))]
[RequireComponent(typeof(Script_PlayerCheckCollisions))]
public class Script_PlayerMovement : MonoBehaviour
{
    public const string PlayerMovingAnimatorParam   = "PlayerMoving";
    public const string MoveXAnimatorParam          = "MoveX";
    public const string MoveZAnimatorParam          = "MoveZ";
    public const string LastMoveXAnimatorParam      = "LastMoveX";
    public const string LastMoveZAnimatorParam      = "LastMoveZ";
    private const string Horizontal                  = "Horizontal";
    private const string Vertical                    = "Vertical";
    
    [SerializeField] private Animator animator;
    
    public Script_PlayerGhost PlayerGhostPrefab;
    public Script_PlayerReflection PlayerReflectionPrefab;

    [SerializeField] private float defaultRepeatDelay;
    [SerializeField] private float runRepeatDelay;
    [SerializeField] private float devRunRepeatDelay;
    [SerializeField] private float defaultGhostSpeed;
    [SerializeField] private float runGhostSpeed;
    
    public int exitUpStairsOrderLayer;
    [SerializeField] private bool isMoving;
    
    [SerializeField] private TimelineAsset moveUpTimeline;
    [SerializeField] private TimelineAsset enterElevatorTimeline;

    private float repeatDelay;
    private Script_Game game;
    private Script_Player player;
    private Script_PlayerGhost playerGhost;
    private Script_PlayerReflection playerReflection;
    private Dictionary<Directions, Vector3> directionToVector;
    private SpriteRenderer spriteRenderer;
    private Transform grid;

    public Directions lastMove;
    public float timer;

    public Animator MyAnimator
    {
        get => animator;
    }

    public Script_PlayerGhost PlayerGhost
    {
        get => playerGhost;
    }

    public Script_InteractionBoxController InteractionBoxController
    {
        get => player.interactionBoxController;
    }

    public float Progress
    {
        get => 1 - timer / repeatDelay;
    }
    
    void OnDestroy()
    {
        if (playerGhost != null)    Destroy(playerGhost.gameObject);
    }
    
    public void HandleMoveInput(bool isReversed = false)
    {
        HandleWalkSpeed();
        
        timer = Mathf.Max(0f, timer - Time.deltaTime);

        HandleAnimations();
        HandleGhostTransform();

        Vector2 dirVector = new Vector2(Input.GetAxis(Horizontal), Input.GetAxis(Vertical));
        if (dirVector == Vector2.zero)  return;
        
        if (isReversed)     HandleMoveReversed();
        else                HandleMoveDefault();

        void HandleMoveDefault()
        {
            if (
                Mathf.Abs(dirVector.x) > Mathf.Abs(dirVector.y)
                && dirVector.x > 0
            )    
            {
                Move(Directions.Right);
            }
            else if (Mathf.Abs(dirVector.x) > Mathf.Abs(dirVector.y) && dirVector.x < 0)
            {
                Move(Directions.Left);
            }
            else if (Mathf.Abs(dirVector.y) > Mathf.Abs(dirVector.x) && dirVector.y > 0)
            {
                Move(Directions.Up);
            }
            else if (Mathf.Abs(dirVector.y) > Mathf.Abs(dirVector.x) && dirVector.y < 0)
            {
                Move(Directions.Down);
            }
        }

        void HandleMoveReversed()
        {
            if (
                Mathf.Abs(dirVector.x) > Mathf.Abs(dirVector.y)
                && dirVector.x > 0
            )    
            {
                Move(Directions.Left);
            }
            else if (Mathf.Abs(dirVector.x) > Mathf.Abs(dirVector.y) && dirVector.x < 0)
            {
                Move(Directions.Right);
            }
            else if (Mathf.Abs(dirVector.y) > Mathf.Abs(dirVector.x) && dirVector.y > 0)
            {
                Move(Directions.Down);
            }
            else if (Mathf.Abs(dirVector.y) > Mathf.Abs(dirVector.x) && dirVector.y < 0)
            {
                Move(Directions.Up);
            }
        }
    }

    // Handle the animation of ghost following this pointer.
    public void HandleGhostTransform()
    {
        playerGhost.Move(Progress);
    }

    private void HandleWalkSpeed()
    {
        /// TBD Check active sticker for speedwalk 
        bool isSpeedwalkStickerActive = Script_ActiveStickerManager.Control.IsActiveSticker("speedwalk-sticker");
        bool isDev = Debug.isDebugBuild && Const_Dev.IsDevMode;

        if (
            Input.GetButton(Const_KeyCodes.Action3) &
            (isSpeedwalkStickerActive || isDev)
        )
        {
            if (isDev)  repeatDelay = devRunRepeatDelay;
            else        repeatDelay = runRepeatDelay;
        }
        else
        {
            repeatDelay = defaultRepeatDelay;
        }
    }

    // Handle Moving State in Animator.
    private void HandleAnimations()
    {
        animator.SetBool(
            PlayerMovingAnimatorParam,
            Input.GetAxis(Vertical) != 0f || Input.GetAxis(Horizontal) != 0f
        );

        playerGhost.SetMoveAnimation();
    }

    public void Move(Directions dir)
    {
        // On Button Press reset timer to allow for instant direction changes.
        if (
            Input.GetButtonDown(Const_KeyCodes.Up)
            || Input.GetButtonDown(Const_KeyCodes.Right)
            || Input.GetButtonDown(Const_KeyCodes.Down)
            || Input.GetButtonDown(Const_KeyCodes.Left)
        )
        {
            timer = 0;
        }

        // Throttle repeat moves by timer.
        if (dir == lastMove && timer != 0f)
        {
            return;
        }

        Vector3 desiredMove = directionToVector[dir];
        
        AnimatorSetDirection(dir);
        PushPushables(dir);
        
        if (CheckCollisions(dir, ref desiredMove))   return;

        // DDR mode, only changing directions to look like dancing.
        if (game.state == Const_States_Game.DDR)    return;
        
        timer = repeatDelay;

        // Move player to desired location.
        playerGhost.startLocation = player.location;
        player.location += desiredMove;
        playerGhost.location += desiredMove;
        
        // Move player pointer.
        transform.position = player.location;
        
        isMoving = true;
        lastMove = dir;
    }

    public void AnimatorSetDirection(Directions dir)
    {
        InteractionBoxController.HandleActiveInteractionBox(dir);
        player.FacingDirection = dir;

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

        playerGhost.AnimatorSetDirection(dir);
    }

    bool CheckCollisions(Directions dir, ref Vector3 desiredMove)
    {   
        // If reflection is interactive check if it can move; if not, disallow player from moving as well.
        if (playerReflection is Script_PlayerReflectionInteractive)
        {
            if (!playerReflection.GetComponent<Script_PlayerReflectionInteractive>().CanMove())
                return true;
        }
        
        return GetComponent<Script_PlayerCheckCollisions>().CheckCollisions(
            player.location, dir, ref desiredMove
        );
    }

    void PushPushables(Directions dir)
    {
        player.TryPushPushable(dir); // push needs to come before collision detection
        
        if (playerReflection is Script_PlayerReflectionInteractive)
        {
            playerReflection.GetComponent<Script_PlayerReflectionInteractive>().TryPushPushable(dir);
        }
    }

    public void TrackPlayerGhost()
    {
        if (Progress >= 1f && isMoving)
        {
            // Sprite must be visible before playerGhost invisible to avoid flicker.
            // Only make visible if the player is no longer moving.
            Vector2 dirVector = new Vector2(Input.GetAxis(Horizontal), Input.GetAxis(Vertical));
            if (dirVector == Vector2.zero)
            {
                playerGhost.SetIsNotMoving();
                isMoving = false;
            }
        }
    }

    /// <summary>
    /// Used when player is controlled by Timeline to update all locations
    /// </summary>
    public void UpdateLocation(Vector3 updatedPlayerLoc)
    {
        playerGhost?.UpdateLocation(updatedPlayerLoc);
    }

    public void HandleExitTile()
    {
        Tilemap entrancesTileMap = game.EntranceTileMap;
        Tilemap[] exitsTileMaps = game.ExitTileMaps;
        
        Vector3Int tileLocation = new Vector3Int(
            (int)Mathf.Round(player.location.x - grid.position.x), // *adjust for grid offset*
            (int)Mathf.Round(player.location.z - grid.position.z),
            0
        );

        if (exitsTileMaps != null && exitsTileMaps.Length > 0)
        {
            foreach(Tilemap tm in exitsTileMaps)
            {
                if (tm.HasTile(tileLocation) && tm.gameObject.activeSelf)
                {
                    Script_TileMapExitEntrance exitInfo = tm.GetComponent<Script_TileMapExitEntrance>();
                    
                    if (exitInfo.Type == Script_Exits.ExitType.StairsUp)
                    {
                        Debug.Log("HandleStairsExitAnimation()");
                        HandleStairsExitAnimation();
                    }

                    if (exitInfo.Type == Script_Exits.ExitType.CutScene)
                    {
                        game.HandleExitCutSceneLevelBehavior();
                        return;
                    }                    
                    
                    game.Exit(
                        exitInfo.Level,
                        exitInfo.PlayerNextSpawnPosition,
                        exitInfo.PlayerFacingDirection,
                        true,
                        exitInfo.IsSilent,
                        exitInfo.Type
                    );
                    return;
                }
            }
        }

        if (entrancesTileMap != null && entrancesTileMap.HasTile(tileLocation) && entrancesTileMap.gameObject.activeSelf)
        {
            Script_TileMapExitEntrance entranceInfo = entrancesTileMap.GetComponent<Script_TileMapExitEntrance>();
            
            if (entranceInfo.Type == Script_Exits.ExitType.StairsUp)
            {
                HandleStairsExitAnimation();
            }

            game.Exit(
                entranceInfo.Level,
                entranceInfo.PlayerNextSpawnPosition,
                entranceInfo.PlayerFacingDirection,
                false,
                entranceInfo.IsSilent,
                entranceInfo.Type
            );
            return;
        }
    }

    private void HandleStairsExitAnimation()
    {
        spriteRenderer.GetComponent<Script_SortingOrder>().enabled = false;
        spriteRenderer.sortingOrder = exitUpStairsOrderLayer;
        player.PlayerGhostMatchSortingLayer();
    }

    Script_PlayerGhost CreatePlayerGhost(bool isLightOn)
    {
        Script_PlayerGhost pg = Instantiate(
            PlayerGhostPrefab,
            player.transform.position,
            Quaternion.identity
        );

        pg.SwitchLight(isLightOn);

        return pg;
    }

    public void PlayerGhostSortOrder(int sortingOrder)
    {
        playerGhost.spriteRenderer.sortingOrder = sortingOrder;
    }

    public Script_PlayerReflection CreatePlayerReflection(Vector3 reflectionAxis)
    {
        Script_PlayerReflection pr = Instantiate(
            PlayerReflectionPrefab,
            player.transform.position, // will update within Script_PlayerReflection
            Quaternion.identity
        );
        pr.Setup(
            playerGhost,
            player,
            reflectionAxis
        );
        pr.transform.SetParent(game.playerContainer, false);
        
        SetPlayerReflection(pr);
        return pr;
    }

    public void SetPlayerReflection(Script_PlayerReflection pr)
    {
        playerReflection = pr;
    }

    public void RemoveReflection()
    {
        if (playerReflection != null)
        {
            Destroy(playerReflection.gameObject);
        }
    }

    public void SwitchLight(bool isOn)
    {
        playerGhost.SwitchLight(isOn);
    }
    
    public void InitializeOnLevel(Transform _grid)
    {
        playerGhost.Setup(player);
        grid = _grid;
    }

    // ------------------------------------------------------------------
    // Timeline
    /// NOTE: should only be called from Player
    /// <summary>
    /// Move up one space via Timeline
    /// </summary>
    public void TimelineMoveUp()
    {
        PlayableDirector playerDirector = GetComponent<PlayableDirector>();
        playerDirector.Play(moveUpTimeline);
    }

    public void EnterElevator()
    {
        PlayableDirector playerDirector = GetComponent<PlayableDirector>();
        playerDirector.Play(enterElevatorTimeline);
    }
    /// <summary>
    /// Called from Timeline
    /// </summary>
    public void EnteredElevatorEvent()
    {
        Debug.Log("Calling this Enter Elevator EVENT!!!");
        Script_PlayerEventsManager.EnteredElevator();
    }
    // ------------------------------------------------------------------

    public void Setup(Script_Game _game, bool isLightOn)
    {
        game = _game;
        player = GetComponent<Script_Player>();
        
        spriteRenderer = (SpriteRenderer)player.graphics;
        spriteRenderer.enabled = false;

        // setup ghost for smooth movement (allows cancellation of mid-animation)
        playerGhost = CreatePlayerGhost(isLightOn);
        playerGhost.Setup(player);
        playerGhost.transform.SetParent(game.playerContainer, false);
        
        directionToVector = Script_Utils.GetDirectionToVectorDict();

        timer = repeatDelay;
    }
}
