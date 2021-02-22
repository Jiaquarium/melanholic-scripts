using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "TMPInputAnswerValidator", menuName = "TextMeshPro/TMPInputValidator/TMPInputAnswerValidator")]
public class Script_TMPInputAnswerValidator : Script_TMPInputValidator
{
    public override char Validate(ref string text, ref int pos, char ch)
    {
        int ASCIICode = (int)ch;
        
        if (
            ASCIICode >= Const_InputValidation.Answer.minASCII
            && ASCIICode <= Const_InputValidation.Answer.maxASCII
            && pos < Const_InputValidation.Answer.maxCharCount
        )
        {
            return Insert(ref text, ref pos, ch);
        }
        
        return Error(ch);
    }
}
