using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Changes game state to interact after moves are done
/// </summary>
public class Script_MovingNPC : Script_StaticNPC
{
    public static string LastMoveX  = "LastMoveX";
    public static string LastMoveZ  = "LastMoveZ";
    public static string MoveX      = "MoveX";
    public static string MoveZ      = "MoveZ";
    public static string NPCMoving  = "NPCMoving";

    private static string LocalStateInteract = "interact";
    private static string LocalStateMove = "move";

    public int MovingNPCId;
    public Vector3 startLocation;
    public Vector3 location;
    public float speed;
    public float progress;
    public string localState;
    public bool shouldExit = true;
    [SerializeField] private bool canUseMoveSets;
    public int moveSetIndex;
    [Tooltip("Maintain direction after talking with.")]
    [SerializeField] private bool defaultFacingDirectionDisabled;
    [Tooltip("Disable turning to Player when speaking.")]
    [SerializeField] private bool disableFacingPlayerOnDialogue;
    public Model_MoveSet[] moveSets = new Model_MoveSet[0];
    // use this to keep reference to model moveSets
    public Script_MovingNPCMoveSets movingNPCMoveSets;
    public AnimationCurve progressCurve;
    public Dictionary<Directions, Vector3> directionToVector;
    public Queue<Directions> currentMoves = new Queue<Directions>();
    public Queue<Directions[]> allMoves = new Queue<Directions[]>();

    [SerializeField] private bool isApproachingTarget; // bool used to know if current moves are result of appraoching
    [SerializeField] private Directions lastFacingDirection;
    [SerializeField] private PlayableDirector myDirector;
    [SerializeField] private float runSpeed;
    [SerializeField] private float walkSpeed;

    [Tooltip("Specify an adjuster if different facing Directions need to change position of Graphics")]
    [SerializeField] private Script_AnimatorPositionAdjuster positionAdjuster;

    [Tooltip("Options to adjust the AutoMove Timeline playback speed")]
    [SerializeField] private bool isSetAutoMoveTimelineSpeed;
    [SerializeField] private float autoMoveTimelineSpeed = 1f;
    [SerializeField] private AudioSource moveStartAudio;

    [SerializeField] private AudioClip moveSetStartSFXClip;

    private Animator animator;
    
    public bool DefaultFacingDirectionDisabled
    {
        get => defaultFacingDirectionDisabled;
        set => defaultFacingDirectionDisabled = value;
    }
    public PlayableDirector MyDirector { get => myDirector; }
    
    public Animator MyAnimator
    {
        get
        {
            if (animator == null)
                SetupAnimator();
            
            return animator;
        }
        set => animator = value;
    }
    
    public Script_MovingNPCMatchPlayer MovingNPCMatchPlayer { get => GetComponent<Script_MovingNPCMatchPlayer>(); }
    
    private Script_InteractionBoxController interactionBoxController { get; set; }

    protected override void OnEnable() {
        base.OnEnable();
    }
    
    // Update is called once per frame
    protected virtual void Update()
    {   
        if (canUseMoveSets && localState == LocalStateMove)
        {
            ActuallyMove();
        }

        if (myDirector != null)
            HandleTimelineAutoMove();
    }

    protected override void TriggerDialogue()
    {
        if (!disableFacingPlayerOnDialogue)     FacePlayer();
        base.TriggerDialogue();
    }

    /// <summary>
    /// if is ending the conversation, NPC should return to original
    ///  specified facing direction if any
    /// 
    /// </summary>
    public override bool? ContinueDialogue()
    {
        bool? didContinue = dialogueManager.ContinueDialogue();
        
        HandleReturnToDefaultDirection(didContinue);
        
        if (didContinue == false)
        {
            Debug.Log($"{this.name} returning my state to {States.Interact}");

            State = States.Interact;
        }
        
        return didContinue;
    }

    public void SetMovingNPCMatchPlayer(bool isActive)
    {
        MovingNPCMatchPlayer.enabled = isActive;
    }

    void HandleReturnToDefaultDirection(bool? didContinue)
    {
        if (
            dialogueManager.currentNode.data.disableReturnToDefaultFaceDir
            || disableFacingPlayerOnDialogue
            || defaultFacingDirectionDisabled
        )
        {
            return;
        }

        // Meaning it was a valid continuation but there are no more nodes
        if (didContinue == false && defaultFacingDirection != Directions.None)
        {
            FaceDefaultDirection();
            Debug.Log($"MovingNPC returning to default direction: {defaultFacingDirection}");
        }
    }

    /// <summary>
    /// For MovingNPCs controlled via Timeline, allows to pause their moves to speak with Player
    /// 
    /// Default behavior is to face the player.
    /// 
    /// NOTE: For not to face player working properly, must update the animator for facing state
    /// (Call public timeline faceDirection functions).
    /// </summary>
    private void HandleTimelineAutoMove()
    {
        if (isSetAutoMoveTimelineSpeed)
            MyDirector.playableGraph.GetRootPlayable(0).SetSpeed(autoMoveTimelineSpeed);

        
        if (State == States.Dialogue)
        {
            if (myDirector.playableGraph.IsPlaying())
            {
                myDirector.Pause();
                MyAnimator.SetBool(NPCMoving, false);
                
                if (!disableFacingPlayerOnDialogue)
                    FacePlayer();
            }
        }
        else if (State == States.Interact)
        {
            if (HandleBlocking(facingDirection))
            {
                myDirector.Pause();
                MyAnimator.SetBool(NPCMoving, false);
            }
            else if (!myDirector.playableGraph.IsPlaying())
            {
                myDirector.Play();
                MyAnimator.SetBool(NPCMoving, true);
            }
            else if (myDirector.playableGraph.IsPlaying())
                MyAnimator.SetBool(NPCMoving, true);
        }

        bool HandleBlocking(Directions dir)
        {
            List<Script_Interactable> interactables = interactionBoxController.GetInteractablesBlocking(dir);
            return interactables.Count > 0;
        }
    }

    void QueueUpAllMoves()
    {
        allMoves.Clear();
        
        foreach(Model_MoveSet moveSet in moveSets)
        {
            allMoves.Enqueue(moveSet.moves);
        }
    }

    void QueueUpCurrentMoves()
    {
        Directions[] moves = allMoves.Dequeue();
        currentMoves.Clear();

        foreach (Directions move in moves)
        {
            currentMoves.Enqueue(move);
        }
    }

    public void QueueMoves()
    {
        QueueUpAllMoves();
        QueueUpCurrentMoves();
    }

    public override void Move()
    {
        Directions desiredDir = currentMoves.Dequeue();
        Vector3 desiredVector = directionToVector[desiredDir];

        startLocation = location;
        location += desiredVector;
        localState = LocalStateMove;

        progress = 0f;

        AnimatorSetDirection(desiredDir);
        MyAnimator.SetBool(NPCMoving, true);
    }

    public void ActuallyMove()
    {
        progress += speed * Time.deltaTime;
        transform.position = Vector3.Lerp(
            startLocation,
            location,
            progressCurve.Evaluate(progress)
        );

        if (progress >= 1f)
        {
            progress = 1f;
            transform.position = location;
            
            if (currentMoves.Count == 0) {
                localState = LocalStateInteract;
                MyAnimator.SetBool(NPCMoving, false);
                
                NPCEndCommands endCommand = moveSets[moveSetIndex].NPCEndCommand;
                Directions endFaceDirection = moveSets[moveSetIndex].endFaceDirection;
                FaceDirection(endFaceDirection);
                
                game.CurrentMovesDoneAction();

                if (allMoves.Count == 0)
                {
                    game.AllMovesDoneAction(MovingNPCId);

                    if (isApproachingTarget)
                    {
                        isApproachingTarget = false;
                        // allow LB to handle this event
                        game.OnApproachedTarget(MovingNPCId);
                    }

                    if (endCommand == NPCEndCommands.Exit)
                    {
                        Exit();
                    }

                    Script_NPCEventsManager.NPCMoveSetsDone();
                    StopForceMoveSFX();
                    
                    return;
                }
                // TODO: this might need to happen before calling current moves done
                moveSetIndex++;
                QueueUpCurrentMoves();
                
                return;
            }
            Move();
        }
    }

    void Exit()
    {
        game.DestroyMovingNPC(MovingNPCId);
    }

    void AnimatorSetDirection(Directions dir)
    {
        interactionBoxController?.HandleActiveInteractionBox(dir);
        facingDirection = dir;
        
        float x = 0f, z = 0f;
        if (dir == Directions.Up) {
            x = 0f;
            z = 1f;
        }
        else if (dir == Directions.Down)
        {
            x = 0f;
            z = -1f;
        }
        else if (dir == Directions.Left)
        {
            x = -1f;
            z = 0f;
        }
        else if (dir == Directions.Right)
        {
            x = 1f;
            z = 0f;
        }
        
        MyAnimator.SetFloat(LastMoveX, x);
        MyAnimator.SetFloat(LastMoveZ, z);
        MyAnimator.SetFloat(MoveX, x);
        MyAnimator.SetFloat(MoveZ, z);

        if (positionAdjuster != null)
            positionAdjuster.Adjust(dir);
    }

    public void FaceDirection(Directions direction)
    {
        AnimatorSetDirection(direction);

        if (direction != Directions.None)
            lastFacingDirection = direction;
    }

    /// Face the last set direction by FaceDirection()
    public void FaceLastDirection()
    {
        FaceDirection(lastFacingDirection);
    }

    public void FaceDefaultDirection()
    {
        FaceDirection(defaultFacingDirection);
    }

    public void FacePlayer()
    {
        Directions directionToPlayer = transform.GetMyDirectionToTarget(game.GetPlayer().transform);
        Debug.Log($"{name} facing player in Direction: {directionToPlayer}");

        FaceDirection(directionToPlayer);
    }

    public void ChangeSpeed(float _speed)
    {
        speed = _speed;
    }

    public void ForceMove(Model_MoveSet _moveSet)
    {
        moveSets = new Model_MoveSet[]{ _moveSet };
        moveSetIndex = 0;
        
        QueueUpAllMoves();
        QueueUpCurrentMoves();

        if (_moveSet.moves.Length == 0)
        {
            // also face direction
            FaceDirection(_moveSet.endFaceDirection); 
            game.OnApproachedTarget(MovingNPCId);
        }
        else
        {
            isApproachingTarget = true;
            Move();

            PlayForceMoveStartSFX();
        }
    }

    public void ApproachTarget(
        Vector3 target,
        Vector3 adjustment,
        Directions endFaceDirection,
        NPCEndCommands endCommand
    )
    {
        UpdateLocation();
        Vector3 toMove = (target + adjustment) - location;
        Directions xMove;
        Directions zMove;
        Directions[] moves;

        int x = (int)Mathf.Round(toMove.x);
        int z = (int)Mathf.Round(toMove.z);
        int absX = Mathf.Abs(x);
        int absZ = Mathf.Abs(z);

        if (x < 0)  xMove = Directions.Left;
        else        xMove = Directions.Right;

        if (z < 0)  zMove = Directions.Down;
        else        zMove = Directions.Up;
        
        moves = new Directions[absX + absZ];

        for (int i = 0; i < moves.Length; i++)
        {
            if (i < absX)   moves[i] = xMove;
            else            moves[i] = zMove;   
        }

        ForceMove(new Model_MoveSet(
            moves,
            endFaceDirection,
            endCommand
        ));
    }

    /// <summary>
    /// used when NPC is controlled by Timeline
    /// </summary>
    public void UpdateLocation()
    {
        location = transform.position;
    }

    public void SetMoveSpeedRun()
    {
        speed = runSpeed;
    }
    public void SetMoveSpeedWalk()
    {
        speed = walkSpeed;
    }

    /// <summary> =================================================================================
    /// Timeline Signal Functions START
    /// </summary> ================================================================================
    public void FaceLeft()
    {
        FaceDirection(Directions.Left);
    }

    public void FaceRight()
    {
        FaceDirection(Directions.Right);
    }

    public void FaceUp()
    {
        FaceDirection(Directions.Up);
    }

    public void FaceDown()
    {
        FaceDirection(Directions.Down);
    }
    
    /// Timeline Signal Functions END =============================================================

    // At beginning of the force moves sequence (for dynamic moves)
    private void PlayForceMoveStartSFX()
    {
        if (moveStartAudio != null && moveSetStartSFXClip != null)
            moveStartAudio.PlayOneShot(moveSetStartSFXClip);
    }

    // At the end of force move sequence (in case the SFX is very long)
    private void StopForceMoveSFX()
    {
        if (moveStartAudio != null)
        {
            moveStartAudio.volume = 0f;
            moveStartAudio.Stop();
            moveStartAudio.volume = 1f;
        }
    }
    
    protected override void AutoSetup()
    {
        base.AutoSetup();
        game.AutoSetupMovingNPC(this);
        Setup();
    }

    void HandleSpawnFacingDirection()
    {
        if (MyAnimator != null)
        {
            if (lastFacingDirection == Directions.None)
            {
                FaceDefaultDirection();
            }
            else
            {
                FaceLastDirection();
            }
        }
    }

    private void SetupAnimator()
    {
        MyAnimator = rendererChild.GetComponent<Animator>();
    }
    
    public override void Setup()
    {
        directionToVector = Script_Utils.GetDirectionToVectorDict();
        interactionBoxController = GetComponent<Script_InteractionBoxController>();

        base.Setup();

        SetupAnimator();
        MyAnimator.SetBool(NPCMoving, false);
        
        progress = 1f;
        location = transform.position;

        // first queue up moves
        if (movingNPCMoveSets != null)  moveSets = movingNPCMoveSets.moveSets;
        if (moveSets.Length != 0)
        {
            QueueUpAllMoves();
            QueueUpCurrentMoves();
        }

        HandleSpawnFacingDirection();
    }
}
