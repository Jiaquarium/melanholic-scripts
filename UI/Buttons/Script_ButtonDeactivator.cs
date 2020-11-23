using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Prevents SlotViewController from error'ing as it can only handle slots in its event system
/// </summary>
public class Script_ButtonDeactivator : MonoBehaviour, ISelectHandler
{
    [SerializeField] private Script_SlotsViewController savedGameSlotsController;
    public virtual void OnSelect(BaseEventData e)
    {
        savedGameSlotsController.gameObject.SetActive(false);
    }
}
