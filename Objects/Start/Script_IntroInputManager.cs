using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_IntroInputManager : MonoBehaviour
{
    [SerializeField] private Script_IntroController introController;
    [SerializeField] private bool isDisabled;

    public bool IsDisabled
    {
        get => isDisabled;
        set => isDisabled = value;
    }
    
    public virtual void HandleEnterInput()
    {
        if (
            Script_PlayerInputManager.Instance.RewiredInput.GetButtonDown(Const_KeyCodes.RWUISubmit)
            && !isDisabled
        )
        {
            Dev_Logger.Debug($"{name} HandleEnterInput() skipping to Start Screen");
            introController.SkipToStartScreen();
        }
    }
}
