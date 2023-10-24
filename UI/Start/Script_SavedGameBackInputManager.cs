using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Script_SavedGameBackInputManager : Script_ExitViewInputManager
{
    [SerializeField] private Script_SavedGameInputManager savedGameInputManager;
    [SerializeField] private GameObject backButton;
    
    void OnDisable()
    {
        // Must set initial state when going back because OnDeselect will not be triggered
        // on Back's Activator
        InitialState();
    }

    void LateUpdate()
    {
        if (masterUIState != null && masterUIState.state == UIState.Disabled)
            return;
        
        // Also double check if back is currently selected game object
        var currentEventSystem = EventSystem.current;
        if (currentEventSystem != null && currentEventSystem.currentSelectedGameObject == backButton)
            savedGameInputManager.HandleExitInput();
    }

    public void InitialState()
    {
        gameObject.SetActive(false);
    }
}
