﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Using Player to move while keeping a Queue of buffered moves.
/// </summary>
[RequireComponent(typeof(PlayableDirector))]
[RequireComponent(typeof(Script_PlayerCheckCollisions))]
public class Script_PlayerMovement : MonoBehaviour
{
    public enum Speeds
    {
        Default     = 0,
        Run         = 1,
        Dev         = 999
    }
    
    // ------------------------------------------------------------------
    // PlayerMovement Animator Controller Params
    public const string PlayerMovingAnimatorParam   = "PlayerMoving";
    public const string MoveXAnimatorParam          = "MoveX";
    public const string MoveZAnimatorParam          = "MoveZ";
    public const string LastMoveXAnimatorParam      = "LastMoveX";
    public const string LastMoveZAnimatorParam      = "LastMoveZ";
    
    [SerializeField] private Animator animator;
    
    public Script_PlayerReflection PlayerReflectionPrefab;

    [SerializeField] private float defaultSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float devRunSpeed;
    
    public int exitUpStairsOrderLayer;
    
    // Tracks if Ghost is done animating and no axis is pressed.
    [SerializeField] private bool isMoving;
    [SerializeField] private Vector3 startLocation;
    
    [SerializeField] private TimelineAsset moveUpTimeline;
    [SerializeField] private TimelineAsset enterElevatorTimeline;

    [SerializeField] private RuntimeAnimatorController defaultAnimatorController;

    private float progress;
    private Speeds walkSpeed;
    
    private Script_Game game;
    
    private Script_Player player;
    private PlayableDirector director;

    private Script_PlayerReflection playerReflection;
    private Dictionary<Directions, Vector3> directionToVector;
    private SpriteRenderer spriteRenderer;
    private Transform grid;

    [SerializeField] private Directions lastMoveInput;

    [SerializeField] private Script_LimitedQueue<Directions> inputBuffer = new Script_LimitedQueue<Directions>(4);
    
    // TBD TODO: Remove, Only for Dev
    [Tooltip("Only to show what is happening in the underlying Queue")]
    [SerializeField] private List<Directions> inputBufferDisplay = new List<Directions>(4);

    public float xWeight;
    public float yWeight;

    [SerializeField] private Script_PlayerAction playerActionHandler;
    private bool didTryExit;

    private Script_PlayerCheckCollisions playerCheckCollisions;

    private bool isForceMoveAnimation;

    public Animator MyAnimator
    {
        get => animator;
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
        get => progress;
    }

    public Directions FacingDirection
    {
        get => player.FacingDirection;
    }
    
    public float WalkSpeed
    {
        get
        {
            switch (walkSpeed)
            {
                case (Speeds.Default):
                    return defaultSpeed;
                case (Speeds.Run):
                    return runSpeed;
                case (Speeds.Dev):
                    return devRunSpeed;
                default:
                    return defaultSpeed;    
            }
        }
    }

    public bool IsMoving
    {
        get => isMoving;
    }
    
    void Awake()
    {
        playerCheckCollisions = GetComponent<Script_PlayerCheckCollisions>();
    }

    void LateUpdate()
    {
        if (Const_Dev.IsDevMode)
            inputBufferDisplay = inputBuffer.ToList();
    }
    
    public void HandleMoveInput(bool isReversed = false)
    {
        HandleWalkSpeed();
        HandleAnimations();

        // ------------------------------------------------------------------
        // Input Buffer
        
        // Fill buffer with any inputs caught during buffer time.
        if (progress < 1f)
        {
            BufferInput();
            return;
        }

        Directions bufferedInput = Directions.None;
        if (inputBuffer.Count > 0)
            bufferedInput = inputBuffer.Peek();
        
        // ------------------------------------------------------------------

        Vector2 dirVector = new Vector2(
            Input.GetAxis(Const_KeyCodes.Horizontal), Input.GetAxis(Const_KeyCodes.Vertical)
        );

        // ------------------------------------------------------------------
        // Buffered Moves
        // Buffer new button presses if we're in the middle of a move.
        // Buffered moves always take priority.
        
        if (bufferedInput == Directions.Up)
        {
            UpWeights();
        }
        else if (bufferedInput == Directions.Down)
        {
            DownWeights();
        }
        else if (bufferedInput == Directions.Right)
        {
            RightWeights();
        }
        else if (bufferedInput == Directions.Left)
        {
            LeftWeights();
        }
        
        // ------------------------------------------------------------------
        // New Button Presses
        
        // Only relevant for Keyboard.
        else if (Input.GetButtonDown(Const_KeyCodes.Up))
        {
            UpWeights();
        }
        else if (Input.GetButtonDown(Const_KeyCodes.Down))
        {
            DownWeights();
        }
        else if (Input.GetButtonDown(Const_KeyCodes.Right))
        {
            RightWeights();
        }
        else if (Input.GetButtonDown(Const_KeyCodes.Left))
        {
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
            // If coming from a moving state, use the last weights if lastMoveInput is still being held down.
            // (Note: isMoving is reset at every level initialization)
            if (isMoving && Input.GetButton(lastMoveInput.DirectionToKeyCode()))
            {
                dirVector = new Vector2(xWeight, yWeight);
            }
            
            // If coming from a nonmoving state, must define new weights based on input axis,
            // giving priority Up, Down, Left, then Right.
            else
            {
                if (dirVector.y > 0)
                {
                    UpWeights();
                }
                else if (dirVector.y < 0)
                {
                    DownWeights();
                }
                else if (dirVector.x > 0)
                {
                    RightWeights();
                }
                else if (dirVector.x < 0)
                {
                    LeftWeights();
                }
            }
        }

        if (isReversed)     HandleMoveReversed();
        else                HandleMoveDefault();

        ClearInputBuffer();

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

    // Buffer new button presses during buffer timer.
    private void BufferInput()
    {
        if (Input.GetButtonDown(Const_KeyCodes.Up))
        {
            inputBuffer.Enqueue(Const_KeyCodes.Up.KeyCodeToDirections());
        }
        else if (Input.GetButtonDown(Const_KeyCodes.Down))
        {
            inputBuffer.Enqueue(Const_KeyCodes.Down.KeyCodeToDirections());
        }
        else if (Input.GetButtonDown(Const_KeyCodes.Right))
        {
            inputBuffer.Enqueue(Const_KeyCodes.Right.KeyCodeToDirections());
        }
        else if (Input.GetButtonDown(Const_KeyCodes.Left))
        {
            inputBuffer.Enqueue(Const_KeyCodes.Left.KeyCodeToDirections());
        }
    }

    public void ClearInputBuffer()
    {
        inputBuffer.Clear();
    }

    public void Move(Directions dir)
    {
        lastMoveInput = dir;
        
        // Handle Exits (once per tile)
        if (!didTryExit && TryExit(dir))
            return;
        else
            didTryExit = true;
        
        if (progress < 1f)
            return;
        
        AnimatorSetDirection(dir);
        PushPushables(dir);
        
        Vector3 desiredMove = directionToVector[dir];
        if (CheckCollisions(dir, ref desiredMove))
            return;

        // DDR mode, only changing directions to look like dancing.
        if (game.state == Const_States_Game.DDR)
            return;
        
        progress = 0f;

        // Actually move player to desired location.
        startLocation = player.location;
        player.location += desiredMove;

        isMoving = true;
        didTryExit = false;
    }

    private bool TryExit(Directions dir)
    {
        // Handle Exits, if in the desired Direction there is an exit,
        // do not move, instead Player turns and exits.
        if (
            HandleExitTile(dir)
            || HandleExitObject(dir)
        )
        {
            return true;
        }

        return false;
    }

    public void HandleMoveTransform()
    {
        if (progress < 1f)
        {
            progress += WalkSpeed * Time.smoothDeltaTime;
            
            if (progress > 1f)
                progress = 1f;
            
            Vector3 newPosition = Vector3.Lerp(
                startLocation,
                player.location,
                progress
            );    
            transform.position = newPosition;

            isMoving = true;
        }
    }

    private void HandleWalkSpeed()
    {
        /// TBD Check active sticker for speedwalk 
        bool isSpeedwalkStickerActive = Script_ActiveStickerManager.Control.IsActiveSticker("speedwalk-sticker");
        bool isDev = Debug.isDebugBuild || Const_Dev.IsDevMode;

        if (
            Input.GetButton(Const_KeyCodes.Action3)
            && (isSpeedwalkStickerActive || isDev)
        )
        {
            if (isDev)  walkSpeed = Speeds.Dev;
            else        walkSpeed = Speeds.Run;
        }
        else
        {
            walkSpeed = Speeds.Default;
        }
    }

    public void StopMovingAnimations()
    {
        if (isForceMoveAnimation)
            return;
        
        animator.SetBool(PlayerMovingAnimatorParam, false);
    }
    
    // Handle Moving State in Animator.
    private void HandleAnimations()
    {
        if (isForceMoveAnimation)
            return;
        
        bool isMovingAnimation = isMoving
            || Input.GetAxis(Const_KeyCodes.Vertical) != 0f
            || Input.GetAxis(Const_KeyCodes.Horizontal) != 0f;
        
        animator.SetBool(PlayerMovingAnimatorParam, isMovingAnimation);
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
    }

    bool CheckCollisions(Directions dir, ref Vector3 desiredMove)
    {   
        // Do not allow Player to move if Reflection cannot move.
        if (playerReflection is Script_PlayerReflectionInteractive && playerReflection.gameObject.activeInHierarchy)
        {
            if (!playerReflection.GetComponent<Script_PlayerReflectionInteractive>().CanMove())
                return true;
        }
        
        return playerCheckCollisions.CheckCollisions(player.location, dir, ref desiredMove);
    }

    void PushPushables(Directions dir)
    {
        // Push needs to come before collision detection.
        player.TryPushPushable(dir);
        
        if (playerReflection is Script_PlayerReflectionInteractive)
        {
            playerReflection.GetComponent<Script_PlayerReflectionInteractive>().TryPushPushable(dir);
        }
    }

    public void HandleIsMoving()
    {
        if (Progress >= 1f && isMoving)
        {
            Vector2 dirVector = new Vector2(
                Input.GetAxis(Const_KeyCodes.Horizontal), Input.GetAxis(Const_KeyCodes.Vertical)
            );
            
            if (dirVector == Vector2.zero)
            {
                isMoving = false;
            }
        }
    }

    /// <summary>
    /// Used when player is controlled by Timeline to update all locations
    /// </summary>
    public void UpdateLocation(Vector3 updatedPlayerLoc)
    {
        player.location = transform.position;
    }

    public virtual bool HandleExitTile(Directions dir)
    {
        Tilemap entrancesTileMap = game.EntranceTileMap;
        Tilemap[] exitsTileMaps = game.ExitTileMaps;
        
        Vector3 desiredMove = directionToVector[dir];
        Vector3 desiredLocation = player.location + desiredMove;
        
        Vector3Int tileLocation = new Vector3Int(
            (int)Mathf.Round(desiredLocation.x - grid.position.x), // *adjust for grid offset*
            (int)Mathf.Round(desiredLocation.z - grid.position.z),
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
                        return true;
                    }                    
                    
                    OnExitAnimations(dir);
                    
                    game.Exit(
                        exitInfo.Level,
                        exitInfo.PlayerNextSpawnPosition,
                        exitInfo.PlayerFacingDirection,
                        true,
                        exitInfo.IsSilent,
                        exitInfo.Type
                    );

                    return true;
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

            OnExitAnimations(dir);

            game.Exit(
                entranceInfo.Level,
                entranceInfo.PlayerNextSpawnPosition,
                entranceInfo.PlayerFacingDirection,
                false,
                entranceInfo.IsSilent,
                entranceInfo.Type
            );
            
            return true;
        }

        return false;
    }

    public void OnExitAnimations(Directions dir)
    {
        player.FaceDirection(dir);
        StopMovingAnimations();
    }

    private bool HandleExitObject(Directions dir)
    {
        return playerActionHandler.DetectDoorExit(dir);
    }

    private void HandleStairsExitAnimation()
    {
        spriteRenderer.GetComponent<Script_SortingOrder>().enabled = false;
        spriteRenderer.sortingOrder = exitUpStairsOrderLayer;
    }

    public Script_PlayerReflection CreatePlayerReflection(Vector3 reflectionAxis)
    {
        Script_PlayerReflection pr = Instantiate(
            PlayerReflectionPrefab,
            player.transform.position, // will update within Script_PlayerReflection
            Quaternion.identity
        );
        pr.Setup(
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

    public void UnsetPlayerReflection()
    {
        playerReflection = null;
    }

    public void RemoveReflection()
    {
        if (playerReflection != null)
        {
            playerReflection.gameObject.SetActive(false);
        }
    }

    public Script_WorldTile GetCurrentWorldTile(Vector3 location)
    {
        Vector3Int tileWorldLocation = location.ToVector3Int();
        Script_WorldTile[] worldTiles = Script_Game.Game.WorldTiles;

        if (worldTiles == null || worldTiles.Length == 0)
            return null;
        
        return playerCheckCollisions.GetCurrentWorldTile(worldTiles, tileWorldLocation);
    }

    // Called at each level initialization.
    public void InitializeOnLevel(Transform _grid)
    {
        grid = _grid;

        progress = 1f;
        
        xWeight = 0f;
        yWeight = 0f;

        lastMoveInput = Directions.None;
        isMoving = false;
    }

    // ------------------------------------------------------------------
    // Timeline Signals
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
        playerObjsToBind.Add(player.MySignalReceiver.gameObject);

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

    /// <summary>
    /// Use when we don't want to define an animator for Player via Timeline
    /// because animator controller may have changed based on Mask.
    /// 
    /// Ensure to call StopForceMoveAnimation at the end of Timeline.
    /// </summary>
    public void ForceMoveAnimation(int i)
    {
        isForceMoveAnimation = true;

        player.FaceDirection(i.IntToDirection());
        animator.SetBool(PlayerMovingAnimatorParam, true);
    }

    public void StopForceMoveAnimation()
    {
        isForceMoveAnimation = false;
    }

    // ------------------------------------------------------------------

    public void Setup(Script_Game _game)
    {
        game = _game;
        player = GetComponent<Script_Player>();
        director = GetComponent<PlayableDirector>();
        
        spriteRenderer = (SpriteRenderer)player.graphics;
        
        directionToVector = Script_Utils.GetDirectionToVectorDict();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_PlayerMovement))]
public class Script_PlayerMovementTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_PlayerMovement player = (Script_PlayerMovement)target;
        if (GUILayout.Button("Change Game State: Cut Scene"))
        {
            Script_Game.Game.ChangeStateCutScene();
        }

        if (GUILayout.Button("Change Game State: Interact"))
        {
            Script_Game.Game.ChangeStateInteract();
        }
    }
}
#endif