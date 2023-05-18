using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Rewired;
using UnityEngine.UI;

[RequireComponent(typeof(Script_CanvasGroupController))]
public class Script_LetterSelectGrid : MonoBehaviour
{
    [SerializeField] private Script_EntryInput entryInput;
    [SerializeField] private Button firstSelected;

    private Player rewiredInput;
    private bool isSelected;

    public Button FirstSelected => firstSelected;
    private Script_CanvasGroupController CanvasGroupController => GetComponent<Script_CanvasGroupController>();

    private bool isCaretVisible;
    
    void Start()
    {
        rewiredInput = Script_PlayerInputManager.Instance.RewiredInput;
    }
    
    void Update()
    {
        HandleInputs();
    }

    public void Open() => CanvasGroupController.Open();
    
    public void Close() => CanvasGroupController.Close();

    private void HandleInputs()
    {
        if (!GetIsActive() || !EventSystem.current.sendNavigationEvents)
            return;

        HandleBackspace();
        
        var lastController = rewiredInput.controllers.GetLastActiveController();
        // If lastController is null, then keyboard must be active at least
        if (lastController == null || lastController.type == ControllerType.Keyboard)
            HandleCancel();

        void HandleBackspace()
        {
            if (rewiredInput.GetButtonDown(Const_KeyCodes.RWBackspace))
            {
                entryInput.DeleteLetter();
            }
        }

        // Keyboard RWUICancel should nav to submit
        // Joystick RWUICancel should not nav to submit since it contains a binding to the same button as RWUIBackspace
        // action (handling Backspace for Letter Select Grid)
        void HandleCancel()
        {
            if (rewiredInput.GetButtonDown(Const_KeyCodes.RWUICancel))
            {
                entryInput.NavToSubmitShortcut();
            }
        }
    }
    
    public bool GetIsActive()
    {
        isSelected = false;
        EventSystem currentEventSystem = EventSystem.current;

        if (currentEventSystem == null || !this.gameObject.activeInHierarchy)
            return isSelected;

        var letterSelect = currentEventSystem.currentSelectedGameObject?.GetComponent<Script_LetterSelect>();

        if (letterSelect != null && letterSelect.gameObject.activeInHierarchy)
            isSelected = letterSelect.LetterSelectGrid == this;

        return isSelected;
    }

    public void SetNavInteractable(bool isActive)
    {
        firstSelected.enabled = isActive;
    }
}
