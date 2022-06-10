using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class Script_UIRebindAction : MonoBehaviour
{
    [Header("Rebind Settings")]
    [SerializeField] private string actionName;

    [SerializeField] private TextMeshProUGUI TMP;
    [SerializeField] private GameObject rebindingHighlight;
    
    private InputAction inputAction;
    private InputActionRebindingExtensions.RebindingOperation rebindOperation;

    public void UpdateBehavior()
    {
        if (String.IsNullOrEmpty(actionName))
            return;
        
        SetupInputAction();
        UpdateBindingDisplayUI();
    }

    // ------------------------------------------------------------
    // Controls
    
    // Key Button OnClick
    public void StartRebindProcess()
    {
        // TBD Disable Event System
        
        rebindingHighlight.SetActive(true);
        
        inputAction.Disable();

        List<InputAction> takenActions = Script_PlayerInputManager.Instance.GetRebindableActions();
        
        // Will only rebind the first binding if there are multiple.
        rebindOperation = inputAction.PerformInteractiveRebinding(0)
            .WithControlsExcluding("<Mouse>")
            .OnMatchWaitForAnother(0.1f)
            .OnPotentialMatch(_ => FilterCandidates(_))
            .OnComplete(_ => RebindCompleted())
            .OnCancel(_ => RebindCompleted());

        rebindOperation.Start();

        void FilterCandidates(InputActionRebindingExtensions.RebindingOperation operation)
        {
            // Check if potential match is currently in use as a control.
            var candidate = operation.selectedControl;
            bool isTaken = false;
            takenActions.ForEach(action => {
                if (InputControlPath.Matches(candidate.path, action.controls[0]))
                {
                    Debug.Log($"Matched Exclusion: candidate.path {candidate.path} action.controls[0] {action.controls[0]}");
                    isTaken = true;
                }
            });

            if (
                isTaken
                || candidate.path == "/Keyboard/escape" // Reserved for UI/Cancel
                || candidate.path == "/Keyboard/enter" // Reserved for Player/Interact & UI/Submit
                || candidate.path == "/Keyboard/anyKey" // Not allowed
            )
            {
                operation.Cancel();
            }
        }

        void RebindCompleted()
        {
            rebindOperation.Dispose();
            rebindOperation = null;

            UpdateBehavior();
            inputAction.Enable();
            
            Script_PlayerInputManager.Instance.Save();
            
            rebindingHighlight.SetActive(false);

            // TBD Reenable Event System
        }
    }
    
    private void SetupInputAction()
    {
        inputAction = Script_PlayerInputManager.Instance.MyPlayerInput.actions
            .FindActionMap(Const_KeyCodes.PlayerMap).FindAction(actionName);
    }
    
    private void UpdateBindingDisplayUI()
    {
        // Note: only allows for single binding per action.
        int controlBindingIndex = inputAction.GetBindingIndexForControl(inputAction.controls[0]);
        string currentBindingInput = InputControlPath.ToHumanReadableString(
            inputAction.bindings[controlBindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice
        );

        TMP.text = currentBindingInput;
    }
}
