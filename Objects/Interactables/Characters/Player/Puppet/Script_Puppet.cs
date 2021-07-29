using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected override void OnEnable()
    {
        base.OnEnable();
        
        Script_PlayerEventsManager.OnPuppeteerActivate      += OnPuppeteerActivate;
        Script_PlayerEventsManager.OnPuppeteerDeactivate    += OnPuppeteerDeactivate;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        Script_PlayerEventsManager.OnPuppeteerActivate      -= OnPuppeteerActivate;
        Script_PlayerEventsManager.OnPuppeteerDeactivate    -= OnPuppeteerDeactivate;
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

        if (game.state == Const_States_Game.Interact && puppetState == PuppetStates.Active)
        {
            if (IsNotMovingState())
            {
                StopMovingAnimations();

                // Finish up Ghost Follow on nonmoving states.
                playerMovementHandler.HandleGhostTransform(isForceTimerUpdate: true);
            }
            else
            {
                playerMovementHandler.HandleMoveInput(isReversed);
            }
        }
        else
        {
            StopMovingAnimations();
            playerMovementHandler.HandleGhostTransform(isForceTimerUpdate: true);
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
                playerMovementHandler.PlayerGhost.MyAnimator.runtimeAnimatorController = activatedAnimatorController;
                
                MyAnimator.AnimatorSetDirection(FacingDirection);
                playerMovementHandler.PlayerGhost.MyAnimator.AnimatorSetDirection(FacingDirection);
            }
        }
        else
        {
            MyAnimator.runtimeAnimatorController = inactiveAnimatorController;
            playerMovementHandler.PlayerGhost.MyAnimator.runtimeAnimatorController = inactiveAnimatorController;

            MyAnimator.AnimatorSetDirection(FacingDirection);
            playerMovementHandler.PlayerGhost.MyAnimator.AnimatorSetDirection(FacingDirection);
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
        }
    }

    protected virtual void OnPuppeteerDeactivate()
    {
        puppetState = PuppetStates.Inactive;
        SetAnimatorControllerActive(false);
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