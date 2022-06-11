using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Note: References to this may need to be manually relinked for the Prefab Variant
/// SettingsControllerGame as the Unity Event refs the non-variant.
/// </summary>
public class Script_SettingsControllerGame : Script_SettingsController
{
    [SerializeField] private Script_CanvasGroupController quitToMainMenuPrompt;

    // ------------------------------------------------------------
    // Unity Events
    
    public void OpenQuitToMainMenuPrompt()
    {
        state = States.MainMenu;
        
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

        state = States.Overview;
    }

    public override void Back()
    {
        base.Back();

        switch (state)
        {
            case (States.MainMenu):
                CancelQuitToMainMenu();
                break;
        }
    }

    public override void Setup()
    {
        base.Setup();

        quitToMainMenuPrompt.Close();
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_SettingsControllerGame))]
    public class Script_SettingsControllerGameTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_SettingsControllerGame t = (Script_SettingsControllerGame)target;
            
            if (GUILayout.Button("Open Reset Defaults Submenu"))
            {
                t.OpenResetDefaultsSubmenu();
            }
        }
    }
#endif
}
