using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Can either directly reference specific lights or reference
/// LightControllers
/// </summary>
public class Script_LightSwitch : Script_Switch
{
    [SerializeField] private Transform lightsParent;
    [SerializeField] private Light[] lights;
    [SerializeField] private Script_LightsController[] lightsControllers;
    
    public float onIntensity;
    public float offIntensity;
    public AudioSource audioSource;

    protected override void OnValidate()
    {
        FindLights();    
    }
    
    protected override void Awake()
    {
        FindLights();

        base.Awake();
    }
    
    protected override void Start()
    {
        base.Start();
        if (lightsControllers.Length > 0)
        {
            foreach (Script_LightsController lc in lightsControllers)   lc.ShouldUpdate = true;
            return;
        }
    }
    
    public override void TurnOn()
    {
        base.TurnOn();
        
        var sfx = Script_SFXManager.SFX;
        audioSource.PlayOneShot(sfx.LightSwitchOn, sfx.LightSwitchOnVol);

        if (lightsControllers.Length > 0)
        {
            foreach (Script_LightsController lc in lightsControllers)   lc.Intensity = onIntensity;
            return;
        }
        
        foreach (Light l in lights)     l.intensity = onIntensity;
    }

    public override void TurnOff()
    {
        base.TurnOff();

        var sfx = Script_SFXManager.SFX;
        audioSource.PlayOneShot(sfx.LightSwitchOff, sfx.LightSwitchOffVol);
        
        if (lightsControllers.Length > 0)
        {
            foreach (Script_LightsController lc in lightsControllers)   lc.Intensity = offIntensity;
            return;
        }
        
        foreach (Light l in lights)     l.intensity = offIntensity;
    }

    private void FindLights()
    {
        if (lightsParent != null)
        {
            lights = lightsParent.GetComponentsInChildren<Light>(true);
        }
    }

    public override void SetupSwitch(
        bool _isOn,
        Sprite onSprite,
        Sprite offSprite
    )
    {
        base.SetupSwitch(_isOn, onSprite, offSprite);
        isSFXOverriden = true;
    }

    // for instantiation
    public override void SetupLights(
        Light[] _lights,
        float _onIntensity,
        float _offIntensity,
        bool isOn,
        Sprite onSprite,
        Sprite offSprite
    )
    {
        lights = _lights;
        onIntensity = _onIntensity;
        offIntensity = _offIntensity;
        SetupSwitch(isOn, onSprite, offSprite);

        foreach (Light l in lights)
        {
            l.intensity = isOn ? onIntensity : offIntensity;
        }
    }

    public void SetupSceneLights(
        bool isOn
    )
    {
        SetupSwitch(isOn, onSprite, offSprite);

        if (lightsControllers.Length > 0)
        {
            foreach (Script_LightsController lc in lightsControllers)
            {
                lc.Intensity = isOn ? onIntensity : offIntensity;
            }
            return;
        }
        
        foreach (Light l in lights)
        {
            l.intensity = isOn ? onIntensity : offIntensity;
        }
    }
}
