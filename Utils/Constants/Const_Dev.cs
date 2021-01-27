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
    
    // Use for dev and dev prod.
    public readonly static bool IsDevMode       = false;
    public readonly static bool IsPersisting    = true;
    public readonly static bool IsPGVersion     = false;
    
    // Use for actual prod builds.
    public readonly static bool IsProd          = false;
}
