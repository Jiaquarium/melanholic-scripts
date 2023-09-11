using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
using System.Text;
#endif

public class Script_LightFXManager : MonoBehaviour
{
    public static Script_LightFXManager Control;
    
    [SerializeField] private bool isPaused;
    
    [SerializeField] float currentIntensity = 0.15f;
    [SerializeField] float currentSoftEndRampIntensity = 0.15f;
    [SerializeField] float currentRampFrom1Intensity = 0.9983386f;
    [SerializeField] float defaultIntensity = 0.15f;
    [SerializeField] float endingIntensity = 20f;
    
    [SerializeField] private AnimationCurve lightCurve;
    
    // Ramps from 2.5 to 4.0 for last 5 game minutes
    [SerializeField] private Script_LightFXCurve softEndRampLightCurve;
    [SerializeField] private Script_LightFXCurve rampFrom1LightCurve;
    
    [SerializeField] private Script_Game game;
    [SerializeField] private List<Light> directionalLights;
    [SerializeField] private Light wellsWorldDirectionalLight;
    [SerializeField] private Light rockGardenDirectionalLight;
    [SerializeField] private Light catWalk2DirectionalLight;
    [SerializeField] private Script_ClockManager clockManager;

    [Tooltip("Set to buffer intensity changes so we're not doing this every frame")]
    // Also gives an interesting old school affect when getting down to Danger Time.
    [SerializeField] private float bufferTime;

    private float timer;

    public float LightCurvePercent => lightCurve.Evaluate(clockManager.PercentTimeElapsed);
    public float SoftEndRampLightCurvePercent => softEndRampLightCurve.LightCurve
        .Evaluate(clockManager.PercentTimeElapsed);
    public float RampFrom1LightCurvePercent => rampFrom1LightCurve.LightCurve
        .Evaluate(clockManager.PercentTimeElapsed);
    public float RampFrom1LightCurvePercentAtTime0 => rampFrom1LightCurve.LightCurve.Evaluate(0f);

    // ------------------------------------------------------------------
    // Trailer Only
    
    [SerializeField] private float intensityIncrement = 0.10f;

    // ------------------------------------------------------------------

    public bool IsPaused
    {
        get => isPaused;
        set => isPaused = value;
    }
    
    public float CurrentIntensity
    {
        get => currentIntensity;
        private set => currentIntensity = value;
    }

    public float CurrentSoftEndRampIntensity
    {
        get => currentSoftEndRampIntensity;
        private set => currentSoftEndRampIntensity = value;
    }

    public float CurrentRampFrom1Intensity
    {
        get => currentRampFrom1Intensity;
        private set => currentRampFrom1Intensity = value;
    }

    private float IntensityDelta => endingIntensity - defaultIntensity;
    
    void OnDisable()
    {
        foreach (Light l in directionalLights)
        {
            if (l != null)  l.gameObject.SetActive(false);
        }
    }
    
    void Start()
    {
        SetDefaultIntensity();    
    }
    
    void Update()
    {
        if (Const_Dev.IsTrailerMode)
        {
            if (Input.GetButtonDown(Const_KeyCodes.DevIncrement) && Input.GetButton(Const_KeyCodes.Lights))
                AdjustCurrentLightsIntensity(true);
            else if (Input.GetButtonDown(Const_KeyCodes.DevDecrement) && Input.GetButton(Const_KeyCodes.Lights))
                AdjustCurrentLightsIntensity(false);
            else if (Input.GetKeyDown(KeyCode.P) && Input.GetButton(Const_KeyCodes.Lights))
                AdjustCurrentLightsIntensity(true, true);
            else if (Input.GetKeyDown(KeyCode.O) && Input.GetButton(Const_KeyCodes.Lights))
                AdjustCurrentLightsIntensity(false, true);
            
            return;
        }
        
        if (game.IsInHotel() || game.IsHideHUD)
            return;
        
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            UpdateDirectionalLights();
            
            timer = bufferTime;
        }
    }

    /// <summary>
    /// Force a light intensity (only works when LightFXManager is paused)
    /// Does not update current intensity.
    /// </summary>
    public void SetDirectionalLightsIntensity(float intensity)
    {
        foreach (Light l in directionalLights)
        {
            if (l.type != LightType.Directional)
                return;

            l.intensity = intensity;
        }
    }

    /// <summary>
    /// Explicitly set current lights intensity and set lights. Only works if
    /// LightFXManager is paused or Const_Dev.IsTrailerMode is true.
    /// Used for Trailer
    /// </summary>
    public void AdjustCurrentLightsIntensity(bool isIncrement, bool isBigIncrement = false)
    {
        var incrementStep = isBigIncrement ? intensityIncrement * 10 : intensityIncrement;
        
        var increment = isIncrement ? incrementStep : -incrementStep;   
        var newIntensity = Mathf.Max(0f, CurrentIntensity + increment);
        CurrentIntensity = newIntensity;
        
        SetDirectionalLightsIntensity(CurrentIntensity);
    }

    private void UpdateDirectionalLights()
    {
        foreach (Light l in directionalLights)
        {
            if (l.type != LightType.Directional)
                return;

            // Soft End Ramp Maps
            if (IsSoftEndRampLight(l))
            {
                CurrentSoftEndRampIntensity = GetCurrentIntensity(SoftEndRampLightCurvePercent);
                SetLightHandlePause(l, CurrentSoftEndRampIntensity);
            }
            // Ramp From 1 Maps
            else if (IsRampFrom1Light(l))
            {
                CurrentRampFrom1Intensity = GetCurrentIntensity(RampFrom1LightCurvePercent);
                SetLightHandlePause(l, CurrentRampFrom1Intensity);
            }
            // Default
            else
            {
                CurrentIntensity = GetCurrentIntensity(LightCurvePercent);
                SetLightHandlePause(l, CurrentIntensity);
            }
        }
    }

    private float GetCurrentIntensity(float lightCurvePercent) => lightCurvePercent * IntensityDelta + defaultIntensity;
    
    private void SetLightHandlePause(Light light, float intensity)
    {
        if (IsPaused)
            return;
        
        light.intensity = intensity;
    }
    
    private bool IsSoftEndRampLight(Light l) => l == wellsWorldDirectionalLight
        || l == rockGardenDirectionalLight;
    
    private bool IsRampFrom1Light(Light l) => l == catWalk2DirectionalLight;

    private void SetDefaultIntensity()
    {   
        foreach (Light l in directionalLights)
        {
            if (l.type != LightType.Directional)
                return;

            // Ramp from 1 starts at 0.9983386 at time 0. Every other light starts at 0.15f.
            if (IsSoftEndRampLight(l))
            {
                CurrentSoftEndRampIntensity = defaultIntensity;
                SetLightHandlePause(l, CurrentSoftEndRampIntensity);
            }
            else if (IsRampFrom1Light(l))
            {
                CurrentRampFrom1Intensity = GetCurrentIntensity(RampFrom1LightCurvePercentAtTime0);
                SetLightHandlePause(l, CurrentRampFrom1Intensity);
            }
            else
            {
                CurrentIntensity = defaultIntensity;
                SetLightHandlePause(l, CurrentIntensity);
            }
        }
    }

    public void Setup()
    {
        if (Control == null)
        {
            Control = this;
        }
        else if (Control != this)
        {
            Destroy(this.gameObject);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_LightFXManager))]
    public class Script_LightFXManagerTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_LightFXManager t = (Script_LightFXManager)target;
            if (GUILayout.Button("Print Light Chart"))
            {
                int MaxTime = 60;
                var stringBuilder = new StringBuilder();

                for (int i = 0; i <= MaxTime; i = i + 1)
                {
                    float percentElapsed = t.lightCurve.Evaluate(i / 60f);
                    var lightIntensity = t.defaultIntensity + (percentElapsed * (t.endingIntensity - t.defaultIntensity));
                    
                    stringBuilder.AppendLine($"{lightIntensity}");
                }

                Debug.Log($"{stringBuilder}");
            }
        }
    }
#endif
}
