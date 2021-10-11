using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Script_UIChoice))]
public class Script_DialogueChoiceSelectSound : MonoBehaviour, ISelectHandler, ISubmitHandler
{
    public Script_InventoryAudioSettings settings;
    private AudioSource source;
    
    void OnEnable()
    {
        Debug.Log($"{name} {this.GetType()} OnEnable");
    }
    
    void Awake()
    {
        source = settings.selectAudioSource;
    }
    
    public void OnSubmit(BaseEventData e)
    {
    }

    public virtual void OnSelect(BaseEventData e)
    {
        Debug.Log($"{name} {this.GetType()} OnSelect");
        
        SelectSFX();
    }

    private void SelectSFX()
    {
        source.PlayOneShot(Script_SFXManager.SFX.Select, Script_SFXManager.SFX.SelectVol);
    }
}
