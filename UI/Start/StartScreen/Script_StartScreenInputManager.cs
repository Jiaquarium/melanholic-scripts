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
    
    private bool isCTADone;

    void Update()
    {
        if (Input.anyKey)
        {
            startController.Initialize();
        }
    }

    public virtual void HandleEnterInput()
    {
        if (isCTADone)
            return;

        if (Input.GetButtonDown(Const_KeyCodes.Submit))
        {
            mainController.StartOptionsOpen(isInitial: true);
            
            mainController.EnterMenuSFX();

            isCTADone = true;
        }
    }
}
