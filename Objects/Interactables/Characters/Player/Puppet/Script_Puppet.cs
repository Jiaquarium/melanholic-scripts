using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_Puppet : Script_PlayerCopy
{
    protected enum PuppetStates
    {
        Inactive        = 0,
        Active          = 1
    }

    [SerializeField] protected PuppetStates puppetState;
    [SerializeField] protected bool isReversed;

    // The new animator controller to swap when the puppet is active.
    [SerializeField] private RuntimeAnimatorController activatedAnimatorController;
    
    [SerializeField] private RuntimeAnimatorController inactiveAnimatorController;

    [SerializeField] private UnityEvent onPuppeteerActivate;
    [SerializeField] private UnityEvent onPuppeteerDeactivate;

    protected override void OnEnable()
    {
        base.OnEnable();
        
        Script_PlayerEventsManager.OnPuppeteerActivate          += OnPuppeteerActivate;
        Script_PlayerEventsManager.OnPuppeteerDeactivate        += OnPuppeteerDeactivate;
        Script_GameEventsManager.OnLevelBeforeDestroy           += OnPuppeteerDeactivate;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        Script_PlayerEventsManager.OnPuppeteerActivate          -= OnPuppeteerActivate;
        Script_PlayerEventsManager.OnPuppeteerDeactivate        -= OnPuppeteerDeactivate;
        Script_GameEventsManager.OnLevelBeforeDestroy           -= OnPuppeteerDeactivate;
    }

    protected override void Awake()
    {
        base.Awake();

        puppetState = PuppetStates.Inactive;
    }

    void Start()
    {
        Debug.Log($"My animator: {MyAnimator}");
        inactiveAnimatorController = MyAnimator.runtimeAnimatorController as RuntimeAnimatorController;
        
        SetAnimatorControllerActive(false);
    }
    
    protected override void Update()
    {
        // ------------------------------------------------------------------
        // Visuals
        HandleIsMoving();
        // ------------------------------------------------------------------

        HandleAction(
            // Puppets do not take action inputs
            null,
            () => playerMovementHandler.HandleMoveInput(isReversed),
            StopMoving
        );
    }

    protected override void FixedUpdate()
    {
        playerMovementHandler.HandleMoveTransform(isReversed);
    }

    public override void HandleAction(
        Action actionHandler,
        Action moveHandler,
        Action stopMovingHandler    
    )
    {
        if (game.state == Const_States_Game.Interact && puppetState == PuppetStates.Active)
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
            if (stopMovingHandler != null)
                stopMovingHandler();
        }        
    }

    // Swaps the animator controller for specified one when active as a puppet.
    public void SetAnimatorControllerActive(bool isActive)
    {
        if (isActive)
        {
            if (activatedAnimatorController != null)
            {
                MyAnimator.runtimeAnimatorController = activatedAnimatorController;
                
                MyAnimator.AnimatorSetDirection(FacingDirection);
            }
        }
        else
        {
            MyAnimator.runtimeAnimatorController = inactiveAnimatorController;

            MyAnimator.AnimatorSetDirection(FacingDirection);
        }
    }

    protected virtual void OnPuppeteerActivate()
    {
        // Wait to set active on next frame, so we don't take in the input for the current frame
        // (e.g. the input for switching to Puppeteer mode) and call PlayerStickerEffect() on same frame.
        StartCoroutine(WaitNextFrameSwitchState());
        
        IEnumerator WaitNextFrameSwitchState()
        {
            yield return null;
            
            puppetState = PuppetStates.Active;
            
            SetAnimatorControllerActive(true);

            onPuppeteerActivate.SafeInvoke();
        }
    }

    protected virtual void OnPuppeteerDeactivate()
    {
        puppetState = PuppetStates.Inactive;
        SetAnimatorControllerActive(false);

        onPuppeteerDeactivate.SafeInvoke();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_Puppet))]
public class Script_PuppetTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_Puppet t = (Script_Puppet)target;
        if (GUILayout.Button("Switch to Active Animator"))
        {
            t.SetAnimatorControllerActive(true);
        }
        if (GUILayout.Button("Switch to Inactive Animator"))
        {
            t.SetAnimatorControllerActive(false);
        }
    }
}
#endif