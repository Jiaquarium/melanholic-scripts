using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "TMPInputNameValidator", menuName = "TextMeshPro/TMPInputValidator/TMPInputNameValidator")]
public class Script_TMPInputNameValidator : Script_TMPInputValidator
{
    public override char Validate(ref string text, ref int pos, char ch)
    {
        int ASCIICode = (int)ch;
        
        if (
            ASCIICode >= Const_InputValidation.Name.minASCII
            && ASCIICode <= Const_InputValidation.Name.maxASCII
            && pos < Const_InputValidation.Name.maxCharCount
        )
        {
            return Insert(ref text, ref pos, ch);
        }
        
        return Error(ch);
    }
}
