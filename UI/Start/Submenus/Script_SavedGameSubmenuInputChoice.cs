using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SavedGameSubmenuInputChoice : Script_UIChoice
{
    public Script_StartOverviewController mainController;
    
    /// <summary>
    /// called from OnClick
    /// </summary>
    public void HandleContinueGameSelect()
    {
        mainController.ContinueGame(Id);
    }

    public void HandleNewGameSelect()
    {
        mainController.NewGame(Id);
    }

    public void HandleDeleteGameSelect()
    {
        mainController.DeleteGame(Id);
    }

    public void HandlePasteGameSelect()
    {
        mainController.CopyGame(Id);
    }

    public void HandleSubmenuCancel()
    {
        Debug.Log($"Firing ExitSubmenu() from choice");
        Script_StartEventsManager.ExitSubmenu();
    }
}
