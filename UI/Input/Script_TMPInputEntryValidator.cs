using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "TMPInputEntryValidator", menuName = "TextMeshPro/TMPInputValidator/TMPInputEntryValidator")]
public class Script_TMPInputEntryValidator : Script_TMPInputValidator
{
    public override char Validate(ref string text, ref int pos, char ch)
    {
        int ASCIICode = (int)ch;
        
        // Restrict by ASCII Code and Char Count
        if (
            ASCIICode >= Const_InputValidation.Entry.minASCII
            && ASCIICode <= Const_InputValidation.Entry.maxASCII
            && pos < Const_InputValidation.Entry.maxCharCount
            && text.Length < Const_InputValidation.Entry.maxCharCount
        )
        {
            return Insert(ref text, ref pos, ch);
        }
        
        return Error(ch, ASCIICode);
    }
}
