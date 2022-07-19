using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class Script_UIRebindAction : MonoBehaviour
{
    private const string KeyboardEPath = "/Keyboard/e";
    private const string KeyboardEscPath = "/Keyboard/escape";
    private const string KeyboardEnterPath = "/Keyboard/enter";
    private const string KeyboardAnyKeyPath = "/Keyboard/anyKey";
    private const string Keyboard1Path = "/Keyboard/1";
    private const string Keyboard2Path = "/Keyboard/2";
    private const string Keyboard3Path = "/Keyboard/3";
    private const string Keyboard4Path = "/Keyboard/4";
    private const string Keyboard5Path = "/Keyboard/5";
    private const string Keyboard6Path = "/Keyboard/6";
    private const string Keyboard7Path = "/Keyboard/7";
    private const string Keyboard8Path = "/Keyboard/8";
    private const string Keyboard9Path = "/Keyboard/9";

    private const string UpPath = "*/UpArrow";
    private const string LeftPath = "*/LeftArrow";
    private const string DownPath = "*/DownArrow";
    private const string RightPath = "*/RightArrow";
    private const string WPath = "*/W";
    private const string APath = "*/A";
    private const string SPath = "*/S";
    private const string DPath = "*/D";

    private enum ErrorTypes
    {
        Move = 0,
        Taken = 1,
        Menu = 2,
        Other = 3
    }

    [Header("Rebind Settings")]
    [SerializeField] private string actionName;

    [SerializeField] private TextMeshProUGUI keyTextTMP;
    [SerializeField] private GameObject rebindingHighlight;

    [Header("Messaging")]
    [SerializeField] private TextMeshProUGUI detectTMP;
    [SerializeField] private TextMeshProUGUI errorMoveTMP;
    [SerializeField] private TextMeshProUGUI errorTakenTMP;
    [SerializeField] private TextMeshProUGUI errorMenuTMP;
    [SerializeField] private TextMeshProUGUI errorOtherTMP;
    
    private InputAction inputAction;
    private InputActionRebindingExtensions.RebindingOperation rebindOperation;
    private ErrorTypes errorType;

    void OnEnable()
    {
        MessagingInitialState();
    }

    void OnDisable()
    {
        MessagingInitialState();
    }
    
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
        bool isError = false;
        
        Script_SettingsController.Instance.OnStartRebindProcess();
        MessagingInitialState();
        detectTMP.gameObject.SetActive(true);
        keyTextTMP.fontStyle = FontStyles.Underline;
        EnterSFX();
        
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
            InputControl candidate = operation.selectedControl;
            
            bool isTaken = false;
            foreach (var action in takenActions)
            {
                if (InputControlPath.Matches(candidate.path, action.controls[0]))
                {
                    // Unity Bug: If input was escape or enter, having a control set as E would match.
                    // Ensure, we don't match here, since this should be a menu error, not a taken error.
                    if (
                        (candidate.path == KeyboardEscPath || candidate.path == KeyboardEnterPath)
                        && action.controls[0].path == KeyboardEPath
                    )
                        continue;
                    
                    // Ignore if overwriting with same value
                    if (candidate.path == inputAction.controls[0].path)
                        continue;
                    
                    Debug.Log($"Matched Exclusion: candidate.path {candidate.path} action.controls[0] {action.controls[0]}");
                    isTaken = true;
                }
            }
            
            if (
                isTaken
                || InputControlPath.Matches(Keyboard1Path, candidate)
                || InputControlPath.Matches(Keyboard2Path, candidate)
                || InputControlPath.Matches(Keyboard3Path, candidate)
                || InputControlPath.Matches(Keyboard4Path, candidate)
                || InputControlPath.Matches(Keyboard5Path, candidate)
                || InputControlPath.Matches(Keyboard6Path, candidate)
                || InputControlPath.Matches(Keyboard7Path, candidate)
                || InputControlPath.Matches(Keyboard8Path, candidate)
                || InputControlPath.Matches(Keyboard9Path, candidate)
            )
            {
                errorType = ErrorTypes.Taken;
                isError = true;
            }
            // Prevent overriding Move Keys
            else if (
                InputControlPath.Matches(WPath, candidate)
                || InputControlPath.Matches(APath, candidate)
                || InputControlPath.Matches(SPath, candidate)
                || InputControlPath.Matches(DPath, candidate)
                || InputControlPath.Matches(UpPath, candidate)
                || InputControlPath.Matches(LeftPath, candidate)
                || InputControlPath.Matches(DownPath, candidate)
                || InputControlPath.Matches(RightPath, candidate)
            )
            {
                errorType = ErrorTypes.Move;
                isError = true;
            }
            // Reserved for UI/Cancel, Player/Interact & UI/Submit
            else if (
                (
                    InputControlPath.Matches(KeyboardEscPath, candidate)
                    || InputControlPath.Matches(KeyboardEnterPath, candidate)
                )
                // Note: Fixes current Unity Bug where escape and enter will match with e.
                && candidate.path != KeyboardEPath
            )
            {
                errorType = ErrorTypes.Menu;
                isError = true;
            }
            // Not allowed
            else if (InputControlPath.Matches(KeyboardAnyKeyPath, candidate))
            {
                errorType = ErrorTypes.Other;
                isError = true;
            }

            if (isError)
                operation.Cancel();
        }

        void RebindCompleted()
        {
            detectTMP.gameObject.SetActive(false);
            keyTextTMP.fontStyle = FontStyles.Normal;

            if (isError)
            {
                ErrorSFX();
                
                // Set proper error message
                switch (errorType)
                {
                    case (ErrorTypes.Move):
                        errorMoveTMP.gameObject.SetActive(true);
                        break;
                    case (ErrorTypes.Taken):
                        errorTakenTMP.gameObject.SetActive(true);
                        break;
                    case (ErrorTypes.Menu):
                        errorMenuTMP.gameObject.SetActive(true);
                        break;
                    case (ErrorTypes.Other):
                        errorOtherTMP.gameObject.SetActive(true);
                        break;
                }
            }
            else
            {
                SuccessSFX();
            }
            
            rebindOperation.Dispose();
            rebindOperation = null;

            UpdateBehavior();
            inputAction.Enable();
            
            Script_PlayerInputManager.Instance.Save();
            
            rebindingHighlight.SetActive(false);

            Script_SettingsController.Instance.OnDoneRebindProcess();
        }
    }
    
    private void SetupInputAction()
    {
        inputAction = Script_PlayerInputManager.Instance.MyPlayerInput.actions
            .FindActionMap(Const_KeyCodes.PlayerMap).FindAction(actionName);
    }

    public void MessagingInitialState()
    {
        detectTMP.gameObject.SetActive(false);
        errorMoveTMP.gameObject.SetActive(false);
        errorTakenTMP.gameObject.SetActive(false);
        errorMenuTMP.gameObject.SetActive(false);
        errorOtherTMP.gameObject.SetActive(false);
    }

    /// <summary>
    // Note: Do not call every frame, ToHumanReadableString's performance is very slow.
    /// </summary>
    private void UpdateBindingDisplayUI()
    {
        // Note: only returns the first binding control path.
        // https://github.com/UnityTechnologies/InputSystem_Warriors/blob/056e74b1701f3bd2f218a34a46d6d2cc1e167a78/InputSystem_Warriors_Project/Assets/Scripts/Behaviours/UI/UIRebindActionBehaviour.cs#L116
        int controlBindingIndex = inputAction.GetBindingIndexForControl(inputAction.controls[0]);
        string currentBindingInput = InputControlPath.ToHumanReadableString(
            inputAction.bindings[controlBindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice
        );

        keyTextTMP.text = currentBindingInput.ToUpper();
    }

    private void EnterSFX()
    {
        Script_SFXManager.SFX.PlayEnterSubmenu();
    }

    private void ErrorSFX()
    {
        Script_SFXManager.SFX.PlayDullError();
    }

    private void SuccessSFX()
    {
        Script_SFXManager.SFX.PlayUISuccessEdit();
    }
}
