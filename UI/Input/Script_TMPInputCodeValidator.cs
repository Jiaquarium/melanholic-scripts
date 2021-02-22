using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "TMPInputCodeValidator", menuName = "TextMeshPro/TMPInputValidator/TMPInputCodeValidator")]
public class Script_TMPInputCodeValidator : Script_TMPInputValidator
{
    public override char Validate(ref string text, ref int pos, char ch)
    {
        int ASCIICode = (int)ch;
        
        if (
            ASCIICode >= Const_InputValidation.Code.minASCII
            && ASCIICode <= Const_InputValidation.Code.maxASCII
            && pos < Script_ScarletCipherManager.QuestionCount
        )
        {
            return Insert(ref text, ref pos, ch);
        }
        
        return Error(ch);
    }
}
