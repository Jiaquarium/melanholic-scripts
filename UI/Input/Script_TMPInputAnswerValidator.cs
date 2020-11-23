using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "TMPInputAnswerValidator", menuName = "TMPInputAnswerValidator")]
public class Script_TMPInputAnswerValidator : TMP_InputValidator
{
    public override char Validate(ref string text, ref int pos, char ch)
    {
        int ASCIICode = (int)ch;
        
        // handle maxChar count
        if (pos >= Const_InputValidation.Answer.maxCharCount)     return ch;

        // restrict ASCIICodes
        if (
            ASCIICode >= Const_InputValidation.Answer.minASCII
            && ASCIICode <= Const_InputValidation.Answer.maxASCII
        )
        {
            text = text.Insert(pos, ch.ToString());
            pos++;
            return ch;
        }
        
        return ch;
    }
}
