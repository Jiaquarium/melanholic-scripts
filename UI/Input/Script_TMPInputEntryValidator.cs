using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "TMPInputEntryValidator", menuName = "TMPInputEntryValidator")]
public class Script_TMPInputEntryValidator : TMP_InputValidator
{
    public override char Validate(ref string text, ref int pos, char ch)
    {
        int ASCIICode = (int)ch;
        
        // handle maxChar count
        if (pos >= Const_InputValidation.Entry.maxCharCount)     return ch;
        
        // restrict ASCIICodes
        if (
            ASCIICode >= Const_InputValidation.Entry.minASCII
            && ASCIICode <= Const_InputValidation.Entry.maxASCII
        )
        {
            text = text.Insert(pos, ch.ToString());
            pos++;
            return ch;
        }
        
        return ch;
    }
}
