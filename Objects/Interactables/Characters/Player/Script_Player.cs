﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System;
using UnityEngine.Timeline;
using UnityEngine.InputSystem;
using Rewired;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Script_PlayerStats))]
[RequireComponent(typeof(PlayableDirector))]
public class Script_Player : Script_Character
{
    public static readonly int IsEffectTrigger = Animator.StringToHash("IsEffect");
    public static readonly int IsEffectHoldBool = Animator.StringToHash("IsEffectHold");
    
    public static int PlayerId = 0;
    
    public Renderer graphics;
    public Action onAttackDone; 
    [SerializeField] protected Script_PlayerMovement playerMovementHandler;
    
    protected Script_PlayerAction playerActionHandler;
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
    [SerializeField] private bool isFinalRound;
    
    [SerializeField] private Transform focalPoint;
    
    [SerializeField] private PlayableDirector director;

    [SerializeField] private Light playerLight;
    [SerializeField] private SignalReceiver signalReceiver;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Player rewiredInput;

    [Space][Header("Animator Controllers")][Space]

    [SerializeField] private Script_PsychicDuckEffect psychicDuckEffect;
    [SerializeField] private RuntimeAnimatorController cachedAnimatorController;

    protected Script_Game game;
    
    private Script_PlayerReflection reflection;
    private const string PlayerGlitch = "Base Layer.Player_Glitch";
    private Dictionary<Directions, Vector3> directionsToVector;

    public string State
    {
        get => state;
        private set => state = value;
    }

    public string LastState
    {
        get => lastState;
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

    public RuntimeAnimatorController DefaultAnimator
    {
        get => playerMovementHandler.DefaultAnimatorController;
    }

    public Script_PlayerCheckCollisions PlayerCheckCollisions => playerMovementHandler.PlayerCheckCollisions;

    public PlayableDirector Director
    {
        get => director;
    }

    public SignalReceiver MySignalReceiver
    {
        get => signalReceiver;
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

    public Material MySharedMaterial => playerGraphics.PlayerGraphicsSharedMaterial;

    public Transform FocalPoint
    {
        get => focalPoint;
    }

    public bool AnimatorEffectHold
    {
        get => MyAnimator.GetBool(IsEffectHoldBool);
        set
        {
            MyAnimator.SetBool(IsEffectHoldBool, value);

            Script_StickerEffectEventsManager.AnimatorEffectHold(value);
        }
    }

    public bool IsLightOn
    {
        get => playerActionHandler.IsLanternLightOn;
    }

    public bool IsPuppeteerEffectHoldOn
    {
        get => playerActionHandler.IsPuppeteerEffectHoldOn;
    }

    public bool IsNorthWind
    {
        get => playerMovementHandler.IsNorthWind;
        set => playerMovementHandler.IsNorthWind = value;
    }

    public bool IsEmphasizeWalk
    {
        get => playerMovementHandler.IsEmphasizeWalk;
        set => playerMovementHandler.IsEmphasizeWalk = value;
    }

    public bool IsActuallyRunning
    {
        get
        {
            if (playerMovementHandler != null)
                return playerMovementHandler.IsActuallyRunning;

            return false;
        }
    }

    public Directions LastMove
    {
        get => playerMovementHandler.LastMove;
    }

    public bool SpriteXFlip
    {
        get
        {
            var spriteRenderer = graphics as SpriteRenderer;
            if (spriteRenderer != null)
                return spriteRenderer.flipX;
            
            return false;
        }
    }

    public bool SpriteYFlip
    {
        get
        {
            var spriteRenderer = graphics as SpriteRenderer;
            if (spriteRenderer != null)
                return spriteRenderer.flipY;
            
            return false;
        }
    }

    public bool IsPassive
    {
        get => playerMovementHandler.IsPassive;
        set
        {
            Dev_Logger.Debug($"Setting IsPassive: {IsPassive}");
            playerMovementHandler.IsPassive = value;
        }
    }

    public AudioClip PassiveNotificationSFX
    {
        get => playerMovementHandler.PassiveNotificationSFX;
        set => playerMovementHandler.PassiveNotificationSFX = value;
    }

    public bool IsFinalRound
    {
        get => isFinalRound;
        set => isFinalRound = value;
    }

    public Player RewiredInput
    {
        get => rewiredInput;
        private set => rewiredInput = value;
    }

    public void FlipSprite(bool flipX, bool flipY)
    {
        var spriteRenderer = graphics as SpriteRenderer;
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = flipX;
            spriteRenderer.flipY = flipY;
        }
    }

    protected virtual void OnEnable()
    {

    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    
    protected virtual void Update()
    {   
        if (game.IsSettingsOpen)
            return;
        
        // ------------------------------------------------------------------
        // Visuals
        HandleIsMoving();
        // ------------------------------------------------------------------
        
        HandleAction(
            () => playerActionHandler.HandleActionInput(FacingDirection, location),
            () => playerMovementHandler.HandleMoveInput(),
            StopMoving
        );
    }

    protected virtual void FixedUpdate()
    {
        playerMovementHandler.HandleMoveTransform();
    }

    public virtual void HandleAction(
        Action actionHandler,
        Action moveHandler,
        Action stopMovingHandler
    )
    {
        if (game.state == Const_States_Game.Interact)
        {
            if (actionHandler != null)
                actionHandler();
            
            if (IsNotMovingState())
            {
                if (stopMovingHandler != null)
                    stopMovingHandler();
            }
            else
            {
                if (moveHandler != null)
                    moveHandler();
            }
        }
        else
        {
            if (game.state == Const_States_Game.DDR)
            {
                if (moveHandler != null)
                    moveHandler();
            }
            else
            {
                if (stopMovingHandler != null)
                    stopMovingHandler();
            }
        }
    }

    public void ClearLevelState()
    {
        playerMovementHandler.UnsetPlayerReflection();
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
        Dev_Logger.Debug($"Player state set to {state}!");
    }

    public void EndOfFrameSetIsInteract()
    {
        StartCoroutine(WaitSetIsInteract());
        
        IEnumerator WaitSetIsInteract()
        {
            yield return new WaitForEndOfFrame();

            SetState(Const_States_Player.Interact);
            Dev_Logger.Debug($"Player state set to {state}!");
        }
    }

    public void SetIsTalking()
    {
        SetState(Const_States_Player.Dialogue);
        StopMoving();
        Dev_Logger.Debug($"Player state set to {state}!");
    }

    public void SetIsViewing()
    {
        SetState(Const_States_Player.Viewing);
        StopMoving();
        Dev_Logger.Debug($"Player state set to {state}!");
    }

    public void SetIsPickingUp(Script_Item item)
    {
        SetState(Const_States_Player.PickingUp);
        StopMoving();
        Dev_Logger.Debug($"Player state set to {state}!");
        
        SetItemShown(item);
    }

    public void SetIsInventory()
    {
        SetState(Const_States_Player.Inventory);
        StopMoving();
        Dev_Logger.Debug($"Player state set to {state}!");
    }

    public void SetIsAttacking()
    {
        SetState(Const_States_Player.Attack);
        StopMoving();
        Dev_Logger.Debug($"Player state set to {state}!");
    }

    public void SetIsStandby()
    {
        SetState(Const_States_Player.Standby);
        StopMoving();
        Dev_Logger.Debug($"Player state set to {state}!");
    }

    public void SetIsPuppeteer()
    {
        SetState(Const_States_Player.Puppeteer);
        StopMoving();
        Dev_Logger.Debug($"Player state set to {state}!");
    }

    public void SetIsPuppeteerNull()
    {
        SetState(Const_States_Player.PuppeteerNull);
        StopMoving();
        Dev_Logger.Debug($"Player state set to {state}!");
    }

    public void SetIsEffect()
    {
        SetState(Const_States_Player.Effect);
        StopMoving();
        Dev_Logger.Debug($"Player state set to {state}!");
    }

    public void SetIsLastElevatorEffect()
    {
        SetState(Const_States_Player.LastElevatorEffect);
        StopMoving();
        Dev_Logger.Debug($"Player state set to {state}!");
    }

    public void SetIsMelancholyPianoEffect()
    {
        SetState(Const_States_Player.MelancholyPiano);
        StopMoving();
        Dev_Logger.Debug($"Player state set to {state}!");
    }

    protected bool IsNotMovingState()
    {
        return State == Const_States_Player.Attack
                || State == Const_States_Player.Effect
                || State == Const_States_Player.Dialogue
                || State == Const_States_Player.Viewing
                || State == Const_States_Player.PickingUp
                || State == Const_States_Player.Standby
                || State == Const_States_Player.Inventory
                || State == Const_States_Player.Puppeteer
                || State == Const_States_Player.PuppeteerNull
                || State == Const_States_Player.LastElevatorEffect
                || State == Const_States_Player.MelancholyPiano
                || Script_Exits.Control.IsHandlingExit
                // Fix bug: walking into elevator with interact, then stutter stepping. Interactable may only change
                // game state which would allow movement handler to still animate and work. Collision freeze handling
                // only works if game is Interact state.
                || game.state != Const_States_Game.Interact;
    }

    public void StopMoving()
    {
        playerMovementHandler.StopMovingAnimations();
        playerMovementHandler.ClearInputBuffer();
    }

    public bool IsInputAxesMoving(float x, float y) => playerMovementHandler.IsInputAxesMoving(x, y);

    public void DefaultStickerState()
    {
        playerActionHandler.HandleDefaultStickerState();
    }

    // Called externally to make a certain mask active.
    public void ForceStickerSwitchBackground(int i)
    {
        playerActionHandler.HandleForceStickerSwitchBackground(i);
    }

    public void MyMaskEquipEffectTimeline()
    {
        playerActionHandler.MyMaskEquipEffectTimeline();
    }

    public void SetIceSpikeDepthMasksEnabled(bool isEnabled)
    {
        playerActionHandler.SetIceSpikeDepthMasksEnabled(isEnabled);
    }

    // ------------------------------------------------------------------
    // Combat
    public int FullHeal()
    {
        return GetComponent<Script_PlayerStats>().FullHeal();
    }

    public int Hurt(int dmg, Script_HitBox hitBox, Script_HitBoxBehavior hitBoxBehavior)
    {
        return GetComponent<Script_PlayerStats>().Hurt(dmg, hitBox, hitBoxBehavior);
    }
    
    // ------------------------------------------------------------------
    // Graphics & Animations
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
    }

    public void SetInvisible(bool isHide, float fadeTime = -1f)
    {
        _isInvisible = isHide;
        
        playerGraphics.SetHidden(_isInvisible);
    }

    public void SetBuffEffectActive(bool isActive)
    {
        playerEffect.SetBuffEffectActive(isActive);
    }

    public void AnimatorEffectTrigger()
    {
        MyAnimator.SetTrigger(IsEffectTrigger);

        Script_StickerEffectEventsManager.AnimatorEffectTrigger();
    }

    public void OnExitAnimations(Directions dir)
    {
        playerMovementHandler.OnExitAnimations(dir);
    }

    public void SyncAnimatorState(AnimatorStateInfo animatorStateInfo)
    {
        playerMovementHandler.SyncAnimatorState(animatorStateInfo);
    }

    public void SetAnimatorControllerPsychicDuck(bool isActive, bool isSFX = false)
    {
        if (isActive)
        {
            AnimatorStateInfo animatorStateInfo = MyAnimator.GetCurrentAnimatorStateInfo(Script_PlayerMovement.Layer);
            cachedAnimatorController = MyAnimator.runtimeAnimatorController;
            MyAnimator.runtimeAnimatorController = psychicDuckEffect.StickerAnimatorController;
            
            SyncAnimatorState(animatorStateInfo);
            MyAnimator.AnimatorSetDirection(FacingDirection);
        }
        else
        {
            AnimatorStateInfo animatorStateInfo = MyAnimator.GetCurrentAnimatorStateInfo(Script_PlayerMovement.Layer);
            MyAnimator.runtimeAnimatorController = cachedAnimatorController;
            
            SyncAnimatorState(animatorStateInfo);
            MyAnimator.AnimatorSetDirection(FacingDirection);
        }

        if (isSFX)
            SwitchMaskSFX();
    }

    // ------------------------------------------------------------------
    // Interactions

    public void SwitchLight(bool isOn)
    {
        if (playerLight != null)
            playerLight.enabled = isOn;
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
        Dev_Logger.Debug($"{name}: HandleEndItemDescriptionDialogue() itemShown: {playerActionHandler.itemShown}");
        playerActionHandler.HandleEndItemDescriptionDialogue(playerActionHandler.itemShown);
    }

    /// <summary>
    /// To be called externally from Script.
    /// </summary>
    public void HandlePuppeteerEffectHold()
    {
        playerActionHandler.PuppetMasterEffect(FacingDirection);
    }

    // ------------------------------------------------------------------
    // Public Utils & Timeline Signals
    
    /// <summary>
    /// used when player is controlled by Timeline
    /// </summary>
    public void UpdateLocation()
    {
        playerMovementHandler.UpdateLocation(location);
    }

    public void DropSFX()
    {
        playerEffect.DropSFX();
    }

    public void SwitchMaskSFX() => playerActionHandler.SwitchMaskSFX();

    public void Teleport(Vector3 newLocation)
    {
        transform.position = newLocation;
        UpdateLocation();
    }

    /// <summary>
    /// Called from Timeline Signal
    /// </summary>    
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
        Dev_Logger.Debug($"Player is force pushed dir {dir}");
        
        playerMovementHandler.Move(dir);
    }

    public Script_WorldTile GetCurrentWorldTile()
    {
        return playerMovementHandler.GetCurrentWorldTile(location);
    }

    public void ChangeMaterialFromTimeline(int materialIdx)
    {
        Script_PlayerGraphics.Materials material = (Script_PlayerGraphics.Materials)materialIdx;
        playerGraphics.ChangeMaterial(material);
    }

    // ------------------------------------------------------------------
    // Spawning

    public void InitializeOnLevel(
        Model_PlayerState playerState,
        Transform grid
    )
    {
        Vector3 spawnLocation = new Vector3(
            playerState.spawnX,
            playerState.spawnY,
            playerState.spawnZ
        );

        transform.position = spawnLocation;
        location = transform.position;
        FaceDirection(playerState.faceDirection);
        playerMovementHandler.InitializeOnLevel(grid);
        
        SwitchLight(IsLightOn);

        Dev_Logger.Debug($"Player initialized at position: {spawnLocation.x}, {spawnLocation.y}, {spawnLocation.z}");
    }
    
    public void Setup(
        Directions direction,
        Model_PlayerState playerState
    )
    {   
        game = Script_Game.Game;
        RewiredInput = ReInput.players.GetPlayer(PlayerId);

        directionsToVector = Script_Utils.GetDirectionToVectorDict();
        
        playerMovementHandler = GetComponent<Script_PlayerMovement>();
        playerActionHandler = GetComponent<Script_PlayerAction>();
        playerEffect = GetComponent<Script_PlayerEffect>();
        
        interactionBoxController = GetComponent<Script_InteractionBoxController>();
        playerStats = GetComponent<Script_PlayerStats>();
        
        playerMovementHandler.Setup(game);
        
        playerActionHandler.Setup(game);
        playerEffect.Setup();
        
        // Setup character stats
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

        if (GUILayout.Button("Set Emphasize Walk: True"))
        {
            player.IsEmphasizeWalk = true;
        }

        if (GUILayout.Button("Set Emphasize Walk: False"))
        {
            player.IsEmphasizeWalk = false;
        }

        if (GUILayout.Button("Default Sticker State"))
        {
            player.DefaultStickerState();
        }
    }
}
#endif
