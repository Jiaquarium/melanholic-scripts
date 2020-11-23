using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class Script_SelectSound : MonoBehaviour, ISelectHandler, ISubmitHandler
{
    public Script_InventoryAudioSettings settings;
    public Script_EventSystemLastSelected eventSystem;
    // describes gameObject where we don't want to play SFX when coming from it
    public GameObject transition;
    public Transform noSFXTransitionParent;
    [SerializeField]
    private Button button { get { return GetComponent<Button>(); } }
    private AudioSource source;
    

    void Awake()
    {
        source = settings.selectAudioSource;
    }

    public void OnSubmit(BaseEventData e)
    {
    }

    public virtual void OnSelect(BaseEventData e)
    {
        // other option is to tell manager which sounds to play
        // lateUpdate can decide which one to pick (onSubmit takes priority)
        if (
            eventSystem.lastSelected == transition
            || eventSystem.lastSelected == null 
        ) 
        {
            return;
        }

        if (noSFXTransitionParent != null)
        {
            foreach (Transform child in noSFXTransitionParent)
            {
                if (child.gameObject == eventSystem.lastSelected){
                    Debug.Log("Ignoring SelectSFX because coming from a noSFXTransition");
                    return;
                }
            }
        }

        PlaySFX();
    }


    protected void PlaySFX()
    {
        source.PlayOneShot(settings.selectSFX, settings.selectVolume);
    }
}
