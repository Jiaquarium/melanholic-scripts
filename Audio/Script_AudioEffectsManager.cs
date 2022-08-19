using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For transitions e.g. fadeTimes
/// </summary>
public class Script_AudioEffectsManager : MonoBehaviour
{
    public const float fadeXFastTime    = 0.25f;
    public const float fadeFastTime     = 0.5f;
    public const float fadeMedTime      = 1.0f;
    public const float fadeSlowTime     = 1.5f;
    public const float fadeXSlowTime    = 2.0f;
    public const float fadeXXSlowTime   = 5.0f;

    public static float GetFadeTime(FadeSpeeds fadeSpeed) => fadeSpeed switch
    {
            FadeSpeeds.XFast => fadeXFastTime,
            FadeSpeeds.Fast => fadeFastTime,
            FadeSpeeds.Med => fadeMedTime,
            FadeSpeeds.Slow => fadeSlowTime,
            FadeSpeeds.XSlow => fadeXSlowTime,
            FadeSpeeds.XXSlow => fadeXXSlowTime,
            _ => 0f,
    };
}
