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
    [SerializeField] private bool isPressed;

    [SerializeField] protected List<Const_Tags.Tags> detectTags;

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

    protected override void Start()
    {
        base.Start();
        
        InitialState();
    }
    
    void OnTriggerEnter(Collider other)
    {
        Script_ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);
        
        Debug.Log($"{name} OnTriggerEnter other <{other}> tag <{other.tag}>");

        if (detectTags.CheckInTags(other.tag))
        {   
            if (onTriggerEnterAction.CheckUnityEventAction())
            {
                Debug.Log($"Invoking onTriggerAction");
                onTriggerEnterAction.Invoke();
            }
            
            OnEnter(other);

            SetIsPressed(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Script_ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);

        Debug.Log($"{name} OnTriggerExit other <{other}> tag <{other.tag}>");
        
        if (detectTags.CheckInTags(other.tag))
        {   
            if (onTriggerExitAction.CheckUnityEventAction())
            {
                Debug.Log($"Invoking onTriggerAction");
                onTriggerExitAction.Invoke();
            }
            
            OnExit(other);

            SetIsPressed(false);
        }
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
