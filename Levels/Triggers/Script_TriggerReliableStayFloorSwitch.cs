using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_TriggerReliableStayFloorSwitch : Script_TriggerReliableStay
{
    [SerializeField] private Transform switchUpGraphics;
    [SerializeField] private Transform switchDownGraphics;
    [SerializeField] private Light pointLight;

    protected override void HandleUpState()
    {
        base.HandleUpState();

        Script_SFXManager SFX = Script_SFXManager.SFX;
        GetComponent<AudioSource>().PlayOneShot(SFX.FloorSwitchUp, SFX.FloorSwitchUpVol);   
        
        if (switchUpGraphics != null)       switchUpGraphics.gameObject.SetActive(true);
        if (switchDownGraphics != null)     switchDownGraphics.gameObject.SetActive(false);
    }
    
    protected override void HandleDownState()
    {
        base.HandleDownState();

        Script_SFXManager SFX = Script_SFXManager.SFX;
        GetComponent<AudioSource>().PlayOneShot(SFX.FloorSwitchDown, SFX.FloorSwitchDownVol);   
        
        if (switchUpGraphics != null)       switchUpGraphics.gameObject.SetActive(false);
        if (switchDownGraphics != null)     switchDownGraphics.gameObject.SetActive(true);
    }

    public override void InitialState()
    {
        base.InitialState();
        
        if (pointLight != null)             pointLight.gameObject.SetActive(true);
    }
}
