using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Script_TriggerReliableStay : MonoBehaviour
{
    private enum DetectTags
    {
        None        = 0,
        Puppet      = 1,
        Player      = 2,
    }
    
    [SerializeField] private List<DetectTags> detectTags;

    [SerializeField] private UnityEvent onTriggerEnterAction;
    [SerializeField] private UnityEvent onTriggerExitAction;
    
    void OnTriggerEnter(Collider other)
    {
        Script_ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);
        
        // Results in DetectTags.None if none is found.
        var result = detectTags.FirstOrDefault(tag => HandleDetectionTag(tag) == other.tag);

        if (!result.Equals(default(DetectTags)))
        {   
            if (onTriggerEnterAction.CheckUnityEventAction()) onTriggerEnterAction.Invoke();
        }
    }

    void OnTriggerExit(Collider other)
    {
        Script_ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);
        
        // Results in DetectTags.None if none is found.
        var result = detectTags.FirstOrDefault(tag => HandleDetectionTag(tag) == other.tag);
        
        if (!result.Equals(default(DetectTags)))
        {   
            if (onTriggerExitAction.CheckUnityEventAction()) onTriggerExitAction.Invoke();
        }
    }

    private string HandleDetectionTag(DetectTags tag)
    {
        switch (tag)
        {
            case (DetectTags.Puppet):
                return Const_Tags.Puppet;
            case (DetectTags.Player):
                return Const_Tags.Player;
            case (DetectTags.None):
                return null;
            default:
                Debug.LogError($"{name} You are missing handling tag {tag}");
                return Const_Tags.Player;
        }
    }
}
