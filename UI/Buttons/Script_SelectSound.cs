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
    
    [Tooltip("Specify gameObject to ignore OnSelect SFX when coming it. Useful to squelch SFX on initialize.")]
    public GameObject transition;
    
    // Specify this parent to ignore OnSelect SFX when coming it.
    // Useful to squelch SFX on initialize.
    public Transform noSFXTransitionParent;
    
    // Specify this parent to only make OnSelect SFX when coming from a child of it.
    // Useful to squelch SFX on initialize.
    [SerializeField] private Transform onlySFXTransitionParent;
    
    [SerializeField] private Button button { get { return GetComponent<Button>(); } }
    private AudioSource source;
    

    void Awake()
    {
        source = settings.selectAudioSource;
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
        
        if (onlySFXTransitionParent != null)
        {
            bool isOutsideSFXParent = true;

            foreach (Transform child in onlySFXTransitionParent)
            {
                if (child.gameObject == eventSystem.lastSelected){
                    Debug.Log("Ignoring SelectSFX because coming from child not specified by onlySFXTransitionParent");
                    isOutsideSFXParent = false;
                }
            }

            if (isOutsideSFXParent)
                return;
        }

        PlaySFX();
    }

    public void OnSubmit(BaseEventData e) { }

    protected void PlaySFX()
    {
        source.PlayOneShot(Script_SFXManager.SFX.Select, Script_SFXManager.SFX.SelectVol);
    }
}
