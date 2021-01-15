using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Script_PlayerStats))]
public class Script_Player : Script_Character
{
    /*
        persistent data, start
    */
    /*
        persistent data, end
    */
    public Animator animator;

    public Renderer graphics;
    public Action onAttackDone; 
    private Script_PlayerAction playerActionHandler;
    private Script_PlayerThoughtManager playerThoughtManager;
    private Script_PlayerMovement playerMovementHandler;
    private Script_PlayerEffect playerEffect;
    private Script_PlayerMovementAnimator playerMovementAnimator;
    public Script_InteractionBoxController interactionBoxController { get; private set; }


    private Script_PlayerStats playerStats;
    public Directions facingDirection;
    public Vector3 location;


    [SerializeField] private FadeSpeeds fadeSpeed;
    [Space]
    [SerializeField] private string state;
    [SerializeField] private string lastState;
    [SerializeField] private bool _isInvincible;
    [SerializeField] private bool _isInvisible;
    // storing soundFX here and not in manager because only 1 player exists
    private Script_Game game;
    private Script_PlayerReflection reflection;
    private bool isPlayerGhostMatchSortingLayer = false;
    private const string PlayerGlitch = "Base Layer.Player_Glitch";
    private Dictionary<Directions, Vector3> directionsToVector;
       
    public string State {
        get {
            return state;
        }
        private set {
            state = value;
        }
    }

    // Update is called once per frame
    void Update()
    {   
        /* ========================================================
            visuals
        ======================================================== */
        if (isPlayerGhostMatchSortingLayer)
        {
            playerMovementHandler.PlayerGhostSortOrder(
                Script_Utils.FindComponentInChildWithTag<SpriteRenderer>(
                    this.gameObject, Const_Tags.PlayerAnimator
                ).sortingOrder
            );
        }
        playerMovementHandler.TrackPlayerGhost();
        /* ======================================================== */

        if (game.state == Const_States_Game.Interact)
        {
            // playerMovementHandler.FinishMoveAnimation();
            // animator.SetBool("PlayerMoving", false);
            playerActionHandler.HandleActionInput(facingDirection, location);
            if (IsNotMovingState())
            {
                animator.SetBool("PlayerMoving", false);
            }
            else
            {
                // once we know we move onto an exit space
                // begin fading the screen out
                playerMovementHandler.HandleExitTile();
                playerMovementHandler.HandleMoveInput();
            }
        }
        else
        {
            if (game.state == Const_States_Game.DDR)
            {
                playerMovementHandler.HandleMoveInput();   
            }
            else
            {
                animator.SetBool("PlayerMoving", false);
            }
        }
    }

    // For migrating to new Input System
    // public void OnMove(InputValue input)
    // {
    //     if (
    //         game.state == Const_States_Game.Interact & IsMovingState()
    //         || game.state == Const_States_Game.DDR
    //     )
    //     {
    //         playerMovementHandler.HandleMoveInput(input.Get<Vector2>());
    //     }
    // }

    /// =========================================================================================
    /// STATE FUNCTIONS
    /// =========================================================================================
    public void SetState(string s)
    {
        lastState = State;
        State = s;
    }

    public void SetLastState()
    {
        SetState(lastState);
    }
    
    public void SetIsTalking()
    {
        SetState(Const_States_Player.Dialogue);
        animator.SetBool("PlayerMoving", false);
        Debug.Log($"Player state set to {state}!");
    }

    public void SetIsViewing()
    {
        SetState(Const_States_Player.Viewing);
        animator.SetBool("PlayerMoving", false);
        Debug.Log($"Player state set to {state}!");
    }

    public void SetIsPickingUp(Script_Item item)
    {
        SetState(Const_States_Player.PickingUp);
        animator.SetBool("PlayerMoving", false);
        Debug.Log($"Player state set to {state}!");
        
        SetItemShown(item);
    }

    public void SetIsInteract()
    {
        SetState(Const_States_Player.Interact);
        Debug.Log($"Player state set to {state}!");
    }

    public void SetIsInventory()
    {
        SetState(Const_States_Player.Inventory);
        Debug.Log($"Player state set to {state}!");
    }

    public void SetIsAttacking()
    {
        SetState(Const_States_Player.Attack);
        animator.SetBool("PlayerMoving", false);
        Debug.Log($"Player state set to {state}!");
    }

    public void SetIsStandby()
    {
        SetState(Const_States_Player.Standby);
        Debug.Log($"Player state set to {state}!");
    }

    private bool IsNotMovingState()
    {
        return State == Const_States_Player.Attack
                || State == Const_States_Player.Dialogue
                || State == Const_States_Player.Viewing
                || State == Const_States_Player.PickingUp
                || State == Const_States_Player.Standby
                || State == Const_States_Player.Inventory;
    }
    /// =========================================================================================
    /// COMBAT
    /// =========================================================================================
    public int FullHeal()
    {
        return GetComponent<Script_PlayerStats>().FullHeal();
    }

    public int Hurt(int dmg, Script_HitBox hitBox)
    {
        return GetComponent<Script_PlayerStats>().Hurt(dmg, hitBox);
    }
    /// =========================================================================================

    public bool isInvincible
    {
        get { return _isInvincible; }
        set { _isInvincible = value; }
    }

    public bool isInvisible
    {
        get { return _isInvisible; }
        set {
            _isInvisible = value;
            
            float t = Script_GraphicsManager.GetFadeTime(fadeSpeed);
            Script_PlayerGhost pg = GetPlayerGhost();

            if (_isInvisible)   playerEffect.SetVisibility(0, t, graphics, pg, null);
            else                playerEffect.SetVisibility(1, t, graphics, pg, null);
        }
    }

    public void RemoveReflection()
    {
        playerMovementHandler.RemoveReflection();
    }

    public void SetItemShown(Script_Item item)
    {
        playerActionHandler.itemShown = item;
    }
    public Script_Item GetItemShown()
    {
        return playerActionHandler.itemShown;
    }

    public void AnimatorSetDirection(Directions dir)
    {
        interactionBoxController.HandleActiveInteractionBox(dir);
        facingDirection = dir;

        if (dir == Directions.Up)
        {
            animator.SetFloat("LastMoveX", 0f);
            animator.SetFloat("LastMoveZ", 1f);
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveZ", 1f);
        }
        else if (dir == Directions.Down)
        {
            animator.SetFloat("LastMoveX", 0f);
            animator.SetFloat("LastMoveZ", -1f);
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveZ", -1f);
        }
        else if (dir == Directions.Left)
        {
            animator.SetFloat("LastMoveX", -1f);
            animator.SetFloat("LastMoveZ", 0f);
            animator.SetFloat("MoveX", -1f);
            animator.SetFloat("MoveZ", 0f);
        }
        else if (dir == Directions.Right)
        {
            animator.SetFloat("LastMoveX", 1f);
            animator.SetFloat("LastMoveZ", 0f);
            animator.SetFloat("MoveX", 1f);
            animator.SetFloat("MoveZ", 0f);
        }
    }

    public void CreatePlayerReflection(Vector3 axis)
    {
        playerMovementHandler.CreatePlayerReflection(axis);
    }

    public void SetPlayerReflection(Script_PlayerReflection pr)
    {
        playerMovementHandler.SetPlayerReflection(pr);
    }

    public void FaceDirection(Directions direction)
    {
        AnimatorSetDirection(direction);
    }

    public void QuestionMark(bool isShow)
    {
        playerEffect.QuestionMark(isShow);
    }

    public void ItemPickUpEffect(bool isShow, Script_Item item)
    {
        playerEffect.ItemPickUp(isShow, item);
    }

    public void GiantBoarNeedleEffect()
    {
        playerEffect.GiantBoarNeedle();
    }

    public void ForceSortingLayer(bool isForceSortingOrder, bool isAxisZ)
    {
        if (isForceSortingOrder)
        {
            Script_Utils.FindComponentInChildWithTag<Script_SortingOrder>(
                this.gameObject,
                Const_Tags.PlayerAnimator
            ).EnableWithOffset(
                Script_Graphics.playerSortOrderOffset,
                isAxisZ
            );
        }
        else
        {
            Script_Utils.FindComponentInChildWithTag<Script_SortingOrder>(
                this.gameObject,
                Const_Tags.PlayerAnimator
            ).DefaultSortingOrder();
        }
        PlayerGhostMatchSortingLayer();
    }

    public void PlayerGhostMatchSortingLayer()
    {
        isPlayerGhostMatchSortingLayer = true;
    }

    public Script_PlayerGhost GetPlayerGhost()
    {
        return playerMovementHandler.GetPlayerGhost();
    }

    public Script_PlayerMovementAnimator GetPlayerMovementAnimator()
    {
        return playerMovementAnimator;
    }

    public void SwitchLight(bool isOn)
    {
        playerMovementHandler.SwitchLight(isOn);
    }

    public void TryPushPushable(Directions dir)
    {
        playerActionHandler.TryPushPushable(dir);
    }

    public bool UseUsableKey(Script_UsableKey key)
    {
        return playerActionHandler.UseUsableKey(key, facingDirection);
    }

    /// <summary>
    /// To be called when manually need to finish picking up dialogue during cut scenes
    /// </summary>
    public void HandleEndItemDescriptionDialogue()
    {
        Debug.Log($"{name}: HandleEndItemDescriptionDialogue() itemShown: {playerActionHandler.itemShown}");
        playerActionHandler.HandleEndItemDescriptionDialogue(playerActionHandler.itemShown);
    }

    /// <summary>
    /// used when player is controlled by Timeline
    /// </summary>
    public void UpdateLocation()
    {
        location = transform.position;
        playerMovementHandler.UpdateLocation(location);
    }

    public void Teleport(Vector3 newLocation)
    {
        transform.position = newLocation;
        UpdateLocation();
    }

    public void DropSFX()
    {
        playerEffect.DropSFX();
    }

    /// Timeline ==========================================================================
    
    public void TimelineMoveUp()
    {
        playerMovementHandler.TimelineMoveUp();
    }

    public void EnterElevator()
    {
        playerMovementHandler.EnterElevator();
    }
    
    /// ===================================================================================

    public void InitializeOnLevel(
        Model_PlayerState playerState,
        bool isLightOn,
        Transform grid
    )
    {
        Vector3 spawnLocation = new Vector3(
            playerState.spawnX,
            playerState.spawnY,
            playerState.spawnZ
        );

        // Currently unadjusted
        Vector3 adjustedSpawnLocation = new Vector3(
            spawnLocation.x,
            spawnLocation.y,
            spawnLocation.z
        );

        transform.position = adjustedSpawnLocation;
        location = transform.position;
        FaceDirection(playerState.faceDirection);
        playerMovementHandler.InitializeOnLevel(grid);
        SwitchLight(isLightOn);

        Debug.Log($"Player initialized at position: {adjustedSpawnLocation.x}, {adjustedSpawnLocation.y}, {adjustedSpawnLocation.z}");
    }
    
    public void Setup(
        Directions direction,
        Model_PlayerState playerState,
        bool isLightOn
    )
    {   
        game = Script_Game.Game;
        // animator = GetComponent<Animator>();
        animator = Script_Utils.FindComponentInChildWithTag<Animator>(
            this.gameObject,
            Const_Tags.PlayerAnimator
        );
        directionsToVector = Script_Utils.GetDirectionToVectorDict();
        
        playerMovementHandler = GetComponent<Script_PlayerMovement>();
        playerActionHandler = GetComponent<Script_PlayerAction>();
        playerThoughtManager = GetComponent<Script_PlayerThoughtManager>();
        playerEffect = GetComponent<Script_PlayerEffect>();
        playerMovementAnimator = Script_Utils.FindChildWithTag(
            this.gameObject, Const_Tags.PlayerAnimator)
            .GetComponent<Script_PlayerMovementAnimator>();
        interactionBoxController = GetComponent<Script_InteractionBoxController>();
        playerStats = GetComponent<Script_PlayerStats>();

        playerMovementHandler.Setup(game, isLightOn);
        playerActionHandler.Setup(game);
        playerThoughtManager.Setup();
        playerEffect.Setup();
        
        /// Setup character stats
        base.Setup();
        
        location = transform.position;
        SetState(Const_States_Player.Interact);
        FaceDirection(direction);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_Player))]
public class Script_PlayerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_Player player = (Script_Player)target;
        if (GUILayout.Button("isInvisible = true"))
        {
            player.isInvisible = true;
        }

        if (GUILayout.Button("isInvisible = false"))
        {
            player.isInvisible = false;
        }

        if (GUILayout.Button("Giant Boar Needle"))
        {
            player.GiantBoarNeedleEffect();
        }
    }
}
#endif
