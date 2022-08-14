using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SavedGameViewController : Script_SlotsViewController
{
    [SerializeField] private Script_SavedGameTitle[] savedGames;
    
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

    public override void Setup()
    {
        foreach (Script_SavedGameTitle savedGame in savedGames)
        {
            savedGame.Setup();
        }

        base.Setup();
    }
}
