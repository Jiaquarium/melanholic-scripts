using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Script_LevelCustomFadeBehavior : MonoBehaviour
{
    [Tooltip("If set to >0, sets the fade in behavior on (first) entrance for level")]
    [SerializeField] private float fadeInTimeInitial;
    [Tooltip("If set to >0, sets the (first) time to wait in black before fading in level")]
    [SerializeField] private float waitInBlackTimeInitial;
    
    [Tooltip("If set to >0, sets the fade in behavior for level")]
    [SerializeField] private float fadeInTime;
    [Tooltip("If set to >0, sets the time to wait in black before fading in level")]
    [SerializeField] private float waitInBlackTime;
    
    [Tooltip("Specify to limit this behavior only when coming from these levels")]
    [SerializeField] private List<Script_LevelBehavior> behaviorsToFadeFrom;
    
    private bool isFadeInDone;
    private bool isWaitInBlackDone;
    
    public float FadeInTime => isFadeInDone ? fadeInTime : fadeInTimeInitial;
    public float WaitInBlackTime => isWaitInBlackDone ? waitInBlackTime : waitInBlackTimeInitial;
    
    public float GetFadeInTime()
    {
        float t = isFadeInDone ? fadeInTime : fadeInTimeInitial;
        isFadeInDone = true;
        return t;
    }
    
    public float GetWaitInBlackTime()
    {
        float t = isWaitInBlackDone ? waitInBlackTime : waitInBlackTimeInitial;
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
}
