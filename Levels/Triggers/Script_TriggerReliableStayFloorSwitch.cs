using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_TriggerReliableStayFloorSwitch : Script_TriggerReliableStay
{
    [SerializeField] private Transform switchUpGraphics;
    [SerializeField] private Transform switchDownGraphics;
    [SerializeField] private Light pointLight;

    protected override void HandleUpState(bool SFXOn)
    {
        Debug.Log($"{name} HandleUpState()");
        
        base.HandleUpState(SFXOn);

        GetComponent<AudioSource>().PlayOneShot(
            Script_SFXManager.SFX.FloorSwitchUp, Script_SFXManager.SFX.FloorSwitchUpVol
        );
        
        if (switchUpGraphics != null)
            switchUpGraphics.gameObject.SetActive(true);
        if (switchDownGraphics != null)
            switchDownGraphics.gameObject.SetActive(false);
    }
    
    protected override void HandleDownState(bool SFXOn)
    {
        base.HandleDownState(SFXOn);

        GetComponent<AudioSource>().PlayOneShot(
            Script_SFXManager.SFX.FloorSwitchDown, Script_SFXManager.SFX.FloorSwitchDownVol
        );
        
        if (switchUpGraphics != null)
            switchUpGraphics.gameObject.SetActive(false);
        if (switchDownGraphics != null)
            switchDownGraphics.gameObject.SetActive(true);
    }

    public override void InitialState()
    {
        base.InitialState();
        
        if (pointLight != null)
            pointLight.gameObject.SetActive(true);
    }
}
