using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LightFXManager : MonoBehaviour
{
    public static Script_LightFXManager Control;
    
    [SerializeField] private bool isPaused;
    
    [SerializeField] float currentIntensity = 0.15f;
    [SerializeField] float defaultIntensity = 0.15f;
    [SerializeField] float endingIntensity = 20f;

    [SerializeField] private AnimationCurve lightCurve;
    
    [SerializeField] private Script_Game game;
    [SerializeField] private List<Light> directionalLights;
    [SerializeField] private Script_ClockManager clockManager;

    [Tooltip("Set to buffer intensity changes so we're not doing this every frame")]
    // Also gives an interesting old school affect when getting down to Danger Time.
    [SerializeField] private float bufferTime;

    private float timer;

    public float LightCurvePercent => lightCurve.Evaluate(clockManager.PercentTimeElapsed);

    // ------------------------------------------------------------------
    // Trailer Only
    
    [SerializeField] private float intensityIncrement = 0.10f;

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
    public void AdjustCurrentLightsIntensity(bool isIncrement)
    {
        var increment = isIncrement ? intensityIncrement : -intensityIncrement;   
        var newIntensity = Mathf.Max(defaultIntensity, CurrentIntensity + increment);
        CurrentIntensity = newIntensity;
        
        SetDirectionalLightsIntensity(CurrentIntensity);
    }

    private void UpdateDirectionalLights()
    {
        foreach (Light l in directionalLights)
        {
            if (l.type != LightType.Directional)
                return;

            float intensityDelta = endingIntensity - defaultIntensity;
            float newIntensity = LightCurvePercent * intensityDelta + defaultIntensity;
            
            CurrentIntensity = newIntensity;
            
            if (IsPaused)
                return;
            
            l.intensity = CurrentIntensity;
        }
    }

    private void SetDefaultIntensity()
    {   
        foreach (Light l in directionalLights)
        {
            if (l.type != LightType.Directional)    return;

            CurrentIntensity = defaultIntensity;
            
            if (IsPaused)
                return;
            
            l.intensity = CurrentIntensity;
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
}
