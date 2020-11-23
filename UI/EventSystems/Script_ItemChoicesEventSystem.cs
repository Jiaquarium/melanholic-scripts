using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Script_ItemChoicesEventSystem : Script_EventSystemLastSelected
{
    [SerializeField] private Transform itemChoicesParent;
    [SerializeField] private Script_ItemChoice[] itemChoices;

    void OnValidate()
    {
        itemChoices = itemChoicesParent.GetComponentsInChildren<Script_ItemChoice>(includeInactive: false);
        failCaseObject = itemChoices[0].GetComponent<Button>();
        GetComponent<EventSystem>().firstSelectedGameObject = itemChoices[0].gameObject;
    }
}
