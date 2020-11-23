using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Script_ButtonActivator : MonoBehaviour, ISelectHandler
{
    [SerializeField] private Script_SlotsViewController savedGameSlotsController;
    public virtual void OnSelect(BaseEventData e)
    {
        savedGameSlotsController.gameObject.SetActive(true);
    }
}
