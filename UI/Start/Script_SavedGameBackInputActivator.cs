using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Script_SavedGameBackInputActivator : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Script_SavedGameBackInputManager savedGameBackInputManager;
    
    public virtual void OnSelect(BaseEventData e)
    {
        savedGameBackInputManager.gameObject.SetActive(true);
    }

    public virtual void OnDeselect(BaseEventData e)
    {
        savedGameBackInputManager.gameObject.SetActive(false);
    }    
}
