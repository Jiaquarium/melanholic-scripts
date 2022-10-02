using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Script_InputNameHandler : Script_InputHandler
{
    [SerializeField] private Script_TMPInputNameValidator TMPInputNameValidator;

    public override void SetValidation(TMP_InputField TMPInputField)
    {
        TMPInputField.inputValidator = TMPInputNameValidator;
    }
    
    public override int HandleSubmit(string text)
    {
        var stripped = text.Trim();
        
        if (stripped.Length == 0)
            return -1;

        Script_Names.Player = stripped;

        Dev_Logger.Debug("Saving name: " + stripped);

        Script_SFXManager.SFX.PlayContractSign();
        
        return 0;
    }
}
