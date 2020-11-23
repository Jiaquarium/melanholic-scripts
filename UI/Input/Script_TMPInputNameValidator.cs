using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "TMPInputNameValidator", menuName = "TMPInputNameValidator")]
public class Script_TMPInputNameValidator : TMP_InputValidator
{
    public override char Validate(ref string text, ref int pos, char ch)
    {
        int ASCIICode = (int)ch;
        
        if (pos >= Const_InputValidation.Name.maxCharCount)     return ch;

        if (
            ASCIICode >= Const_InputValidation.Name.minASCII
            && ASCIICode <= Const_InputValidation.Name.maxASCII
        )
        {
            text = text.Insert(pos, ch.ToString());
            pos++;
            return ch;
        }
        
        return ch;
    }
}
