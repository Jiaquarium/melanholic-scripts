using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Const_Dev
{
    /// <summary>
    /// Allows you to do prod in Unity editor
    /// Use pattern if
    ///     if (Debug.IsDebugMode && Const_Dev.IsDevMode)       { do dev tasks; } OR
    ///     if (!Debug.isDebugBuild || !Const_Dev.IsDevMode)    { do prod tasks; }
    /// </summary>
    
                                                                    // Settings for Release
    public readonly static bool GiveItems                   = false;            // false
    
    // False for "dev-prod".
    public readonly static bool IsDevMode                   = false;            // false
    
    public readonly static bool IsPersisting                = true;             // true
    
    public readonly static bool IsPGVersion                 = false;            // false
    
    // Toggle to turn off "time damage" to Player.
    public readonly static bool IsNoTimeHurt                = false;            // false
    
    // Toggle to force camera guides on in prod.
    public readonly static bool IsCamGuides                 = false;            // false

    // Toggle to force Endings
    public readonly static bool IsTrueEnding                = false;            // false
    public readonly static bool IsGoodEnding                = false;            // false
    
    // True for release builds.
    // Forces spawn in hotel.
    public readonly static bool IsProd                      = true;             // true
    
    public readonly static bool IsLoggerAvailable           = true;            // false
    
    public readonly static string Lang                      = "EN";             // "EN"

}
