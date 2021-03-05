using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

/// <summary>
/// Tag Detection Reliable Stay Trigger
/// </summary>
public class Script_TriggerReliableStay : Script_Trigger
{
    protected enum DetectTags
    {
        None            = 0,
        Everything      = 1,
        Puppet          = 2,
        Player          = 3,
    }

    [SerializeField] protected List<DetectTags> detectTags;

    [SerializeField] private UnityEvent onTriggerEnterAction;
    [SerializeField] private UnityEvent onTriggerExitAction;
    
    void OnTriggerEnter(Collider other)
    {
        Script_ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);
        
        // Results in DetectTags.None if none is found.
        bool isDetectEverything = detectTags.FindIndex(tag => tag == DetectTags.Everything) != -1;
        DetectTags result = detectTags.FirstOrDefault(tag => HandleDetectionTag(tag, other.tag));

        if (isDetectEverything || !result.Equals(default(DetectTags)))
        {   
            if (onTriggerEnterAction.CheckUnityEventAction()) onTriggerEnterAction.Invoke();
            OnEnter(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Script_ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);
        
        // Results in DetectTags.None if none is found.
        bool isDetectEverything = detectTags.FindIndex(tag => tag == DetectTags.Everything) != -1;
        DetectTags result = detectTags.FirstOrDefault(tag => HandleDetectionTag(tag, other.tag));
        
        if (isDetectEverything || !result.Equals(default(DetectTags)))
        {   
            if (onTriggerExitAction.CheckUnityEventAction()) onTriggerExitAction.Invoke();
            OnExit(other);
        }
    }

    protected virtual void OnEnter(Collider other)
    {

    }

    protected virtual void OnExit(Collider other)
    {
        
    }

    protected bool HandleDetectionTag(DetectTags tag, string otherTag)
    {
        string tagToCompare;
        
        switch (tag)
        {
            case (DetectTags.Puppet):
                tagToCompare = Const_Tags.Puppet;
                break;
            case (DetectTags.Player):
                tagToCompare = Const_Tags.Player;
                break;
            case (DetectTags.None):
                return false;
            case (DetectTags.Everything):
                return true;
            default:
                Debug.LogError($"{name} You are missing handling tag {tag}");
                return false;
        }

        return otherTag == tagToCompare;
    }
}
