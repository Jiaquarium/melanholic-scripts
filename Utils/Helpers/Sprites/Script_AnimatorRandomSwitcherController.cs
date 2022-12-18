using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_AnimatorRandomSwitcherController : MonoBehaviour
{
    [SerializeField] private bool isForceDefault;
    
    [SerializeField] private Vector2 interval;
    [SerializeField] private Vector2 duration;
    [SerializeField] private float startUpDelay;

    private float switchAnimatorTimer;
    private float defaultAnimatorTimer;
    private float startUpDelayTimer;
    
    private bool isSwitched;

    void OnEnable()
    {
        switchAnimatorTimer = GetRandomInterval();
        startUpDelayTimer = startUpDelay;
        isSwitched = false;
        Script_NPCEventsManager.OnNPCRandomAnimatorForceDefault += ForceDefault;
        Script_NPCEventsManager.OnNPCRandomAnimatorStopForceDefault += StopForceDefault;
    }

    void OnDisable()
    {
        Script_NPCEventsManager.OnNPCRandomAnimatorForceDefault -= ForceDefault;
        Script_NPCEventsManager.OnNPCRandomAnimatorStopForceDefault -= StopForceDefault;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isForceDefault)
            return;
        
        if (startUpDelayTimer > 0f)
        {
            startUpDelayTimer -= Time.deltaTime;
            return;
        }
        
        if (!isSwitched)
        {
            switchAnimatorTimer -= Time.deltaTime;
            
            if (switchAnimatorTimer <= 0f)
                HandleSwitch();
        }
        else
        {
            defaultAnimatorTimer -= Time.deltaTime;

            if (defaultAnimatorTimer <= 0f)
                HandleDefault();
        }
    }

    private void HandleSwitch()
    {
        switchAnimatorTimer = 0f;
        SwitchAnimator();
        defaultAnimatorTimer = GetRandomDuration();
        isSwitched = true;
    }

    private void HandleDefault()
    {
        defaultAnimatorTimer = 0f;
        DefaultAnimator();
        switchAnimatorTimer = GetRandomInterval();
        isSwitched = false;
    }
    
    private void SwitchAnimator()
    {
        Script_NPCEventsManager.NPCRandomAnimatorSwitch();
    }

    private void DefaultAnimator()
    {
        Script_NPCEventsManager.NPCRandomAnimatorDefault();
    }

    private void ForceDefault()
    {
        isForceDefault = true;
        HandleDefault();
    }

    private void StopForceDefault()
    {
        isForceDefault = false;
    }

    private float GetRandomInterval() => Random.Range(interval.x, interval.y);

    private float GetRandomDuration() => Random.Range(duration.x, duration.y);

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_AnimatorRandomSwitcherController))]
    public class Script_AnimatorRandomSwitcherControllerTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_AnimatorRandomSwitcherController t = (Script_AnimatorRandomSwitcherController)target;

            if (GUILayout.Button("Switch Animator"))
            {
                t.SwitchAnimator();
            }

            if (GUILayout.Button("Default Animator"))
            {
                t.DefaultAnimator();
            }
        }
    }
#endif
}
