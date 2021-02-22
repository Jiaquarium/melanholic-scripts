using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Script_InteractableObjectInputChoice : Script_UIChoice
{
    public Script_InputManager inputManager;
    public TMP_InputField inputField;

    public override void HandleSelect()
    {
        inputManager.InteractableObjectInputSubmit(Id, inputField.text);
    }
}
