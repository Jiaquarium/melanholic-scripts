using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

/// <summary>
/// Special Cases Notes:
/// - Special cases will override any other checks.
/// - Special cases are only checked once on the very first entrance.
/// - Special cases will NOT normally override the initial fade; after the special case, the initial fade occurs,
///   and only then do the default fade times occur.
/// - They will only occur once per play.
/// </summary>
public class Script_LevelCustomFadeBehavior : MonoBehaviour
{
    [SerializeField] private bool isSpecialCase;
    [Tooltip("Allow special cases check to always be called. DidCheckSpecialCaseFadeIn can be set by handlers.")]
    [SerializeField] private bool isAlwaysCheckSpecialCases;
    [Tooltip("Set to skip initial transition fade times for Level. Useful if Special Cases act as initial fades.")]
    [SerializeField] private bool isOptOutInitial;
    [Tooltip("Set to configure level of transition fade for Level")]
    [SerializeField] private Script_Exits.TransitionFades transitionFade;

    [Header("-------- Set by OnValidate --------")]

    [Tooltip("Set by OnValidate. If set to >0, sets the fade in behavior on (first) entrance for level")]
    [SerializeField] private float fadeInTimeInitial;
    [Tooltip("Set by OnValidate. If set to >0, sets the (first) time to wait in black before fading in level")]
    [SerializeField] private float waitInBlackTimeInitial;
    
    [Tooltip("Set by OnValidate. If set to >0, sets the fade in behavior for level")]
    [SerializeField] private float fadeInTime;
    [Tooltip("Set by OnValidate. If set to >0, sets the time to wait in black before fading in level")]
    [SerializeField] private float waitInBlackTime;
    
    [Header("-----------------------------------")][Space]
    
    [Tooltip("Specify to limit this behavior only when coming from these levels")]
    [SerializeField] private List<Script_LevelBehavior> behaviorsToFadeFrom;

    [SerializeField] private UnityEvent specialCaseFadeIn;
    [SerializeField] private UnityEvent specialCaseWaitInBlack;

    [SerializeField] private Script_Exits exitsManager;
    
    private bool isFadeInDone;
    private bool isWaitInBlackDone;

    private bool IsNotInitialFadeIn => isFadeInDone || isOptOutInitial;
    private bool IsNotInitialWaitInBlack => isWaitInBlackDone || isOptOutInitial;
    
    public bool IsSpecialCase => isSpecialCase;
    public bool IsOptOutInitial
    {
        set => isOptOutInitial = value;
    }
    
    public float FadeInTime => IsNotInitialFadeIn ? fadeInTime : fadeInTimeInitial;
    public float WaitInBlackTime => IsNotInitialWaitInBlack ? waitInBlackTime : waitInBlackTimeInitial;

    public float SpecialCaseFadeInTime { get; set; }
    public bool DidCheckSpecialCaseFadeIn { get; set; }
    public bool IsSpecialFadeIn { get; set; }
    public float SpecialCaseWaitInBlackTime { get; set; }
    public bool DidCheckSpecialCaseWaitInBlack { get; set; }
    public bool IsSpecialWaitInBlack { get; set; }
    
    void OnValidate()
    {
        switch (transitionFade)
        {
            case (Script_Exits.TransitionFades.XHeavy):
                fadeInTimeInitial = exitsManager.xHeavyTransitions.fadeInTimeInitial;
                waitInBlackTimeInitial = exitsManager.xHeavyTransitions.waitInBlackTimeInitial;
                fadeInTime = exitsManager.xHeavyTransitions.fadeInTime;
                waitInBlackTime = exitsManager.xHeavyTransitions.waitInBlackTime;
                break;
            case (Script_Exits.TransitionFades.Heavy):
                fadeInTimeInitial = exitsManager.heavyTransitions.fadeInTimeInitial;
                waitInBlackTimeInitial = exitsManager.heavyTransitions.waitInBlackTimeInitial;
                fadeInTime = exitsManager.heavyTransitions.fadeInTime;
                waitInBlackTime = exitsManager.heavyTransitions.waitInBlackTime;
                break;
            case (Script_Exits.TransitionFades.Medium):
                fadeInTimeInitial = exitsManager.medTransitions.fadeInTimeInitial;
                waitInBlackTimeInitial = exitsManager.medTransitions.waitInBlackTimeInitial;
                fadeInTime = exitsManager.medTransitions.fadeInTime;
                waitInBlackTime = exitsManager.medTransitions.waitInBlackTime;
                break;
            case (Script_Exits.TransitionFades.Light):
                fadeInTimeInitial = exitsManager.lightTransitions.fadeInTimeInitial;
                waitInBlackTimeInitial = exitsManager.lightTransitions.waitInBlackTimeInitial;
                fadeInTime = exitsManager.lightTransitions.fadeInTime;
                waitInBlackTime = exitsManager.lightTransitions.waitInBlackTime;
                break;
            default:
                fadeInTimeInitial = 0f;
                waitInBlackTimeInitial = 0f;
                fadeInTime = 0f;
                waitInBlackTime = 0f;
                break;
        }    
    }

    public float GetFadeInTime()
    {
        float t = IsNotInitialFadeIn ? fadeInTime : fadeInTimeInitial;
        isFadeInDone = true;
        return t;
    }
    
    public float GetWaitInBlackTime()
    {
        float t = IsNotInitialWaitInBlack ? waitInBlackTime : waitInBlackTimeInitial;
        isWaitInBlackDone = true;
        return t;
    }
    
    /// <summary>
    /// Use to declare specific fade behaviors for certain exits.
    /// </summary>
    /// <returns>True, if the exit came from specified Level</returns>
    public bool CheckLastBehavior(Script_LevelBehavior levelBehavior)
    {
        if (behaviorsToFadeFrom == null || behaviorsToFadeFrom.Count == 0)
            return true;
        
        Script_LevelBehavior found = behaviorsToFadeFrom.FirstOrDefault(
            lb => lb == levelBehavior
        );

        return found != null;
    }

    /// <summary>
    /// Can define a special case to override any other custom set time.
    /// Set the new time to the out variable.
    /// </summary>
    public bool InvokeSettersFadeInTime(out float time, out bool isSpecial)
    {
        // NOTE: specialCaseFadeIn event MUST SET the following:
        // - SpecialCaseFadeInTime
        // - IsSpecialFadeIn
        bool hasSpecialCaseSetter = specialCaseFadeIn.SafeInvoke();
        
        time = SpecialCaseFadeInTime;
        isSpecial = IsSpecialFadeIn;

        if (!isAlwaysCheckSpecialCases)
            DidCheckSpecialCaseFadeIn = true;
        
        return hasSpecialCaseSetter;
    }
    
    public bool InvokeSettersWaitInBlackTime(out float time, out bool isSpecial)
    {
        // NOTE: specialCaseWaitInBlack event MUST SET the following:
        // - SpecialCaseWaitInBlackTime
        // - IsSpecialWaitInBlack
        bool hasSpecialCaseSetter = specialCaseWaitInBlack.SafeInvoke();

        time = SpecialCaseWaitInBlackTime;
        isSpecial = IsSpecialWaitInBlack;

        if (!isAlwaysCheckSpecialCases)
            DidCheckSpecialCaseWaitInBlack = true;
        
        return hasSpecialCaseSetter;
    }
}
