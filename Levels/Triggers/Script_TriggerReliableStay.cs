using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Script_TriggerReliableStay : MonoBehaviour
{
    private enum DetectTags
    {
        Puppet      = 0,
        Player      = 1,
    }
    
    [SerializeField] private List<DetectTags> detectTags;

    [SerializeField] private UnityEvent onTriggerEnterAction;
    [SerializeField] private UnityEvent onTriggerExitAction;
    
    void OnTriggerEnter(Collider other)
    {
        Script_ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);
        
        DetectTags? result = detectTags.Find(tag => HandleDetectionTag(tag) == other.tag);

        Debug.Log($"OnTriggerEnter result {result}");
        
        if (result != null)
        {   
            if (onTriggerEnterAction.CheckUnityEventAction()) onTriggerEnterAction.Invoke();
        }
    }

    void OnTriggerExit(Collider other)
    {
        Script_ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);
        
        DetectTags? result = detectTags.Find(tag => HandleDetectionTag(tag) == other.tag);
        
        Debug.Log($"OnTriggerExit result {result}");
        
        if (result != null)
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
            default:
                Debug.LogError($"{name} You are missing handling tag {tag}");
                return Const_Tags.Player;
        }
    }
}
