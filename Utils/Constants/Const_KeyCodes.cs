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
    public const string RWHorizontal = "Move Horizontal";
    public const string RWVertical = "Move Vertical";
    public const string RWUICancel = "UICancel";
    public const string RWUISubmit = "UISubmit";
    public const string RWHotKey1 = "Hot Key 1";
    public const string RWHotKey2 = "Hot Key 2";
    public const string RWHotKey3 = "Hot Key 3";
    public const string RWHotKey4 = "Hot Key 4";
    public const string RWHotKey5 = "Hot Key 5";
    public const string RWHotKey6 = "Hot Key 6";
    public const string RWHotKey7 = "Hot Key 7";
    public const string RWHotKey8 = "Hot Key 8";
    public const string RWHotKey9 = "Hot Key 9";
    public static KeyCode KeyboardDownArrow = KeyCode.DownArrow;
    public static KeyCode KeyboardSpace = KeyCode.Space;

    
    public const string InteractAction = "Interact";
    public const string Inventory = "Inventory";
    public const string MaskEffect = "MaskEffect";
    public const string Speed = "Speed";
    public const string Action3 = "Action3";
    public const string Up = "Up";
    public const string Left = "Left";
    public const string Down = "Down";
    public const string Right = "Right";
    public const string Backspace = "Backspace";
    public const string Effect1 = "Effect1";
    public const string Effect2 = "Effect2";
    public const string Effect3 = "Effect3";
    public const string Effect4 = "Effect4";
    public const string Effect5 = "Effect5";
    public const string Effect6 = "Effect6";
    public const string Effect7 = "Effect7";
    public const string Effect8 = "Effect8";
    public const string Effect9 = "Effect9";
    public const string Horizontal = "Horizontal";
    public const string Vertical = "Vertical";
    
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
