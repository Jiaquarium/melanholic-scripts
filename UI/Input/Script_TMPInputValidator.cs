using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Script_TMPInputValidator : TMP_InputValidator
{
    public override char Validate(ref string text, ref int pos, char ch)
    {
        return ch;
    }
    
    protected char Error(char ch, int ASCIICode)
    {
        // Handle ignoring ESC
        if (ASCIICode != Const_InputValidation.EscASCIICode)
            Script_InputManager.Control.ErrorSFX();
        
        return ch;
    }

    protected char Insert(ref string text, ref int pos, char ch)
    {
        Script_InputManager.Control.InsertSFX();   
        
        text = text.Insert(pos, ch.ToString());
        pos++;
        return ch;   
    }
}
