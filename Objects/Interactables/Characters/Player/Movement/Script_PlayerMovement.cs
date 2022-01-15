using System.Collections;
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
    
    public const int Layer = 0;
    
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
    [SerializeField] private float IceSpikeSpeedMultiplier;
    
    public int exitUpStairsOrderLayer;
    
    [SerializeField] private bool isMoving;
    [SerializeField] private Vector3 startLocation;

    [SerializeField] private bool isNorthWind;
    [SerializeField] private float windAdjustment;
    
    [SerializeField] private TimelineAsset moveUpTimeline;
    [SerializeField] private TimelineAsset enterElevatorTimeline;

    [SerializeField] private RuntimeAnimatorController defaultAnimatorController;

    [SerializeField] private Script_PlayerInteractionBox[] diagonalInteractionBoxes;

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

    [SerializeField] private Script_LimitedQueue<Directions> inputButtonDownBuffer = new Script_LimitedQueue<Directions>(4);

    public float xWeight;
    public float yWeight;

    [SerializeField] private Script_PlayerAction playerActionHandler;
    private bool didTryExit;

    private Script_PlayerCheckCollisions playerCheckCollisions;

    private bool isForceMoveAnimation;
    private bool isNoInputMoveByWind;
    
    private Script_WindManager windManager;

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

    public bool IsMoving
    {
        get => isMoving;
    }

    public bool IsNorthWind
    {
        get => isNorthWind;
        set => isNorthWind = value;
    }

    public Directions LastMove
    {
        get => lastMoveInput;
    }

    public AudioClip PassiveNotificationSFX { get; set; }

    /// <summary>
    /// Disable input but still maintain Player in moving state.
    /// E.g. Need Player to be affected by wind movement but do not
    /// move on input.
    /// </summary>
    public bool IsPassive { get; set; }
    
    void Awake()
    {
        playerCheckCollisions = GetComponent<Script_PlayerCheckCollisions>();
    }

    public void HandleMoveInput(bool isReversed = false)
    {
        HandleWalkSpeed();
        HandleAnimations();
        
        Vector2 dirVector = new Vector2(
            Input.GetAxis(Const_KeyCodes.Horizontal), Input.GetAxis(Const_KeyCodes.Vertical)
        );
        
        // ------------------------------------------------------------------
        // Input Buffer
        
        // Fill buffer with any new Button Presses caught during buffer time.
        if (progress < 1f)
        {
            BufferInput();
            return;
        }
        
        // Checking for passiveness must occur after progress check;
        // otherwise, the SFX will occur while Player is still making
        // the current move (forward).
        if (IsPassive)
        {
            HandlePassive();
            
            if (PassiveNotificationSFX != null)
            {
                Script_SFXManager.SFX.Play(PassiveNotificationSFX);
                PassiveNotificationSFX = null;
            }
            
            return;
        }

        // ------------------------------------------------------------------
        // Buffered Moves
        
        // Buffered moves always take priority.
        Directions bufferedInput = Directions.None;
        if (inputButtonDownBuffer.Count > 0)
        {
            bufferedInput = inputButtonDownBuffer.Peek();
            
            // If buffered a move, then set Animator.
            HandleBufferedMoveAnimations(bufferedInput);
            ExecuteMove(bufferedInput, isReversed);
            return;
        }
        
        // ------------------------------------------------------------------
        // New Button Presses
        
        if (Input.GetButtonDown(Const_KeyCodes.Up))
            ExecuteMove(Directions.Up, isReversed);
        else if (Input.GetButtonDown(Const_KeyCodes.Down))
            ExecuteMove(Directions.Down, isReversed);
        else if (Input.GetButtonDown(Const_KeyCodes.Right))
            ExecuteMove(Directions.Right, isReversed);
        else if (Input.GetButtonDown(Const_KeyCodes.Left))
            ExecuteMove(Directions.Left, isReversed);

        // ------------------------------------------------------------------
        // Button Holds

        else if (
            Input.GetButton(Const_KeyCodes.Up)
            || Input.GetButton(Const_KeyCodes.Down)
            || Input.GetButton(Const_KeyCodes.Right)
            || Input.GetButton(Const_KeyCodes.Left)
        )
        {
            // Handle coming from moving state, use the last move if it's still being held down.
            // (e.g. holding down Left continuously)
            // Note: isMoving is reset at every level initialization
            if (isMoving && Input.GetButton(lastMoveInput.DirectionToKeyCode()))
                ExecuteMove(lastMoveInput, isReversed);
            
            // If coming from a nonmoving state, define the new direction, giving priority
            // to Up, Down, Left, then Right.
            // (e.g. Game State was cut-scene when Button Press event happened so was missed,
            // and still holding that direction)
            if (!isMoving)
            {
                if (Input.GetButton(Const_KeyCodes.Up))
                    ExecuteMove(Directions.Up, isReversed);
                else if (Input.GetButton(Const_KeyCodes.Down))
                    ExecuteMove(Directions.Down, isReversed);
                else if (Input.GetButton(Const_KeyCodes.Right))
                    ExecuteMove(Directions.Right, isReversed);
                else if (Input.GetButton(Const_KeyCodes.Left))
                    ExecuteMove(Directions.Left, isReversed);
            }
        }
        
        // ------------------------------------------------------------------
        // No input
        else
        {
            HandlePassive();
            return;
        }

        void HandlePassive()
        {
            if (IsNorthWind)
            {
                Move(Directions.Down, _isNoInputMoveByWind: true);
                ClearInputBuffer();
                StopMovingAnimations();
            }
        }
    }
    
    private Directions ExecuteMove(Directions dir, bool isReversed)
    {
        var desiredDir = dir;

        if (isReversed)
        {
            desiredDir = dir switch
            {
                Directions.Up => Directions.Down,
                Directions.Down => Directions.Up,
                Directions.Right => Directions.Left,
                Directions.Left => Directions.Right,
                _ => Directions.None
            };
        }

        Move(desiredDir);

        ClearInputBuffer();

        return dir;
    }

    // Buffer input during buffer timer when Player is in motion.
    private void BufferInput()
    {
        // Buffer new button presses
        if (Input.GetButtonDown(Const_KeyCodes.Up))
            inputButtonDownBuffer.Enqueue(Const_KeyCodes.Up.KeyCodeToDirections());
        else if (Input.GetButtonDown(Const_KeyCodes.Down))
            inputButtonDownBuffer.Enqueue(Const_KeyCodes.Down.KeyCodeToDirections());
        else if (Input.GetButtonDown(Const_KeyCodes.Right))
            inputButtonDownBuffer.Enqueue(Const_KeyCodes.Right.KeyCodeToDirections());
        else if (Input.GetButtonDown(Const_KeyCodes.Left))
            inputButtonDownBuffer.Enqueue(Const_KeyCodes.Left.KeyCodeToDirections());
    }

    public void ClearInputBuffer()
    {
        inputButtonDownBuffer.Clear();
    }

    public void Move(Directions dir, bool _isNoInputMoveByWind = false)
    {
        isNoInputMoveByWind = _isNoInputMoveByWind;
        
        lastMoveInput = dir;
        
        // Handle Exits (once per tile)
        // If the exit has a Disabled Reaction, return and give control to the Action.
        if (!didTryExit && TryExit(dir))
            return;
        else
            didTryExit = true;
        
        if (progress < 1f)
            return;
        
        if (!isNoInputMoveByWind)
            AnimatorSetDirection(dir);
        
        PushPushables(dir);
        
        Vector3 desiredMove = directionToVector[dir];

        HandleNorthWindAdjustment(dir, ref desiredMove);

        if (CheckCollisions(player.location, dir, ref desiredMove))
        {
            didTryExit = false;    
            return;
        }

        // DDR mode, only changing directions to look like dancing.
        if (game.state == Const_States_Game.DDR)
            return;
        
        progress = 0f;

        // Actually move player to desired location.
        startLocation = player.location;
        player.location += desiredMove;

        isMoving = true;
        didTryExit = false;

        // Adjust desiredMove and walk speed to simulate wind resistance.
        void HandleNorthWindAdjustment(Directions dir, ref Vector3 desiredMove)
        {
            if (IsNorthWind)
            {
                string activeMaskId = Script_ActiveStickerManager.Control.ActiveSticker?.id ?? string.Empty;
                bool isRunning = walkSpeed == Speeds.Run || walkSpeed == Speeds.Dev;
                
                // On LR moves, move also 1 down.
                if (dir == Directions.Left || dir == Directions.Right)
                {
                    var adjustedLocation = new Vector3(player.location.x, player.location.y, player.location.z - 1);
                    
                    if (
                        // Handle adjusted tile collisions.
                        (CheckCollisions(adjustedLocation, dir, ref desiredMove))
                        // Detect SW & SE collisions.
                        || (dir == Directions.Left && diagonalInteractionBoxes[0].GetInteractablesBlocking().Count > 0)
                        || (dir == Directions.Right && diagonalInteractionBoxes[1].GetInteractablesBlocking().Count > 0)
                    )
                    {
                        windAdjustment = windManager.Lateral(activeMaskId, isRunning);
                    }
                    else
                    {
                        windAdjustment = windManager.Diagonal(activeMaskId, isRunning);
                        desiredMove.z -= 1;
                    }
                }
                else if (dir == Directions.Up)
                {
                    windAdjustment = windManager.Headwind(activeMaskId, isRunning);
                }
                else if (dir == Directions.Down)
                {
                    if (isNoInputMoveByWind)
                        windAdjustment = windManager.Passive(activeMaskId, isRunning);
                    else
                        windAdjustment = windManager.Tailwind(activeMaskId, isRunning);
                }
            }
            else
            {
                windAdjustment = windManager.NoEffectFactor;
            }
        }
    }

    private float WalkSpeed(string maskId)
    {
        float speed = walkSpeed switch
        {
            Speeds.Default => defaultSpeed,
            Speeds.Run => runSpeed,
            Speeds.Dev => devRunSpeed,
            _ => defaultSpeed
        };

        float maskMultiplier = maskId switch
        {
            Const_Items.IceSpikeId => IceSpikeSpeedMultiplier,
            _ => 1f
        };

        // Apply wind adjustments if wind is blowing, ignoring any Mask Multiplier adjustments.
        // Run speed is applied via its own WindFactors. Run speed will never be affected by
        // Mask Multiplier because you can only Run when in Former Self.
        float adjustedSpeed;
        if (isNorthWind)
            adjustedSpeed = speed * windAdjustment;
        else
            adjustedSpeed = speed * maskMultiplier;
        
        float framesPerMove = 1f / (Time.fixedDeltaTime * adjustedSpeed);
        float roundedFramesPerMove = Mathf.RoundToInt(framesPerMove);
        if (!Mathf.Approximately(roundedFramesPerMove, framesPerMove))
            Debug.LogWarning($"Motion not smooth; frames per move {framesPerMove} floored {roundedFramesPerMove}");
        
        return adjustedSpeed;
    }

    private bool TryExit(Directions dir) => HandleExitTile(dir) || HandleExitObject(dir);

    public void HandleMoveTransform(bool isReversed = false)
    {
        if (progress < 1f)
        {
            MoveTransform();   
        }
        // Make button holds smooth.
        // If we're already at progress == 1f and waiting for the Update clock,
        // execute the Move() call here (which is called on FixedUpdate clock),
        // allowing for smooth movement.
        else if (progress == 1f && isMoving)
        {
            // Check for Player Moving State to stop moving if State
            // was modified mid-transform (e.g. Changing Mask to a non-moving one).
            bool isStopMoving = false;
            player.HandleAction(null, null, () => {
                Debug.Log($"Player is not in moving state, stopping Fixed Move Transform");
                isStopMoving = true;
            });
            
            if (isStopMoving || IsPassive)
            {
                StopMovingAnimations();
                ClearInputBuffer();
                isMoving = false;
                return;
            }
            
            Vector2 dirVector = new Vector2(
                Input.GetAxis(Const_KeyCodes.Horizontal),
                Input.GetAxis(Const_KeyCodes.Vertical)
            );
            // Get any bufferedInput from buffered Button Downs or currently Held button.
            Directions bufferedInput = GetBufferedButtonDownOrHold(dirVector);
            if (bufferedInput != Directions.None)
            {
                ExecuteMove(bufferedInput, isReversed);
                MoveTransform();
            }
        }

        void MoveTransform()
        {
            progress += WalkSpeed(Script_ActiveStickerManager.Control.ActiveSticker?.id) * Time.fixedDeltaTime;
            
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

        Directions GetBufferedButtonDownOrHold(Vector2 dirVector)
        {
            Directions bufferedInput = Directions.None;

            // Check for buffered ButtonDowns
            if (inputButtonDownBuffer.Count > 0)
            {
                bufferedInput = inputButtonDownBuffer.Peek();

                Debug.Log($"Player using Fixed Buffered PRESS Move: {bufferedInput}");
            }
            // If no button down presses buffered, check current axis being held down.
            else if (dirVector != Vector2.zero)
            {
                if (dirVector.y > 0f)
                    bufferedInput = Directions.Up;
                else if (dirVector.y < 0f)
                    bufferedInput = Directions.Down;
                else if (dirVector.x > 0f)
                    bufferedInput = Directions.Right;
                else if (dirVector.x < 0f)
                    bufferedInput = Directions.Left;
                
                Debug.Log($"Player using Fixed Buffered HOLD Move: {bufferedInput}; dirVector {dirVector}");
            }
            
            return bufferedInput;
        }
    }

    private void HandleWalkSpeed()
    {
        int slot = 0;
        bool hasSpeedSealAndIsFormerSelf = game.GetItemsInventoryItem(Const_Items.SpeedSeal, out slot)
            && Script_ActiveStickerManager.Control.ActiveSticker == null;
        
        bool isDev = Debug.isDebugBuild || Const_Dev.IsDevMode;

        if (Input.GetButton(Const_KeyCodes.Dev) && isDev)
            walkSpeed = Speeds.Dev;
        else if (Input.GetButton(Const_KeyCodes.Action3) && hasSpeedSealAndIsFormerSelf)
            walkSpeed = Speeds.Run;
        else
            walkSpeed = Speeds.Default;
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
        
        bool isMovingAnimation = !isNoInputMoveByWind && (
            isMoving
            || Input.GetAxis(Const_KeyCodes.Vertical) != 0f
            || Input.GetAxis(Const_KeyCodes.Horizontal) != 0f
        );
        
        animator.SetBool(PlayerMovingAnimatorParam, isMovingAnimation);
    }

    private void HandleBufferedMoveAnimations(Directions bufferedMove)
    {
        animator.SetBool(PlayerMovingAnimatorParam, bufferedMove != Directions.None);
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

    public void SyncAnimatorState(AnimatorStateInfo animatorStateInfo)
    {
        MyAnimator.Play(animatorStateInfo.fullPathHash, Layer, animatorStateInfo.normalizedTime);
    }

    private bool CheckCollisions(Vector3 location, Directions dir, ref Vector3 desiredMove)
    {   
        // Do not allow Player to move if Reflection cannot move.
        if (playerReflection is Script_PlayerReflectionInteractive && playerReflection.gameObject.activeInHierarchy)
        {
            if (!playerReflection.GetComponent<Script_PlayerReflectionInteractive>().CanMove())
                return true;
        }
        
        return playerCheckCollisions.CheckCollisions(location, dir, ref desiredMove);
    }

    private void PushPushables(Directions dir)
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

    protected virtual bool HandleExitTile(Directions dir)
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
            foreach (Tilemap tm in exitsTileMaps)
            {
                if (tm.HasTile(tileLocation) && tm.gameObject.activeSelf)
                {
                    Script_TileMapExitEntrance exitInfo = tm.GetComponent<Script_TileMapExitEntrance>();

                    if (exitInfo.IsDisabled)
                    {
                        // To avoid flicker when approaching a disabled reaction
                        // that turns state to non-moving.
                        OnExitAnimations(dir);
                        return exitInfo.HandleDisabledReaction();
                    }
                    
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
            
            if (entranceInfo.IsDisabled)
            {
                // To avoid flicker when approaching a disabled reaction
                // that turns state to non-moving.
                OnExitAnimations(dir);
                return entranceInfo.HandleDisabledReaction();
            }
            
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

        isNoInputMoveByWind = false;
        windAdjustment = windManager.NoEffectFactor;
        IsNorthWind = false;
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
    /// 
    /// Elevator
    /// Fireplace Retreat
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

        windManager = Script_WindManager.Control;
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