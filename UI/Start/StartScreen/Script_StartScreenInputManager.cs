using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles CTA input.
/// </summary>
public class Script_StartScreenInputManager : MonoBehaviour
{
    [SerializeField] private Script_StartOverviewController mainController;
    [SerializeField] private Script_StartScreenController startController;
    
    public bool IsCTADone { get; set; }

    public virtual void HandleEnterInput()
    {
        if (IsCTADone)
            return;

        if (Script_PlayerInputManager.Instance.RewiredInput.GetButtonDown(Const_KeyCodes.RWUISubmit))
        {
            Dev_Logger.Debug("HandleEnterInput(): StartOptionsOpen() UI Submit input detected");
            
            mainController.StartOptionsOpen(isFadeIn: true);
            
            mainController.EnterMenuSFX();

            IsCTADone = true;
        }
    }
}
