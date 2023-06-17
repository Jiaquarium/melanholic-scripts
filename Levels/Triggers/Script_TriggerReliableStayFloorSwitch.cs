using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_TriggerReliableStayFloorSwitch : Script_TriggerReliableStay
{
    [SerializeField] private Transform switchUpGraphics;
    [SerializeField] private Transform switchDownGraphics;
    [SerializeField] private Light pointLight;
    
    public bool IsSFXDisabled { get; set; }

    protected override void HandleUpState(bool SFXOn)
    {
        Dev_Logger.Debug($"{name} HandleUpState()");
        
        base.HandleUpState(SFXOn);

        if (!IsSFXDisabled)
        {
            GetComponent<AudioSource>().PlayOneShot(
                Script_SFXManager.SFX.FloorSwitchUp, Script_SFXManager.SFX.FloorSwitchUpVol
            );
        }
        
        if (switchUpGraphics != null)
            switchUpGraphics.gameObject.SetActive(true);
        
        if (switchDownGraphics != null)
            switchDownGraphics.gameObject.SetActive(false);
    }
    
    protected override void HandleDownState(bool SFXOn)
    {
        base.HandleDownState(SFXOn);

        if (!IsSFXDisabled)
        {
            GetComponent<AudioSource>().PlayOneShot(
                Script_SFXManager.SFX.FloorSwitchDown, Script_SFXManager.SFX.FloorSwitchDownVol
            );
        }
        
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
