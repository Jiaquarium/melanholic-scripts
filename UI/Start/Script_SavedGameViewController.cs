using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SavedGameViewController : Script_SlotsViewController
{
    public enum MenuStates
    {
        Overview = 0,
        Submenu = 1
    }
    
    public MenuStates menuState;
    [SerializeField] private Script_SavedGameTitle[] savedGames;
    [SerializeField] private Script_SavedGameBackInputManager savedGameBackInputManager;

    /// <summary>
    /// This should track the last frame an ESC shortcut was pressed for the following cases:
    ///     - Exiting file action submenu (i.e. new game, continue game, paste prompt, delete prompt)
    ///     - Exiting file action mode
    /// </summary>
    public int LastExitInputFrameCount { get; set; }
    
    public override void UpdateSlots()
    {
        base.UpdateSlots();
        UpdateSavedGames();

        void UpdateSavedGames()
        {
            savedGames = slotsHolder.GetChildren<Script_SavedGameTitle>();
        }
    }

    public void HoldHighlights(bool isHold)
    {
        foreach (var savedGame in savedGames)
            savedGame.HoldHighlight(isHold);
    }

    public void RenderInitedLangTexts()
    {
        foreach (Script_SavedGameTitle savedGame in savedGames)
        {
            if (savedGame.isRendered)
                savedGame.RenderInitedLangTexts();
        }
    }
    
    public override void Setup()
    {
        foreach (Script_SavedGameTitle savedGame in savedGames)
        {
            savedGame.Setup();
        }

        savedGameBackInputManager.InitialState();

        base.Setup();
    }
}
