using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Should always reference Input Manager inputs using these KeyCodes
/// </summary>
public static class Const_KeyCodes
{
    // ------------------------------------------------------------------
    // Player Map Actions

    // Ensure Rewired codes are different than KeyCodes to differentiate for now until we 
    // completely migrate to Rewired.
    public const string RWInteract = "Confirm";
    public const string RWMaskCommand = "Mask Command";
    public const string RWInventory = "Menu";
    public const string RWSpeed = "Run";
    public const string RWMask1 = "Mask 1";
    public const string RWMask2 = "Mask 2";
    public const string RWMask3 = "Mask 3";
    public const string RWMask4 = "Mask 4";
    public const string RWUnknownControllerMaskHz = "Mask Unknown Controller Horizontal";
    public const string RWUnknownControllerMaskVert = "Mask Unknown Controller Vertical";
    
    public const string RWHorizontal = "Move Horizontal";
    public const string RWVertical = "Move Vertical";
    public const string RWUIHorizontal = "UIHorizontal";
    public const string RWUIVertical = "UIVertical";
    
    public const string RWUICancel = "UICancel";
    public const string RWUISubmit = "UISubmit";
    public const string RWUnknownControllerSettings = "Unknown Controller Settings";

    public const string RWHotKey1 = "Hot Key 1";
    public const string RWHotKey2 = "Hot Key 2";
    public const string RWHotKey3 = "Hot Key 3";
    public const string RWHotKey4 = "Hot Key 4";
    public const string RWHotKey5 = "Hot Key 5";
    public const string RWHotKey6 = "Hot Key 6";
    public const string RWHotKey7 = "Hot Key 7";
    public const string RWHotKey8 = "Hot Key 8";
    public const string RWHotKey9 = "Hot Key 9";
    public const string RWBackspace = "Backspace";
    public const string RWKeyboardDownArrow = "Keyboard Down Arrow";
    public const string RWKeyboardSpace = "Keyboard Space";
    public const string RWKeyboardWasdS = "Keyboard WASD S";
    public const string RWKeyboardWasdA = "Keyboard WASD A";
    public const string RWKeyboardWasdD = "Keyboard WASD D";
    
    public static KeyCode KeycodeDownArrow = KeyCode.DownArrow;
    public static KeyCode KeycodeUpArrow = KeyCode.UpArrow;
    public static KeyCode KeycodeBackspace = KeyCode.Backspace;
    public static KeyCode KeycodeDelete = KeyCode.Delete;
    
    public const string InteractAction = "Interact";
    
    // ------------------------------------------------------------------
    // UI Map Actions
    public const string UISubmit = "Submit";
    public const string UICancel = "Cancel";

    // ------------------------------------------------------------------
    // Input System Maps
    public const string PlayerMap = "player";
    public const string UIMap = "UI";
    
    // ------------------------------------------------------------------
    // Dev & Trailer
    public const string Dev = "Dev";
    public const string DevIncrement = "DevIncrement";
    public const string DevDecrement = "DevDecrement";
    public const string Lights = "Lights";
    public const string Time = "Time";
    public const string Day = "Day";
    public const string UIVisibility = "UIVisibility";
    public const string PlayerVisibility = "PlayerVisibility";
    public const string SpecsDisplay = "SpecsDisplay";
    public const string TrailerCam = "TrailerCam";
}
