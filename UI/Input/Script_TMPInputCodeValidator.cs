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
            && pos < Const_InputValidation.Code.maxCharCount
        )
        {
            string copy = text;
            text = copy.Substring(0, pos) + ch + copy.Substring(pos + 1);
            
            Script_InputManager.Control.InsertCodeSFX();
            
            return ch;
        }
        
        return Error(ch, ASCIICode);
    }

    protected override char Error(char ch, int ASCIICode)
    {
        // Ignore WASD error SFX for CCTV Code, since these are used for navigation here
        bool isWASD = ch == 'w' || ch == 'a' || ch == 's' || ch == 'd'
            || ch == 'W' || ch == 'A' || ch == 'S' || ch == 'D';
        
        // Handle ignoring ESC
        if (ASCIICode != Const_InputValidation.EscASCIICode && !isWASD)
            Script_InputManager.Control.ErrorSFX();
        
        return ch;
    }
}
