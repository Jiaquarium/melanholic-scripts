using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Script_InputHandler : MonoBehaviour
{
    public virtual void SetValidation(TMP_InputField TMPInputField) {}
    /// <summary>
    /// check for allowing the submission
    /// </summary>
    /// <returns>index of which childnode to move to
    /// return -1 if we want to disallow the entry (error)</returns>
    public virtual int HandleSubmit(string text) { return -1; }
}
