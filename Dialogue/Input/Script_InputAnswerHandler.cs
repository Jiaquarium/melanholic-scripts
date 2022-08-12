using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Script_InputAnswerHandler : Script_InputHandler
{
    [SerializeField] private Script_TMPInputAnswerValidator TMPInputAnswerValidator;

    public override void SetValidation(TMP_InputField TMPInputField)
    {
        Debug.Log($"Setting input validation to {TMPInputAnswerValidator}");
        TMPInputField.inputValidator = TMPInputAnswerValidator;
    }

    public override int HandleSubmit(string text)
    {
        // allow game -> level behavior to handle
        print("give submission to Script_Game: " + text);

        Script_SFXManager.SFX.PlayUIChoiceSubmit();

        int childNodeIdx = Script_Game.Game.HandleSubmit(text);
        return childNodeIdx;
    }
}
