using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Script_InputCodeHandler : Script_InputHandler
{
    [SerializeField] private Script_TMPInputAnswerValidator TMPInputAnswerValidator;

    public override void SetValidation(TMP_InputField TMPInputField)
    {
        Debug.Log($"Setting input validation to {TMPInputAnswerValidator}");
        TMPInputField.inputValidator = TMPInputAnswerValidator;
    }
    
    public override int HandleSubmit(string text)
    {
        // Game to direct submission to level behavior to do customized behavior
        Script_Game.Game.HandleSubmit(text);

        return 0;
    }
}
