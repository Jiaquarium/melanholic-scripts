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
    private bool isInitialize;
    
    void OnEnable()
    {
        isInitialize = true;
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
        // isInitialize flag prevents SFX when pulling up the Canvas
        if (!isInitialize)  PlaySFX();
        else                isInitialize = false;
    }

    protected void PlaySFX()
    {
        source.PlayOneShot(Script_SFXManager.SFX.Select, Script_SFXManager.SFX.SelectVol);
    }
}
