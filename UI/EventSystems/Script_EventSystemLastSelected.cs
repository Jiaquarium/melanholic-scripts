using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Script_EventSystemLastSelected : MonoBehaviour {
    public GameObject currentSelected;
    // gets set to NULL on inventory close
    public GameObject lastSelected;
    public Button failCaseObject;
    
    void OnEnable()
    {
        // need this to manually refresh its current selected
        // e.g. some cases Event System's Update() won't fire if we're
        // doing an action directly after exiting submenu
        Script_MenuEventsManager.OnExitSubmenu += UpdateCurrentSelected;
    }

    void OnDisable() {
        Script_MenuEventsManager.OnExitSubmenu += UpdateCurrentSelected;
    }

    void Update () {         
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(currentSelected);
        }
        
        lastSelected = currentSelected;
        UpdateCurrentSelected();

        // to be extra safe, if the rare case currentSelected is still empty, set it to the first choice
        if (currentSelected == null)
        {
            if (failCaseObject != null)   currentSelected = failCaseObject.gameObject;
        }
    }

    public void UpdateCurrentSelected()
    {
        if (EventSystem.current == null)    return;
        
        currentSelected = EventSystem.current.currentSelectedGameObject;
    }

    public void InitializeState()
    {
        currentSelected = null;
        lastSelected = null;
    }
}