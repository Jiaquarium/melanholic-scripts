using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AlphaInputUtils : MonoBehaviour
{
    [SerializeField] private List<KeyCode> alphaKeyCodes = new List<KeyCode>(){
        KeyCode.A,
        KeyCode.B,
        KeyCode.C,
        KeyCode.D,
        KeyCode.E,
        KeyCode.F,
        KeyCode.G,
        KeyCode.H,
        KeyCode.I,
        KeyCode.J,
        KeyCode.K,
        KeyCode.L,
        KeyCode.M,
        KeyCode.N,
        KeyCode.O,
        KeyCode.P,
        KeyCode.Q,
        KeyCode.R,
        KeyCode.S,
        KeyCode.T,
        KeyCode.U,
        KeyCode.V,
        KeyCode.W,
        KeyCode.X,
        KeyCode.Y,
        KeyCode.Z
    };

    public bool IsKeyDownAlpha()
    {
        for (var i = 0; i < alphaKeyCodes.Count; i++)
        {
            if (Input.GetKeyDown(alphaKeyCodes[i]))
            {
                Dev_Logger.Debug($"{alphaKeyCodes[i]} pressed");
                return true;
            }
        }

        return false;
    }
}
