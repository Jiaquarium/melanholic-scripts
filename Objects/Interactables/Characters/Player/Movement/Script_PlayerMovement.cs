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
    // ------------------------------------------------------------------
    // PlayerMovement Animator Controller Params
    public const string PlayerMovingAnimatorParam   = "PlayerMoving";
    public const string MoveXAnimatorParam          = "MoveX";
    public const string MoveZAnimatorParam          = "MoveZ";
    public const string LastMoveXAnimatorParam      = "LastMoveX";
    public const string LastMoveZAnimatorParam      = "LastMoveZ";
    
    // ------------------------------------------------------------------
    // Input Codes
    private const string Horizontal                  = "Horizontal";
    private const string Vertical                    = "Vertical";
    
    [SerializeField] private Animator animator;
    
    public Script_PlayerGhost PlayerGhostPrefab;
    public Script_PlayerReflection PlayerReflectionPrefab;

    [SerializeField] private float defaultRepeatDelay;
    [SerializeField] private float changeDirectionDelay;
    [SerializeField] private float runRepeatDelay;
    [SerializeField] private float devRunRepeatDelay;
    
    public int exitUpStairsOrderLayer;
    
    // Tracks if Ghost is done animating and no axis is pressed.
    [SerializeField] private bool isMoving;
    
    [SerializeField] private TimelineAsset moveUpTimeline;
    [SerializeField] private TimelineAsset enterElevatorTimeline;

    [SerializeField] private RuntimeAnimatorController defaultAnimatorController;

    private float repeatDelay;
    private Script_Game game;
    
    private Script_Player player;
    private PlayableDirector director;

    private Script_PlayerGhost playerGhost;
    private Script_PlayerReflection playerReflection;
    private Dictionary<Directions, Vector3> directionToVector;
    private SpriteRenderer spriteRenderer;
    private Transform grid;

    [SerializeField] private Directions lastMove;
    [SerializeField] private float timer;
    [SerializeField] private float changeDirectionTimer;

    public float xWeight;
    public float yWeight;

    private Script_PlayerCheckCollisions playerCheckCollisions;

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

    public RuntimeAnimatorController DefaultAnimatorController
    {
        get => defaultAnimatorController;
    }

    public float Progress
    {
        get => 1 - timer / repeatDelay;
    }

    public Directions LastMove
    {
        get => lastMove;
    }
    
    void OnDestroy()
    {
        if (playerGhost != null)    Destroy(playerGhost.gameObject);
    }
    
    void Awake()
    {
        playerCheckCollisions = GetComponent<Script_PlayerCheckCollisions>();
    }
    
    public void HandleMoveInput(bool isReversed = false)
    {
        HandleWalkSpeed();
        
        HandleMoveBufferTimers();

        HandleAnimations();
        HandleGhostTransform();

        Vector2 dirVector = new Vector2(
            Input.GetAxis(Const_KeyCodes.Horizontal), Input.GetAxis(Const_KeyCodes.Vertical)
        );

        // ------------------------------------------------------------------
        // New Button Presses
        
        // Give priority to new button presses.
        // Only relevant for Keyboard.
        if (Input.GetButtonDown(Const_KeyCodes.Up))
        {
            Debug.Log($"{name}____{Const_KeyCodes.Up} New Button Press");
            UpWeights();
        }
        else if (Input.GetButtonDown(Const_KeyCodes.Down))
        {
            Debug.Log($"{name}____{Const_KeyCodes.Down} New Button Press");
            DownWeights();
        }
        else if (Input.GetButtonDown(Const_KeyCodes.Right))
        {
            Debug.Log($"{name}____{Const_KeyCodes.Right} New Button Press");
            RightWeights();
        }
        else if (Input.GetButtonDown(Const_KeyCodes.Left))
        {
            Debug.Log($"{name}____{Const_KeyCodes.Left} New Button Press");
            LeftWeights();
        }
        
        // ------------------------------------------------------------------
        // Button Holds
        else if (
            Input.GetButton(Const_KeyCodes.Up)
            || Input.GetButton(Const_KeyCodes.Down)
            || Input.GetButton(Const_KeyCodes.Right)
            || Input.GetButton(Const_KeyCodes.Left)
        )
        {
            // If coming from a moving state, use the last weights if lastMove is still being held down.
            // (Note: isMoving is reset at every level initialization)
            if (isMoving && Input.GetButton(lastMove.DirectionToKeyCode()))
            {
                Debug.Log($"{name}____isMoving: {isMoving}, xWeight: {xWeight}, yWeight: {yWeight}");

                dirVector = new Vector2(xWeight, yWeight);
            }
            
            // If coming from a nonmoving state, must define new weights based on input axis,
            // giving priority Up, Down, Left, then Right.
            else
            {
                Debug.Log($"{name}____coming from nonmoving state, dirVector.x: {dirVector.x} dirVector.y: {dirVector.y}");
                
                if (dirVector.y > 0)
                    UpWeights();
                else if (dirVector.y < 0)
                    DownWeights();
                else if (dirVector.x > 0)
                    RightWeights();
                else if (dirVector.x < 0)
                    LeftWeights();
            }
        }

        if (isReversed)     HandleMoveReversed();
        else                HandleMoveDefault();

        void HandleMoveDefault()
        {
            if (dirVector.y > 0)
                Move(Directions.Up);
            else if (dirVector.y < 0)
                Move(Directions.Down);
            else if (dirVector.x > 0)    
                Move(Directions.Right);
            else if (dirVector.x < 0)
                Move(Directions.Left);
        }

        void HandleMoveReversed()
        {
            if (dirVector.y > 0)
                Move(Directions.Down);
            else if (dirVector.y < 0)
                Move(Directions.Up);
            else if (dirVector.x > 0)    
                Move(Directions.Left);
            else if (dirVector.x < 0)
                Move(Directions.Right);
        }

        void UpWeights()
        {
            xWeight = 0f;
            yWeight = 1f;
            dirVector = new Vector2(xWeight, yWeight);
        }

        void DownWeights()
        {
            xWeight = 0f;
            yWeight = -1f;
            dirVector = new Vector2(xWeight, yWeight);
        }

        void RightWeights()
        {
            xWeight = 1f;
            yWeight = 0f;
            dirVector = new Vector2(xWeight, yWeight);
        }

        void LeftWeights()
        {
            xWeight = -1f;
            yWeight = 0f;
            dirVector = new Vector2(xWeight, yWeight);
        }
    }

    public void Move(Directions dir)
    {
        // Refresh direction change buffer when coming from nonmoving state.
        if (!isMoving)
            changeDirectionTimer = 0f;

        // When changing directions. 
        if (dir != lastMove)
        {
            // Allow for first instant direction change and buffer preceding ones.
            if (changeDirectionTimer <= 0f)
            {
                timer = 0;

                // Start buffering changing directions when coming from moving state.
                if (isMoving)
                    changeDirectionTimer = changeDirectionDelay;
            }
            // Subsequent direction changes are buffered by changeDirectionDelay,
            // so Player can't spam direction changes to travel faster.
            else
            {
                return;
            }            
        }

        // Throttle repeat moves by timer.
        if (timer > 0f)
        {
            return;
        }

        Vector3 desiredMove = directionToVector[dir];
        
        AnimatorSetDirection(dir);
        PushPushables(dir);
        
        if (CheckCollisions(dir, ref desiredMove))
        {
            Debug.Log($"{name}____Player Collision");

            return;
        }

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

    public void HandleMoveBufferTimers()
    {
        timer = Mathf.Max(0f, timer - Time.smoothDeltaTime);
        changeDirectionTimer = Mathf.Max(0f, changeDirectionTimer - Time.smoothDeltaTime);
    }

    // Handle the animation of ghost following this pointer.
    public void HandleGhostTransform(bool isForceTimerUpdate = false)
    {
        // Manually reduce timer for non-moving game states where the timer is paused.
        if (isForceTimerUpdate)
            HandleMoveBufferTimers();

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
        
        return playerCheckCollisions.CheckCollisions(player.location, dir, ref desiredMove);
    }

    void PushPushables(Directions dir)
    {
        player.TryPushPushable(dir); // push needs to come before collision detection
        
        if (playerReflection is Script_PlayerReflectionInteractive)
        {
            playerReflection.GetComponent<Script_PlayerReflectionInteractive>().TryPushPushable(dir);
        }
    }

    public void HandleIsMoving()
    {
        if (Progress >= 1f && isMoving)
        {
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

    // Called at each level initialization.
    public void InitializeOnLevel(Transform _grid)
    {
        playerGhost.Setup(player);
        grid = _grid;

        timer = 0f;
        changeDirectionTimer = 0f;
        
        xWeight = 0f;
        yWeight = 0f;

        lastMove = Directions.None;
        isMoving = false;
    }

    // ------------------------------------------------------------------
    // Timeline
    /// NOTE: should only be called from Player
    /// <summary>
    /// Move up one space via Timeline
    /// </summary>
    public void TimelineMoveUp()
    {
        director.Play(moveUpTimeline);
    }

    public void EnterElevator()
    {
        director.Play(enterElevatorTimeline);

        // Need to bind Player Ghost to timeline.
        List<GameObject> playerObjsToBind = new List<GameObject>();

        // Ensure ordering of objects to bind is in sync with timeline tracks.
        playerObjsToBind.Add(player.gameObject);
        playerObjsToBind.Add(player.MyAnimator.gameObject);
        playerObjsToBind.Add(playerGhost.gameObject);
        playerObjsToBind.Add(playerGhost.MyAnimator.gameObject);

        director.BindTimelineTracks(enterElevatorTimeline, playerObjsToBind);
        director.Play(enterElevatorTimeline);
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
        director = GetComponent<PlayableDirector>();
        
        // Setup ghost for smooth movement (allows cancellation of mid-animation)
        spriteRenderer = (SpriteRenderer)player.graphics;
        spriteRenderer.enabled = false;
        playerGhost = CreatePlayerGhost(isLightOn);
        playerGhost.Setup(player);
        playerGhost.transform.SetParent(game.playerContainer, false);
        
        directionToVector = Script_Utils.GetDirectionToVectorDict();
    }
}
