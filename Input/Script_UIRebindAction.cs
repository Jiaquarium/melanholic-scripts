using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;
using Rewired;

public class Script_UIRebindAction : MonoBehaviour
{
    private const string NoJoystickConnectedText = "No Joystick Connected";
    private const string UnsupportedJoystickText = "Unsupported Joystick";
    private const string NoControllerText = "--";

    public enum ErrorTypes
    {
        Move = 0,
        Taken = 1,
        Menu = 2,
        HotKey = 3
    }

    public enum InputTypes
    {
        Any = 0,
        Button = 1,
        Axis = 2,
    }

    [Header("Rewired Settings")]
    [SerializeField] private RWActions myRWAction;
    [Tooltip("The underlying action to be bound's axis range")]
    [SerializeField] private AxisRange actionAxisRange = AxisRange.Positive;
    [SerializeField] private InputTypes inputType = InputTypes.Any;
    
    [Header("Rebind Settings")]
    [SerializeField] private TextMeshProUGUI keyTextTMP;
    [SerializeField] private GameObject rebindingHighlight;

    [Header("Messaging")]
    [SerializeField] private TextMeshProUGUI detectTMP;
    [SerializeField] private TextMeshProUGUI errorMoveTMP;
    [SerializeField] private TextMeshProUGUI errorTakenTMP;
    [SerializeField] private TextMeshProUGUI errorMenuTMP;
    [SerializeField] private TextMeshProUGUI errorHotkeyTMP;

    [SerializeField] private Script_SettingsController settingsController;

    public int ActionId => GetRWActionName(myRWAction).RWActionNamesToId();
    public InputTypes MyInputType => inputType;
    public AxisRange MyActionAxisRange => actionAxisRange;
    public Player MyPlayer => settingsController.MyPlayer;

    void OnEnable()
    {
        MessagingInitialState();
    }

    void OnDisable()
    {
        MessagingInitialState();
    }

    // Update controls map with First keybind map. isDisplayMessage allows for first keybind to display the
    // error message to Player.
    public void UpdateBehaviorUIText(bool isDisplayMessage = false)
    {
        // If ControlsState:Keyboard state, will always be connected. Need this for ControlsState:Joystick
        if (!settingsController.IsControllerConnectedForState)
        {
            // Handle displaying a more specific message for joystick
            if (
                isDisplayMessage
                && settingsController.MyControlsState == Script_SettingsController.ControlsStates.Joystick
            )
            {
                bool IsJoystickConnectedNotSupported = MyPlayer.controllers.joystickCount > 0
                    && !MyPlayer.controllers.Joysticks[0].IsJoystickSupported();
                
                keyTextTMP.text = (IsJoystickConnectedNotSupported
                    ? UnsupportedJoystickText
                    : NoJoystickConnectedText).ToUpper();
            }
            else
                keyTextTMP.text = NoControllerText;

            return;
        }
        
        string myAction = GetRWActionName(myRWAction);
        AxisRange? axisRange = MyActionAxisRange == AxisRange.Full ? AxisRange.Full : null;
        
        string currentBindingInput = settingsController.MyControlsState switch
        {
            Script_SettingsController.ControlsStates.Joystick => settingsController.MyControllerMap.GetFirstMappingByMap(
                ActionId,
                axisRange: axisRange
            ),
            _ => MyPlayer.GetFirstMappingKeyboardByActionName(myAction),
        };
        
        Dev_Logger.Debug($"{name} currentBindingInput {currentBindingInput}");

        if (string.IsNullOrEmpty(currentBindingInput))
        {
            keyTextTMP.text = NoControllerText;
            return;
        }

        keyTextTMP.text = currentBindingInput.ToUpper();
    }

    private string GetRWActionName(RWActions RWAction) => RWAction switch
    {
        RWActions.MaskCommand => Const_KeyCodes.RWMaskCommand,
        RWActions.Inventory => Const_KeyCodes.RWInventory,
        RWActions.Speed => Const_KeyCodes.RWSpeed,
        RWActions.Mask1 => Const_KeyCodes.RWMask1,
        RWActions.Mask2 => Const_KeyCodes.RWMask2,
        RWActions.Mask3 => Const_KeyCodes.RWMask3,
        RWActions.Mask4 => Const_KeyCodes.RWMask4,
        RWActions.MoveHorizontalAxis => Const_KeyCodes.RWHorizontal,
        RWActions.MoveVerticalAxis => Const_KeyCodes.RWVertical,
        RWActions.Settings => Const_KeyCodes.RWUnknownControllerSettings,
        _ => Const_KeyCodes.RWInteract,
    };

    // ------------------------------------------------------------
    // Controls

    // ------------------------
    // Unity Events
    
    // Key Button OnClick
    public void StartRebindProcess()
    {
        if (!settingsController.IsControllerConnectedForState)
        {
            ErrorSFX();
            return;
        }
        
        MessagingInitialState();
        detectTMP.gameObject.SetActive(true);
        keyTextTMP.fontStyle = FontStyles.Underline;
        EnterSFX();
        
        rebindingHighlight.SetActive(true);
        
        // Start listening for input
        settingsController.OnInputFieldClicked(this);
    }
    // ------------------------
    
    public void DisplayError(ErrorTypes errorType)
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
            case (ErrorTypes.HotKey):
                errorHotkeyTMP.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
    
    public void RebindCompleted()
    {
        detectTMP.gameObject.SetActive(false);
        keyTextTMP.fontStyle = FontStyles.Normal;

        rebindingHighlight.SetActive(false);
        UpdateBehaviorUIText();
    }
    
    public void MessagingInitialState()
    {
        detectTMP.gameObject.SetActive(false);
        errorMoveTMP.gameObject.SetActive(false);
        errorTakenTMP.gameObject.SetActive(false);
        errorMenuTMP.gameObject.SetActive(false);
        errorHotkeyTMP.gameObject.SetActive(false);
    }

    private void EnterSFX()
    {
        Script_SFXManager.SFX.PlayEnterSubmenu();
    }

    private void ErrorSFX()
    {
        Script_SFXManager.SFX.PlayDullError();
    }
}
