using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ItemChoices
{
    Stick,
    Drop,
    Examine,
    Use,
    Cancel
}

public class Script_ItemChoices : MonoBehaviour
{
    [SerializeField] private Script_ItemChoice[] itemChoiceChildren;
    [SerializeField] private EventSystem eventSystem;

    public int itemSlotId { get; set;}

    void OnValidate()
    {
        UpdateActiveChildren();
    }

    public void SetFirstChoiceActive()
    {
        var firstChildChoice = itemChoiceChildren[0].gameObject;
        
        eventSystem.SetSelectedGameObject(firstChildChoice);
    }
    
    public void SetDropChoice(bool isActive)
    {
        foreach (Script_ItemChoice itemChoice in itemChoiceChildren)
        {
            if (itemChoice.ItemChoice == ItemChoices.Drop)
            {
                itemChoice.gameObject.SetActive(isActive);
                UpdateActiveChildren();
            }
        }
    }

    /// <summary>
    /// Sets only active children and sets their explicit nav accordingly
    /// </summary>
    void UpdateActiveChildren()
    {
        itemChoiceChildren = transform.GetChildren<Script_ItemChoice>(true);
        itemChoiceChildren.SetExplicitListNav();
    }
}
