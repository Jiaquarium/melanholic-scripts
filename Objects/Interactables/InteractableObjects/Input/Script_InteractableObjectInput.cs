using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Displays an input canvas on interaction.
/// 
/// CCTV Choice must have Script_InteractableObjectInputChoice which 
/// </summary>
public class Script_InteractableObjectInput : Script_InteractableObject
{
    [SerializeField] private Script_InputManager inputManager;
    [SerializeField] private InputMode inputMode;
    [SerializeField] private TMP_InputField inputField;
    
    // Subscribe to Input Submit event
    
    public override void ActionDefault()
    {
        if (CheckDisabled())  return;

        game.GetPlayer().SetIsTalking();

        // Set input canvas active
        inputManager.Initialize(inputMode, inputField);
        inputManager.gameObject.SetActive(true);
    }

    // Called from Level Behavior
    public void OnSubmitSuccess()
    {

    }

    // Called from Level Behavior
    public void OnSubmitFailure()
    {

    }
}
