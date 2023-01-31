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
    [SerializeField] private AudioSource audioSourceOverride;
    
    private AudioSource source;
    
    void OnEnable()
    {
        Dev_Logger.Debug($"{name} {this.GetType()} OnEnable");
    }
    
    void Awake()
    {
        if (audioSourceOverride != null)
            source = audioSourceOverride;
        else
            source = settings.selectAudioSource;
    }
    
    public void OnSubmit(BaseEventData e)
    {
    }

    public virtual void OnSelect(BaseEventData e)
    {
        Dev_Logger.Debug($"{name} {this.GetType()} OnSelect");

        // If selected during Slow Awake, make sure to not play SFX
        Script_SlowAwakeEventSystem slowAwakeEventSystem = null;
        if (EventSystem.current != null)
            slowAwakeEventSystem = EventSystem.current.GetComponent<Script_SlowAwakeEventSystem>();
        
        if (slowAwakeEventSystem != null && !slowAwakeEventSystem.IsTimerDone)
            return;
        
        SelectSFX();
    }

    private void SelectSFX()
    {
        source.PlayOneShot(Script_SFXManager.SFX.Select, Script_SFXManager.SFX.SelectVol);
    }
}
