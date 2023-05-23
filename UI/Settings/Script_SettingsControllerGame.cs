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
    private enum PromptStates
    {
        None = 0,
        MainMenu = 1,
        EndGame = 2
    }
    
    [SerializeField] private Script_CanvasGroupController quitToMainMenuPrompt;
    [SerializeField] private Script_CanvasGroupController endGamePrompt;

    private PromptStates promptState;

    // ------------------------------------------------------------
    // Unity Events
    
    public void OpenQuitToMainMenuPrompt()
    {
        state = States.MainMenu;
        promptState = PromptStates.MainMenu;
        
        quitToMainMenuPrompt.Open();
        EventSystem.current.SetSelectedGameObject(quitToMainMenuPrompt.firstToSelect.gameObject);

        EnterSubmenuSFX();
    }

    public void CancelQuitToMainMenu()
    {
        quitToMainMenuPrompt.Close();
        
        // Open back overview with focus on "Quit to Main Menu" Button
        OpenOverview(3);

        ExitMenuSFX();

        state = States.Overview;
        promptState = PromptStates.None;
    }

    public void OpenEndGamePrompt()
    {
        state = States.MainMenu;
        promptState = PromptStates.EndGame;

        endGamePrompt.Open();
        EventSystem.current.SetSelectedGameObject(endGamePrompt.firstToSelect.gameObject);

        EnterSubmenuSFX();
    }

    public void CancelEndGame()
    {
        endGamePrompt.Close();
        
        // Open back overview with focus on "Quit to Desktop" Button
        OpenOverview(4);

        ExitMenuSFX();

        state = States.Overview;
        promptState = PromptStates.None;
    }

    /// <summary>
    /// Handle Back input listening
    /// </summary>
    public override void Back()
    {
        base.Back();

        switch (state)
        {
            case (States.MainMenu):
                switch (promptState)
                {
                    case (PromptStates.MainMenu):
                        CancelQuitToMainMenu();
                        break;
                    case (PromptStates.EndGame):
                        CancelEndGame();
                        break;
                }
                break;
        }
    }

    // QuitToMainMenuPrompt Yes, go to title with 1 sec fade out
    public void ToTitleScreen()
    {
        Script_TransitionManager.Control.ToTitleScreen();
        PlaySubmitTransitionCancel();
    }

    // EndGamePrompt Yes
    public void EndGame()
    {
        PlaySubmitTransitionCancel();
        Dev_Logger.Debug($"Yes; Settings End Game Prompt");

        // Disable input during black fade in
        if (EventSystem.current != null)
            EventSystem.current.sendNavigationEvents = false;
        
        var bgm = Script_BackgroundMusicManager.Control;
        bgm.FadeOut(null, SceneTransitionFadeInTime, Const_AudioMixerParams.ExposedMusicVolume, isUnscaledTime: true);
        bgm.FadeOutExtra(
            out Coroutine coroutine,
            null,
            SceneTransitionFadeInTime,
            Const_AudioMixerParams.ExposedFXVolume,
            isUnscaledTime: true
        );
        
        Script_Game.Game.SceneTransitionFadeIn(SceneTransitionFadeInTime, () => {
            Dev_Logger.Debug($"Application.Quit()");
            Application.Quit();
        });
    }

    // ------------------------------------------------------------

    public override void Setup()
    {
        base.Setup();

        quitToMainMenuPrompt.Close();
        promptState = PromptStates.None;
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

            if (GUILayout.Button("Open Reset System Defaults Submenu"))
            {
                t.OpenResetSystemDefaultsSubmenu();
            }

            if (GUILayout.Button("Open Reset Sound Defaults Submenu"))
            {
                t.OpenResetSoundDefaultsSubmenu();
            }
        }
    }
#endif
}
