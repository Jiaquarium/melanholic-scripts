using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Script_Switch : Script_InteractableObject
{
    public int switchId;
    public Sprite onSprite;
    public Sprite offSprite;
    public bool isOn;
    public bool isStickyOn;
    public bool isStickyOff;
    [SerializeField] protected bool isSFXOverriden; // allows lightSwitch to use different default SFX

    public virtual void TurnOff()
    {
        isOn = false;
        rendererChild.GetComponent<SpriteRenderer>().sprite = offSprite;
        
        game.SetSwitchState(switchId, false);

        if (!isSFXOverriden)
        {
            GetComponent<AudioSource>().PlayOneShot(Script_SFXManager.SFX.SwitchOff, Script_SFXManager.SFX.SwitchOffVol);
        }

        Script_InteractableObjectEventsManager.SwitchOff(nameId);   
    }

    public virtual void TurnOn()
    {
        isOn = true;
        rendererChild.GetComponent<SpriteRenderer>().sprite = onSprite;
        
        game.SetSwitchState(switchId, true);

        if (!isSFXOverriden)
        {
            GetComponent<AudioSource>().PlayOneShot(Script_SFXManager.SFX.SwitchOn, Script_SFXManager.SFX.SwitchOnVol);
        }

        Script_InteractableObjectEventsManager.SwitchOn(nameId);
    }

    public override void ActionDefault()
    {
        print("action default called in Switch");
        if (isOn)
        {
            if (!isStickyOn)    TurnOff();
        }
        else
        {
            if (!isStickyOff)   TurnOn();
        }
    }

    public override void SetupSwitch(
        bool _isOn,
        Sprite _onSprite,
        Sprite _offSprite
    )
    {
        isOn = _isOn;
        
        if (_onSprite != null)    onSprite = _onSprite;
        if (_offSprite != null)   offSprite = _offSprite;

        rendererChild.GetComponent<SpriteRenderer>().sprite = isOn ? onSprite : offSprite;
    }
}
