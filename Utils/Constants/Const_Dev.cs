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
    
    // Forces dev custom spawn defined in inspector.
    public readonly static bool IsDevSpawn                  = false;            // false
    
    public readonly static string Lang                      = "EN";             // "EN"

    public readonly static bool IsDemo                      = false;             // false

    // For copies distributed directly, turn this false, Steam should not be required to boot up.  
    public readonly static bool IsForceSteamRestart         = false;             // false

    // ------------------------------------------------------------------
    // Extra Dev Flags

    public readonly static bool IsTrailerMode               = false;            // false
    public readonly static bool IsSpecsDisplayOn            = false;            // false
    public readonly static bool IsDevHelperOn               = false;            // false

    public readonly static bool IsClockDebug                = false;            // false
    
    // Use when taking screenshots
    public readonly static bool IsClockShowColonsAlways     = false;            // false

    // To force World Painting entrances to always show special intros regardless of face off counter
    public readonly static bool IsSpecialIntroOnEntrances   = false;            // false              
}
