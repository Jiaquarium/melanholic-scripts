using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LightFXManager : MonoBehaviour
{
    const float DefaultIntensity            = 0.05f;
    const float DefaultEndingIntensity      = 0.2f;
    const float AwareEndingIntensity        = 1f;
    const float WarningEndingIntensity      = 5f;
    const float DangerEndingIntensity       = 20f;
    
    [SerializeField] private List<Light> directionalLights;
    [SerializeField] private Script_ClockManager clockManager;

    [SerializeField] private float intensityDelta;
    [SerializeField] private float totalIntervalTime;

    [Tooltip("Set to buffer intensity changes so we're not doing this every frame")]
    // Also gives an interesting old school affect when getting down to Danger Time.
    [SerializeField] private float bufferTime;

    private Script_Clock.TimeStates lastState;
    private float lastFrame;
    private float timer;

    void Start()
    {
        SetDefaultIntensity();    
    }
    
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            UpdateDirectionalLights();
            
            lastFrame = Time.time;
            timer = bufferTime;
        }
    }

    private void UpdateDirectionalLights()
    {
        bool isNewInterval = clockManager.ClockTimeState != lastState;
        if (isNewInterval)  lastState = clockManager.ClockTimeState;
        
        foreach (Light l in directionalLights)
        {
            if (l.type != LightType.Directional)    return;

            IncreaseIntensity(l);

            if (isNewInterval)  UpdateTimeInterval(l);
        }

        void IncreaseIntensity(Light l)
        {
            float timeElapsed   = Time.time - lastFrame;
            float t             = (timeElapsed * Script_Clock.TimeMultiplier) / totalIntervalTime;
            float increment     = intensityDelta * t;

            l.intensity         = Mathf.Clamp(l.intensity + increment, DefaultIntensity, DangerEndingIntensity);
        }

        void UpdateTimeInterval(Light l)
        {
            switch (clockManager.ClockTimeState)
            {
                case (Script_Clock.TimeStates.Aware):
                    intensityDelta      = AwareEndingIntensity - DefaultEndingIntensity;
                    totalIntervalTime   = Script_Clock.WarningTime - Script_Clock.AwareTime;
                    
                    l.intensity         = DefaultEndingIntensity; 
                    Debug.Log($"{name} Setting Directional light {l.name} intensity to {l.intensity}");

                    break;
                
                case (Script_Clock.TimeStates.Warning):
                    intensityDelta      = WarningEndingIntensity - AwareEndingIntensity;
                    totalIntervalTime   = Script_Clock.DangerTime - Script_Clock.WarningTime;
                    
                    l.intensity         = AwareEndingIntensity;
                    Debug.Log($"{name} Setting Directional light {l.name} intensity to {l.intensity}");

                    break;

                case (Script_Clock.TimeStates.Danger):
                    intensityDelta      = DangerEndingIntensity - WarningEndingIntensity;
                    totalIntervalTime   = Script_Clock.EndTime - Script_Clock.DangerTime;
                    
                    l.intensity         = WarningEndingIntensity;
                    Debug.Log($"{name} Setting Directional light {l.name} intensity to {l.intensity}");

                    break;
            }
        }
    }

    private void SetDefaultIntensity()
    {
        intensityDelta      = DefaultEndingIntensity - DefaultIntensity;
        totalIntervalTime   = Script_Clock.AwareTime - clockManager.ClockTime;
        lastState           = Script_Clock.TimeStates.None;
        
        foreach (Light l in directionalLights)
        {
            if (l.type != LightType.Directional)    return;

            l.intensity = DefaultIntensity;
        }
    }
}
