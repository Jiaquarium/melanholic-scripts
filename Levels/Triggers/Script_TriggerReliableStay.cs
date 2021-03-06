using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Tag Detection Reliable Stay Trigger
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Script_TriggerReliableStay : Script_Trigger
{
    protected enum DetectTags
    {
        None            = 0,
        Everything      = 1,
        Puppet          = 2,
        Player          = 3,
    }

    [SerializeField] private bool isPressed;

    [SerializeField] protected List<DetectTags> detectTags;

    [SerializeField] private UnityEvent onTriggerEnterAction;
    [SerializeField] private UnityEvent onTriggerExitAction;

    public bool IsPressed
    {
        get => isPressed;
    }

    public bool SetIsPressed(bool _isPressed, bool isInitialState = false)
    {
        if (isPressed != _isPressed)
        {
            isPressed = _isPressed;

            var SFXOn = !isInitialState;
            if (isPressed)      HandleDownState(SFXOn);
            else                HandleUpState(SFXOn);
        }

        return isPressed;
    }

    void Start()
    {
        InitialState();
    }
    
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

            SetIsPressed(true);
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

            SetIsPressed(false);
        }
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

    protected virtual void OnEnter(Collider other) {}

    protected virtual void OnExit(Collider other) {}

    protected virtual void HandleUpState(bool SFXOn) {}

    protected virtual void HandleDownState(bool SFXOn) {}

    public virtual void InitialState()
    {
        SetIsPressed(false, isInitialState: true);
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Script_TriggerReliableStay))]
    public class Script_TriggerReliableStayTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_TriggerReliableStay t = (Script_TriggerReliableStay)target;
            if (GUILayout.Button("IsPressed = true"))
            {
                t.SetIsPressed(true);
            }

            if (GUILayout.Button("IsPressed = false"))
            {
                t.SetIsPressed(false);
            }
        }
    }
    #endif
}
