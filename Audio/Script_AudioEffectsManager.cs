using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For transitions e.g. fadeTimes
/// </summary>
public class Script_AudioEffectsManager : MonoBehaviour
{
    public const float fadeFastTime     = 0.5f;
    public const float fadeMedTime      = 1.0f;
    public const float fadeSlowTime     = 1.5f;
    public const float fadeXSlowTime    = 2.0f;

    public static float GetFadeTime(FadeSpeeds fadeSpeed)
    {
        float fadeTime;
        switch (fadeSpeed)
        {
            case FadeSpeeds.Fast:
                fadeTime = fadeFastTime;
                break;
            case FadeSpeeds.Med:
                fadeTime = fadeMedTime;
                break;
            case FadeSpeeds.Slow:
                fadeTime = fadeSlowTime;
                break;
            default:
                fadeTime = 0f;
                break;
        }
        return fadeTime;
    }
}
