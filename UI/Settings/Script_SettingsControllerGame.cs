using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Script_SettingsControllerGame : Script_SettingsController
{
    [SerializeField] private Script_CanvasGroupController quitToMainMenuPrompt;

    // ------------------------------------------------------------
    // Unity Events
    
    public void OpenQuitToMainMenuPrompt()
    {
        quitToMainMenuPrompt.Open();
        EventSystem.current.SetSelectedGameObject(quitToMainMenuPrompt.firstToSelect.gameObject);

        EnterSubmenuSFX();
    }

    public void CancelQuitToMainMenu()
    {
        quitToMainMenuPrompt.Close();
        
        // Open back overview with focus on "Quit to Main Menu" Button
        OpenOverview(2);

        ExitMenuSFX();
    }

    public override void Setup()
    {
        base.Setup();

        quitToMainMenuPrompt.Close();
    }
}
