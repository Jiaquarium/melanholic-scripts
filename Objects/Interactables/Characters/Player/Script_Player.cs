using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Script_PlayerStats))]
[RequireComponent(typeof(PlayableDirector))]
public class Script_Player : Script_Character
{
    public const string IsEffectTrigger = "IsEffect";
    
    public Renderer graphics;
    public Action onAttackDone; 
    [SerializeField] protected Script_PlayerMovement playerMovementHandler;
    
    protected Script_PlayerAction playerActionHandler;
    private Script_PlayerThoughtManager playerThoughtManager;
    [SerializeField] private Script_PlayerEffect playerEffect;
    public Script_InteractionBoxController interactionBoxController { get; private set; }

    [SerializeField] private Script_PlayerGraphics playerGraphics;

    private Script_PlayerStats playerStats;
    [SerializeField] private Directions _facingDirection;
    public Vector3 location;

    [SerializeField] private FadeSpeeds fadeSpeed;
    
    [Space]
    [SerializeField] private string state;
    [SerializeField] private string lastState;
    
    [SerializeField] private bool _isInvincible;
    [SerializeField] private bool _isInvisible;
    
    [SerializeField] private Transform focalPoint;
    
    [SerializeField] private PlayableDirector director;

    protected Script_Game game;
    
    private Script_PlayerReflection reflection;
    protected bool isPlayerGhostMatchSortingLayer = false;
    private const string PlayerGlitch = "Base Layer.Player_Glitch";
    private Dictionary<Directions, Vector3> directionsToVector;
       
    public string State {
        get => state;
        private set => state = value;
    }

    public Directions FacingDirection
    {
        get => _facingDirection;
        set => _facingDirection = value;
    }

    public Animator MyAnimator
    {
        get => playerMovementHandler.MyAnimator;
    }

    public PlayableDirector Director
    {
        get => director;
    }

    public bool isInvincible
    {
        get { return _isInvincible; }
        set { _isInvincible = value; }
    }

    public bool isInvisible
    {
        get => _isInvisible;
    }

    public Material MyMaterial
    {
        get => playerGraphics.PlayerGraphicsMaterial;
        set => playerGraphics.PlayerGraphicsMaterial = value;
    }

    public Transform FocalPoint
    {
        get => focalPoint;
    }

    protected virtual void OnEnable()
    {
        if (playerMovementHandler.PlayerGhost != null)
            playerMovementHandler.PlayerGhost.gameObject.SetActive(true);
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();

        if (playerMovementHandler.PlayerGhost != null)
            playerMovementHandler.PlayerGhost.gameObject.SetActive(false);
    }
    
    protected virtual void Update()
    {   
        // ------------------------------------------------------------------
        // Visuals
        HandleIsMoving();
        // ------------------------------------------------------------------

        if (game.state == Const_States_Game.Interact)
        {
            playerActionHandler.HandleActionInput(FacingDirection, location);
            
            if (IsNotMovingState())
            {
                StopMovingAnimations();
                
                // Finish up Ghost Follow on nonmoving states.
                playerMovementHandler.HandleGhostTransform(isForceTimerUpdate: true);
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
                StopMovingAnimations();
                playerMovementHandler.HandleGhostTransform(isForceTimerUpdate: true);
            }
        }
    }

    

    // ------------------------------------------------------------------
    // State
    public void SetState(string s)
    {
        lastState = State;
        State = s;
    }

    public void SetLastState()
    {
        SetState(lastState);
    }

    public void SetIsInteract()
    {
        SetState(Const_States_Player.Interact);
        Debug.Log($"Player state set to {state}!");
    }

    public void EndOfFrameSetIsInteract()
    {
        StartCoroutine(WaitSetIsInteract());
        
        IEnumerator WaitSetIsInteract()
        {
            yield return new WaitForEndOfFrame();

            SetState(Const_States_Player.Interact);
            Debug.Log($"Player state set to {state}!");
        }
    }

    public void SetIsTalking()
    {
        SetState(Const_States_Player.Dialogue);
        StopMovingAnimations();
        Debug.Log($"Player state set to {state}!");
    }

    public void SetIsViewing()
    {
        SetState(Const_States_Player.Viewing);
        StopMovingAnimations();
        Debug.Log($"Player state set to {state}!");
    }

    public void SetIsPickingUp(Script_Item item)
    {
        SetState(Const_States_Player.PickingUp);
        StopMovingAnimations();
        Debug.Log($"Player state set to {state}!");
        
        SetItemShown(item);
    }

    public void SetIsInventory()
    {
        SetState(Const_States_Player.Inventory);
        StopMovingAnimations();
        Debug.Log($"Player state set to {state}!");
    }

    public void SetIsAttacking()
    {
        SetState(Const_States_Player.Attack);
        StopMovingAnimations();
        Debug.Log($"Player state set to {state}!");
    }

    public void SetIsStandby()
    {
        SetState(Const_States_Player.Standby);
        StopMovingAnimations();
        Debug.Log($"Player state set to {state}!");
    }

    public void SetIsPuppeteer()
    {
        SetState(Const_States_Player.Puppeteer);
        StopMovingAnimations();
        Debug.Log($"Player state set to {state}!");
    }

    public void SetIsPuppeteerNull()
    {
        SetState(Const_States_Player.PuppeteerNull);
        StopMovingAnimations();
        Debug.Log($"Player state set to {state}!");
    }

    public void SetIsEffect()
    {
        SetState(Const_States_Player.Effect);
        StopMovingAnimations();
        Debug.Log($"Player state set to {state}!");
    }

    public void SetIsLastElevatorEffect()
    {
        SetState(Const_States_Player.LastElevatorEffect);
        StopMovingAnimations();
        Debug.Log($"Player state set to {state}!");
    }

    protected bool IsNotMovingState()
    {
        return State == Const_States_Player.Attack
                || State == Const_States_Player.Effect
                || State == Const_States_Player.Dialogue
                || State == Const_States_Player.Viewing
                || State == Const_States_Player.PickingUp
                || State == Const_States_Player.Standby
                || State == Const_States_Player.Puppeteer
                || State == Const_States_Player.Inventory
                || State == Const_States_Player.PuppeteerNull
                || State == Const_States_Player.LastElevatorEffect;
    }

    protected void StopMovingAnimations()
    {
        MyAnimator.SetBool(Script_PlayerMovement.PlayerMovingAnimatorParam, false);
        playerMovementHandler.PlayerGhost.StopMoveAnimation();
    }

    // ------------------------------------------------------------------
    // Combat
    public int FullHeal()
    {
        return GetComponent<Script_PlayerStats>().FullHeal();
    }

    public int Hurt(int dmg, Script_HitBox hitBox)
    {
        return GetComponent<Script_PlayerStats>().Hurt(dmg, hitBox);
    }
    
    // ------------------------------------------------------------------
    // Graphics
    protected void HandleIsMoving()
    {
        playerMovementHandler.HandleIsMoving();
    }

    public void ChangeMaterial(Script_PlayerGraphics.Materials material)
    {
        playerGraphics.ChangeMaterial(material);
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
        playerMovementHandler.AnimatorSetDirection(direction);
    }

    public void QuestionMark(bool isShow)
    {
        playerEffect.QuestionMark(isShow);
    }

    public void ItemPickUpEffect(bool isShow, Script_Item item)
    {
        playerEffect.ItemPickUp(isShow, item);
    }

    public void ForceSortingLayer(bool isForceSortingOrder, bool isAxisZ)
    {
        if (isForceSortingOrder)
        {
            Script_Utils.FindComponentInChildWithTag<Script_SortingOrder>(
                this.gameObject,
                Const_Tags.PlayerAnimator
            ).EnableWithOffset(
                Script_SortingOrder.playerSortOrderOffset,
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
        return playerMovementHandler.PlayerGhost;
    }

    // Call with fadeTime = 0f for instant.
    // For materials where alpha is not affected, this will instantly disable player Graphics. 
    public void SetInvisible(bool isHide, float fadeTime = -1f)
    {
        _isInvisible = isHide;
        
        Script_PlayerGhost pg = GetPlayerGhost();
        
        // playerGraphics.SetHidden(_isInvisible);
        pg.PlayerGhostGraphics.SetHidden(_isInvisible);
    }

    public void HandleEatGraphics(bool isEating)
    {
        Script_PlayerGhost pg = GetPlayerGhost();

        playerGraphics.SetHidden(!isEating);
        pg.PlayerGhostGraphics.SetHidden(isEating);
    }

    public void SetBuffEffectActive(bool isActive)
    {
        playerEffect.SetBuffEffectActive(isActive);
    }

    public void TriggerEffect()
    {
        MyAnimator.SetTrigger(IsEffectTrigger);
        playerMovementHandler.PlayerGhost.MyAnimator.SetTrigger(IsEffectTrigger);
    }

    // ------------------------------------------------------------------
    // Interactions

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
        return playerActionHandler.UseUsableKey(key, FacingDirection);
    }

    /// <summary>
    /// To be called when manually need to finish picking up dialogue during cut scenes
    /// </summary>
    public void HandleEndItemDescriptionDialogue()
    {
        Debug.Log($"{name}: HandleEndItemDescriptionDialogue() itemShown: {playerActionHandler.itemShown}");
        playerActionHandler.HandleEndItemDescriptionDialogue(playerActionHandler.itemShown);
    }

    // ------------------------------------------------------------------
    // Public Utils
    
    /// <summary>
    /// used when player is controlled by Timeline
    /// </summary>
    public void UpdateLocation()
    {
        location = transform.position;
        playerMovementHandler.UpdateLocation(location);
    }

    public void DropSFX()
    {
        playerEffect.DropSFX();
    }

    public void ScarletCipherPickUpSFX()
    {
        SetIsStandby();
        playerEffect.ScarletCipherPickUpSFX(() => {
            SetIsInteract();
        }); 
    }

    public void Teleport(Vector3 newLocation)
    {
        transform.position = newLocation;
        UpdateLocation();
    }
    
    public void TimelineMoveUp()
    {
        playerMovementHandler.TimelineMoveUp();
    }

    public void EnterElevator()
    {
        playerMovementHandler.EnterElevator();
    }

    /// <summary>
    /// Use to control Player movement from an outside source instead of Input.
    /// </summary>
    public override void ForcePush(Directions dir)
    {
        playerMovementHandler.Move(dir);
    }
    // ------------------------------------------------------------------

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
        directionsToVector = Script_Utils.GetDirectionToVectorDict();
        
        playerMovementHandler = GetComponent<Script_PlayerMovement>();
        playerActionHandler = GetComponent<Script_PlayerAction>();
        playerThoughtManager = GetComponent<Script_PlayerThoughtManager>();
        playerEffect = GetComponent<Script_PlayerEffect>();
        
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
            player.SetInvisible(true);
        }

        if (GUILayout.Button("isInvisible = false"))
        {
            player.SetInvisible(false);
        }

        if (GUILayout.Button("Force Push Down"))
        {
            player.ForcePush(Directions.Down);
        }

        if (GUILayout.Button("Teleport Up 1"))
        {
            Vector3 newPos = new Vector3(
                player.transform.position.x,
                player.transform.position.y + 1,
                player.transform.position.z
            );
            
            player.Teleport(newPos);
        }

        if (GUILayout.Button("Buff Effect"))
        {
            player.SetBuffEffectActive(true);
        }   
    }
}
#endif
