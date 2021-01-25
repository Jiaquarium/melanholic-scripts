using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(PlayableDirector))]
[RequireComponent(typeof(Script_PlayerCheckCollisions))]
public class Script_PlayerMovement : MonoBehaviour
{
    public Script_PlayerGhost PlayerGhostPrefab;
    public Script_PlayerReflection PlayerReflectionPrefab;


    [SerializeField] private float defaultRepeatDelay;
    [SerializeField] private float runRepeatDelay;
    [SerializeField] private float defaultGhostSpeed;
    [SerializeField] private float runGhostSpeed;
    public int exitUpStairsOrderLayer;
    public bool isMoving;
    [SerializeField] private TimelineAsset moveUpTimeline;
    [SerializeField] private TimelineAsset enterElevatorTimeline;
    

    private float repeatDelay;
    private Script_Game game;
    private Script_Player player;
    private Script_PlayerGhost playerGhost;
    private Script_PlayerReflection playerReflection;
    private Dictionary<Directions, Vector3> directionToVector;
    private SpriteRenderer spriteRenderer;
    private Vector3[] NPCLocations = new Vector3[0];
    private Vector3[] DemonLocations = new Vector3[0];
    private Vector3[] InteractableObjectLocations = new Vector3[0];
    private Transform grid;


    public float progress;
    
    
    public Directions lastMove;
    public float timer;
    
    void OnDestroy() {
        if (playerGhost != null)    Destroy(playerGhost.gameObject);
    }
    
    public void HandleMoveInput()
    {
        HandleWalkSpeed();
        
        timer = Mathf.Max(0f, timer - Time.deltaTime);

        SetMoveAnimation();

        Vector2 dirVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (dirVector == Vector2.zero)  return;
        
        // determine if vector is up, down, left or right direction headed
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
            repeatDelay = runRepeatDelay;
            playerGhost.Speed = runGhostSpeed;
        }
        else
        {
            repeatDelay = defaultRepeatDelay;
            playerGhost.Speed = defaultGhostSpeed;
        }
    }

    void SetMoveAnimation()
    {
        // move animation when direction button down 
        player.animator.SetBool(
            "PlayerMoving",
            Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f
        );

        playerGhost.SetMoveAnimation();
    }

    bool CheckRepeatMove(Directions dir)
    {
        return timer == 0f;
    }

    void Move(Directions dir)
    {
        /// on button presses reset timer to allow for instant direction changes
        if (
            Input.GetButtonDown(Const_KeyCodes.Up)
            || Input.GetButtonDown(Const_KeyCodes.Right)
            || Input.GetButtonDown(Const_KeyCodes.Down)
            || Input.GetButtonDown(Const_KeyCodes.Left)
        )
        {
            timer = 0;
        }

        if (dir == lastMove)
        {
            if (!CheckRepeatMove(dir)) return;
        }

        Vector3 desiredDirection = directionToVector[dir];
        
        player.AnimatorSetDirection(dir);
        playerGhost.AnimatorSetDirection(dir);
        PushPushables(dir);

        if (CheckCollisions(dir))  return;

        /// in DDR mode, only changing directions to look like dancing
        if (game.state == "ddr")    return;
        
        progress = 0f;
        timer = repeatDelay;

        // move player to desired loc, and start playerGhost's animation
        // after-the-fact
        playerGhost.startLocation = player.location;

        player.location += desiredDirection;
        playerGhost.location += desiredDirection;
        // move player pointer immediately
        transform.position = player.location;
        HandleMoveAnimation(dir);

        lastMove = dir;
    }

    void HandleMoveAnimation(Directions dir)
    {
        isMoving = true;
        playerGhost.Move(dir);
        spriteRenderer.enabled = false;
    }

    bool CheckCollisions(Directions dir)
    {   
        // if reflection is interactive check if it can move; if not, disallow player from moving as well
        if (playerReflection is Script_PlayerReflectionInteractive)
        {
            if (!playerReflection.GetComponent<Script_PlayerReflectionInteractive>().CanMove())
                return true;
        }
        
        return GetComponent<Script_PlayerCheckCollisions>().CheckCollisions(
            player.location, dir
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
        progress = playerGhost.progress;
        
        if (progress >= 1f && isMoving)
        {
            FinishMoveAnimation();
        }
    }

    /// <summary>
    /// used when player is controlled by Timeline to update all locations
    /// </summary>
    public void UpdateLocation(Vector3 updatedPlayerLoc)
    {
        playerGhost.UpdateLocation(updatedPlayerLoc);
    }

    void FinishMoveAnimation()
    {
        // must be visible before playerghost invisible to avoid flicker
        spriteRenderer.enabled = true;
        playerGhost.SetIsNotMoving();
        isMoving = false;
    }

    public void HandleExitTile()
    {
        Tilemap entrancesTileMap = game.GetEntrancesTileMap();
        Tilemap[] exitsTileMaps = game.GetExitsTileMaps();
        
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

    void HandleStairsExitAnimation()
    {
        // Script_Utils.FindComponentInChildWithTag<SpriteRenderer>(
        //     this.gameObject, Const_Tags.PlayerAnimator
        // ).sortingOrder = exitUpStairsOrderLayer;
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

    public Script_PlayerGhost GetPlayerGhost()
    {
        return playerGhost;
    }

    public void SwitchLight(bool isOn)
    {
        playerGhost.SwitchLight(isOn);
    }
    
    public void InitializeOnLevel(Transform _grid)
    {
        playerGhost.Setup(player.transform.position);
        grid = _grid;
    }

    /// Timeline ==========================================================================
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
    /// ===================================================================================

    public void Setup(Script_Game _game, bool isLightOn)
    {
        game = _game;
        player = GetComponent<Script_Player>();
        
        spriteRenderer = (SpriteRenderer)player.graphics;

        // setup ghost for smooth movement (allows cancellation of mid-animation)
        playerGhost = CreatePlayerGhost(isLightOn);
        playerGhost.Setup(player.transform.position);
        playerGhost.transform.SetParent(game.playerContainer, false);
        
        directionToVector = Script_Utils.GetDirectionToVectorDict();

        timer = repeatDelay;
        progress = 1f;
    }
}
