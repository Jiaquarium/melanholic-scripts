﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LightSwitch : Script_Switch
{
    public Light[] lights;
    
    public float onIntensity;
    public float offIntensity;
    public float volumeScale;
    public AudioSource audioSource;
    public AudioClip onOffSFX;

    public override void TurnOn()
    {
        base.TurnOn();
        
        audioSource.PlayOneShot(onOffSFX, volumeScale);

        foreach (Light l in lights)
        {
            l.intensity = onIntensity;
        }
    }

    public override void TurnOff()
    {
        base.TurnOff();

        audioSource.PlayOneShot(onOffSFX, volumeScale);
        
        foreach (Light l in lights)
        {
            l.intensity = offIntensity;
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

        foreach (Light l in lights)
        {
            l.intensity = isOn ? onIntensity : offIntensity;
        }
    }
}
