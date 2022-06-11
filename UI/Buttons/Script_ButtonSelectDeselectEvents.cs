using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class Script_ButtonSelectDeselectEvents : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private UnityEvent onSelectAction;
    [SerializeField] private UnityEvent onDeselectAction;
    
    public virtual void OnSelect(BaseEventData e)
    {
        onSelectAction.SafeInvoke();   
    }

    public virtual void OnDeselect(BaseEventData e)
    {
        onDeselectAction.SafeInvoke();
    }
}
