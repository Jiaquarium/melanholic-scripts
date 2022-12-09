using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_AnimatorRandomSwitcherController : MonoBehaviour
{
    [SerializeField] private bool isManualStart;
    
    [SerializeField] private Vector2 interval;
    [SerializeField] private Vector2 duration;
    [SerializeField] private float startUpDelay;

    private float intervalTimer;
    private float durationTimer;
    private float startUpDelayTimer;
    
    private bool isSwitched;

    void OnEnable()
    {
        intervalTimer = GetRandomInterval();
        startUpDelayTimer = startUpDelay;
        isSwitched = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (startUpDelayTimer > 0f || isManualStart)
        {
            startUpDelayTimer -= Time.deltaTime;
            return;
        }
        
        if (!isSwitched)
        {
            intervalTimer -= Time.deltaTime;
            
            if (intervalTimer <= 0f)
                HandleStartDuration();
        }
        else
        {
            durationTimer -= Time.deltaTime;

            if (durationTimer <= 0f)
                HandleStartInterval();
        }
    }

    public void StartSwitching()
    {
        isManualStart = false;
        startUpDelayTimer = 0f;

        HandleStartDuration();
    }

    private void HandleStartDuration()
    {
        intervalTimer = 0f;
        SwitchAnimator();
        durationTimer = GetRandomDuration();
        isSwitched = true;
    }

    private void HandleStartInterval()
    {
        durationTimer = 0f;
        DefaultAnimator();
        intervalTimer = GetRandomInterval();
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
