using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Script_Switch : Script_InteractableObject
{
    public int switchId;
    public Sprite onSprite;
    public Sprite offSprite;
    public bool isOn;
    public bool isStickyOn;
    public bool isStickyOff;
    // Allows lightSwitch to use different default SFX.
    [SerializeField] protected bool isSFXOverriden;
    // Use this when the switch needs to react to state; default is the level behavior reacting to switches state
    [SerializeField] private bool isStateless;
    [Tooltip("To specify an event when switch is turned on. Usually use with isStateless = true.")]
    [SerializeField] private UnityEvent onAction;
    [Tooltip("To specify an event when switch is turned off. Usually use with isStateless = true.")]
    [SerializeField] private UnityEvent offAction;

    protected override void OnEnable()
    {
        base.OnEnable();
    }
    
    public virtual void TurnOff()
    {
        isOn = false;
        rendererChild.GetComponent<SpriteRenderer>().sprite = offSprite;
        
        if (!isStateless)   game.SetSwitchState(switchId, false);

        if (!isSFXOverriden)
        {
            GetComponent<AudioSource>().PlayOneShot(Script_SFXManager.SFX.SwitchOff, Script_SFXManager.SFX.SwitchOffVol);
        }

        if (offAction.CheckUnityEventAction())  offAction.Invoke();
        Script_InteractableObjectEventsManager.SwitchOff(nameId);   
    }

    public virtual void TurnOn()
    {
        isOn = true;
        rendererChild.GetComponent<SpriteRenderer>().sprite = onSprite;
        
        if (!isStateless)   game.SetSwitchState(switchId, true);

        if (!isSFXOverriden)
        {
            GetComponent<AudioSource>().PlayOneShot(Script_SFXManager.SFX.SwitchOn, Script_SFXManager.SFX.SwitchOnVol);
        }

        if (onAction.CheckUnityEventAction())    onAction.Invoke();
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

    protected override void AutoSetup()
    {
        base.AutoSetup();
        game.switches.Add(this);
    }
}
